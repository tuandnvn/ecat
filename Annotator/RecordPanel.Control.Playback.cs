using Emgu.CV;
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
    partial class RecordPanel
    {
        Capture rgbCapture = null;
        BinaryReader depthReader = null;
        int depthWidth;
        int depthHeight;
        object playBackRgbLock;
        int depthPlaybackFrameNo;
        List<Int32> depthFrameTimePoints;
        int rgbPlaybackFrameNo;

        private void handlePlayButtonOn()
        {
            finishRecording.Wait();
            playButton.ImageIndex = 3;

            playBackRgbLock = new object();
            rgbBoard.playbackLock = playBackRgbLock;

            // Reread rgb file
            rgbCapture = new Capture(tempRgbFileName);
            rgbBoard.mat = rgbCapture.QueryFrame();
            rgbBoard.Image = rgbBoard.mat.Bitmap;

             rgbPlaybackFrameNo = (int)rgbCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            int frameWidth = (int)rgbCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            int frameHeight = (int)rgbCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);

            // Add a range selector
            range1.recordPanel = this;
            range1.start = 1;
            range1.end = rgbPlaybackFrameNo;

            depthReader = new BinaryReader(File.Open(tempDepthFileName, FileMode.Open));
            depthWidth = depthReader.ReadInt16();
            depthHeight = depthReader.ReadInt16();

            //Read metadata
            depthReader.BaseStream.Seek(-4, SeekOrigin.End);
            depthPlaybackFrameNo = depthReader.ReadInt32();

            depthReader.BaseStream.Seek(-4 * (depthPlaybackFrameNo + 1), SeekOrigin.End);
            byte[] vals = depthReader.ReadBytes(4 * depthPlaybackFrameNo);

            depthFrameTimePoints = new List<Int32>();
            for (int i = 0; i < depthPlaybackFrameNo; i++)
            {
                //Console.WriteLine("depthFrameTimePoint " + BitConverter.ToInt32(vals, 4 * i));
                depthFrameTimePoints.Add(BitConverter.ToInt32(vals, 4 * i));
            }

            depthBitmap = new Bitmap(depthWidth, depthHeight, PixelFormat.Format32bppRgb);
            depthValuesToByte = new byte[depthWidth * depthHeight * 4];

            endTimeLabel.Text = (int) lastWrittenRgbTime.TotalMinutes + ":" + lastWrittenRgbTime.Seconds.ToString("00") + "." + lastWrittenRgbTime.Milliseconds.ToString("000");

            // Set values for playBar
            playBar.Enabled = true;
            playBar.Minimum = 1;
            playBar.Maximum = rgbPlaybackFrameNo;

            helperTextBox.Text = "Temporary rgb file has written " + rgbStreamedFrame + " frames \n"
                + "Temporary depth file has written " + depthFrame + " frames \n"
                + "Temporary rgb file has " + rgbPlaybackFrameNo + " frames of size = ( " + frameWidth + " , " + frameHeight + " ) \n"
                + "Temporary depth file has " + depthFrame + " frames of size = ( " + depthWidth + " , " + depthHeight + " ) \n"
                + "RecordingTime " + lastWrittenRgbTime;

            this.Invalidate();
        }

        private void updateRgbBoardWithFrame()
        {
            if (rgbCapture == null)
            {
                return;
            }

            lock (playBackRgbLock)
            {
                Console.WriteLine("updateRgbBoardWithFrame " + (playBar.Value - 1));
                rgbCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, playBar.Value - 1);

                Mat m = rgbCapture.QueryFrame();
                if (m != null)
                {
                    rgbBoard.mat = m;
                    rgbBoard.Image = rgbBoard.mat.Bitmap;
                }
            }
        }

        private void updateDepthBoardWithFrame()
        {
            try
            {
                int recordedTimeForRgbFrame = (int)(lastWrittenRgbTime.TotalMilliseconds * (playBar.Value - 1) / (rgbPlaybackFrameNo - 1));
                int appropriateDepthFrame = depthFrameTimePoints.BinarySearch(recordedTimeForRgbFrame);

                // bitwise complement if it is not found
                if (appropriateDepthFrame < 0)
                {
                    appropriateDepthFrame = ~appropriateDepthFrame;

                    if (appropriateDepthFrame == depthFrameTimePoints.Count)
                    {
                        appropriateDepthFrame = depthFrameTimePoints.Count - 1;
                    } else if (appropriateDepthFrame > 0)
                    {
                        // Smaller timepoint is closer
                        if ((recordedTimeForRgbFrame - depthFrameTimePoints[appropriateDepthFrame - 1]) < depthFrameTimePoints[appropriateDepthFrame] - recordedTimeForRgbFrame)
                        {
                            appropriateDepthFrame--;
                        }
                    }
                }

                Console.WriteLine("lastWrittenRgbTime " + lastWrittenRgbTime.TotalMilliseconds);
                Console.WriteLine("playBar.Value " + playBar.Value);
                Console.WriteLine("recordedTimeForRgbFrame " + recordedTimeForRgbFrame);
                Console.WriteLine("appropriateDepthFrame " + appropriateDepthFrame);
                Console.WriteLine("recordedTimeForDepthFrame " + depthFrameTimePoints[appropriateDepthFrame]);
                // Plus 4 for depth frame, depth width , each two bytes
                int beginOffset = 4;

                int frameSize = sizeof(short) * depthWidth * depthHeight;
                int offset = appropriateDepthFrame * frameSize + beginOffset;
                depthReader.BaseStream.Seek(offset, SeekOrigin.Begin);
                

                byte[] vals = depthReader.ReadBytes(frameSize);

                BitmapData bmapdata = depthBitmap.LockBits(
                                         new Rectangle(0, 0, depthWidth, depthHeight),
                                         ImageLockMode.WriteOnly,
                                         depthBitmap.PixelFormat);

                IntPtr ptr = bmapdata.Scan0;

                for (int i = 0; i < depthWidth * depthHeight; i++)
                {
                    //Console.WriteLine((byte)(((int)vals[2 * i] << 8 + vals[2 * i + 1]) / scale));
                    depthValuesToByte[4 * i] = depthValuesToByte[4 * i + 1] = depthValuesToByte[4 * i + 2] = (byte)(BitConverter.ToUInt16(vals, 2 * i) / scale);
                }

                Marshal.Copy(depthValuesToByte, 0, ptr, depthWidth * depthHeight * 4);

                depthBitmap.UnlockBits(bmapdata);

                this.depthBoard.Image = depthBitmap;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            } catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }

        private void updateRigWithFrame()
        {

        }

        internal void setTrackbarLocation(int value)
        {
            playBar.Value = value;
        }

        private void handlePlayButtonOff()
        {
            playButton.ImageIndex = 2;
        }

    }
}
