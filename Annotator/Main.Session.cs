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
            Session choosedSession = selectedProject.getSession(treeView.SelectedNode.ToString().Substring(10));


            //if (comboBox1.Items.Count > 0)
            //    comboBox1.SelectedIndex = 0;
            //MessageBox.Show(choosedSession.getSessionName() + ", getEdited == " + choosedSession.getEdited());
            //Check all sessions inside selected project   
            Session checkSession = null;
            foreach (TreeNode projectNode in treeView.Nodes)
            {
                if (projectNode.ToString().Contains(selectedProject.getProjectName()))
                {
                    //MessageBox.Show(projectNode.ToString());
                    foreach (TreeNode sessionNode in projectNode.Nodes)
                    {
                        String nodeName = sessionNode.ToString().Substring(10);
                        if (!nodeName.Contains(choosedSession.getSessionName()) && nodeName.Contains("*"))
                            nodeName = nodeName.Substring(1);
                        checkSession = selectedProject.getSession(nodeName);
                        if (checkSession.getSessionName() != choosedSession.getSessionName())
                        {
                            if (checkSession.getEdited())
                            {
                                //MessageBox.Show(checkSession.getSessionName() + checkSession.getEdited());
                                checkSession.setEdited(false);
                                treeView.BeginUpdate();
                                sessionNode.Text = sessionNode.Text.Substring(1);
                                treeView.EndUpdate();
                                if (MessageBox.Show(("Session " + checkSession.getSessionName() + " currently editing, Do you want to save this session?"), "Save session", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    checkSession.saveSession();
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (checkSession != null)
            {

            }
            if (choosedSession != null && !choosedSession.getEdited())
            {
                choosedSession.setEdited(true);
                TreeNode n = treeView.SelectedNode;
                n.Text = "*" + n.Text;
                this.Text = "Project " + selectedProject.getProjectName() + " selected, edited session = " + choosedSession.getSessionName();
            }


            //Set comboBox:
            String[] viewsList = choosedSession.getViews();
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
            currentSession = null;
            for (int i = 0; i < selectedProject.getSessionN(); i++)
            {
                currentSession = selectedProject.getSession(i);
                //MessageBox.Show(selectedProject.getSessionN() + "");
                if (currentSession.getEdited())
                {
                    //MessageBox.Show(currentSession.getSessionName());
                    //Save
                    currentSession.saveSession();
                    //Enable Edit
                    TreeNode t = treeView.SelectedNode;
                    if (t.Text.Contains("*"))
                        t.Text = t.Text.Substring(1);
                    currentSession.setEdited(false);
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
            foreach (ObjectAnnotation o in currentSession.objectTracks)
            {
                addObjectTracking(o);
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
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                //Add file to session:
                Session checkSession = null;
                foreach (TreeNode projectNode in treeView.Nodes)
                {
                    if (projectNode.ToString().Contains(selectedProject.getProjectName()))
                    {
                        //MessageBox.Show(projectNode.ToString());
                        foreach (TreeNode sessionNode in projectNode.Nodes)
                        {
                            String nodeName = sessionNode.ToString().Substring(10);
                            if (nodeName.Contains("*"))
                            {
                                nodeName = nodeName.Substring(1);
                                checkSession = selectedProject.getSession(nodeName);
                                break;
                            }
                        }
                    }
                }

                String fileName = openFileDialog1.FileName.Split(Path.DirectorySeparatorChar)[openFileDialog1.FileName.Split(Path.DirectorySeparatorChar).Length - 1];
                //MessageBox.Show("inputFile = " + openFileDialog1.FileName);
                String dstFileName = selectedProject.getLocation() + Path.DirectorySeparatorChar + selectedProject.getProjectName() + Path.DirectorySeparatorChar + checkSession.getSessionName() + Path.DirectorySeparatorChar + fileName;
                //MessageBox.Show("outputFile = " + dstFileName);
                //If file doesnt exist in session folder add file to session folder
                if (!File.Exists(dstFileName))
                    File.Copy(openFileDialog1.FileName, dstFileName);
                //Check if file contains video stream:

                //MessageBox.Show("*.avi file loaded! :" + fileName);
                if (!checkSession.checkFileInSession(fileName) && !fileName.Contains("files.param"))
                {
                    checkSession.addFile(dstFileName);
                    //If file didnt exist in treeView update treeView
                    treeView.BeginUpdate();
                    TreeNode fileNode = new TreeNode(fileName);
                    fileNode.ImageIndex = 2;
                    fileNode.SelectedImageIndex = fileNode.ImageIndex;
                    treeView.SelectedNode.Nodes.Add(fileNode);
                    treeView.EndUpdate();

                    //Add view to comboBox1:
                    comboBox1.Items.Add(fileName);

                }
            }
        }
    }
}
