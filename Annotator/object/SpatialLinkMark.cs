using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class SpatialLinkMark : ObjectMark
    {
        public enum SpatialLinkType
        {
            ON,
            IN,
            ATTACH_TO,
            NEXT_TO
        };

        // A set of link to other objects at a certain frame
        // Each link is of < objectID , qualified, spatialLink >
        public SortedSet<Tuple<string, bool, SpatialLinkType>> spatialLinks { get; } // By default, there is no spatial configuration attached to an object location

        public SpatialLinkMark(int frameNo) : base(frameNo)
        {
            spatialLinks = new SortedSet<Tuple<string, bool, SpatialLinkType>>();
        }

        public void addLinkToObject(string objectId, bool qualified, SpatialLinkType linkType)
        {
            spatialLinks.Add(new Tuple<string, bool, SpatialLinkType>(objectId, qualified, linkType));
        }

        override public String ToString()
        {
            return String.Join(",", spatialLinks.Select(u => getLiteralForm(u)));
        }

        private static String getLiteralForm(Tuple<string, bool, SpatialLinkType> t)
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
