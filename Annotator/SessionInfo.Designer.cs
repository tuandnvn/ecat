namespace Annotator
{
    partial class SessionInfo
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
            this.sessionNameTxtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.warningTxt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sessionNameTxtBox
            // 
            this.sessionNameTxtBox.Location = new System.Drawing.Point(112, 23);
            this.sessionNameTxtBox.Name = "sessionNameTxtBox";
            this.sessionNameTxtBox.Size = new System.Drawing.Size(211, 20);
            this.sessionNameTxtBox.TabIndex = 16;
            this.sessionNameTxtBox.TextChanged += new System.EventHandler(this.sessionNameTxtBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Session name: ";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(143, 69);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(87, 23);
            this.okButton.TabIndex = 14;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Visible = false;
            this.okButton.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(236, 69);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(87, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // warningTxt
            // 
            this.warningTxt.AutoSize = true;
            this.warningTxt.ForeColor = System.Drawing.Color.Red;
            this.warningTxt.Location = new System.Drawing.Point(112, 46);
            this.warningTxt.Name = "warningTxt";
            this.warningTxt.Size = new System.Drawing.Size(211, 13);
            this.warningTxt.TabIndex = 18;
            this.warningTxt.Text = "Incorrect project name(at least three letters)";
            // 
            // SessionInfo
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(335, 97);
            this.Controls.Add(this.warningTxt);
            this.Controls.Add(this.sessionNameTxtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SessionInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Create new session";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SessionInfo_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sessionNameTxtBox;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button okButton;
        internal System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label warningTxt;
    }
}