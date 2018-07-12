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

        public void enableAnnotationText(String txt)
        {
            annotationText.Text = txt;
            annotationText.Enabled = true;
            this.Invalidate();
        }

        public void disableAnnotationText()
        {
            annotationText.Text = "";
            annotationText.Enabled = false;
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
            disableAnnotationText();

            annoRefView.Rows.Clear();
            this.Invalidate();
            this.logSession($"Event {ev.id} removed");
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

        internal void findObjectForEvent(Event ev)
        {
            string annotationText = ev.text;
            currentSession.resetTempoEmpty(ev);
            annoRefView.Rows.Clear();
            annoRefView.Refresh();
            var foundObjects = currentSession.findObjectsByNames(ev, getAnnotationText());

            foreach (var o in foundObjects)
            {
                ev.addTempoReference(o.Item1, o.Item2, o.Item3);
                addRightBottomTableReference(o.Item1, o.Item2, annotationText.Substring(o.Item1, o.Item2 - o.Item1), o.Item3);
            }

            this.logSession($"{foundObjects.Count} event references are added.");
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

        internal void linkSubEvent(Event ev)
        {
            LinkEventForm linkEventForm = new LinkEventForm();
            linkEventForm.populate(ev, currentSession.events);
            linkEventForm.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = linkEventForm.ShowDialog(this);

            // We bind DialogResult = OK to Add button
            if (result == DialogResult.OK)
            {
                logSession($"Add a link for event {ev.id}");
            }
        }

        private void annoRefView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                annoRefView.Rows[e.RowIndex].Selected = true;
                annoRefView.Invalidate();
            } catch (System.ArgumentOutOfRangeException exc)
            {
                // Click on the header part
            }
        }

        private void annoRefView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Console.WriteLine("annoRefView_UserDeletedRow");

            if (selectedEvent != null && mapFromEventToEventAnnotations.ContainsKey(selectedEvent))
            {
                mapFromEventToEventAnnotations[selectedEvent].deleteTempoEventParticipantByRowIndex(e.Row.Index);
            }
        }
    }
}
