﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic.PowerPacks;

namespace Annotator
{
    public partial class ObjectAnnotation : UserControl
    {
        private const int TOOLTIP_TIME = 3000;
        int minLeftPosition;
        int maxLeftPosition;
        private double frameStepX;
        private Main main;
        public Object o { get; }
        private int borderWidth = 5;
        private int sessionLength;

        /// <summary>
        /// Start of frame to draw annotation
        /// With assumption that annotator could zoom in and zoom out one interval
        /// of video, 
        /// </summary>
        public int start { get; private set; }

        /// <summary>
        /// End of frame to draw annotation
        /// </summary>
        public int end { get; private set; }

        public ObjectAnnotation(Object o, Main main, int start, int end)
        {
            InitializeComponent();

            axis.X1 = 10;
            axis.X2 = 820;

            tt.AutoPopDelay = 5000;
            tt.InitialDelay = 1000;
            tt.ReshowDelay = 500;
            tt.ShowAlways = true;

            this.main = main;
            this.o = o;

            this.start = start;
            this.end = end;
            this.sessionLength = end - start + 1;

            this.updateInfo();
            Rendering();

            if (this.o.genType == Object.GenType.MANUAL)
            {
                this.generate3d.Visible = true;
            }
            else
            {
                this.generate3d.Visible = false;
            }
        }

        public void updateInfo()
        {
            this.info.Text = "Id=" + o.id + "; Name=" + o.name;
        }

        private void Rendering()
        {
            minLeftPosition = axis.X1;
            maxLeftPosition = axis.X2;

            frameStepX = (double)(maxLeftPosition - minLeftPosition) / (sessionLength - 1);

            drawObjectMarks();
        }

        public void resetStartEnd(int start, int end)
        {
            this.start = start;
            this.end = end;

            this.sessionLength = end - start + 1;

            frameStepX = (double)(maxLeftPosition - minLeftPosition) / (sessionLength - 1);

            drawObjectMarks();
        }

