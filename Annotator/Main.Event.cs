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
            Console.WriteLine("setAnnotationText " + txt);
            annotationText.Text = txt;
            this.Invalidate();
        }

        //Add annotation 
        internal void addAnnotation(Event ev)
        {
            EventAnnotation annotation = new EventAnnotation(ev, this, currentSession);
            this.mapFromEventToEventAnnotations[ev] = annotation;
            currentSession.addEvent(ev);
            annotation.Location = lastAnnotation;
            middleBottomPanel.Controls.Add(annotation);
            lastAnnotation.Y += annotation.Height + 5;
        }

        internal void removeAnnotation(Event ev)
        {
            currentSession.removeEvent(ev);

            EventAnnotation annotation = this.mapFromEventToEventAnnotations[ev];
            middleBottomPanel.Controls.Remove(annotation);

            foreach (EventAnnotation other in mapFromEventToEventAnnotations.Values)
            {
                if (other.Location.Y > annotation.Location.Y)
                {
                    other.Location = new Point(other.Location.X, other.Location.Y - annotation.Height - 5);
                }
            }

            lastAnnotation.Y -= annotation.Height + 5;

            annoRefView.Rows.Clear();
            this.Invalidate();
        }

        //Add annotation button
        private void button2_Click(object sender, EventArgs e)
        {
            Event annotation = new Event(null, frameTrackBar.Minimum, frameTrackBar.Maximum, "");
            addAnnotation(annotation);
        }


        public void addRightBottomTableReference(int start, int end, String text, String refID)
        {
            annoRefView.Rows.Add(start, end, text, refID);
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
                        mapFromEventToEventAnnotations[ev].setSelected(false);
                        mapFromEventToEventAnnotations[ev].deselectDeco();
                    }
                }
            }
        }

        private void toolStripMenuItem6_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //addReferenceLabel();
            String refID = e.ClickedItem.ToString();
            int start = annotationText.SelectionStart;
            int end = annotationText.SelectionStart + annotationText.SelectionLength;
            String txt = annotationText.SelectedText;
            //MessageBox.Show(start + "," + end + "," + txt);

            if (selectedEvent != null)
            {
                selectedEvent.addReference(start, end, refID);
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
                selectedEvent.addAction(start, end, txt);
                addRightBottomTableReference(start, end, txt, txt);
            }
        }


        private void annotationText_MouseDown(object sender, MouseEventArgs e)
        {
            addObjRefToolStripMenuItem.DropDownItems.Clear();
            foreach (Object o in currentSession.getObjects())
            {
                addObjRefToolStripMenuItem.DropDownItems.Add(o.id);
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

        internal void linkSubEvent(Event ev)
        {
            LinkEventForm linkEventForm = new LinkEventForm();
            linkEventForm.populate(ev, currentSession.events);
            linkEventForm.Show(this);
            linkEventForm.Location = new Point()
            {
                X = Math.Max(this.Location.X, this.Location.X + (this.Width - linkEventForm.Width) / 2),
                Y = Math.Max(this.Location.Y, this.Location.Y + (this.Height - linkEventForm.Height) / 2)
            };
        }
    }
}
