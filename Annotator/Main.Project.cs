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
        private void recordSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabs.SelectedIndex = 1;
        }

        private void newSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(selectedProjectName);
            if (!newSession)
            {
                addNewSession();
            }
        }

        //Select project from available projects in workspace
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if there is editing session:
            foreach (TreeNode projectNode in treeView.Nodes)
            {
                String sessionName = "";
                foreach (TreeNode sessionNode in projectNode.Nodes)
                {
                    sessionName = sessionNode.Text;
                    if (sessionName.Contains("*"))
                        sessionName = sessionName.Substring(1);
                    Project project = workspace.getProject(projectNode.Text);
                    Session session = project.getSession(sessionName);
                    if (session.getEdited())
                    {
                        MessageBox.Show("Cannot select project, session " + sessionName + " in project " + project.getProjectName() + " is editing");
                        return;
                    }
                }
            }
            ///////////////////////////////////
            //Get selected node from treeView:
            selectedProjectNode = treeView.SelectedNode;
            String prjName = selectedProjectNode.Text;
            treeView.BeginUpdate();
            foreach (TreeNode node in treeView.Nodes)
            {
                node.BackColor = Color.White;
            }
            
            selectedProjectNode.BackColor = Color.Silver;
            treeView.EndUpdate();
            selectedProject = workspace.getProject(prjName);
            selectedProject.setSelected(true);

            this.simpleEventDataCreateMenuItem.Enabled = true;
            this.Text = "Project " + selectedProject.getProjectName() + " selected";

            foreach (TreeNode node in treeView.Nodes)
            {
                if (node.Text != prjName)
                {
                    node.Collapse();
                }
                if (node.Text.Contains(prjName))
                    node.Expand();
            }
        }

        //Close project if selected
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.Text.Equals(selectedProject.getProjectName()))
            {
                //Check if project session is editing:
                //Check if there is editing session:
                foreach (TreeNode projectNode in treeView.Nodes)
                {
                    String sessionName = "";
                    foreach (TreeNode sessionNode in projectNode.Nodes)
                    {
                        sessionName = sessionNode.Text;
                        if (sessionName.Contains("*"))
                            sessionName = sessionName.Substring(1);
                        Project project = workspace.getProject(projectNode.Text);
                        Session session = project.getSession(sessionName);
                        if (session.getEdited())
                        {
                            MessageBox.Show("Cannot close project, session " + sessionName + " is editing");
                            return;
                        }
                    }
                }

                treeView.SelectedNode.BackColor = Color.White;
                selectedProject.setSelected(false);
                selectedProject = null;
                this.Text = "No project selected";
            }
        }
    }
}
