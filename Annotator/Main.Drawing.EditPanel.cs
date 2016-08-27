using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {
        Form spatialLinkForm;

        private void InitializeEditPanel()
        {
        }

        private void addLocationBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                doneEditingAtAFrame();
                var linear = getLinearTransform();

                if (selectedObject != null && selectedObject is RectangleObject)
                {
                    RectangleF boundingBox = boundingBoxLocationMark.boundingBox;
                    if (boundingBox.Width == 0 && boundingBox.Height == 0)
                    {
                        // This case happens when there is no bounding box
                        selectedObject.setCopyBounding(frameTrackBar.Value);
                    }
                    else
                    {
                        (selectedObject as RectangleObject).setBounding(frameTrackBar.Value, boundingBox, 1, new PointF());
                    }
                }

                if (selectedObject != null && selectedObject is PolygonObject)
                {
                    if (polygonPointsLocationMark.boundingPolygon.Count == 0)
                    {
                        // This case happens when there is no  polygon points
                        selectedObject.setCopyBounding(frameTrackBar.Value);
                    }
                    else
                    {
                        (selectedObject as PolygonObject).setBounding(frameTrackBar.Value, polygonPointsLocationMark.boundingPolygon, 1, new PointF());
                    }
                }

                redrawObjectMarks();
                Invalidate();

                logSession($"Object {selectedObject.id} edited at location {frameTrackBar.Value}");
            }
        }

        private void delAtFrameBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                doneEditingAtAFrame();

                if (selectedObject != null &&
                    (selectedObject is RectangleObject || selectedObject is PolygonObject))
                {
                    selectedObject.delete(frameTrackBar.Value);
                }

                redrawObjectMarks();

                logSession($"Object {selectedObject.id} deleted at location {frameTrackBar.Value}");
            }
        }

        private void delMarkerBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                doneEditingAtAFrame();

                if (selectedObject != null &&
                    (selectedObject is RectangleObject || selectedObject is PolygonObject))
                {
                    selectedObject.deleteMarker(frameTrackBar.Value);
                }

                redrawObjectMarks();

                logSession($"Object {selectedObject.id} marker deleted at location {frameTrackBar.Value}");
            }
        }

        private void addSpatialBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                doneEditingAtAFrame();

                if (currentSession != null && selectedObject != null)
                {
                    spatialLinkForm = new ObjectLinkForm(this, currentProject, currentSession, selectedObject, frameTrackBar.Value);
                    spatialLinkForm.StartPosition = FormStartPosition.CenterParent;
                    spatialLinkForm.ShowDialog();
                }
            }
        }


        private void cancelEditObjBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                doneEditingAtAFrame();
            }
        }

        private void doneEditingAtAFrame()
        {
            editingAtAFrame = false;
            this.frameTrackBar.Enabled = true;
            editObjectContextPanel.Visible = false;
            editingPolygon = false;
            doneWithCentroid();
            draggingSelectBoxes = false;
            selectBoxes = new List<RectangleF>();

            invalidatePictureBoard();
        }

        private void doneWithCentroid()
        {
            centroidMode = CentroidMode.None;
            tempoPolygonPoints = new List<PointF>();
        }

        public void redrawObjectMarks()
        {
            if (objectToObjectTracks.ContainsKey(selectedObject))
            {
                objectToObjectTracks[selectedObject].drawObjectMarks();
                //objectToObjectTracks[selectedObject].Invalidate();
            }
        }
    }
}
