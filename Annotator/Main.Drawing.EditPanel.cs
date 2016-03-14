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
                editingAtAFrame = false;
                this.frameTrackBar.Enabled = true;
                editObjectContextPanel.Visible = false;

                if (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle)
                {
                    selectedObject.setBounding(frameTrackBar.Value, boundingBox);
                }

                if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)
                {
                    selectedObject.setBounding(frameTrackBar.Value, polygonPoints);
                }

                redrawObjectMarks();
            }
        }

        private void delAtFrameBtn_Click(object sender, EventArgs e)
        {
            if (editingAtAFrame)
            {
                editingAtAFrame = false;
                this.frameTrackBar.Enabled = true;
                editObjectContextPanel.Visible = false;
                if (selectedObject != null && selectedObject.borderType == Object.BorderType.Rectangle)
                {
                    selectedObject.delete(frameTrackBar.Value);
                }

                if (selectedObject != null && selectedObject.borderType == Object.BorderType.Polygon)
                {
                    selectedObject.delete(frameTrackBar.Value);
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

                if (currentVideo != null && selectedObject != null)
                {
                    spatialLinkForm = new SpatialLinkForm(this, currentVideo, selectedObject, frameTrackBar.Value);
                    spatialLinkForm.Show();
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
                selectBoxes = new Rectangle[] { };
                pictureBoard.Invalidate();
            }
        }

        public void redrawObjectMarks()
        {
            if (currentSession.objectToObjectTracks.ContainsKey(selectedObject))
            {
                currentSession.objectToObjectTracks[selectedObject].drawObjectMarks();
                currentSession.objectToObjectTracks[selectedObject].Invalidate();
            }
        }
    }
}
