using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    partial class Main
    {
        private void showPredicates()
        {
            clearPredicates();
            if (selectedObject != null)
            {
                int frame = frameTrackBar.Value;
                var locationMark = selectedObject.getScaledLocationMark(frame, 1, new System.Drawing.PointF());

                // If there is location mark, object currently shown on the screen
                // Predicates will be repopulated
                if (locationMark != null)
                {
                    HashSet<PredicateMark> holdingPredicates = new HashSet<PredicateMark>();

                    foreach (int frameNo in selectedObject.linkMarks.Keys)
                    {
                        if (frameNo <= frame)
                            foreach (var predicateMark in selectedObject.linkMarks[frameNo].predicateMarks)
                            {
                                if (predicateMark.qualified)
                                {
                                    holdingPredicates.RemoveWhere(m => Options.getOption().predicateConstraints.Any(constraint => constraint.isConflict(m, predicateMark)));
                                    holdingPredicates.Add(predicateMark);
                                }
                                else
                                {
                                    holdingPredicates.RemoveWhere(m => m.isNegateOf(predicateMark));
                                }
                            }
                    }

                    foreach (var holdingPredicate in holdingPredicates)
                    {
                        predicateView.Rows.Add(holdingPredicate.ToString());
                    }
                }
            }
        }

        private void clearPredicates()
        {
            predicateView.Rows.Clear();
        }
    }
}
