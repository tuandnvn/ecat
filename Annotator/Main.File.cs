using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public partial class Main
    {
        private void removeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Double guard
            // This method should only work when current Session is not null
            if (currentSession == null) return;
            string relFileName = treeView.SelectedNode.Text;

            if (relFileName.Equals("files.param"))
            {
                System.Windows.Forms.MessageBox.Show("You could not delete file.params");
                return;
            }
            
            var dr = System.Windows.Forms.MessageBox.Show("Do you want to delete file " + relFileName + "", "Delete file", System.Windows.Forms.MessageBoxButtons.YesNo);

            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                currentSession.removeFile(relFileName);
                var absolutePath = currentProject.locationFolder + Path.DirectorySeparatorChar + currentProject.name + Path.DirectorySeparatorChar + currentSession.name + Path.DirectorySeparatorChar + relFileName;
                File.Delete(absolutePath);

                refreshSessionMenuItem_Click(null, null);
            }
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
