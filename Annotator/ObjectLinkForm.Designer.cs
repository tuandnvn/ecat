namespace Annotator
{
    partial class ObjectLinkForm
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
            this.linkComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.qualifiedSelectComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.objectSelectComboBox = new System.Windows.Forms.ComboBox();
            this.addLinkBtn = new System.Windows.Forms.Button();
            this.objectNameLbl = new System.Windows.Forms.Label();
            this.sessionSelectComboBox = new System.Windows.Forms.ComboBox();
            this.crossSessionChkBox = new System.Windows.Forms.CheckBox();
            this.infoLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.linkComboBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.qualifiedSelectComboBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.addLinkBtn, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.objectSelectComboBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.objectNameLbl, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.sessionSelectComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.crossSessionChkBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.infoLbl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(547, 180);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // linkComboBox
            // 
            this.linkComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkComboBox.FormattingEnabled = true;
            this.linkComboBox.Location = new System.Drawing.Point(167, 128);
            this.linkComboBox.Name = "linkComboBox";
            this.linkComboBox.Size = new System.Drawing.Size(377, 21);
            this.linkComboBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Spatial link type";
            // 
            // qualifiedSelectComboBox
            // 
            this.qualifiedSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.qualifiedSelectComboBox.FormattingEnabled = true;
            this.qualifiedSelectComboBox.Location = new System.Drawing.Point(167, 103);
            this.qualifiedSelectComboBox.Name = "qualifiedSelectComboBox";
            this.qualifiedSelectComboBox.Size = new System.Drawing.Size(377, 21);
            this.qualifiedSelectComboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Qualified";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Link to object";
            // 
            // objectSelectComboBox
            // 
            this.objectSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.objectSelectComboBox.FormattingEnabled = true;
            this.objectSelectComboBox.Location = new System.Drawing.Point(167, 53);
            this.objectSelectComboBox.Name = "objectSelectComboBox";
            this.objectSelectComboBox.Size = new System.Drawing.Size(377, 21);
            this.objectSelectComboBox.TabIndex = 1;
            // 
            // addLinkBtn
            // 
            this.addLinkBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.addLinkBtn.Location = new System.Drawing.Point(469, 155);
            this.addLinkBtn.Name = "addLinkBtn";
            this.addLinkBtn.Size = new System.Drawing.Size(75, 20);
            this.addLinkBtn.TabIndex = 6;
            this.addLinkBtn.Text = "Add link";
            this.addLinkBtn.UseVisualStyleBackColor = true;
            this.addLinkBtn.Click += new System.EventHandler(this.addLinkBtn_Click);
            // 
            // objectNameLbl
            // 
            this.objectNameLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.objectNameLbl.AutoSize = true;
            this.objectNameLbl.Location = new System.Drawing.Point(167, 81);
            this.objectNameLbl.Name = "objectNameLbl";
            this.objectNameLbl.Size = new System.Drawing.Size(0, 13);
            this.objectNameLbl.TabIndex = 7;
            this.objectNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sessionSelectComboBox
            // 
            this.sessionSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionSelectComboBox.Enabled = false;
            this.sessionSelectComboBox.FormattingEnabled = true;
            this.sessionSelectComboBox.Location = new System.Drawing.Point(167, 28);
            this.sessionSelectComboBox.Name = "sessionSelectComboBox";
            this.sessionSelectComboBox.Size = new System.Drawing.Size(377, 21);
            this.sessionSelectComboBox.TabIndex = 8;
            this.sessionSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.sessionSelectComboBox_SelectedIndexChanged);
            // 
            // crossSessionChkBox
            // 
            this.crossSessionChkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.crossSessionChkBox.AutoSize = true;
            this.crossSessionChkBox.Location = new System.Drawing.Point(3, 29);
            this.crossSessionChkBox.Name = "crossSessionChkBox";
            this.crossSessionChkBox.Size = new System.Drawing.Size(109, 17);
            this.crossSessionChkBox.TabIndex = 9;
            this.crossSessionChkBox.Text = "Cross session link";
            this.crossSessionChkBox.UseVisualStyleBackColor = true;
            this.crossSessionChkBox.CheckedChanged += new System.EventHandler(this.crossSessionChkBox_CheckedChanged);
            // 
            // infoLbl
            // 
            this.infoLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.infoLbl.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.infoLbl, 2);
            this.infoLbl.Location = new System.Drawing.Point(3, 6);
            this.infoLbl.Name = "infoLbl";
            this.infoLbl.Size = new System.Drawing.Size(150, 13);
            this.infoLbl.TabIndex = 10;
            this.infoLbl.Text = "Add link from object of session";
            // 
            // ObjectLinkForm
            // 
            this.AcceptButton = this.addLinkBtn;
            this.ClientSize = new System.Drawing.Size(547, 180);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ObjectLinkForm";
            this.Text = "Add link";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox linkComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox qualifiedSelectComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox objectSelectComboBox;
        private System.Windows.Forms.Button addLinkBtn;
        private System.Windows.Forms.Label objectNameLbl;
        private System.Windows.Forms.ComboBox sessionSelectComboBox;
        private System.Windows.Forms.CheckBox crossSessionChkBox;
        private System.Windows.Forms.Label infoLbl;
    }
}