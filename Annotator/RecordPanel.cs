using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    class RecordPanel : UserControl
    {
        private Panel recordingButtonGroup;
        private Button playButton;
        private Button recordButton;
        private GroupBox recordedBox;
        private GroupBox helpBox;
        private RichTextBox richTextBox1;
        private Panel viewPanel;
        private GroupBox viewBox;
        private ImageList imageList2;
        private System.ComponentModel.IContainer components;
        private Button saveRecordedSession;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private TrackBar playBar;
        private GroupBox detectBox;
        private Panel detectedPanel;
        private GroupBox cropBox;
        private GroupBox optionBox;
        private DataGridView optionsTable;
        private DataGridViewTextBoxColumn PropertyName;
        private DataGridViewTextBoxColumn Value;
        private Button calibrateButton;

        public RecordPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordPanel));
            this.recordingButtonGroup = new System.Windows.Forms.Panel();
            this.playButton = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.recordButton = new System.Windows.Forms.Button();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.recordedBox = new System.Windows.Forms.GroupBox();
            this.cropBox = new System.Windows.Forms.GroupBox();
            this.detectBox = new System.Windows.Forms.GroupBox();
            this.detectedPanel = new System.Windows.Forms.Panel();
            this.playBar = new System.Windows.Forms.TrackBar();
            this.saveRecordedSession = new System.Windows.Forms.Button();
            this.helpBox = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.viewPanel = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.viewBox = new System.Windows.Forms.GroupBox();
            this.optionBox = new System.Windows.Forms.GroupBox();
            this.optionsTable = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordingButtonGroup.SuspendLayout();
            this.recordedBox.SuspendLayout();
            this.detectBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playBar)).BeginInit();
            this.helpBox.SuspendLayout();
            this.viewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.viewBox.SuspendLayout();
            this.optionBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // recordingButtonGroup
            // 
            this.recordingButtonGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.recordingButtonGroup.Controls.Add(this.playButton);
            this.recordingButtonGroup.Controls.Add(this.recordButton);
            this.recordingButtonGroup.Controls.Add(this.calibrateButton);
            this.recordingButtonGroup.Location = new System.Drawing.Point(0, 0);
            this.recordingButtonGroup.Name = "recordingButtonGroup";
            this.recordingButtonGroup.Size = new System.Drawing.Size(349, 43);
            this.recordingButtonGroup.TabIndex = 1;
            // 
            // playButton
            // 
            this.playButton.ImageIndex = 2;
            this.playButton.ImageList = this.imageList2;
            this.playButton.Location = new System.Drawing.Point(232, 2);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(108, 37);
            this.playButton.TabIndex = 2;
            this.playButton.Text = "Play";
            this.playButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.playButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.playButton.UseVisualStyleBackColor = true;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "1457434070_player_record.png");
            this.imageList2.Images.SetKeyName(1, "1457434097_player_stop.png");
            this.imageList2.Images.SetKeyName(2, "1457434100_player_play.png");
            this.imageList2.Images.SetKeyName(3, "1457435491_player_pause.png");
            // 
            // recordButton
            // 
            this.recordButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.recordButton.ImageIndex = 0;
            this.recordButton.ImageList = this.imageList2;
            this.recordButton.Location = new System.Drawing.Point(118, 2);
            this.recordButton.Name = "recordButton";
            this.recordButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.recordButton.Size = new System.Drawing.Size(108, 37);
            this.recordButton.TabIndex = 1;
            this.recordButton.Text = "Record";
            this.recordButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.recordButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.recordButton.UseVisualStyleBackColor = true;
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(4, 2);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(108, 37);
            this.calibrateButton.TabIndex = 0;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            // 
            // recordedBox
            // 
            this.recordedBox.Controls.Add(this.cropBox);
            this.recordedBox.Controls.Add(this.detectBox);
            this.recordedBox.Controls.Add(this.playBar);
            this.recordedBox.Controls.Add(this.saveRecordedSession);
            this.recordedBox.Location = new System.Drawing.Point(269, 463);
            this.recordedBox.Name = "recordedBox";
            this.recordedBox.Size = new System.Drawing.Size(1148, 394);
            this.recordedBox.TabIndex = 8;
            this.recordedBox.TabStop = false;
            this.recordedBox.Text = "Recorded";
            // 
            // cropBox
            // 
            this.cropBox.Location = new System.Drawing.Point(20, 264);
            this.cropBox.Name = "cropBox";
            this.cropBox.Size = new System.Drawing.Size(1110, 84);
            this.cropBox.TabIndex = 3;
            this.cropBox.TabStop = false;
            this.cropBox.Text = "Crop";
            // 
            // detectBox
            // 
            this.detectBox.Controls.Add(this.detectedPanel);
            this.detectBox.Location = new System.Drawing.Point(20, 80);
            this.detectBox.Name = "detectBox";
            this.detectBox.Size = new System.Drawing.Size(1110, 167);
            this.detectBox.TabIndex = 2;
            this.detectBox.TabStop = false;
            this.detectBox.Text = "Detected";
            // 
            // detectedPanel
            // 
            this.detectedPanel.AutoScroll = true;
            this.detectedPanel.Location = new System.Drawing.Point(0, 19);
            this.detectedPanel.Name = "detectedPanel";
            this.detectedPanel.Size = new System.Drawing.Size(1110, 142);
            this.detectedPanel.TabIndex = 0;
            // 
            // playBar
            // 
            this.playBar.Location = new System.Drawing.Point(29, 29);
            this.playBar.Maximum = 100;
            this.playBar.Name = "playBar";
            this.playBar.Size = new System.Drawing.Size(1091, 45);
            this.playBar.TabIndex = 1;
            // 
            // saveRecordedSession
            // 
            this.saveRecordedSession.Location = new System.Drawing.Point(984, 365);
            this.saveRecordedSession.Name = "saveRecordedSession";
            this.saveRecordedSession.Size = new System.Drawing.Size(158, 23);
            this.saveRecordedSession.TabIndex = 0;
            this.saveRecordedSession.Text = "Save";
            this.saveRecordedSession.UseVisualStyleBackColor = true;
            // 
            // helpBox
            // 
            this.helpBox.Controls.Add(this.richTextBox1);
            this.helpBox.Location = new System.Drawing.Point(0, 49);
            this.helpBox.Name = "helpBox";
            this.helpBox.Size = new System.Drawing.Size(263, 408);
            this.helpBox.TabIndex = 7;
            this.helpBox.TabStop = false;
            this.helpBox.Text = "Help";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(251, 381);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // viewPanel
            // 
            this.viewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewPanel.Controls.Add(this.pictureBox2);
            this.viewPanel.Controls.Add(this.pictureBox1);
            this.viewPanel.Location = new System.Drawing.Point(6, 19);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(1136, 381);
            this.viewPanel.TabIndex = 6;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(587, 15);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(526, 321);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(22, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(526, 321);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // viewBox
            // 
            this.viewBox.Controls.Add(this.viewPanel);
            this.viewBox.Location = new System.Drawing.Point(269, 49);
            this.viewBox.Name = "viewBox";
            this.viewBox.Size = new System.Drawing.Size(1148, 408);
            this.viewBox.TabIndex = 9;
            this.viewBox.TabStop = false;
            this.viewBox.Text = "View";
            // 
            // optionBox
            // 
            this.optionBox.Controls.Add(this.optionsTable);
            this.optionBox.Location = new System.Drawing.Point(0, 463);
            this.optionBox.Name = "optionBox";
            this.optionBox.Size = new System.Drawing.Size(263, 394);
            this.optionBox.TabIndex = 10;
            this.optionBox.TabStop = false;
            this.optionBox.Text = "Options";
            // 
            // optionsTable
            // 
            this.optionsTable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.optionsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.optionsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.Value});
            this.optionsTable.Location = new System.Drawing.Point(6, 19);
            this.optionsTable.Name = "optionsTable";
            this.optionsTable.RowHeadersVisible = false;
            this.optionsTable.Size = new System.Drawing.Size(251, 369);
            this.optionsTable.TabIndex = 0;
            // 
            // PropertyName
            // 
            this.PropertyName.Frozen = true;
            this.PropertyName.HeaderText = "Property";
            this.PropertyName.Name = "PropertyName";
            this.PropertyName.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.Width = 150;
            // 
            // RecordPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionBox);
            this.Controls.Add(this.viewBox);
            this.Controls.Add(this.recordedBox);
            this.Controls.Add(this.helpBox);
            this.Controls.Add(this.recordingButtonGroup);
            this.DoubleBuffered = true;
            this.Name = "RecordPanel";
            this.Size = new System.Drawing.Size(1420, 860);
            this.recordingButtonGroup.ResumeLayout(false);
            this.recordedBox.ResumeLayout(false);
            this.recordedBox.PerformLayout();
            this.detectBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playBar)).EndInit();
            this.helpBox.ResumeLayout(false);
            this.viewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.viewBox.ResumeLayout(false);
            this.optionBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optionsTable)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
