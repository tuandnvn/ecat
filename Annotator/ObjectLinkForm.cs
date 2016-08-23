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
            this.infoLbl.Text = "Link from object " + obj.id + " of session " + session.name + " at frame " + frameNo;
            this.sessionSelectComboBox.Items.AddRange(project.sessions.Select(s => (s.name.Equals(session.name) ? "(current session)" : s.name)).ToArray());
            this.sessionSelectComboBox.SelectedIndex = sessionIndex;
            this.objectSelectComboBox.Items.AddRange(session.getObjects().Select(o => o.id + (o.name.Equals("") ? "" : (" (\"" + o.name + "\")"))).ToArray());
            this.objectSelectComboBox.SelectedIndex = 0;
            this.qualifiedSelectComboBox.Items.AddRange(new object[] { true, false });
            this.qualifiedSelectComboBox.SelectedIndex = 0;
            this.linkComboBox.Items.AddRange(Options.getOption().objectPredicates.ToArray());
            this.linkComboBox.SelectedIndex = 0;
            renderPredicateList();
        }

        private void renderPredicateList()
        {
            this.existingPredicateListBox.Items.Clear();
            LinkMark lm = this.obj.getLink(frameNo);
            if (lm != null)
            {
                this.existingPredicateListBox.Items.AddRange(lm.predicateMarks.Select(s => s.ToString()).ToArray());
            }
        }

        private void addLinkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var qualified = (bool)this.qualifiedSelectComboBox.SelectedItem;
                var predicateType = (Predicate)this.linkComboBox.SelectedItem;

                if (!crossSessionChkBox.Checked)
                {
                    // Binary predicate
                    if (predicateType.combination.size == 2)
                    {
                        var otherObject = session.getObjects()[this.objectSelectComboBox.SelectedIndex];
                        session.addPredicate(frameNo, qualified, predicateType, new Object[] { obj, otherObject });
                    }

                    // Unary predicate
                    if (predicateType.combination.size == 1)
                    {
                        session.addPredicate(frameNo, qualified, predicateType, new Object[] { obj });
                    }
                }
                else
                {
                    var sessionIndex = this.sessionSelectComboBox.SelectedIndex;
                    var tempoSession = project.sessions[sessionIndex];
                    var otherObject = tempoSession.getObjects()[this.objectSelectComboBox.SelectedIndex];
                    session.addPredicate(frameNo, qualified, predicateType, new Object[] { obj, otherObject });
                }

                this.main.redrawObjectMarks();
                this.main.showPredicates();

                this.main.logSession($"Predicate added at frame {frameNo} for object {obj.id}");
            }
            catch (Exception exc)
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
                var qualified = (bool)this.qualifiedSelectComboBox.SelectedItem;
                var predicateType = (Predicate)this.linkComboBox.SelectedItem;

                if (!crossSessionChkBox.Checked)
                {
                    if (predicateType.combination.size == 1)
                    {
                        this.predicateLbl.Text = new PredicateMark(0, qualified, predicateType, session, new Object[] { obj }).ToString();
                        this.objectSelectComboBox.Enabled = false;
                    }
                    else if (predicateType.combination.size == 2)
                    {
                        var otherObject = session.getObjects()[this.objectSelectComboBox.SelectedIndex];
                        this.predicateLbl.Text = new PredicateMark(0, qualified, predicateType, session, new Object[] { obj, otherObject }).ToString();
                        this.objectSelectComboBox.Enabled = true;
                    }
                }
                else
                {
                    var sessionIndex = this.sessionSelectComboBox.SelectedIndex;
                    var tempoSession = project.sessions[sessionIndex];
                    var otherObject = tempoSession.getObjects()[this.objectSelectComboBox.SelectedIndex];
                    this.predicateLbl.Text = new PredicateMark(0, qualified, predicateType, session, new Object[] { obj, otherObject }).ToString();
                }

            }
            catch (Exception exc)
            {
                this.predicateLbl.Text = "";
            }
        }

        //private String getLiteralForm(string id, string name, bool qualified, Predicate predicateType)
        //{
        //    String q = predicateType.predicate + "( " + id + (name.Equals("") ? "" : (" (\"" + name + "\")")) + " )";
        //    if (!qualified)
        //    {
        //        q = "NOT( " + q + " )";
        //    }
        //    return q;
        //}

        //private String getLiteralForm(string id, string name, string oidAndName, bool qualified, Predicate predicateType)
        //{
        //    String q = "";

        //    if (predicateType.combination.values.SequenceEqual(new int[2] { 1, 2 }))
        //        q = predicateType.predicate + "( " + id + (name.Equals("") ? "" : (" (\"" + name + "\")")) + ", " + oidAndName + " )";
        //    if (predicateType.combination.values.SequenceEqual(new int[2] { 2, 1 }))
        //        q = predicateType.predicate + "( " + oidAndName + ", " + id + (name.Equals("") ? "" : (" (\"" + name + "\")")) + " )";

        //    if (!qualified)
        //    {
        //        q = "NOT( " + q + " )";
        //    }
        //    return q;
        //}

        //private String getLiteralForm(string id, string name, string osession, string oidAndName, bool qualified, Predicate predicateType)
        //{
        //    String q = "";

        //    if (predicateType.combination.values.SequenceEqual(new int[2] { 1, 2 }))
        //        q = predicateType.predicate + "( " + id + (name.Equals("") ? "" : (" (\"" + name + "\")")) + ", " + osession + "/" + oidAndName + " )";
        //    if (predicateType.combination.values.SequenceEqual(new int[2] { 2, 1 }))
        //        q = predicateType.predicate + "( " + osession + "/" + oidAndName + ", " + id + (name.Equals("") ? "" : (" (\"" + name + "\")")) + " )";

        //    if (!qualified)
        //    {
        //        q = "NOT( " + q + " )";
        //    }
        //    return q;
        //}

        private void removePredBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = existingPredicateListBox.SelectedIndex;

            if (selectIndex != -1)
            {
                this.obj.session.removePredicate((string)existingPredicateListBox.SelectedItem);
            }
            renderPredicateList();

            this.main.logSession($"Predicate removed at frame {frameNo} for object {obj.id}");
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

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
