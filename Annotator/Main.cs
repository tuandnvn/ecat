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
        }

        

        //Project workspace 
        private Workspace workspace = null;
        private String parametersFileName = Environment.CurrentDirectory + @"\params.param";
        private bool newProject = false;//true if new project is creating
        private bool newSession = false;//true if new session is creating     
        private Project selectedProject = null; //currently selected project
        private Session currentSession = null;
        private Video currentVideo = null;      //currently edited video

        private Font myFont = new Font("Microsoft Sans Serif", 5.75f);//font to write not-string colors
        private Point lastAnnotation = new Point(94, 0);              // last annotation location for middle-bottom panel
        private Point lastObjectTrack = new Point(94, 0);

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
                        TreeNode currentSessionNode = new TreeNode(sessions[i].Split(Path.DirectorySeparatorChar)[sessions[i].Split(Path.DirectorySeparatorChar).Length - 1], arrayFiles);

                        currentSessionNode.ImageIndex = 1;
                        currentSessionNode.SelectedImageIndex = currentSessionNode.ImageIndex;
                        //Add session to workspace 
                        String sessionName = currentSessionNode.ToString().Substring(10);
                        Project project = workspace.getProject(prjName);

                        if (project.checkSessionInProject(sessionName))
                        {
                            //MessageBox.Show("OK");
                            project.addSession(new Session(sessionName, project.getProjectName(), project.getLocation(), this));
                            //Add to threeview session list only files which exists in session filesList

                            Session session = project.getSession(sessionName);

                            for (int ii = 0; ii < currentSessionNode.Nodes.Count; ii++)
                            {
                                if (!session.checkFileInSession(currentSessionNode.Nodes[ii].ToString().Substring(10)))
                                {
                                    //MessageBox.Show(currentSessionNode.Nodes[ii].ToString().Substring(10));
                                    currentSessionNode.Nodes[ii].Remove();
                                }
                            }
                            array.Add(currentSessionNode);
                        }

                    }
                    else if (files.Length == 0)
                    {
                        TreeNode currentSessionNode = new TreeNode(sessions[i].Split(Path.DirectorySeparatorChar)[sessions[i].Split(Path.DirectorySeparatorChar).Length - 1]);
                        currentSessionNode.ImageIndex = 1;
                        currentSessionNode.SelectedImageIndex = currentSessionNode.ImageIndex;
                        //Add session to workspace
                        String sessionName = currentSessionNode.ToString().Substring(10);
                        //MessageBox.Show(sessionName);
                        Project project = workspace.getProject(prjName);
                        if (project.checkSessionInProject(sessionName))
                        {
                            project.addSession(new Session(sessionName, project.getProjectName(), project.getLocation(), this));
                            array.Add(currentSessionNode);
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
                Point location = this.Location;
                location.X += e.Location.X;
                location.Y += e.Location.Y;
                location.X += leftMostPanel.Location.X;
                location.Y += leftMostPanel.Location.Y;
                location.Y += 80;
                location.X += 15;
                if (selectedProject != null && treeView.SelectedNode.ToString().Contains(selectedProject.getProjectName()))
                {
                    selectToolStripMenuItem.Enabled = false;
                    closeToolStripMenuItem.Enabled = true;
                    newSessionToolStripMenuItem.Enabled = true;
                }
                else if (selectedProject != null && !(treeView.SelectedNode.ToString().Contains(selectedProject.getProjectName())))
                {
                    selectToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = false;
                    newSessionToolStripMenuItem.Enabled = false;
                }
                if (selectedProject == null)
                {
                    selectToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = false;
                    newSessionToolStripMenuItem.Enabled = false;
                }
                projectRightClickPanel.Show(location);
            }
            else if (treeView.SelectedNode.Level == 1 && e.Button == MouseButtons.Right)
            {

                TreeNode selectedNode = treeView.SelectedNode;
                //MessageBox.Show(selectedNode.ToString() + ", selectedProject = " +  selectedProject.getProjectName());
                Session choosedSession = null;
                if (selectedNode != null && selectedProject != null && selectedNode.Parent.ToString().Contains(selectedProject.getProjectName()))
                {
                    //MessageBox.Show("OK");
                    editSessionMenuItem.Enabled = true;
                    saveSessionMenuItem.Enabled = true;
                    deleteSessionMenuItem.Enabled = true;
                    addSessionMenuItem.Enabled = true;
                    //Check if session is editing:
                    choosedSession = selectedProject.getSession(selectedNode.ToString().Substring(10));
                    if (choosedSession == null)
                        choosedSession = selectedProject.getSession(selectedNode.ToString().Substring(11));
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
                location.X += e.Location.X;
                location.Y += e.Location.Y;
                location.X += leftMostPanel.Location.X;
                location.Y += leftMostPanel.Location.Y;
                location.Y += 80;
                location.X += 15;
                sessionRightClickPanel.Show(location);
            }
        }
        
        // Add object tracking
        public void addObjectTracking(ObjectAnnotation objectTrack)
        {
            objectTrack.Location = lastObjectTrack;
            middleCenterPanel.Controls.Add(objectTrack);
            lastObjectTrack.Y = lastObjectTrack.Y + objectTrack.Height + 5;
            middleCenterPanel.Invalidate();
        }

        public void removeObjectTracking(ObjectAnnotation objectTrack)
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
            SessionInfo sessionInfo = new SessionInfo(this, treeView.SelectedNode.ToString());
            sessionInfo.Location = new Point(this.Location.X + (int)(sessionInfo.Width / 2.5), this.Location.Y + sessionInfo.Height / 2);
            sessionInfo.Show();
        }

        //Add new session to workspace
        public void addNewSessionToWorkspace(String projectName, String sessionName)
        {
            treeView.BeginUpdate();
            TreeNodeCollection nodes = getTreeViewNodes();
            foreach (TreeNode currentProject in nodes)
            {
                if (currentProject.ToString().Contains(projectName))
                {
                    //MessageBox.Show(projectName);
                    TreeNode newSession = new TreeNode(sessionName);
                    newSession.ImageIndex = 1;
                    newSession.SelectedImageIndex = newSession.ImageIndex;
                    newSession.Name = sessionName;
                    //1)Update workspace project by adding new session
                    Project project = workspace.getProject(projectName);
                    project.addSession(new Session(sessionName, project.getProjectName(), project.getLocation(), this));
                    //2)Update treeView
                    currentProject.Nodes.Add(newSession);
                    currentProject.Expand();
                    break;
                }
            }
            treeView.EndUpdate();
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
        public void addNewProjectToWorkspace(String projectName)
        {
            //MessageBox.Show(projectName);
            //1)Add new project to tree view:
            TreeNode projectNode = new TreeNode(projectName);
            treeView.Nodes.Add(projectNode);
            //2)Add new project to the workspace:
            workspace.addProject(projectName);
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
            comboBox1.Items.Clear();
            comboBox1.Enabled = false;
            frameTrackBar.Enabled = false;
        }

        private void loadVideo(String fileName)
        {
            Application.DoEvents();
            currentSession = null;
            //MessageBox.Show(selectedProject.getSessionN() + "");
            for (int i = 0; i < selectedProject.getSessionN(); i++)
            {
                //MessageBox.Show(selectedProject.getProjectName() + ", " + selectedProject.getSession(i).getSessionName() + ", " + selectedProject.getSession(i).getEdited());
                if (selectedProject.getSession(i).getEdited())
                {
                    currentSession = selectedProject.getSession(i);
                    break;
                }
            }

            //MessageBox.Show(currentSession.getVideosN() + "");            
            for (int i = 0; i < currentSession.getVideosN(); i++)
            {
                if (currentSession.getVideo(i).getFileName().Contains(comboBox1.SelectedItem.ToString()))
                {
                    currentVideo = currentSession.getVideo(i);
                    break;
                }
            }
            //2)Load video:
            if (currentVideo != null)
                frameTrackBar.Maximum = (int)currentVideo.getFramesNumber();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadVideo(comboBox1.SelectedItem.ToString());

            if (currentVideo != null)
            {
                endPoint = startPoint;
                label3.Text = "Frame: " + frameTrackBar.Value;
                pictureBoard.Image = currentVideo.GetFrame(frameTrackBar.Value - 1).Bitmap;
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
                pictureBoard.Image = currentVideo.GetFrame(frameTrackBar.Value - 1).Bitmap;
                goToFrameCount++;
                if (goToFrameCount == GARBAGE_COLLECT_BITMAP_COUNT)
                {
                    System.GC.Collect();
                    goToFrameCount = 0;
                }
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

        
    }
}
