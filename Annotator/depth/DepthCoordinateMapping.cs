using Accord.Math;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    class DepthCoordinateMappingWriter
    {
        public String filename { get; }

        public DepthCoordinateMappingWriter(String filename)
        {
            this.filename = filename;
        }

        public void write(Int32 width, Int32 height, Int32 shortRange, Int32 longRange, ColorSpacePoint[,] shortRangeMap, ColorSpacePoint[,] longRangeMap)
        {
            if (File.Exists(filename))
            {
                var result = System.Windows.Forms.MessageBox.Show("File " + filename + " exists. Do you want to replace it?", "File exists", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            using (BinaryWriter coordinateMappingWriter = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                coordinateMappingWriter.Write(width);
                coordinateMappingWriter.Write(height);
                coordinateMappingWriter.Write(shortRange);
                coordinateMappingWriter.Write(longRange);

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        coordinateMappingWriter.Write(shortRangeMap[i, j].X);
                        coordinateMappingWriter.Write(shortRangeMap[i, j].Y);
                    }

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        coordinateMappingWriter.Write(longRangeMap[i, j].X);
                        coordinateMappingWriter.Write(longRangeMap[i, j].Y);
                    }
            }
        }
    }

    public class DepthCoordinateMappingReader
    {
        public String filename { get; }
        public int width { private set; get; }
        public int height { private set; get; }
        public int shortRange { private set; get; }
        public int longRange { private set; get; }
        public System.Drawing.PointF[,] shortRangeMap;
        public System.Drawing.PointF[,] longRangeMap;
        static Point3 initiate = new Point3(-1, -1, -1);
        private bool DEBUG = true;

        public DepthCoordinateMappingReader(String filename)
        {
            this.filename = filename;

            read();
        }

        public void read()
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("File " + filename + " not found !");
            }

            using (var coordinateMappingReader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                width = coordinateMappingReader.ReadInt32();
                height = coordinateMappingReader.ReadInt32();
                shortRange = coordinateMappingReader.ReadInt32();
                longRange = coordinateMappingReader.ReadInt32();

                shortRangeMap = new System.Drawing.PointF[width, height];
                longRangeMap = new System.Drawing.PointF[width, height];

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        var x = coordinateMappingReader.ReadSingle();
                        var y = coordinateMappingReader.ReadSingle();
                        shortRangeMap[i, j] = new System.Drawing.PointF { X = x, Y = y };
                    }

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        var x = coordinateMappingReader.ReadSingle();
                        var y = coordinateMappingReader.ReadSingle();
                        longRangeMap[i, j] = new System.Drawing.PointF { X = x, Y = y };
                    }
            }

            Console.WriteLine("Finish reading coordinate mapping file " + filename);
        }

        /// <summary>
        /// Map from a depth point to a color point
        /// </summary>
        /// <param name="p"> 
        ///     p.x = depth pixel on x axis, 
        ///     p.y = depth pixel on y axis,
        ///     p.z = depth in meter
        /// </param>
        /// <returns>Color space point in pixels</returns>
        public System.Drawing.PointF projectedPoint(Point3 p)
        {
            int x = (int)p.X;
            int y = (int)p.Y;

            if (x < 0 || x >= 512) return new System.Drawing.PointF();

            if (y < 0 || y >= 424) return new System.Drawing.PointF();

            var shortP = shortRangeMap[x, y];
            var longP = longRangeMap[x, y];

            float shortP_X = shortP.X;
            float longP_X = longP.X;

            var b = (longP_X * longRange - shortP_X * shortRange) / (longRange - shortRange);
            var a = (shortP_X - b) * shortRange;

            var p_x = a / p.Z + b;

            float shortP_Y = shortP.Y;
            float longP_Y = longP.Y;

            b = (longP_Y * longRange - shortP_Y * shortRange) / (longRange - shortRange);
            a = (shortP_Y - b) * shortRange;

            var p_y = a / p.Z + b;

            return new System.Drawing.PointF(p_x, p_y);
        }

        public Point3[,] projectDepthImageToColor(ushort[] depthImage, int depthWidth, int depthHeight, int colorWidth, int colorHeight)
        {
            Point3[,] result = new Point3[colorWidth, colorHeight];

            for (int x = 0; x < colorWidth; x++)
                for (int y = 0; y < colorHeight; y++)
                {
                    result[x, y] = initiate;
                }

            for (int x = 0; x < depthWidth; x++)
                for (int y = 0; y < depthHeight; y++)
                {
                    ushort depth = depthImage[y * depthWidth + x];

                    // Project this depth pixel coordinate to camera space point
                    Point3 depthPixel = new Point3 { X = x, Y = y, Z = depth };

                    Point3 cameraSpacePoint = KinectUtils.projectDepthPixelToCameraSpacePoint(depthPixel);

                    System.Drawing.PointF colorPixelPoint = projectedPoint(depthPixel);

                    int color_X = (int)colorPixelPoint.X;
                    int color_Y = (int)colorPixelPoint.Y;

                    if (color_X >= 0 && color_X < colorWidth && color_Y >= 0 && color_Y < colorHeight)
                    {
                        if (!result[color_X, color_Y].Equals(initiate))
                        {
                            // Replace the current one with the one closer to the camera
                            if (result[color_X, color_Y].Z > cameraSpacePoint.Z)
                            {
                                result[color_X, color_Y] = cameraSpacePoint;
                            }
                        }
                        else
                        {
                            result[color_X, color_Y] = cameraSpacePoint;
                        }
                    }
                }

            return result;
        }

        public void projectDepthImageToCameraSpacePoint(ushort[] depthImage, int depthWidth, int depthHeight, int colorWidth, int colorHeight, CameraSpacePoint[] result)
        {
            // For debug purpose
            Bitmap bm = null;
            byte[] depthValuesToByte = null;
            BitmapData bmapdata = null;
            IntPtr? ptr = null;

            if (DEBUG)
            {
                bm = new Bitmap(colorWidth, colorHeight, PixelFormat.Format32bppRgb);

                bmapdata = bm.LockBits(
                                     new Rectangle(0, 0, colorWidth, colorHeight),
                                     ImageLockMode.WriteOnly,
                                     bm.PixelFormat);

                ptr = bmapdata.Scan0;
                depthValuesToByte = new byte[colorWidth * colorHeight * 4];

                for (int i = 0; i < colorWidth * colorHeight; i++)
                {
                    depthValuesToByte[4 * i] = depthValuesToByte[4 * i + 1] = depthValuesToByte[4 * i + 2] = 0;
                }
            }

            Point3[,] tempo = new Point3[colorWidth, colorHeight];

            for (int x = 0; x < colorWidth; x++)
                for (int y = 0; y < colorHeight; y++)
                {
                    tempo[x, y] = initiate;
                }

            int xsmallest = colorWidth, ysmallest = colorHeight, xlargest = -1, ylargest = -1;
            for (int x = 0; x < depthWidth; x++)
            {
                for (int y = 0; y < depthHeight; y++)
                {
                    ushort depth = depthImage[y * depthWidth + x];

                    // Project this depth pixel coordinate to camera space point
                    Point3 depthPixel = new Point3 { X = x, Y = y, Z = depth };

                    Point3 cameraSpacePoint = KinectUtils.projectDepthPixelToCameraSpacePoint(depthPixel);

                    System.Drawing.PointF colorPixelPoint = projectedPoint(depthPixel);

                    int color_X = (int)colorPixelPoint.X;
                    int color_Y = (int)colorPixelPoint.Y;

                    if (color_X >= 0 && color_X < colorWidth && color_Y >= 0 && color_Y < colorHeight)
                    {
                        // Replace smallest and largest X and Y
                        if (color_X < xsmallest)
                        {
                            //Console.WriteLine("XSmallest; depth = " + depth + " x = " + x + " y = " + y + " cameraSpacePoint = " + cameraSpacePoint.ToSString() + " colorPixelPoint = " + colorPixelPoint);
                            xsmallest = color_X;
                        }

                        if (color_X > xlargest)
                        {
                            //Console.WriteLine("XLargest; depth = " + depth + " x = " + x + " y = " + y + " cameraSpacePoint = " + cameraSpacePoint.ToSString() + " colorPixelPoint = " + colorPixelPoint);
                            xlargest = color_X;
                        }

                        if (color_Y < ysmallest)
                        {
                            //Console.WriteLine("YSmallest; depth = " + depth + " x = " + x + " y = " + y + " cameraSpacePoint = " + cameraSpacePoint.ToSString() + " colorPixelPoint = " + colorPixelPoint);
                            ysmallest = color_Y;
                        }

                        if (color_Y > ylargest)
                        {
                            //Console.WriteLine("YLargest; depth = " + depth + " x = " + x + " y = " + y + " cameraSpacePoint = " + cameraSpacePoint.ToSString() + " colorPixelPoint = " + colorPixelPoint);
                            ylargest = color_Y;
                        }

                        if (!tempo[color_X, color_Y].Equals(initiate))
                        {
                            // Replace the current one with the one closer to the camera
                            if (tempo[color_X, color_Y].Z > cameraSpacePoint.Z)
                            {
                                tempo[color_X, color_Y] = cameraSpacePoint;
                            }
                        }
                        else
                        {
                            tempo[color_X, color_Y] = cameraSpacePoint;
                        }
                    }
                }
            }

            for (int x = 0; x < colorWidth; x++)
                for (int y = 0; y < colorHeight; y++)
                {
                    result[x * colorHeight + y] = new CameraSpacePoint { X = -1, Y = -1, Z = -1 };
                }

            //Console.WriteLine("xsmallest " + xsmallest);
            //Console.WriteLine("xlargest " + xlargest);
            //Console.WriteLine("ysmallest " + ysmallest);
            //Console.WriteLine("ylargest " + ylargest);

            for (int x = xsmallest; x < xlargest; x++)
            {
                for (int y = ysmallest; y < ylargest; y++)
                {
                    var index = y * colorWidth + x;
                    // If tempo[x,y] has value
                    if (!tempo[x, y].Equals(initiate))
                    {
                        result[index] = new CameraSpacePoint { X = tempo[x, y].X, Y = tempo[x, y].Y, Z = tempo[x, y].Z };
                        //writetext.Write("1");
                        if (DEBUG)
                            depthValuesToByte[4 * index] = depthValuesToByte[4 * index + 1] = depthValuesToByte[4 * index + 2] = 255;
                    }
                    else
                    {
                        //writetext.Write("0");
                        if (DEBUG)
                            depthValuesToByte[4 * index] = depthValuesToByte[4 * index + 1] = depthValuesToByte[4 * index + 2] = 0;
                    }
                }
                //writetext.WriteLine();
            }

            if (DEBUG)
            {
                Marshal.Copy(depthValuesToByte, 0, ptr.Value, colorWidth * colorHeight * 4);
                bm.UnlockBits(bmapdata);

                bm.Save("0.png");

                bmapdata = bm.LockBits(
                                    new Rectangle(0, 0, colorWidth, colorHeight),
                                    ImageLockMode.WriteOnly,
                                    bm.PixelFormat);
            }

            // Linear interpolate
            for (int x = xsmallest; x < xlargest; x++)
                for (int y = ysmallest; y < ylargest; y++)
                {
                    var index = y * colorWidth + x;

                    // If tempo[x,y] has value
                    if (!tempo[x, y].Equals(initiate))
                    {
                        result[index] = new CameraSpacePoint { X = tempo[x, y].X, Y = tempo[x, y].Y, Z = tempo[x, y].Z };
                        //writetext.Write("1");
                        if (DEBUG)
                            depthValuesToByte[4 * index] = depthValuesToByte[4 * index + 1] = depthValuesToByte[4 * index + 2] = 255;
                        continue;
                    }

                    Point3 interpolatedOnBoundary = initiate;
                    for (int boundarySize = 1; boundarySize <= 2; boundarySize++)
                    {
                        interpolatedOnBoundary = getLinearInterpolateOnBoundary(colorWidth, colorHeight, tempo, x, y, boundarySize);
                        if (!interpolatedOnBoundary.Equals(initiate))
                        {
                            result[index] = new CameraSpacePoint { X = interpolatedOnBoundary.X, Y = interpolatedOnBoundary.Y, Z = interpolatedOnBoundary.Z };
                            //writetext.Write("1");
                            if (DEBUG)
                                depthValuesToByte[4 * index] = depthValuesToByte[4 * index + 1] = depthValuesToByte[4 * index + 2] = 255;
                            break;
                        }
                    }

                    //writetext.Write("0");
                    if (DEBUG)
                    {
                        if (interpolatedOnBoundary.Equals(initiate))
                            depthValuesToByte[4 * index] = depthValuesToByte[4 * index + 1] = depthValuesToByte[4 * index + 2] = 0;
                    }
                }


            if (DEBUG)
            {
                Marshal.Copy(depthValuesToByte, 0, ptr.Value, colorWidth * colorHeight * 4);
                bm.UnlockBits(bmapdata);

                bm.Save("1.png");
                bm.Dispose();
            }
        }

        private static Point3 getLinearInterpolateOnBoundary(int colorWidth, int colorHeight, Point3[,] tempo, int x, int y, int size)
        {
            float X = 0.0f, Y = 0.0f, Z = 0.0f;
            var no_neighbors = 0;
            // size-cell boundary
            for (int i = -size; i <= size; i++)
                for (int j = -size; j <= size; j++)
                    if (i == -size || i == size || j == size || j == -size)
                    {
                        if (x + i >= 0 && x + i < colorWidth &&
                            y + j >= 0 && y + j < colorHeight)
                        {
                            if (!tempo[x + i, y + j].Equals(initiate))
                            {
                                X += tempo[x + i, y + j].X;
                                Y += tempo[x + i, y + j].Y;
                                Z += tempo[x + i, y + j].Z;
                                no_neighbors += 1;
                            }
                        }
                    }
            if (no_neighbors != 0)
            {
                return new Point3 { X = X / no_neighbors, Y = Y / no_neighbors, Z = Z / no_neighbors };
            }

            return initiate;
        }
    }
}
