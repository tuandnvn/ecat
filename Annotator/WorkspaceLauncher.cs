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
        
        public WorkspaceLauncher(Main frm1)
        {
            InitializeComponent();
            this.frm1 = frm1;
        }
        private Main frm1;
        private void WorkspaceLauncher_Load(object sender, EventArgs e)
        {
            try
            {
                //Load default workspace location:
                String defaultLocation = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    defaultLocation = Directory.GetParent(defaultLocation).ToString();
                }
                defaultLocation += Path.DirectorySeparatorChar + "AnnotatorWorkspace";
                //Create AnnotatorWorkspace folder in default location if it doesn't exists:
                if(!Directory.Exists(defaultLocation)){
                    Directory.CreateDirectory(defaultLocation);
                }
                textBox1.Text = defaultLocation;
                textBox1.Select(0, 0);
                //Set default folderBrowser properties:
                folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.Desktop;
                folderBrowserDialog1.SelectedPath = defaultLocation;                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm1.Close();
            Close();
        }
        //OK button clicked
        private void button3_Click(object sender, EventArgs e)
        {
            //Set workspace options:
            frm1.setWorkspace(textBox1.Text, checkBox1.Checked);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Show folder browser for Annotator workspace:
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

    }
}
