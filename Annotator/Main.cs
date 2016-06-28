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
//using System.Collections.Generic;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Annotator
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            InitializeOtherControls();

            InitDrawingComponent();
            InitEventAnnoComponent();
            //Iinitialize workspace object
            workspace = new Workspace();
            //Load images to imageList
            //loadImages();

            //comboBox1.SelectedIndex = 0;
            //Just for sample GUI test
            setMinimumFrameTrackBar(0);
            setMaximumFrameTrackBar(100);

            previousSize = this.Size;
        }

        private void InitializeOtherControls()
        {
            // Record panel only for >= windows 8 
            if (System.Environment.OSVersion.Version.Major >= 6 && System.Environment.OSVersion.Version.Minor >= 2)
            {
                this.recordPanel = new Annotator.RecordPanel();
            }

            if (recordPanel != null)
            {
                this.recordTab.Controls.Add(this.recordPanel);
            }

            // 
            // recordPanel
            // 
            if (recordPanel != null)
            {
                this.recordPanel.main = this;
                this.recordPanel.Location = new System.Drawing.Point(0, 0);
                this.recordPanel.Name = "recordPanel";
                this.recordPanel.Size = new System.Drawing.Size(1420, 860);
                this.recordPanel.TabIndex = 0;
            }
        }

        Size previousSize;

        //Project workspace 
        private Workspace workspace = null;
        private String parametersFileName = Environment.CurrentDirectory + @"\params.param";
        private bool newProject = false;//true if new project is creating
        private bool newSession = false;//true if new session is creating     
        internal Project selectedProject = null; //currently selected project
        internal TreeNode selectedProjectNode = null;
        internal Session currentSession = null;
        internal TreeNode currentSessionNode = null;
        private VideoReader currentVideo = null;      //currently edited video
        private Font myFont = new Font("Microsoft Sans Serif", 5.75f);//font to write not-string colors
        private Point lastAnnotationCell = new Point(94, 0);              // last annotation location for middle-bottom panel
        private Point lastObjectCell = new Point(1, 0);
        //internal List<ObjectAnnotation> objectAnnotations { get; set; }
        internal Dictionary<Object, ObjectAnnotation> objectToObjectTracks { get; set; }
        List<Button> drawingButtonGroup = new List<Button>();
        Dictionary<Button, bool> drawingButtonSelected = new Dictionary<Button, bool>();

        // Increment each time user move frameTrackBar to new Location
        // Keep track of how many bitmaps has not been garbage collected
        // Will use for garbage collection
        private int goToFrameCount = 0;
        private const int GARBAGE_COLLECT_BITMAP_COUNT = 20;

        private void Form1_Load(object sender, EventArgs e)
        {
            //Parameters hidden file open:
            loadParameters();

            //Show workspace launcher at the beginning:
            if (!workspace.getDefaultOption())
            {
                WorkspaceLauncher workspaceLauncher = new WorkspaceLauncher(this);
                workspaceLauncher.Show();
            }

            else
            {
                setWorkspace(workspace.getLocationFolder(), workspace.getDefaultOption());
            }

            //this.objectAnnotations = new List<ObjectAnnotation>();
            this.objectToObjectTracks = new Dictionary<Object, ObjectAnnotation>();
        }

        //Set workspace option: folder and default option
        public void setWorkspace(String locationFolder, bool defaulOption)
        {
            workspace.setLocationFolder(locationFolder);
            workspace.setDefaulOption(defaulOption);
            //If deaflt workspace option choosed set parametrs in hidden param file
            if (workspace.getDefaultOption())
            {
                // Remove the hidden attribute of the file   
                if (File.Exists(parametersFileName))
                {
                    FileInfo myFile = new FileInfo(parametersFileName);
                    myFile.Attributes &= ~FileAttributes.Hidden;
                }

                File.WriteAllText(parametersFileName, workspace.getLocationFolder());

                FileInfo myFile1 = new FileInfo(parametersFileName);
                // Set the hidden attribute of the file
                myFile1.Attributes = FileAttributes.Hidden;
            }

            //Load workspace treeView:
            initworkspaceTreeView();
        }

        private void loadParameters()
        {
            //1)Check if file exists
            if (!File.Exists(parametersFileName))
            {
            }
            else//File already exists:
            {
                //Set file as hidden                
                FileInfo myFile = new FileInfo(parametersFileName);
                // Remove the hidden attribute of the file
                myFile.Attributes &= ~FileAttributes.Hidden;

                //Read file line by line
                List<String> list = new List<String>();
                string line;

                // Read the file and display it line by line.
                System.IO.StreamReader file =
                   new System.IO.StreamReader(parametersFileName);
                while ((line = file.ReadLine()) != null)
                {
                    list.Add(line);
                }
                if (list.Count == 1)
                {
                    workspace.setLocationFolder(list[0]);
                    workspace.setDefaulOption(true);
                    //MessageBox.Show("OK");
                }
                file.Close();
                myFile.Attributes |= FileAttributes.Hidden;
            }
        }


        //Load workspace treeView:
        private void initworkspaceTreeView()
        {
            //Step 1: detect all projects in workspace
            String workspaceFolder = workspace.getLocationFolder();
            String[] projects = Directory.GetDirectories(workspaceFolder);

            foreach (String projectName in projects)
            {
                TreeNode projectNode;

                String prjName = projectName.Split(Path.DirectorySeparatorChar)[projectName.Split(Path.DirectorySeparatorChar).Length - 1];
                List<TreeNode> array = new List<TreeNode>();
                String[] sessions = Directory.GetDirectories(projectName);
                workspace.addProject(prjName);

                // Initiate sessions in project
                for (int i = 0; i < Directory.GetDirectories(projectName).Length; i++)
                {
                    //Check files in current Session folder
                    String[] files = Directory.GetFiles(sessions[i]);

                    if (files.Length > 0)
                    {
                        TreeNode[] arrayFiles = new TreeNode[files.Length];
                        for (int j = 0; j < arrayFiles.Length; j++)
                        {
                            arrayFiles[j] = new TreeNode(files[j].Split(Path.DirectorySeparatorChar)[files[j].Split(Path.DirectorySeparatorChar).Length - 1]);
                            arrayFiles[j].ImageIndex = 2;
                            arrayFiles[j].SelectedImageIndex = arrayFiles[j].ImageIndex;
                        }

                        TreeNode sessionNode = new TreeNode(sessions[i].Split(Path.DirectorySeparatorChar)[sessions[i].Split(Path.DirectorySeparatorChar).Length - 1], arrayFiles);

                        sessionNode.ImageIndex = 1;
                        sessionNode.SelectedImageIndex = sessionNode.ImageIndex;
                        //Add session to workspace 
                        String sessionName = sessionNode.Text;
                        Project project = workspace.getProject(prjName);

                        if (project.checkSessionInProject(sessionName))
                        {
                            project.addSession(new Session(sessionName, project.getProjectName(), project.getLocation()));
                            //Add to treeview session list only files which exists in session filesList

                            Session session = project.getSession(sessionName);

                            array.Add(sessionNode);
                        }

                    }
                    else if (files.Length == 0)
                    {
                        TreeNode sessionNode = new TreeNode(sessions[i].Split(Path.DirectorySeparatorChar)[sessions[i].Split(Path.DirectorySeparatorChar).Length - 1]);
                        sessionNode.ImageIndex = 1;
                        sessionNode.SelectedImageIndex = sessionNode.ImageIndex;
                        String sessionName = sessionNode.Text;
                        Project project = workspace.getProject(prjName);
                        if (project.checkSessionInProject(sessionName))
                        {
                            project.addSession(new Session(sessionName, project.getProjectName(), project.getLocation()));
                            array.Add(sessionNode);
                        }
                    }
                }

                // If there are sessions in the project
                if (array.Count > 0)
                {
                    projectNode = new TreeNode(prjName, array.ToArray());
                    projectNode.ImageIndex = 0;
                    projectNode.SelectedImageIndex = projectNode.ImageIndex;
                    treeView.Nodes.Add(projectNode);
                }

                // If the project is empty
                else if (array.Count == 0)
                {
                    projectNode = new TreeNode(prjName);
                    projectNode.ImageIndex = 0;
                    projectNode.SelectedImageIndex = projectNode.ImageIndex;
                    treeView.Nodes.Add(projectNode);
                }
            }
        }

        private void loadImages()
        {
            String folder = Environment.CurrentDirectory + @"\images";
            String[] files = Directory.GetFiles(folder);
            foreach (String imgFileName in files)
            {
                //MessageBox.Show(imgFileName);
                Console.WriteLine(imgFileName);
                imageList1.Images.Add(Image.FromFile(imgFileName));
            }
        }

        /// <summary>
        /// Double click on a file open it in a native 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            // Click no nothing
            if (treeView.SelectedNode == null)
                return;

            String p = treeView.SelectedNode.FullPath;
            List<char> r = p.ToList();
            r.RemoveAll(c => c == '*');
            p = new string(r.ToArray());
            String fileName = workspace.getLocationFolder() + Path.DirectorySeparatorChar + p;
            //MessageBox.Show(p); 
            //MessageBox.Show(fileName + " nested level = " + treeView.SelectedNode.Level);

            Console.WriteLine(treeView.SelectedNode.Text);
            if (treeView.SelectedNode.Level == 2)
            {
                //Open file only if this inside session folder
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = fileName;
                myProcess.Start();
            }
        }

        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            //Right-click on project(nested level is 0)
            if (treeView.SelectedNode == null)
                return;

            if (treeView.SelectedNode.Level == 0 && e.Button == MouseButtons.Right)
            {
                rightClickOnProjectTreeNode(e);
            }
            else if (treeView.SelectedNode.Level == 1 && e.Button == MouseButtons.Right)
            {
                rightClickOnSessionTreeNode(e);
            }
            else if (treeView.SelectedNode.Level == 2 && e.Button == MouseButtons.Right)
            {
                rightClickOnFileTreeNode(e);
            }
        }

        private void rightClickOnFileTreeNode(MouseEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            TreeNode selectedNode = treeView.SelectedNode;
            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            fileRightClickPanel.Show(location);
        }

        private void rightClickOnSessionTreeNode(MouseEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            TreeNode selectedNode = treeView.SelectedNode;
            Session choosedSession = null;
            if (selectedNode != null && selectedProject != null && selectedNode.Parent.Text.Equals(selectedProject.getProjectName()))
            {
                editSessionMenuItem.Enabled = true;
                saveSessionMenuItem.Enabled = true;
                deleteSessionMenuItem.Enabled = true;
                addSessionMenuItem.Enabled = true;
                refreshSessionMenuItem.Enabled = true;

                //Check if session is editing:
                choosedSession = selectedProject.getSession(selectedNode.Text);
                if (choosedSession == null)
                    choosedSession = selectedProject.getSession(selectedNode.Text);
                if (choosedSession != null && choosedSession.getEdited())
                {
                    //MessageBox.Show("OK1");
                    editSessionMenuItem.Enabled = false;
                }
                else if (choosedSession != null && !choosedSession.getEdited())
                {
                    editSessionMenuItem.Enabled = true;
                    saveSessionMenuItem.Enabled = false;
                    addSessionMenuItem.Enabled = false;
                    //MessageBox.Show("OK2");
                }
            }
            else
            {
                editSessionMenuItem.Enabled = false;
                saveSessionMenuItem.Enabled = false;
                deleteSessionMenuItem.Enabled = false;
                addSessionMenuItem.Enabled = false;
                refreshSessionMenuItem.Enabled = false;
            }

            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            sessionRightClickPanel.Show(location);
        }

        private void rightClickOnProjectTreeNode(MouseEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            if (selectedProject != null && treeView.SelectedNode.Text.Equals(selectedProject.getProjectName()))
            {
                selectToolStripMenuItem.Enabled = false;
                closeToolStripMenuItem.Enabled = true;
                newSessionToolStripMenuItem.Enabled = true;
                recordSessionToolStripMenuItem.Enabled = true;
            }
            else if (selectedProject != null && !(treeView.SelectedNode.Text.Equals(selectedProject.getProjectName())))
            {
                selectToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = false;
                newSessionToolStripMenuItem.Enabled = false;
                recordSessionToolStripMenuItem.Enabled = false;
            }
            if (selectedProject == null)
            {
                selectToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = false;
                newSessionToolStripMenuItem.Enabled = false;
                recordSessionToolStripMenuItem.Enabled = false;
            }
            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            projectRightClickPanel.Show(location);
        }


        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //Select node with right click also
            if (e.Button == MouseButtons.Right)
            {
                treeView.SelectedNode = e.Node;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newProject == false)
            {
                addNewProject();
            }
        }

        private void simpleEventDataCreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedProject != null)
            {
                SimpleLearningDataGenerator g = new SimpleLearningDataGenerator();

                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    String fullFileName = saveFileDialog.FileName;
                    g.writeExtractedDataIntoFile(selectedProject, fullFileName);
                }
            }
        }

        //Add new Session - open poup window:
        private void addNewSession()
        {
            //1)Set new session state
            newSession = true;
            //2)Show popup for session name
            SessionInfo sessionInfo = new SessionInfo(this, treeView.SelectedNode.Text);
            sessionInfo.Location = new Point(this.Location.X + (int)(sessionInfo.Width / 2.5), this.Location.Y + sessionInfo.Height / 2);
            sessionInfo.Show();
        }

        /// <summary>
        /// Add new session to workspace
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public Session addNewSessionToWorkspace(String projectName, String sessionName)
        {
            treeView.BeginUpdate();
            TreeNodeCollection nodes = getTreeViewNodes();
            //MessageBox.Show(projectName);

            TreeNode newSessionNode = new TreeNode(sessionName);
            newSessionNode.ImageIndex = 1;
            newSessionNode.SelectedImageIndex = newSessionNode.ImageIndex;
            newSessionNode.Name = sessionName;

            //1) Update workspace project by adding new session
            Project project = workspace.getProject(projectName);
            Session newSession = new Session(sessionName, project.getProjectName(), project.getLocation());

            project.addSession(newSession);

            this.currentSession = newSession;
            //this.currentSession.loadIfNotLoaded();
            this.currentSessionNode = newSessionNode;
            this.treeView.SelectedNode = this.currentSessionNode;

            //2) Update treeView
            selectedProjectNode.Nodes.Add(newSessionNode);
            selectedProjectNode.Expand();
            treeView.EndUpdate();

            return newSession;
        }

        //Add new project - open popup window:
        private void addNewProject()
        {
            //1)Set new project state
            newProject = true;
            //2)Show popup for project name
            ProjectInfo projectInfo = new ProjectInfo(this);
            projectInfo.Location = new Point(this.Location.X + projectInfo.Width / 4, this.Location.Y + projectInfo.Height / 10);
            projectInfo.Show();

        }

        //Add new project to workspace
        public Project addNewProjectToWorkspace(String projectName)
        {
            //MessageBox.Show(projectName);
            //1)Add new project to tree view:
            TreeNode projectNode = new TreeNode(projectName);
            treeView.Nodes.Add(projectNode);
            //2)Add new project to the workspace:
            return workspace.addProject(projectName);
        }

        //Set new project as false(not/creating new project currently)
        public void setNewProject(bool option)
        {
            newProject = option;
        }
        //get project from workspace by project name
        public Project getProjectFromWorkspace(String projectName)
        {
            return workspace.getProject(projectName);
        }
        //Return choosed project name from workspace
        public String getWorkspaceProjectName(int index)
        {
            return workspace.getProjectName(index);
        }
        //Get workspace projects list size
        public int getWorkspaceProjectSize()
        {
            return workspace.getProjectsSize();
        }

        public void setTrackbarLocation(int value)
        {
            if (value >= frameTrackBar.Minimum && value <= frameTrackBar.Maximum && !editingAtAFrame)
                frameTrackBar.Value = value;
        }

        public void setNewSession(bool option)
        {
            this.newSession = option;
        }
        //Get treeView nodes:
        public TreeNodeCollection getTreeViewNodes()
        {
            return treeView.Nodes;
        }

        private void clearPlaybackFileComboBox()
        {
            playbackFileComboBox.Items.Clear();
            playbackFileComboBox.Enabled = false;
            frameTrackBar.Enabled = false;
        }


        private void setLeftTopPanel()
        {
            if (currentVideo != null)
            {
                clearRightBottomPanel();
                //MessageBox.Show(currentVideo.getObjects().Count + "");
                foreach (Object o in currentSession.getObjects())
                {
                    String c = o.color.ToString().Substring(7).Replace(']', ' ');
                    if (c.Contains("="))
                    {
                        c = "(" + o.color.R + "," + o.color.G + "," + o.color.B + ")";
                    }
                    //MessageBox.Show(c);
                    //addObjectToList(o.getID().ToString(), c, o.getType(), o.getBoundingBox().X, o.getBoundingBox().Y, o.getBoundingBox().Width, o.getBoundingBox().Height); 
                }

                clearMiddleCenterPanel();
                clearMidleBottomPanel();
                populateMiddleCenterPanel();
                populateMiddleBottomPanel();
            }
        }


        /// <summary>
        /// Dirty clean
        /// </summary>
        private void runGCForImage()
        {
            goToFrameCount++;
            if (goToFrameCount == GARBAGE_COLLECT_BITMAP_COUNT)
            {
                System.GC.Collect();
                goToFrameCount = 0;
            }
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                if (recordPanel != null)
                    recordPanel.releaseKinectViewers();
            }

            if (tabs.SelectedIndex == 1)
            {
                if (recordPanel != null)
                    recordPanel.initiateKinectViewers();
            }
        }


        internal void addObjectAnnotation(Object o)
        {
            var objectAnnotation = new ObjectAnnotation(o, this, this.frameTrackBar.Minimum, this.frameTrackBar.Maximum);
            //objectAnnotations.Add(objectAnnotation);
            objectToObjectTracks[o] = objectAnnotation;

            //objectAnnotation.Location = lastObjectCell;
            //if (lastObjectCell.Y >= 0) return;

            renderObjectAnnotation(objectAnnotation);
        }

        internal void removeObject(Object o)
        {
            removeDrawingObject(o);
            currentSession.removeObject(o.id);

            // Remove the annotation corresponding to this object
            // and rearrage all object annotations
            if (this.objectToObjectTracks.ContainsKey(o) )
            {
                ObjectAnnotation ot = this.objectToObjectTracks[o];
                if (ot != null)
                {
                    this.objectToObjectTracks.Remove(o);
                    //this.objectAnnotations.Remove(ot);
                }
            }

            clearMiddleCenterPanel();
            foreach (ObjectAnnotation objectAnnotation in objectToObjectTracks.Values)
            {
                renderObjectAnnotation(objectAnnotation);
            }
        }

        private void renderObjectAnnotation(ObjectAnnotation objectAnnotation)
        {
            middleCenterTableLayoutPanel.RowCount = lastObjectCell.Y + 1;
            middleCenterTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            middleCenterTableLayoutPanel.Size = new System.Drawing.Size(970, 60 * middleCenterTableLayoutPanel.RowCount + 4);
            middleCenterTableLayoutPanel.Controls.Add(objectAnnotation, lastObjectCell.X, lastObjectCell.Y);
            objectAnnotation.Dock = DockStyle.Fill;

            lastObjectCell.Y = lastObjectCell.Y + 1;

            middleCenterPanel.Invalidate();
        }

        internal void selectObject(Object o)
        {
            cancelSelectObject();

            //Remove any decoration of other objects
            foreach (Object other in objectToObjectTracks.Keys)
            {
                objectToObjectTracks[other].deselectDeco();
            }
            objectToObjectTracks[o].selectDeco();

            this.selectedObject = o;

            if (selectedObject != null)
            {
                selectObjContextPanel.Visible = true;
            }

            foreach (Button b in drawingButtonGroup)
            {
                selectButtonDrawing(b, drawingButtonGroup, false);
            }

            this.showInformation(o);

            polygonDrawing.Enabled = false;
            rectangleDrawing.Enabled = false;

            invalidatePictureBoard();
        }

        private int sessionStart;
        private int sessionEnd;

        private void StartInSecondTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                changeStartInSecond();
            }
        }

        private void StartInSecondTextBox_LostFocus(object sender, EventArgs e)
        {
            changeStartInSecond();
        }

        private void changeStartInSecond()
        {
            try
            {
                var startInSecond = int.Parse(startInSecondTextBox.Text);
                if (currentVideo != null)
                {
                    if (startInSecond * currentVideo.fps < currentVideo.frameCount)
                    {
                        // Plus one because frame is counted from 1
                        setMinimumFrameTrackBar((int)(currentVideo.fps * startInSecond));
                        frameTrackBar_ValueChanged(null, null);
                        if (frameTrackBar.Value < sessionStart)
                        {
                            frameTrackBar.Value = sessionStart;
                        }

                        rescaleFrameTrackBar();
                    }
                }
            }
            catch (Exception)
            {
                startInSecondTextBox.Text = "";
            }
        }

        private void EndInSecondTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                changeEndInSecond();
            }
        }

        private void EndInSecondTextBox_LostFocus(object sender, EventArgs e)
        {
            changeEndInSecond();
        }

        private void changeEndInSecond()
        {
            try
            {
                var endInSecond = int.Parse(endInSecondTextBox.Text);
                if (currentVideo != null)
                {
                    if (endInSecond * currentVideo.fps < currentVideo.frameCount)
                    {
                        setMaximumFrameTrackBar((int)(currentVideo.fps * endInSecond) - 1);
                        frameTrackBar_ValueChanged(null, null);
                        rescaleFrameTrackBar();
                    }
                }
            }
            catch (Exception)
            {
                endInSecondTextBox.Text = "";
            }
        }

        private void rescaleFrameTrackBar()
        {
            if (currentSession != null)
            {
                foreach (var objectAnnotation in objectToObjectTracks.Values)
                {
                    objectAnnotation.resetStartEnd(frameTrackBar.Minimum, frameTrackBar.Maximum);
                }

                foreach (var eventAnnotation in mapFromEventToEventAnnotations.Values)
                {
                    eventAnnotation.resetStartEnd(frameTrackBar.Minimum, frameTrackBar.Maximum);
                }
            }

            frameTrackBar.Invalidate();

            Invalidate();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                handleKeyDownOnAnnotatorTab(e);
            }

            if (tabs.SelectedIndex == 1)
            {
                if (e.KeyCode == Keys.D && recordPanel.recordMode != RecordPanel.RecordMode.Playingback && recordPanel != null)
                {
                    recordPanel.rgbBoard.Image.Save("temp.png");
                }
            }
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                handleKeyUpOnAnnotatorTab(e);
            }
        }

        private void setMinimumFrameTrackBar(int value)
        {
            frameTrackBar.Minimum = value;
            this.sessionStart = value;
        }

        private void setMaximumFrameTrackBar(int value)
        {
            frameTrackBar.Maximum = value;
            this.sessionEnd = value;
        }


        //private void Main_SizeChanged(object sender, EventArgs e)
        //{
        //    int widthChanged = this.Size.Width - previousSize.Width;
        //    int heightChanged = this.Size.Height - previousSize.Height;

        //    this.middleTopPanel.Size = new Size(this.middleTopPanel.Size.Width - widthChanged, this.middleTopPanel.Size.Height - heightChanged);
        //    this.middleCenterPanel.Size = new Size(this.middleCenterPanel.Size.Width - widthChanged, this.middleCenterPanel.Size.Height);
        //    this.middleBottomPanel.Size = new Size(this.middleBottomPanel.Size.Width - widthChanged, this.middleBottomPanel.Size.Height);

        //    previousSize = this.Size;
        //}

    }
}
