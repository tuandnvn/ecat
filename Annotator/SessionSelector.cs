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
    public partial class SessionSelector : Form
    {
        private Main main;
        private Project project;
        private Session session;
        /// <summary>
        /// Session index in project
        /// </summary>
        private int sessionIndex;

        public SessionSelector(Main main, Project project, Session session)
        {
            InitializeComponent();

            this.main = main;
            this.project = project;
            this.session = session;
            this.sessionIndex = project.sessions.IndexOf(session);
            if (sessionIndex == -1)
            {
                MessageBox.Show("You can only copy objects from session of the same project");
                this.Dispose();
                return;
            }
            this.sessionSelectComboBox.Items.AddRange(project.sessions.Select(s => (s.name.Equals(session.name) ? "(current session)" : s.name)).ToArray());
            this.sessionSelectComboBox.SelectedIndex = sessionIndex;
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
            var otherSessionIndex = this.sessionSelectComboBox.SelectedIndex;

            if (sessionIndex == otherSessionIndex)
            {
                MessageBox.Show("You could not copy objects from the same session");
            } else
            {
                main.copyFromSession(sessionIndex, otherSessionIndex);
            }
            
            this.Dispose();
        }
    }
}
