using Accord.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class RigLocationMark2D : LocationMark2D
    {
        public RigFigure<PointF> rigFigure { get; }
        public RigLocationMark2D(int frameNo, RigFigure<PointF> rigFigure) : base(frameNo)
        {
            this.rigFigure = rigFigure;
        }

        public override float Score(Point testPoint)
        {
            float score = 0;

            var rigJoints = rigFigure.rigJoints.Values.Cast<PointF>().ToList();
            if (rigJoints.Count != 0)
            {
                var convexHull = Utils.getConvexHull(rigJoints);
                PolygonLocationMark2D temp = new PolygonLocationMark2D(frameNo, convexHull);
                return temp.Score(testPoint);
            }

            return score;
        }

        public RigLocationMark2D getUpperBody()
        {
            return new RigLocationMark2D(frameNo, Rigs.getUpperBody(rigFigure));
        }

        public override void drawOnGraphics(Graphics g, Pen p)
        {
            // Draw joints
            foreach (var joint in rigFigure.rigJoints.Values)
            {
                System.Drawing.PointF jointPoint = (System.Drawing.PointF)(object)joint;
                g.DrawEllipse(p, jointPoint.X - 2 * p.Width, jointPoint.Y - 2 * p.Width, p.Width * 4, p.Width * 4);
            }

            // Draw bones
            foreach (var bone in rigFigure.rigBones)
            {
                System.Drawing.PointF from = (System.Drawing.PointF)(object)(rigFigure.rigJoints[bone.Item1]);
                System.Drawing.PointF to = (System.Drawing.PointF)(object)(rigFigure.rigJoints[bone.Item2]);
                g.DrawLine(p, from, to);
            }
        }

        public override List<RectangleF> getCornerSelectBoxes(int boxSize)
        {
            List<string> markedJointNames = new List<string>() { "head", "hand" };
            List<RectangleF> selectBoxes = new List<RectangleF>();
            foreach (String jointName in rigFigure.rigJoints.Keys)
            {
                foreach (string s in markedJointNames)
                    if (jointName.ToLower().Contains(s))
                    {
                        PointF joint = (PointF)(object)rigFigure.rigJoints[jointName];
                        selectBoxes.Add(new RectangleF(joint.X - (boxSize - 1) / 2,
                            joint.Y - (boxSize - 1) / 2, boxSize, boxSize));
                        break;
                    }
            }
            return selectBoxes;
        }

        public override LocationMark2D getScaledLocationMark(float scale, PointF translation)
        {
            return new RigLocationMark2D(frameNo, ((RigFigure<PointF>)(object)rigFigure).scaleBound(scale, translation));
        }

        
        public override LocationMark2D addLocationMark(int resultFrameNo, LocationMark2D added)
        {
            if (added is RigLocationMark2D)
            {
                var addedRigFigure = (added as RigLocationMark2D).rigFigure;
                return new RigLocationMark2D(resultFrameNo,
                    new RigFigure<PointF>((rigFigure as RigFigure<PointF>).rigJoints.ToDictionary(
                        k => k.Key, k => k.Value.scalePoint(1, addedRigFigure.rigJoints[k.Key]
                 )), rigFigure.rigBones));
            }
            else
            {
                throw new ArgumentException(" Adding location mark needs to be of the same type !");
            }
        }
    }
}
