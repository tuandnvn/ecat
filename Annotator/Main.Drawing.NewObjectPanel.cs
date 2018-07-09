﻿using System;
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
        //Add object to the video
        private void addObjBtn_Click(object sender, EventArgs e)
        {
            var linear = getLinearTransform();
            Object objectToAdd = null;
            if (drawingButtonSelected[rectangleDrawing])
            {
                var relFileName = videoReader.fileName.Split(Path.DirectorySeparatorChar)[videoReader.fileName.Split(Path.DirectorySeparatorChar).Length - 1];
                objectToAdd = new RectangleObject(currentSession, null, colorDialog1.Color, (int)borderSizeNumeric.Value, relFileName);
                (objectToAdd as RectangleObject).setBounding(frameTrackBar.Value, boundingBoxLocationMark.boundingBox, 1, new PointF());
                //startPoint = new Point();
                //endPoint = new Point();
                boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
                startPoint = null;
            }

            if (drawingButtonSelected[polygonDrawing])
            {
                var relFileName = videoReader.fileName.Split(Path.DirectorySeparatorChar)[videoReader.fileName.Split(Path.DirectorySeparatorChar).Length - 1];
                objectToAdd = new PolygonObject(currentSession, null, colorDialog1.Color, (int)borderSizeNumeric.Value, relFileName);
                (objectToAdd as PolygonObject).setBounding(frameTrackBar.Value, polygonPointsLocationMark.boundingPolygon, 1, new PointF());
                polygonPointsLocationMark = new PolygonLocationMark2D(-1, new List<PointF>());

                // (drawingNewPolygon, editingPolygon) = (false, false) when you're added the polygon
                drawingNewPolygon = false;
                editingPolygon = false;
            }

            if (objectToAdd != null)
            {
                currentSession.addObject(objectToAdd);
                newObjectContextPanel.Visible = false;
                selectBoxes = new List<RectangleF>();
                invalidatePictureBoard();

                addObjectAnnotation(objectToAdd);
                this.logSession($"Object {objectToAdd.id} added at location {frameTrackBar.Value}");
            }
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
                //startPoint = new Point();
                //endPoint = new Point();
                boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
                startPoint = null;
                drawingNewRectangle = false;
            }

            if (drawingButtonSelected[polygonDrawing])
            {
                polygonPointsLocationMark = new PolygonLocationMark2D(-1, new List<PointF>());
                drawingNewPolygon = editingPolygon = false;
            }
            draggingSelectBoxes = false;
            selectBoxes = new List<RectangleF>();
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
            boundingBorder = (int)borderSizeNumeric.Value;
            invalidatePictureBoard();
        }

    }
}
