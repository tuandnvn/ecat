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
        internal MyPictureBox depthBoard;
        internal MyPictureBox rgbBoard;
        private TrackBar playBar;
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
        private Button calibrateButton;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        internal Main main;


        public RecordPanel()
        {
            InitializeComponent();
            InitializeObjectRecognizers();
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
            this.playBar = new System.Windows.Forms.TrackBar();
            this.saveRecordedSession = new System.Windows.Forms.Button();
            this.recordTimeLbl = new System.Windows.Forms.Label();
            this.helpBox = new System.Windows.Forms.GroupBox();
            this.helperTextBox = new System.Windows.Forms.RichTextBox();
            this.optionBox = new System.Windows.Forms.GroupBox();
            this.viewPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rgbLabel = new System.Windows.Forms.Label();
            this.depthBoard = new Annotator.MyPictureBox();
            this.rgbBoard = new Annotator.MyPictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.viewBox = new System.Windows.Forms.GroupBox();
            this.optionsTable = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cameraStatusLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.recordingButtonGroup.SuspendLayout();
            this.recordedBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playBar)).BeginInit();
            this.helpBox.SuspendLayout();
            this.viewPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.depthBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbBoard)).BeginInit();
            this.viewBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsTable)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // recordingButtonGroup
            // 
            this.recordingButtonGroup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.recordingButtonGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.recordingButtonGroup, 2);
            this.recordingButtonGroup.Controls.Add(this.playButton);
            this.recordingButtonGroup.Controls.Add(this.recordButton);
            this.recordingButtonGroup.Controls.Add(this.calibrateButton);
            this.recordingButtonGroup.Location = new System.Drawing.Point(3, 8);
            this.recordingButtonGroup.Name = "recordingButtonGroup";
            this.recordingButtonGroup.Size = new System.Drawing.Size(349, 43);
            this.recordingButtonGroup.TabIndex = 1;
            // 
            // playButton
            // 
            this.playButton.Enabled = false;
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
            this.calibrateButton.Enabled = false;
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
            this.tableLayoutPanel1.SetColumnSpan(this.recordedBox, 2);
            this.recordedBox.Controls.Add(this.tableLayoutPanel3);
            this.recordedBox.Controls.Add(this.videoNameLabel);
            this.recordedBox.Controls.Add(this.label2);
            this.recordedBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordedBox.Location = new System.Drawing.Point(273, 543);
            this.recordedBox.Name = "recordedBox";
            this.recordedBox.Size = new System.Drawing.Size(1144, 314);
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
            this.endTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.endTimeLabel.AutoSize = true;
            this.endTimeLabel.Location = new System.Drawing.Point(1080, 46);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(55, 13);
            this.endTimeLabel.TabIndex = 6;
            this.endTimeLabel.Text = "00:00.000";
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.startTimeLabel.AutoSize = true;
            this.startTimeLabel.Location = new System.Drawing.Point(3, 46);
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
            // playBar
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.playBar, 3);
            this.playBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playBar.Enabled = false;
            this.playBar.Location = new System.Drawing.Point(3, 3);
            this.playBar.Maximum = 100;
            this.playBar.Name = "playBar";
            this.playBar.Size = new System.Drawing.Size(1132, 29);
            this.playBar.TabIndex = 1;
            this.playBar.ValueChanged += new System.EventHandler(this.playBar_ValueChanged);
            // 
            // saveRecordedSession
            // 
            this.saveRecordedSession.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveRecordedSession.Location = new System.Drawing.Point(1041, 269);
            this.saveRecordedSession.Name = "saveRecordedSession";
            this.saveRecordedSession.Size = new System.Drawing.Size(94, 23);
            this.saveRecordedSession.TabIndex = 0;
            this.saveRecordedSession.Text = "Save";
            this.saveRecordedSession.UseVisualStyleBackColor = true;
            this.saveRecordedSession.Click += new System.EventHandler(this.saveRecordedSession_Click);
            // 
            // recordTimeLbl
            // 
            this.recordTimeLbl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.recordTimeLbl.AutoSize = true;
            this.recordTimeLbl.Location = new System.Drawing.Point(1057, 436);
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
            this.helpBox.Controls.Add(this.optionBox);
            this.helpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpBox.Location = new System.Drawing.Point(3, 63);
            this.helpBox.Name = "helpBox";
            this.helpBox.Size = new System.Drawing.Size(264, 474);
            this.helpBox.TabIndex = 7;
            this.helpBox.TabStop = false;
            this.helpBox.Text = "Help";
            // 
            // helperTextBox
            // 
            this.helperTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helperTextBox.Location = new System.Drawing.Point(3, 16);
            this.helperTextBox.Name = "helperTextBox";
            this.helperTextBox.Size = new System.Drawing.Size(258, 455);
            this.helperTextBox.TabIndex = 2;
            this.helperTextBox.Text = "";
            // 
            // optionBox
            // 
            this.optionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionBox.Location = new System.Drawing.Point(3, 16);
            this.optionBox.Name = "optionBox";
            this.optionBox.Size = new System.Drawing.Size(258, 455);
            this.optionBox.TabIndex = 10;
            this.optionBox.TabStop = false;
            this.optionBox.Text = "Options";
            // 
            // viewPanel
            // 
            this.viewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewPanel.Controls.Add(this.tableLayoutPanel2);
            this.viewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPanel.Location = new System.Drawing.Point(3, 16);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(1138, 455);
            this.viewPanel.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.Controls.Add(this.rgbLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.depthBoard, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.recordTimeLbl, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.rgbBoard, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1136, 453);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // rgbLabel
            // 
            this.rgbLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rgbLabel.AutoSize = true;
            this.rgbLabel.Location = new System.Drawing.Point(3, 406);
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
            this.depthBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.depthBoard.Location = new System.Drawing.Point(627, 3);
            this.depthBoard.mat = null;
            this.depthBoard.Name = "depthBoard";
            this.depthBoard.Size = new System.Drawing.Size(506, 387);
            this.depthBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.depthBoard.TabIndex = 1;
            this.depthBoard.TabStop = false;
            // 
            // rgbBoard
            // 
            this.rgbBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rgbBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rgbBoard.capture = null;
            this.rgbBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgbBoard.Location = new System.Drawing.Point(3, 3);
            this.rgbBoard.mat = null;
            this.rgbBoard.Name = "rgbBoard";
            this.rgbBoard.Size = new System.Drawing.Size(618, 387);
            this.rgbBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.rgbBoard.TabIndex = 0;
            this.rgbBoard.TabStop = false;
            this.rgbBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.rgbBoard_Paint);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(627, 406);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Depth view";
            // 
            // viewBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.viewBox, 2);
            this.viewBox.Controls.Add(this.viewPanel);
            this.viewBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewBox.Location = new System.Drawing.Point(273, 63);
            this.viewBox.Name = "viewBox";
            this.viewBox.Size = new System.Drawing.Size(1144, 474);
            this.viewBox.TabIndex = 9;
            this.viewBox.TabStop = false;
            this.viewBox.Text = "View";
            // 
            // optionsTable
            // 
            this.optionsTable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.optionsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.optionsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.Value});
            this.optionsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsTable.Location = new System.Drawing.Point(3, 543);
            this.optionsTable.Name = "optionsTable";
            this.optionsTable.RowHeadersVisible = false;
            this.optionsTable.Size = new System.Drawing.Size(264, 314);
            this.optionsTable.TabIndex = 0;
            this.optionsTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.optionsTable_CellValueChanged);
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
            this.cameraStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraStatusLabel.AutoSize = true;
            this.cameraStatusLabel.Location = new System.Drawing.Point(1341, 47);
            this.cameraStatusLabel.Name = "cameraStatusLabel";
            this.cameraStatusLabel.Size = new System.Drawing.Size(76, 13);
            this.cameraStatusLabel.TabIndex = 11;
            this.cameraStatusLabel.Text = "Camera Status";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.optionsTable, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.recordingButtonGroup, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.recordedBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cameraStatusLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.helpBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.viewBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1420, 860);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.Controls.Add(this.endTimeLabel, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.startTimeLabel, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.playBar, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.saveRecordedSession, 2, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1138, 295);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // RecordPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "RecordPanel";
            this.Size = new System.Drawing.Size(1420, 860);
            this.recordingButtonGroup.ResumeLayout(false);
            this.recordedBox.ResumeLayout(false);
            this.recordedBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playBar)).EndInit();
            this.helpBox.ResumeLayout(false);
            this.viewPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.depthBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbBoard)).EndInit();
            this.viewBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optionsTable)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        private void RecordPanel_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyDown " + e.KeyCode);
            if (e.KeyCode == Keys.D && recordMode != RecordMode.Playingback)
            {
                rgbBoard.Image.Save("temp.png");
            }
        }

        private void playBar_ValueChanged(object sender, EventArgs e)
        {
            updateRgbBoardWithFrame();
            updateDepthBoardWithFrame();
            updateRigWithFrame();
        }

        private void RecordPanel_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("KeyPress " + e.KeyChar);
            if (e.KeyChar == 'd' && recordMode != RecordMode.Playingback)
            {
                rgbBoard.Image.Save("temp.png");
            }
        }
    }
}