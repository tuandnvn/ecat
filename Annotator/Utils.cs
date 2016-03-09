using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class Utils
    {
        public static float Score(Rig rig, int frame, int rigIndex, Point testPoint)
        {
            float score = 0;

            var rigJoint = rig.getRigJoints(frame, rigIndex);
            if (rigJoint.Count != 0)
            {
                var convexHull = getConvexHull(rigJoint);
                return Score(convexHull, testPoint);
            }
            return score;
        }

        public static List<Point> getConvexHull(List<Point> points)
        {
            // Select points that has lowest X, than select point that has lowest Y -> must be one on the convex hull's 
            Point leftMost = points.Where(point => point.X == points.Min(p => p.X)).Where(point => point.Y == points.Min(p => p.Y)).First();

            points.Sort(delegate (Point p1, Point p2)
            {
                // Compare the sin(alpha)
                // Point that make with leftmost and x-axis larger sin(alpha) will be considered first
                if ((p1.Y - leftMost.Y)*(p2.X-leftMost.X) != (p1.X - leftMost.X)*( p2.Y - leftMost.Y) )
                {
                    return  (p1.X - leftMost.X) * (p2.Y - leftMost.Y) - (p1.Y - leftMost.Y) * (p2.X - leftMost.X);
                }

                // If alphas are equals, compare the distance between leftmost and p1, p2
                // We want the point that has better chance to be on convex hull to go first

                return  ((p2.X - leftMost.X)^2 + (p2.Y - leftMost.Y)^2 ) - ((p1.X - leftMost.X) ^ 2 + (p1.Y - leftMost.Y) ^ 2);
            });

            Stack<Point> convexHulls = new Stack<Point>();
            convexHulls.Push(leftMost);

            // Last point in points now should also be leftmost
            foreach (Point p in points.Skip(points.Count - 1))
            {
                if (convexHulls.Count >= 3)
                {
                    FixConvexHull(leftMost, convexHulls, p);

                }
                convexHulls.Push(p);
            };
            return convexHulls.ToList();
        }

        private static void FixConvexHull(Point leftMost, Stack<Point> convexHulls, Point p)
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
                score += (float)(1f / Math.Sqrt((p.X - testPoint.X) ^ 2 + (p.Y - testPoint.Y) ^ 2));
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
                score += (float)(1f / Math.Sqrt((p.X - testPoint.X) ^ 2 + (p.Y - testPoint.Y) ^ 2));
            }
            score /= 4;
            return score;
        }

        public static bool IsPointInPolygonL(List<Point> polygon, Point testPoint)
        {
            return IsPointInPolygon(Array.ConvertAll(polygon.ToArray(), p => (PointF)p), testPoint);
        }

        public static bool IsPointInPolygon(PointF[] polygon, PointF testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if ( (testPoint.Y - polygon[i].Y)  * (polygon[j].X - polygon[i].X) < (testPoint.X - polygon[i].X) * (polygon[j].Y - polygon[i].Y))
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
