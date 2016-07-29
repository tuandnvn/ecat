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
        private void recordSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabs.SelectedIndex = 1;
        }

        private void newSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(selectedProjectName);
            if (!newSession)
            {
                addNewSession();
            }
        }

        //Select project from available projects in workspace
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null && currentSession.getEdited())
            {
                MessageBox.Show("Cannot select project, session " + currentSession.sessionName + " in project " + currentSession.project + " is editing");
                return;
            }

            treeView.BeginUpdate();


            ///////////////////////////////////
            //Release current project
            if (currentProject != null)
            {
                currentProjectNode.BackColor = Color.White;
                // Release resources to free memory
                foreach (Session s in currentProject.sessions)
                {
                    s.releaseResources();
                }
                currentProject.selected = false;
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


        //Close project if selected
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentSession != null && currentSession.getEdited())
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
                        List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
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
