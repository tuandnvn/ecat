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
                var linear = getLinearTransform();
                
                editingAtAFrame = false;
                this.frameTrackBar.Enabled = true;
                editObjectContextPanel.Visible = false;

                if (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle)
                {
                    if (boundingBox.Width == 0 && boundingBox.Height == 0)
                    {
                        // This case happens when there is no bounding box
                        selectedObject.setCopyBounding(frameTrackBar.Value);
                    } else
                    {
                        selectedObject.setBounding(frameTrackBar.Value, boundingBox, linear.Item1, linear.Item2);
                    }
                }

                if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)
                {
                    if (polygonPoints.Count == 0)
                    {
                        // This case happens when there is no  polygon points
                        selectedObject.setCopyBounding(frameTrackBar.Value);
                    } else
                    {
                        selectedObject.setBounding(frameTrackBar.Value, polygonPoints, linear.Item1, linear.Item2);
                    }
                }

                redrawObjectMarks();
                Invalidate();
            }
        }

        private void delAtFrameBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                editingAtAFrame = false;
                this.frameTrackBar.Enabled = true;
                editObjectContextPanel.Visible = false;
                if (selectedObject != null && 
                    (selectedObject.borderType == Object.BorderType.Rectangle || selectedObject.borderType == Object.BorderType.Polygon) )
                {
                    selectedObject.delete(frameTrackBar.Value);
                }

                redrawObjectMarks();
            }
        }

        private void delMarkerBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                editingAtAFrame = false;
                this.frameTrackBar.Enabled = true;
                editObjectContextPanel.Visible = false;
                if (selectedObject != null &&
                    (selectedObject.borderType == Object.BorderType.Rectangle || selectedObject.borderType == Object.BorderType.Polygon))
                {
                    selectedObject.deleteMarker(frameTrackBar.Value);
                }

                redrawObjectMarks();
            }
        }

        private void addSpatialBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                editingAtAFrame = false;
                this.frameTrackBar.Enabled = true;
                editObjectContextPanel.Visible = false;

                if (currentSession != null && selectedObject != null)
                {
                    spatialLinkForm = new SpatialLinkForm(this, currentSession, selectedObject, frameTrackBar.Value);
                    spatialLinkForm.Show();
                    spatialLinkForm.Location = new Point()
                    {
                        X = Math.Max(this.Location.X, this.Location.X + (this.Width - spatialLinkForm.Width) / 2),
                        Y = Math.Max(this.Location.Y, this.Location.Y + (this.Height - spatialLinkForm.Height) / 2)
                    };
                }
            }
        }

        
        private void cancelEditObjBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                editingAtAFrame = false;
                this.frameTrackBar.Enabled = true;
                editObjectContextPanel.Visible = false;

                draggingSelectBoxes = false;
                selectBoxes = new List<Rectangle>();
                invalidatePictureBoard();
            }
        }

        public void redrawObjectMarks()
        {
            if (objectToObjectTracks.ContainsKey(selectedObject))
            {
                objectToObjectTracks[selectedObject].drawObjectMarks();
                objectToObjectTracks[selectedObject].Invalidate();
            }
        }
    }
}
