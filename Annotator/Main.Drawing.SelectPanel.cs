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
        private void editObjBtn_Click(object sender, EventArgs e)
        {
            editingAtAFrame = true;
            this.frameTrackBar.Enabled = false;
            editObjectContextPanel.Visible = true;
            editingPolygon = true;

            // Enable the delMarkerBtn if necessary
            if (selectedObject != null)
            {
                if (selectedObject.hasMarkerAt(this.frameTrackBar.Value))
                {
                    delMarkerBtn.Enabled = true;
                }
                else {
                    delMarkerBtn.Enabled = false;
                }

                if (selectedObject.genType == Object.GenType.PROVIDED || selectedObject.genType == Object.GenType.TRACKED)
                {
                    delMarkerBtn.Enabled = false;
                    delAtFrameBtn.Enabled = false;
                    addLocationBtn.Enabled = false;
                } else
                {
                    delMarkerBtn.Enabled = true;
                    delAtFrameBtn.Enabled = true;
                    addLocationBtn.Enabled = true;
                }
            }

            this.pictureBoard.Invalidate();
            this.Invalidate();
        }

        private void deleteObjBtn_Click(object sender, EventArgs e)
        {
            var tempoObject = selectedObject;
            cancelSelectObject();

            DialogResult result = MessageBox.Show("Are you sure you want to remove this object?",
                "Remove",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                removeObject(tempoObject);
            }
        }

        private void cancelSelectObjBtn_Click(object sender, EventArgs e)
        {
            cancelSelectObject();
        }

        private void cancelSelectObject()
        {
            // Cancel edit at a frame
            cancelEditObjBtn_Click(null, null);

            selectObjContextPanel.Visible = false;

            if (selectedObject != null)
            {
                if (objectToObjectTracks[selectedObject] != null)
                {
                    objectToObjectTracks[selectedObject].deselectDeco();
                }
            }

            if (selectedObject != null && selectedObject is RectangleObject)
            {
                doneEditRectangle();
            }

            if (selectedObject != null && selectedObject is PolygonObject)
            {
                doneEditPolygon();
            }
            selectBoxes = new List<RectangleF>();

            clearInformation();
            selectedObject = null;

            polygonDrawing.Enabled = true;
            rectangleDrawing.Enabled = true;

            this.invalidatePictureBoard();
            this.Invalidate();
        }

        private void doneEditRectangle()
        {
            //startPoint = new Point();
            //endPoint = new Point();
            boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
            drawingNewRectangle = false;
        }

        private void doneEditPolygon()
        {
            polygonPointsLocationMark = new PolygonLocationMark2D(-1, new List<PointF>());
            drawingNewPolygon = editingPolygon = false;
        }
    }
}
