using System;
using System.Collections.Generic;
using System.Drawing;
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
            currentSession.removeFile(relFileName);
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


    }
}
