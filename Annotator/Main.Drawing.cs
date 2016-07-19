using Accord.Math;
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

        enum CentroidMode
        {
            None,
            /// <summary>
            /// If true: 
            /// User holds the centroid point of polygon
            /// to drag it across the image board
            /// </summary>
            Dragging,
            /// <summary>
            /// If true: 
            /// User holds the centroid point of polygon while pressing Ctrl 
            /// to rotate the polygon
            /// </summary>
            Rotating,
            /// <summary>
            /// If true: 
            /// User holds the centroid point of polygon while pressing Shift
            /// to zoom in and out the polygon
            /// </summary>
            Zooming
        }

        CentroidMode centroidMode = CentroidMode.None;
        // To be used with Rotating and Zooming mode
        private List<PointF> tempoPolygonPoints = new List<PointF>();

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
                (selectedObject != null && selectedObject is RectangleObject))
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && videoReader != null)
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
                (selectedObject != null && selectedObject is PolygonObject)) // Editing mode
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && videoReader != null)
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

                        if (centroidMode == CentroidMode.None && !centroid.Equals(new Point()))
                        {
                            RectangleF r = new RectangleF(centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                            if (r.Contains(e.Location))
                            {
                                centroidMode = CentroidMode.Dragging;
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
                if (e.Button == System.Windows.Forms.MouseButtons.Right && videoReader != null && drawingNewPolygon)
                {
                    drawingNewPolygon = false;
                    editingPolygon = true;
                    temporaryPoint = null;
                    newObjectContextPanel.Visible = true;
                    resetSelectBoxes();

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

            if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject is RectangleObject))
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

            if (drawingButtonSelected[polygonDrawing] || (selectedObject != null && selectedObject is PolygonObject))
            {
                if (draggingSelectBoxes)
                {
                    polygonPoints[draggingSelectBoxIndex] = e.Location;
                    selectBoxes[draggingSelectBoxIndex] = new Rectangle(e.Location.X - (boxSize - 1) / 2, e.Location.Y - (boxSize - 1) / 2, boxSize, boxSize);

                    calculateCentroid();

                    invalidatePictureBoard();

                    return;
                }

                switch (centroidMode)
                {
                    case CentroidMode.Dragging:
                        {
                            PointF oldCentroid = getCentroid(polygonPoints);
                            PointF translation = new PointF(e.Location.X - oldCentroid.X, e.Location.Y - oldCentroid.Y);
                            for (int i = 0; i < polygonPoints.Count; i++)
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
                            break;
                        }
                    case CentroidMode.Rotating:
                        {
                            // Using the difference on vertical direction to measure rotating angle
                            float verticalDiff = centroid.Y - e.Location.Y;
                            float alpha = verticalDiff / 180;

                            for (int i = 0; i < polygonPoints.Count; i++)
                            {
                                PointF translation = new PointF(tempoPolygonPoints[i].X - centroid.X, tempoPolygonPoints[i].Y - centroid.Y);
                                PointF rotatedTranslation = new PointF((float)(translation.X * Math.Cos(alpha) - translation.Y * Math.Sin(alpha))
                                    , (float)(translation.X * Math.Sin(alpha) + translation.Y * Math.Cos(alpha)));
                                polygonPoints[i] = new PointF(centroid.X + rotatedTranslation.X, centroid.Y + rotatedTranslation.Y);
                            }

                            resetSelectBoxes();
                            break;
                        }
                    case CentroidMode.Zooming:
                        {
                            // Using the difference on vertical direction to measure zooming scale
                            float verticalDiff = centroid.Y - e.Location.Y;
                            float alpha = verticalDiff / 100;
                            for (int i = 0; i < polygonPoints.Count; i++)
                            {
                                PointF translation = new PointF(tempoPolygonPoints[i].X - centroid.X, tempoPolygonPoints[i].Y - centroid.Y);
                                PointF scaledTranslation = new PointF((float)(translation.X * Math.Exp(alpha))
                                    , (float)(translation.Y * Math.Exp(alpha)));
                                polygonPoints[i] = new PointF(centroid.X + scaledTranslation.X, centroid.Y + scaledTranslation.Y);
                            }
                            resetSelectBoxes();
                            break;
                        }
                    case CentroidMode.None:
                        break;
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

            // Make sure there is some object to select
            if (objectWithScore.Count == 0)
                return;

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

            if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject is RectangleObject))
            {
                if (draggingSelectBoxes)
                {
                    draggingSelectBoxes = false;
                }
            }

            if (drawingButtonSelected[polygonDrawing] ||   // Drawing mode 
                (selectedObject != null && selectedObject is PolygonObject)) // Editing mode
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && videoReader != null)
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
                        // ---- Handle when release hold of centroid
                        // Turn centroid mode to None
                        if (centroidMode == CentroidMode.Dragging || centroidMode == CentroidMode.Rotating || centroidMode == CentroidMode.Zooming)
                        {
                            centroidMode = CentroidMode.None;
                            invalidatePictureBoard();
                            return;
                        }
                    }
                }
            }
        }

        private void pictureBoard_Paint(object sender, PaintEventArgs e)
        {
            pictureBoardPaintOnVideoFile(e);
            pictureBoardPaintOnDepthFile(e);
        }

        private void pictureBoardPaintOnDepthFile(PaintEventArgs e)
        {
            try
            {
                if (depthReader != null && currentSession != null)
                {
                    var linear = getDepthLinearTransform();

                    foreach (Object o in currentSession.getObjects())
                    {
                        if (o.objectType == Object.ObjectType._3D)
                        {
                            Pen p = new Pen(o.color, o.borderSize);
                            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                            if (o.object3DMarks.ContainsKey(frameTrackBar.Value))
                            {
                                LocationMark mark3d = o.object3DMarks[frameTrackBar.Value];
                                DrawableLocationMark depthMark2d = null;

                                if (mark3d is GlyphBoxLocationMark<Point3>)
                                {
                                    depthMark2d = ((GlyphBoxLocationMark<Point3>)mark3d).getDepthViewLocationMark(linear.Item1, linear.Item2);
                                }

                                if (mark3d is RigLocationMark<Point3>)
                                {
                                    switch (options.showRigOption)
                                    {
                                        case Options.ShowRig.SHOW_UPPER:
                                            mark3d = ((RigLocationMark<Point3>)mark3d).getUpperBody();
                                            break;
                                    }
                                    depthMark2d = ((RigLocationMark<Point3>)mark3d).getDepthViewLocationMark(linear.Item1, linear.Item2);
                                }


                                if (depthMark2d != null)
                                {
                                    depthMark2d.drawOnGraphics(e.Graphics, p);
                                }

                                if (o.Equals(selectedObject))
                                {
                                    var selectBoxes = depthMark2d.getCornerSelectBoxes(boxSize);

                                    foreach (Rectangle r in selectBoxes)
                                    {
                                        e.Graphics.DrawRectangle(new Pen(Color.Black), r);
                                        e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private void pictureBoardPaintOnVideoFile(PaintEventArgs e)
        {
            try
            {
                if (videoReader != null && currentSession != null)
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
                                // Clip to upper body for Rig object
                                if (r is RigLocationMark<PointF>)
                                {
                                    switch (options.showRigOption)
                                    {
                                        case Options.ShowRig.SHOW_UPPER:
                                            r = ((RigLocationMark<PointF>)r).getUpperBody();
                                            break;
                                    }
                                }

                                r.drawOnGraphics(e.Graphics, p);
                            }
                        }
                    }


                    if (selectedObject != null && !editingAtAFrame)
                    {
                        LocationMark lm = selectedObject.getScaledLocationMark(frameTrackBar.Value, linear.Item1, linear.Item2);
                        if (lm != null)
                        {
                            if (selectedObject is RectangleObject)
                            {
                                boundingBox = ((RectangleLocationMark)lm).boundingBox;
                                startPoint = new Point(boundingBox.X, boundingBox.Y);
                                endPoint = new Point(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height);
                            }
                            if (selectedObject is PolygonObject)
                            {
                                polygonPoints = ((PolygonLocationMark)lm).boundingPolygon;

                                resetSelectBoxes();
                                calculateCentroid();
                            }
                        }
                        else
                        {
                            if (selectedObject is RectangleObject)
                            {
                                startPoint = new Point();
                                endPoint = new Point();
                                selectBoxes = new List<Rectangle>();
                            }
                            if (selectedObject is PolygonObject)
                            {
                                polygonPoints = new List<PointF>();
                                selectBoxes = new List<Rectangle>();
                                calculateCentroid();
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
                    if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject is RectangleObject))
                    {
                        if (endPoint.HasValue && startPoint.HasValue && videoReader != null)
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
                    if (drawingButtonSelected[polygonDrawing] || (selectedObject != null && selectedObject is PolygonObject))
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
                                    Pen dashedPen = new Pen(Color.Black);
                                    dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                                    Pen normalPen = new Pen(Color.Black);
                                    switch (centroidMode)
                                    {
                                        case CentroidMode.Dragging:
                                            e.Graphics.DrawLine(normalPen, centroid.X - centroidRadius + 4, centroid.Y, centroid.X + centroidRadius - 4, centroid.Y);
                                            e.Graphics.DrawLine(normalPen, centroid.X, centroid.Y - centroidRadius + 4, centroid.X, centroid.Y + centroidRadius - 4);
                                            e.Graphics.DrawEllipse(new Pen(Color.LightGray), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(150, 100, 100, 100)), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            break;
                                        case CentroidMode.Rotating:
                                            e.Graphics.DrawLine(normalPen, centroid.X - 3 * centroidRadius, centroid.Y, centroid.X + 3 * centroidRadius, centroid.Y);
                                            e.Graphics.DrawLine(normalPen, centroid.X, centroid.Y - centroidRadius + 4, centroid.X, centroid.Y + centroidRadius - 4);
                                            e.Graphics.DrawEllipse(new Pen(Color.LightGray), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(150, 100, 100, 100)), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            break;
                                        case CentroidMode.Zooming:
                                            e.Graphics.DrawLine(normalPen, centroid.X - centroidRadius, centroid.Y, centroid.X + centroidRadius, centroid.Y);
                                            e.Graphics.DrawLine(normalPen, centroid.X, centroid.Y - centroidRadius, centroid.X, centroid.Y + centroidRadius);


                                            e.Graphics.DrawLine(dashedPen, centroid.X - 2 * centroidRadius, centroid.Y, centroid.X, centroid.Y - 2 * centroidRadius);
                                            e.Graphics.DrawLine(dashedPen, centroid.X - 2 * centroidRadius, centroid.Y, centroid.X, centroid.Y + 2 * centroidRadius);
                                            e.Graphics.DrawLine(dashedPen, centroid.X + 2 * centroidRadius, centroid.Y, centroid.X, centroid.Y - 2 * centroidRadius);
                                            e.Graphics.DrawLine(dashedPen, centroid.X + 2 * centroidRadius, centroid.Y, centroid.X, centroid.Y + 2 * centroidRadius);

                                            e.Graphics.DrawEllipse(new Pen(Color.LightGray), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(150, 100, 100, 100)), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            break;
                                        case CentroidMode.None:
                                            e.Graphics.DrawLine(normalPen, centroid.X - centroidRadius + 4, centroid.Y, centroid.X + centroidRadius - 4, centroid.Y);
                                            e.Graphics.DrawLine(normalPen, centroid.X, centroid.Y - centroidRadius + 4, centroid.X, centroid.Y + centroidRadius - 4);
                                            e.Graphics.DrawEllipse(new Pen(Color.LightGray), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(50, 100, 100, 100)), centroid.X - centroidRadius, centroid.Y - centroidRadius, 2 * centroidRadius, 2 * centroidRadius);
                                            break;
                                    }
                                }
                            }
                        }
                    }


                    // Selecting other kind of objects
                    if (selectedObject != null && !(selectedObject is PolygonObject) && !(selectedObject is RectangleObject))
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
            // While editing a polygon
            if (selectedObject != null && selectedObject is PolygonObject && editingAtAFrame)
            {
                // While dragging a select box, and user press delete, handle delete that polygon point
                if (draggingSelectBoxes && e.KeyCode == Keys.Delete)
                {
                    polygonPoints.RemoveAt(draggingSelectBoxIndex);
                    selectBoxes.RemoveAt(draggingSelectBoxIndex);
                    draggingSelectBoxes = false;
                }

                if (e.KeyCode == Keys.Insert)
                {
                    this.Cursor = Cursors.Cross;
                    addPolygonPoint = true;
                }

                if (centroidMode == CentroidMode.Dragging && e.KeyCode == Keys.ControlKey)
                {
                    centroidMode = CentroidMode.Rotating;
                    tempoPolygonPoints.AddRange(polygonPoints);
                }

                if (centroidMode == CentroidMode.Dragging && e.KeyCode == Keys.ShiftKey)
                {
                    centroidMode = CentroidMode.Zooming;
                    tempoPolygonPoints.AddRange(polygonPoints);
                }

                invalidatePictureBoard();
            }
        }


        private void handleKeyUpOnAnnotatorTab(KeyEventArgs e)
        {
            if (centroidMode == CentroidMode.Rotating && e.KeyCode == Keys.ControlKey)
            {
                centroidMode = CentroidMode.None;
                tempoPolygonPoints = new List<PointF>();
            }

            if (centroidMode == CentroidMode.Zooming && e.KeyCode == Keys.ShiftKey)
            {
                centroidMode = CentroidMode.None;
                tempoPolygonPoints = new List<PointF>();
            }

            invalidatePictureBoard();
        }

        private Tuple<float, Point> getLinearTransform()
        {
            float scale = Math.Min((float)pictureBoard.Width / videoReader.frameWidth, (float)pictureBoard.Height / videoReader.frameHeight);
            Point translation = new Point((int)(pictureBoard.Width - videoReader.frameWidth * scale) / 2, (int)(pictureBoard.Height - videoReader.frameHeight * scale) / 2);
            return new Tuple<float, Point>(scale, translation);
        }

        private Tuple<float, Point> getDepthLinearTransform()
        {
            float scale = Math.Min((float)pictureBoard.Width / depthReader.depthWidth, (float)pictureBoard.Height / depthReader.depthHeight);
            Point translation = new Point((int)(pictureBoard.Width - depthReader.depthWidth * scale) / 2, (int)(pictureBoard.Height - depthReader.depthHeight * scale) / 2);
            return new Tuple<float, Point>(scale, translation);
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

        private void resetSelectBoxes()
        {
            List<Rectangle> listOfSelectBox = new List<Rectangle>();
            foreach (PointF p in polygonPoints)
            {
                listOfSelectBox.Add(new Rectangle((int)(p.X - (boxSize - 1) / 2), (int)(p.Y - (boxSize - 1) / 2), boxSize, boxSize));
            }
            selectBoxes = listOfSelectBox;
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
