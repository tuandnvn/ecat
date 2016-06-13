using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {
        int boxSize = 7;

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
        private List<PointF> polygonPoints = new List<PointF>();
        private bool drawingNewPolygon = false;
        private bool editingPolygon = false;            // (drawingNewPolygon, editingPolygon) = ( true, false ) when you're keep drawing the polygon;
                                                        // = (false, true)  when you're done drawing, editing the location of the newly created polygon
                                                        // = (false, false) when you're added the polygon
        private bool addPolygonPoint = false;

        private Point? temporaryPoint; // When the mouse is moving, temporary point hold the current location of cursor, to suggest the shape of the polygon

        List<Rectangle> selectBoxes;
        PointF centroid = new PointF();
        int centroidRadius = 8;

        /// <summary>
        /// If true: 
        /// User holds the centroid point of polygon
        /// to drag it across the image board
        /// </summary>
        private bool draggingCentroid = false;

        /// <summary>
        /// If true: 
        /// User holds the centroid point of polygon while pressing Ctrl 
        /// to rotate the polygon
        /// </summary>
        private bool rotatingCentroid = false;

        private bool zoomingCentroid = false;

        // Editing bounding at a certain frame
        private bool editingAtAFrame = false;

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
            if (drawingButtonSelected[rectangleDrawing] ||
                (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle))
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && currentVideo != null)
                {
                    // If mouse down click on resize select boxes
                    if (selectBoxes != null && selectBoxes.Count != 0)
                    {
                        for (int i = 0; i < selectBoxes.Count; i++)
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

            if (drawingButtonSelected[polygonDrawing] ||   // Drawing mode 
                (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)) // Editing mode
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && currentVideo != null)
                {
                    if (editingPolygon)
                    {
                        if (!draggingSelectBoxes)
                        {
                            if (selectBoxes != null && selectBoxes.Count != 0)
                            {
                                for (int i = 0; i < selectBoxes.Count; i++)
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

                        if (!draggingCentroid && !centroid.Equals(new Point()))
                        {
                            RectangleF r = new RectangleF(centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                            if (r.Contains(e.Location))
                            {
                                draggingCentroid = true;
                                invalidatePictureBoard();
                                return;
                            }
                        }


                        if (addPolygonPoint)
                        {
                            double minDistance = double.MaxValue;
                            int minPos = -1;
                            for (int i = 0; i < polygonPoints.Count; i++)
                            {
                                double distance = Math.Pow(polygonPoints[i].X - e.Location.X, 2) + Math.Pow(polygonPoints[i].Y - e.Location.Y, 2);
                                if (minDistance > distance)
                                {
                                    minDistance = distance;
                                    minPos = i;
                                }
                            }

                            draggingSelectBoxIndex = minPos + 1;
                            polygonPoints.Insert(draggingSelectBoxIndex, e.Location);
                            selectBoxes.Insert(draggingSelectBoxIndex, new Rectangle(e.Location.X - (boxSize - 1) / 2, e.Location.Y - (boxSize - 1) / 2, boxSize, boxSize));

                            // Recalculate centroid
                            calculateCentroid();

                            draggingSelectBoxes = true;
                            addPolygonPoint = false;

                            this.Cursor = Cursors.Default;
                            invalidatePictureBoard();
                            return;
                        }
                    }
                    else
                    {
                        if (drawingButtonSelected[polygonDrawing])
                        {
                            drawingNewPolygon = true;
                            polygonPoints.Add(e.Location);
                        }
                    }
                }

                // Allow edit the polygon just have been drawn
                // Right click after drawing polygon
                if (e.Button == System.Windows.Forms.MouseButtons.Right && currentVideo != null && drawingNewPolygon)
                {
                    drawingNewPolygon = false;
                    editingPolygon = true;
                    temporaryPoint = null;
                    newObjectContextPanel.Visible = true;
                    List<Rectangle> listOfSelectBox = new List<Rectangle>();
                    foreach (PointF p in polygonPoints)
                    {
                        listOfSelectBox.Add(new Rectangle((int)(p.X - (boxSize - 1) / 2), (int)(p.Y - (boxSize - 1) / 2), boxSize, boxSize));
                    }
                    selectBoxes = listOfSelectBox;

                    calculateCentroid();

                    invalidatePictureBoard();
                }
            }


            if (drawingButtonSelected[cursorDrawing])
            {
                whenCursorButtonAndMouseDown(e);
            }

        }


        private void pictureBoard_MouseMove(object sender, MouseEventArgs e)
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

                    calculateCentroid();

                    invalidatePictureBoard();

                    return;
                }

                if (draggingCentroid)
                {
                    PointF oldCentroid = getCentroid(polygonPoints);
                    PointF translation = new PointF(e.Location.X - oldCentroid.X, e.Location.Y - oldCentroid.Y);
                    for ( int i = 0; i < polygonPoints.Count; i ++ )
                    {
                        polygonPoints[i] = new PointF(polygonPoints[i].X + translation.X, polygonPoints[i].Y + translation.Y);
                    }
                    centroid = e.Location;

                    List<Rectangle> listOfSelectBox = new List<Rectangle>();
                    foreach (PointF p in polygonPoints)
                    {
                        listOfSelectBox.Add(new Rectangle((int)(p.X - (boxSize - 1) / 2), (int)(p.Y - (boxSize - 1) / 2), boxSize, boxSize));
                    }
                    selectBoxes = listOfSelectBox;
                    invalidatePictureBoard();
                    return;
                }
            }
        }

        private void whenCursorButtonAndMouseDown(MouseEventArgs e)
        {
            var objectWithScore = new List<Tuple<float, Object>>();
            var linear = getLinearTransform();
            foreach (Object o in currentSession.getObjects())
            {
                DrawableLocationMark lm = o.getScaledLocationMark(frameTrackBar.Value, linear.Item1, linear.Item2);
                if (lm != null)
                {
                    objectWithScore.Add(new Tuple<float, Object>(lm.Score(e.Location), o));
                }
            }

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
                Object o = objectWithScore.Last().Item2;

                selectObject(o);
            }
        }

        private void pictureBoard_MouseUp(object sender, MouseEventArgs e)
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

            if (drawingButtonSelected[polygonDrawing] ||   // Drawing mode 
                (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)) // Editing mode
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && currentVideo != null)
                {
                    if (editingPolygon)
                    {
                        // ----------------------------------
                        // ---- Handle dragging select boxes
                        // If mouse up release selected box
                        if (draggingSelectBoxes)
                        {
                            draggingSelectBoxes = false;
                            invalidatePictureBoard();
                            return;
                        }

                        // ----------------------------------
                        // ---- Handle dragging centroid of polygon
                        // If mouse up release selected box
                        if (draggingCentroid)
                        {
                            draggingCentroid = false;
                            invalidatePictureBoard();
                            return;
                        }
                    }
                }
            }
        }

        private void pictureBoard_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (currentVideo != null && currentSession != null)
                {
                    var linear = getLinearTransform();

                    foreach (Object o in currentSession.getObjects())
                    {
                        if (o != selectedObject || o.genType != Object.GenType.MANUAL)
                        {

                            Pen p = new Pen(o.color, o.borderSize);
                            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            DrawableLocationMark r = o.getScaledLocationMark(frameTrackBar.Value, linear.Item1, linear.Item2);

                            if (r != null)
                            {
                                r.drawOnGraphics(e.Graphics, p);
                            }
                        }
                    }


                    if (selectedObject != null && !editingAtAFrame)
                    {
                        LocationMark lm = selectedObject.getScaledLocationMark(frameTrackBar.Value, linear.Item1, linear.Item2);
                        if (lm != null)
                        {
                            switch (selectedObject.borderType)
                            {
                                case Object.BorderType.Rectangle:
                                    boundingBox = ((RectangleLocationMark)lm).boundingBox;
                                    startPoint = new Point(boundingBox.X, boundingBox.Y);
                                    endPoint = new Point(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height);
                                    break;
                                case Object.BorderType.Polygon:
                                    polygonPoints = ((PolygonLocationMark)lm).boundingPolygon;
                                    List<Rectangle> listOfSelectBox = new List<Rectangle>();
                                    foreach (PointF p in polygonPoints)
                                    {
                                        listOfSelectBox.Add(new Rectangle((int)(p.X - (boxSize - 1) / 2), (int)(p.Y - (boxSize - 1) / 2), boxSize, boxSize));
                                    }
                                    selectBoxes = listOfSelectBox;

                                    calculateCentroid();
                                    break;
                            }
                        }
                        else
                        {
                            switch (selectedObject.borderType)
                            {
                                case Object.BorderType.Rectangle:
                                    startPoint = new Point();
                                    endPoint = new Point();
                                    selectBoxes = new List<Rectangle>();
                                    break;
                                case Object.BorderType.Polygon:
                                    polygonPoints = new List<PointF>();
                                    selectBoxes = new List<Rectangle>();
                                    calculateCentroid();
                                    break;
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
                        }
                    }

                    // Currently drawing or selecting a polygon object
                    if (drawingButtonSelected[polygonDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon))
                    {
                        if (drawingNewPolygon)
                        {
                            if (temporaryPoint.HasValue && polygonPoints.Count != 0)
                            {
                                polygonPoints.Add(temporaryPoint.Value);
                                e.Graphics.DrawPolygon(pen, ((List<PointF>)polygonPoints).ToArray());
                                polygonPoints.Remove(temporaryPoint.Value);
                            }
                        }
                        else
                        {
                            if (polygonPoints.Count != 0)
                            {
                                e.Graphics.DrawPolygon(pen, ((List<PointF>)polygonPoints).ToArray());
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

                            if (editingAtAFrame)
                            {
                                if (!centroid.Equals(new Point()))
                                {
                                    e.Graphics.DrawLine(new Pen(Color.Black), centroid.X - centroidRadius + 4, centroid.Y, centroid.X + centroidRadius - 4, centroid.Y);
                                    e.Graphics.DrawLine(new Pen(Color.Black), centroid.X, centroid.Y - centroidRadius + 4, centroid.X, centroid.Y + centroidRadius - 4);
                                    e.Graphics.DrawEllipse(new Pen(Color.LightGray), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                    if ( draggingCentroid )
                                    {
                                        e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(150, 100, 100, 100)), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                    } else
                                    {
                                        e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(50, 100, 100, 100)), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                    }
                                }
                            }
                        }
                    }


                    // Selecting a rig object
                    if (selectedObject != null && selectedObject.borderType == Object.BorderType.Others)
                    {
                        DrawableLocationMark lm = selectedObject.getScaledLocationMark(frameTrackBar.Value, linear.Item1, linear.Item2);
                        if (lm != null)
                        {
                            selectBoxes = lm.getCornerSelectBoxes(boxSize);

                            foreach (Rectangle r in selectBoxes)
                            {
                                e.Graphics.DrawRectangle(new Pen(Color.Black), r);
                                e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private void calculateCentroid()
        {
            centroid = getCentroid(polygonPoints);
        }

        private PointF getCentroid(List<PointF> polygonPoints)
        {
            float x = 0, y = 0;
            foreach (var point in polygonPoints)
            {
                x += point.X;
                y += point.Y;
            }

            if (polygonPoints.Count != 0)
            {
                return new PointF(x / polygonPoints.Count, y / polygonPoints.Count);
            }
            else
            {
                return new PointF();
            }
        }

        private void handleKeyDownOnAnnotatorTab(KeyEventArgs e)
        {

            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon && editingAtAFrame && draggingSelectBoxes)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    polygonPoints.RemoveAt(draggingSelectBoxIndex);
                    selectBoxes.RemoveAt(draggingSelectBoxIndex);
                    draggingSelectBoxes = false;
                    invalidatePictureBoard();
                }
            }

            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon && editingAtAFrame && e.KeyCode == Keys.Insert)
            {
                //Cursor.Current = Cursors.Cross;
                this.Cursor = Cursors.Cross;
                addPolygonPoint = true;
                invalidatePictureBoard();
            }
        }

        private Tuple<double, Point> getLinearTransform()
        {
            double scale = Math.Min((double)pictureBoard.Width / currentVideo.frameWidth, (double)pictureBoard.Height / currentVideo.frameHeight);
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

        internal void removeDrawingObject(Object o)
        {
            selectedObject = null;
            newObjectContextPanel.Visible = false;
            selectObjContextPanel.Visible = false;
            this.clearInformation();
            selectBoxes = new List<Rectangle>();
            invalidatePictureBoard();
        }

        private void invalidatePictureBoard()
        {
            pictureBoard.Invalidate();
        }
    }
}
