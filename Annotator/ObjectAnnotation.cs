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
using Microsoft.VisualBasic.PowerPacks;

namespace Annotator
{
    public partial class ObjectAnnotation : UserControl
    {
        private const int TOOLTIP_TIME = 3000;
        int minLeftPosition;
        int maxLeftPosition;
        private double frameStepX;
        private Main mainGUI;
        private Session session;
        public Object o { get; }
        int borderWidth = 2;

        public ObjectAnnotation(Object o, Session session)
        {
            InitializeComponent();

            tt.AutoPopDelay = 5000;
            tt.InitialDelay = 1000;
            tt.ReshowDelay = 500;
            tt.ShowAlways = true;

            this.mainGUI = session.mainGUI;
            this.session = session;
            minLeftPosition = axis.X1;
            maxLeftPosition = axis.X2;

            this.o = o;

            frameStepX = (double)(maxLeftPosition - minLeftPosition) / (session.sessionLength - 1);

            drawObjectMarks();

            this.info.Text = "Id=" + o.id + "; Name=" + o.name;
        }

        public void drawObjectMarks()
        {
            this.shapeContainer1.Shapes.Clear();
            int start = 1;
            int end = this.session.sessionLength;
            bool finishOneRectangle = true; // Has just finish drawing one rectangle, or haven't started drawing any rectangle

            foreach (var entry in o.objectMarks)
            {
                int frameNo = entry.Key;
                Object.LocationMark objectMark = entry.Value;

                RectangleShapeWithFrame mark = new RectangleShapeWithFrame(frameNo);
                mark.FillColor = System.Drawing.Color.Black;
                mark.BackColor = System.Drawing.Color.Black;
                mark.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
                mark.Location = new System.Drawing.Point((int)(minLeftPosition + frameStepX * (objectMark.frameNo - 1)) - borderWidth, 4);
                mark.Size = new System.Drawing.Size( borderWidth, 20);

                this.shapeContainer1.Shapes.Add(mark);
                mark.MouseEnter += Mark_MouseEnter;
                mark.MouseClick += Mark_MouseClick;
                mark.Click += Mark_Click;

                if (objectMark.markType == Object.LocationMark.LocationMarkType.Location && finishOneRectangle)
                {
                    start = objectMark.frameNo;
                    finishOneRectangle = false;
                }

                if (objectMark.markType == Object.LocationMark.LocationMarkType.Delete)
                {
                    end = objectMark.frameNo;
                    drawLifeSpan(start, end);
                    finishOneRectangle = true;
                }
            }

            foreach (var entry in o.spatialLinkMarks)
            {
                int frameNo = entry.Key;
                Object.SpatialLinkMark objectMark = entry.Value;

                RectangleShapeWithFrame mark = new RectangleShapeWithFrame(frameNo);
                mark.BorderColor = System.Drawing.Color.Green;
                mark.FillColor = System.Drawing.Color.Green;
                mark.BackColor = System.Drawing.Color.Green;
                mark.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
                mark.Location = new System.Drawing.Point((int)(minLeftPosition + frameStepX * (objectMark.frameNo - 1)) - borderWidth, 4);
                mark.Size = new System.Drawing.Size(borderWidth, 20);
                mark.MouseEnter += Mark_MouseEnter;
                mark.MouseClick += Mark_MouseClick;
                mark.Click += Mark_Click;
                this.shapeContainer1.Shapes.Add(mark);
            }

            if (!finishOneRectangle)
            {
                drawLifeSpan(start, this.session.sessionLength);
            }
        }

        private void Mark_Click(object sender, EventArgs e)
        {
            var mark = (RectangleShapeWithFrame)sender;
            ShowToolTipMouseAt(mark.frameNo);
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

        private void drawLifeSpan(int start, int end)
        {
            RectangleShape lifespan = new RectangleShape();
            lifespan.BackColor = System.Drawing.Color.Yellow;
            lifespan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            lifespan.FillGradientColor = System.Drawing.Color.Yellow;
            lifespan.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Percent80;
            lifespan.Location = new System.Drawing.Point((int)(minLeftPosition + frameStepX * (start - 1)), 8);
            lifespan.Size = new System.Drawing.Size((int)(frameStepX * (end - start)), 12);
            this.shapeContainer1.Shapes.Add(lifespan);
        }

        private void select_Click(object sender, EventArgs e)
        {
            session.selectObject(o);
            selectDeco();
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
            session.generate3dforObject(o);
        }

        private void remove_Click(object sender, EventArgs e)
        {
            session.removeObject(o);
        }

        ToolTip tt = new ToolTip();

        // Tuple of frame and time showing a tooltip
        Tuple<int, int> currentToolTipShow = null;

        private void ShowToolTipMouseAt(int frameNo)
        {
            if (o.spatialLinkMarks.ContainsKey(frameNo))
            {
                Object.SpatialLinkMark objectMark = o.spatialLinkMarks[frameNo];

                int X1 = (int)(minLeftPosition + frameStepX * (objectMark.frameNo - 1));
                int ms = (int)((DateTime.Now - DateTime.MinValue).TotalMilliseconds);
                if (currentToolTipShow != null && currentToolTipShow.Item1 == frameNo && currentToolTipShow.Item2 - ms < TOOLTIP_TIME)
                {
                    // Don't display tooltip
                }
                else
                {
                    currentToolTipShow = new Tuple<int, int>(frameNo, ms);
                    tt.Show("Frame " + frameNo + "\n" + objectMark.ToString(), this, new Point(X1, 14), TOOLTIP_TIME);
                }
            }
            else if (o.objectMarks.ContainsKey(frameNo))
            {
                Object.LocationMark objectMark = o.objectMarks[frameNo];

                int X1 = (int)(minLeftPosition + frameStepX * (objectMark.frameNo - 1));
                int ms = (int)((DateTime.Now - DateTime.MinValue).TotalMilliseconds);
                if (currentToolTipShow != null && currentToolTipShow.Item1 == frameNo && currentToolTipShow.Item2 - ms < TOOLTIP_TIME)
                {
                    // Don't display tooltip
                }
                else
                {
                    currentToolTipShow = new Tuple<int, int>(frameNo, ms);
                    tt.Show("Frame " + frameNo, this, new Point(X1, 14), TOOLTIP_TIME);
                }
            }
        }
    }
}
