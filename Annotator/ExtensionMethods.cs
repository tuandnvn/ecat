using Accord.Math;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public static class Extensions
    {
        public static RectangleF scaleBound(this RectangleF original, float scale, System.Drawing.PointF translation)
        {
            return new RectangleF(original.X * scale + translation.X,
                original.Y * scale + translation.Y,
                original.Width * scale,
                original.Height * scale);
        }

        public static System.Drawing.PointF scalePoint(this Point original, float scale, System.Drawing.PointF translation)
        {
            return new System.Drawing.PointF(original.X * scale + translation.X, original.Y * scale + translation.Y);
        }

        public static System.Drawing.PointF scalePoint(this System.Drawing.PointF original, float scale, System.Drawing.PointF translation)
        {
            return new System.Drawing.PointF((float)(original.X * scale + translation.X), (float)(original.Y * scale + translation.Y));
        }

        public static Point3 scalePoint(this Point3 original, float scale, Point3 translation)
        {
            return new Point3((float)(original.X * scale + translation.X), 
                (float)(original.Y * scale + translation.Y),
                (float)(original.Z * scale + translation.Z));
        }

        public static List<System.Drawing.PointF> scaleBound(this List<System.Drawing.PointF> original, float scale, System.Drawing.PointF translation)
        {
            return original.Select(p => scalePoint(p, scale, translation)).ToList();
        }

        public static List<Point3> scaleBound(this List<Point3> original, float scale, Point3 translation)
        {
            return original.Select(p => new Point3(p.X * scale + translation.X, p.Y * scale + translation.Y, p.Z * scale + translation.Z)).ToList();
        }

        public static RigFigure<System.Drawing.PointF> scaleBound(this RigFigure<System.Drawing.PointF> original, float scale, System.Drawing.PointF translation)
        {
            return new RigFigure<System.Drawing.PointF>(original.rigJoints.ToDictionary(k => k.Key, k => scalePoint(k.Value, scale, translation)),
                original.rigBones);
        }

        public static System.Drawing.PointF getCenter(this Rectangle r)
        {
            return new System.Drawing.PointF(r.X + r.Width * 1.0f / 2, r.Y + r.Height * 1.0f / 2);
        }


        public static System.Drawing.PointF getCenter(this RectangleF r)
        {
            return new System.Drawing.PointF(r.X + r.Width / 2, r.Y + r.Height / 2);
        }

        public static void DrawRectangle(this Graphics graphics, Pen pen, RectangleF rectangle)
        {
            graphics.DrawRectangles(pen, new RectangleF[] { rectangle });
        }

        public static List<RectangleF> getCornerSelectBoxes(this RectangleF boundingBox, int boxSize)
        {
            float lowerX = boundingBox.X;
            float lowerY = boundingBox.Y;
            float higherX = lowerX + boundingBox.Width;
            float higherY = lowerY + boundingBox.Height;

            RectangleF[] selectBoxes = new RectangleF[] { new RectangleF(lowerX - boxSize / 2.0f, lowerY - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF(lowerX - boxSize/2.0f, higherY - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF(higherX - boxSize/2.0f, lowerY - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF(higherX - boxSize/2.0f, higherY - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF((lowerX + higherX)/2.0f - boxSize/2.0f, lowerY - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF((lowerX + higherX)/2.0f - boxSize/2.0f, higherY - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF(lowerX - boxSize/2.0f, (lowerY + higherY)/2.0f - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF(higherX - boxSize/2.0f, (lowerY + higherY)/2.0f - boxSize/2.0f,boxSize,boxSize),
                                                        new RectangleF(boundingBox.getCenter().X - boxSize/2.0f, boundingBox.getCenter().Y - boxSize/2.0f,
                                                        boxSize,boxSize)
            };
            return selectBoxes.ToList();
        }

        public static string ToSString(this ColorSpacePoint scp)
        {
            return "( " + scp.X + ", " + scp.Y + " )";
        }

        public static string ToSString(this DepthSpacePoint dsp)
        {
            return "( " + dsp.X + ", " + dsp.Y + " )";
        }

        public static string ToSString(this CameraSpacePoint csp)
        {
            return "( " + csp.X + ", " + csp.Y + ", " + csp.Z + " )";
        }

        public static string ToSString(this Point3 p)
        {
            return "( " + p.X + ", " + p.Y + ", " + p.Z + " )";
        }

        public static Point3 Add(this Point3 x, Point3 y)
        {
            return new Point3(x.X + y.X, x.Y + y.Y, x.Z + y.Z);
        }

        public static Point3 Multiple(this Point3 x, float y)
        {
            return new Point3(x.X * y, x.Y * y, x.Z * y);
        }

        public static List<T> minVal<T>(this List<T> list, Func<T, float> mapper)
        {
            if (list.Count == 0) return new List<T>();

            T min = list.First();
            for (int i = 1; i < list.Count; i++)
            {
                if (mapper(list[i]) < mapper(min))
                {
                    min = list[i];
                }
            }

            var listMin = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                if (mapper(list[i]) == mapper(min))
                    listMin.Add(list[i]);
            }

            return listMin;
        }
    }
}
