//using Accord.Math;
using Accord.Math;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    class KinectUtils
    {
        private const string COORDINATE_MAPPING = "coordinateMapping.dat";

        public static void calculateProject( CoordinateMapper coordinateMapper, String outputFilename)
        {
            CameraSpacePoint[] spacePointBasics = new CameraSpacePoint[] {
                new CameraSpacePoint { X = -0.1f, Y = 0.0f, Z = 1.0f },
                new CameraSpacePoint { X = -0.7f, Y = 0.0f, Z = 1.0f },
                new CameraSpacePoint { X = 0.0f, Y = -0.1f, Z = 1.0f },
                new CameraSpacePoint { X = 0.0f, Y = -0.7f, Z = 1.0f },
                new CameraSpacePoint { X = 0.7f, Y = 0.0f, Z = 1.0f },
                new CameraSpacePoint { X = 0.35f, Y = 0.0f, Z = 1.0f },
                new CameraSpacePoint { X = 0.0f, Y = 0.3f, Z = 1.0f },
                new CameraSpacePoint { X = 0.0f, Y = 0.0f, Z = 1.0f },

                //Skeleton_Joint_Locations 0.51399,0.163888,1.20627,0.494376,0.362051,1.19127
                new CameraSpacePoint { X = 0.51399f, Y = 0.163888f, Z = 1.20627f },
                new CameraSpacePoint { X = 0.494376f, Y = 0.362051f, Z = 1.19127f },

                //Skeleton_Joint_Locations_Orig 0.534418,-0.159339,1.38631,0.523629,0.0243074,1.30818 
                new CameraSpacePoint { X = 0.534418f, Y = -0.159339f, Z = 1.38631f },
                new CameraSpacePoint { X = 0.523629f, Y = 0.0243074f, Z = 1.30818f }, };

            DepthSpacePoint[] spaceBasicToDepth = new DepthSpacePoint[spacePointBasics.Count()];

            coordinateMapper.MapCameraPointsToDepthSpace(spacePointBasics, spaceBasicToDepth);

            ColorSpacePoint[] spaceBasicToColor = new ColorSpacePoint[spacePointBasics.Count()];

            coordinateMapper.MapCameraPointsToColorSpace(spacePointBasics, spaceBasicToColor);

            ColorSpacePoint[] spaceBasicToDepthToColor = new ColorSpacePoint[spacePointBasics.Count()];

            coordinateMapper.MapDepthPointsToColorSpace(spaceBasicToDepth, Enumerable.Repeat((ushort)1000, spacePointBasics.Count()).ToArray(), spaceBasicToDepthToColor);

            Console.WriteLine("Camera space points to depth space points");
            foreach (var t in spaceBasicToDepth)
            {
                Console.WriteLine(t.ToSString());
            }


            Console.WriteLine("Camera space points to color space points");
            foreach (var t in spaceBasicToColor)
            {
                Console.WriteLine(t.ToSString());
            }
            Console.WriteLine("Camera space points to depth space points then to color space points");
            foreach (var t in spaceBasicToDepthToColor)
            {
                Console.WriteLine(t.ToSString());
            }

            DepthSpacePoint[] depthBasics = new DepthSpacePoint[]
            {
                new DepthSpacePoint() {  X = 0.0f, Y = 30.0f },
                new DepthSpacePoint() {  X = 0.0f, Y = 380.0f },
                new DepthSpacePoint() {  X = 511.0f, Y = 30.0f },
                new DepthSpacePoint() {  X = 511.0f, Y = 380.0f },
                new DepthSpacePoint() {  X = 262.7343f, Y = 203.6235f }, // Center of origin
            };

            int noOfPoints = depthBasics.Count();
            ColorSpacePoint[] depthBasicToColor = new ColorSpacePoint[noOfPoints];
            CameraSpacePoint[] depthBasicToCamera = new CameraSpacePoint[noOfPoints];

            // Depth used for depth field are the same as z-axis
            // Not the distance from IR sensor to the point
            ushort[] distances = new ushort[] { 1, 10, 100, 1000, 2000, 40000 };
            foreach (var distance in distances)
            {
                coordinateMapper.MapDepthPointsToColorSpace(depthBasics, Enumerable.Repeat(distance, noOfPoints).ToArray(), depthBasicToColor);

                Console.WriteLine("Projection from depth to color ");
                Console.WriteLine("z-axis = " + distance);
                foreach (var point in depthBasicToColor)
                {
                    Console.WriteLine(point.ToSString());
                }

                coordinateMapper.MapDepthPointsToCameraSpace(depthBasics, Enumerable.Repeat(distance, noOfPoints).ToArray(), depthBasicToCamera);

                Console.WriteLine("Projection from depth to camera ");
                Console.WriteLine("z-axis = " + distance);
                foreach (var point in depthBasicToCamera)
                {
                    Console.WriteLine(point.ToSString());
                }

                for (int i = 0; i < noOfPoints; i++)
                {
                    Console.WriteLine(projectDepthPixelToCameraSpacePoint(new Point3(depthBasics[i].X, depthBasics[i].Y, distance)).ToSString());
                }
            }

            // Let's X_ir(P) being P.X in depth image, X_rgb(P) being P.X in RGB

            // The calculation here is just an estimation, doesn't take into account camera calibration
            // Solve the problem on the z-plane of 1 meter
            // Center of depth field has the same X, Y as the zero point of coordinate space
            System.Drawing.PointF centerDepthField_ir = new System.Drawing.PointF(spaceBasicToDepth[4].X, spaceBasicToDepth[4].Y);

            System.Drawing.PointF centerDepthField_rgb = new System.Drawing.PointF(spaceBasicToColor[4].X, spaceBasicToColor[4].Y);

            // It is difficult to get the center point of rgb field directly, so let's assume it is the center of rgb field

            System.Drawing.PointF centerRgbField_rgb = new System.Drawing.PointF(964.5f, 544.5f);

            // Vector from centerRgbFieldInRgb to centerDepthFieldInRgb
            System.Drawing.PointF translation = new System.Drawing.PointF(centerDepthField_rgb.X - centerRgbField_rgb.X,
                 centerDepthField_rgb.Y - centerRgbField_rgb.Y);

            // Ratio of depth unit / rgb unit
            float ratioX = (spaceBasicToDepth[2].X - centerDepthField_ir.X) / (spaceBasicToColor[2].X - centerDepthField_rgb.X);
            float ratioY = (spaceBasicToDepth[3].Y - centerDepthField_ir.Y) / (spaceBasicToColor[3].Y - centerDepthField_rgb.Y);

            ColorSpacePoint[,] shortRange = new ColorSpacePoint[512, 424];
            ColorSpacePoint[,] longRange = new ColorSpacePoint[512, 424];

            for ( int X = 0; X < 511; X ++ )
                for ( int Y = 0; Y < 423; Y ++ )
                {
                    Point3 p1 = new Point3 { X = X, Y = Y, Z = 500 };
                    Point3 p2 = new Point3 { X = X, Y = Y, Z = 8000 };

                    var p1Projected = coordinateMapper.MapDepthPointToColorSpace(new DepthSpacePoint { X = p1.X, Y = p1.Y }, (ushort)p1.Z);
                    var p2Projected = coordinateMapper.MapDepthPointToColorSpace(new DepthSpacePoint { X = p2.X, Y = p2.Y }, (ushort)p2.Z);

                    shortRange[X, Y] = p1Projected;
                    longRange[X, Y] = p2Projected;
                }
            
            // Write these projected points into a file
            if ( !File.Exists(COORDINATE_MAPPING) )
            {
                var coordinateWriter = new DepthCoordinateMappingWriter(COORDINATE_MAPPING);

                coordinateWriter.write(512, 424, 500, 8000, shortRange, longRange);
            }

            // Let's get a random space point with a depth
            Point3[] PointAs = new Point3[] { new Point3 { X = 300f, Y = 300f, Z = 500 },
                                                new Point3 { X = 400f, Y = 400f, Z = 500 },
                                                new Point3 { X = 300f, Y = 300f, Z = 1000 },
                                                new Point3 { X = 400f, Y = 400f, Z = 1000 },
                                                new Point3 { X = 300f, Y = 300f, Z = 2000 },
                                                new Point3 { X = 400f, Y = 400f, Z = 2000 },
                                                new Point3 { X = 100f, Y = 100f, Z = 1000 },
                                                new Point3 { X = 100f, Y = 100f, Z = 2000 },
                                                new Point3 { X = 0f, Y = 100f, Z = 1000 },
                                                new Point3 { X = 0f, Y = 100f, Z = 2000 },
                                                new Point3 { X = 50f, Y = 100f, Z = 1000 },
                                                new Point3 { X = 50f, Y = 100f, Z = 2000 },
                                                new Point3 { X = 50f, Y = 50f, Z = 1000 },
                                                new Point3 { X = 50f, Y = 50f, Z = 2000 },
                                                new Point3 { X = 500f, Y = 50f, Z = 1000 },
                                                new Point3 { X = 500f, Y = 50f, Z = 2000 },
                                                new Point3 { X = 10f, Y = 10f, Z = 1000 },
                                                new Point3 { X = 10f, Y = 10f, Z = 2000 },
                                                new Point3 { X = 500f, Y = 200f, Z = 1000 },
                                                new Point3 { X = 500f, Y = 200f, Z = 2000 },
                                                new Point3 { X = depthBasics[4].X, Y = depthBasics[4].Y, Z = 2000 },
                                                new Point3 { X = depthBasics[4].X, Y = depthBasics[4].Y, Z = 1000 },
                                                new Point3 { X = depthBasics[4].X, Y = depthBasics[4].Y, Z = 4000 },
                                                new Point3 { X = depthBasics[4].X, Y = depthBasics[4].Y, Z = 500 },
                                                new Point3 { X = spaceBasicToDepth[2].X, Y = spaceBasicToDepth[2].Y, Z = 1000 },
                                                new Point3 { X = spaceBasicToDepth[2].X, Y = spaceBasicToDepth[2].Y, Z = 2000 },
            };

            foreach (Point3 PointA in PointAs)
            {
                // Project randomDepthPoint using ray from IR camera on the 1-z plane
                // It still has X_ir = 300, y_ir = 300 

                System.Drawing.PointF projectedA_ir = new System.Drawing.PointF(PointA.X, PointA.Y);

                // Let's find projectedA_rgb
                System.Drawing.PointF projectedA_rgb = new System.Drawing.PointF(translation.X * 1000 / PointA.Z + (projectedA_ir.X - centerDepthField_ir.X) / ratioX + centerRgbField_rgb.X,
                                                                                   translation.Y * 1000 / PointA.Z + (projectedA_ir.Y - centerDepthField_ir.Y) / ratioY + centerRgbField_rgb.Y);

                // Test
                Console.WriteLine("======Test=====");
                Console.WriteLine(PointA.X + ", " + PointA.Y + ", " + PointA.Z);
                Console.WriteLine(projectedPoint(shortRange, longRange, 500, 8000, PointA));
                Console.WriteLine(projectedA_rgb);
                Console.WriteLine(coordinateMapper.MapDepthPointToColorSpace(new DepthSpacePoint { X = PointA.X, Y = PointA.Y }, (ushort)PointA.Z).ToSString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depthPoint">depthPoint.X = pixel in X; depthPoint.Y = pixel in Y; depthPoint.Z in milimeter</param>
        /// <returns></returns>
        public static Point3 projectDepthPixelToCameraSpacePoint( Point3 depthPixel )
        {
            float x = (depthPixel.X - 262.7343f) * ( depthPixel.Z/ 1000 ) / 367.187f;
            float y = (depthPixel.Y - 203.6235f) * (depthPixel.Z / 1000) / -369f;

            return new Point3(x, y, depthPixel.Z / 1000);
        }

        public static System.Drawing.PointF projectedPoint(ColorSpacePoint[,] shortRange , ColorSpacePoint[,] longRange, int short_range, int long_range, Point3 p )
        {
            int x = (int)p.X;
            int y = (int)p.Y;

            if (x < 0 || x >= 512) return new System.Drawing.PointF();

            if (y < 0 || y >= 424) return new System.Drawing.PointF();

            var shortP = shortRange[x, y];
            var longP = longRange[x, y];

            float shortP_X = shortP.X;
            float longP_X = longP.X;

            var b = ( longP_X * long_range - shortP_X * short_range) / ( long_range - short_range );
            var a = (shortP_X - b) * short_range ;

            var p_x = a / p.Z + b;

            float shortP_Y = shortP.Y;
            float longP_Y = longP.Y;

            b = (longP_Y * long_range - shortP_Y * short_range) / (long_range - short_range);
            a = (shortP_Y - b) * short_range;

            var p_y = a / p.Z + b;

            return new System.Drawing.PointF(p_x, p_y);
        }

        public static ushort[,] MapDepthPointsToColorSpace(Func<DepthSpacePoint, ushort, ColorSpacePoint> mappingFunction, ushort[] depthValues, int depthWidth, int depthHeight, int colorWidth, int colorHeight)
        {
            ushort[,] colorDepthValues = new ushort[colorHeight, colorWidth];

            for (int i = 0; i < depthHeight; i++)
                for (int j = 0; j < depthWidth; j++)
                {
                    ColorSpacePoint csp = mappingFunction(new DepthSpacePoint { X = i, Y = j }, depthValues[i * depthWidth + j]);
                    int gridX = (int)csp.X;
                    int gridY = (int)csp.Y;
                    if (gridX >= 0 && gridX < colorHeight && gridY >= 0 && gridY < colorHeight)
                    {
                        if (depthValues[i * depthWidth + j] < colorDepthValues[gridX, gridY])
                            colorDepthValues[gridX, gridY] = depthValues[i * depthWidth + j];
                    }
                }

            // Interpolating
            // I actually should leave the color image to have holes. 


            return colorDepthValues;
        }
    }
}
