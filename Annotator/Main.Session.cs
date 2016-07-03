using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {

        BaseDepthReader depthReader;
        byte[] depthValuesToByte;
        Bitmap depthBitmap;

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
                        saveCurrentSession();
                    }
                    else if (result == DialogResult.No)
                    {
                        closeWithoutSaveCurrentSession();
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

            playbackFileComboBox.Items.Clear();

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

        private void toggleFileToolStripsOfSession(bool value)
        {
            addObjectToolStripMenuItem.Enabled = value;
            addRigsFromFileToolStripMenuItem.Enabled = value;
            removeToolStripMenuItem.Enabled = value;
        }

        private void playbackVideoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string videoFilename = playbackFileComboBox.SelectedItem.ToString();

                if (videoFilename.isVideoFile())
                {
                    depthReader = null;
                    loadVideo(videoFilename);

                    if (currentVideo != null)
                    {
                        endPoint = startPoint = new Point();
                        label3.Text = "Frame: " + frameTrackBar.Value;

                        Mat m = currentVideo.getFrame(0);
                        if (m != null)
                        {
                            pictureBoard.mat = m;
                            pictureBoard.Image = pictureBoard.mat.Bitmap;
                        }

                        setLeftTopPanel();
                    }
                }

                if (videoFilename.isDepthFile())
                {
                    currentVideo = null;
                    depthReader = currentSession.getDepth(videoFilename);

                    if (depthReader == null) return;

                    if (depthValuesToByte == null)
                    {
                        depthValuesToByte = new byte[depthReader.getWidth() * depthReader.getHeight() * 4];
                    }

                    if (depthBitmap == null)
                    {
                        depthBitmap = new Bitmap(depthReader.getWidth(), depthReader.getHeight(), PixelFormat.Format32bppRgb);
                    }

                    depthReader.readFrameAtTimeToBitmap(0, depthBitmap, depthValuesToByte, 8000.0f / 256);

                    if (depthBitmap != null)
                    {
                        pictureBoard.Image = depthBitmap;
                    }
                }

                frameTrackBar_ValueChanged(null, null);
            } catch (Exception exc)
            {
                Console.WriteLine("Select video file exception");
                MessageBox.Show("Exception in opening this file, please try another file", "File exception", MessageBoxButtons.OK);
            }
        }


        private void frameTrackBar_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = "Frame: " + frameTrackBar.Value;
            int frameStartWithZero = frameTrackBar.Value - 1;
            if (currentVideo != null)
            {
                Mat m = currentVideo.getFrame(frameStartWithZero);
                Console.WriteLine(frameStartWithZero);
                if (m != null)
                {
                    pictureBoard.mat = m;
                    pictureBoard.Image = pictureBoard.mat.Bitmap;
                }
                else
                {
                    Console.WriteLine("Could not get frame for " + frameStartWithZero);
                }
                runGCForImage();
            }

            if (depthReader != null)
            {
                int timeStepForFrame = (int)(currentSession.duration / currentSession.sessionLength);
                int timeFromStart = frameStartWithZero * timeStepForFrame;
                depthReader.readFrameAtTimeToBitmap(timeFromStart, depthBitmap, depthValuesToByte, 8000.0f / 256);

                if (depthBitmap != null)
                {
                    pictureBoard.Image = depthBitmap;
                }
                else
                {
                    Console.WriteLine("Could not get frame for " + frameStartWithZero);
                }

                runGCForImage();
            }
        }

        private void loadVideo(string videoFilename)
        {
            Application.DoEvents();
            currentVideo = currentSession.getVideo(videoFilename);

            if (currentVideo != null)
            {
                setMaximumFrameTrackBar(currentVideo.frameCount - 1);
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
                saveCurrentSession();
            }

            toggleFileToolStripsOfSession(false);
        }

        internal void saveCurrentSession()
        {
            currentSession.saveSession();
            Console.WriteLine("File list when saved");
            foreach (string fileName in currentSession.filesList)
            {
                Console.WriteLine(fileName);
            }
            cleanSessionUI();
        }

        private void closeWithoutSaveCurrentSession()
        {
            //Reload session
            currentSession.reloadAnnotation();
            cleanSessionUI();
        }

        private void cleanSessionUI()
        {
            TreeNode t = treeView.SelectedNode;
            if (t.Text.Contains("*"))
                t.Text = t.Text.Substring(1);
            currentSession.setEdited(false);
            this.Text = "Project " + selectedProject.getProjectName() + " selected";

            // Clean the playbackFileComboBox
            clearPlaybackFileComboBox();

            // Clean picture board frame
            pictureBoard.Image = null;

            // Clean object annotations
            clearMiddleCenterPanel();

            // Clean event annotation
            clearMidleBottomPanel();

            // Clean object properties
            clearRightCenterPanel();

            // Clean panel for annotating events
            clearRightBottomPanel();

            // Start point, end point drawn on the picture frame
            startPoint = endPoint = new Point();

            // Visible the new object panel and edit object panel
            editObjectContextPanel.Visible = false;
            newObjectContextPanel.Visible = false;
            selectObjContextPanel.Visible = false;

            // Cancel select buttons in drawing button toolbox
            foreach (Button b in drawingButtonGroup)
            {
                selectButtonDrawing(b, drawingButtonGroup, false);
            }

            // No object selected
            selectedObject = null;


            currentVideo = null;
            addEventAnnotationBtn.Enabled = false;

            startInSecondTextBox.Text = "";
            endInSecondTextBox.Text = "";
            setMinimumFrameTrackBar(0);
            setMaximumFrameTrackBar(100);
        }


        internal void clearMiddleCenterPanel()
        {
            middleCenterTableLayoutPanel.Controls.Clear();
            lastObjectCell = new Point(1, 0);
        }

        internal void clearMidleBottomPanel()
        {
            middleBottomTableLayoutPanel.Controls.Clear();
            middleBottomTableLayoutPanel.Controls.Add(addEventAnnotationBtn, 0, 0);
            lastAnnotationCell = new Point(1, 0);
        }

        internal void clearRightCenterPanel()
        {
            objectProperties.Rows.Clear();
        }

        internal void clearRightBottomPanel()
        {
            annotationText.Text = "";
            annoRefView.Rows.Clear();
        }

        internal void populateMiddleCenterPanel()
        {
            foreach (Object o in currentSession.getObjects())
            {
                addObjectAnnotation(o);
            }
        }

        internal void populateMiddleBottomPanel()
        {
            foreach (Event ev in currentSession.events)
            {
                //a.setID(0);
                addAnnotation(ev);
            }
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
                if (currentSession != null)
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
            if (currentSession == null)
            { return; }

            //Check files in current Session folder
            String[] files = Directory.GetFiles(workspace.getLocationFolder() + Path.DirectorySeparatorChar + 
                currentSession.getProject() + Path.DirectorySeparatorChar + currentSession.sessionName );

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

        /// <summary>
        /// Copy a file into the current session
        /// </summary>
        /// <param name="fileName">Original full path of file</param>
        /// <returns>The full path of copied file in the current session</returns>
        internal string copyFileIntoLocalSession(string fileName)
        {
            string relFileName = fileName.Split(Path.DirectorySeparatorChar)[fileName.Split(Path.DirectorySeparatorChar).Length - 1];
            //MessageBox.Show("inputFile = " + openFileDialog1.FileName);
            string dstFileName = selectedProject.getLocation() + Path.DirectorySeparatorChar + selectedProject.getProjectName() + Path.DirectorySeparatorChar + currentSession.sessionName + Path.DirectorySeparatorChar + relFileName;
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

                ////Add view to comboBox1:
                if (relFileName.isVideoFile() || relFileName.isDepthFile())
                    playbackFileComboBox.Items.Add(relFileName);
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
            string dstFileName = selectedProject.getLocation() + Path.DirectorySeparatorChar + selectedProject.getProjectName() + Path.DirectorySeparatorChar + currentSession.sessionName + Path.DirectorySeparatorChar + newRelFileName;
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

                ////Add view to comboBox1:
                //if (newRelFileName.isVideoFile() || newRelFileName.isDepthFile())
                //    playbackFileComboBox.Items.Add(newRelFileName);
            }
            return dstFileName;
        }
    }
}
