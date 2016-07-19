namespace Annotator
{
    partial class EventTemplateGenerator
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
            this.templateDescriptionTxtBox = new System.Windows.Forms.TextBox();
            this.durationTxtBox = new System.Windows.Forms.TextBox();
            this.skippingLengthTxtBox = new System.Windows.Forms.TextBox();
            this.genBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.startFrameTxtBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ignoreEventRb = new System.Windows.Forms.RadioButton();
            this.overwriteEventRb = new System.Windows.Forms.RadioButton();
            this.separateEventRb = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.removeExistingRb = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tableLayoutPanel1.Controls.Add(this.templateDescriptionTxtBox, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.durationTxtBox, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.skippingLengthTxtBox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.genBtn, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.cancelBtn, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.startFrameTxtBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(361, 316);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // templateDescriptionTxtBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.templateDescriptionTxtBox, 2);
            this.templateDescriptionTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateDescriptionTxtBox.Location = new System.Drawing.Point(163, 123);
            this.templateDescriptionTxtBox.Name = "templateDescriptionTxtBox";
            this.templateDescriptionTxtBox.Size = new System.Drawing.Size(195, 20);
            this.templateDescriptionTxtBox.TabIndex = 10;
            // 
            // durationTxtBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.durationTxtBox, 2);
            this.durationTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.durationTxtBox.Location = new System.Drawing.Point(163, 93);
            this.durationTxtBox.Name = "durationTxtBox";
            this.durationTxtBox.Size = new System.Drawing.Size(195, 20);
            this.durationTxtBox.TabIndex = 9;
            this.durationTxtBox.Enter += new System.EventHandler(this.durationTxtBox_Enter);
            // 
            // skippingLengthTxtBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.skippingLengthTxtBox, 2);
            this.skippingLengthTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skippingLengthTxtBox.Location = new System.Drawing.Point(163, 63);
            this.skippingLengthTxtBox.Name = "skippingLengthTxtBox";
            this.skippingLengthTxtBox.Size = new System.Drawing.Size(195, 20);
            this.skippingLengthTxtBox.TabIndex = 8;
            this.skippingLengthTxtBox.Enter += new System.EventHandler(this.skippingLengthTxtBox_Enter);
            // 
            // genBtn
            // 
            this.genBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genBtn.Location = new System.Drawing.Point(163, 288);
            this.genBtn.Name = "genBtn";
            this.genBtn.Size = new System.Drawing.Size(94, 25);
            this.genBtn.TabIndex = 0;
            this.genBtn.Text = "Generate";
            this.genBtn.UseVisualStyleBackColor = true;
            this.genBtn.Click += new System.EventHandler(this.genBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelBtn.Location = new System.Drawing.Point(263, 288);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(95, 25);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Location = new System.Drawing.Point(3, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start frame";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Location = new System.Drawing.Point(3, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Event duration length";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
            this.label3.Location = new System.Drawing.Point(3, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Skipping step";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 4);
            this.label4.Location = new System.Drawing.Point(3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Generate a set of empty events for annotating";
            // 
            // startFrameTxtBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.startFrameTxtBox, 2);
            this.startFrameTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startFrameTxtBox.Location = new System.Drawing.Point(163, 33);
            this.startFrameTxtBox.Name = "startFrameTxtBox";
            this.startFrameTxtBox.Size = new System.Drawing.Size(195, 20);
            this.startFrameTxtBox.TabIndex = 7;
            this.startFrameTxtBox.Enter += new System.EventHandler(this.startFrameTxtBox_Enter);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.Location = new System.Drawing.Point(3, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Template description";
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 4);
            this.groupBox1.Controls.Add(this.removeExistingRb);
            this.groupBox1.Controls.Add(this.ignoreEventRb);
            this.groupBox1.Controls.Add(this.overwriteEventRb);
            this.groupBox1.Controls.Add(this.separateEventRb);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 129);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Overwrite";
            // 
            // ignoreEventRb
            // 
            this.ignoreEventRb.AutoSize = true;
            this.ignoreEventRb.Location = new System.Drawing.Point(23, 78);
            this.ignoreEventRb.Name = "ignoreEventRb";
            this.ignoreEventRb.Size = new System.Drawing.Size(293, 17);
            this.ignoreEventRb.TabIndex = 3;
            this.ignoreEventRb.Text = "Ignore events if they have the same start and end frames";
            this.ignoreEventRb.UseVisualStyleBackColor = true;
            // 
            // overwriteEventRb
            // 
            this.overwriteEventRb.AutoSize = true;
            this.overwriteEventRb.Checked = true;
            this.overwriteEventRb.Location = new System.Drawing.Point(23, 55);
            this.overwriteEventRb.Name = "overwriteEventRb";
            this.overwriteEventRb.Size = new System.Drawing.Size(308, 17);
            this.overwriteEventRb.TabIndex = 2;
            this.overwriteEventRb.TabStop = true;
            this.overwriteEventRb.Text = "Overwrite events if they have the same start and end frames";
            this.overwriteEventRb.UseVisualStyleBackColor = true;
            // 
            // separateEventRb
            // 
            this.separateEventRb.AutoSize = true;
            this.separateEventRb.Location = new System.Drawing.Point(23, 32);
            this.separateEventRb.Name = "separateEventRb";
            this.separateEventRb.Size = new System.Drawing.Size(149, 17);
            this.separateEventRb.TabIndex = 1;
            this.separateEventRb.Text = "Write as separated events";
            this.separateEventRb.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Overwrite existing events";
            // 
            // removeExistingRb
            // 
            this.removeExistingRb.AutoSize = true;
            this.removeExistingRb.Location = new System.Drawing.Point(23, 101);
            this.removeExistingRb.Name = "removeExistingRb";
            this.removeExistingRb.Size = new System.Drawing.Size(237, 17);
            this.removeExistingRb.TabIndex = 4;
            this.removeExistingRb.Text = "Remove all existing events before generating";
            this.removeExistingRb.UseVisualStyleBackColor = true;
            // 
            // EventTemplateGenerator
            // 
            this.AcceptButton = this.genBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(361, 316);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EventTemplateGenerator";
            this.Text = "EventTemplateGenerator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button genBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox durationTxtBox;
        private System.Windows.Forms.TextBox skippingLengthTxtBox;
        private System.Windows.Forms.TextBox startFrameTxtBox;
        private System.Windows.Forms.TextBox templateDescriptionTxtBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton ignoreEventRb;
        private System.Windows.Forms.RadioButton overwriteEventRb;
        private System.Windows.Forms.RadioButton separateEventRb;
        private System.Windows.Forms.RadioButton removeExistingRb;
    }
}