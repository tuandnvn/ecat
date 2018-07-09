using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.IO;

namespace Annotator
{
    public partial class WorkspaceLauncher : Form
    {
        private Main main;
        public WorkspaceLauncher(Main main)
        {
            InitializeComponent();
            this.main = main;
        }
        
        private void WorkspaceLauncher_Load(object sender, EventArgs e)
        {
            try
            {
                String annotationLocation = "";
                if (main.workspace == null)
                {
                    //Load default workspace location:
                    String defaultLocation = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                    Console.WriteLine("defaultLocation = " + defaultLocation);
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        defaultLocation = Directory.GetParent(defaultLocation).ToString();
                    }

                    defaultLocation = Path.Combine(defaultLocation, "AnnotatorWorkspace");
                } else
                {
                    annotationLocation = main.workspace.location;
                }

                workspacePath.Text = annotationLocation;
                workspacePath.Select(0, 0);
                //Set default folderBrowser properties:
                folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.Desktop;
                folderBrowserDialog1.SelectedPath = annotationLocation;                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void cancelWsBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void selectWsBtn_Click(object sender, EventArgs e)
        {
            //Create AnnotatorWorkspace folder in default location if it doesn't exists:
            if (!Directory.Exists(workspacePath.Text))
            {
                Directory.CreateDirectory(workspacePath.Text);
            }

            //Set workspace options:
            main.setWorkspace(workspacePath.Text, checkBox1.Checked);
            Close();
        }

        private void wsBrowserBtn_Click(object sender, EventArgs e)
        {
            //Show folder browser for Annotator workspace:
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                workspacePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

    }
}
