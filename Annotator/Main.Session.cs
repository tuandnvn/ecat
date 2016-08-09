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
                this.Text = "Project " + currentProject.name + " selected, edited session = " + chosenSession.sessionName;
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

                    if (videoReader != null)
                    {
                        //endPoint = startPoint = new Point();
                        boundingBoxLocationMark = new RectangleLocationMark(-1, new RectangleF());
                        label3.Text = "Frame: " + frameTrackBar.Value;

                        Mat m = videoReader.getFrame(0);
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
                    videoReader = null;
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
            }
            catch (Exception exc)
            {
                Console.WriteLine("Select video file exception");
                MessageBox.Show("Exception in opening this file, please try another file", "File exception", MessageBoxButtons.OK);
            }
        }


        private void frameTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Don't allow the track bar value to get out of the range [MinDragValue, MaxDragValue] 
            if (frameTrackBar.Value < frameTrackBar.MinDragVal)
            {
                frameTrackBar.Value = frameTrackBar.MinDragVal;
                return;
            }

            if (frameTrackBar.Value > frameTrackBar.MaxDragVal)
            {
                frameTrackBar.Value = frameTrackBar.MaxDragVal;
                return;
            }

            label3.Text = "Frame: " + frameTrackBar.Value;

            int frameStartWithZero = frameTrackBar.Value - 1;
            if (videoReader != null)
            {
                Mat m = videoReader.getFrame(frameStartWithZero);
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
            this.Text = "Project " + currentProject.name + " selected";

            if (selectedObject != null)
            {
                cancelSelectObject();
            }

            // Clean the playbackFileComboBox
            clearPlaybackFileComboBox();

            // Clean picture board frame
            pictureBoard.Image = null;

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

            startInSecondTextBox.Text = "";
            endInSecondTextBox.Text = "";
            setMinimumFrameTrackBar(0);
            setMaximumFrameTrackBar(100);

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
                currentSession.project.name + Path.DirectorySeparatorChar + currentSession.sessionName);

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
            string dstFileName = currentProject.locationFolder + Path.DirectorySeparatorChar + currentProject.name + Path.DirectorySeparatorChar + currentSession.sessionName + Path.DirectorySeparatorChar + relFileName;
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
            string dstFileName = currentProject.locationFolder + Path.DirectorySeparatorChar + currentProject.name + Path.DirectorySeparatorChar + currentSession.sessionName + Path.DirectorySeparatorChar + newRelFileName;
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

                List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
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
                List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
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

        /// <summary>
        /// A handful method when annotating movie,
        /// copy the list of objects from the end of previous session to current session,
        /// when these two sessions are in continuation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fromPreviousSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentSessionIndex = currentProject.sessions.IndexOf(currentSession);
            if (currentSessionIndex > 0)
            {
                copyFromSession(currentSessionIndex, currentSessionIndex - 1);
            }
        }

        private void fromSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionSelector ss = new SessionSelector(this, currentProject, currentSession);
            ss.StartPosition = FormStartPosition.CenterParent;
            ss.ShowDialog();
        }

        /// <summary>
        /// Copy to current Session the objects from the other session
        /// The form of the objects are taken from the end of the other session
        /// </summary>
        /// <param name="currentSessionIndex"></param>
        /// <param name="otherSessonIndex"></param>
        internal void copyFromSession(int currentSessionIndex, int otherSessonIndex)
        {
            var previousSession = currentProject.getSession(otherSessonIndex);
            previousSession.loadIfNotLoaded();
            foreach (var o in previousSession.getObjects())
            {
                var newObject = (Object)Activator.CreateInstance(o.GetType(), new object[] { currentSession, "", o.color, o.borderSize, this.playbackFileComboBox.Text });
                newObject.name = o.name;
                var lastLocationMark = o.getScaledLocationMark(previousSession.getVideo(0).frameCount - 1, 1, new System.Drawing.PointF());
                if (lastLocationMark != null)
                {
                    // Change the internal frameNo to current one
                    lastLocationMark.frameNo = frameTrackBar.Value;
                    newObject.objectMarks[frameTrackBar.Value] = lastLocationMark;
                    newObject.addLink(frameTrackBar.Value, o, true, new Predicate("IDENTITY", new Combination(new int[] { 1, 2 })));
                    currentSession.addObject(newObject);
                    addObjectAnnotation(newObject);
                }
            }

            invalidatePictureBoard();
        }
    }
}
