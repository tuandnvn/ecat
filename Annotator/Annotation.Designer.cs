using Microsoft.VisualBasic.PowerPacks;
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
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.axis = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.label1 = new System.Windows.Forms.Label();
            this.selectBtn = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(164, 29);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(463, 20);
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
            this.rectangleShape1,
            this.lineShape3,
            this.lineShape2,
            this.axis});
            this.shapeContainer1.Size = new System.Drawing.Size(800, 50);
            this.shapeContainer1.TabIndex = 1;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.BackColor = System.Drawing.Color.Yellow;
            this.rectangleShape1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.rectangleShape1.FillGradientColor = System.Drawing.Color.Yellow;
            this.rectangleShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Percent60;
            this.rectangleShape1.Location = new System.Drawing.Point(14, 3);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(775, 23);
            // 
            // lineShape3
            // 
            this.lineShape3.BorderWidth = 4;
            this.lineShape3.Enabled = false;
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = this.axis.X2;
            this.lineShape3.X2 = this.axis.X2;
            this.lineShape3.Y1 = 24;
            this.lineShape3.Y2 = 4;
            this.lineShape3.Move += new System.EventHandler(this.lineShape3_Move);
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
            // lineShape2
            // 
            this.lineShape2.BorderWidth = 4;
            this.lineShape2.Enabled = false;
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = this.axis.X1;
            this.lineShape2.X2 = this.axis.X1;
            this.lineShape2.Y1 = 24;
            this.lineShape2.Y2 = 4;
            this.lineShape2.Move += new System.EventHandler(this.lineShape2_Move);
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
            // 
            // Annotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
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
        private LineShape lineShape2;
        private LineShape lineShape3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectBtn;
        private RectangleShape rectangleShape1;
        private System.Windows.Forms.Button remove;
    }
}
