using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class RigLocationMark<T> : LocationMark2D
    {
        public RigFigure<T> rigFigure { get; }
        public RigLocationMark(int frameNo, RigFigure<T> rigFigure) : base(frameNo)
        {
            this.rigFigure = rigFigure;
        }

        public override float Score(Point testPoint)
        {
            float score = 0;

            if (typeof(T) != typeof(Point) && typeof(T) != typeof(PointF)) return 0;

            var rigJoints = rigFigure.rigJoints.Values.Cast<PointF>().ToList();
            if (rigJoints.Count != 0)
            {
                var convexHull = Utils.getConvexHull(rigJoints);
                PolygonLocationMark temp = new PolygonLocationMark(frameNo, convexHull);
                return temp.Score(testPoint);
            }

            return score;
        }

        public override void drawOnGraphics(Graphics g, Pen p)
        {
            g.DrawRig(p, rigFigure);
        }

        public override Rectangle[] getCornerSelectBoxes(int boxSize)
        {
            if (typeof(T) != typeof(Point) && typeof(T) != typeof(PointF)) return new Rectangle[0];

            List<string> markedJointNames = new List<string>() { "head", "hand" };
            List<Rectangle> selectBoxes = new List<Rectangle>();
            foreach (String jointName in rigFigure.rigJoints.Keys)
            {
                foreach (string s in markedJointNames)
                    if (jointName.ToLower().Contains(s))
                    {
                        PointF joint = (PointF)(object)rigFigure.rigJoints[jointName];
                        selectBoxes.Add(new Rectangle((int)(joint.X - (boxSize - 1) / 2),
                            (int)(joint.Y - (boxSize - 1) / 2), boxSize, boxSize));
                        break;
                    }
            }
            return selectBoxes.ToArray();
        }

        public override LocationMark2D getScaledLocationMark(double scale, Point translation)
        {
            if (typeof(T) != typeof(Point) && typeof(T) != typeof(PointF)) return null;

            return new RigLocationMark<PointF>(frameNo, ((RigFigure<PointF>)(object)rigFigure).scaleBound(scale, translation));
        }
    }

}
