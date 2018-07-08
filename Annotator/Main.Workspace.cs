﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Annotator
{
    /// <summary>
    /// Handle events and routines that are related to workspace and treeview
    /// </summary>
    partial class Main
    {   //Project workspace 
        private const string WS = "ws";
        private const string DEFAULT_WORKSPACE = "defaultWorkspace";
        private const string RECENT_WORKSPACES = "recentWorkspaces";
        private const string WORKSPACE = "workspace";

        internal Workspace workspace = null;
        private string defaultWorkspace = null;
        private String parametersFileName = Environment.CurrentDirectory + @"\params.param";
        private SortedSet<String> recentWorkspaceLocations;

        /// <summary>
        /// 
        /// Load workspace by reading contents of the directory
        /// Set title on the GUI to the path of the workspace
        /// Populate left panel with sub-directories
        /// </summary>
        public void loadWorkspace()
        {
            workspace.load();

            annotationWorkspaceTitle.Text = workspace.location;

            //Load workspace treeView:
            initWorkspaceTreeview();
        }

        private void loadParameters()
        {

            //1)Check if file exists
            if (!File.Exists(parametersFileName))
            {
            }
            else//File already exists:
            {
                try
                {
                    FileInfo myFile = new FileInfo(parametersFileName);
                    // Remove the hidden attribute of the file
                    myFile.Attributes &= ~FileAttributes.Hidden;

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(parametersFileName);

                    try
                    {
                        defaultWorkspace = xmlDocument.DocumentElement.SelectSingleNode(DEFAULT_WORKSPACE).InnerText;
                        workspace = new Workspace(defaultWorkspace, true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("No default workspace");
                    }

                    var recentWorkspacesNode = xmlDocument.DocumentElement.SelectSingleNode(RECENT_WORKSPACES);
                    foreach (XmlNode recentWorkspaceNode in recentWorkspacesNode.SelectNodes(WORKSPACE))
                    {
                        string location = recentWorkspaceNode.InnerText;
                        recentWorkspaceLocations.Add(location);
                    }

                    updateRecentWorkspacesToolStripMenuItems();

                    myFile.Attributes |= FileAttributes.Hidden;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Exception in loading parameter files");
                }
            }
        }


        private void saveParameters()
        {
            try
            {
                FileInfo paramFile = new FileInfo(parametersFileName);
                // Remove the hidden attribute of the file
                paramFile.Attributes &= ~FileAttributes.Hidden;

                XmlWriterSettings setting = new XmlWriterSettings();
                setting.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(parametersFileName, setting))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement(WS);

                    if (defaultWorkspace != null)
                    {
                        writer.WriteElementString(DEFAULT_WORKSPACE, defaultWorkspace);
                    }

                    if (recentWorkspaceLocations != null)
                    {
                        writer.WriteStartElement(RECENT_WORKSPACES);

                        foreach (var location in recentWorkspaceLocations)
                        {
                            writer.WriteElementString(WORKSPACE, location);
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                paramFile.Attributes |= FileAttributes.Hidden;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        internal void setWorkspace(string locationFolder, bool defaultOption)
        {
            if (currentProject != null)
                cleanUpCurrentProject();

            clearWorkspaceTreeview();
            workspace = new Workspace(locationFolder, defaultOption);
            if (defaultOption)
            {
                defaultWorkspace = locationFolder;
            }
            loadWorkspace();

            // Add this workspace into recent workspaces
            recentWorkspaceLocations.Add(locationFolder);

            if (recentWorkspaceLocations.Count != 0)
            {
                updateRecentWorkspacesToolStripMenuItems();
            }
        }

        private void updateRecentWorkspacesToolStripMenuItems()
        {
            this.switchWorkspaceToolStripMenuItem.DropDownItems.Clear();
            this.switchWorkspaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.othersToolStripMenuItem,
                new ToolStripSeparator()
            });

            this.switchWorkspaceToolStripMenuItem.DropDownItems.AddRange(recentWorkspaceLocations.Select(
                location =>
                {
                    var v = new ToolStripMenuItem(location);
                    v.Click += recentWorkspace_Click;
                    return v;
                }).ToArray());
        }

        private void recentWorkspace_Click(object sender, EventArgs e)
        {
            setWorkspace((sender as ToolStripMenuItem).Text, false);
        }

        private void clearWorkspaceTreeview()
        {
            treeView.Nodes.Clear();
        }

        /// <summary>
        /// Load workspace treeView:
        /// </summary>
        private void initWorkspaceTreeview()
        {
            try
            {
                for (int projectIndex = 0; projectIndex < workspace.getProjectCount(); projectIndex++)
                {
                    addProjectTreeNode(projectIndex);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Add a treeNode at the projectIndex position in treeView
        /// </summary>
        /// <param name="projectIndex"></param>
        private TreeNode addProjectTreeNode(int projectIndex)
        {
            TreeNode projectNode;

            String projectName = workspace.getProject(projectIndex).name;
            Project project = workspace.getProject(projectName);

            List<TreeNode> sessionTreeNodes = new List<TreeNode>();
            String[] sessionPaths = Directory.GetDirectories(Path.Combine(workspace.location, projectName));

            // Initiate sessions in project
            for (int i = 0; i < sessionPaths.Length; i++)
            {
                string sessionPath = sessionPaths[i];
                TreeNode sessionNode = addSessionTreeNode(project, sessionPath);
                if (sessionNode!= null)
                    sessionTreeNodes.Add(sessionNode);
            }

            // If there are sessions in the project
            if (sessionTreeNodes.Count > 0)
            {
                projectNode = new TreeNode(projectName, sessionTreeNodes.ToArray());
            }
            // If the project is empty
            else
            {
                projectNode = new TreeNode(projectName);
            }

            projectNode.ImageIndex = 0;
            projectNode.SelectedImageIndex = projectNode.ImageIndex;
            treeView.Nodes.Insert(projectIndex, projectNode);

            return projectNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="sessionPath"></param>
        /// <returns>A treeNode correspond to a sessionPath, = null if the path is not accepted </returns>
        private TreeNode addSessionTreeNode(Project project, string sessionPath)
        {
            //Check files in current Session folder
            String[] files = Directory.GetFiles(sessionPath);

            //Notice sessionName is # from sessionPath
            string sessionName = sessionPath.Split(Path.DirectorySeparatorChar)[sessionPath.Split(Path.DirectorySeparatorChar).Length - 1];

            //if (!project.checkSessionInProject(sessionName))
            //{
            //    return null;
            //}

            TreeNode sessionNode = null;
            if (files.Length > 0)
            {
                TreeNode[] arrayFiles = new TreeNode[files.Length];
                for (int j = 0; j < arrayFiles.Length; j++)
                {
                    arrayFiles[j] = new TreeNode(files[j].Split(Path.DirectorySeparatorChar)[files[j].Split(Path.DirectorySeparatorChar).Length - 1]);
                    arrayFiles[j].ImageIndex = 2;
                    arrayFiles[j].SelectedImageIndex = arrayFiles[j].ImageIndex;
                }

                sessionNode = new TreeNode(sessionName, arrayFiles);
            }
            else if (files.Length == 0)
            {
                 sessionNode = new TreeNode(sessionName);
            }

            sessionNode.ImageIndex = 1;
            sessionNode.SelectedImageIndex = sessionNode.ImageIndex;
            project.addSession(new Session(sessionName, project, project.path));

            return sessionNode;
        }

        /// <summary>
        /// Double click on a file open it in a native browser
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
            String fileName = workspace.location + Path.DirectorySeparatorChar + p;
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

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView.SelectedNode = treeView.GetNodeAt(e.X, e.Y);
            }
        }

        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            if (treeView.SelectedNode == null)
            {
                rightClickOut(e);
                return;
            }

            if (treeView.SelectedNode.Level == 0)
            {
                rightClickOnProjectTreeNode(e);
            }
            else if (treeView.SelectedNode.Level == 1)
            {
                rightClickOnSessionTreeNode(e);
            }
            else if (treeView.SelectedNode.Level == 2)
            {
                rightClickOnFileTreeNode(e);
            }
        }

        private void rightClickOut(MouseEventArgs e)
        {
            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            workspaceRightClickPanel.Show(location);
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
                        reloadToolStripMenuItem.Enabled = false;
                        resetToolStripMenuItem.Enabled = false;
                        saveSessionMenuItem.Enabled = false;
                        deleteSessionMenuItem.Enabled = false;
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
                reloadToolStripMenuItem.Enabled = false;
                resetToolStripMenuItem.Enabled = false;
                saveSessionMenuItem.Enabled = false;
                deleteSessionMenuItem.Enabled = false;
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

        private void rightClickOnProjectTreeNode(MouseEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            if (currentProject != null && treeView.SelectedNode.Text.Equals(currentProject.name))
            {
                selectToolStripMenuItem.Enabled = false;
                closeToolStripMenuItem.Enabled = true;
                statisticsToolStripMenuItem.Enabled = true;
                newSessionToolStripMenuItem.Enabled = true;
                refreshProjectMenuItem.Enabled = true;
                recordSessionToolStripMenuItem.Enabled = true;
                projectDetectToolStripMenuItem.Enabled = true;
                projectGenerateToolStripMenuItem.Enabled = true;
            }
            else if (currentProject != null && !(treeView.SelectedNode.Text.Equals(currentProject.name)))
            {
                selectToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = false;
                statisticsToolStripMenuItem.Enabled = false;
                newSessionToolStripMenuItem.Enabled = false;
                refreshProjectMenuItem.Enabled = false;
                recordSessionToolStripMenuItem.Enabled = false;
                projectDetectToolStripMenuItem.Enabled = false;
                projectGenerateToolStripMenuItem.Enabled = false;
            }

            if (currentProject == null)
            {
                selectToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = false;
                statisticsToolStripMenuItem.Enabled = false;
                newSessionToolStripMenuItem.Enabled = false;
                refreshProjectMenuItem.Enabled = false;
                recordSessionToolStripMenuItem.Enabled = false;
                projectDetectToolStripMenuItem.Enabled = false;
                projectGenerateToolStripMenuItem.Enabled = false;
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

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newProject == false)
            {
                showAddProjectPopup();
            }
        }

        //Add new project - open popup window:
        private void showAddProjectPopup()
        {
            //1)Set new project state
            newProject = true;
            //2)Show popup for project name
            ProjectInfo projectInfo = new ProjectInfo(this);
            projectInfo.StartPosition = FormStartPosition.CenterParent;
            projectInfo.ShowDialog();
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
            return workspace.getProjectCount();
        }

        private void otherWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkspaceLauncher workspaceLauncher = new WorkspaceLauncher(this);
            workspaceLauncher.Show();
        }
    }
}
