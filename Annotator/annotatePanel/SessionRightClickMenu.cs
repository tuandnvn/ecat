using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class SessionRightClickMenu
    {
        LeftMostPanel leftMostPanel;

        public SessionRightClickMenu(IContainer container, LeftMostPanel leftMostPanel) : base(container)
        {
            InitializeComponent();
            this.leftMostPanel = leftMostPanel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSessionMenuItem_Click(object sender, EventArgs e)
        {
            //Check selected node:
            Session chosenSession = null;
            if (treeView.SelectedNode.Text[0] == '*')
            {
                chosenSession = selectedProject.getSession(treeView.SelectedNode.Text.Substring(1));
            }
            else
            {
                chosenSession = selectedProject.getSession(treeView.SelectedNode.Text);
            }

            // Save current session if it is edited
            if (currentSession != null && chosenSession != null && currentSession.sessionName != chosenSession.sessionName)
            {
                if (currentSession.getEdited())
                {
                    currentSession.setEdited(false);
                    treeView.BeginUpdate();
                    currentSessionNode.Text = currentSessionNode.Text.Substring(1);
                    treeView.EndUpdate();

                    var result = MessageBox.Show(("Session " + currentSession.sessionName + " currently editing, Do you want to save this session?"), "Save session", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cleanCurrentSession();
                    }
                    else if (result == DialogResult.No)
                    {
                        cleanSessionUI();
                    }
                }
            }

            // Set current session = chosen session
            if (chosenSession != null && !chosenSession.getEdited())
            {
                chosenSession.setEdited(true);
                currentSessionNode = treeView.SelectedNode;
                currentSession = chosenSession;
                currentSession.loadIfNotLoaded();
                currentSessionNode.Text = "*" + currentSessionNode.Text;

                frameTrackBar.Value = frameTrackBar.Minimum;
                this.Text = "Project " + selectedProject.getProjectName() + " selected, edited session = " + chosenSession.sessionName;
            }

            //Set comboBox:
            String[] viewsList = chosenSession.getViews();
            //MessageBox.Show(viewsList.Length + "");
            for (int i = 0; i < viewsList.Length; i++)
            {
                playbackFileComboBox.Items.Add(viewsList[i]);
            }

            if (playbackFileComboBox.Items.Count > 0)
            {
                playbackFileComboBox.SelectedIndex = 0;
                playbackFileComboBox.Enabled = true;
                frameTrackBar.Enabled = true;
                addEventAnnotationBtn.Enabled = true;
                //pictureBox1.BackgroundImage = null;

                // All toolstrips of file inside session are enables
                toggleFileToolStripsOfSession(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSessionMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode nodeS = treeView.SelectedNode;
            //MessageBox.Show(selectedProject.getSessionN() + "");
            if (currentSession.getEdited())
            {
                cleanCurrentSession();
            }

            toggleFileToolStripsOfSession(false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteSessionMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(item.ToString());     
            TreeNode sessionToDeleteName = treeView.SelectedNode;
            String sName = sessionToDeleteName.Text;
            TreeNode projectNode = sessionToDeleteName.Parent;
            Project project = workspace.getProject(projectNode.Text);
            if (MessageBox.Show("Confirm session removal (exclude from project): " + sName + " from " + project.getProjectName(), "Delete session", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //1)Remove session from project:
                project.removeSession(sName);
                //2)Remove session from treeView:
                treeView.BeginUpdate();
                foreach (TreeNode currentSessionNode in selectedProjectNode.Nodes)
                {
                    if (currentSessionNode.Text.Equals(sName))
                    {
                        //MessageBox.Show("Removing " + sName + " from" + project.getProjectName());
                        selectedProjectNode.Nodes.Remove(currentSessionNode);
                        break;
                    }
                }
                treeView.EndUpdate();
            }

            //Disable button2:
            addEventAnnotationBtn.Enabled = false;
            newObjectContextPanel.Visible = false;
            clearPlaybackFileComboBox();
            clearRightBottomPanel();
            pictureBoard.Image = null;
            startPoint = endPoint;
            currentVideo = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addFileToSessionMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                if (leftMostPanel.main.currentSession != null)
                {
                    String fullFileName = openFileDialog.FileName;
                    copyFileIntoLocalSession(fullFileName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshSessionMenuItem_Click(object sender, EventArgs e)
        {
            if (leftMostPanel.main.currentSession == null)
            { return; }

            //Check files in current Session folder
            String[] files = Directory.GetFiles(workspace.getLocationFolder() + Path.DirectorySeparatorChar +
                leftMostPanel.main.currentSession.getProject() + Path.DirectorySeparatorChar + leftMostPanel.main.currentSession.sessionName);

            TreeNode[] arrayFiles = new TreeNode[files.Length];
            for (int j = 0; j < arrayFiles.Length; j++)
            {
                arrayFiles[j] = new TreeNode(files[j].Split(Path.DirectorySeparatorChar)[files[j].Split(Path.DirectorySeparatorChar).Length - 1]);
                arrayFiles[j].ImageIndex = 2;
                arrayFiles[j].SelectedImageIndex = arrayFiles[j].ImageIndex;
            }

            treeView.BeginUpdate();
            currentSessionNode.Nodes.Clear();
            currentSessionNode.Nodes.AddRange(arrayFiles);
            treeView.EndUpdate();
        }

    }
}
