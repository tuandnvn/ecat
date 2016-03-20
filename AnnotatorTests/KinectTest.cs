using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Kinect;
using System.Diagnostics;

namespace AnnotatorTests
{
    [TestClass]
    public class KinectTest
    {
        private KinectSensor kinectSensor = null;
        private CoordinateMapper coordinateMapper = null;
        [TestInitialize]
        public void initiateKinectViewers()
        {
            this.kinectSensor = KinectSensor.GetDefault();
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;
        }

        [TestMethod]
        public void TestSkeletonRecordedByCwcApparatus()
        {
            CameraSpacePoint[] csps = new CameraSpacePoint[6];
            // Skeleton_Joint_Locations
            //-0.426651,-0.11075,1.68441,-0.404479,0.103393,1.64627
            csps[0] = new CameraSpacePoint { X = -0.426651f, Y = -0.11075f, Z = 1.68441f };
            csps[1] = new CameraSpacePoint { X = -0.404479f, Y = 0.103393f, Z = 1.64627f };
            // Skeleton_Joint_Locations_Orig
            //-0.456807,-0.213856,1.96254,-0.428633,-0.0252217,1.85457
            csps[2] = new CameraSpacePoint { X = -0.456807f, Y = -0.213856f, Z = 1.96254f };
            csps[3] = new CameraSpacePoint { X = -0.428633f, Y = -0.0252217f, Z = 1.85457f };

            // Capture using aruco 
            // 0.00749806 -0.236892 1.49116 0.00622179 0.0498621 1.52369
            csps[4] = new CameraSpacePoint { X = -0.456807f, Y = -0.213856f, Z = 1.96254f };
            csps[5] = new CameraSpacePoint { X = -0.428633f, Y = -0.0252217f, Z = 1.85457f };

            foreach (CameraSpacePoint csp in csps)
            {
                ColorSpacePoint c = this.coordinateMapper.MapCameraPointToColorSpace(csp);
                Trace.WriteLine(c.X + " " + c.Y);
            }
        }
    }
}
