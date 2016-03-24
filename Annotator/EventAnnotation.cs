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
    public partial class EventAnnotation : UserControl
    {

        int minLeftPosition;
        int maxLeftPosition;
        private bool selected = false;

        private int start;   //minimum value for left slider
        private int end;   //maximum value for right slider
        private bool slider1Move; //true if slider1 can change position
        private bool slider2Move; //true if slider1 can change position
        private double frameStepX;

        Brush rBrush = new SolidBrush(Color.FromArgb(128, Color.Yellow));
        private Main mainGUI;

        private int rectangleYPos = 8;
        private int rectangleSize = 12;

        Event ev;

        public EventAnnotation(Event ev, Main mainGUI, Session session)
        {

            InitializeComponent();

            this.ev = ev;
            minLeftPosition = axis.X1;
            maxLeftPosition = axis.X2;

            //MessageBox.Show(txt);
            if (ev.text != null)
                textAnnotation.Text = ev.text;
            this.mainGUI = mainGUI;
            this.start = ev.startFrame;
            this.end = ev.endFrame;
            this.slider1Move = false;
            this.slider2Move = false;

            frameStepX = (double)(maxLeftPosition - minLeftPosition) / (session.sessionLength - 1);

            leftMarker.X1 = leftMarker.X2 = (int)(minLeftPosition + frameStepX * (start - 1));
            rightMarker.X1 = rightMarker.X2 = (int)(minLeftPosition + frameStepX * (end - 1));
            this.rectangleShape.Bounds = new Rectangle(leftMarker.X1 + (leftMarker.BorderWidth - 1), rectangleYPos,
                rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);

            rectangleShape.MouseClick += Mark_MouseClick;

            //MessageBox.Show("minimum = " + minimum + ", maximum = " + maximum + " stepX = " + frameStepX);
            intervalLbl.Text = "Start: " + start + ", Stop: " + end;
        }

        //Get annotation text
        public String getText()
        {
            return textAnnotation.Text;
        }

        //Get minimum
        public int getMinimum()
        {
            return start;
        }
        //Get maximum
        public int getMaximum()
        {
            return end;
        }

        public bool getSelected()
        {
            return selected;
        }
        public void setSelected(bool option)
        {
            this.selected = option;
        }

        private void Annotation_MouseMove(object sender, MouseEventArgs e)
        {
            if (slider1Move)
            {
                int newX = (e.Location.X < minLeftPosition) ? minLeftPosition : (e.Location.X > maxLeftPosition) ? maxLeftPosition : e.Location.X;

                if (newX < rightMarker.X1)
                {
                    leftMarker.X1 = newX;
                    leftMarker.X2 = newX;
                    this.rectangleShape.Location = new Point(leftMarker.X1 + leftMarker.BorderWidth - 1, rectangleYPos);
                    this.rectangleShape.Size = new Size(rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);
                    this.mainGUI.setTrackbarLocation((int)((newX - minLeftPosition) / frameStepX) + 1);
                }
            }
            if (slider2Move)
            {
                int newX = (e.Location.X < minLeftPosition) ? minLeftPosition : (e.Location.X > maxLeftPosition) ? maxLeftPosition : e.Location.X;

                if (leftMarker.X1 < newX)
                {
                    rightMarker.X1 = newX;
                    rightMarker.X2 = newX;
                    this.rectangleShape.Size = new Size(rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);
                    this.mainGUI.setTrackbarLocation((int)((newX - minLeftPosition) / frameStepX) + 1);
                }
            }
        }

        private void Annotation_MouseUp(object sender, MouseEventArgs e)
        {
            slider1Move = false;
            slider2Move = false;
        }

        private void Annotation_MouseDown(object sender, MouseEventArgs e)
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
            ev.startFrame = (int)((leftMarker.X1 - minLeftPosition) / frameStepX) + 1;
            intervalLbl.Text = "Start: " + ev.startFrame + ", Stop: " + ev.endFrame;
            mainGUI.Invalidate();
        }

        private void lineShape3_Move(object sender, EventArgs e)
        {
            ev.endFrame = (int)((rightMarker.X1 - minLeftPosition) / frameStepX) + 1;
            intervalLbl.Text = "Start: " + ev.startFrame + ", Stop: " + ev.endFrame;
            mainGUI.Invalidate();
        }

        private void lineShape1_Paint(object sender, PaintEventArgs e)
        {
            axis.BringToFront();
        }

        private void Mark_MouseClick(object sender, MouseEventArgs e)
        {
            ShowToolTipMouseAt(e.Location);
        }

        ToolTip tt = new ToolTip();
        int TOOLTIP_TIME = 5000;

        private void ShowToolTipMouseAt(Point location)
        {
            string tooltip = string.Join("\n", this.ev.linkToEvents.Select( t => t.Item1 + "( " + this.ev.id + ", " + t.Item2 +  " )" ));
            tt.Show(tooltip, this, location, TOOLTIP_TIME);
        }

        private void textAnnotation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textAnnotation.ReadOnly = true;
                ev.text = textAnnotation.Text;
                selectAnnotation();
                //Set middle_bottom panel:
                /*
                frm1.setStartFrameLabel(startFrame + "");
                frm1.setEndFrameLabel(endFrame + "");
                frm1.setTextBox1Text(textBox1.Text);*/
            }
        }

        private void textAnnotation_DoubleClick(object sender, EventArgs e)
        {
            textAnnotation.ReadOnly = false;
        }


        private void selectBtn_Click(object sender, EventArgs e)
        {
            selectAnnotation();
        }

        private void selectAnnotation()
        {
            mainGUI.setAnnotationText(textAnnotation.Text);
            mainGUI.unselectAnnotations();
            mainGUI.selectedEvent = this.ev;
            setSelected(true);
            selectDeco();

            foreach (Event.Reference reference in ev.references)
            {
                int start = reference.start;
                int end = reference.end;
                String refID = reference.refObjectId;
                String text = this.getText().Substring(start, end - start);
                mainGUI.addRightBottomTableReference(start, end, text, refID);
            }

            foreach (Event.Action ev in ev.actions)
            {
                int start = ev.start;
                int end = ev.end;
                String semanticType = ev.semanticType;
                String text = this.getText().Substring(start, end - start);
                mainGUI.addRightBottomTableReference(start, end, text, semanticType);
            }
        }

        internal void selectDeco()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        internal void deselectDeco()
        {
            this.BorderStyle = BorderStyle.None;
        }

        private void remove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove this annotation?",
                "Remove",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                mainGUI.removeAnnotation(this.ev);
            }
        }


        private void subEventLink_Click(object sender, EventArgs e)
        {
            mainGUI.linkSubEvent(this.ev);
        }
    }
}
