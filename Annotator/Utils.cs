using Accord.Math;
using System;
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
        public static List<PointF> getConvexHull(List<PointF> points)
        {
            // Select points that has lowest X, than select point that has lowest Y -> must be one on the convex hull's 
            IEnumerable<PointF> smallestXs = points.Where(point => point.X == points.Min(p => p.X));
            PointF leftMost = smallestXs.Where(point => point.Y == smallestXs.Min(p => p.Y)).First();

            var tempo = points.FindAll(t => !t.Equals(leftMost));

            tempo.Sort(delegate (PointF p1, PointF p2)
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

            Stack<PointF> convexHulls = new Stack<PointF>();
            convexHulls.Push(leftMost);

            foreach (PointF p in tempo)
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
        static float cotWithX(PointF p, PointF p1)
        {
            return (float)(p1.X - p.X) / (p1.Y - p.Y);
        }

        private static void FixConvexHull(PointF leftMost, Stack<PointF> convexHulls, PointF p)
        {
            if (convexHulls.Count >= 3)
            {
                PointF lastPoint = convexHulls.Pop();
                PointF nearLastPoint = convexHulls.Pop();

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

        public static bool IsPointInPolygonL(List<PointF> polygon, Point testPoint)
        {
            return IsPointInPolygon(polygon.ToArray(), testPoint);
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

        /// <summary>
        /// A method that try to fit a plane for planarPoints, than output the 
        /// corresponding 3d points for targetPoints
        /// </summary>
        /// <param name="planarPoints"> A list of 3-d points that need to be fitted on a plane </param>
        /// <param name="targetPoints"> A list of 3-d points to get the camera space points, target Points are in color space </param>
        /// <returns></returns>
        public static List<Point3> solvePlanar ( List<Point3> planarPoints, List<PointF> targetPoints)
        {
            return null;
        }
    }
}
