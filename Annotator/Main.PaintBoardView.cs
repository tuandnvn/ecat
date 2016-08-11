using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void clearPlaybackFileComboBox()
        {
            playbackFileComboBox.Items.Clear();
            playbackFileComboBox.Enabled = false;
            frameTrackBar.Enabled = false;
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
    }
}
