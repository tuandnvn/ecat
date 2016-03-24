using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class LinkEventForm : Form
    {
        private Event ev;
        private List<Event> allEvents;

        public LinkEventForm()
        {
            InitializeComponent();
        }

        public void populate(Event ev, List<Event> allEvents)
        {
            this.ev = ev;
            this.allEvents = allEvents;
            eventId.Text = ev.id;
            eventTxt.Text = ev.text;
            linkEventTypeComboBox.Items.AddRange(Enum.GetValues(typeof(Event.EventLinkType)).Cast<object>().ToArray());
            linkToEventIdComboBox.Items.AddRange(allEvents.FindAll(x => x.id != ev.id).Select(e => e.id).ToArray());
        }

        private void linkToEventIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event selectedEvent = allEvents.Find(x => x.id == (string)linkToEventIdComboBox.SelectedItem);
            linkToEventTxt.Text = selectedEvent.text;
        }

        private void addLinkEventBtn_Click(object sender, EventArgs e)
        {
            Event selectedEvent = allEvents.Find(x => x.id == (string)linkToEventIdComboBox.SelectedItem);
            var selectedLinkType = linkEventTypeComboBox.SelectedItem;
            if (selectedEvent != null && linkEventTypeComboBox.SelectedItem != null)
            {
                ev.addLinkTo(selectedEvent.id, (Event.EventLinkType)selectedLinkType);
                this.Dispose();
            }
        }
    }
}
