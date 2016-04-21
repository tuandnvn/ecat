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

            InitDrawingComponent();
            InitEventAnnoComponent();
            //Iinitialize workspace object
            workspace = new Workspace();
            //Load images to imageList
            //loadImages();

            //comboBox1.SelectedIndex = 0;
            //Just for sample GUI test
            frameTrackBar.Maximum = 100;

            Console.WriteLine(DateTime.Now.ToLongDateString());
            Console.WriteLine(DateTime.Now.ToShortDateString());
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            Console.WriteLine(DateTime.Now.ToShortTimeString());
        }

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
        private Point lastAnnotation = new Point(94, 0);              // last annotation location for middle-bottom panel
        private Point lastObjectTrack = new Point(94, 0);
        internal List<ObjectAnnotation> objectAnnotations { get; set; }
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

            this.objectAnnotations = new List<ObjectAnnotation>();
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

            TreeNode treeNode;
            foreach (String projectName in projects)
            {
                String prjName = projectName.Split(Path.DirectorySeparatorChar)[projectName.Split(Path.DirectorySeparatorChar).Length - 1];
                List<TreeNode> array = new List<TreeNode>();
                String[] sessions = Directory.GetDirectories(projectName);
                //Add project to workspace///////////////////////////////////////////////////////
                workspace.addProject(prjName);///////////////////////////////////////////////////
                                              /////////////////////////////////////////////////////////////////////////////////

                for (int i = 0; i < Directory.GetDirectories(projectName).Length; i++)
                {
                    //Check files in current Session folder
                    String[] files = Directory.GetFiles(sessions[i]);
                    if (files.Length > 0)
                    {
                        TreeNode[] arrayFiles = new TreeNode[files.Length];
                        for (int j = 0; j < arrayFiles.Length; j++)
                        {
                            Console.WriteLine(files[j]);
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
                            //MessageBox.Show("OK");
                            project.addSession(new Session(sessionName, project.getProjectName(), project.getLocation()));
                            //Add to threeview session list only files which exists in session filesList

                            Session session = project.getSession(sessionName);

                            for (int ii = 0; ii < sessionNode.Nodes.Count; ii++)
                            {
                                if (!session.checkFileInSession(sessionNode.Nodes[ii].Text))
                                {
                                    sessionNode.Nodes[ii].Remove();
                                }
                            }
                            array.Add(sessionNode);
                        }

                    }
                    else if (files.Length == 0)
                    {
                        TreeNode sessionNode = new TreeNode(sessions[i].Split(Path.DirectorySeparatorChar)[sessions[i].Split(Path.DirectorySeparatorChar).Length - 1]);
                        sessionNode.ImageIndex = 1;
                        sessionNode.SelectedImageIndex = sessionNode.ImageIndex;
                        //Add session to workspace
                        String sessionName = sessionNode.Text;
                        //MessageBox.Show(sessionName);
                        Project project = workspace.getProject(prjName);
                        if (project.checkSessionInProject(sessionName))
                        {
                            project.addSession(new Session(sessionName, project.getProjectName(), project.getLocation()));
                            array.Add(sessionNode);
                        }
                    }
                }
                if (array.Count > 0)
                {
                    treeNode = new TreeNode(prjName, array.ToArray());
                    treeNode.ImageIndex = 0;
                    treeNode.SelectedImageIndex = treeNode.ImageIndex;
                    treeView.Nodes.Add(treeNode);
                    //workspace.addProject(prjName);
                }
                else if (array.Count == 0)
                {
                    treeNode = new TreeNode(prjName);
                    treeNode.ImageIndex = 0;
                    treeNode.SelectedImageIndex = treeNode.ImageIndex;
                    treeView.Nodes.Add(treeNode);
                    //workspace.addProject(prjName);
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
            String p = treeView.SelectedNode.FullPath;
            List<char> r = p.ToList();
            r.RemoveAll(c => c == '*');
            p = new string(r.ToArray());
            String fileName = workspace.getLocationFolder() + Path.DirectorySeparatorChar + p;
            //MessageBox.Show(p); 
            //MessageBox.Show(fileName + " nested level = " + treeView.SelectedNode.Level);
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
            TreeNode selectedNode = treeView.SelectedNode;
            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            fileRightClickPanel.Show(location);
        }

        private void rightClickOnSessionTreeNode(MouseEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;
            Session choosedSession = null;
            if (selectedNode != null && selectedProject != null && selectedNode.Parent.Text.Equals(selectedProject.getProjectName()))
            {
                //MessageBox.Show("OK");
                editSessionMenuItem.Enabled = true;
                saveSessionMenuItem.Enabled = true;
                deleteSessionMenuItem.Enabled = true;
                addSessionMenuItem.Enabled = true;
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
            }
            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            sessionRightClickPanel.Show(location);
        }

        private void rightClickOnProjectTreeNode(MouseEventArgs e)
        {
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

        public void removeObjectAnnotation(ObjectAnnotation objectTrack)
        {
            middleCenterPanel.Controls.Remove(objectTrack);
            lastObjectTrack.Y = lastObjectTrack.Y - objectTrack.Height - 5;
            middleCenterPanel.Invalidate();
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

        private void clearComboBox1()
        {
            videoBox.Items.Clear();
            videoBox.Enabled = false;
            frameTrackBar.Enabled = false;
        }

        private void loadVideo(int videoIndex)
        {
            Application.DoEvents();
            currentVideo = currentSession.getVideo(videoIndex);

            if (currentVideo != null)
            {
                frameTrackBar.Maximum = currentVideo.frameCount;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int videoIndex = videoBox.SelectedIndex;
            loadVideo(videoIndex);

            if (currentVideo != null)
            {
                endPoint = startPoint;
                label3.Text = "Frame: " + frameTrackBar.Value;

                Mat m = currentVideo.getFrame(0);
                if ( m != null)
                {
                    pictureBoard.mat = m;
                    pictureBoard.Image = pictureBoard.mat.Bitmap;
                }
                
                setLeftTopPanel();
            }
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

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = "Frame: " + frameTrackBar.Value;
            //if(trackBar1.Value >= trackBar1.Minimum && trackBar1.Value < trackBar1.
            if (currentVideo != null)
            {
                Mat m = currentVideo.getFrame(frameTrackBar.Value);
                if (m != null)
                {
                    pictureBoard.mat = m;
                    pictureBoard.Image = pictureBoard.mat.Bitmap;
                }
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
                recordPanel.releaseKinectViewers();
            }

            if (tabs.SelectedIndex == 1)
            {
                recordPanel.initiateKinectViewers();
            }
        }

        private void addRigsFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string relVideoFileName = treeView.SelectedNode.Text;
            AddRigFileForm addRigForm = new AddRigFileForm(this, currentSession, relVideoFileName);
            addRigForm.Show(this);
            addRigForm.Location = new Point()
            {
                X = Math.Max(this.Location.X, this.Location.X + (this.Width - addRigForm.Width) / 2),
                Y = Math.Max(this.Location.Y, this.Location.Y + (this.Height - addRigForm.Height) / 2)
            };
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        internal void removeObject(Object o)
        {
            removeDrawingObject(o);
            currentSession.removeObject(o.id);

            // Remove the annotation corresponding to this object
            // and rearrage all object annotations
            ObjectAnnotation ot = this.objectToObjectTracks[o];
            if (ot != null)
            {
                this.objectToObjectTracks.Remove(o);
                this.objectAnnotations.Remove(ot);
            }

            foreach (ObjectAnnotation other in objectAnnotations)
            {
                if (other.Location.Y > ot.Location.Y)
                {
                    other.Location = new Point(other.Location.X, other.Location.Y - ot.Height - 5);
                }
            }
            removeObjectAnnotation(ot);
        }


        internal void addObjectAnnotation(Object o)
        {
            var objectAnnotation = new ObjectAnnotation(o, this, currentSession.sessionLength);
            objectAnnotations.Add(objectAnnotation);
            objectToObjectTracks[o] = objectAnnotation;

            objectAnnotation.Location = lastObjectTrack;
            middleCenterPanel.Controls.Add(objectAnnotation);
            lastObjectTrack.Y = lastObjectTrack.Y + objectAnnotation.Height + 5;
            middleCenterPanel.Invalidate();
        }

        internal void selectObject(Object o)
        {
            //Remove any decoration of other objects
            foreach (Object other in objectToObjectTracks.Keys)
            {
                objectToObjectTracks[other].deselectDeco();
            }
            objectToObjectTracks[o].selectDeco();

            lock (selectedObjectLock)
            {
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
            }
            invalidatePictureBoard();
        }


        internal void generate3dforObject(Object o)
        {
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyDown " + e.KeyCode);
            if (e.KeyCode == Keys.D && recordPanel.recordMode != RecordPanel.RecordMode.Playingback)
            {
                recordPanel.rgbBoard.Image.Save("temp.png");
            }
        }
    }
}
