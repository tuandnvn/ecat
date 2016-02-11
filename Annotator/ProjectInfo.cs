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
        public ProjectInfo(Main frm1)
        {
            InitializeComponent();
            this.frm1 = frm1;
            this.ActiveControl = this.textBox1;
        }

        private Main frm1 = null;

        private void button2_Click(object sender, EventArgs e)
        {
            frm1.setNewProject(false);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (parseProjectName())
            {
                //Check if such project already exists in workspace:
                bool exists = false;
                for (int i = 0; i < frm1.getWorkspaceProjectSize(); i++)
                {
                    //MessageBox.Show(frm1.getWorkspaceProjectName(i) + ", textBox1.Text = " + textBox1.Text + frm1.getWorkspaceProjectName(i).Contains(textBox1.Text));
                    if (frm1.getWorkspaceProjectName(i).Contains(textBox1.Text))
                    {
                        exists = true;
                    }
                }
                if(exists){
                    label2.Text = "Project already exists!";
                    label2.Visible = true;
                    return;
                }

                frm1.addNewProjectToWorkspace(textBox1.Text);
                frm1.setNewProject(false);
                this.Close();
            }
           
        }
        //Parse project name:
        private bool parseProjectName()
        {
            if(label2.Visible){
                return false;
            }

            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text.Trim().Length >= 3)
            {
                label2.Visible = false;
            }
            else
                label2.Visible = true;
        }

        private void ProjectInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm1.setNewProject(false);
        }
    }
}
