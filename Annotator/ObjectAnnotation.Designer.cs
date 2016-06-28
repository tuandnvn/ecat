﻿using System.Collections.Generic;

namespace Annotator
{
    partial class ObjectAnnotation
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
            this.axis = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.select = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.info = new System.Windows.Forms.Label();
            this.generate3d = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.SuspendLayout();
            // 
            // axis
            // 
            this.axis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axis.Enabled = false;
            this.axis.Name = "lineShape1";
            this.axis.X1 = 8;
            this.axis.X2 = 786;
            this.axis.Y1 = 13;
            this.axis.Y2 = 13;
            // 
            // select
            // 
            this.select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.select.Location = new System.Drawing.Point(644, 29);
            this.select.Name = "select";
            this.select.Size = new System.Drawing.Size(75, 21);
            this.select.TabIndex = 3;
            this.select.Text = "Select";
            this.select.UseVisualStyleBackColor = true;
            this.select.Click += new System.EventHandler(this.select_Click);
            // 
            // remove
            // 
            this.remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remove.Location = new System.Drawing.Point(725, 29);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(75, 21);
            this.remove.TabIndex = 4;
            this.remove.Text = "Remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // info
            // 
            this.info.AutoSize = true;
            this.info.Location = new System.Drawing.Point(12, 32);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(24, 13);
            this.info.TabIndex = 5;
            this.info.Text = "info";
            // 
            // generate3d
            // 
            this.generate3d.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generate3d.Location = new System.Drawing.Point(563, 29);
            this.generate3d.Name = "generate3d";
            this.generate3d.Size = new System.Drawing.Size(75, 21);
            this.generate3d.TabIndex = 6;
            this.generate3d.Text = "Generate3D";
            this.generate3d.UseVisualStyleBackColor = true;
            this.generate3d.Click += new System.EventHandler(this.generate3d_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.axis});
            this.shapeContainer1.Size = new System.Drawing.Size(800, 54);
            this.shapeContainer1.TabIndex = 1;
            this.shapeContainer1.TabStop = false;
            // 
            // ObjectAnnotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.generate3d);
            this.Controls.Add(this.info);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.select);
            this.Controls.Add(this.shapeContainer1);
            this.DoubleBuffered = true;
            this.Name = "ObjectAnnotation";
            this.Size = new System.Drawing.Size(800, 54);
            this.SizeChanged += new System.EventHandler(this.ObjectAnnotation_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Microsoft.VisualBasic.PowerPacks.LineShape axis;
        private System.Windows.Forms.Button select;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Label info;
        private System.Windows.Forms.Button generate3d;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
    }
}
