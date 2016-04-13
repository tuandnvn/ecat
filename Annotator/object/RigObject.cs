using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class RigObject : Object
    {
        public RigObject(String id, Color color, int borderSize, string videoFile) : base(id, color, borderSize, videoFile)
        {
        }

        public void setBounding(int frameNumber, RigFigure<Point> boundingRig, double scale, Point translation)
        {
            RigFigure<Point> inverseScaleBoundingRig = scaleBound(boundingRig, 1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
            LocationMark ob = new RigLocationMark<Point>(frameNumber, LocationMark.LocationMarkType.Location, inverseScaleBoundingRig);
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

        protected static RigFigure<Point> scaleBound(RigFigure<Point> original, double scale, Point translation)
        {
            return new RigFigure<Point>(original.rigJoints.ToDictionary(k => k.Key, k => scalePoint(k.Value, scale, translation)),
                original.rigBones.Select(t => new Tuple<Point, Point>(scalePoint(t.Item1, scale, translation), scalePoint(t.Item2, scale, translation))).ToList());
        }

        protected override LocationMark getScaledLocationMark(LocationMark locationMark, double scale, Point translation)
        {
            var casted = (RigLocationMark<Point>)locationMark;
            return new RigLocationMark<Point>(locationMark.frameNo, locationMark.markType, scaleBound(casted.rigFigure, scale, translation));
        }

        protected override void loadObjectAdditionalFromXml()
        {
            string sourceScheme = this.otherProperties["sourceScheme"];
            string source = this.otherProperties["source"];
            int rigIndex = int.Parse(this.otherProperties["rigIndex"]);
            Rigs<Point>.loadDataForRig(source, sourceScheme, rigIndex, this);
        }
    }
}
