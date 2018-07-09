using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class ProjectInfo : Form
    {
        private Main main = null;

        public ProjectInfo(Main main)
        {
            InitializeComponent();
            this.main = main;
            this.ActiveControl = this.projectNameTxtBox;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            main.setNewProject(false);
            this.Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (parseProjectName())
            {
                //Check if such project already exists in workspace:
                bool exists = false;
                for (int i = 0; i < main.getWorkspaceProjectSize(); i++)
                {
                    //MessageBox.Show(frm1.getWorkspaceProjectName(i) + ", textBox1.Text = " + textBox1.Text + frm1.getWorkspaceProjectName(i).Contains(textBox1.Text));
                    if (main.getWorkspaceProjectName(i).Contains(projectNameTxtBox.Text))
                    {
                        exists = true;
                    }
                }
                if(exists){
                    infoLbl.Text = "Project already exists!";
                    infoLbl.Visible = true;
                    return;
                }

                main.addNewProjectToWorkspace(projectNameTxtBox.Text);
                main.setNewProject(false);
                this.Close();
            }
           
        }
        //Parse project name:
        private bool parseProjectName()
        {
            if(infoLbl.Visible){
                return false;
            }

            return true;
        }

        private void projectNameTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (projectNameTxtBox.Text != null && projectNameTxtBox.Text.Trim().Length >= 3)
            {
                infoLbl.Visible = false;
            }
            else
                infoLbl.Visible = true;
        }

        private void ProjectInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            main.setNewProject(false);
        }
    }
}
