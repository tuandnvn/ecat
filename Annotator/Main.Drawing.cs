using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {
        int boxSize = 5;

        private Color boundingColor = Color.Red;//bounding box color(red as default)
        private int boundingBorder = 1;         //bounding box border size

        // Rectangle new drawing
        private Point? startPoint;               //start point for selection in frame from selected video
        private Point? endPoint;                 //end point for selection in frame from selected video
        private Rectangle boundingBox;             //current mouse selection in video frame
        private bool drawingNewRectangle = false; //flag variable for pictureBox1 drawing selection rectangle
        private bool draggingSelectBoxes = false;
        private int draggingSelectBoxIndex;

        // Polygon new drawing
        private List<Point> polygonPoints = new List<Point>();
        private bool drawingNewPolygon = false;
        private bool editingPolygon = false;            // (drawingNewPolygon, editingPolygon) = ( true, false ) when you're keep drawing the polygon;
                                                        // = (false, true)  when you're done drawing, editing the location of the newly created polygon

        private Point? temporaryPoint; // When the mouse is moving, temporary point hold the current location of cursor, to suggest the shape of the polygon

        Rectangle[] selectBoxes;

        // Editing bounding at a certain frame
        private bool editingAtAFrame = false;

        object selectedObjectLock = new object();

        protected void InitDrawingComponent()
        {
            drawingButtonGroup.Add(cursorDrawing);
            drawingButtonGroup.Add(rectangleDrawing);
            drawingButtonGroup.Add(polygonDrawing);

            drawingButtonSelected[cursorDrawing] = drawingButtonSelected[rectangleDrawing] = drawingButtonSelected[polygonDrawing] = false;

            InitializeEditPanel();
        }

        //Start drawing selection rectangle
        private void pictureBoard_MouseDown(object sender, MouseEventArgs e)
        {
            lock (this)
            {
                if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left && currentVideo != null)
                    {
                        // If mouse down click on resize select boxes
                        if (selectBoxes != null && selectBoxes.Length != 0)
                        {
                            for (int i = 0; i < selectBoxes.Length; i++)
                            {
                                Rectangle r = selectBoxes[i];
                                if (r.Contains(e.Location))
                                {
                                    draggingSelectBoxes = true;
                                    draggingSelectBoxIndex = i;
                                    return;
                                }
                            }
                        }

                        //set selection point for a new rectangle
                        {
                            drawingNewRectangle = true;
                            startPoint = endPoint = e.Location;
                        }
                    }
                }

                if (drawingButtonSelected[polygonDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left && currentVideo != null)
                    {
                        if (editingPolygon)
                        {
                            // If mouse down click on resize select boxes
                            if (draggingSelectBoxes)
                            {
                                draggingSelectBoxes = false;
                                invalidatePictureBoard();
                            }
                            else {
                                if (selectBoxes != null && selectBoxes.Length != 0)
                                {
                                    for (int i = 0; i < selectBoxes.Length; i++)
                                    {
                                        Rectangle r = selectBoxes[i];
                                        if (r.Contains(e.Location))
                                        {
                                            draggingSelectBoxes = true;
                                            draggingSelectBoxIndex = i;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            drawingNewPolygon = true;
                            polygonPoints.Add(e.Location);
                        }
                    }
                    if (e.Button == System.Windows.Forms.MouseButtons.Right && currentVideo != null && drawingNewPolygon)
                    {
                        drawingNewPolygon = false;
                        editingPolygon = true;
                        temporaryPoint = null;
                        newObjectContextPanel.Visible = true;
                        List<Rectangle> listOfSelectBox = new List<Rectangle>();
                        foreach (Point p in polygonPoints)
                        {
                            listOfSelectBox.Add(new Rectangle(p.X - (boxSize - 1) / 2, p.Y - (boxSize - 1) / 2, boxSize, boxSize));
                        }
                        selectBoxes = listOfSelectBox.ToArray();
                        invalidatePictureBoard();
                    }
                }


                if (drawingButtonSelected[cursorDrawing])
                {
                    whenCursorButtonAndMouseDown(e);
                }
            }
        }

        private void pictureBoard_MouseMove(object sender, MouseEventArgs e)
        {
            lock (this)
            {
                if (drawingButtonSelected[rectangleDrawing])
                {
                    if (drawingNewRectangle)
                    {
                        endPoint = e.Location;
                        //pictureBox1.Refresh();
                        invalidatePictureBoard();
                        return;
                    }
                }

                if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle))
                {
                    if (draggingSelectBoxes)
                    {
                        switch (draggingSelectBoxIndex)
                        {
                            // The first four case are corners
                            // Use e.Location as 1 corner and use another box center point as the opposite corner 
                            case 0:
                                startPoint = e.Location;
                                endPoint = selectBoxes[3].getCenter();
                                this.Cursor = Cursors.SizeNWSE;
                                break;
                            case 1:
                                startPoint = e.Location;
                                endPoint = selectBoxes[2].getCenter();
                                this.Cursor = Cursors.SizeNESW;
                                break;
                            case 2:
                                startPoint = e.Location;
                                endPoint = selectBoxes[1].getCenter();
                                this.Cursor = Cursors.SizeNESW;
                                break;
                            case 3:
                                startPoint = e.Location;
                                endPoint = selectBoxes[0].getCenter();
                                this.Cursor = Cursors.SizeNWSE;
                                break;
                            case 4:
                                startPoint = new Point(selectBoxes[0].getCenter().X, e.Location.Y);
                                endPoint = selectBoxes[3].getCenter();
                                this.Cursor = Cursors.SizeNS;
                                break;
                            case 5:
                                startPoint = new Point(selectBoxes[1].getCenter().X, e.Location.Y);
                                endPoint = selectBoxes[2].getCenter();
                                this.Cursor = Cursors.SizeNS;
                                break;
                            case 6:
                                startPoint = new Point(e.Location.X, selectBoxes[2].getCenter().Y);
                                endPoint = selectBoxes[3].getCenter();
                                this.Cursor = Cursors.SizeWE;
                                break;
                            case 7:
                                startPoint = new Point(e.Location.X, selectBoxes[3].getCenter().Y);
                                endPoint = selectBoxes[0].getCenter();
                                this.Cursor = Cursors.SizeWE;
                                break;
                            // Center point
                            case 8:
                                startPoint = new Point(e.Location.X - (selectBoxes[2].X - selectBoxes[1].X) / 2, e.Location.Y - (selectBoxes[1].Y - selectBoxes[0].Y) / 2);
                                endPoint = new Point(e.Location.X + (selectBoxes[2].X - selectBoxes[1].X) / 2, e.Location.Y + (selectBoxes[1].Y - selectBoxes[0].Y) / 2);
                                this.Cursor = Cursors.Hand;
                                break;
                        }
                        invalidatePictureBoard();
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                    }
                }


                if (drawingButtonSelected[polygonDrawing])
                {
                    if (drawingNewPolygon)
                    {
                        temporaryPoint = e.Location;
                        invalidatePictureBoard();
                    }
                }

                if (drawingButtonSelected[polygonDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon))
                {
                    if (draggingSelectBoxes)
                    {
                        polygonPoints[draggingSelectBoxIndex] = e.Location;
                        selectBoxes[draggingSelectBoxIndex] = new Rectangle(e.Location.X - (boxSize - 1) / 2, e.Location.Y - (boxSize - 1) / 2, boxSize, boxSize);

                        invalidatePictureBoard();
                    }
                }

            }
        }

        private void whenCursorButtonAndMouseDown(MouseEventArgs e)
        {
            var objectWithScore = new List<Tuple<float, Object>>();
            var linear = getLinearTransform();
            foreach (Object o in currentSession.getObjects())
            {
                object obj = o.getCurrentBounding(frameTrackBar.Value, linear.Item1, linear.Item2);
                if (obj != null)
                {
                    switch (o.borderType)
                    {
                        case Object.BorderType.Rectangle:
                            objectWithScore.Add(new Tuple<float, Object>(Utils.Score((Rectangle)obj, e.Location), o));
                            break;
                        case Object.BorderType.Polygon:
                            objectWithScore.Add(new Tuple<float, Object>(Utils.Score((List<Point>)obj, e.Location), o));
                            break;
                        case Object.BorderType.Rig:
                            objectWithScore.Add(new Tuple<float, Object>(Utils.Score((RigFigure<Point>)obj, e.Location), o));
                            break;
                    }
                }
            }

            Console.WriteLine(string.Join(",", objectWithScore.Select(x => x.Item1).ToArray()));
            objectWithScore.Sort(delegate (Tuple<float, Object> p1, Tuple<float, Object> p2)
            {
                if (p1.Item1 < p2.Item1) return -1;
                if (p1.Item1 > p2.Item1) return 1;
                return 0;
            }
            );

            double bestScore = objectWithScore.Last().Item1;
            if (bestScore > 0)
            {
                Console.WriteLine("bestScore " + bestScore);
                Object o = objectWithScore.Last().Item2;

                currentSession.selectObject(o);
            }
        }

        private void pictureBoard_MouseUp(object sender, MouseEventArgs e)
        {
            lock (this)
            {
                if (drawingButtonSelected[rectangleDrawing])
                {
                    if (drawingNewRectangle)
                    {
                        endPoint = e.Location;
                        if (drawingNewRectangle && boundingBox != null && boundingBox.Width > 0 && boundingBox.Height > 0)
                            newObjectContextPanel.Visible = true;
                        else
                            newObjectContextPanel.Visible = false;
                        drawingNewRectangle = false;
                    }
                }

                if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle))
                {
                    if (draggingSelectBoxes)
                    {
                        draggingSelectBoxes = false;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //if (capture == null && currentVideo != null)
            //{
            //    capture = new Capture(currentVideo.fileName);
            //}

            //if (capture != null)
            //{
            //    capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, frameTrackBar.Value - 1);
            //    pictureBoard.Image = capture.QueryFrame().Bitmap;
            //    runGCForImage();
            //}
        }


        private void pictureBoard_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("pictureBoard_Paint");
            // Has been drawn before
            if (currentVideo != null)
            {
                var linear = getLinearTransform();

                foreach (Object o in currentSession.getObjects())
                {
                    if (o != selectedObject || o.genType != Object.GenType.MANUAL)
                    {
                        Pen p = new Pen(o.color, o.borderSize);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        object r = o.getCurrentBounding(frameTrackBar.Value, linear.Item1, linear.Item2);
                        if (r != null)
                        {

                            switch (o.borderType)
                            {
                                case Object.BorderType.Rectangle:
                                    e.Graphics.DrawRectangle(p, (Rectangle)r);
                                    break;
                                case Object.BorderType.Polygon:
                                    e.Graphics.DrawPolygon(p, ((List<Point>)r).ToArray());
                                    break;
                                case Object.BorderType.Rig:
                                    e.Graphics.DrawRig(p, (RigFigure<Point>)r);
                                    break;
                            }
                        }
                    }
                }


                if (selectedObject != null && !editingAtAFrame)
                {
                    lock (selectedObjectLock)
                    {
                        object obj = selectedObject.getCurrentBounding(frameTrackBar.Value, linear.Item1, linear.Item2);
                        if (obj != null)
                        {
                            switch (selectedObject.borderType)
                            {
                                case Object.BorderType.Rectangle:
                                    boundingBox = (Rectangle)obj;
                                    startPoint = new Point(boundingBox.X, boundingBox.Y);
                                    endPoint = new Point(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height);
                                    break;
                                case Object.BorderType.Polygon:
                                    polygonPoints = (List<Point>)obj;
                                    List<Rectangle> listOfSelectBox = new List<Rectangle>();
                                    foreach (Point p in polygonPoints)
                                    {
                                        listOfSelectBox.Add(new Rectangle(p.X - (boxSize - 1) / 2, p.Y - (boxSize - 1) / 2, boxSize, boxSize));
                                    }
                                    selectBoxes = listOfSelectBox.ToArray();
                                    break;
                            }
                        }
                        else
                        {
                            switch (selectedObject.borderType)
                            {
                                case Object.BorderType.Rectangle:
                                    startPoint = null;
                                    endPoint = null;
                                    selectBoxes = new Rectangle[0] { };
                                    break;
                                case Object.BorderType.Polygon:
                                    polygonPoints = null;
                                    selectBoxes = new Rectangle[0] { };
                                    break;
                            }
                        }
                    }
                }

                Pen pen = null;
                if (selectedObject == null)
                {
                    pen = new Pen(boundingColor, (float)boundingBorder);
                }
                else
                {
                    pen = new Pen(selectedObject.color, selectedObject.borderSize);
                }
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                // Currently drawing or selecting a rectangle object
                if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle))
                {
                    lock (selectedObjectLock)
                    {
                        if (endPoint.HasValue && startPoint.HasValue && currentVideo != null)
                        {
                            int lowerX = Math.Min(startPoint.Value.X, endPoint.Value.X);
                            int lowerY = Math.Min(startPoint.Value.Y, endPoint.Value.Y);
                            int higherX = Math.Max(startPoint.Value.X, endPoint.Value.X);
                            int higherY = Math.Max(startPoint.Value.Y, endPoint.Value.Y);

                            boundingBox = new Rectangle(lowerX, lowerY, higherX - lowerX, higherY - lowerY);
                            selectBoxes = boundingBox.getCornerSelectBoxes(boxSize);

                            e.Graphics.DrawRectangle(pen, boundingBox);

                            foreach (Rectangle r in selectBoxes)
                            {
                                e.Graphics.DrawRectangle(new Pen(Color.Black), r);
                                e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                            }
                            e.Graphics.Save();
                        }
                    }
                }

                // Selecting a rig object
                if (selectedObject != null && selectedObject.borderType == Object.BorderType.Rig)
                {
                    lock (selectedObjectLock)
                    {
                        var rigFigure = (RigFigure<Point>) selectedObject.getCurrentBounding(frameTrackBar.Value, linear.Item1, linear.Item2);

                        selectBoxes = rigFigure.getCornerSelectBoxes(boxSize);

                        e.Graphics.DrawRectangle(pen, boundingBox);

                        foreach (Rectangle r in selectBoxes)
                        {
                            e.Graphics.DrawRectangle(new Pen(Color.Black), r);
                            e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                        }
                        e.Graphics.Save();
                    }
                }

                // Currently drawing or selecting a polygon object
                if (drawingButtonSelected[polygonDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon))
                {
                    lock (selectedObjectLock)
                    {
                        if (drawingNewPolygon)
                        {
                            if (temporaryPoint.HasValue && polygonPoints.Count != 0)
                            {
                                polygonPoints.Add(temporaryPoint.Value);
                                e.Graphics.DrawPolygon(pen, ((List<Point>)polygonPoints).ToArray());
                                polygonPoints.Remove(temporaryPoint.Value);
                            }
                        }
                        else
                        {
                            if (polygonPoints.Count != 0)
                            {
                                e.Graphics.DrawPolygon(pen, ((List<Point>)polygonPoints).ToArray());
                            }

                            for (int index = 0; index < selectBoxes.Count(); index++)
                            {
                                Rectangle r = selectBoxes[index];
                                e.Graphics.DrawRectangle(new Pen(Color.Black), r);
                                if (draggingSelectBoxes && index == draggingSelectBoxIndex)
                                {
                                    e.Graphics.FillRectangle(new SolidBrush(Color.Turquoise), r);
                                }
                                else { e.Graphics.FillRectangle(new SolidBrush(Color.White), r); }
                            }
                        }
                    }
                }
            }
        }

        internal void selectObject(Object o)
        {
            lock (selectedObjectLock)
            {
                this.selectedObject = o;

                if (selectedObject != null)
                {
                    selectObjContextPanel.Visible = true;
                }

                foreach (Button b in drawingButtonGroup)
                {
                    selectButtonDrawing(b, drawingButtonGroup, false);
                }

                this.showInformation(o);
            }
            invalidatePictureBoard();
        }


        private void cancelObjectBtn_Click(object sender, EventArgs e)
        {
            cancelDrawing();
        }

        private void cancelDrawing()
        {
            newObjectContextPanel.Visible = false;
            if (drawingButtonSelected[rectangleDrawing])
            {
                startPoint = null;
                endPoint = null;
                drawingNewRectangle = false;
            }

            if (drawingButtonSelected[polygonDrawing])
            {
                polygonPoints = new List<Point>();
                drawingNewPolygon = editingPolygon = false;
            }
            draggingSelectBoxes = false;
            selectBoxes = new Rectangle[] { };
            invalidatePictureBoard();
        }

        //Choose bounding box color
        private void chooseColorBtn_Click(object sender, EventArgs e)
        {
            // Show the color dialog.
            DialogResult result = colorDialog1.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                boundingColor = colorDialog1.Color;
                invalidatePictureBoard();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            boundingBorder = (int)numericUpDown1.Value;
            invalidatePictureBoard();
        }

        //Add object to the video
        private void addObjBtn_Click(object sender, EventArgs e)
        {
            var linear = getLinearTransform();
            Object objectToAdd = null;
            if (drawingButtonSelected[rectangleDrawing])
            {
                objectToAdd = new Object(null, colorDialog1.Color, (int)numericUpDown1.Value, currentVideo.getFileName(), linear.Item1, linear.Item2, frameTrackBar.Value, boundingBox);
                startPoint = null;
                endPoint = null;
            }

            if (drawingButtonSelected[polygonDrawing])
            {
                objectToAdd = new Object(null, colorDialog1.Color, (int)numericUpDown1.Value, currentVideo.getFileName(), linear.Item1, linear.Item2, frameTrackBar.Value, polygonPoints);
                polygonPoints = new List<Point>();
            }

            if (objectToAdd != null)
            {
                currentSession.addObject(objectToAdd);
                newObjectContextPanel.Visible = false;
                selectBoxes = new Rectangle[] { };
                invalidatePictureBoard();
            }

            clearMiddleCenterPanel();
            populateMiddleCenterPanel();
        }

        private Tuple<double, Point> getLinearTransform()
        {
            double scale = Math.Min(pictureBoard.Width / currentVideo.frameWidth, pictureBoard.Height / currentVideo.frameHeight);
            Point translation = new Point((int)(pictureBoard.Width - currentVideo.frameWidth * scale) / 2, (int)(pictureBoard.Height - currentVideo.frameHeight * scale) / 2);
            return new Tuple<double, Point>(scale, translation);
        }

        private void cursorDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(cursorDrawing, drawingButtonGroup, !drawingButtonSelected[cursorDrawing]);
            cancelSelectObject();
        }

        private void rectangleDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(rectangleDrawing, drawingButtonGroup, !drawingButtonSelected[rectangleDrawing]);
        }

        private void polygonDrawing_MouseDown(object sender, MouseEventArgs e)
        {

            selectButtonDrawing(polygonDrawing, drawingButtonGroup, !drawingButtonSelected[polygonDrawing]);
        }

        private void selectButtonDrawing(Button b, List<Button> buttonGroup, bool select)
        {
            drawingButtonSelected[b] = select;

            if (select)
            {
                foreach (Button b2 in buttonGroup)
                {
                    if (b2 != b)
                    {
                        selectButtonDrawing(b2, buttonGroup, false);
                    }
                }
                b.BackColor = Color.White;
                b.FlatAppearance.BorderColor = Color.Silver;
            }
            else
            {
                b.BackColor = Color.Transparent;
                b.FlatAppearance.BorderColor = Color.White;
            }
        }

        internal void removeObject(Object o)
        {
            selectedObject = null;
            newObjectContextPanel.Visible = false;
            selectObjContextPanel.Visible = false;
            this.clearInformation();
            selectBoxes = new Rectangle[] { };
            invalidatePictureBoard();
        }

        private void invalidatePictureBoard()
        {
            Console.WriteLine("invalidatePictureBoard");
            pictureBoard.Invalidate();
        }
    }
}
