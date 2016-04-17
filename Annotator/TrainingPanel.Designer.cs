namespace Annotator
{
    partial class TrainingPanel
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.featureBox = new System.Windows.Forms.GroupBox();
            this.featuresLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.resultBox = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trainBox = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.testBox = new System.Windows.Forms.GroupBox();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.testConfigBox = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.useTrainedModelBtn = new System.Windows.Forms.Button();
            this.testBtn = new System.Windows.Forms.Button();
            this.loadModelBtn = new System.Windows.Forms.Button();
            this.trainConfigBox = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.featureBox.SuspendLayout();
            this.featuresLayoutPanel.SuspendLayout();
            this.resultBox.SuspendLayout();
            this.trainBox.SuspendLayout();
            this.testBox.SuspendLayout();
            this.testConfigBox.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.trainConfigBox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.Controls.Add(this.featureBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.resultBox, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.trainBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.testBox, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.testConfigBox, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.trainConfigBox, 2, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1414, 854);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // featureBox
            // 
            this.featureBox.Controls.Add(this.featuresLayoutPanel);
            this.featureBox.Location = new System.Drawing.Point(285, 3);
            this.featureBox.Name = "featureBox";
            this.featureBox.Size = new System.Drawing.Size(418, 421);
            this.featureBox.TabIndex = 3;
            this.featureBox.TabStop = false;
            this.featureBox.Text = "Features";
            // 
            // featuresLayoutPanel
            // 
            this.featuresLayoutPanel.ColumnCount = 1;
            this.featuresLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.featuresLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.featuresLayoutPanel.Controls.Add(this.groupBox2, 0, 1);
            this.featuresLayoutPanel.Controls.Add(this.groupBox1, 0, 0);
            this.featuresLayoutPanel.Location = new System.Drawing.Point(6, 19);
            this.featuresLayoutPanel.Name = "featuresLayoutPanel";
            this.featuresLayoutPanel.RowCount = 2;
            this.featuresLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.featuresLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.featuresLayoutPanel.Size = new System.Drawing.Size(412, 396);
            this.featuresLayoutPanel.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(3, 201);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 192);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Object features";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 192);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rig features";
            // 
            // resultBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.resultBox, 3);
            this.resultBox.Controls.Add(this.panel1);
            this.resultBox.Location = new System.Drawing.Point(285, 430);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(1126, 421);
            this.resultBox.TabIndex = 4;
            this.resultBox.TabStop = false;
            this.resultBox.Text = "Result";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1114, 396);
            this.panel1.TabIndex = 0;
            // 
            // trainBox
            // 
            this.trainBox.Controls.Add(this.treeView1);
            this.trainBox.Location = new System.Drawing.Point(3, 3);
            this.trainBox.Name = "trainBox";
            this.trainBox.Size = new System.Drawing.Size(276, 421);
            this.trainBox.TabIndex = 0;
            this.trainBox.TabStop = false;
            this.trainBox.Text = "Train";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(6, 19);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(264, 396);
            this.treeView1.TabIndex = 0;
            // 
            // testBox
            // 
            this.testBox.Controls.Add(this.treeView2);
            this.testBox.Location = new System.Drawing.Point(3, 430);
            this.testBox.Name = "testBox";
            this.testBox.Size = new System.Drawing.Size(276, 421);
            this.testBox.TabIndex = 1;
            this.testBox.TabStop = false;
            this.testBox.Text = "Test";
            // 
            // treeView2
            // 
            this.treeView2.Location = new System.Drawing.Point(6, 19);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(264, 396);
            this.treeView2.TabIndex = 1;
            // 
            // testConfigBox
            // 
            this.testConfigBox.Controls.Add(this.groupBox6);
            this.testConfigBox.Controls.Add(this.groupBox5);
            this.testConfigBox.Controls.Add(this.useTrainedModelBtn);
            this.testConfigBox.Controls.Add(this.testBtn);
            this.testConfigBox.Controls.Add(this.loadModelBtn);
            this.testConfigBox.Location = new System.Drawing.Point(1062, 3);
            this.testConfigBox.Name = "testConfigBox";
            this.testConfigBox.Size = new System.Drawing.Size(349, 421);
            this.testConfigBox.TabIndex = 3;
            this.testConfigBox.TabStop = false;
            this.testConfigBox.Text = "Test";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.richTextBox2);
            this.groupBox6.Location = new System.Drawing.Point(8, 199);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(335, 187);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Progress";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox2.Location = new System.Drawing.Point(8, 19);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(321, 162);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(8, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(335, 168);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Congifuration";
            // 
            // useTrainedModelBtn
            // 
            this.useTrainedModelBtn.Location = new System.Drawing.Point(50, 392);
            this.useTrainedModelBtn.Name = "useTrainedModelBtn";
            this.useTrainedModelBtn.Size = new System.Drawing.Size(131, 23);
            this.useTrainedModelBtn.TabIndex = 3;
            this.useTrainedModelBtn.Text = "Use just trained model";
            this.useTrainedModelBtn.UseVisualStyleBackColor = true;
            // 
            // testBtn
            // 
            this.testBtn.Location = new System.Drawing.Point(187, 392);
            this.testBtn.Name = "testBtn";
            this.testBtn.Size = new System.Drawing.Size(75, 23);
            this.testBtn.TabIndex = 2;
            this.testBtn.Text = "Test";
            this.testBtn.UseVisualStyleBackColor = true;
            // 
            // loadModelBtn
            // 
            this.loadModelBtn.Location = new System.Drawing.Point(268, 392);
            this.loadModelBtn.Name = "loadModelBtn";
            this.loadModelBtn.Size = new System.Drawing.Size(75, 23);
            this.loadModelBtn.TabIndex = 2;
            this.loadModelBtn.Text = "Load model";
            this.loadModelBtn.UseVisualStyleBackColor = true;
            // 
            // trainConfigBox
            // 
            this.trainConfigBox.Controls.Add(this.groupBox4);
            this.trainConfigBox.Controls.Add(this.groupBox3);
            this.trainConfigBox.Controls.Add(this.button2);
            this.trainConfigBox.Controls.Add(this.button1);
            this.trainConfigBox.Location = new System.Drawing.Point(709, 3);
            this.trainConfigBox.Name = "trainConfigBox";
            this.trainConfigBox.Size = new System.Drawing.Size(347, 421);
            this.trainConfigBox.TabIndex = 2;
            this.trainConfigBox.TabStop = false;
            this.trainConfigBox.Text = "Train";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.richTextBox1);
            this.groupBox4.Location = new System.Drawing.Point(6, 196);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(335, 187);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Progress";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.Location = new System.Drawing.Point(8, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(321, 162);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(6, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(335, 168);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Congifuration";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(185, 389);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Train";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(266, 389);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save model";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TrainingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "TrainingPanel";
            this.Size = new System.Drawing.Size(1420, 860);
            this.tableLayoutPanel.ResumeLayout(false);
            this.featureBox.ResumeLayout(false);
            this.featuresLayoutPanel.ResumeLayout(false);
            this.resultBox.ResumeLayout(false);
            this.trainBox.ResumeLayout(false);
            this.testBox.ResumeLayout(false);
            this.testConfigBox.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.trainConfigBox.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.GroupBox trainBox;
        private System.Windows.Forms.GroupBox testBox;
        private System.Windows.Forms.GroupBox trainConfigBox;
        private System.Windows.Forms.GroupBox testConfigBox;
        private System.Windows.Forms.GroupBox resultBox;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox featureBox;
        private System.Windows.Forms.TableLayoutPanel featuresLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button useTrainedModelBtn;
        private System.Windows.Forms.Button testBtn;
        private System.Windows.Forms.Button loadModelBtn;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}
