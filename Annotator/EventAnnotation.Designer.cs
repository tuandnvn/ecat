﻿using Microsoft.VisualBasic.PowerPacks;
using System.Drawing;

namespace Annotator
{
    partial class EventAnnotation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textAnnotation = new System.Windows.Forms.TextBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.axis = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rightMarker = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.leftMarker = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rectangleShape = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.intervalLbl = new System.Windows.Forms.Label();
            this.selectBtn = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.subEventLinkBtn = new System.Windows.Forms.Button();
            this.findObjectBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textAnnotation
            // 
            this.textAnnotation.Enabled = false;
            this.textAnnotation.Location = new System.Drawing.Point(164, 29);
            this.textAnnotation.Multiline = true;
            this.textAnnotation.Name = "textAnnotation";
            this.textAnnotation.Size = new System.Drawing.Size(303, 20);
            this.textAnnotation.TabIndex = 0;
            this.textAnnotation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textAnnotation_KeyPress);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.axis,
            this.rightMarker,
            this.leftMarker,
            this.rectangleShape});
            this.shapeContainer1.Size = new System.Drawing.Size(800, 54);
            this.shapeContainer1.TabIndex = 1;
            this.shapeContainer1.TabStop = false;
            // 
            // axis
            // 
            this.axis.Enabled = false;
            this.axis.Name = "lineShape1";
            this.axis.X1 = 12;
            this.axis.X2 = 790;
            this.axis.Y1 = 14;
            this.axis.Y2 = 14;
            // 
            // rightMarker
            // 
            this.rightMarker.BorderWidth = 2;
            this.rightMarker.Enabled = false;
            this.rightMarker.Name = "lineShape3";
            this.rightMarker.X1 = this.axis.X2;
            this.rightMarker.X2 = this.axis.X2;
            this.rightMarker.Y1 = 24;
            this.rightMarker.Y2 = 4;
            this.rightMarker.Move += new System.EventHandler(this.rightMarker_Move);
            // 
            // leftMarker
            // 
            this.leftMarker.BorderWidth = 2;
            this.leftMarker.Enabled = false;
            this.leftMarker.Name = "lineShape2";
            this.leftMarker.X1 = this.axis.X1;
            this.leftMarker.X2 = this.axis.X1;
            this.leftMarker.Y1 = 24;
            this.leftMarker.Y2 = 4;
            this.leftMarker.Move += new System.EventHandler(this.leftMarker_Move);
            // 
            // rectangleShape
            // 
            this.rectangleShape.BackColor = System.Drawing.Color.Yellow;
            this.rectangleShape.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.rectangleShape.FillGradientColor = System.Drawing.Color.Yellow;
            this.rectangleShape.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Percent80;
            this.rectangleShape.Location = new System.Drawing.Point(14, 3);
            this.rectangleShape.Name = "rectangleShape1";
            this.rectangleShape.Size = new System.Drawing.Size(775, 23);
            // 
            // intervalLbl
            // 
            this.intervalLbl.AutoSize = true;
            this.intervalLbl.Location = new System.Drawing.Point(12, 32);
            this.intervalLbl.Name = "intervalLbl";
            this.intervalLbl.Size = new System.Drawing.Size(69, 13);
            this.intervalLbl.TabIndex = 2;
            this.intervalLbl.Text = "Start: , Stop: ";
            // 
            // selectBtn
            // 
            this.selectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectBtn.Location = new System.Drawing.Point(635, 29);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(75, 21);
            this.selectBtn.TabIndex = 3;
            this.selectBtn.Text = "Edit";
            this.selectBtn.UseVisualStyleBackColor = true;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // remove
            // 
            this.remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remove.Location = new System.Drawing.Point(716, 29);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(75, 21);
            this.remove.TabIndex = 5;
            this.remove.Text = "Remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // subEventLinkBtn
            // 
            this.subEventLinkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.subEventLinkBtn.Enabled = false;
            this.subEventLinkBtn.Location = new System.Drawing.Point(554, 29);
            this.subEventLinkBtn.Name = "subEventLinkBtn";
            this.subEventLinkBtn.Size = new System.Drawing.Size(75, 21);
            this.subEventLinkBtn.TabIndex = 6;
            this.subEventLinkBtn.Text = "Link to";
            this.subEventLinkBtn.UseVisualStyleBackColor = true;
            this.subEventLinkBtn.Click += new System.EventHandler(this.subEventLink_Click);
            // 
            // findObjectBtn
            // 
            this.findObjectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findObjectBtn.Enabled = false;
            this.findObjectBtn.Location = new System.Drawing.Point(473, 29);
            this.findObjectBtn.Name = "findObjectBtn";
            this.findObjectBtn.Size = new System.Drawing.Size(75, 21);
            this.findObjectBtn.TabIndex = 7;
            this.findObjectBtn.Text = "Find objects";
            this.findObjectBtn.UseVisualStyleBackColor = true;
            this.findObjectBtn.Click += new System.EventHandler(this.findObjectBtn_Click);
            // 
            // EventAnnotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.findObjectBtn);
            this.Controls.Add(this.subEventLinkBtn);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.intervalLbl);
            this.Controls.Add(this.textAnnotation);
            this.Controls.Add(this.shapeContainer1);
            this.DoubleBuffered = true;
            this.Name = "EventAnnotation";
            this.Size = new System.Drawing.Size(800, 54);
            this.SizeChanged += new System.EventHandler(this.EventAnnotation_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Annotation_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Annotation_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Annotation_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textAnnotation;
        private ShapeContainer shapeContainer1;
        private LineShape axis;
        private LineShape leftMarker;
        private LineShape rightMarker;
        private System.Windows.Forms.Label intervalLbl;
        private System.Windows.Forms.Button selectBtn;
        private RectangleShape rectangleShape;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button subEventLinkBtn;
        private System.Windows.Forms.Button findObjectBtn;
    }
}
