using Microsoft.VisualBasic.PowerPacks;
using System.Drawing;

namespace Annotator
{
    partial class Range
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
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.axis = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rightMarker = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.leftMarker = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rectangleShape = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.intervalLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.shapeContainer1.Size = new System.Drawing.Size(800, 53);
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
            // rectangleShape
            // 
            this.rectangleShape.BackColor = System.Drawing.Color.Yellow;
            this.rectangleShape.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.rectangleShape.FillGradientColor = System.Drawing.Color.Yellow;
            this.rectangleShape.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Percent80;
            this.rectangleShape.Location = new System.Drawing.Point(14, 8);
            this.rectangleShape.Name = "rectangleShape1";
            this.rectangleShape.Size = new System.Drawing.Size(775, 12);
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
            // Range
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.intervalLbl);
            this.Controls.Add(this.shapeContainer1);
            this.DoubleBuffered = true;
            this.Name = "Range";
            this.Size = new System.Drawing.Size(800, 53);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Range_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Range_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Range_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ShapeContainer shapeContainer1;
        private LineShape axis;
        private LineShape leftMarker;
        private LineShape rightMarker;
        private System.Windows.Forms.Label intervalLbl;
        private RectangleShape rectangleShape;
    }
}
