using System;
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

        /// <summary>
        /// Add new session to workspace
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
            Session newSession = new Session(sessionName, project, project.locationFolder);

            project.addSession(newSession);

            this.currentSession = newSession;
            //this.currentSession.loadIfNotLoaded();
            this.currentSessionNode = newSessionNode;
            this.treeView.SelectedNode = this.currentSessionNode;

            //2) Update treeView
            currentProjectNode.Nodes.Add(newSessionNode);
            currentProjectNode.Expand();
            treeView.EndUpdate();

            return newSession;
        }

        public void setNewSession(bool option)
        {
            this.newSession = option;
        }

        private void recordSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabs.SelectedIndex = 1;
        }

        private void newSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(selectedProjectName);
            if (!newSession)
            {
                showNewSessionPopup();
            }
        }

        //Select project from available projects in workspace
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            currentProject.selected = (true);

            this.simpleEventDataCreateMenuItem.Enabled = true;
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

        private void cleanUpCurrentProject()
        {
            if (currentSession != null)
            {
                cleanUpCurrentSession();
            }
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
            foreach (Session s in currentProject.sessions)
            {
                s.loadIfNotLoaded();
                s.getObjects;
            }
        }

        //Close project if selected
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null)
            {
                MessageBox.Show("Cannot close project, session " + currentSession.name + " is editing");
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

        private void projectOnlineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(null, options.prototypeList, 5);
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
                        List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.name, currentSession.getVideo(0),
                            currentSession.getDepth(0),
                            new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                            coordinateMapper.MapColorFrameToCameraSpace
                        );
                        AddObjectsIntoSession(detectedObjects);
                        currentSession.saveSession();
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            });
        }

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
    }
}
