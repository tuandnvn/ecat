using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public partial class Main
    {
        private void editObjBtn_Click(object sender, EventArgs e)
        {
            editingAtAFrame = true;
            this.frameTrackBar.Enabled = false;
            editObjectContextPanel.Visible = true;
            editingPolygon = true;

            // Enable the delMarkerBtn if necessary
            if (selectedObject != null)
            {
                if (selectedObject.hasMarkerAt(this.frameTrackBar.Value) )
                {
                    delMarkerBtn.Enabled = true;
                }
            }

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
                if (objectToObjectTracks[selectedObject] != null)
                {
                    objectToObjectTracks[selectedObject].deselectDeco();
                }
            }

            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle)
            {
                doneEditRectangle();
            }

            if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)
            {
                doneEditPolygon();
            }
            selectBoxes = new List<Rectangle>();

            clearInformation();
            selectedObject = null;

            polygonDrawing.Enabled = true;
            rectangleDrawing.Enabled = true;

            this.invalidatePictureBoard();
            this.Invalidate();
        }

        private void doneEditRectangle()
        {
            startPoint = new Point();
            endPoint = new Point();
            drawingNewRectangle = false;
        }

        private void doneEditPolygon()
        {
            polygonPoints = new List<PointF>();
            drawingNewPolygon = editingPolygon = false;
        }
    }
}
