namespace Annotator
{
    partial class WorkspaceLauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.workspacePath = new System.Windows.Forms.TextBox();
            this.workspaceBrowser = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cancelWsBtn = new System.Windows.Forms.Button();
            this.selectWsBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a workspace";
            // 
            // infoLbl
            // 
            this.label2.AllowDrop = true;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "A workspace includes a set of projects ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Workspace:";
            // 
            // workspacePath
            // 
            this.workspacePath.Location = new System.Drawing.Point(74, 4);
            this.workspacePath.Name = "workspacePath";
            this.workspacePath.Size = new System.Drawing.Size(392, 20);
            this.workspacePath.TabIndex = 3;
            // 
            // workspaceBrowser
            // 
            this.workspaceBrowser.Location = new System.Drawing.Point(472, 3);
            this.workspaceBrowser.Name = "workspaceBrowser";
            this.workspaceBrowser.Size = new System.Drawing.Size(75, 23);
            this.workspaceBrowser.TabIndex = 4;
            this.workspaceBrowser.Text = "Browse";
            this.workspaceBrowser.UseVisualStyleBackColor = true;
            this.workspaceBrowser.Click += new System.EventHandler(this.wsBrowserBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.workspaceBrowser);
            this.panel1.Controls.Add(this.workspacePath);
            this.panel1.Location = new System.Drawing.Point(17, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 78);
            this.panel1.TabIndex = 5;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 47);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(289, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Use this as the default workspace and do not ask again";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cancelWsBtn
            // 
            this.cancelWsBtn.Location = new System.Drawing.Point(489, 208);
            this.cancelWsBtn.Name = "cancelWsBtn";
            this.cancelWsBtn.Size = new System.Drawing.Size(87, 23);
            this.cancelWsBtn.TabIndex = 6;
            this.cancelWsBtn.Text = "Cancel";
            this.cancelWsBtn.UseVisualStyleBackColor = true;
            this.cancelWsBtn.Click += new System.EventHandler(this.cancelWsBtn_Click);
            // 
            // selectWsBtn
            // 
            this.selectWsBtn.Location = new System.Drawing.Point(396, 208);
            this.selectWsBtn.Name = "selectWsBtn";
            this.selectWsBtn.Size = new System.Drawing.Size(87, 23);
            this.selectWsBtn.TabIndex = 7;
            this.selectWsBtn.Text = "OK";
            this.selectWsBtn.UseVisualStyleBackColor = true;
            this.selectWsBtn.Click += new System.EventHandler(this.selectWsBtn_Click);
            // 
            // WorkspaceLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 239);
            this.Controls.Add(this.selectWsBtn);
            this.Controls.Add(this.cancelWsBtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WorkspaceLauncher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workspace Launcher";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.WorkspaceLauncher_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox workspacePath;
        private System.Windows.Forms.Button workspaceBrowser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button cancelWsBtn;
        private System.Windows.Forms.Button selectWsBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}