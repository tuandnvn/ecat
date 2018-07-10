using AForge.Video.FFMPEG;
using Microsoft.Kinect;
using System;
using System.Collections.Concurrent;
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
        private int quality = 5000000;
        VideoFileWriter writer = new VideoFileWriter();
        BlockingCollection<Tuple<Bitmap, TimeSpan>> bufferedImages;
        int scaleVideo = 1;
        int rgbStreamedFrame = 0;
        int rgbWritenFrame = 0;
        Bitmap tempo = null;
        Tuple<Bitmap, TimeSpan> bitMapTakenFromBuffer = null;
        object writeRgbLock = new object();
        private int fps = 30;
        DateTime? startRecordingRgb;
        TimeSpan lastWrittenRgbTime = default(TimeSpan);
        TimeSpan? tmspStartRecording = null;

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (!hasColorArrived)
            {
                hasArrived.Signal();
                Console.WriteLine("Signal at color");
                hasColorArrived = true;
            }

            if (recordMode != RecordMode.Playingback)
            {
                // ColorFrame is IDisposable
                using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
                {
                    if (colorFrame != null)
                    {
                        using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                        {
                            // Write the data from colorFrame into rgbBitmap
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

                            // Show rgbBitmap into rgbBoard
                            this.rgbBoard.Image = rgbBitmap;

                            if (recordMode == RecordMode.Recording && this.writer != null)
                            {
                                
                                tmspStartRecording = tmspStartRecording ?? DateTime.Now.TimeOfDay;
                                var currentTime = DateTime.Now.TimeOfDay;
                                if (!startRecordingRgb.HasValue)
                                {
                                    startRecordingRgb = DateTime.Now;
                                }
                                TimeSpan elapse = currentTime - tmspStartRecording.Value;

                                //ResizeAndWriteImageAsync();
                                WriteImageIntoBufferAsync(elapse);
                            }
                        }
                    }
                }
            }
        }

        

        private void startRecordRgb()
        {
            startRecordingRgb = null;
            if (colorFrameDescription != null)
            {
                rgbStreamedFrame = 0;
                rgbWritenFrame = 0;

                Thread thread = new Thread(new ThreadStart(ColorRecordingFunction));
                thread.Start();

                try
                {
                    writer = new VideoFileWriter();
                    writer.Open(tempRgbFileName, colorFrameDescription.Width / scaleVideo, colorFrameDescription.Height / scaleVideo, fps, VideoCodec.MPEG4, quality);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        private async Task WriteImageIntoBufferAsync(TimeSpan timePoint)
        {
            await Task.Run(() => WriteImageIntoBuffer(timePoint));
        }

        private async void WriteImageIntoBuffer(TimeSpan timePoint)
        {
            try
            {
                Interlocked.Increment(ref rgbStreamedFrame);

                Bitmap cloneRgbBitmap = new Bitmap(colorFrameDescription.Width, colorFrameDescription.Height, PixelFormat.Format32bppRgb);
                BitmapData bmapdata = cloneRgbBitmap.LockBits(
                 new Rectangle(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
                 ImageLockMode.WriteOnly,
                 cloneRgbBitmap.PixelFormat);

                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(rgbValues, 0, ptr, colorFrameDescription.Width * colorFrameDescription.Height * 4);
                cloneRgbBitmap.UnlockBits(bmapdata);

                bufferedImages.Add(new Tuple<Bitmap, TimeSpan>(cloneRgbBitmap, timePoint));

                if (rgbStreamedFrame % 50 == 0)
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                Console.WriteLine(colorFrameDescription.Width);
                Console.WriteLine(colorFrameDescription.Height);
            }
        }
        
        private void ColorRecordingFunction()
        {
            // Wait for 10s 
            try
            {
                while (bufferedImages.TryTake(out bitMapTakenFromBuffer, 2000) && bitMapTakenFromBuffer != null)
                {
                    //Console.WriteLine("No of frames in buffer " + bufferedImages.Count);
                    ResizeAndWriteImage(bitMapTakenFromBuffer.Item1, bitMapTakenFromBuffer.Item2);
                    lastWrittenRgbTime = bitMapTakenFromBuffer.Item2;
                }
            }
            finally
            {
                if (writer != null)
                {
                    Console.WriteLine("Finish writing RGB");
                    writer.Dispose();
                    writer = null;

                    finishRecording.Signal();
                }
            }
        }

        public async void ResizeAndWriteImageAsync()
        {
            Bitmap cloneRgbBitmap = new Bitmap(colorFrameDescription.Width, colorFrameDescription.Height, PixelFormat.Format32bppRgb);
            BitmapData bmapdata = cloneRgbBitmap.LockBits(
             new Rectangle(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
             ImageLockMode.WriteOnly,
             cloneRgbBitmap.PixelFormat);

            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(rgbValues, 0, ptr, colorFrameDescription.Width * colorFrameDescription.Height * 4);
            cloneRgbBitmap.UnlockBits(bmapdata);
            await Task.Run(() => ResizeAndWriteImage(cloneRgbBitmap));
        }

        public void ResizeAndWriteImage(Bitmap bitmap, TimeSpan elapse = default(TimeSpan))
        {
            lock (writeRgbLock)
            {
                try
                {
                    if (writer != null && bitmap != null)
                    {
                        if (scaleVideo == 1)
                        {
                            try
                            {
                                //writer.WriteVideoFrame(bitMapTakenFromBuffer);
                                if (elapse == default(TimeSpan))
                                {
                                    writer.WriteVideoFrame(bitmap);
                                }
                                else
                                {
                                    // Console.WriteLine("After elapse " + elapse);
                                    writer.WriteVideoFrame(bitmap, elapse);
                                }

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("IO Exception " + e);
                            }
                        }
                        else
                        {
                            tempo = new Bitmap(bitmap, colorFrameDescription.Width / scaleVideo, colorFrameDescription.Height / scaleVideo);
                            if (elapse == default(TimeSpan))
                            {
                                writer.WriteVideoFrame(bitmap);
                            }
                            else
                            {
                                // Console.WriteLine("After elapse " + elapse);
                                writer.WriteVideoFrame(bitmap, elapse);
                            }
                        }
                    }
                }
                finally
                {
                    bitmap.Dispose();
                }
            }
        }

        private void finishWriteRgb()
        {
            //lock (writeRgbLock)
            //{
            //    if (writer != null)
            //    {
            //        writer.Dispose();
            //        writer = null;
            //    }
            //}
        }
    }
}
