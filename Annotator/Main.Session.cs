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

            if (chosenSession == null)
                return;

            if (currentSession != null && currentSession.name == chosenSession.name)
            {
                return;
            }

            // Save current session if it is edited
            if (currentSession != null && currentSession.name != chosenSession.name)
            {
                cleanUpCurrentSession();
            }

            // Set current session = chosen session
            if (!chosenSession.edited)
            {
                currentSessionNode = treeView.SelectedNode;
                currentSession = chosenSession;
                currentSession.loadIfNotLoaded();
                chosenSession.edited = true;
                currentSessionNode.Text = "*" + currentSessionNode.Text;

                frameTrackBar.Value = frameTrackBar.Minimum;
                this.Text = "Project " + currentProject.name + " selected, edited session = " + chosenSession.name;
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

                // All toolstrips of file inside session are enables
                toggleFileToolStripsOfSession(true);
            }

            logSession($"Session {currentSession.name} loaded");
        }

        /// <summary>
        /// Close current session
        /// Asking for saving it down or revert change
        /// </summary>
        private void cleanUpCurrentSession()
        {
            currentSession.edited = false;
            treeView.BeginUpdate();
            currentSessionNode.Text = currentSessionNode.Text.Substring(1);
            treeView.EndUpdate();

            var result = MessageBox.Show(("Session " + currentSession.name + " of project " + currentProject.name + "currently editing, Do you want to save this session?"), "Save session", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                saveCurrentSession();
            }
            else if (result == DialogResult.No)
            {
                closeWithoutSaveCurrentSession();
            }
        }

        private void toggleFileToolStripsOfSession(bool value)
        {
            addObjectToolStripMenuItem.Enabled = value;
            addRigsFromFileToolStripMenuItem.Enabled = value;
            removeToolStripMenuItem.Enabled = value;
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
            cleanSessionUI();
            logMessage($"Session {currentSession.name} saved");
            currentSession = null;
            clearMemento();
        }

        private void closeWithoutSaveCurrentSession()
        {
            //Reload session
            currentSession.reload();
            cleanSessionUI();
            logMessage($"Session {currentSession.name} closed without saved");
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
            if (MessageBox.Show("Confirm session removal (exclude from project): " + sName + " from " + project.name, "Delete session", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //1)Remove session from project:
                project.removeSession(sName);
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
            }

            //Disable button2:
            addEventAnnotationBtn.Enabled = false;
            newObjectContextPanel.Visible = false;
            clearPlaybackFileComboBox();
            clearRightBottomPanel();
            pictureBoard.Image = null;
            //startPoint = endPoint;
            boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
            videoReader = null;
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
            String[] files = Directory.GetFiles(workspace.locationFolder + Path.DirectorySeparatorChar +
                currentSession.project.name + Path.DirectorySeparatorChar + currentSession.name);

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
            string dstFileName = currentProject.locationFolder + Path.DirectorySeparatorChar + currentProject.name + Path.DirectorySeparatorChar + currentSession.name + Path.DirectorySeparatorChar + relFileName;
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
            string dstFileName = currentProject.locationFolder + Path.DirectorySeparatorChar + currentProject.name + Path.DirectorySeparatorChar + currentSession.name + Path.DirectorySeparatorChar + newRelFileName;
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

        CountdownEvent isAvailable;
        volatile bool sensorAvailabel = false;
        volatile bool colorFrameArrived = false;
        volatile bool depthFrameArrived = false;
        volatile bool currentlySetupKinect = false;
        KinectSensor kinectSensor;
        CoordinateMapper coordinateMapper;
        DepthCoordinateMappingReader mappingReader;



        private void sessionOnlineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(null, options.prototypeList, 5);
            var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
            objectRecognizerIncluded[objectRecognizer] = true;
            setupKinectIfNeeded();

            Task t = Task.Run(async () =>
            {

                if (currentlySetupKinect)
                {
                    Console.WriteLine("Await");
                    isAvailable.Wait();
                    currentlySetupKinect = false;
                }

                List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.name, currentSession.getVideo(0),
                currentSession.getDepth(0),
                new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                coordinateMapper.MapColorFrameToCameraSpace
                    );

                // Run on UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    AddDetectedObjects(detectedObjects);
                });
            });
        }

        private void setupKinectIfNeeded()
        {
            if (kinectSensor == null)
            {
                isAvailable = new CountdownEvent(3);
                sensorAvailabel = false;
                colorFrameArrived = false;
                depthFrameArrived = false;
                kinectSensor = KinectSensor.GetDefault();
                coordinateMapper = kinectSensor.CoordinateMapper;
                var colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();
                var depthFrameReader = this.kinectSensor.DepthFrameSource.OpenReader();
                colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;
                depthFrameReader.FrameArrived += this.Reader_DepthFrameArrived;
                kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;
                kinectSensor.Open();
                currentlySetupKinect = true;
            }
        }

        private void sessionOfflineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(null, options.prototypeList, 5);
            var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
            objectRecognizerIncluded[objectRecognizer] = true;

            if (mappingReader == null)
            {
                mappingReader = new DepthCoordinateMappingReader("coordinateMapping.dat");
            }

            Task t = Task.Run(async () =>
            {
                List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.name, currentSession.getVideo(0),
                currentSession.getDepth(0),
                new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                (depthImage, result) => mappingReader.projectDepthImageToCameraSpacePoint(depthImage,
                    currentSession.getDepth(0).depthWidth,
                    currentSession.getDepth(0).depthHeight,
                    currentSession.getVideo(0).frameWidth,
                    currentSession.getVideo(0).frameHeight, result)
                    );

                // Run on UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    AddDetectedObjects(detectedObjects);
                });

            });
        }

        private void AddDetectedObjects(List<Object> detectedObjects)
        {
            AddObjectsIntoSession(detectedObjects);
            RefreshUI(detectedObjects);
        }

        private void RefreshUI(List<Object> detectedObjects)
        {
            // Redraw object annotation panel
            if (detectedObjects.Count != 0)
            {
                foreach (Object o in detectedObjects)
                {
                    addObjectAnnotation(o);
                }
                invalidatePictureBoard();
            }
        }

        private void AddObjectsIntoSession(List<Object> detectedObjects)
        {
            // Handle adding identical objects or not
            switch (options.detectionMode)
            {
                case Options.OverwriteMode.ADD_SEPARATE:
                    foreach (var detectedObject in detectedObjects)
                    {
                        currentSession.addObject(detectedObject);
                    }
                    break;
                case Options.OverwriteMode.NO_OVERWRITE:
                    foreach (GlyphBoxObject detectedObject in detectedObjects)
                    {
                        bool exist = false;
                        foreach (var existObject in currentSession.getObjects())
                        {
                            if (existObject is GlyphBoxObject && detectedObject.boxPrototype.Equals(((GlyphBoxObject)existObject).boxPrototype))
                            {
                                exist = true;
                                break;
                            }
                        }

                        if (!exist)
                        {
                            currentSession.addObject(detectedObject);
                        }
                    }
                    break;
                case Options.OverwriteMode.OVERWRITE:
                    foreach (GlyphBoxObject detectedObject in detectedObjects)
                    {
                        Object exist = null;
                        foreach (var existObject in currentSession.getObjects())
                        {

                            if (existObject is GlyphBoxObject && detectedObject.boxPrototype.Equals(((GlyphBoxObject)existObject).boxPrototype))
                            {
                                exist = existObject;
                                break;
                            }
                        }

                        if (exist != null)
                        {
                            currentSession.removeObject(exist.id);
                        }
                        currentSession.addObject(detectedObject);
                    }
                    break;
            }
        }

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            if (!sensorAvailabel && currentlySetupKinect)
            {
                isAvailable.Signal();
                sensorAvailabel = true;
            }
        }

        private void Reader_DepthFrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            if (!depthFrameArrived && currentlySetupKinect)
            {
                isAvailable.Signal();
                depthFrameArrived = true;
            }
        }

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (!colorFrameArrived && currentlySetupKinect)
            {
                isAvailable.Signal();
                colorFrameArrived = true;
            }
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

        internal void findObjectForEvent(Event ev)
        {
            currentSession.resetTempoEmpty(ev);
            currentSession.findObjectsByNames(ev);
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
                var toCopyPredicateMarks = prevObject.getHoldingPredicates(prevObject.session.frameLength - 1);
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
            this.logSession($"Session {currentSession.name} copied objects from session {previousSession.name}");
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
