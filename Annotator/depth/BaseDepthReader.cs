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
    public class BaseDepthReader : IDepthReader
    {
        public String fileName { get; }
        private BinaryReader depthReader = null;
        public int depthWidth { get; private set; }
        public int depthHeight { get; private set; }
        public int depthFrame { get; private set; }
        private List<Int32> depthFrameTimePoints;

        public BaseDepthReader ( string fileName )
        {
            try
            {
                this.fileName = fileName;
                depthReader = new BinaryReader(File.Open(fileName, FileMode.Open));
                depthWidth = depthReader.ReadInt16();
                depthHeight = depthReader.ReadInt16();

                //Read metadata
                depthReader.BaseStream.Seek(-4, SeekOrigin.End);
                depthFrame = depthReader.ReadInt32();

                depthReader.BaseStream.Seek(-4 * (depthFrame + 1), SeekOrigin.End);
                byte[] vals = depthReader.ReadBytes(4 * depthFrame);

                depthFrameTimePoints = new List<Int32>();
                for (int i = 0; i < depthFrame; i++)
                {
                    //Console.WriteLine("depthFrameTimePoint " + BitConverter.ToInt32(vals, 4 * i));
                    depthFrameTimePoints.Add(BitConverter.ToInt32(vals, 4 * i));
                }
            } catch (Exception e)
            {
                Console.WriteLine("Exception in BaseDepthReader initialization");
                Console.WriteLine(e);
                this.Dispose();
            }
        }

        public ushort[] readFrame(int frame)
        {
            
            if (depthReader != null)
            {
                ushort[] depthVals = new ushort[depthWidth * depthHeight];
                // Plus 4 for depth frame, depth width , each two bytes
                int beginOffset = 4;

                int frameSize = sizeof(short) * depthWidth * depthHeight;
                int offset = frame * frameSize + beginOffset;
                depthReader.BaseStream.Seek(offset, SeekOrigin.Begin);
                byte[] vals = depthReader.ReadBytes(frameSize);

                Buffer.BlockCopy(vals, 0, depthVals, 0, frameSize);
                

                return depthVals;
            }
            return null;
        }

        public ushort[] readFrameAtTime(int milisecondFromStart)
        {
            int appropriateDepthFrame = depthFrameTimePoints.BinarySearch(milisecondFromStart);

            // bitwise complement if it is not found
            if (appropriateDepthFrame < 0)
            {
                appropriateDepthFrame = ~appropriateDepthFrame;

                if (appropriateDepthFrame == depthFrame)
                {
                    appropriateDepthFrame = depthFrame - 1;
                }
                else if (appropriateDepthFrame > 0)
                {
                    // Smaller timepoint is closer
                    if ((milisecondFromStart - depthFrameTimePoints[appropriateDepthFrame - 1]) < depthFrameTimePoints[appropriateDepthFrame] - milisecondFromStart)
                    {
                        appropriateDepthFrame--;
                    }
                }
            }

            if (appropriateDepthFrame < 0 || appropriateDepthFrame >= depthFrame) return null;
            return readFrame(appropriateDepthFrame);
        }

        public void readFrameAtTimeToBitmap(int milisecondFromStart, Bitmap depthBitmap, byte[] depthValuesToByte, float scale)
        {
            ushort[] vals = readFrameAtTime(milisecondFromStart);
            if ( vals != null )
            {
                BitmapData bmapdata = depthBitmap.LockBits(
                                     new Rectangle(0, 0, depthWidth, depthHeight),
                                     ImageLockMode.WriteOnly,
                                     depthBitmap.PixelFormat);

                IntPtr ptr = bmapdata.Scan0;

                for (int i = 0; i < depthWidth * depthHeight; i++)
                {
                    depthValuesToByte[4 * i] = depthValuesToByte[4 * i + 1] = depthValuesToByte[4 * i + 2] = (byte)(vals[i] / scale);
                }

                Marshal.Copy(depthValuesToByte, 0, ptr, depthWidth * depthHeight * 4);

                depthBitmap.UnlockBits(bmapdata);
            }
        }

        public void Dispose()
        {
            if (depthReader != null)
            {
                depthReader.Dispose();
                depthReader = null;
            }
        }

        public int getFrameCount()
        {
            return depthFrame;
        }

        public int getWidth()
        {
            return depthWidth;
        }

        public int getHeight()
        {
            return depthHeight;
        }
    }
}
