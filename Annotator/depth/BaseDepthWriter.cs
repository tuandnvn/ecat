using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Annotator.depth
{
    class BaseDepthWriter : IDepthWriter, IDisposable
    {
        private object writeDepthLock = new object();
        private BinaryWriter depthWriter;
        private List<int> timePoints;
        private int _depthFrame = 0;
        public int depthFrame { get { return _depthFrame; } private set { _depthFrame = value; } }
        public int depthWidth { get; private set; }
        public int depthHeight { get; private set; }

        public BaseDepthWriter(string fileName, int depthWidth, int depthHeight)
        {
            try
            {
                depthWriter = new BinaryWriter(File.Open(fileName, FileMode.Create));

                depthWriter.Write((short)depthWidth);
                depthWriter.Write((short)depthHeight);

                timePoints = new List<int>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void writeFrame(int milisecondFromStart, ushort[] depthValues)
        {
            try
            {
                lock (writeDepthLock)
                {
                    for (int i = 0; i < depthWidth * depthHeight; i++)
                        depthWriter.Write(depthValues[i]);
                }
                Interlocked.Increment(ref _depthFrame);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(depthWidth + " " + depthHeight + " " + depthValues.Count() + " " + e);
            }
        }

        public void Dispose()
        {
            if (depthWriter != null)
            {
                writeMetadataAtEnd();
                depthWriter.Dispose();
                depthWriter = null;
            }
        }

        ~BaseDepthWriter()
        {
            if (depthWriter != null)
            {
                writeMetadataAtEnd();
                depthWriter.Dispose();
                depthWriter = null;
            }
        }

        private void writeMetadataAtEnd()
        {
            lock (writeDepthLock)
            {
                if (depthWriter != null)
                {
                    // Write metadata
                    foreach (int elapse in timePoints)
                    {
                        depthWriter.Write((UInt32)elapse);
                    }
                    depthWriter.Write((UInt32)depthFrame);

                    // Close file
                    depthWriter.Dispose();
                    depthWriter = null;
                }
            }
        }
    }
}
