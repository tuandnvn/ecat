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
                if (e.Button == System.Windows.Forms.MouseButtons.Left && currentVideo != null )
                {
                    if (editingPolygon) {
                        // If mouse down click on resize select boxes
                        if (draggingSelectBoxes)
                        {
                            draggingSelectBoxes = false;
                            pictureBoard.Invalidate();
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
                    } else
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
                    pictureBoard.Invalidate();
                }
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
                    pictureBoard.Invalidate();
                    return;
                }
                pictureBoard.Invalidate();
            }

            if (drawingButtonSelected[rectangleDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle) )
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
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
                pictureBoard.Invalidate();
            }
               

            if (drawingButtonSelected[polygonDrawing])
            {
                if (drawingNewPolygon)
                {
                    temporaryPoint = e.Location;
                    pictureBoard.Invalidate();
                }
            }

            if (drawingButtonSelected[polygonDrawing]  || (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon))
            {
                if (draggingSelectBoxes)
                {
                    polygonPoints[draggingSelectBoxIndex] = e.Location;
                    selectBoxes[draggingSelectBoxIndex] = new Rectangle(e.Location.X - (boxSize - 1) / 2, e.Location.Y - (boxSize - 1) / 2, boxSize, boxSize);

                    pictureBoard.Invalidate();
                }
            }

            if (drawingButtonSelected[cursorDrawing])
            {
                foreach (Object o in currentSession.getObjects())
                {
                    var linear = getLinearTransform();
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
                }
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
        }

        private void pictureBoard_Paint(object sender, PaintEventArgs e)
        {
            // Has been drawn before
            if (currentVideo != null)
            {
                var linear = getLinearTransform();

                foreach (Object o in currentSession.getObjects())
                {
                    if (o != selectedObject)
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

                // Currently drawing
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
                        e.Graphics.Save();
                    }
                }

                if (drawingButtonSelected[polygonDrawing] || (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon))
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
                        e.Graphics.Save();
                    }
                }
            }
        }

        internal void selectObject(Object o)
        {
            this.selectedObject = o;

            if (selectedObject != null)
            {
                selectObjContextPanel.Visible = true;
            }

            this.showInformation(o);
            //pictureBoard.Invalidate();
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
            pictureBoard.Invalidate();
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
                pictureBoard.Invalidate();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            boundingBorder = (int)numericUpDown1.Value;
            pictureBoard.Invalidate();
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
                pictureBoard.Invalidate();
            }

            currentSession.addObjectAnnotation(objectToAdd);
            clearMiddleCenterPanel();
            populateMiddleCenterPanel();
        }

        private Tuple<double, Point> getLinearTransform()
        {
            double scale = Math.Min(pictureBoard.Width / currentVideo.getFrameWidth(), pictureBoard.Height / currentVideo.getFrameHeight());
            Point translation = new Point ( (int)(pictureBoard.Width - currentVideo.getFrameWidth() * scale )/2, (int)(pictureBoard.Height - currentVideo.getFrameHeight()  * scale ) /2 );
            return new Tuple<double, Point>(scale, translation);
        }

        private void editObjBtn_Click(object sender, EventArgs e)
        {
            editingAtAFrame = true;
            this.frameTrackBar.Enabled = false;
            editObjectContextPanel.Visible = true;
            this.Invalidate();
        }

        private void deleteObjBtn_Click(object sender, EventArgs e)
        {
            editingAtAFrame = false;
            editObjectContextPanel.Visible = false;
            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle)
            {
                doneEditRectangle();
            }

            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)
            {
                doneEditPolygon();
            }
            selectedObject = null;

            this.Invalidate();
        }

        private void cancelSelectObjBtn_Click(object sender, EventArgs e)
        {
            cancelSelectObject();
        }

        private void cancelSelectObject()
        {
            editingAtAFrame = false;
            selectObjContextPanel.Visible = false;
            editObjectContextPanel.Visible = false;

            if (selectedObject != null)
            {
                currentSession.deselectObject(selectedObject);
            }

            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle)
            {
                doneEditRectangle();
            }

            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)
            {
                doneEditPolygon();
            }
            selectBoxes = new Rectangle[0] { };

            clearInformation();
            selectedObject = null;
            this.Invalidate();
        }

        private void doneEditRectangle()
        {
            startPoint = null;
            endPoint = null;
            drawingNewRectangle = false;
        }

        private void doneEditPolygon()
        {
            polygonPoints = new List<Point>();
            drawingNewPolygon = editingPolygon = false;
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
            pictureBoard.Invalidate();
        }

        
    }
}
