using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class LinkMark : ObjectMark
    {
        // A set of link to other objects at a certain frame
        // Each link is of < objectID , qualified, spatialLink >
        public SortedSet<Tuple<string, bool, string>> spatialLinks { get; } // By default, there is no spatial configuration attached to an object location

        public LinkMark(int frameNo) : base(frameNo)
        {
            spatialLinks = new SortedSet<Tuple<string, bool, string>>();
        }

        public void addLinkToObject(string objectId, bool qualified, string linkType)
        {
            spatialLinks.Add(new Tuple<string, bool, string>(objectId, qualified, linkType));
        }

        override public String ToString()
        {
            return String.Join(",", spatialLinks.Select(u => getLiteralForm(u)));
        }

        private static String getLiteralForm(Tuple<string, bool, string> t)
        {
            String q = t.Item3 + "( " + t.Item1 + " )";
            if (!t.Item2)
            {
                q = "NOT( " + q + " )";
            }
            return q;
        }
    }
}
