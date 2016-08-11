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

                // If there is location mark, object currently shown on the screen
                // Predicates will be repopulated
                if (selectedObject.hasMark(frame))
                {
                    HashSet<PredicateMark> holdingPredicates = new HashSet<PredicateMark>();

                    foreach (int frameNo in selectedObject.linkMarks.Keys)
                    {
                        if (frameNo <= frame)
                            foreach (var predicateMark in selectedObject.linkMarks[frameNo].predicateMarks)
                            {
                                // Only add predicateMark if it is POSITIVE
                                // Otherwise remove its negation
                                if (predicateMark.qualified)
                                {
                                    holdingPredicates.RemoveWhere(m => Options.getOption().predicateConstraints.Any(constraint => constraint.isConflict(m, predicateMark)));

                                    //Except from IDENTITY relationship
                                    // Other relationship only hold when all objects in relationship appears
                                    // We still need to consider predicate mark to remove nullified predicates before it
                                    // However we don't add it if some object disappears
                                    if (predicateMark.predicate.predicate != "IDENTITY")
                                    {
                                        bool allExist = true;
                                        foreach (var o in predicateMark.objects)
                                        {
                                            // This object o still appear in the move
                                            if (!o.hasMark(frame))
                                            {
                                                allExist = false;
                                                break;
                                            }
                                        }

                                        if (allExist)
                                        {
                                            holdingPredicates.Add(predicateMark);
                                        }
                                    }
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
