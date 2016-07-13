﻿using Accord.Math;
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
        public static Rectangle scaleBound(this Rectangle original, float scale, Point translation)
        {
            return new Rectangle((int)(original.X * scale + translation.X),
                (int)(original.Y * scale + translation.Y),
                (int)(original.Width * scale),
                (int)(original.Height * scale));
        }

        public static Point scalePoint(this Point original, float scale, Point translation)
        {
            return new Point((int)(original.X * scale + translation.X), (int)(original.Y * scale + translation.Y));
        }

        public static System.Drawing.PointF scalePoint(this System.Drawing.PointF original, float scale, System.Drawing.PointF translation)
        {
            return new System.Drawing.PointF((float)(original.X * scale + translation.X), (float)(original.Y * scale + translation.Y));
        }

        public static List<System.Drawing.PointF> scaleBound(this List<System.Drawing.PointF> original, float scale, Point translation)
        {
            return original.Select(p => scalePoint(p, scale, translation)).ToList();
        }

        public static List<Point3> scaleBound(this List<Point3> original, float scale, Point translation)
        {
            return original.Select(p => new Point3(p.X * scale + translation.X, p.Y * scale + translation.Y, p.Z)).ToList();
        }

        public static RigFigure<System.Drawing.PointF> scaleBound(this RigFigure<System.Drawing.PointF> original, float scale, System.Drawing.PointF translation)
        {
            return new RigFigure<System.Drawing.PointF>(original.rigJoints.ToDictionary(k => k.Key, k => scalePoint(k.Value, scale, translation)),
                original.rigBones);
        }

        public static Point getCenter(this Rectangle r)
        {
            return new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
        }


        public static List<Rectangle> getCornerSelectBoxes(this Rectangle boundingBox, int boxSize)
        {
            int lowerX = boundingBox.X;
            int lowerY = boundingBox.Y;
            int higherX = lowerX + boundingBox.Width;
            int higherY = lowerY + boundingBox.Height;

            Rectangle[] selectBoxes = new Rectangle[] { new Rectangle(lowerX - (boxSize - 1)/2, lowerY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(lowerX - (boxSize - 1)/2, higherY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(higherX - (boxSize - 1)/2, lowerY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(higherX - (boxSize - 1)/2, higherY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle((lowerX + higherX)/2 - (boxSize - 1)/2, lowerY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle((lowerX + higherX)/2 - (boxSize - 1)/2, higherY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(lowerX - (boxSize - 1)/2, (lowerY + higherY)/2 - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(higherX - (boxSize - 1)/2, (lowerY + higherY)/2 - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(boundingBox.getCenter().X, boundingBox.getCenter().Y,boxSize,boxSize)
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
