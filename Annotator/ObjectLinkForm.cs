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
            this.infoLbl.Text = "Link from object " + obj.id + " of session " + session.sessionName + " at frame " + frameNo;
            this.sessionSelectComboBox.Items.AddRange(project.sessions.Select(s => (s.sessionName.Equals(session.sessionName) ? "(current session)" : s.sessionName)).ToArray());
            this.sessionSelectComboBox.SelectedIndex = sessionIndex;
            this.objectSelectComboBox.Items.AddRange(session.getObjects().Select(o => o.id + (o.name.Equals("") ? "" : (" (\"" + o.name + "\")"))).ToArray());
            this.objectSelectComboBox.SelectedIndex = 0;
            this.qualifiedSelectComboBox.Items.AddRange(new object[] { true, false });
            this.qualifiedSelectComboBox.SelectedIndex = 0;
            this.linkComboBox.Items.AddRange(Options.getOption().objectLinkTypes.ToArray());
            this.linkComboBox.SelectedIndex = 0;
            renderPredicateList();
        }

        private void renderPredicateList()
        {
            this.existingPredicateListBox.Items.Clear();
            LinkMark lm = this.obj.getLink(frameNo);
            if (lm != null)
            {
                this.existingPredicateListBox.Items.AddRange(lm.binaryPredicates.Select(s => lm.getLiteralForm(s)).ToArray());
                this.existingPredicateListBox.Items.AddRange(lm.crossSessionBinaryPredicates.Select(s => lm.getLiteralForm(s)).ToArray());
            }
        }

        private void addLinkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var qualified = (bool)this.qualifiedSelectComboBox.SelectedItem;
                var predicateType = (string)this.linkComboBox.SelectedItem;

                if (!crossSessionChkBox.Checked)
                {
                    var oid = session.getObjects()[this.objectSelectComboBox.SelectedIndex].id;
                    obj.addLink(frameNo, oid, qualified, predicateType);
                } else
                {
                    var sessionIndex = this.sessionSelectComboBox.SelectedIndex;
                    var tempoSession = project.sessions[sessionIndex];
                    var oid = tempoSession.getObjects()[this.objectSelectComboBox.SelectedIndex].id;
                    obj.addLink(frameNo, tempoSession.sessionName, oid, qualified, predicateType);
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
            if (crossSessionChkBox.Checked)
            {
                var sessionIndex = this.sessionSelectComboBox.SelectedIndex;

                var tempoSession = project.sessions[sessionIndex];

                tempoSession.loadIfNotLoaded();
                //this.objectSelectComboBox.Text = "";
                this.objectSelectComboBox.Items.Clear();
                this.objectSelectComboBox.Items.AddRange(tempoSession.getObjects().Select(o => o.id + (o.name.Equals("") ? "" : (" (\"" + o.name + "\")"))).ToArray());
                this.objectSelectComboBox.SelectedIndex = 0;

                updatePredicateForm();
            }
        }

        private void objectSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePredicateForm();
        }

        private void qualifiedSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePredicateForm();
        }

        private void linkComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePredicateForm();
        }

        private void updatePredicateForm()
        {
            try
            {
                var oid = (string)this.objectSelectComboBox.SelectedItem;
                var qualified = (bool)this.qualifiedSelectComboBox.SelectedItem;
                var predicateType = (string)this.linkComboBox.SelectedItem;

                if (!crossSessionChkBox.Checked)
                {
                    this.predicateLbl.Text = getLiteralForm(obj.id, obj.name, oid, qualified, predicateType);
                }
                else
                {
                    var sessionIndex = this.sessionSelectComboBox.SelectedIndex;
                    var tempoSession = project.sessions[sessionIndex];
                    this.predicateLbl.Text = getLiteralForm(obj.id, tempoSession.sessionName, oid, qualified, predicateType);
                }

            } catch (Exception exc)
            {
                this.predicateLbl.Text = "";
            }
        }

        private String getLiteralForm(string id, string name, string oid, bool qualified, string predicateType)
        {
            String q = predicateType + "( " + id + (name.Equals("") ? "" : (" (\"" + name + "\")")) + ", " + oid + " )";
            if (!qualified)
            {
                q = "NOT( " + q + " )";
            }
            return q;
        }

        private String getLiteralForm(string id, string name, string osession, string oid, bool qualified, string predicateType)
        {
            String q = predicateType + "( " + id + (name.Equals("") ? "" : (" (\"" + name + "\")")) + ", " + osession + "/" + oid + " )";
            if (!qualified)
            {
                q = "NOT( " + q + " )";
            }
            return q;
        }

        private void removePredBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = existingPredicateListBox.SelectedIndex;
            LinkMark lm = this.obj.getLink(frameNo);

            if (selectIndex != -1 && lm != null)
            {
                lm.binaryPredicates.RemoveWhere(t => lm.getLiteralForm(t).Equals((string)existingPredicateListBox.SelectedItem));
                lm.crossSessionBinaryPredicates.RemoveWhere(t => lm.getLiteralForm(t).Equals((string)existingPredicateListBox.SelectedItem));
            }
            renderPredicateList();
        }


        private void existingPredicateListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectIndex = existingPredicateListBox.SelectedIndex;
            if (selectIndex != -1)
            {
                removePredBtn.Enabled = true;
            }
            else
            {
                removePredBtn.Enabled = false;
            }
        }
    }
}
