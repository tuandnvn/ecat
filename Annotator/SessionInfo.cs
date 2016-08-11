using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Annotator
{
    public partial class SessionInfo : Form
    {
        private Main main = null;
        private String projectName = "";
        internal bool sessionNameAcceptable = false;

        public SessionInfo(Main frm1, String projectName)
        {
            InitializeComponent();
            this.main = frm1;
            this.ActiveControl = this.sessionNameTxtBox;
            this.projectName = projectName;//"cut-off TreeNode: "
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main.setNewSession(false);
            this.Close();
        }
        //Parse session name:
        private bool parseSessionName()
        {
            if (warningTxt.Visible)
            {
                return false;
            }

            return true;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (warningTxt.Visible == false || (warningTxt.ForeColor == Color.Green))
            {
                Project project = main.getProjectFromWorkspace(projectName);
                String sessionLocation = project.locationFolder + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + sessionNameTxtBox.Text;
                if (!Directory.Exists(sessionLocation))
                    Directory.CreateDirectory(sessionLocation);

                main.addNewSessionToProject(projectName, sessionNameTxtBox.Text);
                main.setNewSession(false);
                this.Close();
            }
        }

        private void sessionNameTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (sessionNameTxtBox.Text != null && sessionNameTxtBox.Text.Trim().Length >= 3)
            {
                warningTxt.Visible = false;
                okButton.Visible = true;
            }
            else
            {
                warningTxt.Text = "Incorrect session name (at least three letters)";
                warningTxt.ForeColor = Color.Red;
                warningTxt.Visible = true;
                okButton.Text = "OK";
                okButton.Visible = false;
                return;
            }

            Project project = main.getProjectFromWorkspace(projectName);
            String sessionLocation = project.locationFolder + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + sessionNameTxtBox.Text;
            //MessageBox.Show(sessionLocation + Directory.Exists(sessionLocation));

            bool existSession = false;
            foreach (TreeNode sessionNode in main.currentProjectNode.Nodes)
            {
                if (sessionNode.Text.Equals(sessionNameTxtBox.Text))
                {
                    existSession = true;
                    break;
                }
            }

            if (existSession)
            {
                warningTxt.Text = "Session " + sessionNameTxtBox.Text + " already exists in project " + projectName;
                okButton.Visible = false;
                warningTxt.Visible = true;
            }
            else
            {
                warningTxt.Visible = false;
            }
            if (Directory.Exists(sessionLocation) && !existSession)
            {
                //Directory.CreateDirectory(workspace.getLocationFolder() + Path.DirectorySeparatorChar + project.getProjectName() + Path.DirectorySeparatorChar + sessionName);
                warningTxt.Text = "Session folder already exists";
                warningTxt.Visible = true;
                warningTxt.ForeColor = Color.Green;
                okButton.Text = "Use existing directory as new session";
            }
            else
            {
                okButton.Text = "OK";
            }
        }

        private void SessionInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            main.setNewSession(false);
        }

    }
}
