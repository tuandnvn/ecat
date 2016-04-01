﻿using AForge.Video.FFMPEG;
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
        VideoFileWriter writer = new VideoFileWriter();
        BlockingCollection<Tuple<Bitmap, TimeSpan>> bufferedImages;
        int scaleVideo = 1;
        int rgbStreamedFrame = 0;
        int rgbWritenFrame = 0;
        Bitmap tempo = null;
        Tuple<Bitmap, TimeSpan> bitMapTakenFromBuffer = null;
        object writeRgbLock = new object();
        private const int FRAME_PER_SECOND = 25;
        


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
                                TimeSpan elapse = currentTime - tmspStartRecording.Value;

                                //ResizeAndWriteImageAsync();
                                WriteImageIntoBufferAsync(elapse);
                            }
                        }
                    }
                }
            }
        }

        TimeSpan? tmspStartRecording = null;

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
                    writer.Open(tempRgbFileName, colorFrameDescription.Width / scaleVideo, colorFrameDescription.Height / scaleVideo, FRAME_PER_SECOND, VideoCodec.MPEG4, 2000000);
                    //writer.Open(tempRgbFileName, colorFrameDescription.Width / scaleVideo, colorFrameDescription.Height / scaleVideo);
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

        TimeSpan lastWrittenRgbTime = default(TimeSpan);
        private void ColorRecordingFunction()
        {
            Console.WriteLine("Begin writing");

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
                    Console.WriteLine("Finish writing");
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
                                    Console.WriteLine("After elapse " + elapse);
                                    writer.WriteVideoFrame(bitmap, elapse);
                                }

                            }
                            catch (System.IO.IOException e)
                            {
                                System.Windows.Forms.MessageBox.Show("IO Exception " + e);
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
                                Console.WriteLine("After elapse " + elapse);
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
