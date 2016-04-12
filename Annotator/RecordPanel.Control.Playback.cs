using Annotator.depth;
using Emgu.CV;
using Microsoft.Kinect;
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
        VideoReader playBackVideo;
        IDepthReader depthReader;
        object playBackRgbLock;
        int depthPlaybackFrameNo;
        int depthFrame;
        int depthWidth;
        int depthHeight;
        int rgbPlaybackFrameNo;
        private Body[] playbackBodies = null;

        private void handlePlayButtonOn()
        {
            finishRecording.Wait();
            recordMode = RecordMode.Playingback;

            playButton.ImageIndex = 3;
            
            playBackRgbLock = new object();
            rgbBoard.playbackLock = playBackRgbLock;

            // Reread rgb file

            playBackVideo = new VideoReader(tempRgbFileName, (int) lastWrittenRgbTime.TotalMilliseconds);
            rgbBoard.mat = playBackVideo.getFrame(0);
            rgbBoard.Image = rgbBoard.mat.Bitmap;

            rgbPlaybackFrameNo = playBackVideo.frameCount;
            int frameWidth = playBackVideo.frameWidth;
            int frameHeight = playBackVideo.frameHeight;

            // Add a range selector
            cropBar.recordPanel = this;
            cropBar.start = 1;
            cropBar.end = rgbPlaybackFrameNo;
            cropBar.Visible = true;

            depthReader = new BaseDepthReader(tempDepthFileName);
            depthFrame = depthReader.getFrameCount();
            depthWidth = depthReader.getWidth();
            depthHeight = depthReader.getHeight();
            depthBitmap = new Bitmap(depthWidth, depthHeight, PixelFormat.Format32bppRgb);
            depthValuesToByte = new byte[depthWidth * depthHeight * 4];

            endTimeLabel.Text = (int) lastWrittenRgbTime.TotalMinutes + ":" + lastWrittenRgbTime.Seconds.ToString("00") + "." + lastWrittenRgbTime.Milliseconds.ToString("000");

            // Set values for playBar
            playBar.Enabled = true;
            playBar.Minimum = 1;
            playBar.Maximum = rgbPlaybackFrameNo;
            playBar.Value = 1;

            helperTextBox.Text = "Temporary rgb file has written " + rgbStreamedFrame + " frames \n"
                + "Temporary rgb file has " + rgbPlaybackFrameNo + " frames of size = ( " + frameWidth + " , " + frameHeight + " ) \n"
                + "Temporary depth file has " + depthPlaybackFrameNo + " frames of size = ( " + depthWidth + " , " + depthHeight + " ) \n"
                + "RecordingTime " + lastWrittenRgbTime;

            main.Invalidate();
        }

        private void updateRgbBoardWithFrame()
        {
            if (playBackVideo == null)
            {
                return;
            }

            lock (playBackRgbLock)
            {
                Mat m = playBackVideo.getFrame(playBar.Value - 1);
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

                ushort[] vals = depthReader.readFrameAtTime(recordedTimeForRgbFrame);

                BitmapData bmapdata = depthBitmap.LockBits(
                                         new Rectangle(0, 0, depthWidth, depthHeight),
                                         ImageLockMode.WriteOnly,
                                         depthBitmap.PixelFormat);

                IntPtr ptr = bmapdata.Scan0;

                for (int i = 0; i < depthWidth * depthHeight; i++)
                {
                    //Console.WriteLine((byte)(((int)vals[2 * i] << 8 + vals[2 * i + 1]) / scale));
                    depthValuesToByte[4 * i] = depthValuesToByte[4 * i + 1] = depthValuesToByte[4 * i + 2] = (byte)(vals[i] / scale);
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
            int recordedTimeForRgbFrame = (int)(lastWrittenRgbTime.TotalMilliseconds * (playBar.Value - 1) / (rgbPlaybackFrameNo - 1));
            int appropriateRigFrame = recordedRigTimePoints.BinarySearch(recordedTimeForRgbFrame);

            // bitwise complement if it is not found
            if (appropriateRigFrame < 0)
            {
                appropriateRigFrame = ~appropriateRigFrame;

                if (appropriateRigFrame == recordedRigTimePoints.Count)
                {
                    appropriateRigFrame = recordedRigTimePoints.Count - 1;
                }
                else if (appropriateRigFrame > 0)
                {
                    // Smaller timepoint is closer
                    if ((recordedTimeForRgbFrame - recordedRigTimePoints[appropriateRigFrame - 1]) < recordedRigTimePoints[appropriateRigFrame] - recordedTimeForRgbFrame)
                    {
                        appropriateRigFrame--;
                    }
                }
            }

            this.playbackBodies = recordedRigs[appropriateRigFrame];
            Invalidate();
        }

        internal void setTrackbarLocation(int value)
        {
            playBar.Value = value;
        }

        private void handlePlayButtonOff()
        {
            playButton.ImageIndex = 2;

            recordMode = RecordMode.None;
            cropBar.Visible = false;
            tmspStartRecording = null;

            helperTextBox.Text = "";

            if (playBackVideo != null)
            {
                playBackVideo.Dispose();
                playBackVideo = null;
            }

            if (depthReader != null)
            {
                depthReader.Dispose();
                depthReader = null;
            }
            endTimeLabel.Text = "00:00.000";

            main.Invalidate();
        }
    }
}
