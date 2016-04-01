using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class RecordPanel : UserControl
    {
        private Panel recordingButtonGroup;
        private Button playButton;
        private Button recordButton;
        private GroupBox recordedBox;
        private GroupBox helpBox;
        private RichTextBox helperTextBox;
        private Panel viewPanel;
        private GroupBox viewBox;
        private ImageList imageList2;
        private System.ComponentModel.IContainer components;
        private Button saveRecordedSession;
        private MyPictureBox depthBoard;
        private MyPictureBox rgbBoard;
        private TrackBar playBar;
        private GroupBox cropBox;
        private GroupBox optionBox;
        private DataGridView optionsTable;
        private DataGridViewTextBoxColumn PropertyName;
        private DataGridViewTextBoxColumn Value;
        private Label label1;
        private Label rgbLabel;
        private Label startTimeLabel;
        private Label label2;
        private Label cameraStatusLabel;
        private Label videoNameLabel;
        private Label recordTimeLbl;
        private Label endTimeLabel;
        private Range range1;
        private Button calibrateButton;

        public RecordPanel()
        {
            InitializeComponent();
            InitializeOptionsTable();
            InitializeButtons();
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
            this.videoNameLabel = new System.Windows.Forms.Label();
            this.endTimeLabel = new System.Windows.Forms.Label();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cropBox = new System.Windows.Forms.GroupBox();
            this.playBar = new System.Windows.Forms.TrackBar();
            this.saveRecordedSession = new System.Windows.Forms.Button();
            this.recordTimeLbl = new System.Windows.Forms.Label();
            this.helpBox = new System.Windows.Forms.GroupBox();
            this.helperTextBox = new System.Windows.Forms.RichTextBox();
            this.viewPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rgbLabel = new System.Windows.Forms.Label();
            this.depthBoard = new Annotator.MyPictureBox();
            this.rgbBoard = new Annotator.MyPictureBox();
            this.viewBox = new System.Windows.Forms.GroupBox();
            this.optionBox = new System.Windows.Forms.GroupBox();
            this.optionsTable = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cameraStatusLabel = new System.Windows.Forms.Label();
            this.recordingButtonGroup.SuspendLayout();
            this.recordedBox.SuspendLayout();
            this.cropBox.SuspendLayout();
            this.range1 = new Range();
            ((System.ComponentModel.ISupportInitialize)(this.playBar)).BeginInit();
            this.helpBox.SuspendLayout();
            this.viewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.depthBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbBoard)).BeginInit();
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
            this.playButton.Text = "Play back";
            this.playButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.playButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.playButton_MouseDown);
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
            this.recordButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.recordButton_MouseDown);
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(4, 2);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(108, 37);
            this.calibrateButton.TabIndex = 0;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.calibrateButton_MouseDown);
            // 
            // recordedBox
            // 
            this.recordedBox.Controls.Add(this.videoNameLabel);
            this.recordedBox.Controls.Add(this.endTimeLabel);
            this.recordedBox.Controls.Add(this.startTimeLabel);
            this.recordedBox.Controls.Add(this.label2);
            this.recordedBox.Controls.Add(this.cropBox);
            this.recordedBox.Controls.Add(this.playBar);
            this.recordedBox.Controls.Add(this.saveRecordedSession);
            this.recordedBox.Location = new System.Drawing.Point(269, 463);
            this.recordedBox.Name = "recordedBox";
            this.recordedBox.Size = new System.Drawing.Size(1148, 394);
            this.recordedBox.TabIndex = 8;
            this.recordedBox.TabStop = false;
            this.recordedBox.Text = "Recorded";
            // 
            // videoNameLabel
            // 
            this.videoNameLabel.AutoSize = true;
            this.videoNameLabel.Location = new System.Drawing.Point(1096, 16);
            this.videoNameLabel.Name = "videoNameLabel";
            this.videoNameLabel.Size = new System.Drawing.Size(0, 13);
            this.videoNameLabel.TabIndex = 7;
            // 
            // endTimeLabel
            // 
            this.endTimeLabel.AutoSize = true;
            this.endTimeLabel.Location = new System.Drawing.Point(1096, 72);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(34, 13);
            this.endTimeLabel.TabIndex = 6;
            this.endTimeLabel.Text = "00:00";
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.AutoSize = true;
            this.startTimeLabel.Location = new System.Drawing.Point(26, 72);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(55, 13);
            this.startTimeLabel.TabIndex = 5;
            this.startTimeLabel.Text = "00:00.000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 4;
            // 
            // cropBox
            // 
            this.cropBox.Controls.Add(this.range1);
            this.cropBox.Location = new System.Drawing.Point(23, 104);
            this.cropBox.Name = "cropBox";
            this.cropBox.Size = new System.Drawing.Size(1110, 84);
            this.cropBox.TabIndex = 3;
            this.cropBox.TabStop = false;
            this.cropBox.Text = "Crop";
            // 
            // range1
            // 
            this.range1.axisLeft = 6;
            this.range1.axisRight = 1088;
            this.range1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.range1.Location = new System.Drawing.Point(9, 25);
            this.range1.Name = "range1";
            this.range1.Size = new System.Drawing.Size(1092, 53);
            this.range1.TabIndex = 0;
            // 
            // playBar
            // 
            this.playBar.Enabled = false;
            this.playBar.Location = new System.Drawing.Point(29, 40);
            this.playBar.Maximum = 100;
            this.playBar.Name = "playBar";
            this.playBar.Size = new System.Drawing.Size(1091, 45);
            this.playBar.TabIndex = 1;
            this.playBar.ValueChanged += new System.EventHandler(this.playBar_ValueChanged);
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
            // recordTimeLbl
            // 
            this.recordTimeLbl.AutoSize = true;
            this.recordTimeLbl.Location = new System.Drawing.Point(1034, 356);
            this.recordTimeLbl.Name = "recordTimeLbl";
            this.recordTimeLbl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.recordTimeLbl.Size = new System.Drawing.Size(76, 13);
            this.recordTimeLbl.TabIndex = 8;
            this.recordTimeLbl.Text = "Recorded time";
            this.recordTimeLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // helpBox
            // 
            this.helpBox.Controls.Add(this.helperTextBox);
            this.helpBox.Location = new System.Drawing.Point(0, 49);
            this.helpBox.Name = "helpBox";
            this.helpBox.Size = new System.Drawing.Size(263, 408);
            this.helpBox.TabIndex = 7;
            this.helpBox.TabStop = false;
            this.helpBox.Text = "Help";
            // 
            // helperTextBox
            // 
            this.helperTextBox.Location = new System.Drawing.Point(6, 19);
            this.helperTextBox.Name = "helperTextBox";
            this.helperTextBox.Size = new System.Drawing.Size(251, 381);
            this.helperTextBox.TabIndex = 2;
            this.helperTextBox.Text = "";
            // 
            // viewPanel
            // 
            this.viewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewPanel.Controls.Add(this.recordTimeLbl);
            this.viewPanel.Controls.Add(this.label1);
            this.viewPanel.Controls.Add(this.rgbLabel);
            this.viewPanel.Controls.Add(this.depthBoard);
            this.viewPanel.Controls.Add(this.rgbBoard);
            this.viewPanel.Location = new System.Drawing.Point(6, 19);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(1136, 381);
            this.viewPanel.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(584, 339);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Depth view";
            // 
            // rgbLabel
            // 
            this.rgbLabel.AutoSize = true;
            this.rgbLabel.Location = new System.Drawing.Point(19, 339);
            this.rgbLabel.Name = "rgbLabel";
            this.rgbLabel.Size = new System.Drawing.Size(56, 13);
            this.rgbLabel.TabIndex = 2;
            this.rgbLabel.Text = "Color view";
            // 
            // depthBoard
            // 
            this.depthBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.depthBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.depthBoard.capture = null;
            this.depthBoard.Location = new System.Drawing.Point(587, 15);
            this.depthBoard.mat = null;
            this.depthBoard.Name = "depthBoard";
            this.depthBoard.Size = new System.Drawing.Size(526, 321);
            this.depthBoard.TabIndex = 1;
            this.depthBoard.TabStop = false;
            // 
            // rgbBoard
            // 
            this.rgbBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rgbBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rgbBoard.capture = null;
            this.rgbBoard.Location = new System.Drawing.Point(22, 15);
            this.rgbBoard.mat = null;
            this.rgbBoard.Name = "rgbBoard";
            this.rgbBoard.Size = new System.Drawing.Size(526, 321);
            this.rgbBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rgbBoard.TabIndex = 0;
            this.rgbBoard.TabStop = false;
            this.rgbBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.rgbBoard_Paint);
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
            // cameraStatusLabel
            // 
            this.cameraStatusLabel.AutoSize = true;
            this.cameraStatusLabel.Location = new System.Drawing.Point(1335, 33);
            this.cameraStatusLabel.Name = "cameraStatusLabel";
            this.cameraStatusLabel.Size = new System.Drawing.Size(76, 13);
            this.cameraStatusLabel.TabIndex = 11;
            this.cameraStatusLabel.Text = "Camera Status";
            // 
            // RecordPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cameraStatusLabel);
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
            this.cropBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playBar)).EndInit();
            this.helpBox.ResumeLayout(false);
            this.viewPanel.ResumeLayout(false);
            this.viewPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.depthBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbBoard)).EndInit();
            this.viewBox.ResumeLayout(false);
            this.optionBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optionsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void playBar_ValueChanged(object sender, EventArgs e)
        {
            updateRgbBoardWithFrame();
            updateDepthBoardWithFrame();
            updateRigWithFrame();
        }

    }
}