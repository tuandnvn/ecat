using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    /// <summary>
    /// Everything on the middle panel, frameTrackbar
    /// </summary>
    partial class Main
    {
        private int sessionStart;
        private int sessionEnd;
        private bool playStatus = false;
        private int framePerSecond = 24;

        private void playbackVideoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string videoFilename = playbackFileComboBox.SelectedItem.ToString();

                if (videoFilename.isVideoFile())
                {
                    depthReader = null;
                    loadVideo(videoFilename);

                    if (videoReader != null)
                    {
                        //endPoint = startPoint = new Point();
                        boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
                        frameNumberLbl.Text = "Frame: " + frameTrackBar.Value;

                        Mat m = videoReader.getFrame(0);
                        if (m != null)
                        {
                            pictureBoard.mat = m;
                            pictureBoard.Image = pictureBoard.mat.Bitmap;
                        }

                        setLeftTopPanel();
                    }
                }

                if (videoFilename.isDepthFile())
                {
                    videoReader = null;
                    depthReader = currentSession.getDepth(videoFilename);

                    if (depthReader == null) return;

                    if (depthValuesToByte == null)
                    {
                        depthValuesToByte = new byte[depthReader.getWidth() * depthReader.getHeight() * 4];
                    }

                    if (depthBitmap == null)
                    {
                        depthBitmap = new Bitmap(depthReader.getWidth(), depthReader.getHeight(), PixelFormat.Format32bppRgb);
                    }

                    depthReader.readFrameAtTimeToBitmap(0, depthBitmap, depthValuesToByte, 8000.0f / 256);

                    if (depthBitmap != null)
                    {
                        pictureBoard.Image = depthBitmap;
                    }
                }

                frameTrackBar_ValueChanged(null, null);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Select video file exception");
                MessageBox.Show("Exception in opening this file, please try another file", "File exception", MessageBoxButtons.OK);
            }
        }

        private void frameTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Don't allow the track bar value to get out of the range [MinDragValue, MaxDragValue] 
            if (frameTrackBar.Value < frameTrackBar.MinDragVal)
            {
                frameTrackBar.Value = frameTrackBar.MinDragVal;
                return;
            }

            if (frameTrackBar.Value > frameTrackBar.MaxDragVal)
            {
                frameTrackBar.Value = frameTrackBar.MaxDragVal;
                return;
            }

            frameNumberLbl.Text = "Frame: " + frameTrackBar.Value;

            int frameStartWithZero = frameTrackBar.Value - 1;
            if (videoReader != null)
            {
                Mat m = videoReader.getFrame(frameStartWithZero);
                if (m != null)
                {
                    pictureBoard.mat = m;
                    pictureBoard.Image = pictureBoard.mat.Bitmap;
                }
                else
                {
                    Console.WriteLine("Could not get frame for " + frameStartWithZero);
                }
                runGCForImage();
            }

            if (depthReader != null)
            {
                int timeStepForFrame = (int)(currentSession.duration / currentSession.sessionLength);
                int timeFromStart = frameStartWithZero * timeStepForFrame;
                depthReader.readFrameAtTimeToBitmap(timeFromStart, depthBitmap, depthValuesToByte, 8000.0f / 256);

                if (depthBitmap != null)
                {
                    pictureBoard.Image = depthBitmap;
                }
                else
                {
                    Console.WriteLine("Could not get frame for " + frameStartWithZero);
                }

                runGCForImage();
            }

            showPredicates();
        }

        private void StartInSecondTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                changeStartInSecond();
            }
        }

        private void StartInSecondTextBox_LostFocus(object sender, EventArgs e)
        {
            changeStartInSecond();
        }

        private void changeStartInSecond()
        {
            try
            {
                var startInSecond = int.Parse(startInSecondTextBox.Text);
                if (videoReader != null)
                {
                    if (startInSecond * videoReader.fps < videoReader.frameCount)
                    {
                        // Plus one because frame is counted from 1
                        setMinimumFrameTrackBar((int)(videoReader.fps * startInSecond));
                        frameTrackBar_ValueChanged(null, null);
                        if (frameTrackBar.Value < sessionStart)
                        {
                            frameTrackBar.Value = sessionStart;
                        }

                        rescaleFrameTrackBar();
                    }
                }
            }
            catch (Exception)
            {
                startInSecondTextBox.Text = "";
            }
        }

        private void EndInSecondTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                changeEndInSecond();
            }
        }

        private void EndInSecondTextBox_LostFocus(object sender, EventArgs e)
        {
            changeEndInSecond();
        }

        private void changeEndInSecond()
        {
            try
            {
                var endInSecond = int.Parse(endInSecondTextBox.Text);
                if (videoReader != null)
                {
                    if (endInSecond * videoReader.fps < videoReader.frameCount)
                    {
                        setMaximumFrameTrackBar((int)(videoReader.fps * endInSecond) - 1);
                        frameTrackBar_ValueChanged(null, null);
                        rescaleFrameTrackBar();
                    }
                }
            }
            catch (Exception)
            {
                endInSecondTextBox.Text = "";
            }
        }

        private void rescaleFrameTrackBar()
        {
            if (currentSession != null)
            {
                foreach (var objectAnnotation in objectToObjectTracks.Values)
                {
                    objectAnnotation.resetStartEnd(frameTrackBar.Minimum, frameTrackBar.Maximum);
                }

                foreach (var eventAnnotation in mapFromEventToEventAnnotations.Values)
                {
                    eventAnnotation.resetStartEnd(frameTrackBar.Minimum, frameTrackBar.Maximum);
                }
            }

            frameTrackBar.Invalidate();

            Invalidate();
        }

        public void setTrackbarLocation(int value)
        {
            if (value >= frameTrackBar.Minimum && value <= frameTrackBar.Maximum && !editingAtAFrame)
                frameTrackBar.Value = value;
        }

        public void setTrackbarMinDragValue(int value)
        {
            if (value >= frameTrackBar.Minimum && value <= frameTrackBar.Maximum)
                frameTrackBar.MinDragVal = value;
        }

        public void resetTrackbarMinDragValue()
        {
            frameTrackBar.MinDragVal = frameTrackBar.Minimum;
        }

        public void setTrackbarMaxDragValue(int value)
        {
            if (value >= frameTrackBar.Minimum && value <= frameTrackBar.Maximum)
                frameTrackBar.MaxDragVal = value;
        }

        public void resetTrackbarMaxDragValue()
        {
            frameTrackBar.MaxDragVal = frameTrackBar.Maximum;
        }

        private void clearPaintBoardView()
        {
            // Clean the playbackFileComboBox
            clearPlaybackFileComboBox();
            // Clean picture board frame
            pictureBoard.Image = null;
            startInSecondTextBox.Text = "";
            endInSecondTextBox.Text = "";
            setMinimumFrameTrackBar(0);
            setMaximumFrameTrackBar(100);

            handlePause();
        }

        private void clearPlaybackFileComboBox()
        {
            playbackFileComboBox.Items.Clear();
            playbackFileComboBox.Enabled = false;
            frameTrackBar.Enabled = false;
        }

        private void setMinimumFrameTrackBar(int value)
        {
            frameTrackBar.Minimum = value;
            this.sessionStart = value;
        }

        private void setMaximumFrameTrackBar(int value)
        {
            frameTrackBar.Maximum = value;
            this.sessionEnd = value;
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            switchPlay();
        }

        private void switchPlay()
        {
            if (!playStatus)
            {
                handlePlay();
            }
            else
            {
                handlePause();
            }
        }

        private void handlePlay()
        {
            if (!playStatus)
            {
                playBtn.ImageIndex = 3;

                playStatus = !playStatus;

                Task t = Task.Run( async () =>
                {
                    try
                    {
                        int currentFrame = -1;
                        int maxDragVal = -1;
                        this.Invoke((MethodInvoker)delegate {
                            currentFrame = frameTrackBar.Value;
                        });

                        this.Invoke((MethodInvoker)delegate {
                            maxDragVal = frameTrackBar.MaxDragVal;
                        });

                        Stopwatch stopwatch = null;

                        while (currentFrame < maxDragVal && playStatus)
                        {
                            stopwatch = Stopwatch.StartNew();
                            Thread.Sleep(5);

                            if (currentFrame + 1 <= frameTrackBar.MaxDragVal)
                            {
                                this.Invoke((MethodInvoker)delegate {
                                    frameTrackBar.Value = currentFrame + 1;
                                });

                                currentFrame += 1;

                            } else
                            {
                                break;
                            }

                            stopwatch.Stop();
                            Console.WriteLine("Time = " + stopwatch.ElapsedMilliseconds);

                        }

                        // If after running the previous piece of code, the frameTrackBar is at the end of playing
                        if (currentFrame == maxDragVal && playStatus)
                        {
                            this.Invoke((MethodInvoker)delegate {
                                handlePause();
                            });
                        }
                    } catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
            }
        }

        private void handlePause()
        {
            if (playStatus)
            {
                playBtn.ImageIndex = 2;
                playStatus = !playStatus;
            }
        }

        /// <summary>
        /// Dirty clean
        /// </summary>
        private void runGCForImage()
        {
            goToFrameCount++;
            if (goToFrameCount == GARBAGE_COLLECT_BITMAP_COUNT)
            {
                System.GC.Collect();
                goToFrameCount = 0;
            }
        }

    }
}
