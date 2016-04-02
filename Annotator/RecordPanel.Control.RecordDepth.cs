using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Annotator
{
    partial class RecordPanel
    {
        BinaryWriter depthWriter;
        List<TimeSpan> timePoints;
        int depthFrame = 0;

        private void startRecordDepth()
        {
            if (depthFrameDescription != null)
            {
                try
                {
                    depthFrame = 0;

                    depthWriter = new BinaryWriter(File.Open(tempDepthFileName, FileMode.Create));
                    depthWriter.Write((short)depthFrameDescription.Width);
                    depthWriter.Write((short)depthFrameDescription.Height);

                    timePoints = new List<TimeSpan>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void Reader_DepthFrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            if (recordMode != RecordMode.Playingback)
            {
                using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
                {
                    if (depthFrame != null)
                    {
                        try
                        {
                            using (KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                            {
                                depthFrame.CopyFrameDataToArray(depthValues);

                                BitmapData bmapdata = depthBitmap.LockBits(
                                     new Rectangle(0, 0, depthFrameDescription.Width, depthFrameDescription.Height),
                                     ImageLockMode.WriteOnly,
                                     depthBitmap.PixelFormat);

                                IntPtr ptr = bmapdata.Scan0;


                                //int hueScale = 12;
                                //int hueStep = 8000 / hueScale;
                                //int satuationStep = hueStep / 256;
                                //for (int column = 0; column < depthFrameDescription.Width; column++)
                                //    for (int row = 0; row < depthFrameDescription.Height; row++)
                                //    {
                                //        //Hsv newHsv = new Hsv();
                                //        //newHsv.Hue = ( 180/ hueScale) * ( depthValues[column] / hueStep );
                                //        //newHsv.Satuation = (depthValues[column] % hueStep) / satuationStep;
                                //        //newHsv.Value = (depthValues[column] % hueStep) % satuationStep * ( 256 / satuationStep );

                                //        try
                                //        {
                                //            depthImage.Data[row, column, 0] = (byte)((180 / hueScale) * (depthValues[column] / hueStep));
                                //            depthImage.Data[row, column, 1] = (byte)((depthValues[column] % hueStep) / satuationStep);
                                //            depthImage.Data[row, column, 2] = (byte)((depthValues[column] % hueStep) % satuationStep * (256 / satuationStep));
                                //        }
                                //        catch (CvException cv)
                                //        {
                                //            Console.WriteLine("Row " + row + " column " + column);
                                //        }
                                //    }

                                for (int i = 0; i < depthFrameDescription.Width * depthFrameDescription.Height; i++)
                                {
                                    depthValuesToByte[4 * i] = depthValuesToByte[4 * i + 1] = depthValuesToByte[4 * i + 2] = (byte)(depthValues[i] / scale);

                                    //if (depthValues[i] < 2000)
                                    //    depthValuesToByte[4 * i] = (byte)(depthValues[i] / scale);
                                    //else if (depthValues[i] < 4000)
                                    //    depthValuesToByte[4 * i + 1] = (byte)((depthValues[i] - 2000) / scale);
                                    //else if (depthValues[i] < 6000)
                                    //    depthValuesToByte[4 * i + 2] = (byte)((depthValues[i] - 4000) / scale);
                                    //else if (depthValues[i] < 8000)
                                    //    depthValuesToByte[4 * i + 3] = (byte)((depthValues[i] - 6000) / scale);
                                }
                                Marshal.Copy(depthValuesToByte, 0, ptr, depthFrameDescription.Width * depthFrameDescription.Height * 4);

                                depthBitmap.UnlockBits(bmapdata);

                                this.depthBoard.Image = depthBitmap;
                                //this.depthBoard.Image = depthImage.Bitmap;
                                if (recordMode == RecordMode.Recording && this.depthWriter != null)
                                {
                                    if (tmspStartRecording.HasValue)
                                    {
                                        var currentTime = DateTime.Now.TimeOfDay;
                                        TimeSpan elapse = currentTime - tmspStartRecording.Value;

                                        timePoints.Add(elapse);

                                        WriteDepthIntoFileAsync();
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
        }

        private async Task WriteDepthIntoFileAsync()
        {
            await Task.Run(() => WriteDepthIntoFile());
        }

        private void WriteDepthIntoFile()
        {
            try
            {
                lock (writeDepthLock)
                {
                    for (int i = 0; i < depthFrameDescription.Width * depthFrameDescription.Height; i++)
                        depthWriter.Write((UInt16)depthValues[i]);
                }
                Interlocked.Increment(ref depthFrame);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(depthFrameDescription.Width  + " " + depthFrameDescription.Height + " " + depthValues.Count() + " "+ e);
            }

        }

        private void finishWriteDepth()
        {
            lock (writeDepthLock)
            {
                if (depthWriter != null)
                {
                    // Write metadata
                    foreach (TimeSpan elapse in timePoints)
                    {
                        int miliseconds = (int)elapse.TotalMilliseconds;
                        depthWriter.Write((UInt32)miliseconds);
                    }
                    depthWriter.Write((UInt32)depthFrame);

                    // Close file
                    depthWriter.Dispose();
                    depthWriter = null;
                    finishRecording.Signal();
                }
            }
        }
    }
}
