namespace Annotator
{
    partial class SessionSelector
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
            this.sessionSelectComboBox = new System.Windows.Forms.ComboBox();
            this.copyObjectBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sessionSelectComboBox
            // 
            this.sessionSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionSelectComboBox.FormattingEnabled = true;
            this.sessionSelectComboBox.Location = new System.Drawing.Point(160, 5);
            this.sessionSelectComboBox.Name = "sessionSelectComboBox";
            this.sessionSelectComboBox.Size = new System.Drawing.Size(361, 21);
            this.sessionSelectComboBox.TabIndex = 9;
            // 
            // copyObjectBtn
            // 
            this.copyObjectBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.copyObjectBtn.Location = new System.Drawing.Point(434, 37);
            this.copyObjectBtn.Name = "copyObjectBtn";
            this.copyObjectBtn.Size = new System.Drawing.Size(87, 20);
            this.copyObjectBtn.TabIndex = 10;
            this.copyObjectBtn.Text = "Copy";
            this.copyObjectBtn.UseVisualStyleBackColor = true;
            this.copyObjectBtn.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Select session";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.copyObjectBtn, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sessionSelectComboBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(524, 63);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // SessionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 63);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SessionSelector";
            this.Text = "Copy from session";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox sessionSelectComboBox;
        private System.Windows.Forms.Button copyObjectBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}