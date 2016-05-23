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

    class DepthCoordinateMappingReader
    {
        public String filename { get; }
        public int width { private set; get; }
        public int height { private set; get; }
        public int shortRange { private set; get; }
        public int longRange { private set; get; }
        public PointF[,] shortRangeMap;
        public PointF[,] longRangeMap;

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

                shortRangeMap = new PointF[width, height];
                longRangeMap = new PointF[width, height];

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        var x = coordinateMappingReader.ReadSingle();
                        var y = coordinateMappingReader.ReadSingle();
                        shortRangeMap[i, j] = new PointF { X = x, Y = y };
                    }

                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        var x = coordinateMappingReader.ReadSingle();
                        var y = coordinateMappingReader.ReadSingle();
                        longRangeMap[i, j] = new PointF { X = x, Y = y };
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

            var initiate = new Point3(-1, -1, -1);
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

                    int color_X = (int) colorPixelPoint.X;
                    int color_Y = (int) colorPixelPoint.Y;

                    if (color_X >= 0 && color_X < colorWidth && color_Y >= 0 && color_Y < colorHeight)
                    {
                        if ( ! result[color_X, color_Y].Equals(initiate) )
                        {
                            // Replace the current one with the one closer to the camera
                            if ( result[color_X, color_Y].Z > cameraSpacePoint.Z )
                            {
                                result[color_X, color_Y] = cameraSpacePoint;
                            }
                        } else
                        {
                            result[color_X, color_Y] = cameraSpacePoint;
                        }
                    }
                }

            return result;
        }
    }
}
