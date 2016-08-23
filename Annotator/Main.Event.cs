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
        internal Event selectedEvent = null;
        private Dictionary<Event, EventAnnotation> mapFromEventToEventAnnotations;

        private void InitEventAnnoComponent()
        {
            mapFromEventToEventAnnotations = new Dictionary<Event, EventAnnotation>();
        }

        public void setAnnotationText(String txt)
        {
            annotationText.Text = txt;
            this.Invalidate();
        }

        public string getAnnotationText()
        {
            return annotationText.Text;
        }

        //Add annotation 
        internal void addAnnotation(Event ev)
        {
            EventAnnotation annotation = new EventAnnotation(ev, this, sessionStart, sessionEnd);
            this.mapFromEventToEventAnnotations[ev] = annotation;
            currentSession.addEvent(ev);
            renderEventAnnotation(annotation);
        }

        private void renderEventAnnotation(EventAnnotation annotation)
        {
            annotation.Dock = DockStyle.Fill;
            middleBottomTableLayoutPanel.RowCount = lastAnnotationCell.Y + 1;
            middleBottomTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            middleBottomTableLayoutPanel.Size = new System.Drawing.Size(970, 60 * middleBottomTableLayoutPanel.RowCount + 4);
            middleBottomTableLayoutPanel.Controls.Add(annotation, lastAnnotationCell.X, lastAnnotationCell.Y);
            lastAnnotationCell.Y += 1;

        }

        internal void removeAnnotation(Event ev)
        {
            currentSession.removeEvent(ev);

            clearMidleBottomPanel();
            populateMiddleBottomPanel();

            annoRefView.Rows.Clear();
            this.Invalidate();
        }

        //Add annotation button
        private void addEventAnnotationBtn_Click(object sender, EventArgs e)
        {
            Event eventAnnotation = new Event(null, frameTrackBar.Minimum, frameTrackBar.Maximum, "");
            addAnnotation(eventAnnotation);
            this.logSession($"Event {eventAnnotation.id} added");
        }

        public int addRightBottomTableReference(int start, int end, String text, String refID, Color? c = null)
        {
            int rowIndex = annoRefView.Rows.Add(start, end, text, refID);

            if (c.HasValue)
            {
                annoRefView.Rows[rowIndex].DefaultCellStyle.BackColor = c.Value;
            }

            return rowIndex;
        }

        //Unselect all annotations
        public void unselectAnnotations()
        {
            clearRightBottomPanel();
            if (currentSession != null)
            {
                foreach (Event ev in currentSession.events)
                {
                    if (mapFromEventToEventAnnotations.ContainsKey(ev))
                    {
                        mapFromEventToEventAnnotations[ev].selected = false;
                        ev.resetTempo();
                        mapFromEventToEventAnnotations[ev].deselectDeco();
                    }
                }
            }
        }

        private void addObjRefToolStripMenuItem_Clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //addReferenceLabel();
            String refID = e.ClickedItem.ToString();
            int start = annotationText.SelectionStart;
            int end = annotationText.SelectionStart + annotationText.SelectionLength;
            String txt = annotationText.SelectedText;
            //MessageBox.Show(start + "," + end + "," + txt);

            if (selectedEvent != null)
            {
                selectedEvent.addTempoReference(start, end, refID);
                addRightBottomTableReference(start, end, txt, refID);
            }
        }


        private void addEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int start = annotationText.SelectionStart;
            int end = annotationText.SelectionStart + annotationText.SelectionLength;
            String txt = annotationText.SelectedText;

            if (selectedEvent != null)
            {
                selectedEvent.addTempoAction(start, end, txt);
                addRightBottomTableReference(start, end, txt, txt);
            }
        }


        private void annotationText_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentSession != null)
            {
                addObjRefToolStripMenuItem.DropDownItems.Clear();
                foreach (Object o in currentSession.getObjects())
                {
                    addObjRefToolStripMenuItem.DropDownItems.Add(o.id);
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (annotationText.Text != null && annotationText.Text.Length > 0)
                {
                    addObjRefToolStripMenuItem.Enabled = true;
                }

                else if (annotationText.Text != null && annotationText.Text.Length > 0)
                {
                    addObjRefToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void annoRefView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            annoRefView.Rows[e.RowIndex].Selected = true;
            annoRefView.Invalidate();
        }

        private void AnnoRefView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (selectedEvent != null && mapFromEventToEventAnnotations.ContainsKey(selectedEvent))
            {
                mapFromEventToEventAnnotations[selectedEvent].deleteTempoEventParticipantByRowIndex(e.Row.Index);
            }
        }


        internal void linkSubEvent(Event ev)
        {
            LinkEventForm linkEventForm = new LinkEventForm();
            linkEventForm.populate(ev, currentSession.events);
            linkEventForm.StartPosition = FormStartPosition.CenterParent;
            linkEventForm.ShowDialog(this);
        }
    }
}
