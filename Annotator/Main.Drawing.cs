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
        private PointF? startPoint = null;               //start point for selection in frame from selected video
        //private PointF? endPoint;                 //end point for selection in frame from selected video
        private RectangleLocationMark boundingBoxLocationMark; // Wrapper of currently drawing bounding box
        //private RectangleF boundingBox;             
        private bool drawingNewRectangle = false; //flag variable for pictureBox1 drawing selection rectangle
        private bool draggingSelectBoxes = false;
        private int draggingSelectBoxIndex;

        // Polygon new drawing
        private PolygonLocationMark2D polygonPointsLocationMark;
        //private List<PointF> polygonPoints = new List<PointF>();
        private bool drawingNewPolygon = false;
        private bool editingPolygon = false;            // (drawingNewPolygon, editingPolygon) = ( true, false ) when you're keep drawing the polygon;
                                                        // = (false, true)  when you're done drawing, editing the location of the newly created polygon
                                                        // = (false, false) when you're added the polygon
        private bool addPolygonPoint = false;

        private Point? temporaryPoint; // When the mouse is moving, temporary point hold the current location of cursor, to suggest the shape of the polygon

        List<RectangleF> selectBoxes;
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

        float scale;
        PointF translation;

        protected void InitDrawingComponent()
        {
            drawingButtonGroup.Add(rectangleDrawing);
            drawingButtonGroup.Add(polygonDrawing);
            drawingButtonGroup.Add(zoomDrawing);

            drawingButtonSelected[rectangleDrawing] =
            drawingButtonSelected[polygonDrawing] = drawingButtonSelected[zoomDrawing] = false;

            InitializeZooming();
            InitializeEditPanel();
        }

        private void calculateLinear()
        {
            Tuple<float, PointF> linear = null;

            if (videoReader != null)
            {
                linear = getLinearTransform();
            }
            if (depthReader != null)
            {
                linear = getDepthLinearTransform();
            }

            if (linear != null)
            {
                scale = linear.Item1;
                translation = linear.Item2;
            }
            else
            {
                scale = 1.0f;
                translation = new PointF();
            }
        }

        //Start drawing selection rectangle
        private void pictureBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentSession == null) return;

            // These two lines should be together
            if (videoReader == null && depthReader == null) return;
            calculateLinear();

            if (drawingButtonSelected[zoomDrawing])
            {
                whenZoomButtonAndMouseDown(e);
            }

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
                            RectangleF r = selectBoxes[i];
                            if (r.Contains(e.Location))
                            {
                                draggingSelectBoxes = true;
                                draggingSelectBoxIndex = i;
                                return;
                            }
                        }
                    }


                    if (drawingButtonSelected[rectangleDrawing])
                    {
                        ////set selection point for a new rectangle
                        drawingNewRectangle = true;
                        var scaledPointerLocation = e.Location.scalePoint(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale));
                        startPoint = scaledPointerLocation;
                        boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF(scaledPointerLocation, new SizeF()));
                        return;
                    }

                    // Only allow editing the current rectangle when you edit the rectangle object at a frame
                    if (editingAtAFrame)
                    {
                        return;
                    }
                }
            }

            if (drawingButtonSelected[polygonDrawing] ||   // Drawing mode 
                (selectedObject != null && selectedObject is PolygonObject)) // Editing mode
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && videoReader != null)
                {
                    List<PointF> polygonPoints = polygonPointsLocationMark.boundingPolygon;
                    var scaledPointerLocation = e.Location.scalePoint(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale));

                    if (editingPolygon)
                    {
                        if (!draggingSelectBoxes)
                        {
                            if (selectBoxes != null && selectBoxes.Count != 0)
                            {
                                for (int i = 0; i < selectBoxes.Count; i++)
                                {
                                    RectangleF r = selectBoxes[i];
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
                                double distance = Math.Pow(polygonPoints[i].X - scaledPointerLocation.X, 2) + Math.Pow(polygonPoints[i].Y - scaledPointerLocation.Y, 2);
                                if (minDistance > distance)
                                {
                                    minDistance = distance;
                                    minPos = i;
                                }
                            }

                            draggingSelectBoxIndex = minPos + 1;
                            polygonPoints.Insert(draggingSelectBoxIndex, scaledPointerLocation);
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
                            polygonPoints.Add(scaledPointerLocation);
                            return;
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
                    selectBoxes = (polygonPointsLocationMark.getScaledLocationMark(scale, translation) as PolygonLocationMark2D).getCornerSelectBoxes(boxSize);
                    calculateCentroid();

                    invalidatePictureBoard();
                    return;
                }

                // Only allow editing the current polygon when you edit the polygon object at a frame
                if (editingAtAFrame)
                {
                    return;
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                whenCursorButtonAndMouseDown(e);
            }
        }


        private void pictureBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentSession == null) return;

            // These two lines should be together
            if (videoReader == null && depthReader == null) return;
            calculateLinear();

            var boundingBox = (boundingBoxLocationMark.getScaledLocationMark(scale, translation) as RectangleLocationMark).boundingBox;
            var endPoint = new PointF(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height);

            if (drawingButtonSelected[rectangleDrawing] && drawingNewRectangle)
            {
                endPoint = e.Location;
                boundingBoxLocationMark = new RectangleLocationMark(-1, getRectangleFromStartAndEndPoint(startPoint.Value.scalePoint(scale, translation), endPoint)).
                    getScaledLocationMark(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale)) as RectangleLocationMark;
                invalidatePictureBoard();
                return;
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
                            startPoint = new PointF(selectBoxes[0].getCenter().X, e.Location.Y);
                            endPoint = selectBoxes[3].getCenter();
                            this.Cursor = Cursors.SizeNS;
                            break;
                        case 5:
                            startPoint = new PointF(selectBoxes[1].getCenter().X, e.Location.Y);
                            endPoint = selectBoxes[2].getCenter();
                            this.Cursor = Cursors.SizeNS;
                            break;
                        case 6:
                            startPoint = new PointF(e.Location.X, selectBoxes[2].getCenter().Y);
                            endPoint = selectBoxes[3].getCenter();
                            this.Cursor = Cursors.SizeWE;
                            break;
                        case 7:
                            startPoint = new PointF(e.Location.X, selectBoxes[3].getCenter().Y);
                            endPoint = selectBoxes[0].getCenter();
                            this.Cursor = Cursors.SizeWE;
                            break;
                        // Center point
                        case 8:
                            startPoint = new PointF(e.Location.X - (selectBoxes[2].X - selectBoxes[1].X) / 2, e.Location.Y - (selectBoxes[1].Y - selectBoxes[0].Y) / 2);
                            endPoint = new PointF(e.Location.X + (selectBoxes[2].X - selectBoxes[1].X) / 2, e.Location.Y + (selectBoxes[1].Y - selectBoxes[0].Y) / 2);
                            this.Cursor = Cursors.Hand;
                            break;
                    }
                    boundingBoxLocationMark = new RectangleLocationMark(-1, getRectangleFromStartAndEndPoint(startPoint.Value, endPoint)).
                    getScaledLocationMark(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale)) as RectangleLocationMark;
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
                    polygonPointsLocationMark.boundingPolygon[draggingSelectBoxIndex] = e.Location.scalePoint(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale));
                    selectBoxes[draggingSelectBoxIndex] = new Rectangle(e.Location.X - (boxSize - 1) / 2, e.Location.Y - (boxSize - 1) / 2, boxSize, boxSize);

                    calculateCentroid();

                    invalidatePictureBoard();

                    return;
                }

                // polygonPoints are real coordinates on pictureBoard
                var polygonPoints = new List<PointF>();
                polygonPoints.AddRange(polygonPointsLocationMark.boundingPolygon);
                polygonPoints = polygonPoints.Select(value => value.scalePoint(scale, translation)).ToList();

                switch (centroidMode)
                {
                    case CentroidMode.Dragging:
                        {
                            PointF oldCentroid = getCentroid(polygonPoints);
                            PointF dragTranslation = new PointF(e.Location.X - oldCentroid.X, e.Location.Y - oldCentroid.Y);
                            for (int i = 0; i < polygonPoints.Count; i++)
                            {
                                polygonPoints[i] = new PointF(polygonPoints[i].X + dragTranslation.X, polygonPoints[i].Y + dragTranslation.Y);
                            }
                            centroid = e.Location;

                            List<RectangleF> listOfSelectBox = new List<RectangleF>();
                            foreach (PointF p in polygonPoints)
                            {
                                listOfSelectBox.Add(new RectangleF(p.X - (boxSize - 1) / 2, p.Y - (boxSize - 1) / 2, boxSize, boxSize));
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
                                PointF rotateTranslation = new PointF(tempoPolygonPoints[i].X - centroid.X, tempoPolygonPoints[i].Y - centroid.Y);
                                PointF rotatedTranslation = new PointF((float)(rotateTranslation.X * Math.Cos(alpha) - rotateTranslation.Y * Math.Sin(alpha))
                                    , (float)(rotateTranslation.X * Math.Sin(alpha) + rotateTranslation.Y * Math.Cos(alpha)));
                                polygonPoints[i] = new PointF(centroid.X + rotatedTranslation.X, centroid.Y + rotatedTranslation.Y);
                            }

                            selectBoxes = (polygonPointsLocationMark.getScaledLocationMark(scale, translation) as PolygonLocationMark2D).getCornerSelectBoxes(boxSize);
                            break;
                        }
                    case CentroidMode.Zooming:
                        {
                            // Using the difference on vertical direction to measure zooming scale
                            float verticalDiff = centroid.Y - e.Location.Y;
                            float alpha = verticalDiff / 100;
                            for (int i = 0; i < polygonPoints.Count; i++)
                            {
                                PointF zoomTranslation = new PointF(tempoPolygonPoints[i].X - centroid.X, tempoPolygonPoints[i].Y - centroid.Y);
                                PointF scaledTranslation = new PointF((float)(zoomTranslation.X * Math.Exp(alpha))
                                    , (float)(zoomTranslation.Y * Math.Exp(alpha)));
                                polygonPoints[i] = new PointF(centroid.X + scaledTranslation.X, centroid.Y + scaledTranslation.Y);
                            }
                            selectBoxes = (polygonPointsLocationMark.getScaledLocationMark(scale, translation) as PolygonLocationMark2D).getCornerSelectBoxes(boxSize);
                            break;
                        }
                    case CentroidMode.None:
                        break;
                }

                // Update polygonPointsLocationMark
                polygonPointsLocationMark = new PolygonLocationMark2D(polygonPointsLocationMark.frameNo, polygonPoints.Select(value => value.scalePoint(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale))).ToList());
            }

            if (drawingButtonSelected[zoomDrawing])
            {
                whenZoomButtonAndMouseMove(e);
            }
        }


        private void whenCursorButtonAndMouseDown(MouseEventArgs e)
        {
            var objectWithScore = new List<Tuple<float, Object>>();

            // These two lines should be together
            if (videoReader == null && depthReader == null) return;
            calculateLinear();

            foreach (Object o in currentSession.getObjects())
            {
                LocationMark2D lm = null;

                if (videoReader != null)
                {
                    lm = o.getScaledLocationMark(frameTrackBar.Value, scale, translation);
                }

                if (depthReader != null)
                {
                    lm = getMark2DFromLocationMark3D(o);
                }

                if (lm != null)
                {
                    var score = lm.Score(e.Location);
                    objectWithScore.Add(new Tuple<float, Object>(score, o));
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
            if (currentSession == null)
                return;

            // These two lines should be together
            if (videoReader == null && depthReader == null) return;
            calculateLinear();

            if (drawingButtonSelected[rectangleDrawing] && drawingNewRectangle)
            {
                var boundingBox = (boundingBoxLocationMark.getScaledLocationMark(scale, translation) as RectangleLocationMark).boundingBox;
                var endPoint = e.Location;

                boundingBoxLocationMark = new RectangleLocationMark(-1, getRectangleFromStartAndEndPoint(startPoint.Value.scalePoint(scale, translation), endPoint)).
                    getScaledLocationMark(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale)) as RectangleLocationMark;

                boundingBox = boundingBoxLocationMark.boundingBox;
                if (drawingNewRectangle && boundingBox != null && boundingBox.Width > 0 && boundingBox.Height > 0)
                    newObjectContextPanel.Visible = true;
                else
                    newObjectContextPanel.Visible = false;
                drawingNewRectangle = false;
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
                    calculateLinear();

                    foreach (Object o in currentSession.getObjects())
                    {
                        if (o.objectType == Object.ObjectType._3D)
                        {
                            Pen p = new Pen(o.color, o.borderSize);
                            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                            var depthMark2d = getMark2DFromLocationMark3D(o);

                            if (depthMark2d != null)
                            {
                                depthMark2d.drawOnGraphics(e.Graphics, p);
                            }

                            if (o.Equals(selectedObject))
                            {
                                var selectBoxes = depthMark2d.getCornerSelectBoxes(boxSize);

                                foreach (RectangleF r in selectBoxes)
                                {
                                    e.Graphics.DrawRectangle(new Pen(Color.Black), r);
                                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
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

        private LocationMark2D getMark2DFromLocationMark3D(Object o)
        {
            LocationMark3D mark3d = o.getLocationMark3D(frameTrackBar.Value);

            if (mark3d == null)
                return null;

            LocationMark2D depthMark2d = null;

            if (mark3d is RigLocationMark3D)
            {
                switch (options.showRigOption)
                {
                    case Options.ShowRig.SHOW_UPPER:
                        mark3d = ((RigLocationMark3D)mark3d).getUpperBody();
                        break;
                }
            }

            depthMark2d = mark3d.getDepthViewLocationMark(scale, translation);
            return depthMark2d;
        }

        private void pictureBoardPaintOnVideoFile(PaintEventArgs e)
        {
            try
            {
                if (videoReader != null && currentSession != null)
                {
                    calculateLinear();

                    foreach (Object o in currentSession.getObjects())
                    {
                        if (o != selectedObject || o.genType != Object.GenType.MANUAL)
                        {

                            Pen p = new Pen(o.color, o.borderSize);
                            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            LocationMark2D r = o.getScaledLocationMark(frameTrackBar.Value, scale, translation);

                            if (r != null)
                            {
                                // Clip to upper body for Rig object
                                if (r is RigLocationMark2D)
                                {
                                    switch (options.showRigOption)
                                    {
                                        case Options.ShowRig.SHOW_UPPER:
                                            r = ((RigLocationMark2D)r).getUpperBody();
                                            break;
                                    }
                                }

                                r.drawOnGraphics(e.Graphics, p);
                            }
                        }
                    }


                    if (selectedObject != null)
                    {
                        LocationMark lm = null;
                        if (!editingAtAFrame)
                        {
                            lm = selectedObject.getScaledLocationMark(frameTrackBar.Value, 1, new PointF());
                        }
                        else
                        {
                            if (selectedObject is RectangleObject)
                            {
                                lm = boundingBoxLocationMark.getScaledLocationMark(1, new PointF());
                            }
                            if (selectedObject is PolygonObject)
                            {
                                lm = polygonPointsLocationMark.getScaledLocationMark(1, new PointF());
                            }
                        }

                        if (lm != null)
                        {
                            if (selectedObject is RectangleObject)
                            {
                                boundingBoxLocationMark = lm as RectangleLocationMark;
                            }
                            if (selectedObject is PolygonObject)
                            {
                                polygonPointsLocationMark = lm as PolygonLocationMark2D;
                                selectBoxes = (polygonPointsLocationMark.getScaledLocationMark(scale, translation) as PolygonLocationMark2D).getCornerSelectBoxes(boxSize);
                                calculateCentroid();
                            }
                        }
                        else
                        {
                            if (selectedObject is RectangleObject)
                            {
                                boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
                                selectBoxes = new List<RectangleF>();
                            }
                            if (selectedObject is PolygonObject)
                            {
                                polygonPointsLocationMark = new PolygonLocationMark2D(-1, new List<PointF>());
                                selectBoxes = new List<RectangleF>();
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
                        if (videoReader != null)
                        {
                            //float lowerX = Math.Min(startPoint.Value.X, endPoint.Value.X);
                            //float lowerY = Math.Min(startPoint.Value.Y, endPoint.Value.Y);
                            //float higherX = Math.Max(startPoint.Value.X, endPoint.Value.X);
                            //float higherY = Math.Max(startPoint.Value.Y, endPoint.Value.Y);

                            //RectangleF boundingBox = new RectangleF(lowerX, lowerY, higherX - lowerX, higherY - lowerY);
                            //boundingBoxLocationMark = new RectangleLocationMark(-1, boundingBox).getScaledLocationMark(1 / scale,
                            //    new PointF(-translation.X / scale, -translation.Y / scale)) as RectangleLocationMark;

                            RectangleF boundingBox = (boundingBoxLocationMark.getScaledLocationMark(scale, translation) as RectangleLocationMark).boundingBox;

                            selectBoxes = boundingBox.getCornerSelectBoxes(boxSize);

                            e.Graphics.DrawRectangle(pen, boundingBox);

                            foreach (RectangleF r in selectBoxes)
                            {
                                e.Graphics.DrawRectangle(new Pen(Color.Black), r);
                                e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                            }
                        }
                    }

                    if (polygonPointsLocationMark == null)
                    {
                        polygonPointsLocationMark = new PolygonLocationMark2D(-1, new List<PointF>());
                    }
                    var polygonPoints = new List<PointF>();
                    polygonPoints = (polygonPointsLocationMark.getScaledLocationMark(scale, translation) as PolygonLocationMark2D).boundingPolygon;

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
                                RectangleF r = selectBoxes[index];
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
                        LocationMark2D lm = selectedObject.getScaledLocationMark(frameTrackBar.Value, scale, translation);
                        if (lm != null)
                        {
                            selectBoxes = lm.getCornerSelectBoxes(boxSize);

                            foreach (RectangleF r in selectBoxes)
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

        private RectangleF getRectangleFromStartAndEndPoint(PointF startPoint, PointF endPoint)
        {
            float lowerX = Math.Min(startPoint.X, endPoint.X);
            float lowerY = Math.Min(startPoint.Y, endPoint.Y);
            float higherX = Math.Max(startPoint.X, endPoint.X);
            float higherY = Math.Max(startPoint.Y, endPoint.Y);

            RectangleF boundingBox = new RectangleF(lowerX, lowerY, higherX - lowerX, higherY - lowerY);
            return boundingBox;
        }

        private void calculateCentroid()
        {
            calculateLinear();

            centroid = getCentroid(polygonPointsLocationMark.boundingPolygon.Select(value => value.scalePoint(scale, translation)).ToList());
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
            // Click on rectangle
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.R)
            {
                if (rectangleDrawing.Enabled)
                {
                    rectangleDrawing_MouseDown(null, null);
                }
                return;
            }

            // Click on polygon
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.P)
            {
                if (polygonDrawing.Enabled)
                {
                    polygonDrawing_MouseDown(null, null);
                }
                return;
            }

            // Save down session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                saveCurrentSession();
            }

            // Edit session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.E)
            {
                editSessionMenuItem_Click(null, null);
            }

            // Add a file into session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                addFileToSessionMenuItem_Click(null, null);
            }

            // While editing a polygon
            if (selectedObject != null && selectedObject is PolygonObject && editingAtAFrame)
            {
                calculateLinear();

                // While dragging a select box, and user press delete, handle delete that polygon point
                if (draggingSelectBoxes && e.KeyCode == Keys.Delete)
                {
                    polygonPointsLocationMark.boundingPolygon.RemoveAt(draggingSelectBoxIndex);
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
                    tempoPolygonPoints.AddRange(polygonPointsLocationMark.boundingPolygon.Select(value => value.scalePoint(scale, translation)));
                }

                if (centroidMode == CentroidMode.Dragging && e.KeyCode == Keys.ShiftKey)
                {
                    centroidMode = CentroidMode.Zooming;
                    tempoPolygonPoints.AddRange(polygonPointsLocationMark.boundingPolygon.Select(value => value.scalePoint(scale, translation)));
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

        private Tuple<float, PointF> getLinearTransform()
        {
            float scale = Math.Min((float)pictureBoard.Width / videoReader.frameWidth, (float)pictureBoard.Height / videoReader.frameHeight);
            PointF translation = new PointF((pictureBoard.Width - videoReader.frameWidth * scale) / 2, (pictureBoard.Height - videoReader.frameHeight * scale) / 2);
            return new Tuple<float, PointF>(scale, translation);
        }

        private Tuple<float, PointF> getDepthLinearTransform()
        {
            float scale = Math.Min((float)pictureBoard.Width / depthReader.depthWidth, (float)pictureBoard.Height / depthReader.depthHeight);
            PointF translation = new PointF((pictureBoard.Width - depthReader.depthWidth * scale) / 2, (pictureBoard.Height - depthReader.depthHeight * scale) / 2);
            return new Tuple<float, PointF>(scale, translation);
        }

        private void rectangleDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(rectangleDrawing, drawingButtonGroup, !drawingButtonSelected[rectangleDrawing]);
            cancelDrawing();
        }

        private void polygonDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(polygonDrawing, drawingButtonGroup, !drawingButtonSelected[polygonDrawing]);
            cancelDrawing();
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
            selectBoxes = new List<RectangleF>();
            invalidatePictureBoard();
        }

        private void invalidatePictureBoard()
        {
            pictureBoard.Invalidate();
        }
    }
}
