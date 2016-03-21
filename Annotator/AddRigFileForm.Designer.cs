namespace Annotator
{
    partial class AddRigFileForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rigSchemeBrowseBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.copyLocalChkBox = new System.Windows.Forms.CheckBox();
            this.rigSrcTxtBox = new System.Windows.Forms.TextBox();
            this.rigSchemeTxtBox = new System.Windows.Forms.TextBox();
            this.rigSrcBrowseBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.04255F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.95744F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel1.Controls.Add(this.rigSchemeBrowseBtn, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.copyLocalChkBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rigSrcTxtBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rigSchemeTxtBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.rigSrcBrowseBtn, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.addBtn, 2, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(466, 124);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rigSchemeBrowseBtn
            // 
            this.rigSchemeBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rigSchemeBrowseBtn.Location = new System.Drawing.Point(396, 35);
            this.rigSchemeBrowseBtn.Name = "rigSchemeBrowseBtn";
            this.rigSchemeBrowseBtn.Size = new System.Drawing.Size(67, 22);
            this.rigSchemeBrowseBtn.TabIndex = 6;
            this.rigSchemeBrowseBtn.Text = "Browse";
            this.rigSchemeBrowseBtn.UseVisualStyleBackColor = true;
            this.rigSchemeBrowseBtn.Click += new System.EventHandler(this.rigSchemeBrowseBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rig source file";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rig scheme file";
            // 
            // copyLocalChkBox
            // 
            this.copyLocalChkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.copyLocalChkBox.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.copyLocalChkBox, 3);
            this.copyLocalChkBox.Location = new System.Drawing.Point(3, 69);
            this.copyLocalChkBox.Name = "copyLocalChkBox";
            this.copyLocalChkBox.Size = new System.Drawing.Size(460, 17);
            this.copyLocalChkBox.TabIndex = 2;
            this.copyLocalChkBox.Text = "Copy into current session";
            this.copyLocalChkBox.UseVisualStyleBackColor = true;
            // 
            // rigSrcTxtBox
            // 
            this.rigSrcTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rigSrcTxtBox.Location = new System.Drawing.Point(137, 5);
            this.rigSrcTxtBox.Name = "rigSrcTxtBox";
            this.rigSrcTxtBox.Size = new System.Drawing.Size(253, 20);
            this.rigSrcTxtBox.TabIndex = 3;
            // 
            // rigSchemeTxtBox
            // 
            this.rigSchemeTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rigSchemeTxtBox.Location = new System.Drawing.Point(137, 36);
            this.rigSchemeTxtBox.Name = "rigSchemeTxtBox";
            this.rigSchemeTxtBox.Size = new System.Drawing.Size(253, 20);
            this.rigSchemeTxtBox.TabIndex = 4;
            // 
            // rigSrcBrowseBtn
            // 
            this.rigSrcBrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rigSrcBrowseBtn.Location = new System.Drawing.Point(396, 4);
            this.rigSrcBrowseBtn.Name = "rigSrcBrowseBtn";
            this.rigSrcBrowseBtn.Size = new System.Drawing.Size(67, 22);
            this.rigSrcBrowseBtn.TabIndex = 5;
            this.rigSrcBrowseBtn.Text = "Browse";
            this.rigSrcBrowseBtn.UseVisualStyleBackColor = true;
            this.rigSrcBrowseBtn.Click += new System.EventHandler(this.rigSrcBrowseBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(396, 97);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(67, 22);
            this.addBtn.TabIndex = 7;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // AddRigFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 125);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AddRigFileForm";
            this.Text = "Add rig objects from file";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox copyLocalChkBox;
        private System.Windows.Forms.TextBox rigSrcTxtBox;
        private System.Windows.Forms.TextBox rigSchemeTxtBox;
        private System.Windows.Forms.Button rigSchemeBrowseBtn;
        private System.Windows.Forms.Button rigSrcBrowseBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}