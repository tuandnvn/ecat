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
    public partial class ObjectLinkForm : Form
    {
        private Main main;
        private Project project;
        private Session session;

        /// <summary>
        /// Session index in project
        /// </summary>
        private int sessionIndex;

        private Object obj;
        private int frameNo;

        public ObjectLinkForm(Main main, Project project, Session session, Object obj, int frameNo)
        {
            InitializeComponent();

            this.main = main;
            this.project = project;
            this.session = session;
            this.obj = obj;
            this.frameNo = frameNo;
            this.sessionIndex = project.sessions.IndexOf(session);
            if (sessionIndex == -1)
            {
                MessageBox.Show("Cross session only works inside one project!");
                this.Dispose();
                return;
            }
            this.infoLbl.Text = "Add link from object " + obj.id + " of session " + session.sessionName;
            this.sessionSelectComboBox.Items.AddRange( project.sessions.Select(s => (s.sessionName.Equals(session.sessionName) ? "(current session)": s.sessionName) ).ToArray() );
            this.sessionSelectComboBox.SelectedIndex = sessionIndex;
            this.objectSelectComboBox.Items.AddRange(session.getObjects().Select( o => o.id ).ToArray() );
            this.qualifiedSelectComboBox.Items.AddRange(new object[] { true, false });
            this.linkComboBox.Items.AddRange(Options.getOption().objectLinkTypes.ToArray());
        }

        private void addLinkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var oid = (string)this.objectSelectComboBox.SelectedItem;
                var qualified = (bool)this.qualifiedSelectComboBox.SelectedItem;
                var predicateType = (string)this.linkComboBox.SelectedItem;

                if (!crossSessionChkBox.Checked)
                {
                    obj.setLink(frameNo, oid, qualified, predicateType);
                } else
                {
                    var sessionIndex = this.sessionSelectComboBox.SelectedIndex;
                    var tempoSession = project.sessions[sessionIndex];
                    obj.setLink(frameNo, tempoSession.sessionName, oid, qualified, predicateType);
                }
                
                this.main.redrawObjectMarks();
            } catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Problem when adding link");
            }
            
            this.Hide();
        }

        private void crossSessionChkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.sessionSelectComboBox.Enabled = crossSessionChkBox.Checked;
        }

        private void sessionSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sessionIndex = this.sessionSelectComboBox.SelectedIndex;

            var tempoSession = project.sessions[sessionIndex];

            tempoSession.loadIfNotLoaded();
            this.objectSelectComboBox.Text = "";
            this.objectSelectComboBox.Items.Clear();
            this.objectSelectComboBox.Items.AddRange(tempoSession.getObjects().Select(o => o.id).ToArray());
        }
    }
}
