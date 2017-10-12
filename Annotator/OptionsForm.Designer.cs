namespace Annotator
{
    partial class OptionsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.detectIgnoreObjRb = new System.Windows.Forms.RadioButton();
            this.detectOverwriteObjRb = new System.Windows.Forms.RadioButton();
            this.detectSeparatedObjRb = new System.Windows.Forms.RadioButton();
            this.browserBtn = new System.Windows.Forms.Button();
            this.glyphPrototypeTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.interpolateLeftRigRb = new System.Windows.Forms.RadioButton();
            this.interpolateLinearRigRb = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.interpolateLeftGlyphRb = new System.Windows.Forms.RadioButton();
            this.interpolateLinearGlyphRb = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.interpolateLeftRectRb = new System.Windows.Forms.RadioButton();
            this.interpolateLinearRectRb = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.upperBodyRb = new System.Windows.Forms.RadioButton();
            this.showAllRigRb = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.downObjLinkTypeBtn = new System.Windows.Forms.Button();
            this.upObjLinkTypeBtn = new System.Windows.Forms.Button();
            this.removeLinkTypeBtn = new System.Windows.Forms.Button();
            this.addLinkType = new System.Windows.Forms.Button();
            this.objectLinkTypeTxtBox = new System.Windows.Forms.TextBox();
            this.objectLinkTypeListBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.showMarker = new System.Windows.Forms.RadioButton();
            this.donShowMarker = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 5);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(978, 607);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(970, 574);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Detection";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.detectIgnoreObjRb);
            this.groupBox1.Controls.Add(this.detectOverwriteObjRb);
            this.groupBox1.Controls.Add(this.detectSeparatedObjRb);
            this.groupBox1.Controls.Add(this.browserBtn);
            this.groupBox1.Controls.Add(this.glyphPrototypeTxtBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(942, 278);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glyph box";
            // 
            // detectIgnoreObjRb
            // 
            this.detectIgnoreObjRb.AutoSize = true;
            this.detectIgnoreObjRb.Location = new System.Drawing.Point(44, 223);
            this.detectIgnoreObjRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.detectIgnoreObjRb.Name = "detectIgnoreObjRb";
            this.detectIgnoreObjRb.Size = new System.Drawing.Size(462, 24);
            this.detectIgnoreObjRb.TabIndex = 6;
            this.detectIgnoreObjRb.Text = "Ignore detected object if there are object with the same code";
            this.detectIgnoreObjRb.UseVisualStyleBackColor = true;
            // 
            // detectOverwriteObjRb
            // 
            this.detectOverwriteObjRb.AutoSize = true;
            this.detectOverwriteObjRb.Location = new System.Drawing.Point(44, 188);
            this.detectOverwriteObjRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.detectOverwriteObjRb.Name = "detectOverwriteObjRb";
            this.detectOverwriteObjRb.Size = new System.Drawing.Size(381, 24);
            this.detectOverwriteObjRb.TabIndex = 5;
            this.detectOverwriteObjRb.Text = "Overwrite object if they have the same glyph code";
            this.detectOverwriteObjRb.UseVisualStyleBackColor = true;
            // 
            // detectSeparatedObjRb
            // 
            this.detectSeparatedObjRb.AutoSize = true;
            this.detectSeparatedObjRb.Checked = true;
            this.detectSeparatedObjRb.Location = new System.Drawing.Point(44, 152);
            this.detectSeparatedObjRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.detectSeparatedObjRb.Name = "detectSeparatedObjRb";
            this.detectSeparatedObjRb.Size = new System.Drawing.Size(384, 24);
            this.detectSeparatedObjRb.TabIndex = 4;
            this.detectSeparatedObjRb.TabStop = true;
            this.detectSeparatedObjRb.Text = "Write each detection object as a separated object";
            this.detectSeparatedObjRb.UseVisualStyleBackColor = true;
            // 
            // browserBtn
            // 
            this.browserBtn.Location = new System.Drawing.Point(794, 42);
            this.browserBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.browserBtn.Name = "browserBtn";
            this.browserBtn.Size = new System.Drawing.Size(130, 35);
            this.browserBtn.TabIndex = 3;
            this.browserBtn.Text = "Browse";
            this.browserBtn.UseVisualStyleBackColor = true;
            // 
            // glyphPrototypeTxtBox
            // 
            this.glyphPrototypeTxtBox.Location = new System.Drawing.Point(194, 45);
            this.glyphPrototypeTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.glyphPrototypeTxtBox.Name = "glyphPrototypeTxtBox";
            this.glyphPrototypeTxtBox.Size = new System.Drawing.Size(589, 26);
            this.glyphPrototypeTxtBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Glyph prototype file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 108);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Detection overwrite mode";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(970, 574);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "View";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(9, 166);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(942, 365);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Object interpolation";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.interpolateLeftRigRb);
            this.groupBox6.Controls.Add(this.interpolateLinearRigRb);
            this.groupBox6.Location = new System.Drawing.Point(14, 251);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Size = new System.Drawing.Size(920, 98);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Rig boundary";
            // 
            // interpolateLeftRigRb
            // 
            this.interpolateLeftRigRb.AutoSize = true;
            this.interpolateLeftRigRb.Checked = true;
            this.interpolateLeftRigRb.Location = new System.Drawing.Point(9, 29);
            this.interpolateLeftRigRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.interpolateLeftRigRb.Name = "interpolateLeftRigRb";
            this.interpolateLeftRigRb.Size = new System.Drawing.Size(443, 24);
            this.interpolateLeftRigRb.TabIndex = 4;
            this.interpolateLeftRigRb.TabStop = true;
            this.interpolateLeftRigRb.Text = "Rig boundary is kept from the left marker (no interpolation)";
            this.interpolateLeftRigRb.UseMnemonic = false;
            this.interpolateLeftRigRb.UseVisualStyleBackColor = true;
            // 
            // interpolateLinearRigRb
            // 
            this.interpolateLinearRigRb.AutoSize = true;
            this.interpolateLinearRigRb.Location = new System.Drawing.Point(9, 65);
            this.interpolateLinearRigRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.interpolateLinearRigRb.Name = "interpolateLinearRigRb";
            this.interpolateLinearRigRb.Size = new System.Drawing.Size(283, 24);
            this.interpolateLinearRigRb.TabIndex = 5;
            this.interpolateLinearRigRb.Text = "Rig boundary is linearly interpolated";
            this.interpolateLinearRigRb.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.interpolateLeftGlyphRb);
            this.groupBox5.Controls.Add(this.interpolateLinearGlyphRb);
            this.groupBox5.Location = new System.Drawing.Point(14, 137);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Size = new System.Drawing.Size(920, 98);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Glyph boundary";
            // 
            // interpolateLeftGlyphRb
            // 
            this.interpolateLeftGlyphRb.AutoSize = true;
            this.interpolateLeftGlyphRb.Checked = true;
            this.interpolateLeftGlyphRb.Location = new System.Drawing.Point(9, 29);
            this.interpolateLeftGlyphRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.interpolateLeftGlyphRb.Name = "interpolateLeftGlyphRb";
            this.interpolateLeftGlyphRb.Size = new System.Drawing.Size(460, 24);
            this.interpolateLeftGlyphRb.TabIndex = 4;
            this.interpolateLeftGlyphRb.TabStop = true;
            this.interpolateLeftGlyphRb.Text = "Glyph boundary is kept from the left marker (no interpolation)";
            this.interpolateLeftGlyphRb.UseMnemonic = false;
            this.interpolateLeftGlyphRb.UseVisualStyleBackColor = true;
            // 
            // interpolateLinearGlyphRb
            // 
            this.interpolateLinearGlyphRb.AutoSize = true;
            this.interpolateLinearGlyphRb.Location = new System.Drawing.Point(9, 65);
            this.interpolateLinearGlyphRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.interpolateLinearGlyphRb.Name = "interpolateLinearGlyphRb";
            this.interpolateLinearGlyphRb.Size = new System.Drawing.Size(300, 24);
            this.interpolateLinearGlyphRb.TabIndex = 5;
            this.interpolateLinearGlyphRb.Text = "Glyph boundary is linearly interpolated";
            this.interpolateLinearGlyphRb.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.interpolateLeftRectRb);
            this.groupBox4.Controls.Add(this.interpolateLinearRectRb);
            this.groupBox4.Location = new System.Drawing.Point(14, 29);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(920, 98);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rectangle boundary";
            // 
            // interpolateLeftRectRb
            // 
            this.interpolateLeftRectRb.AutoSize = true;
            this.interpolateLeftRectRb.Checked = true;
            this.interpolateLeftRectRb.Location = new System.Drawing.Point(9, 29);
            this.interpolateLeftRectRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.interpolateLeftRectRb.Name = "interpolateLeftRectRb";
            this.interpolateLeftRectRb.Size = new System.Drawing.Size(492, 24);
            this.interpolateLeftRectRb.TabIndex = 4;
            this.interpolateLeftRectRb.TabStop = true;
            this.interpolateLeftRectRb.Text = "Rectangle boundary is kept from the left marker (no interpolation)";
            this.interpolateLeftRectRb.UseMnemonic = false;
            this.interpolateLeftRectRb.UseVisualStyleBackColor = true;
            // 
            // interpolateLinearRectRb
            // 
            this.interpolateLinearRectRb.AutoSize = true;
            this.interpolateLinearRectRb.Location = new System.Drawing.Point(9, 65);
            this.interpolateLinearRectRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.interpolateLinearRectRb.Name = "interpolateLinearRectRb";
            this.interpolateLinearRectRb.Size = new System.Drawing.Size(332, 24);
            this.interpolateLinearRectRb.TabIndex = 5;
            this.interpolateLinearRectRb.Text = "Rectangle boundary is linearly interpolated";
            this.interpolateLinearRectRb.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.upperBodyRb);
            this.groupBox2.Controls.Add(this.showAllRigRb);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(8, 9);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(942, 148);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rig ";
            // 
            // upperBodyRb
            // 
            this.upperBodyRb.AutoSize = true;
            this.upperBodyRb.Location = new System.Drawing.Point(44, 102);
            this.upperBodyRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.upperBodyRb.Name = "upperBodyRb";
            this.upperBodyRb.Size = new System.Drawing.Size(189, 24);
            this.upperBodyRb.TabIndex = 5;
            this.upperBodyRb.Text = "Only show upper body";
            this.upperBodyRb.UseVisualStyleBackColor = true;
            // 
            // showAllRigRb
            // 
            this.showAllRigRb.AutoSize = true;
            this.showAllRigRb.Checked = true;
            this.showAllRigRb.Location = new System.Drawing.Point(44, 66);
            this.showAllRigRb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.showAllRigRb.Name = "showAllRigRb";
            this.showAllRigRb.Size = new System.Drawing.Size(155, 24);
            this.showAllRigRb.TabIndex = 4;
            this.showAllRigRb.TabStop = true;
            this.showAllRigRb.Text = "Show all rig joints";
            this.showAllRigRb.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 42);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Show rig";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Size = new System.Drawing.Size(970, 574);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Data generator";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox8);
            this.tabPage4.Controls.Add(this.groupBox7);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Size = new System.Drawing.Size(970, 574);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Object";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.downObjLinkTypeBtn);
            this.groupBox7.Controls.Add(this.upObjLinkTypeBtn);
            this.groupBox7.Controls.Add(this.removeLinkTypeBtn);
            this.groupBox7.Controls.Add(this.addLinkType);
            this.groupBox7.Controls.Add(this.objectLinkTypeTxtBox);
            this.groupBox7.Controls.Add(this.objectLinkTypeListBox);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Location = new System.Drawing.Point(9, 9);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Size = new System.Drawing.Size(942, 302);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Object links";
            // 
            // downObjLinkTypeBtn
            // 
            this.downObjLinkTypeBtn.Enabled = false;
            this.downObjLinkTypeBtn.Location = new System.Drawing.Point(765, 232);
            this.downObjLinkTypeBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.downObjLinkTypeBtn.Name = "downObjLinkTypeBtn";
            this.downObjLinkTypeBtn.Size = new System.Drawing.Size(168, 34);
            this.downObjLinkTypeBtn.TabIndex = 6;
            this.downObjLinkTypeBtn.Text = "Down";
            this.downObjLinkTypeBtn.UseVisualStyleBackColor = true;
            this.downObjLinkTypeBtn.Click += new System.EventHandler(this.downObjLinkTypeBtn_Click);
            // 
            // upObjLinkTypeBtn
            // 
            this.upObjLinkTypeBtn.Enabled = false;
            this.upObjLinkTypeBtn.Location = new System.Drawing.Point(765, 189);
            this.upObjLinkTypeBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.upObjLinkTypeBtn.Name = "upObjLinkTypeBtn";
            this.upObjLinkTypeBtn.Size = new System.Drawing.Size(168, 34);
            this.upObjLinkTypeBtn.TabIndex = 5;
            this.upObjLinkTypeBtn.Text = "Up";
            this.upObjLinkTypeBtn.UseVisualStyleBackColor = true;
            this.upObjLinkTypeBtn.Click += new System.EventHandler(this.upObjLinkTypeBtn_Click);
            // 
            // removeLinkTypeBtn
            // 
            this.removeLinkTypeBtn.Enabled = false;
            this.removeLinkTypeBtn.Location = new System.Drawing.Point(765, 146);
            this.removeLinkTypeBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.removeLinkTypeBtn.Name = "removeLinkTypeBtn";
            this.removeLinkTypeBtn.Size = new System.Drawing.Size(168, 34);
            this.removeLinkTypeBtn.TabIndex = 4;
            this.removeLinkTypeBtn.Text = "Remove";
            this.removeLinkTypeBtn.UseVisualStyleBackColor = true;
            this.removeLinkTypeBtn.Click += new System.EventHandler(this.removeLinkType_Click);
            // 
            // addLinkType
            // 
            this.addLinkType.Location = new System.Drawing.Point(765, 91);
            this.addLinkType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.addLinkType.Name = "addLinkType";
            this.addLinkType.Size = new System.Drawing.Size(168, 34);
            this.addLinkType.TabIndex = 3;
            this.addLinkType.Text = "Add";
            this.addLinkType.UseVisualStyleBackColor = true;
            this.addLinkType.Click += new System.EventHandler(this.addLinkType_Click);
            // 
            // objectLinkTypeTxtBox
            // 
            this.objectLinkTypeTxtBox.Location = new System.Drawing.Point(14, 92);
            this.objectLinkTypeTxtBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.objectLinkTypeTxtBox.Name = "objectLinkTypeTxtBox";
            this.objectLinkTypeTxtBox.Size = new System.Drawing.Size(739, 26);
            this.objectLinkTypeTxtBox.TabIndex = 2;
            // 
            // objectLinkTypeListBox
            // 
            this.objectLinkTypeListBox.FormattingEnabled = true;
            this.objectLinkTypeListBox.ItemHeight = 20;
            this.objectLinkTypeListBox.Items.AddRange(new object[] {
            "ON",
            "IN",
            "ATTACH_TO"});
            this.objectLinkTypeListBox.Location = new System.Drawing.Point(14, 146);
            this.objectLinkTypeListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.objectLinkTypeListBox.Name = "objectLinkTypeListBox";
            this.objectLinkTypeListBox.Size = new System.Drawing.Size(739, 144);
            this.objectLinkTypeListBox.TabIndex = 1;
            this.objectLinkTypeListBox.SelectedIndexChanged += new System.EventHandler(this.objectLinkTypeListBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 42);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Link labels";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Controls.Add(this.cancelBtn, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.saveBtn, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(986, 663);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(870, 622);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(111, 35);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(750, 622);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(111, 35);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.showMarker);
            this.groupBox8.Controls.Add(this.donShowMarker);
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.Location = new System.Drawing.Point(14, 319);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox8.Size = new System.Drawing.Size(942, 106);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Object annotation markers ";
            // 
            // radioButton1
            // 
            this.showMarker.AutoSize = true;
            this.showMarker.Location = new System.Drawing.Point(222, 66);
            this.showMarker.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.showMarker.Name = "radioButton1";
            this.showMarker.Size = new System.Drawing.Size(189, 24);
            this.showMarker.TabIndex = 5;
            this.showMarker.Text = "Show";
            this.showMarker.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.donShowMarker.AutoSize = true;
            this.donShowMarker.Checked = true;
            this.donShowMarker.Location = new System.Drawing.Point(44, 66);
            this.donShowMarker.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.donShowMarker.Name = "radioButton2";
            this.donShowMarker.Size = new System.Drawing.Size(155, 24);
            this.donShowMarker.TabIndex = 4;
            this.donShowMarker.TabStop = true;
            this.donShowMarker.Text = "Don't show";
            this.donShowMarker.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 42);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Show rig";
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.saveBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(986, 663);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox glyphPrototypeTxtBox;
        private System.Windows.Forms.Button browserBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.RadioButton detectIgnoreObjRb;
        private System.Windows.Forms.RadioButton detectOverwriteObjRb;
        private System.Windows.Forms.RadioButton detectSeparatedObjRb;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton upperBodyRb;
        private System.Windows.Forms.RadioButton showAllRigRb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton interpolateLinearRectRb;
        private System.Windows.Forms.RadioButton interpolateLeftRectRb;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton interpolateLeftGlyphRb;
        private System.Windows.Forms.RadioButton interpolateLinearGlyphRb;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton interpolateLeftRigRb;
        private System.Windows.Forms.RadioButton interpolateLinearRigRb;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button removeLinkTypeBtn;
        private System.Windows.Forms.Button addLinkType;
        private System.Windows.Forms.TextBox objectLinkTypeTxtBox;
        private System.Windows.Forms.ListBox objectLinkTypeListBox;
        private System.Windows.Forms.Button downObjLinkTypeBtn;
        private System.Windows.Forms.Button upObjLinkTypeBtn;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton showMarker;
        private System.Windows.Forms.RadioButton donShowMarker;
        private System.Windows.Forms.Label label5;
    }
}