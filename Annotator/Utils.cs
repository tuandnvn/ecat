﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public static class Utils
    {
        public static float Score(RigFigure<Point> rig, Point testPoint)
        {
            float score = 0;

            var rigJoints = rig.rigJoints.Values.ToList();
            if (rigJoints.Count != 0)
            {
                var convexHull = getConvexHull(rigJoints);
                Console.WriteLine("convexHull " + string.Join(",", convexHull));
                return Score(convexHull, testPoint);
            }


            return score;
        }

        public static List<Point> getConvexHull(List<Point> points)
        {
            // Select points that has lowest X, than select point that has lowest Y -> must be one on the convex hull's 
            IEnumerable<Point> smallestXs = points.Where(point => point.X == points.Min(p => p.X));
            Point leftMost = smallestXs.Where(point => point.Y == smallestXs.Min(p => p.Y)).First();

            var tempo = points.FindAll(t => !t.Equals(leftMost));

            tempo.Sort(delegate (Point p1, Point p2)
           {
                // Compare the sin(alpha)
                // Point that make with leftmost and x-axis larger sin(alpha) will be considered first
                if (cotWithX(leftMost, p1) < cotWithX(leftMost, p2))
                   return -1;
               if (cotWithX(leftMost, p1) > cotWithX(leftMost, p2))
                   return 1;

                // If alphas are equals, compare the distance between leftmost and p1, p2
                // We want the point that has better chance to be on convex hull to go first

                return (int)(Math.Pow(p2.X - leftMost.X, 2) + Math.Pow(p2.Y - leftMost.Y, 2) - (Math.Pow(p1.X - leftMost.X, 2) + Math.Pow(p1.Y - leftMost.Y, 2)))
                ;
           });

            Stack<Point> convexHulls = new Stack<Point>();
            convexHulls.Push(leftMost);

            foreach (Point p in tempo)
            {
                FixConvexHull(leftMost, convexHulls, p);

                if (cotWithX(leftMost, convexHulls.First()) != cotWithX(leftMost, p))
                {
                    convexHulls.Push(p);
                }

            };
            return convexHulls.ToList();
        }

        /// <summary>
        /// Cotang of angle between line p, p1 and horizontal line
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        static float cotWithX(Point p, Point p1)
        {
            return (float)(p1.X - p.X) / (p1.Y - p.Y);
        }

        private static void FixConvexHull(Point leftMost, Stack<Point> convexHulls, Point p)
        {
            if (convexHulls.Count >= 3)
            {
                Point lastPoint = convexHulls.Pop();
                Point nearLastPoint = convexHulls.Pop();

                // Check if we should remove the last point from the convex hull
                // We remove it if it is inside (or on the edge) of the triangle ( leftMost, nearLastPoint, p)

                if (IsPointInPolygon(new PointF[] { leftMost, nearLastPoint, p }, lastPoint))
                {
                    convexHulls.Push(nearLastPoint);
                    FixConvexHull(leftMost, convexHulls, p);
                }
                else
                {
                    convexHulls.Push(nearLastPoint);
                    convexHulls.Push(lastPoint);
                }
            }

        }

        public static float Score(List<Point> polygon, Point testPoint)
        {
            if (!IsPointInPolygonL(polygon, testPoint))
            {
                return 0;
            }

            float score = 0;
            // Average of reveresed distance to polygon points
            foreach (Point p in polygon)
            {
                score += (float)(1f / Math.Sqrt( Math.Pow(p.X - testPoint.X, 2) + Math.Pow(p.Y - testPoint.Y, 2) + 1));
            }
            score /= polygon.Count;
            return score;
        }

        public static float Score(Rectangle r, Point testPoint)
        {
            if (!r.Contains(testPoint))
                return 0;
            float score = 0;
            foreach (Point p in new Point[] { new Point(r.Top, r.Left), new Point(r.Top, r.Right), new Point(r.Bottom, r.Left), new Point(r.Bottom, r.Right) })
            {
                score += (float)(1f / Math.Sqrt(Math.Pow(p.X - testPoint.X, 2) + Math.Pow(p.Y - testPoint.Y, 2) + 1));
            }
            score /= 4;
            return score;
        }

        public static bool IsPointInPolygonL(List<Point> polygon, Point testPoint)
        {
            return IsPointInPolygon(Array.ConvertAll(polygon.ToArray(), p => (PointF)p), testPoint);
        }

        /// <summary>
        /// 
        /// Ray casting using horizontal axis
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="testPoint"></param>
        /// <returns></returns>
        public static bool IsPointInPolygon(PointF[] polygon, PointF testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if ((testPoint.Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) / (polygon[j].Y - polygon[i].Y) < (testPoint.X - polygon[i].X))
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}
