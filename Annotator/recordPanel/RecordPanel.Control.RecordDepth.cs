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
        IDepthWriter depthWriter;

        private void startRecordDepth()
        {
            if (depthFrameDescription != null)
            {
                depthWriter = new BaseDepthWriter(tempDepthFileName, depthFrameDescription.Width, depthFrameDescription.Height);
                
            }
        }

        private void Reader_DepthFrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            if (!hasDepthArrived)
            {
                hasArrived.Signal();
                Console.WriteLine("Signal at depth");
                hasDepthArrived = true;
            }

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

                                for (int i = 0; i < depthFrameDescription.Width * depthFrameDescription.Height; i++)
                                {
                                    depthValuesToByte[4 * i] = depthValuesToByte[4 * i + 1] = depthValuesToByte[4 * i + 2] = (byte)(depthValues[i] / scale);
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

                                        WriteDepthIntoFileAsync(elapse);
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

        private async Task WriteDepthIntoFileAsync(TimeSpan elapse)
        {
            await Task.Run(() => WriteDepthIntoFile(elapse));
        }

        private void WriteDepthIntoFile(TimeSpan elapse)
        {
            depthWriter.writeFrame((int) elapse.TotalMilliseconds, depthValues);
        }

        private void finishWriteDepth()
        {
            depthWriter.Dispose();
            finishRecording.Signal();
        }
    }
}
