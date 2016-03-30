using AForge.Video.FFMPEG;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Annotator
{
    partial class RecordPanel
    {
        int scaleVideo = 2;
        int rgbStreamedFrame = 0;
        int rgbWritenFrame = 0;
        Bitmap tempo = null;
        Bitmap bitMapTakenFromBuffer = null;
        object writeRgbLock = new object();

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (recordMode != RecordMode.Playingback)
            {
                // ColorFrame is IDisposable
                using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
                {
                    if (colorFrame != null)
                    {
                        using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                        {
                            colorFrame.CopyConvertedFrameDataToArray(rgbValues, ColorImageFormat.Bgra);

                            BitmapData bmapdata = rgbBitmap.LockBits(
                                 new Rectangle(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
                                 ImageLockMode.WriteOnly,
                                 rgbBitmap.PixelFormat);
                            IntPtr ptr = bmapdata.Scan0;
                            Marshal.Copy(rgbValues, 0, ptr, colorFrameDescription.Width * colorFrameDescription.Height * 4);
                            rgbBitmap.UnlockBits(bmapdata);

                            this.widthAspect = (float)this.rgbBoard.Width / colorFrameDescription.Width;
                            this.heightAspect = (float)this.rgbBoard.Height / colorFrameDescription.Height;
                            this.rgbBoard.Image = rgbBitmap;

                            if (recordMode == RecordMode.Recording && this.writer != null)
                            {
                                WriteImageIntoBufferAsync();
                            }
                        }
                    }
                }
            }
        }

        private void startRecordRgb()
        {
            if (colorFrameDescription != null)
            {
                Thread thread = new Thread(new ThreadStart(ColorRecordingFunction));
                thread.Start();

                try
                {
                    //rgbWriter = new VideoWriter(tempRgbFileName, -1, FRAME_PER_SECOND, new Size(colorFrameDescription.Width / scaleVideo, colorFrameDescription.Height / scaleVideo), true);
                    //Console.WriteLine("Finish create writer");

                    writer = new VideoFileWriter();
                    writer.Open(tempRgbFileName, colorFrameDescription.Width / scaleVideo, colorFrameDescription.Height / scaleVideo, FRAME_PER_SECOND, VideoCodec.MPEG4, 20000000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        private async Task WriteImageIntoBufferAsync()
        {
            await Task.Run(() => WriteImageIntoBuffer());
        }

        private async void WriteImageIntoBuffer()
        {
            Interlocked.Increment(ref rgbStreamedFrame);
            Bitmap rgbBitmap = new Bitmap(colorFrameDescription.Width, colorFrameDescription.Height, PixelFormat.Format32bppRgb);
            BitmapData bmapdata = rgbBitmap.LockBits(
             new Rectangle(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
             ImageLockMode.WriteOnly,
             rgbBitmap.PixelFormat);

            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(rgbValues, 0, ptr, colorFrameDescription.Width * colorFrameDescription.Height * 4);
            rgbBitmap.UnlockBits(bmapdata);

            bufferedImages.Add(rgbBitmap);

            if (rgbStreamedFrame % 50 == 0)
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }

        private void ColorRecordingFunction()
        {
            Console.WriteLine("Begin writing");

            // Wait for 10s 
            while (bufferedImages.TryTake(out bitMapTakenFromBuffer, 2000) && bitMapTakenFromBuffer != null)
            {
                ResizeAndWriteImage();
            }

            Console.WriteLine("Finish writing");
        }

        public void ResizeAndWriteImage()
        {
            if (writer != null && bitMapTakenFromBuffer != null)
            {
                lock (writeRgbLock)
                {
                    if (scaleVideo == 1)
                    {
                        writer.WriteVideoFrame(bitMapTakenFromBuffer);
                    }
                    else
                    {
                        tempo = new Bitmap(bitMapTakenFromBuffer, colorFrameDescription.Width / scaleVideo, colorFrameDescription.Height / scaleVideo);
                        writer.WriteVideoFrame(tempo);
                    }
                }
            }
        }

        private void finishWriteRgb()
        {
            lock (writeRgbLock)
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
            }
        }
    }
}
