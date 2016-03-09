using Microsoft.VisualStudio.TestTools.UnitTesting;
using Annotator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Annotator.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        public void ScoreTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void getConvexHullTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ScoreTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ScoreTest2()
        {
            Assert.Fail();
        }

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