using Accord.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class RigLocationMark<T> : DrawableLocationMark
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

        public RigLocationMark<T> getUpperBody()
        {
            return new RigLocationMark<T>(frameNo, Rigs.getUpperBody(rigFigure));
        }

        public override void drawOnGraphics(Graphics g, Pen p)
        {
            if (typeof(T) == typeof(Point))
            {
                // Draw joints
                foreach (var joint in rigFigure.rigJoints.Values)
                {
                    System.Drawing.PointF jointPoint = (System.Drawing.Point)(object)joint;
                    g.DrawEllipse(p, jointPoint.X - 2 * p.Width, jointPoint.Y - 2 * p.Width, p.Width * 4, p.Width * 4);
                }

                // Draw bones
                foreach (var bone in rigFigure.rigBones)
                {
                    System.Drawing.PointF from = (System.Drawing.Point)(object) (rigFigure.rigJoints[bone.Item1]);
                    System.Drawing.PointF to = (System.Drawing.Point)(object)(rigFigure.rigJoints[bone.Item2]);
                    g.DrawLine(p, from, to);
                }
            }

            if (typeof(T) == typeof(System.Drawing.PointF))
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
        }

        public override List<Rectangle> getCornerSelectBoxes(int boxSize)
        {
            if (typeof(T) != typeof(Point) && typeof(T) != typeof(PointF)) return new List<Rectangle>();

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
            return selectBoxes;
        }

        public override DrawableLocationMark getScaledLocationMark(float scale, Point translation)
        {
            if (typeof(T) != typeof(Point) && typeof(T) != typeof(PointF)) return null;

            return new RigLocationMark<PointF>(frameNo, ((RigFigure<PointF>)(object)rigFigure).scaleBound(scale, translation));
        }

        public DrawableLocationMark getDepthViewLocationMark(float scale, Point translation)
        {
            if (typeof(T) == typeof(Point3))
            {
                var mappedJoints = ((Dictionary<string, Point3>)(object)rigFigure.rigJoints).ToDictionary(k => k.Key, k => KinectUtils.projectCameraSpacePointToDepthPixel(k.Value));
                var flattenedMappedJoints = mappedJoints.ToDictionary(k => k.Key, k => new PointF(k.Value.X, k.Value.Y));

                return new RigLocationMark<PointF>(frameNo, new RigFigure<PointF>(flattenedMappedJoints, rigFigure.rigBones)).getScaledLocationMark(scale, translation);
            }
            return null;
        }
    }

}
