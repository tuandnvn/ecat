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
        public SessionInfo(Main frm1, String projectName)
        {
            InitializeComponent();
            this.frm1 = frm1;
            this.ActiveControl = this.textBox1;
            this.projectName = projectName.Substring(10);//"cut-off TreeNode: "
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm1.setNewSession(false);
            this.Close();
        }
        //Parse session name:
        private bool parseSessionName()
        {
            if (label3.Visible)
            {
                return false;
            }

            return true;
        }


        private String projectName = "";
        private Main frm1 = null;

        private void button3_Click(object sender, EventArgs e)
        {
            if (label3.Visible == false || (label3.ForeColor == Color.Green))
            {
                Project project = frm1.getProjectFromWorkspace(projectName);
                String sessionLocation = project.getLocation() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + textBox1.Text;
                if (!Directory.Exists(sessionLocation))
                    Directory.CreateDirectory(sessionLocation);

                frm1.addNewSessionToWorkspace(projectName, textBox1.Text);
                frm1.setNewSession(false);
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text.Trim().Length >= 3)
            {
                label3.Visible = false;
                button3.Visible = true;
            }
            else
            {
                label3.Text = "Incorrect project name(at least three letters)";
                label3.ForeColor = Color.Red;
                label3.Visible = true;
                button3.Text = "OK";
                button3.Visible = false;
                return;
            }
            //Check if there is already a directory named the same as the new session                
            Project project = frm1.getProjectFromWorkspace(projectName);
            String sessionLocation = project.getLocation() + Path.DirectorySeparatorChar + projectName + Path.DirectorySeparatorChar + textBox1.Text;
            //MessageBox.Show(sessionLocation + Directory.Exists(sessionLocation));

            bool exists = false;
            TreeNodeCollection nodes = frm1.getTreeViewNodes();
            foreach (TreeNode currentProject in nodes)
            {
                if (currentProject.ToString().Contains(projectName))
                {
                    foreach (TreeNode currentSession in currentProject.Nodes)
                    {
                        //MessageBox.Show(currentSession.ToString());
                        if (currentSession.ToString().Contains(textBox1.Text))
                        {
                            exists = true;
                            break;
                        }
                    }
                }
            }

            if (exists)
            {
                label3.Text = "Session " + textBox1.Text + " already exists in project " + projectName;
                button3.Visible = false;
                label3.Visible = true;
            }
            else
            {
                label3.Visible = false;
            }
            if (Directory.Exists(sessionLocation) && !exists)
            {
                //Directory.CreateDirectory(workspace.getLocationFolder() + Path.DirectorySeparatorChar + project.getProjectName() + Path.DirectorySeparatorChar + sessionName);
                label3.Text = "Session folder already exists";
                label3.Visible = true;
                label3.ForeColor = Color.Green;
                button3.Text = "Use existing directory as new session";
            }
            else
            {
                button3.Text = "OK";
            }
        }

        private void SessionInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm1.setNewSession(false);
        }

    }
}
