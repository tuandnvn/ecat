﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {
        //Add new Session - open poup window:
        private void showNewSessionPopup()
        {
            //1)Set new session state
            newSession = true;
            //2)Show popup for session name
            SessionInfo sessionInfo = new SessionInfo(this, treeView.SelectedNode.Text);
            sessionInfo.StartPosition = FormStartPosition.CenterParent;
            sessionInfo.ShowDialog();
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
                projectGenerateToolStripMenuItem.Enabled = false;
            }
            Point location = this.Location;
            location.X += e.Location.X + leftMostPanel.Location.X + 15;
            location.Y += e.Location.Y + leftMostPanel.Location.Y + 80;
            projectRightClickPanel.Show(location);
        }

        /// <summary>
        /// Select project from available projects in workspace
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null && currentSession.edited)
            {
                MessageBox.Show("Cannot select project, session " + currentSession.sessionName + " in project " + currentSession.project + " is editing");
                return;
            }

            treeView.BeginUpdate();
            ///////////////////////////////////
            //Release current project
            if (currentProject != null)
            {
                cleanUpCurrentProject();
            }

            ///////////////////////////////////
            //Get selected node from treeView:
            currentProjectNode = treeView.SelectedNode;
            String prjName = currentProjectNode.Text;
            currentProjectNode.BackColor = Color.Silver;
            treeView.EndUpdate();
            currentProject = workspace.getProject(prjName);
            currentProject.selected = true;

            this.Text = "Project " + currentProject.name + " selected";

            foreach (TreeNode node in treeView.Nodes)
            {
                if (node.Text != prjName)
                {
                    node.Collapse();
                }
                if (node.Text.Contains(prjName))
                    node.Expand();
            }
        }

        /// <summary>
        /// Add new session by showing a session info popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeEditedSession();
            if (!newSession)
            {
                showNewSessionPopup();
            }
        }

        /// <summary>
        /// Add new session to project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public Session addNewSessionToProject(String projectName, String sessionName)
        {
            treeView.BeginUpdate();
            TreeNodeCollection nodes = treeView.Nodes;
            //MessageBox.Show(projectName);

            TreeNode newSessionNode = new TreeNode(sessionName);
            newSessionNode.ImageIndex = 1;
            newSessionNode.SelectedImageIndex = newSessionNode.ImageIndex;
            newSessionNode.Name = sessionName;

            //1) Update workspace project by adding new session
            Project project = workspace.getProject(projectName);
            Session newSession = new Session(sessionName, project, project.path);

            project.addSession(newSession);

            this.currentSession = newSession;
            //this.currentSession.loadIfNotLoaded();
            this.currentSessionNode = newSessionNode;
            this.treeView.SelectedNode = this.currentSessionNode;

            //2) Update treeView
            currentProjectNode.Nodes.Add(newSessionNode);
            currentProjectNode.Expand();
            treeView.EndUpdate();

            //Log 
            logMessage($"Session {sessionName} is added to project {projectName}.");

            return newSession;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool acceptRefresh = closeEditedSession();

            if (acceptRefresh)
            {
                Console.WriteLine("Refresh " + currentProjectNode.Text);
                for (int projectIndex = 0; projectIndex < workspace.getProjectCount(); projectIndex++)
                {
                    string projectName = workspace.getProject(projectIndex).name;

                    if (projectName == currentProjectNode.Text)
                    {
                        bool expand = treeView.Nodes[projectIndex].IsExpanded;

                        treeView.Nodes.RemoveAt(projectIndex);
                        
                        TreeNode projectNode = addProjectTreeNode(projectIndex);

                        if (expand)
                        {
                            projectNode.Expand();
                        }
                        break;
                    }
                }
            }
        }

        public void setNewSession(bool option)
        {
            this.newSession = option;
        }

        private void recordSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeEditedSession();
            tabs.SelectedIndex = 1;
        }


        private void cleanUpCurrentProject()
        {
            closeEditedSession();
            currentProjectNode.BackColor = Color.White;
            // Release resources to free memory
            foreach (Session s in currentProject.sessions)
            {
                s.releaseResources();
            }
            currentProject.selected = false;
            currentProject = null;
        }


        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int numberOfSessions = 0;
            int numberOfObjects = 0;
            int numberOfLocationMarks = 0;
            Dictionary<String, int> numberOfLinks = new Dictionary<string, int>();

            foreach (Session s in currentProject.sessions)
            {
                s.loadIfNotLoaded();
                var objects = s.getObjects();
                if (objects.Count !=0 )
                {
                    numberOfSessions++;
                    numberOfObjects += objects.Count;
                    foreach (var o in objects)
                    {
                        numberOfLocationMarks += o.objectMarks.Keys.Count;
                    }
                    foreach ( var p in s.predicates )
                    {
                        if (!numberOfLinks.ContainsKey(p.predicate.predicate))
                        {
                            numberOfLinks[p.predicate.predicate] = 0;
                        }

                        numberOfLinks[p.predicate.predicate]++;
                    }
                }
            }

            Console.WriteLine("numberOfSessions\t" + numberOfSessions);
            Console.WriteLine("numberOfObjects\t" + numberOfObjects);
            Console.WriteLine("numberOfLocationMarks\t" + numberOfLocationMarks);
            foreach (var pred in numberOfLinks.Keys)
            {
                Console.WriteLine( pred + "\t" + numberOfLinks[pred]);
            }
        }

        //Close project if selected
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null)
            {
                MessageBox.Show("Cannot close project, session " + currentSession.sessionName + " is editing");
                return;
            }

            // Release resources to free memory
            foreach (Session s in currentProject.sessions)
            {
                s.releaseResources();
            }

            treeView.SelectedNode.BackColor = Color.White;
            currentProject.selected = false;
            currentProject = null;
            this.Text = "No project selected";
        }

        private void projectEventTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
            {
                EventTemplateGenerator etg = new EventTemplateGenerator(this, true);
                etg.StartPosition = FormStartPosition.CenterParent;
                etg.ShowDialog();
            }
        }

        private void objectReferencesByNameMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var session in currentProject.sessions)
            {
                currentSession = session;
                currentSession.loadIfNotLoaded();
                currentSession.findObjectsByNames();
                currentSession.saveSession();
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////This part is used to detect objects for sessions of a same project. Remove this function from GUI//////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Setup Kinect, then loop through all sessions and do object detection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectOnlineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(currentSession, options.prototypeList, 5);
            var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
            objectRecognizerIncluded[objectRecognizer] = true;
            setupKinectIfNeeded();

            Task t = Task.Run(async () =>
            {
                try
                {
                    if (currentlySetupKinect)
                    {
                        Console.WriteLine("Await");
                        isAvailable.Wait();
                        currentlySetupKinect = false;
                    }

                    foreach (var session in currentProject.sessions)
                    {
                        currentSession = session;
                        currentSession.loadIfNotLoaded();
                        List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
                            currentSession.getDepth(0),
                            new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                            coordinateMapper.MapColorFrameToCameraSpace
                        );
                        addObjectsIntoSession(detectedObjects);
                        currentSession.saveSession();
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            });
        }

        /// <summary>
        /// Loop through all sessions and do object detection with offline mode (no Kinect is needed)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectOfflineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var session in currentProject.sessions)
            {
                currentSession = session;
                currentSession.loadIfNotLoaded();
                sessionOfflineModeGlyphDetectToolStripMenuItem_Click(null, null);
                currentSession.saveSession();
            }
        }

    }
}
