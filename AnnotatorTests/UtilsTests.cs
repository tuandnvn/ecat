using Microsoft.VisualStudio.TestTools.UnitTesting;
using Annotator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace Annotator.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        

        [TestMethod()]
        public void getConvexHullTest()
        {


            PointF[] polygons = new PointF[] { new Point(0,0), new Point(0, 2), new Point(2, 2),
                                                new Point(2,0), new Point(1,1) };
            List<PointF> convexHull = Utils.getConvexHull(polygons.ToList());
            List<PointF> trueConvexHull = new PointF[] {new Point(0,0), new Point(0, 2), new Point(2, 2),
                                                new Point(2,0)}.ToList();
            Assert.AreEqual(trueConvexHull.Except(convexHull).Count(), 0);
            Assert.AreEqual(convexHull.Except(trueConvexHull).Count(), 0);

            polygons = new PointF[] { new Point(0,0), new Point(0, 4), new Point(1, 3), new Point(2, 2), new Point(4, 5),
                                                new Point(4,1), new Point(5,0) };
            convexHull = Utils.getConvexHull(polygons.ToList());
            trueConvexHull = new PointF[] {new Point(0,0), new Point(0, 4), new Point(4, 5),
                                                new Point(5,0) }.ToList();

            Assert.AreEqual(trueConvexHull.Except(convexHull).Count(), 0);
            Assert.AreEqual(convexHull.Except(trueConvexHull).Count(), 0);
        }

        public int compare(Point p1, Point p2)
        {
            if (p1.X < p2.X)
                return -1;
            if (p1.X > p2.X)
                return 1;
            if (p1.Y < p2.Y)
                return -1;
            if (p1.Y > p2.Y)
                return 1;
            return 0;
        }

        [TestMethod()]
        public void ScoreTestRectangle()
        {
            Console.WriteLine(new RectangleLocationMark(0, new Rectangle(0, 0, 2, 2)).Score(new Point(1, 1)));
        }

        [TestMethod()]
        public void ScoreTestPolygon()
        {
            Console.WriteLine(new PolygonLocationMark(0, new PointF[] { new Point(0,0), new Point(0, 2), new Point(2, 2),
                                                new Point(2,0) }.ToList()).Score(new Point(1, 1)));
        }

        //[TestMethod()]
        //public void ScoreTestRig()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void IsPointInPolygonTest()
        {
            PointF[] polygons = new PointF[] { new PointF(1,1), new PointF(2, 1), new PointF(3, 0),
                                                new PointF(4,1), new PointF(5,1), new PointF(5,-1), new PointF(1,-1)};
            PointF testPoint1 = new PointF(0, 0);
            PointF testPoint2 = new PointF(2, 1);
            PointF testPoint3 = new PointF(3, 0.5f);
            PointF testPoint4 = new PointF(4, 0.5f);
            Assert.IsFalse(Utils.IsPointInPolygon(polygons, testPoint1));
            Assert.IsTrue(Utils.IsPointInPolygon(polygons, testPoint2));
            Assert.IsFalse(Utils.IsPointInPolygon(polygons, testPoint3));
            Assert.IsTrue(Utils.IsPointInPolygon(polygons, testPoint4));
        }
    }
}