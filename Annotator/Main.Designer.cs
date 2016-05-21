﻿using System;

namespace Annotator
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.leftMostPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.projectExplorer = new System.Windows.Forms.TabPage();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleEventDataCreateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.middleTopPanel = new System.Windows.Forms.Panel();
            this.frameTrackBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.videoPanel = new System.Windows.Forms.Panel();
            this.playbackFileComboBox = new System.Windows.Forms.ComboBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.annotateTab = new System.Windows.Forms.TabPage();
            this.rightCenterPanel = new System.Windows.Forms.Panel();
            this.objectProperties = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropertyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.middleCenterPanel = new System.Windows.Forms.Panel();
            this.rightTopPanel = new System.Windows.Forms.Panel();
            this.selectObjContextPanel = new System.Windows.Forms.Panel();
            this.cancelSelectObjBtn = new System.Windows.Forms.Button();
            this.deleteObjBtn = new System.Windows.Forms.Button();
            this.editObjBtn = new System.Windows.Forms.Button();
            this.editObjectContextPanel = new System.Windows.Forms.Panel();
            this.addSpatialBtn = new System.Windows.Forms.Button();
            this.delAtFrameBtn = new System.Windows.Forms.Button();
            this.cancelEditObjBtn = new System.Windows.Forms.Button();
            this.addLocationBtn = new System.Windows.Forms.Button();
            this.newObjectContextPanel = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.chooseColorBtn = new System.Windows.Forms.Button();
            this.cancelObjectBtn = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.addObjBtn = new System.Windows.Forms.Button();
            this.drawingButtonTool = new System.Windows.Forms.Panel();
            this.cursorDrawing = new System.Windows.Forms.Button();
            this.polygonDrawing = new System.Windows.Forms.Button();
            this.rectangleDrawing = new System.Windows.Forms.Button();
            this.rightBottomPanel = new System.Windows.Forms.Panel();
            this.annoRefView = new System.Windows.Forms.DataGridView();
            this.StartAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TextAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoteAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.annotationText = new System.Windows.Forms.RichTextBox();
            this.cm3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addObjRefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.middleBottomPanel = new System.Windows.Forms.Panel();
            this.addEventAnnotationBtn = new System.Windows.Forms.Button();
            this.recordTab = new System.Windows.Forms.TabPage();
            //this.trainTestTab = new System.Windows.Forms.TabPage();
            this.trainingPanel1 = new Annotator.TrainingPanel();
            this.projectRightClickPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionRightClickPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.fileRightClickPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRigsFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoard = new Annotator.MyPictureBox();
            this.recordPanel = new Annotator.RecordPanel();
            this.leftMostPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.projectExplorer.SuspendLayout();
            this.menu.SuspendLayout();
            this.middleTopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameTrackBar)).BeginInit();
            this.videoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).BeginInit();
            this.tabs.SuspendLayout();
            this.annotateTab.SuspendLayout();
            this.rightCenterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectProperties)).BeginInit();
            this.rightTopPanel.SuspendLayout();
            this.selectObjContextPanel.SuspendLayout();
            this.editObjectContextPanel.SuspendLayout();
            this.newObjectContextPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.drawingButtonTool.SuspendLayout();
            this.rightBottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.annoRefView)).BeginInit();
            this.cm3.SuspendLayout();
            this.middleBottomPanel.SuspendLayout();
            this.recordTab.SuspendLayout();
            //this.trainTestTab.SuspendLayout();
            this.projectRightClickPanel.SuspendLayout();
            this.sessionRightClickPanel.SuspendLayout();
            this.fileRightClickPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftMostPanel
            // 
            this.leftMostPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftMostPanel.Controls.Add(this.tabControl1);
            this.leftMostPanel.Location = new System.Drawing.Point(2, 5);
            this.leftMostPanel.Name = "leftMostPanel";
            this.leftMostPanel.Size = new System.Drawing.Size(170, 862);
            this.leftMostPanel.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.projectExplorer);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(168, 861);
            this.tabControl1.TabIndex = 2;
            // 
            // projectExplorer
            // 
            this.projectExplorer.Controls.Add(this.treeView);
            this.projectExplorer.Location = new System.Drawing.Point(4, 22);
            this.projectExplorer.Name = "projectExplorer";
            this.projectExplorer.Padding = new System.Windows.Forms.Padding(3);
            this.projectExplorer.Size = new System.Drawing.Size(160, 835);
            this.projectExplorer.TabIndex = 0;
            this.projectExplorer.Text = "Project Explorer";
            this.projectExplorer.UseVisualStyleBackColor = true;
            // 
            // treeView
            // 
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList1;
            this.treeView.Location = new System.Drawing.Point(1, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(160, 836);
            this.treeView.TabIndex = 1;
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1FolderIcon.png");
            this.imageList1.Images.SetKeyName(1, "2SessionIcon.png");
            this.imageList1.Images.SetKeyName(2, "3FileIcon.png");
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            newProjectToolStripMenuItem,
            simpleEventDataCreateMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1449, 24);
            this.menu.TabIndex = 3;
            this.menu.Text = "File";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.simpleEventDataCreateMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.fileToolStripMenuItem.Text = " File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.newProjectToolStripMenuItem.Text = "New project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // newProjectToolStripMenuItem
            // 
            this.simpleEventDataCreateMenuItem.Name = "simpleEventDataCreateMenuItem";
            this.simpleEventDataCreateMenuItem.Size = new System.Drawing.Size(138, 22);
            this.simpleEventDataCreateMenuItem.Text = "Extract data for learning";
            this.simpleEventDataCreateMenuItem.Click += new System.EventHandler(this.simpleEventDataCreateToolStripMenuItem_Click);
            this.simpleEventDataCreateMenuItem.Enabled = false;

            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // middleTopPanel
            // 
            this.middleTopPanel.BackColor = System.Drawing.SystemColors.Control;
            this.middleTopPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.middleTopPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.middleTopPanel.Controls.Add(this.frameTrackBar);
            this.middleTopPanel.Controls.Add(this.label3);
            this.middleTopPanel.Controls.Add(this.videoPanel);
            this.middleTopPanel.Controls.Add(this.playbackFileComboBox);
            this.middleTopPanel.Location = new System.Drawing.Point(178, 5);
            this.middleTopPanel.Name = "middleTopPanel";
            this.middleTopPanel.Size = new System.Drawing.Size(921, 612);
            this.middleTopPanel.TabIndex = 1;
            // 
            // frameTrackBar
            // 
            this.frameTrackBar.Enabled = false;
            this.frameTrackBar.LargeChange = 1;
            this.frameTrackBar.Location = new System.Drawing.Point(94, 564);
            this.frameTrackBar.Margin = new System.Windows.Forms.Padding(1);
            this.frameTrackBar.Maximum = 100;
            this.frameTrackBar.Minimum = 1;
            this.frameTrackBar.Name = "frameTrackBar";
            this.frameTrackBar.Size = new System.Drawing.Size(820, 45);
            this.frameTrackBar.TabIndex = 3;
            this.frameTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.frameTrackBar.Value = 1;
            this.frameTrackBar.ValueChanged += new System.EventHandler(this.frameTrackBar_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(3, 576);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Frame:";
            // 
            // videoPanel
            // 
            this.videoPanel.BackColor = System.Drawing.Color.White;
            this.videoPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("videoPanel.BackgroundImage")));
            this.videoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.videoPanel.Controls.Add(this.pictureBoard);
            this.videoPanel.Location = new System.Drawing.Point(94, 0);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new System.Drawing.Size(820, 560);
            this.videoPanel.TabIndex = 1;
            // 
            // pictureBoard
            // 
            this.pictureBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoard.capture = null;
            this.pictureBoard.Location = new System.Drawing.Point(0, 3);
            this.pictureBoard.mat = null;
            this.pictureBoard.Name = "pictureBoard";
            this.pictureBoard.Size = new System.Drawing.Size(820, 557);
            this.pictureBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoard.TabIndex = 0;
            this.pictureBoard.TabStop = false;
            this.pictureBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoard_Paint);
            this.pictureBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoard_MouseDown);
            this.pictureBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoard_MouseMove);
            this.pictureBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoard_MouseUp);
            // 
            // comboBox1
            // 
            this.playbackFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playbackFileComboBox.Enabled = false;
            this.playbackFileComboBox.FormattingEnabled = true;
            this.playbackFileComboBox.Location = new System.Drawing.Point(3, 0);
            this.playbackFileComboBox.Name = "comboBox1";
            this.playbackFileComboBox.Size = new System.Drawing.Size(88, 21);
            this.playbackFileComboBox.TabIndex = 0;
            this.playbackFileComboBox.SelectedIndexChanged += new System.EventHandler(this.playbackVideoComboBox_SelectedIndexChanged);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.annotateTab);
            this.tabs.Controls.Add(this.recordTab);
            //this.tabs.Controls.Add(this.trainTestTab);
            this.tabs.Location = new System.Drawing.Point(0, 25);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1443, 898);
            this.tabs.TabIndex = 5;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            // 
            // annotateTab
            // 
            this.annotateTab.Controls.Add(this.rightCenterPanel);
            this.annotateTab.Controls.Add(this.middleCenterPanel);
            this.annotateTab.Controls.Add(this.rightTopPanel);
            this.annotateTab.Controls.Add(this.rightBottomPanel);
            this.annotateTab.Controls.Add(this.middleBottomPanel);
            this.annotateTab.Controls.Add(this.middleTopPanel);
            this.annotateTab.Controls.Add(this.leftMostPanel);
            this.annotateTab.Location = new System.Drawing.Point(4, 22);
            this.annotateTab.Name = "annotateTab";
            this.annotateTab.Padding = new System.Windows.Forms.Padding(3);
            this.annotateTab.Size = new System.Drawing.Size(1435, 872);
            this.annotateTab.TabIndex = 0;
            this.annotateTab.Text = "Annotate";
            this.annotateTab.UseVisualStyleBackColor = true;
            // 
            // rightCenterPanel
            // 
            this.rightCenterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightCenterPanel.Controls.Add(this.objectProperties);
            this.rightCenterPanel.Location = new System.Drawing.Point(1105, 184);
            this.rightCenterPanel.Name = "rightCenterPanel";
            this.rightCenterPanel.Size = new System.Drawing.Size(324, 431);
            this.rightCenterPanel.TabIndex = 4;
            // 
            // objectProperties
            // 
            this.objectProperties.BackgroundColor = System.Drawing.SystemColors.Control;
            this.objectProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.objectProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.PropertyValue});
            this.objectProperties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.objectProperties.GridColor = System.Drawing.SystemColors.ControlLight;
            this.objectProperties.Location = new System.Drawing.Point(-1, 3);
            this.objectProperties.Name = "objectProperties";
            this.objectProperties.RowHeadersVisible = false;
            this.objectProperties.Size = new System.Drawing.Size(324, 427);
            this.objectProperties.TabIndex = 0;
            this.objectProperties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.objectProperties_CellValueChanged);
            this.objectProperties.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.objectProperties_CellContentClick);
            // 
            // PropertyName
            // 
            this.PropertyName.HeaderText = "Property name";
            this.PropertyName.Name = "PropertyName";
            this.PropertyName.Width = 130;
            // 
            // PropertyValue
            // 
            this.PropertyValue.HeaderText = "Property Value";
            this.PropertyValue.Name = "PropertyValue";
            this.PropertyValue.Width = 190;
            // 
            // middleCenterPanel
            // 
            this.middleCenterPanel.AutoScroll = true;
            this.middleCenterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.middleCenterPanel.Location = new System.Drawing.Point(178, 623);
            this.middleCenterPanel.Name = "middleCenterPanel";
            this.middleCenterPanel.Size = new System.Drawing.Size(921, 121);
            this.middleCenterPanel.TabIndex = 3;
            // 
            // rightTopPanel
            // 
            this.rightTopPanel.BackColor = System.Drawing.SystemColors.Control;
            this.rightTopPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightTopPanel.Controls.Add(this.selectObjContextPanel);
            this.rightTopPanel.Controls.Add(this.editObjectContextPanel);
            this.rightTopPanel.Controls.Add(this.newObjectContextPanel);
            this.rightTopPanel.Controls.Add(this.drawingButtonTool);
            this.rightTopPanel.Location = new System.Drawing.Point(1105, 5);
            this.rightTopPanel.Name = "rightTopPanel";
            this.rightTopPanel.Size = new System.Drawing.Size(324, 174);
            this.rightTopPanel.TabIndex = 3;
            // 
            // selectObjContextPanel
            // 
            this.selectObjContextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectObjContextPanel.Controls.Add(this.cancelSelectObjBtn);
            this.selectObjContextPanel.Controls.Add(this.deleteObjBtn);
            this.selectObjContextPanel.Controls.Add(this.editObjBtn);
            this.selectObjContextPanel.Location = new System.Drawing.Point(6, 50);
            this.selectObjContextPanel.Name = "selectObjContextPanel";
            this.selectObjContextPanel.Size = new System.Drawing.Size(87, 64);
            this.selectObjContextPanel.TabIndex = 12;
            this.selectObjContextPanel.Visible = false;
            // 
            // cancelSelectObjBtn
            // 
            this.cancelSelectObjBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelSelectObjBtn.Location = new System.Drawing.Point(-1, 41);
            this.cancelSelectObjBtn.Name = "cancelSelectObjBtn";
            this.cancelSelectObjBtn.Size = new System.Drawing.Size(87, 22);
            this.cancelSelectObjBtn.TabIndex = 9;
            this.cancelSelectObjBtn.Text = "Cancel";
            this.cancelSelectObjBtn.UseVisualStyleBackColor = true;
            this.cancelSelectObjBtn.Click += new System.EventHandler(this.cancelSelectObjBtn_Click);
            // 
            // deleteObjBtn
            // 
            this.deleteObjBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.deleteObjBtn.Location = new System.Drawing.Point(-1, 20);
            this.deleteObjBtn.Name = "deleteObjBtn";
            this.deleteObjBtn.Size = new System.Drawing.Size(87, 22);
            this.deleteObjBtn.TabIndex = 8;
            this.deleteObjBtn.Text = "Delete";
            this.deleteObjBtn.UseVisualStyleBackColor = true;
            this.deleteObjBtn.Click += new System.EventHandler(this.deleteObjBtn_Click);
            // 
            // editObjBtn
            // 
            this.editObjBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.editObjBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.editObjBtn.Location = new System.Drawing.Point(-1, -1);
            this.editObjBtn.Name = "editObjBtn";
            this.editObjBtn.Size = new System.Drawing.Size(87, 22);
            this.editObjBtn.TabIndex = 5;
            this.editObjBtn.Text = "Edit";
            this.editObjBtn.UseVisualStyleBackColor = true;
            this.editObjBtn.Click += new System.EventHandler(this.editObjBtn_Click);
            // 
            // editObjectContextPanel
            // 
            this.editObjectContextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editObjectContextPanel.Controls.Add(this.addSpatialBtn);
            this.editObjectContextPanel.Controls.Add(this.delAtFrameBtn);
            this.editObjectContextPanel.Controls.Add(this.cancelEditObjBtn);
            this.editObjectContextPanel.Controls.Add(this.addLocationBtn);
            this.editObjectContextPanel.Location = new System.Drawing.Point(93, 50);
            this.editObjectContextPanel.Name = "editObjectContextPanel";
            this.editObjectContextPanel.Size = new System.Drawing.Size(87, 85);
            this.editObjectContextPanel.TabIndex = 11;
            this.editObjectContextPanel.Visible = false;
            // 
            // addSpatialBtn
            // 
            this.addSpatialBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.addSpatialBtn.Location = new System.Drawing.Point(-1, 41);
            this.addSpatialBtn.Name = "addSpatialBtn";
            this.addSpatialBtn.Size = new System.Drawing.Size(87, 22);
            this.addSpatialBtn.TabIndex = 9;
            this.addSpatialBtn.Text = "Spatial link";
            this.addSpatialBtn.UseVisualStyleBackColor = true;
            this.addSpatialBtn.Click += new System.EventHandler(this.addSpatialBtn_Click);
            // 
            // delAtFrameBtn
            // 
            this.delAtFrameBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.delAtFrameBtn.Location = new System.Drawing.Point(-1, 20);
            this.delAtFrameBtn.Name = "delAtFrameBtn";
            this.delAtFrameBtn.Size = new System.Drawing.Size(87, 22);
            this.delAtFrameBtn.TabIndex = 8;
            this.delAtFrameBtn.Text = "Delete at frame";
            this.delAtFrameBtn.UseVisualStyleBackColor = true;
            this.delAtFrameBtn.Click += new System.EventHandler(this.delAtFrameBtn_Click);
            // 
            // cancelEditObjBtn
            // 
            this.cancelEditObjBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelEditObjBtn.Location = new System.Drawing.Point(-1, 62);
            this.cancelEditObjBtn.Name = "cancelEditObjBtn";
            this.cancelEditObjBtn.Size = new System.Drawing.Size(87, 22);
            this.cancelEditObjBtn.TabIndex = 6;
            this.cancelEditObjBtn.Text = "Cancel";
            this.cancelEditObjBtn.UseVisualStyleBackColor = true;
            this.cancelEditObjBtn.Click += new System.EventHandler(this.cancelEditObjBtn_Click);
            // 
            // addLocationBtn
            // 
            this.addLocationBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.addLocationBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.addLocationBtn.Location = new System.Drawing.Point(-1, -1);
            this.addLocationBtn.Name = "addLocationBtn";
            this.addLocationBtn.Size = new System.Drawing.Size(87, 22);
            this.addLocationBtn.TabIndex = 5;
            this.addLocationBtn.Text = "Add location";
            this.addLocationBtn.UseVisualStyleBackColor = true;
            this.addLocationBtn.Click += new System.EventHandler(this.addLocationBtn_Click);
            // 
            // newObjectContextPanel
            // 
            this.newObjectContextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newObjectContextPanel.Controls.Add(this.numericUpDown1);
            this.newObjectContextPanel.Controls.Add(this.chooseColorBtn);
            this.newObjectContextPanel.Controls.Add(this.cancelObjectBtn);
            this.newObjectContextPanel.Controls.Add(this.label18);
            this.newObjectContextPanel.Controls.Add(this.addObjBtn);
            this.newObjectContextPanel.Location = new System.Drawing.Point(6, 50);
            this.newObjectContextPanel.Name = "newObjectContextPanel";
            this.newObjectContextPanel.Size = new System.Drawing.Size(87, 94);
            this.newObjectContextPanel.TabIndex = 8;
            this.newObjectContextPanel.Visible = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(47, 66);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(36, 20);
            this.numericUpDown1.TabIndex = 10;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // chooseColorBtn
            // 
            this.chooseColorBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chooseColorBtn.Location = new System.Drawing.Point(-1, 20);
            this.chooseColorBtn.Name = "chooseColorBtn";
            this.chooseColorBtn.Size = new System.Drawing.Size(87, 22);
            this.chooseColorBtn.TabIndex = 8;
            this.chooseColorBtn.Text = "Choose color";
            this.chooseColorBtn.UseVisualStyleBackColor = true;
            this.chooseColorBtn.Click += new System.EventHandler(this.chooseColorBtn_Click);
            // 
            // cancelObjectBtn
            // 
            this.cancelObjectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelObjectBtn.Location = new System.Drawing.Point(-1, 41);
            this.cancelObjectBtn.Name = "cancelObjectBtn";
            this.cancelObjectBtn.Size = new System.Drawing.Size(87, 22);
            this.cancelObjectBtn.TabIndex = 6;
            this.cancelObjectBtn.Text = "Cancel";
            this.cancelObjectBtn.UseVisualStyleBackColor = true;
            this.cancelObjectBtn.Click += new System.EventHandler(this.cancelObjectBtn_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 68);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(40, 13);
            this.label18.TabIndex = 9;
            this.label18.Text = "border:";
            // 
            // addObjBtn
            // 
            this.addObjBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.addObjBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.addObjBtn.Location = new System.Drawing.Point(-1, -1);
            this.addObjBtn.Name = "addObjBtn";
            this.addObjBtn.Size = new System.Drawing.Size(87, 22);
            this.addObjBtn.TabIndex = 5;
            this.addObjBtn.Text = "Add object";
            this.addObjBtn.UseVisualStyleBackColor = true;
            this.addObjBtn.Click += new System.EventHandler(this.addObjBtn_Click);
            // 
            // panel4
            // 
            this.drawingButtonTool.AutoScroll = true;
            this.drawingButtonTool.BackColor = System.Drawing.Color.White;
            this.drawingButtonTool.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.drawingButtonTool.Controls.Add(this.cursorDrawing);
            this.drawingButtonTool.Controls.Add(this.polygonDrawing);
            this.drawingButtonTool.Controls.Add(this.rectangleDrawing);
            this.drawingButtonTool.Location = new System.Drawing.Point(3, 3);
            this.drawingButtonTool.Name = "drawingButtons";
            this.drawingButtonTool.Size = new System.Drawing.Size(318, 45);
            this.drawingButtonTool.TabIndex = 0;
            // 
            // cursorDrawing
            // 
            this.cursorDrawing.BackColor = System.Drawing.Color.Transparent;
            this.cursorDrawing.BackgroundImage = global::Annotator.Properties.Resources.cursor;
            this.cursorDrawing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cursorDrawing.Location = new System.Drawing.Point(1, 1);
            this.cursorDrawing.Name = "cursorDrawing";
            this.cursorDrawing.Size = new System.Drawing.Size(38, 38);
            this.cursorDrawing.TabIndex = 2;
            this.toolTip1.SetToolTip(this.cursorDrawing, "Rectangle");
            this.cursorDrawing.UseVisualStyleBackColor = false;
            this.cursorDrawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cursorDrawing_MouseDown);
            // 
            // polygonDrawing
            // 
            this.polygonDrawing.BackColor = System.Drawing.Color.Transparent;
            this.polygonDrawing.BackgroundImage = global::Annotator.Properties.Resources.polygon;
            this.polygonDrawing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.polygonDrawing.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.polygonDrawing.Location = new System.Drawing.Point(77, 1);
            this.polygonDrawing.Name = "polygonDrawing";
            this.polygonDrawing.Size = new System.Drawing.Size(38, 38);
            this.polygonDrawing.TabIndex = 1;
            this.toolTip1.SetToolTip(this.polygonDrawing, "Rectangle");
            this.polygonDrawing.UseVisualStyleBackColor = false;
            this.polygonDrawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.polygonDrawing_MouseDown);
            // 
            // rectangleDrawing
            // 
            this.rectangleDrawing.BackColor = System.Drawing.Color.Transparent;
            this.rectangleDrawing.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rectangleDrawing.BackgroundImage")));
            this.rectangleDrawing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rectangleDrawing.Location = new System.Drawing.Point(39, 1);
            this.rectangleDrawing.Name = "rectangleDrawing";
            this.rectangleDrawing.Size = new System.Drawing.Size(38, 38);
            this.rectangleDrawing.TabIndex = 0;
            this.toolTip1.SetToolTip(this.rectangleDrawing, "Rectangle");
            this.rectangleDrawing.UseVisualStyleBackColor = false;
            this.rectangleDrawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rectangleDrawing_MouseDown);
            // 
            // rightBottomPanel
            // 
            this.rightBottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightBottomPanel.Controls.Add(this.annoRefView);
            this.rightBottomPanel.Controls.Add(this.annotationText);
            this.rightBottomPanel.Location = new System.Drawing.Point(1105, 621);
            this.rightBottomPanel.Name = "rightBottomPanel";
            this.rightBottomPanel.Size = new System.Drawing.Size(324, 248);
            this.rightBottomPanel.TabIndex = 0;
            // 
            // annoRefView
            // 
            this.annoRefView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.annoRefView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.annoRefView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.annoRefView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StartAnno,
            this.EndAnno,
            this.TextAnno,
            this.NoteAnno});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.annoRefView.DefaultCellStyle = dataGridViewCellStyle1;
            this.annoRefView.Location = new System.Drawing.Point(3, 130);
            this.annoRefView.Margin = new System.Windows.Forms.Padding(1);
            this.annoRefView.Name = "annoRefView";
            this.annoRefView.RowHeadersVisible = false;
            this.annoRefView.Size = new System.Drawing.Size(317, 115);
            this.annoRefView.TabIndex = 24;
            this.annoRefView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.annoRefView_CellClick);
            // 
            // StartAnno
            // 
            this.StartAnno.Frozen = true;
            this.StartAnno.HeaderText = "Start";
            this.StartAnno.Name = "StartAnno";
            this.StartAnno.ReadOnly = true;
            this.StartAnno.Width = 30;
            // 
            // EndAnno
            // 
            this.EndAnno.Frozen = true;
            this.EndAnno.HeaderText = "End";
            this.EndAnno.Name = "EndAnno";
            this.EndAnno.ReadOnly = true;
            this.EndAnno.Width = 30;
            // 
            // TextAnno
            // 
            this.TextAnno.Frozen = true;
            this.TextAnno.HeaderText = "Text";
            this.TextAnno.Name = "TextAnno";
            this.TextAnno.ReadOnly = true;
            this.TextAnno.Width = 160;
            // 
            // NoteAnno
            // 
            this.NoteAnno.Frozen = true;
            this.NoteAnno.HeaderText = "Note";
            this.NoteAnno.Name = "NoteAnno";
            this.NoteAnno.ReadOnly = true;
            // 
            // annotationText
            // 
            this.annotationText.ContextMenuStrip = this.cm3;
            this.annotationText.Location = new System.Drawing.Point(2, 1);
            this.annotationText.Name = "annotationText";
            this.annotationText.Size = new System.Drawing.Size(318, 125);
            this.annotationText.TabIndex = 23;
            this.annotationText.Text = "";
            this.annotationText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.annotationText_MouseDown);
            // 
            // cm3
            // 
            this.cm3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addObjRefToolStripMenuItem,
            this.addEventToolStripMenuItem});
            this.cm3.Name = "cm2";
            this.cm3.Size = new System.Drawing.Size(185, 48);
            // 
            // addObjRefToolStripMenuItem
            // 
            this.addObjRefToolStripMenuItem.Enabled = false;
            this.addObjRefToolStripMenuItem.Name = "addObjRefToolStripMenuItem";
            this.addObjRefToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.addObjRefToolStripMenuItem.Text = "Add object reference";
            this.addObjRefToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItem6_DropDownItemClicked);
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            this.addEventToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.addEventToolStripMenuItem.Text = "Add event";
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.addEventToolStripMenuItem_Click);
            // 
            // middleBottomPanel
            // 
            this.middleBottomPanel.AutoScroll = true;
            this.middleBottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.middleBottomPanel.Controls.Add(this.addEventAnnotationBtn);
            this.middleBottomPanel.Location = new System.Drawing.Point(178, 750);
            this.middleBottomPanel.Name = "middleBottomPanel";
            this.middleBottomPanel.Size = new System.Drawing.Size(921, 117);
            this.middleBottomPanel.TabIndex = 2;
            // 
            // addEventAnnotationBtn
            // 
            this.addEventAnnotationBtn.Enabled = false;
            this.addEventAnnotationBtn.Location = new System.Drawing.Point(3, 3);
            this.addEventAnnotationBtn.Name = "addEventAnnotationBtn";
            this.addEventAnnotationBtn.Size = new System.Drawing.Size(75, 23);
            this.addEventAnnotationBtn.TabIndex = 0;
            this.addEventAnnotationBtn.Text = "Add+";
            this.addEventAnnotationBtn.UseVisualStyleBackColor = true;
            this.addEventAnnotationBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // recordTab
            // 
            this.recordTab.Controls.Add(this.recordPanel);
            this.recordTab.Location = new System.Drawing.Point(4, 22);
            this.recordTab.Name = "recordTab";
            this.recordTab.Padding = new System.Windows.Forms.Padding(3);
            this.recordTab.Size = new System.Drawing.Size(1435, 872);
            this.recordTab.TabIndex = 1;
            this.recordTab.Text = "Record";
            this.recordTab.UseVisualStyleBackColor = true;
            //// 
            //// trainTestTab
            //// 
            //this.trainTestTab.Controls.Add(this.trainingPanel1);
            //this.trainTestTab.Location = new System.Drawing.Point(4, 22);
            //this.trainTestTab.Name = "trainTestTab";
            //this.trainTestTab.Size = new System.Drawing.Size(1435, 872);
            //this.trainTestTab.TabIndex = 2;
            //this.trainTestTab.Text = "Train/Test";
            //this.trainTestTab.UseVisualStyleBackColor = true;
            // 
            // trainingPanel1
            // 
            this.trainingPanel1.Location = new System.Drawing.Point(6, 6);
            this.trainingPanel1.Name = "trainingPanel1";
            this.trainingPanel1.Size = new System.Drawing.Size(1420, 860);
            this.trainingPanel1.TabIndex = 0;
            // 
            // projectRightClickPanel
            // 
            this.projectRightClickPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.newSessionToolStripMenuItem,
            this.recordSessionToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.projectRightClickPanel.Name = "cm1";
            this.projectRightClickPanel.Size = new System.Drawing.Size(153, 92);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectToolStripMenuItem.Text = "Select";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // newSessionToolStripMenuItem
            // 
            this.newSessionToolStripMenuItem.Name = "newSessionToolStripMenuItem";
            this.newSessionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newSessionToolStripMenuItem.Text = "New session";
            this.newSessionToolStripMenuItem.Click += new System.EventHandler(this.newSessionToolStripMenuItem_Click);
            // 
            // recordSessionToolStripMenuItem
            // 
            this.recordSessionToolStripMenuItem.Name = "recordSessionToolStripMenuItem";
            this.recordSessionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.recordSessionToolStripMenuItem.Text = "Record session";
            this.recordSessionToolStripMenuItem.Click += new System.EventHandler(this.recordSessionToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // sessionRightClickPanel
            // 
            this.sessionRightClickPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editSessionMenuItem,
            this.saveSessionMenuItem,
            this.deleteSessionMenuItem,
            this.addSessionMenuItem});
            this.sessionRightClickPanel.Name = "cm2";
            this.sessionRightClickPanel.Size = new System.Drawing.Size(108, 92);
            // 
            // editSessionMenuItem
            // 
            this.editSessionMenuItem.Enabled = false;
            this.editSessionMenuItem.Name = "editSessionMenuItem";
            this.editSessionMenuItem.Size = new System.Drawing.Size(107, 22);
            this.editSessionMenuItem.Text = "Edit";
            this.editSessionMenuItem.Click += new System.EventHandler(this.editSessionMenuItem_Click);
            // 
            // saveSessionMenuItem
            // 
            this.saveSessionMenuItem.Enabled = false;
            this.saveSessionMenuItem.Name = "saveSessionMenuItem";
            this.saveSessionMenuItem.Size = new System.Drawing.Size(107, 22);
            this.saveSessionMenuItem.Text = "Save";
            this.saveSessionMenuItem.Click += new System.EventHandler(this.saveSessionMenuItem_Click);
            // 
            // deleteSessionMenuItem
            // 
            this.deleteSessionMenuItem.Enabled = false;
            this.deleteSessionMenuItem.Name = "deleteSessionMenuItem";
            this.deleteSessionMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteSessionMenuItem.Text = "Delete";
            this.deleteSessionMenuItem.Click += new System.EventHandler(this.deleteSessionMenuItem_Click);
            // 
            // addSessionMenuItem
            // 
            this.addSessionMenuItem.Enabled = false;
            this.addSessionMenuItem.Name = "addSessionMenuItem";
            this.addSessionMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addSessionMenuItem.Text = "Add";
            this.addSessionMenuItem.Click += new System.EventHandler(this.addSessionMenuItem_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.Color = System.Drawing.Color.Red;
            // 
            // fileRightClickPanel
            // 
            this.fileRightClickPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addObjectToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.fileRightClickPanel.Name = "cm1";
            this.fileRightClickPanel.Size = new System.Drawing.Size(138, 48);
            // 
            // addObjectToolStripMenuItem
            // 
            this.addObjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRigsFromFileToolStripMenuItem});
            this.addObjectToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addObjectToolStripMenuItem.Name = "addObjectToolStripMenuItem";
            this.addObjectToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.addObjectToolStripMenuItem.Text = "Add objects";
            // 
            // addRigsFromFileToolStripMenuItem
            // 
            this.addRigsFromFileToolStripMenuItem.Name = "addRigsFromFileToolStripMenuItem";
            this.addRigsFromFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addRigsFromFileToolStripMenuItem.Text = "Add rigs from file";
            this.addRigsFromFileToolStripMenuItem.Click += new System.EventHandler(this.addRigsFromFileToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // recordPanel
            // 
            this.recordPanel.main = this;
            this.recordPanel.Location = new System.Drawing.Point(0, 0);
            this.recordPanel.Name = "recordPanel";
            this.recordPanel.Size = new System.Drawing.Size(1420, 860);
            this.recordPanel.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1449, 923);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.menu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1299, 724);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ECAT 0.2";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.leftMostPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.projectExplorer.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.middleTopPanel.ResumeLayout(false);
            this.middleTopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameTrackBar)).EndInit();
            this.videoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).EndInit();
            this.tabs.ResumeLayout(false);
            this.annotateTab.ResumeLayout(false);
            this.rightCenterPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectProperties)).EndInit();
            this.rightTopPanel.ResumeLayout(false);
            this.selectObjContextPanel.ResumeLayout(false);
            this.editObjectContextPanel.ResumeLayout(false);
            this.newObjectContextPanel.ResumeLayout(false);
            this.newObjectContextPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.drawingButtonTool.ResumeLayout(false);
            this.rightBottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.annoRefView)).EndInit();
            this.cm3.ResumeLayout(false);
            this.middleBottomPanel.ResumeLayout(false);
            this.recordTab.ResumeLayout(false);
            //this.trainTestTab.ResumeLayout(false);
            this.projectRightClickPanel.ResumeLayout(false);
            this.sessionRightClickPanel.ResumeLayout(false);
            this.fileRightClickPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Panel leftMostPanel;
        private System.Windows.Forms.Panel middleTopPanel;
        private System.Windows.Forms.Panel middleBottomPanel;
        private System.Windows.Forms.Panel rightBottomPanel;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip projectRightClickPanel;
        private System.Windows.Forms.ToolStripMenuItem newSessionToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip sessionRightClickPanel;
        private System.Windows.Forms.ToolStripMenuItem editSessionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSessionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSessionMenuItem;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simpleEventDataCreateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ComboBox playbackFileComboBox;
        private System.Windows.Forms.Panel videoPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar frameTrackBar;
        private System.Windows.Forms.Button addEventAnnotationBtn;
        private System.Windows.Forms.ToolStripMenuItem addSessionMenuItem;
        internal System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private MyPictureBox pictureBoard;
        private System.Windows.Forms.Button cancelObjectBtn;
        private System.Windows.Forms.Button addObjBtn;
        private System.Windows.Forms.Panel newObjectContextPanel;
        private System.Windows.Forms.Button chooseColorBtn;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip cm3;
        private System.Windows.Forms.ToolStripMenuItem addObjRefToolStripMenuItem;
        private System.Windows.Forms.Panel rightTopPanel;
        private System.Windows.Forms.Panel drawingButtonTool;
        private System.Windows.Forms.Button rectangleDrawing;
        private System.Windows.Forms.Button cursorDrawing;
        private System.Windows.Forms.Button polygonDrawing;
        private System.Windows.Forms.Panel middleCenterPanel;
        private System.Windows.Forms.Panel rightCenterPanel;
        private System.Windows.Forms.DataGridView objectProperties;
        private System.Windows.Forms.Panel editObjectContextPanel;
        private System.Windows.Forms.Button delAtFrameBtn;
        private System.Windows.Forms.Button cancelEditObjBtn;
        private System.Windows.Forms.Button addLocationBtn;
        private System.Windows.Forms.Panel selectObjContextPanel;
        private System.Windows.Forms.Button deleteObjBtn;
        private System.Windows.Forms.Button editObjBtn;
        private System.Windows.Forms.Button cancelSelectObjBtn;
        private System.Windows.Forms.RichTextBox annotationText;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.DataGridView annoRefView;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartAnno;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndAnno;
        private System.Windows.Forms.DataGridViewTextBoxColumn TextAnno;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoteAnno;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyValue;
        private System.Windows.Forms.ToolStripMenuItem recordSessionToolStripMenuItem;
        internal System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage annotateTab;
        private System.Windows.Forms.TabPage recordTab;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage projectExplorer;
        private System.Windows.Forms.Button addSpatialBtn;
        private RecordPanel recordPanel;
        private System.Windows.Forms.ContextMenuStrip fileRightClickPanel;
        private System.Windows.Forms.ToolStripMenuItem addObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRigsFromFileToolStripMenuItem;
        //private System.Windows.Forms.TabPage trainTestTab;
        private TrainingPanel trainingPanel1;
    }
}

