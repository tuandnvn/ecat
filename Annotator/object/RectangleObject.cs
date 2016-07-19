using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class RectangleObject : Object
    {
        public RectangleObject(Session session, String id, Color color, int borderSize, string videoFile) : base(session, id, color, borderSize, videoFile)
        {
        }

        public void setBounding(int frameNumber, Rectangle boundingBox, float scale, Point translation)
        {
            Rectangle inverseScaleBoundingBox = boundingBox.scaleBound(1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
            var ob = new RectangleLocationMark(frameNumber, inverseScaleBoundingBox);
            objectMarks[frameNumber] = ob;
        }

        protected override void loadObjectAdditionalFromXml(XmlNode objectNode)
        {
            XmlNode markersNode = objectNode.SelectSingleNode(MARKERS);

            if (markersNode == null) return;
            foreach (XmlNode markerNode in markersNode.SelectNodes(MARKER))
            {
                int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                String markType = markerNode.Attributes[TYPE].Value;

                switch (markType.ToUpper())
                {
                    case "LOCATION":
                        var lm = new RectangleLocationMark(frame, new Rectangle());
                        lm.readFromXml(markerNode);
                        this.setBounding(frame, lm);
                        break;
                    case "DELETE":
                        this.delete(frame);
                        break;
                }
            }
        }
    }
}
