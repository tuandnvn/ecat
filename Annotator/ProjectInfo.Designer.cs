namespace Annotator
{
    partial class ProjectInfo
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
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.projectNameTxtBox = new System.Windows.Forms.TextBox();
            this.infoLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okBtn.Location = new System.Drawing.Point(80, 69);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(87, 23);
            this.okBtn.TabIndex = 9;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(173, 69);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(87, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Project name: ";
            // 
            // projectNameTxtBox
            // 
            this.projectNameTxtBox.Location = new System.Drawing.Point(94, 25);
            this.projectNameTxtBox.Name = "projectNameTxtBox";
            this.projectNameTxtBox.Size = new System.Drawing.Size(166, 20);
            this.projectNameTxtBox.TabIndex = 11;
            this.projectNameTxtBox.TextChanged += new System.EventHandler(this.projectNameTxtBox_TextChanged);
            // 
            // infoLbl
            // 
            this.infoLbl.AutoSize = true;
            this.infoLbl.ForeColor = System.Drawing.Color.Red;
            this.infoLbl.Location = new System.Drawing.Point(91, 48);
            this.infoLbl.Name = "label2";
            this.infoLbl.Size = new System.Drawing.Size(211, 13);
            this.infoLbl.TabIndex = 12;
            this.infoLbl.Text = "Incorrect project name(at least three letters)";
            // 
            // ProjectInfo
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(335, 104);
            this.Controls.Add(this.infoLbl);
            this.Controls.Add(this.projectNameTxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.cancelBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProjectInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Create new project";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectInfo_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox projectNameTxtBox;
        private System.Windows.Forms.Label infoLbl;
    }
}