using Emgu.CV;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {
        BaseDepthReader depthReader;
        byte[] depthValuesToByte;
        Bitmap depthBitmap;

        private void handleKeyDownOnAnnotatorTab(KeyEventArgs e)
        {
            // Click on rectangle
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.R)
            {
                if (rectangleDrawing.Enabled)
                {
                    rectangleDrawing_MouseDown(null, null);
                }
                return;
            }

            // Click on polygon
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.P)
            {
                if (polygonDrawing.Enabled)
                {
                    polygonDrawing_MouseDown(null, null);
                }
                return;
            }

            // Save down session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                if (currentSession != null)
                    saveCurrentSession();
            }

            // Edit session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.E)
            {
                if (currentProject != null)
                    editSessionMenuItem_Click(null, null);
            }

            // Add a file into session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                if (currentSession != null)
                    addFileToSessionMenuItem_Click(null, null);
            }

            // Undo in session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                if (currentSession != null)
                    undoBtn_Click(null, null);
            }

            // Redo in session
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Y)
            {
                if (currentSession != null)
                    redoBtn_Click(null, null);
            }

            // While editing a polygon
            handleKeyDownOnDrawingPolygon(e);
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
                chosenSession = currentProject.getSession(treeView.SelectedNode.Text.Substring(1));
            }
            else
            {
                chosenSession = currentProject.getSession(treeView.SelectedNode.Text);
            }

            // Save current session if it is edited
            if (currentSession != null && chosenSession != null && currentSession.sessionName != chosenSession.sessionName)
            {
                closeEditedSession();
            }

            // Set current session = chosen session
            if (chosenSession != null && !chosenSession.edited)
            {
                chosenSession.edited = true;
                currentSessionNode = treeView.SelectedNode;
                currentSession = chosenSession;
                currentSession.loadIfNotLoaded();
                currentSessionNode.Text = "*" + currentSessionNode.Text;

                frameTrackBar.Value = frameTrackBar.Minimum;
                this.Text = "Project " + currentProject.name + " is selected, edited session = " + chosenSession.sessionName;
            }

            refreshSessionMenuItem_Click(sender, e);
            loadViewsFromSession();
            // All toolstrips of file inside session are enables
            toggleFileToolStripsOfSession(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if thre is no editing session or the session has been safely handled. False to signal the next action shouldn't be carried out</returns>
        private bool closeEditedSession()
        {
            if (currentSession != null && currentSession.edited)
            {
                var result = MessageBox.Show(("Session " + currentSession.sessionName + " is being edited. Do you want to save this session?"), "Save session", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    closeSessionNode();
                    saveCurrentSession();
                    return true;
                }
                else if (result == DialogResult.No)
                {
                    closeSessionNode();
                    closeWithoutSaveCurrentSession();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void closeSessionNode()
        {
            currentSession.edited = false;
            treeView.BeginUpdate();
            currentSessionNode.Text = currentSessionNode.Text.Substring(1);
            treeView.EndUpdate();
        }

        private void loadViewsFromSession()
        {
            //Set comboBox:
            String[] viewsList = currentSession.getViews();

            playbackFileComboBox.Items.Clear();

            //MessageBox.Show(viewsList.Length + "");
            for (int i = 0; i < viewsList.Length; i++)
            {
                playbackFileComboBox.Items.Add(viewsList[i]);
            }

            Console.WriteLine("playbackFileComboBox.Items.Count " + playbackFileComboBox.Items.Count);

            if (playbackFileComboBox.Items.Count > 0)
            {
                int startIndex = -1;
                foreach (string item in playbackFileComboBox.Items)
                {
                    startIndex++;
                    if (Utils.isVideoFile(item))
                        break;
                }

                playbackFileComboBox.SelectedIndex = startIndex;
                playbackFileComboBox.Enabled = true;
                frameTrackBar.Enabled = true;
                addEventAnnotationBtn.Enabled = true;
            }
            else
            {
                playbackFileComboBox.Enabled = false;
                frameTrackBar.Enabled = false;
                addEventAnnotationBtn.Enabled = false;
                pictureBoard.BackgroundImage = null;
            }
        }

        private void toggleFileToolStripsOfSession(bool value)
        {
            addObjectToolStripMenuItem.Enabled = value;
            addRigsFromFileToolStripMenuItem.Enabled = value;
            removeToolStripMenuItem.Enabled = value;
            deleteToolStripMenuItem.Enabled = value;
        }

        private void loadVideo(string videoFilename)
        {
            Application.DoEvents();
            videoReader = currentSession.getVideo(videoFilename);

            if (videoReader != null)
            {
                setMaximumFrameTrackBar(videoReader.frameCount - 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSessionMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession.edited)
            {
                saveCurrentSession();
            }

            toggleFileToolStripsOfSession(false);
        }

        internal void saveCurrentSession()
        {
            currentSession.saveSession();
            currentSession.resetLastOpenTime();
            cleanSessionUI();
            logMessage($"Session {currentSession.sessionName} saved");
            currentSession = null;
            clearMemento();
        }

        private void closeWithoutSaveCurrentSession()
        {
            //Reload session
            currentSession.reload();
            currentSession.resetLastOpenTime();
            cleanSessionUI();
            logMessage($"Session {currentSession.sessionName} closed without saved");
            currentSession = null;
            clearMemento();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null)
            {
                if (selectedObject != null)
                {
                    cancelSelectObject();
                }
                currentSession.resetAnnotation();
                rerenderAnnotation();
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null)
            {
                if (selectedObject != null)
                {
                    cancelSelectObject();
                }
                currentSession.reload();
                rerenderAnnotation();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void rerenderAnnotation()
        {
            clearMiddleCenterPanel();
            clearMidleBottomPanel();
            clearRightBottomPanel();
            populateMiddleCenterPanel();
            populateMiddleBottomPanel();
            invalidatePictureBoard();
        }

        private void cleanSessionUI()
        {
            if (currentSessionNode.Text.Contains("*"))
                currentSessionNode.Text = currentSessionNode.Text.Substring(1);
            currentSession.edited = false;
            this.Text = "Project " + currentProject.name + " selected";

            if (selectedObject != null)
            {
                cancelSelectObject();
            }


            clearPaintBoardView();

            // Clean object annotations
            clearMiddleCenterPanel();

            // Clean event annotation
            clearMidleBottomPanel();

            // Clean panel for annotating events
            clearRightBottomPanel();

            // Visible the new object panel and edit object panel
            editObjectContextPanel.Visible = false;
            newObjectContextPanel.Visible = false;

            // Cancel select buttons in drawing button toolbox
            foreach (Button b in drawingButtonGroup)
            {
                selectButtonDrawing(b, drawingButtonGroup, false);
            }

            videoReader = null;
            depthReader = null;
            addEventAnnotationBtn.Enabled = false;

            // Reset zooming 
            this.pictureBoard.Dock = DockStyle.Fill;
            inZoomIn = true;
        }

        internal void clearMiddleCenterPanel()
        {

            middleCenterTableLayoutPanel.Controls.Clear();
            lastObjectCell = new Point(1, 0);
            this.objectToObjectTracks = new Dictionary<Object, ObjectAnnotation>();
        }

        internal void clearMidleBottomPanel()
        {
            middleBottomTableLayoutPanel.Controls.Clear();
            middleBottomTableLayoutPanel.Controls.Add(addEventAnnotationBtn, 0, 0);
            lastAnnotationCell = new Point(1, 0);
            this.mapFromEventToEventAnnotations = new Dictionary<Event, EventAnnotation>();
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
                addAnnotation(ev);
            }
        }

        private void rightClickOnSessionTreeNode(MouseEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            TreeNode selectedNode = treeView.SelectedNode;
            Session choosenSession = null;

            // Check if session node is inside currently open project
            if (selectedNode != null && currentProject != null && selectedNode.Parent.Text.Equals(currentProject.name))
            {
                //Check if session is editing:
                choosenSession = currentProject.getSession(selectedNode.Text);
                if (choosenSession == null)
                    choosenSession = currentProject.getSession(selectedNode.Text.Substring(1));

                if (choosenSession != null)
                {
                    if (choosenSession.edited)
                    {
                        editSessionMenuItem.Enabled = false;
                        exitWithoutSavingToolStripMenuItem.Enabled = true;
                        reloadToolStripMenuItem.Enabled = true;
                        resetToolStripMenuItem.Enabled = true;
                        saveSessionMenuItem.Enabled = true;
                        deleteSessionMenuItem.Enabled = true;
                        addFileToSessionMenuItem.Enabled = true;
                        refreshSessionMenuItem.Enabled = true;
                        sessionDetectToolStripMenuItem.Enabled = true;
                        sessionGenerateToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        editSessionMenuItem.Enabled = true;
                        exitWithoutSavingToolStripMenuItem.Enabled = false;
                        reloadToolStripMenuItem.Enabled = false;
                        resetToolStripMenuItem.Enabled = false;
                        saveSessionMenuItem.Enabled = false;
                        deleteSessionMenuItem.Enabled = true;
                        addFileToSessionMenuItem.Enabled = false;
                        refreshSessionMenuItem.Enabled = false;
                        sessionDetectToolStripMenuItem.Enabled = false;
                        sessionGenerateToolStripMenuItem.Enabled = false;
                    }
                }
            }
            else
            {
                editSessionMenuItem.Enabled = false;
                exitWithoutSavingToolStripMenuItem.Enabled = false;
                reloadToolStripMenuItem.Enabled = false;
                resetToolStripMenuItem.Enabled = false;
                saveSessionMenuItem.Enabled = false;
                deleteSessionMenuItem.Enabled = true;
                addFileToSessionMenuItem.Enabled = false;
                refreshSessionMenuItem.Enabled = false;
                sessionDetectToolStripMenuItem.Enabled = false;
                sessionGenerateToolStripMenuItem.Enabled = false;
            }

            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            sessionRightClickPanel.Show(location);
        }


        private void exitWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeSessionNode();
            closeWithoutSaveCurrentSession();
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
            string sessionName = sName;
            if (sName[0] == '*') sessionName = sessionName.Substring(1);

            TreeNode projectNode = sessionToDeleteName.Parent;
            Project project = workspace.getProject(projectNode.Text);
            if (MessageBox.Show("Confirm delete permanently session " + sessionName + " from project " + project.name, "Delete session", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //1)Remove session from project:
                Session removedSession = project.removeSession(sessionName);
                //2)Remove session from treeView:
                treeView.BeginUpdate();
                foreach (TreeNode currentSessionNode in currentProjectNode.Nodes)
                {
                    if (currentSessionNode.Text.Equals(sName))
                    {
                        //MessageBox.Show("Removing " + sName + " from" + project.getProjectName());
                        currentProjectNode.Nodes.Remove(currentSessionNode);
                        break;
                    }
                }
                treeView.EndUpdate();

                //GUI update
                addEventAnnotationBtn.Enabled = false;
                newObjectContextPanel.Visible = false;
                clearPlaybackFileComboBox();
                clearRightBottomPanel();
                pictureBoard.Image = null;
                //startPoint = endPoint;
                boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
                videoReader = null;


                //3)Delete folder on the system
                if (removedSession != null)
                {
                    Directory.Delete(removedSession.getPath(), true);
                }

                logMessage($"Session {removedSession.sessionName} is deleted from project {project.name}.");
            }
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

                    logMessage($"File {fullFileName} is added to session {currentSession.sessionName} of project {currentProject.name}");
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
            String[] files = Directory.GetFiles(Path.Combine(workspace.location, currentSession.project.name, currentSession.sessionName));

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
            string dstFileName = currentProject.path + Path.DirectorySeparatorChar + currentProject.name + Path.DirectorySeparatorChar + currentSession.sessionName + Path.DirectorySeparatorChar + relFileName;
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
            string dstFileName = currentProject.path + Path.DirectorySeparatorChar + currentProject.name + Path.DirectorySeparatorChar + currentSession.sessionName + Path.DirectorySeparatorChar + newRelFileName;
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
        
        private void sessionEventTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null)
            {
                EventTemplateGenerator etg = new EventTemplateGenerator(this, false);
                etg.StartPosition = FormStartPosition.CenterParent;
                etg.ShowDialog();
            }
        }

        private void fromSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionSelector ss = new SessionSelector(this, currentProject, currentSession);
            ss.StartPosition = FormStartPosition.CenterParent;
            ss.ShowDialog();
        }

        internal enum ObjectCopyMode
        {
            LAST_FRAME,
            LAST_APPEARANCE
        }

        /// <summary>
        /// Copy to current Session the objects from the other session
        /// The form of the objects are taken from the end of the other session
        /// </summary>
        /// <param name="currentSessionIndex"></param>
        /// <param name="otherSessonIndex"></param>
        internal void copyFromSession(int currentSessionIndex, int otherSessonIndex, ObjectCopyMode mode = ObjectCopyMode.LAST_FRAME)
        {
            var previousSession = currentProject.getSession(otherSessonIndex);
            previousSession.loadIfNotLoaded();

            Dictionary<Object, Object> identicalPairs = new Dictionary<Object, Object>();
            // Copy objects
            foreach (var prevObject in previousSession.getObjects())
            {
                var newObject = (Object)Activator.CreateInstance(prevObject.GetType(), new object[] { currentSession, "", prevObject.color, prevObject.borderSize, this.playbackFileComboBox.Text });
                newObject.name = prevObject.name;

                LocationMark2D lastLocationMark = null;
                switch (mode)
                {
                    case ObjectCopyMode.LAST_FRAME:
                        lastLocationMark = prevObject.getScaledLocationMark(previousSession.getVideo(0).frameCount - 1, 1, new System.Drawing.PointF());
                        break;
                    case ObjectCopyMode.LAST_APPEARANCE:
                        var frameNo = prevObject.objectMarks.Keys.Last(frame => !(prevObject.objectMarks[frame] is DeleteLocationMark));
                        lastLocationMark = prevObject.getScaledLocationMark(frameNo, 1, new System.Drawing.PointF());
                        break;
                }

                if (lastLocationMark != null)
                {
                    // Change the internal frameNo to current one
                    lastLocationMark.frameNo = frameTrackBar.Value;
                    newObject.objectMarks[frameTrackBar.Value] = lastLocationMark;

                    currentSession.addObject(newObject);
                    identicalPairs[prevObject] = newObject;
                    currentSession.addPredicate(frameTrackBar.Value, true, new Predicate("IDENTITY", new Permutation(new int[] { 1, 2 })), new Object[] { newObject, prevObject });
                }
            }

            // Copy object predicates
            foreach (var identicalPair in identicalPairs)
            {
                var prevObject = identicalPair.Key;
                var currentObject = identicalPair.Value;

                // Add all predicates that still hold true of prevObject at the end of the previous session
                // to new object
                var toCopyPredicateMarks = prevObject.getHoldingPredicates(prevObject.session.sessionLength - 1);
                foreach (var predicateMark in toCopyPredicateMarks)
                {
                    // All objects in the predicate mark has been copied to current session
                    if (predicateMark.objects.All(o => identicalPairs.ContainsKey(o)))
                    {
                        currentSession.addPredicate(frameTrackBar.Value, predicateMark.qualified, predicateMark.predicate, predicateMark.objects.Select(o => identicalPairs[o]).ToArray());
                    }
                }
            }

            // Add annotation
            foreach (var identicalPair in identicalPairs)
            {
                var currentObject = identicalPair.Value;
                addObjectAnnotation(currentObject);
            }

            invalidatePictureBoard();
            this.logSession($"Session {currentSession.sessionName} copied objects from session {previousSession.sessionName}");
        }

        /// <summary>
        /// A handful method when annotating movie,
        /// copy the list of objects from the end of previous session to current session,
        /// when these two sessions are in continuation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lastFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentSessionIndex = currentProject.sessions.IndexOf(currentSession);
            if (currentSessionIndex > 0)
            {
                copyFromSession(currentSessionIndex, currentSessionIndex - 1);
            }
        }

        private void lastAppearanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentSessionIndex = currentProject.sessions.IndexOf(currentSession);
            if (currentSessionIndex > 0)
            {
                copyFromSession(currentSessionIndex, currentSessionIndex - 1, ObjectCopyMode.LAST_APPEARANCE);
            }
        }
    }
}
