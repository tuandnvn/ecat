﻿using Microsoft.VisualBasic.PowerPacks;
using System.Drawing;

namespace Annotator
{
    partial class Annotation
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.axis = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rightMarker = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.leftMarker = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.label1 = new System.Windows.Forms.Label();
            this.selectBtn = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.subEventLink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(164, 29);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(384, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.DoubleClick += new System.EventHandler(this.textBox1_DoubleClick);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
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
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(800, 50);
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
            this.rightMarker.Move += new System.EventHandler(this.lineShape3_Move);
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
            this.leftMarker.Move += new System.EventHandler(this.lineShape2_Move);
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.BackColor = System.Drawing.Color.Yellow;
            this.rectangleShape1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.rectangleShape1.FillGradientColor = System.Drawing.Color.Yellow;
            this.rectangleShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Percent80;
            this.rectangleShape1.Location = new System.Drawing.Point(14, 3);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(775, 23);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Frame: , StopFrame: ";
            // 
            // selectBtn
            // 
            this.selectBtn.Location = new System.Drawing.Point(635, 29);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(75, 21);
            this.selectBtn.TabIndex = 3;
            this.selectBtn.Text = "Select";
            this.selectBtn.UseVisualStyleBackColor = true;
            this.selectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // remove
            // 
            this.remove.Location = new System.Drawing.Point(716, 29);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(75, 21);
            this.remove.TabIndex = 5;
            this.remove.Text = "Remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // subEventLink
            // 
            this.subEventLink.Location = new System.Drawing.Point(554, 29);
            this.subEventLink.Name = "subEventLink";
            this.subEventLink.Size = new System.Drawing.Size(75, 21);
            this.subEventLink.TabIndex = 6;
            this.subEventLink.Text = "Link to";
            this.subEventLink.UseVisualStyleBackColor = true;
            // 
            // Annotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.subEventLink);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.shapeContainer1);
            this.DoubleBuffered = true;
            this.Name = "Annotation";
            this.Size = new System.Drawing.Size(800, 50);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Annotation_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Annotation_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Annotation_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private ShapeContainer shapeContainer1;
        private LineShape axis;
        private LineShape leftMarker;
        private LineShape rightMarker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectBtn;
        private RectangleShape rectangleShape1;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button subEventLink;
    }
}
