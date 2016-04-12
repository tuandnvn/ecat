using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class AddRigFileForm : Form
    {
        private Main main;
        private Session currentSession;
        private string videoFile;

        public AddRigFileForm(Main main, Session currentSession, string videoFile)
        {
            this.main = main;
            this.currentSession = currentSession;
            this.videoFile = videoFile;
            InitializeComponent();
        }

        private void rigSrcBrowseBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                String fullFileName = openFileDialog.FileName;
                rigSrcTxtBox.Text = fullFileName;
            }
        }

        private void rigSchemeBrowseBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) 
            {
                String fullFileName = openFileDialog.FileName;
                rigSchemeTxtBox.Text = fullFileName;
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string srcFilePath = rigSrcTxtBox.Text;
            string schemeFilePath = rigSchemeTxtBox.Text;

            if ( copyLocalChkBox.Checked)
            {
                srcFilePath = main.copyFileIntoLocalSession(srcFilePath);
                schemeFilePath = main.copyFileIntoLocalSession(schemeFilePath);
            }

            Rigs<Point> rigs = Rigs<Point>.getRigFromSource(srcFilePath, schemeFilePath);
            var objects = rigs.generateObjects(videoFile).Values.ToList();
            foreach ( var o in objects ) {
                currentSession.addObject(o);
                main.addObjectAnnotation(o);
            }

            this.Dispose();
        }
    }
}
