using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Annotator
{
    partial class Range : UserControl
    {

        int minLeftPosition;
        int maxLeftPosition;
        private bool selected = false;

        private int _start;
        private int _end;

        internal int start { get { return _start; } set { _start = value; recalFrameStep(); intervalLbl.Text = "Start: " + start + ", Stop: " + end; } }  //minimum value for left slider
        internal int end { get { return _end; } set {_end = value; recalFrameStep(); intervalLbl.Text = "Start: " + start + ", Stop: " + end; } }  //maximum value for right slider

        private bool slider1Move; //true if slider1 can change position
        private bool slider2Move; //true if slider1 can change position
        private double frameStepX;

        Brush rBrush = new SolidBrush(Color.FromArgb(128, Color.Yellow));
        internal RecordPanel recordPanel { get; set; }

        private int rectangleYPos = 8;
        private int rectangleSize = 12;

        private int _axisRight;
        private int _axisLeft;
        public int axisRight { get { return _axisRight; } set { this._axisRight = value; ResetLeftRight(); } }
        public int axisLeft { get { return _axisLeft; } set { this._axisLeft = value; ResetLeftRight(); } }


        internal Range() : this(null, 0)
        {
        }

        internal Range(RecordPanel recordPanel, int sessionLength)
        {
            InitializeComponent();

            minLeftPosition = axis.X1;
            maxLeftPosition = axis.X2;

            this.recordPanel = recordPanel;
            this.start = 1;
            this.end = sessionLength;
            this.slider1Move = false;
            this.slider2Move = false;
            recalFrameStep();

            this.rightMarker.X1 = this.rightMarker.X2 = this.axis.X2;
            this.leftMarker.X1 = this.leftMarker.X2 = this.axis.X1;
            this.rectangleShape.Bounds = new Rectangle(leftMarker.X1 + (leftMarker.BorderWidth - 1), rectangleYPos,
                rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);

            //MessageBox.Show("minimum = " + minimum + ", maximum = " + maximum + " stepX = " + frameStepX);
            intervalLbl.Text = "Start: " + start + ", Stop: " + end;
        }

        private void recalFrameStep()
        {
            frameStepX = (double)(maxLeftPosition - minLeftPosition) / (end - start);
        }

        private void Range_MouseMove(object sender, MouseEventArgs e)
        {
            if (slider1Move)
            {
                int newX = (e.Location.X < minLeftPosition) ? minLeftPosition : (e.Location.X > maxLeftPosition) ? maxLeftPosition : e.Location.X;

                if (newX < rightMarker.X1)
                {
                    //resetLeftWrite(newX, rightMarker.X1);
                    leftMarker.X1 = leftMarker.X2 = newX;
                    this.rectangleShape.Location = new Point(leftMarker.X1 + leftMarker.BorderWidth - 1, rectangleYPos);
                    this.rectangleShape.Size = new Size(rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);
                    this.recordPanel.setTrackbarLocation((int)((newX - minLeftPosition) / frameStepX) + 1);
                }
            }
            if (slider2Move)
            {
                int newX = (e.Location.X < minLeftPosition) ? minLeftPosition : (e.Location.X > maxLeftPosition) ? maxLeftPosition : e.Location.X;

                if (leftMarker.X1 < newX)
                {
                    //resetLeftWrite(leftMarker.X1, newX);
                    rightMarker.X1 = rightMarker.X2 = newX;
                    this.rectangleShape.Size = new Size(rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);
                    this.recordPanel.setTrackbarLocation((int)((newX - minLeftPosition) / frameStepX) + 1);
                }
            }
        }

        private void Range_MouseUp(object sender, MouseEventArgs e)
        {
            slider1Move = false;
            slider2Move = false;
        }

        private void Range_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Location.X >= leftMarker.X1 - leftMarker.BorderWidth && e.Location.X <= leftMarker.X1 + leftMarker.BorderWidth)
            {
                slider1Move = true;
            }
            else
            {
                slider1Move = false;
            }

            if (e.Location.X >= rightMarker.X1 - rightMarker.BorderWidth && e.Location.X <= rightMarker.X1 + rightMarker.BorderWidth)
            {
                slider2Move = true;
            }
            else
            {
                slider2Move = false;
            }

            // Allow only one slider moves at one time
            if (slider1Move && slider2Move)
            {
                slider2Move = false;
            }
        }

        private void lineShape2_Move(object sender, EventArgs e)
        {
            _start = (int)((leftMarker.X1 - minLeftPosition) / frameStepX) + 1;
            intervalLbl.Text = "Start: " + _start + ", Stop: " + _end;
            //recordPanel.Invalidate();
        }

        private void lineShape3_Move(object sender, EventArgs e)
        {
            _end = (int)((rightMarker.X1 - minLeftPosition) / frameStepX) + 1;
            intervalLbl.Text = "Start: " + _start + ", Stop: " + _end;
            //recordPanel.Invalidate();
        }

        private void lineShape1_Paint(object sender, PaintEventArgs e)
        {
            axis.BringToFront();
        }

        protected void ResetLeftRight()
        {
            this.axis.X1 = axisLeft;
            this.axis.X2 = axisRight;

            minLeftPosition = axis.X1;
            maxLeftPosition = axis.X2;
            recalFrameStep();

            resetLeftWrite(this.axis.X1, this.axis.X2);
            Invalidate();
        }

        private void resetLeftWrite(int x, int y)
        {
            this.leftMarker.X1 = this.leftMarker.X2 = x;
            this.rightMarker.X1 = this.rightMarker.X2 = y;

            this.rectangleShape.Location = new Point(leftMarker.X1 + leftMarker.BorderWidth - 1, rectangleYPos);
            this.rectangleShape.Size = new Size(rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);
        }
    }
}
