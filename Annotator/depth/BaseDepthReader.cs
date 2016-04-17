using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator.depth
{
    class BaseDepthReader : IDepthReader
    {
        private BinaryReader depthReader = null;
        public int depthWidth { get; private set; }
        public int depthHeight { get; private set; }
        public int depthFrame { get; private set; }
        private List<Int32> depthFrameTimePoints;

        public BaseDepthReader ( string fileName)
        {
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
        }

        public ushort[] readFrame(int frame)
        {
            // Plus 4 for depth frame, depth width , each two bytes
            int beginOffset = 4;

            int frameSize = sizeof(short) * depthWidth * depthHeight;
            int offset = frame * frameSize + beginOffset;
            depthReader.BaseStream.Seek(offset, SeekOrigin.Begin);
            byte[] vals = depthReader.ReadBytes(frameSize);

            ushort[] depthVals = new ushort[depthWidth * depthHeight];

            Buffer.BlockCopy(vals, 0, depthVals, 0, frameSize);
            return depthVals;
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
