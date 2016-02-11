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

            InitializeAuxilliaryComponent();
            //Iinitialize workspace object
            workspace = new Workspace();
            //Load images to imageList
            //loadImages();

            //comboBox1.SelectedIndex = 0;
            //Just for sample GUI test
            frameTrackBar.Maximum = 100;

        }

        protected void InitializeAuxilliaryComponent()
        {
            drawingButtonGroup.Add(cursorDrawing);
            drawingButtonGroup.Add(rectangleDrawing);
            drawingButtonGroup.Add(polygonDrawing);

            drawingButtonSelected[cursorDrawing] = drawingButtonSelected[rectangleDrawing] = drawingButtonSelected[polygonDrawing] = false;
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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
                String prjName = projectName.Split('\\')[projectName.Split('\\').Length - 1];
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
                            arrayFiles[j] = new TreeNode(files[j].Split('\\')[files[j].Split('\\').Length - 1]);
                            arrayFiles[j].ImageIndex = 2;
                            arrayFiles[j].SelectedImageIndex = arrayFiles[j].ImageIndex;
                        }
                        TreeNode currentSessionNode = new TreeNode(sessions[i].Split('\\')[sessions[i].Split('\\').Length - 1], arrayFiles);

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

                        TreeNode currentSessionNode = new TreeNode(sessions[i].Split('\\')[sessions[i].Split('\\').Length - 1]);
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
            String fileName = workspace.getLocationFolder() + "\\" + p;
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
                    toolStripMenuItem1.Enabled = true;
                }
                else if (selectedProject != null && !(treeView.SelectedNode.ToString().Contains(selectedProject.getProjectName())))
                {
                    selectToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = false;
                    toolStripMenuItem1.Enabled = false;
                }
                if (selectedProject == null)
                {
                    selectToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = false;
                    toolStripMenuItem1.Enabled = false;
                }
                cm1.Show(location);
            }
            else if (treeView.SelectedNode.Level == 1 && e.Button == MouseButtons.Right)
            {

                TreeNode selectedNode = treeView.SelectedNode;
                //MessageBox.Show(selectedNode.ToString() + ", selectedProject = " +  selectedProject.getProjectName());
                Session choosedSession = null;
                if (selectedNode != null && selectedProject != null && selectedNode.Parent.ToString().Contains(selectedProject.getProjectName()))
                {
                    //MessageBox.Show("OK");
                    toolStripMenuItem2.Enabled = true;
                    toolStripMenuItem3.Enabled = true;
                    toolStripMenuItem4.Enabled = true;
                    toolStripMenuItem5.Enabled = true;
                    //Check if session is editing:
                    choosedSession = selectedProject.getSession(selectedNode.ToString().Substring(10));
                    if (choosedSession == null)
                        choosedSession = selectedProject.getSession(selectedNode.ToString().Substring(11));
                    if (choosedSession != null && choosedSession.getEdited())
                    {
                        //MessageBox.Show("OK1");
                        toolStripMenuItem2.Enabled = false;
                    }
                    else if (choosedSession != null && !choosedSession.getEdited())
                    {
                        toolStripMenuItem2.Enabled = true;
                        toolStripMenuItem3.Enabled = false;
                        toolStripMenuItem5.Enabled = false;
                        //MessageBox.Show("OK2");
                    }
                }
                else
                {
                    toolStripMenuItem2.Enabled = false;
                    toolStripMenuItem3.Enabled = false;
                    toolStripMenuItem4.Enabled = false;
                    toolStripMenuItem5.Enabled = false;
                }
                Point location = this.Location;
                location.X += e.Location.X;
                location.Y += e.Location.Y;
                location.X += leftMostPanel.Location.X;
                location.Y += leftMostPanel.Location.Y;
                location.Y += 80;
                location.X += 15;
                cm2.Show(location);
            }
        }
        public void clearRightBottomPanel()
        {
            annoRefView.Rows.Clear();
        }

        //Add annotation 
        public void addAnnotation(Annotation annotation)
        {
            currentSession.addAnnotation(annotation);
            annotation.Location = lastAnnotation;
            middleBottomPanel.Controls.Add(annotation);
            lastAnnotation.Y += annotation.Height + 5;
        }

        // Add objectr tracking
        public void addObjectTracking(ObjectTrack objectTrack)
        {
            objectTrack.Location = lastObjectTrack;
            middleCenterPanel.Controls.Add(objectTrack);
            lastObjectTrack.Y = objectTrack.Height + 5;
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

        private void cm1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            //Add new session for project:
            if (item.Text.Contains("New session"))
            {
                //MessageBox.Show(selectedProjectName);
                if (!newSession)
                {
                    addNewSession();
                }
            }
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

        private void cm2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //ToolStripItem item = e.ClickedItem;
        }

        //Add video to session
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
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

                String fileName = openFileDialog1.FileName.Split('\\')[openFileDialog1.FileName.Split('\\').Length - 1];
                //MessageBox.Show("inputFile = " + openFileDialog1.FileName);
                String dstFileName = selectedProject.getLocation() + "\\" + selectedProject.getProjectName() + "\\" + checkSession.getSessionName() + "\\" + fileName;
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
        //"Delete" option for choosed session item
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
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
            button2.Enabled = false;
            newObjectContextPanel.Visible = false;
            clearComboBox1();
            clearRightBottomPanel();
            pictureBoard.Image = null;
            startPoint = endPoint;
            currentVideo = null;
        }
        public void setAnnotationText(String txt)
        {
            annotationText.Text = txt;
        }
        //Save option choosed
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
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
                    clearMidleBottomPanel();
                    clearRightBottomPanel();
                    startPoint = endPoint;
                    newObjectContextPanel.Visible = false;
                    currentVideo = null;
                    button2.Enabled = false;
                }
            }
        }
        //Clear middle-bottom panel
        public void clearMidleBottomPanel()
        {
            middleBottomPanel.Controls.Clear();
            middleBottomPanel.Controls.Add(button2);
            lastAnnotation = new Point(94, 0);
        }

        //Select project from available projects in workspace
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if there is editing session:
            foreach (TreeNode projectNode in treeView.Nodes)
            {
                String sessionName = "";
                foreach (TreeNode sessionNode in projectNode.Nodes)
                {
                    sessionName = sessionNode.ToString().Substring(10);
                    if (sessionName.Contains("*"))
                        sessionName = sessionName.Substring(1);
                    Project project = workspace.getProject(projectNode.ToString().Substring(10));
                    Session session = project.getSession(sessionName);
                    if (session.getEdited())
                    {
                        MessageBox.Show("Cannot select project, session " + sessionName + " in project " + project.getProjectName() + " is editing");
                        return;
                    }
                }
            }
            ///////////////////////////////////
            //Get selected node from treeView:
            String prjName = treeView.SelectedNode.ToString().Substring(10);
            treeView.BeginUpdate();
            foreach (TreeNode node in treeView.Nodes)
            {
                node.BackColor = Color.White;
            }
            treeView.SelectedNode.BackColor = Color.Silver;
            //treeView.SelectedNode = null;
            treeView.EndUpdate();
            selectedProject = workspace.getProject(prjName);
            selectedProject.setSelected(true);
            //MessageBox.Show(selectedProject.getProjectName());
            this.Text = "Project " + selectedProject.getProjectName() + " selected";
            foreach (TreeNode node in treeView.Nodes)
            {
                if (node.ToString().Substring(10) != prjName)
                {
                    node.Collapse();
                }
                if (node.ToString().Substring(10).Contains(prjName))
                    node.Expand();
            }
        }
        private void clearComboBox1()
        {
            comboBox1.Items.Clear();
            comboBox1.Enabled = false;
            frameTrackBar.Enabled = false;
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
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
                button2.Enabled = true;
                //pictureBox1.BackgroundImage = null;
            }
        }
        //Close project if selected
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode.ToString().Contains(selectedProject.getProjectName()))
            {
                //Check if project session is editing:
                //Check if there is editing session:
                foreach (TreeNode projectNode in treeView.Nodes)
                {
                    String sessionName = "";
                    foreach (TreeNode sessionNode in projectNode.Nodes)
                    {
                        sessionName = sessionNode.ToString().Substring(10);
                        if (sessionName.Contains("*"))
                            sessionName = sessionName.Substring(1);
                        Project project = workspace.getProject(projectNode.ToString().Substring(10));
                        Session session = project.getSession(sessionName);
                        if (session.getEdited())
                        {
                            MessageBox.Show("Cannot close project, session " + sessionName + " is editing");
                            return;
                        }
                    }
                }

                treeView.SelectedNode.BackColor = Color.White;
                selectedProject.setSelected(false);
                selectedProject = null;
                this.Text = "No project selected";
            }
        }

        private void toolStripMenuItem5_CheckStateChanged(object sender, EventArgs e)
        {

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
                Mat frame = currentVideo.GetFrame(frameTrackBar.Value - 1);
                pictureBoard.Image = new Bitmap(frame.Bitmap);
                setLeftTopPanel();
            }
        }

        private void setLeftTopPanel()
        {
            if (currentVideo != null)
            {
                clearRightBottomPanel();
                //MessageBox.Show(currentVideo.getObjects().Count + "");
                foreach (Object o in currentVideo.getObjects())
                {
                    String c = o.color.ToString().Substring(7).Replace(']', ' ');
                    if (c.Contains("="))
                    {
                        c = "(" + o.color.R + "," + o.color.G + "," + o.color.B + ")";
                    }
                    //MessageBox.Show(c);
                    //addObjectToList(o.getID().ToString(), c, o.getType(), o.getBoundingBox().X, o.getBoundingBox().Y, o.getBoundingBox().Width, o.getBoundingBox().Height); 
                }

                clearMidleBottomPanel();

                foreach (ObjectTrack o in currentSession.objectTracks)
                {
                    addObjectTracking(o);
                }

                foreach (Annotation a in currentSession.annotations)
                {
                    //a.setID(0);
                    addAnnotation(a);
                }
            }
        }
        //Add reference label for right-bottom panel
        public void addReferenceLabel()
        {

        }
        //Add annotation button
        private void button2_Click(object sender, EventArgs e)
        {
            Annotation annotation = new Annotation(null, frameTrackBar.Minimum, frameTrackBar.Maximum, "", this, currentSession);
            addAnnotation(annotation);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {

        }

        public void addRightBottomTableReference(int start, int end, String text, String refID)
        {
            annoRefView.Rows.Add(start, end, text, refID);
        }
        //Unselect all annotations
        public void unselectAnnotations()
        {
            if (currentVideo != null)
            {
                foreach (Annotation a in currentSession.annotations)
                {
                    a.setSelected(false);
                }
            }
        }
        private void toolStripMenuItem6_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //addReferenceLabel();
            String refID = e.ClickedItem.ToString();
            int start = annotationText.SelectionStart;
            int end = annotationText.SelectionStart + annotationText.SelectionLength;
            String txt = annotationText.SelectedText;
            //MessageBox.Show(start + "," + end + "," + txt);
            Annotation annotation = null;
            foreach (Annotation a in currentSession.annotations)
            {
                if (a.getSelected())
                {
                    annotation = a;
                    break;
                }
            }
            if (annotation != null)
            {
                annotation.addReference(start, end, refID);
                addRightBottomTableReference(start, end, txt, refID);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = "Frame: " + frameTrackBar.Value;
            //if(trackBar1.Value >= trackBar1.Minimum && trackBar1.Value < trackBar1.
            if (currentVideo != null)
            {
                Mat frame = currentVideo.GetFrame(frameTrackBar.Value - 1);
                pictureBoard.Image = new Bitmap(frame.Bitmap);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = "Frame: " + frameTrackBar.Value;
            //if(trackBar1.Value >= trackBar1.Minimum && trackBar1.Value < trackBar1.
            if (currentVideo != null)
            {
                Mat frame = currentVideo.GetFrame(frameTrackBar.Value - 1);
                pictureBoard.Image = new Bitmap(frame.Bitmap);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void annotationText_MouseDown(object sender, MouseEventArgs e)
        {
            addObjRefToolStripMenuItem.DropDownItems.Clear();
            foreach (Object o in currentVideo.getObjects())
            {
                addObjRefToolStripMenuItem.DropDownItems.Add(o.id);
            }

            if (e.Button == MouseButtons.Right)
            {
                if (annotationText.Text != null && annotationText.Text.Length > 0)
                {
                    Console.WriteLine("Activate right panel");
                    addObjRefToolStripMenuItem.Enabled = true;
                }

                else if (annotationText.Text != null && annotationText.Text.Length > 0)
                {
                    addObjRefToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void annoRefView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            annoRefView.Rows[e.RowIndex].Selected = true;
            annoRefView.Invalidate();
        }
    }
}
