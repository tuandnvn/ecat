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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.existingPredicateListBox = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.removePredBtn = new System.Windows.Forms.Button();
            this.infoLbl = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.crossSessionChkBox = new System.Windows.Forms.CheckBox();
            this.qualifiedSelectComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sessionSelectComboBox = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.addLinkBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.objectSelectComboBox = new System.Windows.Forms.ComboBox();
            this.predicateLbl = new System.Windows.Forms.Label();
            this.linkComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.infoLbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(479, 385);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(473, 174);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Existing";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Controls.Add(this.existingPredicateListBox, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(467, 155);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // existingPredicateListBox
            // 
            this.existingPredicateListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.existingPredicateListBox.FormattingEnabled = true;
            this.existingPredicateListBox.Location = new System.Drawing.Point(3, 3);
            this.existingPredicateListBox.Name = "existingPredicateListBox";
            this.existingPredicateListBox.Size = new System.Drawing.Size(320, 149);
            this.existingPredicateListBox.TabIndex = 2;
            this.existingPredicateListBox.SelectedIndexChanged += new System.EventHandler(this.existingPredicateListBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.removePredBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(329, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(135, 149);
            this.panel1.TabIndex = 3;
            // 
            // removePredBtn
            // 
            this.removePredBtn.Enabled = false;
            this.removePredBtn.Location = new System.Drawing.Point(3, 3);
            this.removePredBtn.Name = "removePredBtn";
            this.removePredBtn.Size = new System.Drawing.Size(129, 22);
            this.removePredBtn.TabIndex = 7;
            this.removePredBtn.Text = "Remove";
            this.removePredBtn.UseVisualStyleBackColor = true;
            this.removePredBtn.Click += new System.EventHandler(this.removePredBtn_Click);
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
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(473, 174);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.Controls.Add(this.crossSessionChkBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.qualifiedSelectComboBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.sessionSelectComboBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.objectSelectComboBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.predicateLbl, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.linkComboBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(467, 155);
            this.tableLayoutPanel2.TabIndex = 0;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // crossSessionChkBox
            // 
            this.crossSessionChkBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.crossSessionChkBox.AutoSize = true;
            this.crossSessionChkBox.Location = new System.Drawing.Point(3, 4);
            this.crossSessionChkBox.Name = "crossSessionChkBox";
            this.crossSessionChkBox.Size = new System.Drawing.Size(109, 17);
            this.crossSessionChkBox.TabIndex = 9;
            this.crossSessionChkBox.Text = "Cross session link";
            this.crossSessionChkBox.UseVisualStyleBackColor = true;
            this.crossSessionChkBox.CheckedChanged += new System.EventHandler(this.crossSessionChkBox_CheckedChanged);
            // 
            // qualifiedSelectComboBox
            // 
            this.qualifiedSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.qualifiedSelectComboBox.FormattingEnabled = true;
            this.qualifiedSelectComboBox.Location = new System.Drawing.Point(143, 53);
            this.qualifiedSelectComboBox.Name = "qualifiedSelectComboBox";
            this.qualifiedSelectComboBox.Size = new System.Drawing.Size(321, 21);
            this.qualifiedSelectComboBox.TabIndex = 3;
            this.qualifiedSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.qualifiedSelectComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Qualified";
            // 
            // sessionSelectComboBox
            // 
            this.sessionSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionSelectComboBox.Enabled = false;
            this.sessionSelectComboBox.FormattingEnabled = true;
            this.sessionSelectComboBox.Location = new System.Drawing.Point(143, 3);
            this.sessionSelectComboBox.Name = "sessionSelectComboBox";
            this.sessionSelectComboBox.Size = new System.Drawing.Size(321, 21);
            this.sessionSelectComboBox.TabIndex = 8;
            this.sessionSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.sessionSelectComboBox_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.addLinkBtn);
            this.panel2.Controls.Add(this.cancelBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(143, 128);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(321, 24);
            this.panel2.TabIndex = 13;
            // 
            // addLinkBtn
            // 
            this.addLinkBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.addLinkBtn.Location = new System.Drawing.Point(162, 1);
            this.addLinkBtn.Name = "addLinkBtn";
            this.addLinkBtn.Size = new System.Drawing.Size(75, 20);
            this.addLinkBtn.TabIndex = 6;
            this.addLinkBtn.Text = "Add link";
            this.addLinkBtn.UseVisualStyleBackColor = true;
            this.addLinkBtn.Click += new System.EventHandler(this.addLinkBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(243, 1);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 19);
            this.cancelBtn.TabIndex = 12;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // objectSelectComboBox
            // 
            this.objectSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.objectSelectComboBox.FormattingEnabled = true;
            this.objectSelectComboBox.Location = new System.Drawing.Point(143, 78);
            this.objectSelectComboBox.Name = "objectSelectComboBox";
            this.objectSelectComboBox.Size = new System.Drawing.Size(321, 21);
            this.objectSelectComboBox.TabIndex = 1;
            this.objectSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.objectSelectComboBox_SelectedIndexChanged);
            // 
            // predicateLbl
            // 
            this.predicateLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.predicateLbl.AutoSize = true;
            this.predicateLbl.Location = new System.Drawing.Point(143, 106);
            this.predicateLbl.Name = "predicateLbl";
            this.predicateLbl.Size = new System.Drawing.Size(321, 13);
            this.predicateLbl.TabIndex = 11;
            // 
            // linkComboBox
            // 
            this.linkComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkComboBox.FormattingEnabled = true;
            this.linkComboBox.Location = new System.Drawing.Point(143, 28);
            this.linkComboBox.Name = "linkComboBox";
            this.linkComboBox.Size = new System.Drawing.Size(321, 21);
            this.linkComboBox.TabIndex = 5;
            this.linkComboBox.SelectedIndexChanged += new System.EventHandler(this.linkComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Link to object";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Predicate type";
            // 
            // ObjectLinkForm
            // 
            this.AcceptButton = this.addLinkBtn;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(479, 385);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ObjectLinkForm";
            this.Text = "Add link";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
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
        private System.Windows.Forms.ComboBox sessionSelectComboBox;
        private System.Windows.Forms.CheckBox crossSessionChkBox;
        private System.Windows.Forms.Label infoLbl;
        private System.Windows.Forms.Label predicateLbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox existingPredicateListBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button removePredBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Panel panel2;
    }
}