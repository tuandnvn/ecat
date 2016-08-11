using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class Main
    {
        internal void removeObject(Object o)
        {
            removeDrawingObject(o);
            currentSession.removeObject(o.id);

            // Remove the annotation corresponding to this object
            // and rearrage all object annotations
            if (this.objectToObjectTracks.ContainsKey(o))
            {
                ObjectAnnotation oa = this.objectToObjectTracks[o];

                this.objectToObjectTracks.Remove(o);

                //// BUGGY
                //if (oa != null)
                //{
                //    var index = this.objectToObjectTracks.Keys.ToList().IndexOf(o);

                //    this.objectToObjectTracks.Remove(o);

                //    middleCenterTableLayoutPanel.Controls.Remove(oa);

                //    // Move all the object annotations following ot up one step
                //    for (var i = index; i < this.objectToObjectTracks.Keys.Count; i++)
                //    {
                //        var moveObjectAnnotation = this.objectToObjectTracks[this.objectToObjectTracks.Keys.ToList()[i]];
                //        middleCenterTableLayoutPanel.Controls.Remove(moveObjectAnnotation);
                //        middleCenterTableLayoutPanel.Controls.Add(moveObjectAnnotation, lastObjectCell.X, i);
                //    }

                //    middleCenterTableLayoutPanel.RowStyles.RemoveAt(middleCenterTableLayoutPanel.RowStyles.Count - 1);
                //    middleCenterTableLayoutPanel.RowCount = lastObjectCell.Y - 1;
                //    middleCenterTableLayoutPanel.Size = new System.Drawing.Size(970, 60 * middleCenterTableLayoutPanel.RowCount + 4);
                //    lastObjectCell.Y--;
                //}

                lastObjectCell.Y = 0;
                middleCenterTableLayoutPanel.Controls.Clear();
                foreach (Object obj in this.objectToObjectTracks.Keys)
                {
                    renderObjectAnnotation(this.objectToObjectTracks[obj]);
                }

                middleCenterPanel.Invalidate();
            }
        }

        private void renderObjectAnnotation(ObjectAnnotation objectAnnotation)
        {
            renderObjectAnnotationWithoutInvalidate(objectAnnotation);

            middleCenterPanel.Invalidate();
        }

        private void renderObjectAnnotationWithoutInvalidate(ObjectAnnotation objectAnnotation)
        {
            middleCenterTableLayoutPanel.RowCount = lastObjectCell.Y + 1;
            middleCenterTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            middleCenterTableLayoutPanel.Size = new System.Drawing.Size(970, 60 * middleCenterTableLayoutPanel.RowCount + 4);
            middleCenterTableLayoutPanel.Controls.Add(objectAnnotation, lastObjectCell.X, lastObjectCell.Y);
            objectAnnotation.Dock = DockStyle.Fill;

            lastObjectCell.Y = lastObjectCell.Y + 1;
        }

        internal void selectObject(Object o)
        {
            cancelSelectObject();

            //Remove any decoration of other objects
            foreach (Object other in objectToObjectTracks.Keys)
            {
                objectToObjectTracks[other].deselectDeco();
            }
            objectToObjectTracks[o].selectDeco();

            this.selectedObject = o;

            if (selectedObject != null)
            {
                selectObjContextPanel.Visible = true;
            }

            foreach (Button b in drawingButtonGroup)
            {
                selectButtonDrawing(b, drawingButtonGroup, false);
            }

            this.showInformation(o);
            this.showPredicates();

            polygonDrawing.Enabled = false;
            rectangleDrawing.Enabled = false;

            invalidatePictureBoard();
        }

        internal void addObjectAnnotation(Object o)
        {
            var objectAnnotation = new ObjectAnnotation(o, this, this.frameTrackBar.Minimum, this.frameTrackBar.Maximum);
            //objectAnnotations.Add(objectAnnotation);
            objectToObjectTracks[o] = objectAnnotation;

            //objectAnnotation.Location = lastObjectCell;
            //if (lastObjectCell.Y >= 0) return;

            renderObjectAnnotation(objectAnnotation);
        }
    }
}
