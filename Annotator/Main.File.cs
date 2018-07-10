using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Main
    {
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

        private void removeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Double guard
            // This method should only work when current Session is not null
            if (currentSession == null) return;

            string relFileName = treeView.SelectedNode.Text;
            string absolutePath = getAbsolutePathFromRelFileName(relFileName);
            currentSession.removeFile(absolutePath);

            logMessage($"Remove file {relFileName} from session {currentSession.sessionName} of project {currentProject.name}.");

            refreshSessionMenuItem_Click(sender, e);
            loadViewsFromSession();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this file from the folder", "Delete file", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (currentSession == null) return;

                string relFileName = treeView.SelectedNode.Text;
                string absolutePath = getAbsolutePathFromRelFileName(relFileName);
                currentSession.removeFile(absolutePath);

                Console.WriteLine("Remove file from session" + relFileName);
                loadViewsFromSession();

                File.Delete(absolutePath);

                refreshSessionMenuItem_Click(sender, e);

                logMessage($"Delete file {absolutePath}.");
            }
        }

        private string getAbsolutePathFromRelFileName(string relFileName)
        {
            string absolutePath = Path.Combine(currentSession.workspacePath, currentSession.project.name, currentSession.sessionName, relFileName);

            return absolutePath;
        }

        private void addRigsFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string relVideoFileName = treeView.SelectedNode.Text;
            AddRigFileForm addRigForm = new AddRigFileForm(this, currentSession, relVideoFileName);
            addRigForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            addRigForm.ShowDialog(this);
        }
    }
}
