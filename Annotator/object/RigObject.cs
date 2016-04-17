using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class RigObject : Object
    {
        public RigObject(Session currentSession, String id, Color color, int borderSize, string videoFile) : base(currentSession, id, color, borderSize, videoFile)
        {
            _borderType = BorderType.Others;
        }

        public void setBounding(int frameNumber, RigFigure<PointF> boundingRig, double scale, Point translation)
        {
            RigFigure<PointF> inverseScaleBoundingRig = boundingRig.scaleBound( 1 / scale, new PointF((float)(-translation.X / scale), (float)(-translation.Y / scale)));
            var ob = new RigLocationMark<PointF>(frameNumber, inverseScaleBoundingRig);
            if (this._borderType == null) // First time appear
            {
                this._borderType = BorderType.Others;
            }
            else
            {
                if (this._borderType != BorderType.Others)
                    throw new Exception("Border type not match");
            }
            objectMarks[frameNumber] = ob;
        }

        protected override void writeLocationMark(XmlWriter xmlWriter)
        {
            // No need to write location mark for rig object
        }

        protected override void loadObjectAdditionalFromXml(XmlNode objectNode)
        {
            Console.WriteLine("loadObjectAdditionalFromXml for RigObject");
            string sourceScheme = this.otherProperties["sourceScheme"];
            string source = this.otherProperties["source"];
            int rigIndex = int.Parse(this.otherProperties["rigIndex"]);
            Rigs.loadDataForRig(source, sourceScheme, rigIndex, this);
        }
    }
}
