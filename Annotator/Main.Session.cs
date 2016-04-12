using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {
        private void editSessionMenuItem_Click(object sender, EventArgs e)
        {
            //Check selected node:
            //MessageBox.Show(treeView.SelectedNode.ToString());
            Session chosenSession = selectedProject.getSession(treeView.SelectedNode.ToString().Substring(10));

            if (currentSession != null && currentSession.getSessionName() != chosenSession.getSessionName())
            {
                if (currentSession.getEdited())
                {
                    //MessageBox.Show(checkSession.getSessionName() + checkSession.getEdited());
                    currentSession.setEdited(false);
                    treeView.BeginUpdate();
                    currentSessionNode.Text = currentSessionNode.Text.Substring(1);
                    treeView.EndUpdate();
                    if (MessageBox.Show(("Session " + currentSession.getSessionName() + " currently editing, Do you want to save this session?"), "Save session", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        currentSession.saveSession();
                    }
                }
            }

            if (chosenSession != null && !chosenSession.getEdited())
            {
                chosenSession.setEdited(true);
                currentSessionNode = treeView.SelectedNode;
                currentSession = chosenSession;
                currentSessionNode.Text = "*" + currentSessionNode.Text;
                this.Text = "Project " + selectedProject.getProjectName() + " selected, edited session = " + chosenSession.getSessionName();
            }


            //Set comboBox:
            String[] viewsList = chosenSession.getViews();
            //MessageBox.Show(viewsList.Length + "");
            for (int i = 0; i < viewsList.Length; i++)
            {
                comboBox1.Items.Add(viewsList[i]);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = true;
                frameTrackBar.Enabled = true;
                addEventAnnotationBtn.Enabled = true;
                //pictureBox1.BackgroundImage = null;
            }
        }

        //Save option choosed
        private void saveSessionMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode nodeS = treeView.SelectedNode;
            Session s = null;
            for (int i = 0; i < selectedProject.getSessionN(); i++)
            {
                s = selectedProject.getSession(i);
                //MessageBox.Show(selectedProject.getSessionN() + "");
                if (s.getEdited())
                {
                    //MessageBox.Show(currentSession.getSessionName());
                    //Save
                    s.saveSession();
                    //Enable Edit
                    TreeNode t = treeView.SelectedNode;
                    if (t.Text.Contains("*"))
                        t.Text = t.Text.Substring(1);
                    s.setEdited(false);
                    this.Text = "Project " + selectedProject.getProjectName() + " selected";
                    clearComboBox1();
                    pictureBoard.Image = null;
                    clearMiddleCenterPanel();
                    clearMidleBottomPanel();
                    clearRightBottomPanel();
                    startPoint = endPoint;
                    newObjectContextPanel.Visible = false;
                    currentVideo = null;
                    addEventAnnotationBtn.Enabled = false;
                }
            }
        }

        public void clearMiddleCenterPanel()
        {
            middleCenterPanel.Controls.Clear();
            lastObjectTrack = new Point(94, 0);
        }

        public void clearMidleBottomPanel()
        {
            middleBottomPanel.Controls.Clear();
            middleBottomPanel.Controls.Add(addEventAnnotationBtn);
            lastAnnotation = new Point(94, 0);
        }

        public void populateMiddleCenterPanel()
        {
            foreach (Object o in currentSession.getObjects())
            {
                addObjectAnnotation(o);
            }
        }

        public void populateMiddleBottomPanel()
        {
            foreach (Event ev in currentSession.events)
            {
                //a.setID(0);
                addAnnotation(ev);
            }
        }

        public void clearRightBottomPanel()
        {
            annoRefView.Rows.Clear();
        }

        //"Delete" option for choosed session item
        private void deleteSessionMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(item.ToString());     
            TreeNode sessionToDeleteName = treeView.SelectedNode;
            String sName = sessionToDeleteName.ToString().Substring(10);
            TreeNode projectNode = sessionToDeleteName.Parent;
            Project project = workspace.getProject(projectNode.ToString().Substring(10));
            if (MessageBox.Show("Confirm session removal(exclude from project): " + sName + " from " + project.getProjectName(), "Delete session", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //1)Remove session from project:
                project.removeSession(sName);
                //2)Remove session from treeView:
                treeView.BeginUpdate();
                TreeNodeCollection nodes = getTreeViewNodes();
                foreach (TreeNode currentProject in nodes)
                {
                    if (currentProject.ToString().Contains(project.getProjectName()))
                    {
                        foreach (TreeNode currentSessionNode in currentProject.Nodes)
                        {
                            if (currentSessionNode.ToString().Contains(sName))
                            {
                                //MessageBox.Show("Removing " + sName + " from" + project.getProjectName());
                                currentProject.Nodes.Remove(currentSessionNode);
                                break;
                            }
                        }
                    }
                }
                treeView.EndUpdate();
            }

            //Disable button2:
            addEventAnnotationBtn.Enabled = false;
            newObjectContextPanel.Visible = false;
            clearComboBox1();
            clearRightBottomPanel();
            pictureBoard.Image = null;
            startPoint = endPoint;
            currentVideo = null;
        }

        //Add video to session
        private void addSessionMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                if ( currentSession != null )
                {
                    String fullFileName = openFileDialog.FileName;
                    copyFileIntoLocalSession(fullFileName);

                    
                }
            }
        }

        /// <summary>
        /// Copy a file into the current session
        /// </summary>
        /// <param name="fileName">Original full path of file</param>
        /// <returns>The full path of copied file in the current session</returns>
        internal string copyFileIntoLocalSession(string fileName)
        {
            string relFileName = fileName.Split(Path.DirectorySeparatorChar)[fileName.Split(Path.DirectorySeparatorChar).Length - 1];
            //MessageBox.Show("inputFile = " + openFileDialog1.FileName);
            string dstFileName = selectedProject.getLocation() + Path.DirectorySeparatorChar + selectedProject.getProjectName() + Path.DirectorySeparatorChar + currentSession.getSessionName() + Path.DirectorySeparatorChar + relFileName;
            //MessageBox.Show("outputFile = " + dstFileName);
            //If file doesnt exist in session folder add file to session folder
            if (!File.Exists(dstFileName))
                File.Copy(fileName, dstFileName);
            //Check if file contains video stream:

            if (!currentSession.checkFileInSession(relFileName) && !relFileName.Contains("files.param"))
            {
                currentSession.addFile(dstFileName);
                //If file didnt exist in treeView update treeView
                treeView.BeginUpdate();
                TreeNode fileNode = new TreeNode(relFileName);
                fileNode.ImageIndex = 2;
                fileNode.SelectedImageIndex = fileNode.ImageIndex;
                currentSessionNode.Nodes.Add(fileNode);
                treeView.EndUpdate();

                //Add view to comboBox1:
                comboBox1.Items.Add(relFileName);
            }
            return dstFileName;
        }

        /// <summary>
        /// Copy a file into the current session with a new name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="newRelFileName"></param>
        /// <returns></returns>
        internal string copyFileIntoLocalSession(string fileName, string newRelFileName)
        {
            //MessageBox.Show("inputFile = " + openFileDialog1.FileName);
            string dstFileName = selectedProject.getLocation() + Path.DirectorySeparatorChar + selectedProject.getProjectName() + Path.DirectorySeparatorChar + currentSession.getSessionName() + Path.DirectorySeparatorChar + newRelFileName;
            //MessageBox.Show("outputFile = " + dstFileName);
            //If file doesnt exist in session folder add file to session folder
            if (!File.Exists(dstFileName))
                File.Copy(fileName, dstFileName);
            //Check if file contains video stream:

            if (!currentSession.checkFileInSession(newRelFileName) && !newRelFileName.Contains("files.param"))
            {
                currentSession.addFile(dstFileName);
                //If file didnt exist in treeView update treeView
                treeView.BeginUpdate();
                TreeNode fileNode = new TreeNode(newRelFileName);
                fileNode.ImageIndex = 2;
                fileNode.SelectedImageIndex = fileNode.ImageIndex;
                currentSessionNode.Nodes.Add(fileNode);
                treeView.EndUpdate();

                //Add view to comboBox1:
                comboBox1.Items.Add(newRelFileName);
            }
            return dstFileName;
        }
    }
}
