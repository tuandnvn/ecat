﻿using System;
using System.Windows.Forms;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.annotateTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.leftMostPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.projectExplorer = new System.Windows.Forms.TabPage();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.logGridView = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Log = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.middleTopPanel = new System.Windows.Forms.Panel();
            this.middleTopTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.videoPanel = new System.Windows.Forms.Panel();
            this.pictureBoard = new Annotator.MyPictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.playbackFileComboBox = new System.Windows.Forms.ComboBox();
            this.startInSecondLbl = new System.Windows.Forms.Label();
            this.endInSecondLbl = new System.Windows.Forms.Label();
            this.endInSecondTextBox = new System.Windows.Forms.TextBox();
            this.startInSecondTextBox = new System.Windows.Forms.TextBox();
            this.frameNumberLbl = new System.Windows.Forms.Label();
            this.frameTrackBar = new Annotator.CustomizedTrackBar();
            this.playBtn = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.middleCenterPanel = new System.Windows.Forms.Panel();
            this.middleCenterTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.middleBottomPanel = new System.Windows.Forms.Panel();
            this.middleBottomTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.addEventAnnotationBtn = new System.Windows.Forms.Button();
            this.rightColumnLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.annoRefView = new System.Windows.Forms.DataGridView();
            this.StartAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TextAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoteAnno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.annotationText = new System.Windows.Forms.RichTextBox();
            this.cm3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addObjRefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightTopPanel = new System.Windows.Forms.Panel();
            this.selectObjContextPanel = new System.Windows.Forms.Panel();
            this.cancelSelectObjBtn = new System.Windows.Forms.Button();
            this.deleteObjBtn = new System.Windows.Forms.Button();
            this.editObjBtn = new System.Windows.Forms.Button();
            this.editObjectContextPanel = new System.Windows.Forms.Panel();
            this.addSpatialBtn = new System.Windows.Forms.Button();
            this.delAtFrameBtn = new System.Windows.Forms.Button();
            this.delMarkerBtn = new System.Windows.Forms.Button();
            this.cancelEditObjBtn = new System.Windows.Forms.Button();
            this.addLocationBtn = new System.Windows.Forms.Button();
            this.newObjectContextPanel = new System.Windows.Forms.Panel();
            this.borderSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.chooseColorBtn = new System.Windows.Forms.Button();
            this.cancelObjectBtn = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.addObjBtn = new System.Windows.Forms.Button();
            this.drawingButtonTool = new System.Windows.Forms.Panel();
            this.redoBtn = new System.Windows.Forms.Button();
            this.undoBtn = new System.Windows.Forms.Button();
            this.zoomDrawing = new System.Windows.Forms.Button();
            this.polygonDrawing = new System.Windows.Forms.Button();
            this.rectangleDrawing = new System.Windows.Forms.Button();
            this.rightCenterPanel = new System.Windows.Forms.Panel();
            this.objectProperties = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropertyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.predicateView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.othersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabs = new System.Windows.Forms.TabControl();
            this.annotateTab = new System.Windows.Forms.TabPage();
            this.recordTab = new System.Windows.Forms.TabPage();
            this.projectRightClickPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectGenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectEventTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectReferencesByNameMatchingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionRightClickPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitWithoutSavingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFileToSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshSessionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionDetectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glyphboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detectGlyphBox2dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detectGlyphBox3dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionGenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionEventTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyObjectsFromPreviousSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromPreviousSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastAppearanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.fileRightClickPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRigsFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.annotationWorkspaceTitle = new System.Windows.Forms.Label();
            this.workspaceRightClickPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newProjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshWorkspaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingPanel1 = new Annotator.TrainingPanel();
            this.annotateTableLayoutPanel.SuspendLayout();
            this.leftMostPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.projectExplorer.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logGridView)).BeginInit();
            this.middleTopPanel.SuspendLayout();
            this.middleTopTableLayoutPanel.SuspendLayout();
            this.videoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameTrackBar)).BeginInit();
            this.middleCenterPanel.SuspendLayout();
            this.middleBottomPanel.SuspendLayout();
            this.middleBottomTableLayoutPanel.SuspendLayout();
            this.rightColumnLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.annoRefView)).BeginInit();
            this.cm3.SuspendLayout();
            this.rightTopPanel.SuspendLayout();
            this.selectObjContextPanel.SuspendLayout();
            this.editObjectContextPanel.SuspendLayout();
            this.newObjectContextPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borderSizeNumeric)).BeginInit();
            this.drawingButtonTool.SuspendLayout();
            this.rightCenterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectProperties)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.predicateView)).BeginInit();
            this.menu.SuspendLayout();
            this.tabs.SuspendLayout();
            this.annotateTab.SuspendLayout();
            this.projectRightClickPanel.SuspendLayout();
            this.sessionRightClickPanel.SuspendLayout();
            this.fileRightClickPanel.SuspendLayout();
            this.workspaceRightClickPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // annotateTableLayoutPanel
            // 
            this.annotateTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.annotateTableLayoutPanel.ColumnCount = 3;
            this.annotateTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.annotateTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.annotateTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 437F));
            this.annotateTableLayoutPanel.Controls.Add(this.leftMostPanel, 0, 0);
            this.annotateTableLayoutPanel.Controls.Add(this.middleTopPanel, 1, 0);
            this.annotateTableLayoutPanel.Controls.Add(this.middleCenterPanel, 1, 2);
            this.annotateTableLayoutPanel.Controls.Add(this.middleBottomPanel, 1, 3);
            this.annotateTableLayoutPanel.Controls.Add(this.rightColumnLayoutPanel, 2, 0);
            this.annotateTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotateTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.annotateTableLayoutPanel.Name = "annotateTableLayoutPanel";
            this.annotateTableLayoutPanel.RowCount = 4;
            this.annotateTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 174F));
            this.annotateTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.annotateTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.annotateTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.annotateTableLayoutPanel.Size = new System.Drawing.Size(1435, 867);
            this.annotateTableLayoutPanel.TabIndex = 0;
            this.annotateTableLayoutPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.annotateTableLayoutPanel_MouseDown);
            this.annotateTableLayoutPanel.MouseLeave += new System.EventHandler(this.annotateTableLayoutPanel_MouseLeave);
            this.annotateTableLayoutPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.annotateTableLayoutPanel_MouseMove);
            this.annotateTableLayoutPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.annotateTableLayoutPanel_MouseUp);
            // 
            // leftMostPanel
            // 
            this.leftMostPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftMostPanel.Controls.Add(this.tabControl1);
            this.leftMostPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftMostPanel.Location = new System.Drawing.Point(4, 4);
            this.leftMostPanel.Name = "leftMostPanel";
            this.annotateTableLayoutPanel.SetRowSpan(this.leftMostPanel, 4);
            this.leftMostPanel.Size = new System.Drawing.Size(164, 859);
            this.leftMostPanel.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.projectExplorer);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(162, 857);
            this.tabControl1.TabIndex = 2;
            // 
            // projectExplorer
            // 
            this.projectExplorer.Controls.Add(this.treeView);
            this.projectExplorer.Location = new System.Drawing.Point(4, 22);
            this.projectExplorer.Name = "projectExplorer";
            this.projectExplorer.Padding = new System.Windows.Forms.Padding(3);
            this.projectExplorer.Size = new System.Drawing.Size(154, 831);
            this.projectExplorer.TabIndex = 0;
            this.projectExplorer.Text = "Project Explorer";
            this.projectExplorer.UseVisualStyleBackColor = true;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList1;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(148, 825);
            this.treeView.TabIndex = 1;
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(154, 831);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.logGridView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(148, 825);
            this.panel2.TabIndex = 0;
            // 
            // logGridView
            // 
            this.logGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.logGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.Log});
            this.logGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logGridView.GridColor = System.Drawing.SystemColors.ControlLight;
            this.logGridView.Location = new System.Drawing.Point(0, 0);
            this.logGridView.Name = "logGridView";
            this.logGridView.RowHeadersVisible = false;
            this.logGridView.Size = new System.Drawing.Size(148, 825);
            this.logGridView.TabIndex = 0;
            // 
            // Number
            // 
            this.Number.FillWeight = 30.45685F;
            this.Number.HeaderText = "#";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Log
            // 
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Log.DefaultCellStyle = dataGridViewCellStyle1;
            this.Log.FillWeight = 169.5432F;
            this.Log.HeaderText = "Log";
            this.Log.Name = "Log";
            this.Log.ReadOnly = true;
            // 
            // middleTopPanel
            // 
            this.middleTopPanel.BackColor = System.Drawing.SystemColors.Control;
            this.middleTopPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.middleTopPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.middleTopPanel.Controls.Add(this.middleTopTableLayoutPanel);
            this.middleTopPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middleTopPanel.Location = new System.Drawing.Point(175, 4);
            this.middleTopPanel.Name = "middleTopPanel";
            this.annotateTableLayoutPanel.SetRowSpan(this.middleTopPanel, 2);
            this.middleTopPanel.Size = new System.Drawing.Size(818, 657);
            this.middleTopPanel.TabIndex = 1;
            // 
            // middleTopTableLayoutPanel
            // 
            this.middleTopTableLayoutPanel.ColumnCount = 2;
            this.middleTopTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.middleTopTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.middleTopTableLayoutPanel.Controls.Add(this.videoPanel, 1, 0);
            this.middleTopTableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.middleTopTableLayoutPanel.Controls.Add(this.frameTrackBar, 1, 1);
            this.middleTopTableLayoutPanel.Controls.Add(this.playBtn, 0, 1);
            this.middleTopTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middleTopTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.middleTopTableLayoutPanel.Name = "middleTopTableLayoutPanel";
            this.middleTopTableLayoutPanel.RowCount = 2;
            this.middleTopTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.middleTopTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.middleTopTableLayoutPanel.Size = new System.Drawing.Size(816, 655);
            this.middleTopTableLayoutPanel.TabIndex = 8;
            // 
            // videoPanel
            // 
            this.videoPanel.AutoScroll = true;
            this.videoPanel.BackColor = System.Drawing.Color.White;
            this.videoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.videoPanel.Controls.Add(this.pictureBoard);
            this.videoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoPanel.Location = new System.Drawing.Point(123, 3);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new System.Drawing.Size(690, 599);
            this.videoPanel.TabIndex = 1;
            // 
            // pictureBoard
            // 
            this.pictureBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoard.Location = new System.Drawing.Point(0, 0);
            this.pictureBoard.mat = null;
            this.pictureBoard.Name = "pictureBoard";
            this.pictureBoard.Size = new System.Drawing.Size(690, 599);
            this.pictureBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoard.TabIndex = 0;
            this.pictureBoard.TabStop = false;
            this.pictureBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoard_Paint);
            this.pictureBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoard_MouseDown);
            this.pictureBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoard_MouseMove);
            this.pictureBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoard_MouseUp);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.playbackFileComboBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.startInSecondLbl, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.endInSecondLbl, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.endInSecondTextBox, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.startInSecondTextBox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.frameNumberLbl, 0, 8);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 9;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(114, 599);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // playbackFileComboBox
            // 
            this.playbackFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playbackFileComboBox.Enabled = false;
            this.playbackFileComboBox.FormattingEnabled = true;
            this.playbackFileComboBox.Location = new System.Drawing.Point(3, 3);
            this.playbackFileComboBox.Name = "playbackFileComboBox";
            this.playbackFileComboBox.Size = new System.Drawing.Size(108, 21);
            this.playbackFileComboBox.TabIndex = 0;
            this.playbackFileComboBox.SelectedIndexChanged += new System.EventHandler(this.playbackVideoComboBox_SelectedIndexChanged);
            // 
            // startInSecondLbl
            // 
            this.startInSecondLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.startInSecondLbl.Location = new System.Drawing.Point(3, 234);
            this.startInSecondLbl.Name = "startInSecondLbl";
            this.startInSecondLbl.Size = new System.Drawing.Size(108, 30);
            this.startInSecondLbl.TabIndex = 4;
            this.startInSecondLbl.Text = "Start:";
            // 
            // endInSecondLbl
            // 
            this.endInSecondLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.endInSecondLbl.Location = new System.Drawing.Point(3, 297);
            this.endInSecondLbl.Name = "endInSecondLbl";
            this.endInSecondLbl.Size = new System.Drawing.Size(100, 23);
            this.endInSecondLbl.TabIndex = 5;
            this.endInSecondLbl.Text = "End:";
            // 
            // endInSecondTextBox
            // 
            this.endInSecondTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endInSecondTextBox.Enabled = false;
            this.endInSecondTextBox.Location = new System.Drawing.Point(3, 327);
            this.endInSecondTextBox.Name = "endInSecondTextBox";
            this.endInSecondTextBox.Size = new System.Drawing.Size(108, 20);
            this.endInSecondTextBox.TabIndex = 7;
            // 
            // startInSecondTextBox
            // 
            this.startInSecondTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startInSecondTextBox.Enabled = false;
            this.startInSecondTextBox.Location = new System.Drawing.Point(3, 267);
            this.startInSecondTextBox.Name = "startInSecondTextBox";
            this.startInSecondTextBox.Size = new System.Drawing.Size(108, 20);
            this.startInSecondTextBox.TabIndex = 6;
            // 
            // frameNumberLbl
            // 
            this.frameNumberLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.frameNumberLbl.AutoSize = true;
            this.frameNumberLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.frameNumberLbl.Location = new System.Drawing.Point(3, 580);
            this.frameNumberLbl.Name = "frameNumberLbl";
            this.frameNumberLbl.Size = new System.Drawing.Size(52, 17);
            this.frameNumberLbl.TabIndex = 2;
            this.frameNumberLbl.Text = "Frame:";
            // 
            // frameTrackBar
            // 
            this.frameTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frameTrackBar.Enabled = false;
            this.frameTrackBar.LargeChange = 1;
            this.frameTrackBar.Location = new System.Drawing.Point(121, 606);
            this.frameTrackBar.Margin = new System.Windows.Forms.Padding(1, 1, 15, 1);
            this.frameTrackBar.MaxDragVal = 100;
            this.frameTrackBar.Maximum = 100;
            this.frameTrackBar.MinDragVal = 1;
            this.frameTrackBar.Minimum = 1;
            this.frameTrackBar.Name = "frameTrackBar";
            this.frameTrackBar.Size = new System.Drawing.Size(680, 48);
            this.frameTrackBar.TabIndex = 3;
            this.frameTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.frameTrackBar.Value = 1;
            this.frameTrackBar.ValueChanged += new System.EventHandler(this.frameTrackBar_ValueChanged);
            // 
            // playBtn
            // 
            this.playBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playBtn.ImageIndex = 2;
            this.playBtn.ImageList = this.imageList2;
            this.playBtn.Location = new System.Drawing.Point(3, 608);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(114, 44);
            this.playBtn.TabIndex = 9;
            this.playBtn.UseVisualStyleBackColor = true;
            this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
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
            // middleCenterPanel
            // 
            this.middleCenterPanel.AutoScroll = true;
            this.middleCenterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.middleCenterPanel.Controls.Add(this.middleCenterTableLayoutPanel);
            this.middleCenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middleCenterPanel.Location = new System.Drawing.Point(175, 668);
            this.middleCenterPanel.Name = "middleCenterPanel";
            this.middleCenterPanel.Size = new System.Drawing.Size(818, 94);
            this.middleCenterPanel.TabIndex = 3;
            // 
            // middleCenterTableLayoutPanel
            // 
            this.middleCenterTableLayoutPanel.ColumnCount = 2;
            this.middleCenterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.middleCenterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.middleCenterTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.middleCenterTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.middleCenterTableLayoutPanel.Name = "middleCenterTableLayoutPanel";
            this.middleCenterTableLayoutPanel.Size = new System.Drawing.Size(816, 0);
            this.middleCenterTableLayoutPanel.TabIndex = 0;
            // 
            // middleBottomPanel
            // 
            this.middleBottomPanel.AutoScroll = true;
            this.middleBottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.middleBottomPanel.Controls.Add(this.middleBottomTableLayoutPanel);
            this.middleBottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middleBottomPanel.Location = new System.Drawing.Point(175, 769);
            this.middleBottomPanel.Name = "middleBottomPanel";
            this.middleBottomPanel.Size = new System.Drawing.Size(818, 94);
            this.middleBottomPanel.TabIndex = 2;
            // 
            // middleBottomTableLayoutPanel
            // 
            this.middleBottomTableLayoutPanel.ColumnCount = 2;
            this.middleBottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.middleBottomTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.middleBottomTableLayoutPanel.Controls.Add(this.addEventAnnotationBtn, 0, 0);
            this.middleBottomTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.middleBottomTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.middleBottomTableLayoutPanel.Name = "middleBottomTableLayoutPanel";
            this.middleBottomTableLayoutPanel.RowCount = 1;
            this.middleBottomTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.middleBottomTableLayoutPanel.Size = new System.Drawing.Size(816, 60);
            this.middleBottomTableLayoutPanel.TabIndex = 1;
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
            this.addEventAnnotationBtn.Click += new System.EventHandler(this.addEventAnnotationBtn_Click);
            // 
            // rightColumnLayoutPanel
            // 
            this.rightColumnLayoutPanel.BackColor = System.Drawing.SystemColors.Window;
            this.rightColumnLayoutPanel.ColumnCount = 1;
            this.rightColumnLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rightColumnLayoutPanel.Controls.Add(this.annoRefView, 0, 4);
            this.rightColumnLayoutPanel.Controls.Add(this.annotationText, 0, 3);
            this.rightColumnLayoutPanel.Controls.Add(this.rightTopPanel, 0, 0);
            this.rightColumnLayoutPanel.Controls.Add(this.rightCenterPanel, 0, 1);
            this.rightColumnLayoutPanel.Controls.Add(this.panel1, 0, 2);
            this.rightColumnLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightColumnLayoutPanel.Location = new System.Drawing.Point(1000, 4);
            this.rightColumnLayoutPanel.Name = "rightColumnLayoutPanel";
            this.rightColumnLayoutPanel.RowCount = 5;
            this.annotateTableLayoutPanel.SetRowSpan(this.rightColumnLayoutPanel, 4);
            this.rightColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.rightColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.rightColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.rightColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.rightColumnLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.rightColumnLayoutPanel.Size = new System.Drawing.Size(431, 859);
            this.rightColumnLayoutPanel.TabIndex = 5;
            // 
            // annoRefView
            // 
            this.annoRefView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.annoRefView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.annoRefView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.annoRefView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.annoRefView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StartAnno,
            this.EndAnno,
            this.TextAnno,
            this.NoteAnno});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.annoRefView.DefaultCellStyle = dataGridViewCellStyle2;
            this.annoRefView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annoRefView.Location = new System.Drawing.Point(1, 759);
            this.annoRefView.Margin = new System.Windows.Forms.Padding(1);
            this.annoRefView.Name = "annoRefView";
            this.annoRefView.RowHeadersVisible = false;
            this.annoRefView.Size = new System.Drawing.Size(429, 99);
            this.annoRefView.TabIndex = 24;
            this.annoRefView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.annoRefView_CellClick);
            this.annoRefView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.annoRefView_UserDeletingRow);
            // 
            // StartAnno
            // 
            this.StartAnno.FillWeight = 42.47768F;
            this.StartAnno.HeaderText = "Start";
            this.StartAnno.Name = "StartAnno";
            this.StartAnno.ReadOnly = true;
            // 
            // EndAnno
            // 
            this.EndAnno.FillWeight = 40.60913F;
            this.EndAnno.HeaderText = "End";
            this.EndAnno.Name = "EndAnno";
            this.EndAnno.ReadOnly = true;
            // 
            // TextAnno
            // 
            this.TextAnno.FillWeight = 226.5279F;
            this.TextAnno.HeaderText = "Text";
            this.TextAnno.Name = "TextAnno";
            this.TextAnno.ReadOnly = true;
            // 
            // NoteAnno
            // 
            this.NoteAnno.FillWeight = 90.38526F;
            this.NoteAnno.HeaderText = "Note";
            this.NoteAnno.Name = "NoteAnno";
            this.NoteAnno.ReadOnly = true;
            // 
            // annotationText
            // 
            this.annotationText.ContextMenuStrip = this.cm3;
            this.annotationText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationText.Enabled = false;
            this.annotationText.Location = new System.Drawing.Point(3, 661);
            this.annotationText.Name = "annotationText";
            this.annotationText.Size = new System.Drawing.Size(425, 94);
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
            this.addObjRefToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.addObjRefToolStripMenuItem_Clicked);
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            this.addEventToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.addEventToolStripMenuItem.Text = "Add event";
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.addEventToolStripMenuItem_Click);
            // 
            // rightTopPanel
            // 
            this.rightTopPanel.BackColor = System.Drawing.SystemColors.Control;
            this.rightTopPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightTopPanel.Controls.Add(this.selectObjContextPanel);
            this.rightTopPanel.Controls.Add(this.editObjectContextPanel);
            this.rightTopPanel.Controls.Add(this.newObjectContextPanel);
            this.rightTopPanel.Controls.Add(this.drawingButtonTool);
            this.rightTopPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightTopPanel.Location = new System.Drawing.Point(3, 3);
            this.rightTopPanel.Name = "rightTopPanel";
            this.rightTopPanel.Size = new System.Drawing.Size(425, 164);
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
            this.cancelSelectObjBtn.Text = "&Cancel";
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
            this.deleteObjBtn.Text = "&Delete";
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
            this.editObjBtn.Text = "&Edit";
            this.editObjBtn.UseVisualStyleBackColor = true;
            this.editObjBtn.Click += new System.EventHandler(this.editObjBtn_Click);
            // 
            // editObjectContextPanel
            // 
            this.editObjectContextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editObjectContextPanel.Controls.Add(this.addSpatialBtn);
            this.editObjectContextPanel.Controls.Add(this.delAtFrameBtn);
            this.editObjectContextPanel.Controls.Add(this.delMarkerBtn);
            this.editObjectContextPanel.Controls.Add(this.cancelEditObjBtn);
            this.editObjectContextPanel.Controls.Add(this.addLocationBtn);
            this.editObjectContextPanel.Location = new System.Drawing.Point(93, 50);
            this.editObjectContextPanel.Name = "editObjectContextPanel";
            this.editObjectContextPanel.Size = new System.Drawing.Size(87, 106);
            this.editObjectContextPanel.TabIndex = 11;
            this.editObjectContextPanel.Visible = false;
            // 
            // addSpatialBtn
            // 
            this.addSpatialBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.addSpatialBtn.Location = new System.Drawing.Point(-1, 62);
            this.addSpatialBtn.Name = "addSpatialBtn";
            this.addSpatialBtn.Size = new System.Drawing.Size(87, 22);
            this.addSpatialBtn.TabIndex = 10;
            this.addSpatialBtn.Text = "&Link to";
            this.toolTip.SetToolTip(this.addSpatialBtn, "Link the object to another object");
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
            this.delAtFrameBtn.Text = "Delete at &frame";
            this.toolTip.SetToolTip(this.delAtFrameBtn, "Add a deletion marker at this frame when the object disappear on the screen");
            this.delAtFrameBtn.UseVisualStyleBackColor = true;
            this.delAtFrameBtn.Click += new System.EventHandler(this.delAtFrameBtn_Click);
            // 
            // delMarkerBtn
            // 
            this.delMarkerBtn.Enabled = false;
            this.delMarkerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.delMarkerBtn.Location = new System.Drawing.Point(-1, 41);
            this.delMarkerBtn.Name = "delMarkerBtn";
            this.delMarkerBtn.Size = new System.Drawing.Size(87, 22);
            this.delMarkerBtn.TabIndex = 9;
            this.delMarkerBtn.Text = "Delete &marker";
            this.toolTip.SetToolTip(this.delMarkerBtn, "Delete the marker at this point");
            this.delMarkerBtn.UseVisualStyleBackColor = true;
            this.delMarkerBtn.Click += new System.EventHandler(this.delMarkerBtn_Click);
            // 
            // cancelEditObjBtn
            // 
            this.cancelEditObjBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelEditObjBtn.Location = new System.Drawing.Point(-1, 83);
            this.cancelEditObjBtn.Name = "cancelEditObjBtn";
            this.cancelEditObjBtn.Size = new System.Drawing.Size(87, 22);
            this.cancelEditObjBtn.TabIndex = 11;
            this.cancelEditObjBtn.Text = "Ca&ncel";
            this.toolTip.SetToolTip(this.cancelEditObjBtn, "Cancel editing at this frame");
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
            this.addLocationBtn.Text = "&Add location";
            this.toolTip.SetToolTip(this.addLocationBtn, "Add a location marker at this frame");
            this.addLocationBtn.UseVisualStyleBackColor = true;
            this.addLocationBtn.Click += new System.EventHandler(this.addLocationBtn_Click);
            // 
            // newObjectContextPanel
            // 
            this.newObjectContextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.newObjectContextPanel.Controls.Add(this.borderSizeNumeric);
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
            // borderSizeNumeric
            // 
            this.borderSizeNumeric.Location = new System.Drawing.Point(47, 66);
            this.borderSizeNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.borderSizeNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.borderSizeNumeric.Name = "borderSizeNumeric";
            this.borderSizeNumeric.Size = new System.Drawing.Size(36, 20);
            this.borderSizeNumeric.TabIndex = 10;
            this.borderSizeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.borderSizeNumeric.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
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
            this.cancelObjectBtn.Text = "&Cancel";
            this.toolTip.SetToolTip(this.cancelObjectBtn, "Deselect object");
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
            this.addObjBtn.Text = "&Add object";
            this.toolTip.SetToolTip(this.addObjBtn, "Add object on the draw");
            this.addObjBtn.UseVisualStyleBackColor = true;
            this.addObjBtn.Click += new System.EventHandler(this.addObjBtn_Click);
            // 
            // drawingButtonTool
            // 
            this.drawingButtonTool.AutoScroll = true;
            this.drawingButtonTool.BackColor = System.Drawing.Color.White;
            this.drawingButtonTool.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.drawingButtonTool.Controls.Add(this.redoBtn);
            this.drawingButtonTool.Controls.Add(this.undoBtn);
            this.drawingButtonTool.Controls.Add(this.zoomDrawing);
            this.drawingButtonTool.Controls.Add(this.polygonDrawing);
            this.drawingButtonTool.Controls.Add(this.rectangleDrawing);
            this.drawingButtonTool.Location = new System.Drawing.Point(3, 3);
            this.drawingButtonTool.Name = "drawingButtonTool";
            this.drawingButtonTool.Size = new System.Drawing.Size(318, 45);
            this.drawingButtonTool.TabIndex = 0;
            // 
            // redoBtn
            // 
            this.redoBtn.BackColor = System.Drawing.Color.Transparent;
            this.redoBtn.BackgroundImage = global::Annotator.Properties.Resources.redo;
            this.redoBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.redoBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.redoBtn.Location = new System.Drawing.Point(154, 1);
            this.redoBtn.Name = "redoBtn";
            this.redoBtn.Size = new System.Drawing.Size(38, 38);
            this.redoBtn.TabIndex = 5;
            this.toolTip.SetToolTip(this.redoBtn, "Redo Ctrl + Y");
            this.redoBtn.UseVisualStyleBackColor = false;
            this.redoBtn.Click += new System.EventHandler(this.redoBtn_Click);
            // 
            // undoBtn
            // 
            this.undoBtn.BackColor = System.Drawing.Color.Transparent;
            this.undoBtn.BackgroundImage = global::Annotator.Properties.Resources.undo;
            this.undoBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.undoBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.undoBtn.Location = new System.Drawing.Point(116, 1);
            this.undoBtn.Name = "undoBtn";
            this.undoBtn.Size = new System.Drawing.Size(38, 38);
            this.undoBtn.TabIndex = 4;
            this.toolTip.SetToolTip(this.undoBtn, "Undo Ctrl + Z");
            this.undoBtn.UseVisualStyleBackColor = false;
            this.undoBtn.Click += new System.EventHandler(this.undoBtn_Click);
            // 
            // zoomDrawing
            // 
            this.zoomDrawing.BackColor = System.Drawing.Color.Transparent;
            this.zoomDrawing.BackgroundImage = global::Annotator.Properties.Resources.Zoom_search_find_magnifying_glass;
            this.zoomDrawing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.zoomDrawing.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.zoomDrawing.Location = new System.Drawing.Point(78, 1);
            this.zoomDrawing.Name = "zoomDrawing";
            this.zoomDrawing.Size = new System.Drawing.Size(38, 38);
            this.zoomDrawing.TabIndex = 3;
            this.toolTip.SetToolTip(this.zoomDrawing, "Zoom");
            this.zoomDrawing.UseVisualStyleBackColor = false;
            this.zoomDrawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zoomDrawing_MouseDown);
            // 
            // polygonDrawing
            // 
            this.polygonDrawing.BackColor = System.Drawing.Color.Transparent;
            this.polygonDrawing.BackgroundImage = global::Annotator.Properties.Resources.polygon;
            this.polygonDrawing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.polygonDrawing.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.polygonDrawing.Location = new System.Drawing.Point(40, 1);
            this.polygonDrawing.Name = "polygonDrawing";
            this.polygonDrawing.Size = new System.Drawing.Size(38, 38);
            this.polygonDrawing.TabIndex = 1;
            this.toolTip.SetToolTip(this.polygonDrawing, "Polygon\r\nAlt+P");
            this.polygonDrawing.UseVisualStyleBackColor = false;
            this.polygonDrawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.polygonDrawing_MouseDown);
            // 
            // rectangleDrawing
            // 
            this.rectangleDrawing.BackColor = System.Drawing.Color.Transparent;
            this.rectangleDrawing.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rectangleDrawing.BackgroundImage")));
            this.rectangleDrawing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rectangleDrawing.Location = new System.Drawing.Point(2, 1);
            this.rectangleDrawing.Name = "rectangleDrawing";
            this.rectangleDrawing.Size = new System.Drawing.Size(38, 38);
            this.rectangleDrawing.TabIndex = 0;
            this.toolTip.SetToolTip(this.rectangleDrawing, "Rectangle\r\nAlt+R");
            this.rectangleDrawing.UseVisualStyleBackColor = false;
            this.rectangleDrawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rectangleDrawing_MouseDown);
            // 
            // rightCenterPanel
            // 
            this.rightCenterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightCenterPanel.Controls.Add(this.objectProperties);
            this.rightCenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightCenterPanel.Location = new System.Drawing.Point(3, 173);
            this.rightCenterPanel.Name = "rightCenterPanel";
            this.rightCenterPanel.Size = new System.Drawing.Size(425, 287);
            this.rightCenterPanel.TabIndex = 4;
            // 
            // objectProperties
            // 
            this.objectProperties.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.objectProperties.BackgroundColor = System.Drawing.SystemColors.Control;
            this.objectProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.objectProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.PropertyValue});
            this.objectProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectProperties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.objectProperties.GridColor = System.Drawing.SystemColors.ControlLight;
            this.objectProperties.Location = new System.Drawing.Point(0, 0);
            this.objectProperties.Name = "objectProperties";
            this.objectProperties.RowHeadersVisible = false;
            this.objectProperties.Size = new System.Drawing.Size(423, 285);
            this.objectProperties.TabIndex = 0;
            this.objectProperties.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.objectProperties_CellContentClick);
            this.objectProperties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.objectProperties_CellValueChanged);
            // 
            // PropertyName
            // 
            this.PropertyName.HeaderText = "Property name";
            this.PropertyName.Name = "PropertyName";
            // 
            // PropertyValue
            // 
            this.PropertyValue.HeaderText = "Property Value";
            this.PropertyValue.Name = "PropertyValue";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 466);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(425, 189);
            this.panel1.TabIndex = 25;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.predicateView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(425, 189);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // predicateView
            // 
            this.predicateView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.predicateView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.predicateView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.predicateView.ColumnHeadersVisible = false;
            this.predicateView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.predicateView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.predicateView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.predicateView.Enabled = false;
            this.predicateView.GridColor = System.Drawing.SystemColors.ControlLight;
            this.predicateView.Location = new System.Drawing.Point(3, 28);
            this.predicateView.Name = "predicateView";
            this.predicateView.RowHeadersVisible = false;
            this.predicateView.Size = new System.Drawing.Size(419, 158);
            this.predicateView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Property name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Predicates hold at frame ";
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1449, 24);
            this.menu.TabIndex = 3;
            this.menu.Text = "&File";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.switchWorkspaceToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.newProjectToolStripMenuItem.Text = "&New project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // switchWorkspaceToolStripMenuItem
            // 
            this.switchWorkspaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.othersToolStripMenuItem});
            this.switchWorkspaceToolStripMenuItem.Name = "switchWorkspaceToolStripMenuItem";
            this.switchWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.switchWorkspaceToolStripMenuItem.Text = "Switch workspace";
            // 
            // othersToolStripMenuItem
            // 
            this.othersToolStripMenuItem.Name = "othersToolStripMenuItem";
            this.othersToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.othersToolStripMenuItem.Text = "Others";
            this.othersToolStripMenuItem.Click += new System.EventHandler(this.otherWorkspaceToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.annotateTab);
            this.tabs.Controls.Add(this.recordTab);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 24);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1449, 899);
            this.tabs.TabIndex = 5;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            // 
            // annotateTab
            // 
            this.annotateTab.Controls.Add(this.annotateTableLayoutPanel);
            this.annotateTab.Location = new System.Drawing.Point(4, 22);
            this.annotateTab.Name = "annotateTab";
            this.annotateTab.Padding = new System.Windows.Forms.Padding(3);
            this.annotateTab.Size = new System.Drawing.Size(1441, 873);
            this.annotateTab.TabIndex = 0;
            this.annotateTab.Text = "Annotate";
            this.annotateTab.UseVisualStyleBackColor = true;
            // 
            // recordTab
            // 
            this.recordTab.Location = new System.Drawing.Point(4, 22);
            this.recordTab.Name = "recordTab";
            this.recordTab.Padding = new System.Windows.Forms.Padding(3);
            this.recordTab.Size = new System.Drawing.Size(1441, 873);
            this.recordTab.TabIndex = 1;
            this.recordTab.Text = "Record";
            this.recordTab.UseVisualStyleBackColor = true;
            // 
            // projectRightClickPanel
            // 
            this.projectRightClickPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.newSessionToolStripMenuItem,
            this.recordSessionToolStripMenuItem,
            this.refreshProjectMenuItem,
            this.projectGenerateToolStripMenuItem,
            this.statisticsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.projectRightClickPanel.Name = "cm1";
            this.projectRightClickPanel.Size = new System.Drawing.Size(153, 158);
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
            // refreshProjectMenuItem
            // 
            this.refreshProjectMenuItem.Name = "refreshProjectMenuItem";
            this.refreshProjectMenuItem.Size = new System.Drawing.Size(152, 22);
            this.refreshProjectMenuItem.Text = "Refresh";
            this.refreshProjectMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // projectGenerateToolStripMenuItem
            // 
            this.projectGenerateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectEventTemplateToolStripMenuItem,
            this.objectReferencesByNameMatchingToolStripMenuItem});
            this.projectGenerateToolStripMenuItem.Enabled = false;
            this.projectGenerateToolStripMenuItem.Name = "projectGenerateToolStripMenuItem";
            this.projectGenerateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.projectGenerateToolStripMenuItem.Text = "Generate";
            // 
            // projectEventTemplateToolStripMenuItem
            // 
            this.projectEventTemplateToolStripMenuItem.Name = "projectEventTemplateToolStripMenuItem";
            this.projectEventTemplateToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.projectEventTemplateToolStripMenuItem.Text = "Event template";
            this.projectEventTemplateToolStripMenuItem.Click += new System.EventHandler(this.projectEventTemplateToolStripMenuItem_Click);
            // 
            // objectReferencesByNameMatchingToolStripMenuItem
            // 
            this.objectReferencesByNameMatchingToolStripMenuItem.Name = "objectReferencesByNameMatchingToolStripMenuItem";
            this.objectReferencesByNameMatchingToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.objectReferencesByNameMatchingToolStripMenuItem.Text = "Object references by name matching";
            this.objectReferencesByNameMatchingToolStripMenuItem.Click += new System.EventHandler(this.objectReferencesByNameMatchingToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Enabled = false;
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
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
            this.exitWithoutSavingToolStripMenuItem,
            this.deleteSessionMenuItem,
            this.addFileToSessionMenuItem,
            this.refreshSessionMenuItem,
            this.resetToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.sessionDetectToolStripMenuItem,
            this.sessionGenerateToolStripMenuItem});
            this.sessionRightClickPanel.Name = "cm2";
            this.sessionRightClickPanel.Size = new System.Drawing.Size(174, 224);
            // 
            // editSessionMenuItem
            // 
            this.editSessionMenuItem.Enabled = false;
            this.editSessionMenuItem.Image = global::Annotator.Properties.Resources.edit_64;
            this.editSessionMenuItem.Name = "editSessionMenuItem";
            this.editSessionMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editSessionMenuItem.Size = new System.Drawing.Size(173, 22);
            this.editSessionMenuItem.Text = "Edit";
            this.editSessionMenuItem.Click += new System.EventHandler(this.editSessionMenuItem_Click);
            // 
            // saveSessionMenuItem
            // 
            this.saveSessionMenuItem.Enabled = false;
            this.saveSessionMenuItem.Image = global::Annotator.Properties.Resources.save_64;
            this.saveSessionMenuItem.Name = "saveSessionMenuItem";
            this.saveSessionMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSessionMenuItem.Size = new System.Drawing.Size(173, 22);
            this.saveSessionMenuItem.Text = "Save";
            this.saveSessionMenuItem.Click += new System.EventHandler(this.saveSessionMenuItem_Click);
            // 
            // exitWithoutSavingToolStripMenuItem
            // 
            this.exitWithoutSavingToolStripMenuItem.Enabled = false;
            this.exitWithoutSavingToolStripMenuItem.Name = "exitWithoutSavingToolStripMenuItem";
            this.exitWithoutSavingToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.exitWithoutSavingToolStripMenuItem.Text = "Exit without saving";
            this.exitWithoutSavingToolStripMenuItem.Click += new System.EventHandler(this.exitWithoutSavingToolStripMenuItem_Click);
            // 
            // deleteSessionMenuItem
            // 
            this.deleteSessionMenuItem.Enabled = false;
            this.deleteSessionMenuItem.Name = "deleteSessionMenuItem";
            this.deleteSessionMenuItem.Size = new System.Drawing.Size(173, 22);
            this.deleteSessionMenuItem.Text = "Delete";
            this.deleteSessionMenuItem.Click += new System.EventHandler(this.deleteSessionMenuItem_Click);
            // 
            // addFileToSessionMenuItem
            // 
            this.addFileToSessionMenuItem.Enabled = false;
            this.addFileToSessionMenuItem.Name = "addFileToSessionMenuItem";
            this.addFileToSessionMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addFileToSessionMenuItem.Text = "Add";
            this.addFileToSessionMenuItem.Click += new System.EventHandler(this.addFileToSessionMenuItem_Click);
            // 
            // refreshSessionMenuItem
            // 
            this.refreshSessionMenuItem.Name = "refreshSessionMenuItem";
            this.refreshSessionMenuItem.Size = new System.Drawing.Size(173, 22);
            this.refreshSessionMenuItem.Text = "Refresh";
            this.refreshSessionMenuItem.Click += new System.EventHandler(this.refreshSessionMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Enabled = false;
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Enabled = false;
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // sessionDetectToolStripMenuItem
            // 
            this.sessionDetectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.glyphboxToolStripMenuItem});
            this.sessionDetectToolStripMenuItem.Enabled = false;
            this.sessionDetectToolStripMenuItem.Name = "sessionDetectToolStripMenuItem";
            this.sessionDetectToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.sessionDetectToolStripMenuItem.Text = "Detect";
            // 
            // glyphboxToolStripMenuItem
            // 
            this.glyphboxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detectGlyphBox2dToolStripMenuItem,
            this.detectGlyphBox3dToolStripMenuItem});
            this.glyphboxToolStripMenuItem.Name = "glyphboxToolStripMenuItem";
            this.glyphboxToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.glyphboxToolStripMenuItem.Text = "Glyph box";
            // 
            // detectGlyphBox2dToolStripMenuItem
            // 
            this.detectGlyphBox2dToolStripMenuItem.Name = "detectGlyphBox2dToolStripMenuItem";
            this.detectGlyphBox2dToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.detectGlyphBox2dToolStripMenuItem.Text = "2D";
            this.detectGlyphBox2dToolStripMenuItem.Click += new System.EventHandler(this.detectGlyphBox2dToolStripMenuItem_Click);
            // 
            // detectGlyphBox3dToolStripMenuItem
            // 
            this.detectGlyphBox3dToolStripMenuItem.Name = "detectGlyphBox3dToolStripMenuItem";
            this.detectGlyphBox3dToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.detectGlyphBox3dToolStripMenuItem.Text = "3D";
            this.detectGlyphBox3dToolStripMenuItem.Click += new System.EventHandler(this.detectGlyphBox3dToolStripMenuItem_Click);
            // 
            // sessionGenerateToolStripMenuItem
            // 
            this.sessionGenerateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sessionEventTemplateToolStripMenuItem,
            this.copyObjectsFromPreviousSessionToolStripMenuItem});
            this.sessionGenerateToolStripMenuItem.Enabled = false;
            this.sessionGenerateToolStripMenuItem.Name = "sessionGenerateToolStripMenuItem";
            this.sessionGenerateToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.sessionGenerateToolStripMenuItem.Text = "Generate";
            // 
            // sessionEventTemplateToolStripMenuItem
            // 
            this.sessionEventTemplateToolStripMenuItem.Name = "sessionEventTemplateToolStripMenuItem";
            this.sessionEventTemplateToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.sessionEventTemplateToolStripMenuItem.Text = "Event template";
            this.sessionEventTemplateToolStripMenuItem.Click += new System.EventHandler(this.sessionEventTemplateToolStripMenuItem_Click);
            // 
            // copyObjectsFromPreviousSessionToolStripMenuItem
            // 
            this.copyObjectsFromPreviousSessionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromPreviousSessionToolStripMenuItem,
            this.fromSessionToolStripMenuItem});
            this.copyObjectsFromPreviousSessionToolStripMenuItem.Name = "copyObjectsFromPreviousSessionToolStripMenuItem";
            this.copyObjectsFromPreviousSessionToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.copyObjectsFromPreviousSessionToolStripMenuItem.Text = "Copy objects";
            // 
            // fromPreviousSessionToolStripMenuItem
            // 
            this.fromPreviousSessionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lastFrameToolStripMenuItem,
            this.lastAppearanceToolStripMenuItem});
            this.fromPreviousSessionToolStripMenuItem.Name = "fromPreviousSessionToolStripMenuItem";
            this.fromPreviousSessionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.fromPreviousSessionToolStripMenuItem.Text = "From previous session";
            // 
            // lastFrameToolStripMenuItem
            // 
            this.lastFrameToolStripMenuItem.Name = "lastFrameToolStripMenuItem";
            this.lastFrameToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.lastFrameToolStripMenuItem.Text = "Last frame";
            this.lastFrameToolStripMenuItem.Click += new System.EventHandler(this.lastFrameToolStripMenuItem_Click);
            // 
            // lastAppearanceToolStripMenuItem
            // 
            this.lastAppearanceToolStripMenuItem.Name = "lastAppearanceToolStripMenuItem";
            this.lastAppearanceToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.lastAppearanceToolStripMenuItem.Text = "Last appearance";
            this.lastAppearanceToolStripMenuItem.Click += new System.EventHandler(this.lastAppearanceToolStripMenuItem_Click);
            // 
            // fromSessionToolStripMenuItem
            // 
            this.fromSessionToolStripMenuItem.Name = "fromSessionToolStripMenuItem";
            this.fromSessionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.fromSessionToolStripMenuItem.Text = "From session";
            this.fromSessionToolStripMenuItem.Click += new System.EventHandler(this.fromSessionToolStripMenuItem_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.Color = System.Drawing.Color.Red;
            // 
            // fileRightClickPanel
            // 
            this.fileRightClickPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addObjectToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.fileRightClickPanel.Name = "cm1";
            this.fileRightClickPanel.Size = new System.Drawing.Size(138, 70);
            // 
            // addObjectToolStripMenuItem
            // 
            this.addObjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRigsFromFileToolStripMenuItem});
            this.addObjectToolStripMenuItem.Enabled = false;
            this.addObjectToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addObjectToolStripMenuItem.Name = "addObjectToolStripMenuItem";
            this.addObjectToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.addObjectToolStripMenuItem.Text = "Add objects";
            // 
            // addRigsFromFileToolStripMenuItem
            // 
            this.addRigsFromFileToolStripMenuItem.Enabled = false;
            this.addRigsFromFileToolStripMenuItem.Name = "addRigsFromFileToolStripMenuItem";
            this.addRigsFromFileToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addRigsFromFileToolStripMenuItem.Text = "Add rigs from file";
            this.addRigsFromFileToolStripMenuItem.Click += new System.EventHandler(this.addRigsFromFileToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Enabled = false;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeFileToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // annotationWorkspaceTitle
            // 
            this.annotationWorkspaceTitle.AutoSize = true;
            this.annotationWorkspaceTitle.Dock = System.Windows.Forms.DockStyle.Right;
            this.annotationWorkspaceTitle.Location = new System.Drawing.Point(1387, 24);
            this.annotationWorkspaceTitle.Name = "annotationWorkspaceTitle";
            this.annotationWorkspaceTitle.Size = new System.Drawing.Size(62, 13);
            this.annotationWorkspaceTitle.TabIndex = 6;
            this.annotationWorkspaceTitle.Text = "Workspace";
            this.annotationWorkspaceTitle.Click += new System.EventHandler(this.label1_Click);
            // 
            // workspaceRightClickPanel
            // 
            this.workspaceRightClickPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem1,
            this.refreshWorkspaceMenuItem});
            this.workspaceRightClickPanel.Name = "workspaceRightClickPanel";
            this.workspaceRightClickPanel.Size = new System.Drawing.Size(139, 48);
            // 
            // newProjectToolStripMenuItem1
            // 
            this.newProjectToolStripMenuItem1.Name = "newProjectToolStripMenuItem1";
            this.newProjectToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
            this.newProjectToolStripMenuItem1.Text = "New project";
            // 
            // refreshWorkspaceMenuItem
            // 
            this.refreshWorkspaceMenuItem.Name = "refreshWorkspaceMenuItem";
            this.refreshWorkspaceMenuItem.Size = new System.Drawing.Size(138, 22);
            this.refreshWorkspaceMenuItem.Text = "Refresh";
            // 
            // trainingPanel1
            // 
            this.trainingPanel1.Location = new System.Drawing.Point(6, 6);
            this.trainingPanel1.Name = "trainingPanel1";
            this.trainingPanel1.Size = new System.Drawing.Size(1420, 860);
            this.trainingPanel1.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1449, 923);
            this.Controls.Add(this.annotationWorkspaceTitle);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.menu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ECAT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Main_KeyUp);
            this.annotateTableLayoutPanel.ResumeLayout(false);
            this.leftMostPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.projectExplorer.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logGridView)).EndInit();
            this.middleTopPanel.ResumeLayout(false);
            this.middleTopTableLayoutPanel.ResumeLayout(false);
            this.middleTopTableLayoutPanel.PerformLayout();
            this.videoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameTrackBar)).EndInit();
            this.middleCenterPanel.ResumeLayout(false);
            this.middleBottomPanel.ResumeLayout(false);
            this.middleBottomTableLayoutPanel.ResumeLayout(false);
            this.rightColumnLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.annoRefView)).EndInit();
            this.cm3.ResumeLayout(false);
            this.rightTopPanel.ResumeLayout(false);
            this.selectObjContextPanel.ResumeLayout(false);
            this.editObjectContextPanel.ResumeLayout(false);
            this.newObjectContextPanel.ResumeLayout(false);
            this.newObjectContextPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borderSizeNumeric)).EndInit();
            this.drawingButtonTool.ResumeLayout(false);
            this.rightCenterPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectProperties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.predicateView)).EndInit();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.annotateTab.ResumeLayout(false);
            this.projectRightClickPanel.ResumeLayout(false);
            this.sessionRightClickPanel.ResumeLayout(false);
            this.fileRightClickPanel.ResumeLayout(false);
            this.workspaceRightClickPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        System.Windows.Forms.Label startInSecondLbl;
        System.Windows.Forms.TextBox startInSecondTextBox;
        System.Windows.Forms.Label endInSecondLbl;
        System.Windows.Forms.TextBox endInSecondTextBox;
        private System.Windows.Forms.TableLayoutPanel annotateTableLayoutPanel;
        private System.Windows.Forms.Panel leftMostPanel;
        private System.Windows.Forms.Panel middleTopPanel;
        private System.Windows.Forms.Panel middleBottomPanel;
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
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ComboBox playbackFileComboBox;
        private System.Windows.Forms.Panel videoPanel;
        private System.Windows.Forms.Label frameNumberLbl;
        private CustomizedTrackBar frameTrackBar;
        private System.Windows.Forms.Button addEventAnnotationBtn;
        private System.Windows.Forms.ToolStripMenuItem addFileToSessionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshSessionMenuItem;
        internal System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private MyPictureBox pictureBoard;
        private System.Windows.Forms.Button cancelObjectBtn;
        private System.Windows.Forms.Button addObjBtn;
        private System.Windows.Forms.Panel newObjectContextPanel;
        private System.Windows.Forms.Button chooseColorBtn;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.NumericUpDown borderSizeNumeric;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip cm3;
        private System.Windows.Forms.ToolStripMenuItem addObjRefToolStripMenuItem;
        private System.Windows.Forms.Panel rightTopPanel;
        private System.Windows.Forms.Panel drawingButtonTool;
        private System.Windows.Forms.Button rectangleDrawing;
        //private System.Windows.Forms.Button cursorDrawing;
        private System.Windows.Forms.Button polygonDrawing;
        private System.Windows.Forms.Panel middleCenterPanel;
        private System.Windows.Forms.Panel rightCenterPanel;
        private System.Windows.Forms.DataGridView objectProperties;
        private System.Windows.Forms.Panel editObjectContextPanel;
        private System.Windows.Forms.Button delAtFrameBtn;
        private System.Windows.Forms.Button delMarkerBtn;
        private System.Windows.Forms.Button cancelEditObjBtn;
        private System.Windows.Forms.Button addLocationBtn;
        private System.Windows.Forms.Panel selectObjContextPanel;
        private System.Windows.Forms.Button deleteObjBtn;
        private System.Windows.Forms.Button editObjBtn;
        private System.Windows.Forms.Button cancelSelectObjBtn;
        private System.Windows.Forms.RichTextBox annotationText;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.DataGridView annoRefView;
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
        private TableLayoutPanel middleTopTableLayoutPanel;
        private TableLayoutPanel middleCenterTableLayoutPanel;
        private TableLayoutPanel middleBottomTableLayoutPanel;
        private ToolStripMenuItem sessionDetectToolStripMenuItem;
        private ToolStripMenuItem glyphboxToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem sessionGenerateToolStripMenuItem;
        private ToolStripMenuItem sessionEventTemplateToolStripMenuItem;
        private ToolStripMenuItem projectGenerateToolStripMenuItem;
        private ToolStripMenuItem projectEventTemplateToolStripMenuItem;
        private ToolStripMenuItem objectReferencesByNameMatchingToolStripMenuItem;
        private ToolStripMenuItem copyObjectsFromPreviousSessionToolStripMenuItem;
        private ToolStripMenuItem fromPreviousSessionToolStripMenuItem;
        private ToolStripMenuItem fromSessionToolStripMenuItem;
        private Button zoomDrawing;
        private ToolStripMenuItem switchWorkspaceToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private TableLayoutPanel rightColumnLayoutPanel;
        private DataGridViewTextBoxColumn PropertyName;
        private DataGridViewTextBoxColumn PropertyValue;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private DataGridView predicateView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private ToolStripMenuItem othersToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem reloadToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel2;
        private Button playBtn;
        private ImageList imageList2;
        private Button redoBtn;
        private Button undoBtn;
        private TabPage tabPage1;
        private Panel panel2;
        private DataGridView logGridView;
        private ToolStripMenuItem lastFrameToolStripMenuItem;
        private ToolStripMenuItem lastAppearanceToolStripMenuItem;
        private DataGridViewTextBoxColumn Number;
        private DataGridViewTextBoxColumn Log;
        private ToolStripMenuItem statisticsToolStripMenuItem;
        private Label annotationWorkspaceTitle;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem refreshProjectMenuItem;
        private ContextMenuStrip workspaceRightClickPanel;
        private ToolStripMenuItem newProjectToolStripMenuItem1;
        private ToolStripMenuItem refreshWorkspaceMenuItem;
        private DataGridViewTextBoxColumn StartAnno;
        private DataGridViewTextBoxColumn EndAnno;
        private DataGridViewTextBoxColumn TextAnno;
        private DataGridViewTextBoxColumn NoteAnno;
        private ToolStripMenuItem exitWithoutSavingToolStripMenuItem;
        private ToolStripMenuItem detectGlyphBox2dToolStripMenuItem;
        private ToolStripMenuItem detectGlyphBox3dToolStripMenuItem;
    }
}

