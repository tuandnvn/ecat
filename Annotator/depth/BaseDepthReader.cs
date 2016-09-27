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
        private Lazy<BinaryReader> depthReader;

        private int _depthWidth;
        private int _depthHeight;
        private int _depthFrame;
        private List<Int32> _depthFrameTimePoints;

        public int depthWidth
        {
            get
            {
                var v = depthReader.Value;
                return _depthWidth;
            }
        }

        public int depthHeight
        {
            get
            {
                var v = depthReader.Value;
                return _depthHeight;
            }
        }
        public int depthFrame
        {
            get
            {
                var v = depthReader.Value;
                return _depthFrame;
            }
        }

        public List<Int32> depthFrameTimePoints
        {
            get
            {
                var v = depthReader.Value;
                return _depthFrameTimePoints;
            }
        }

        public BaseDepthReader(string fileName)
        {
            this.fileName = fileName;
            depthReader = new Lazy<BinaryReader>(() =>
            {
                try
                {
                    var _depthReader = new BinaryReader(File.Open(fileName, FileMode.Open));

                    _depthWidth = _depthReader.ReadInt16();
                    _depthHeight = _depthReader.ReadInt16();

                    //Read metadata
                    _depthReader.BaseStream.Seek(-4, SeekOrigin.End);
                    _depthFrame = _depthReader.ReadInt32();

                    _depthReader.BaseStream.Seek(-4 * (_depthFrame + 1), SeekOrigin.End);
                    byte[] vals = _depthReader.ReadBytes(4 * _depthFrame);

                    _depthFrameTimePoints = new List<Int32>();
                    for (int i = 0; i < _depthFrame; i++)
                    {
                        _depthFrameTimePoints.Add(BitConverter.ToInt32(vals, 4 * i));
                    }

                    return _depthReader;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception in BaseDepthReader initialization");
                    Console.WriteLine(e);
                    this.Dispose();
                }
                return null;
            });
        }

        public ushort[] readFrame(int frame)
        {
            if (depthReader.Value != null)
            {
                ushort[] depthVals = new ushort[depthWidth * depthHeight];
                // Plus 4 for depth frame, depth width , each two bytes
                int beginOffset = 4;

                int frameSize = sizeof(short) * depthWidth * depthHeight;
                int offset = frame * frameSize + beginOffset;
                depthReader.Value.BaseStream.Seek(offset, SeekOrigin.Begin);
                byte[] vals = depthReader.Value.ReadBytes(frameSize);

                Buffer.BlockCopy(vals, 0, depthVals, 0, frameSize);


                return depthVals;
            }
            return null;
        }

        public ushort[] readFrameAtTime(int milisecondFromStart)
        {
            if (depthFrameTimePoints == null)
                return null;

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
            if (vals != null)
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
            if (depthReader != null && depthReader.IsValueCreated)
            {
                depthReader.Value.Dispose();
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
