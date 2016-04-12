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
            selectBoxes = new Rectangle[0] { };

            clearInformation();
            selectedObject = null;
            this.invalidatePictureBoard();
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
    }
}