        public void drawObjectMarks()
        {
            try
            {
                this.shapeContainer1.Shapes.Clear();
                int spanStart = start;
                int spanEnd = end;
                bool finishOneRectangle = true; // Has just finish drawing one rectangle, or haven't started drawing any rectangle

                foreach (var entry in o.objectMarks)
                {
                    int frameNo = entry.Key;
                    LocationMark objectMark = entry.Value;

                    if (Options.getOption().showMarkerMode)
                    {
                        RectangleShapeWithFrame mark = new RectangleShapeWithFrame(frameNo);
                        mark.FillColor = System.Drawing.Color.Black;
                        mark.BackColor = System.Drawing.Color.Black;
                        mark.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
                        mark.Location = new System.Drawing.Point((int)(minLeftPosition + frameStepX * (objectMark.frameNo - start)) - borderWidth, 4);
                        mark.Size = new System.Drawing.Size(borderWidth, 20);

                        this.shapeContainer1.Shapes.Add(mark);
                        mark.MouseEnter += Mark_MouseEnter;
                        mark.MouseClick += Mark_MouseClick;
                        mark.Click += Mark_Click;
                    }
                       

                    if (objectMark.GetType() != typeof(DeleteLocationMark) && finishOneRectangle)
                    {
                        spanStart = objectMark.frameNo;
                        finishOneRectangle = false;
                    }

                    if (objectMark.GetType() == typeof(DeleteLocationMark))
                    {
                        spanEnd = objectMark.frameNo;
                        drawLifeSpan(spanStart, spanEnd);
                        finishOneRectangle = true;
                    }
                }

                if (Options.getOption().showMarkerMode)
                {
                    if (o.session != null)
                        foreach (var entry in o.session.queryLinkMarks(o))
                        {
                            int frameNo = entry.Key;
                            LinkMark objectMark = entry.Value;

                            RectangleShapeWithFrame mark = new RectangleShapeWithFrame(frameNo);
                            mark.BorderColor = System.Drawing.Color.Green;
                            mark.FillColor = System.Drawing.Color.Green;
                            mark.BackColor = System.Drawing.Color.Green;
                            mark.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
                            mark.Location = new System.Drawing.Point((int)(minLeftPosition + frameStepX * (objectMark.frameNo - start)) - borderWidth, 4);
                            mark.Size = new System.Drawing.Size(borderWidth, 20);
                            mark.MouseEnter += Mark_MouseEnter;
                            mark.MouseClick += Mark_MouseClick;
                            mark.Click += Mark_Click;
                            this.shapeContainer1.Shapes.Add(mark);
                        }
                }

                if (!finishOneRectangle)
                {
                    drawLifeSpan(spanStart, end);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void Mark_Click(object sender, EventArgs e)
        {
            var mark = (RectangleShapeWithFrame)sender;
            ShowToolTipMouseAt(mark.frameNo);

            main.selectObject(this.o);
            main.setTrackbarLocation(mark.frameNo);
        }

        private void Mark_MouseClick(object sender, MouseEventArgs e)
        {
            var mark = (RectangleShapeWithFrame)sender;
            ShowToolTipMouseAt(mark.frameNo);
        }

        private void Mark_MouseEnter(object sender, EventArgs e)
        {
            var mark = (RectangleShapeWithFrame)sender;
            ShowToolTipMouseAt(mark.frameNo);
        }

        private void drawLifeSpan(int startSpan, int endSpan)
        {
            RectangleShape lifespan = new RectangleShape();
            lifespan.BackColor = System.Drawing.Color.Yellow;
            lifespan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            lifespan.FillGradientColor = System.Drawing.Color.Yellow;
            lifespan.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Percent80;
            lifespan.Location = new System.Drawing.Point((int)(minLeftPosition + frameStepX * (startSpan - start)), 8);
            lifespan.Size = new System.Drawing.Size((int)(frameStepX * (endSpan - startSpan)), 12);
            this.shapeContainer1.Shapes.Add(lifespan);
        }

        private void select_Click(object sender, EventArgs e)
        {
            main.selectObject(o);
        }

        internal void selectDeco()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        internal void deselectDeco()
        {
            this.BorderStyle = BorderStyle.None;
        }

        private void generate3d_Click(object sender, EventArgs e)
        {
            main.generate3D(o);
        }

        private void remove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove this object?",
                "Remove",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                main.removeObject(o);
            }
        }

        ToolTip tt = new ToolTip();

        // Tuple of frame and time showing a tooltip
        Tuple<int, long> currentToolTipShow = null;

        private void ShowToolTipMouseAt(int frameNo)
        {
            if (o.session == null) return;
            var linkMarks = o.session.queryLinkMarks(o);

            if (linkMarks.ContainsKey(frameNo))
            {
                LinkMark objectMark = linkMarks[frameNo];

                int X1 = (int)(minLeftPosition + frameStepX * (objectMark.frameNo - 1));
                long ms = (long)((DateTime.Now - DateTime.MinValue).TotalMilliseconds);
                if (currentToolTipShow != null && currentToolTipShow.Item1 == frameNo && ms - currentToolTipShow.Item2 < TOOLTIP_TIME)
                {
                    // Don't display tooltip
                }
                else
                {
                    currentToolTipShow = new Tuple<int, long>(frameNo, ms);
                    tt.Show("Frame " + frameNo + "\n" + objectMark.ToString(), this, new Point(X1, 14), TOOLTIP_TIME);
                }
            }
            else if (o.objectMarks.ContainsKey(frameNo))
            {
                LocationMark objectMark = o.objectMarks[frameNo];

                int X1 = (int)(minLeftPosition + frameStepX * (objectMark.frameNo - 1));
                long ms = (long)((DateTime.Now - DateTime.MinValue).TotalMilliseconds);
                if (currentToolTipShow != null && currentToolTipShow.Item1 == frameNo && ms - currentToolTipShow.Item2 < TOOLTIP_TIME)
                {
                    // Don't display tooltip
                }
                else
                {
                    currentToolTipShow = new Tuple<int, long>(frameNo, ms);
                    tt.Show("Frame " + frameNo, this, new Point(X1, 14), TOOLTIP_TIME);
                }
            }
        }

        private void ObjectAnnotation_SizeChanged(object sender, EventArgs e)
        {
            axis.X2 = this.Size.Width - 10;
            Rendering();
        }
    }
}
