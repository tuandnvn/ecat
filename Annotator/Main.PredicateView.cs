using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    partial class Main
    {
        internal void showPredicates()
        {
            clearPredicates();
            if (selectedObject != null)
            {
                int frame = frameTrackBar.Value;

                // If there is location mark, object currently shown on the screen
                // Predicates will be repopulated
                if (selectedObject.hasMark(frame))
                {
                    HashSet<PredicateMark> holdingPredicates = selectedObject.getHoldingPredicates(frame);

                    foreach (var holdingPredicate in holdingPredicates)
                    {
                        try
                        {
                            predicateView.Rows.Add(holdingPredicate.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
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
