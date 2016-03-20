namespace Annotator
{
    partial class LinkEventForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.linkToEventIdComboBox = new System.Windows.Forms.ComboBox();
            this.eventId = new System.Windows.Forms.Label();
            this.eventTxt = new System.Windows.Forms.Label();
            this.linkToEventTxt = new System.Windows.Forms.Label();
            this.linkEventTypeComboBox = new System.Windows.Forms.ComboBox();
            this.addLinkEventBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.linkToEventIdComboBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.eventId, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.eventTxt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.linkToEventTxt, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.linkEventTypeComboBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.addLinkEventBtn, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(588, 74);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // linkToEventIdComboBox
            // 
            this.linkToEventIdComboBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.linkToEventIdComboBox.FormattingEnabled = true;
            this.linkToEventIdComboBox.Location = new System.Drawing.Point(395, 3);
            this.linkToEventIdComboBox.Name = "linkToEventIdComboBox";
            this.linkToEventIdComboBox.Size = new System.Drawing.Size(190, 21);
            this.linkToEventIdComboBox.TabIndex = 4;
            this.linkToEventIdComboBox.SelectedIndexChanged += new System.EventHandler(this.linkToEventIdComboBox_SelectedIndexChanged);
            // 
            // eventId
            // 
            this.eventId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.eventId.AutoSize = true;
            this.eventId.Location = new System.Drawing.Point(3, 5);
            this.eventId.Name = "eventId";
            this.eventId.Size = new System.Drawing.Size(190, 13);
            this.eventId.TabIndex = 0;
            // 
            // eventTxt
            // 
            this.eventTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.eventTxt.AutoSize = true;
            this.eventTxt.Location = new System.Drawing.Point(3, 29);
            this.eventTxt.Name = "eventTxt";
            this.eventTxt.Size = new System.Drawing.Size(190, 13);
            this.eventTxt.TabIndex = 1;
            // 
            // linkToEventTxt
            // 
            this.linkToEventTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkToEventTxt.AutoSize = true;
            this.linkToEventTxt.Location = new System.Drawing.Point(395, 29);
            this.linkToEventTxt.Name = "linkToEventTxt";
            this.linkToEventTxt.Size = new System.Drawing.Size(190, 13);
            this.linkToEventTxt.TabIndex = 2;
            // 
            // linkEventTypeComboBox
            // 
            this.linkEventTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkEventTypeComboBox.FormattingEnabled = true;
            this.linkEventTypeComboBox.Location = new System.Drawing.Point(199, 3);
            this.linkEventTypeComboBox.Name = "linkEventTypeComboBox";
            this.linkEventTypeComboBox.Size = new System.Drawing.Size(190, 21);
            this.linkEventTypeComboBox.TabIndex = 3;
            // 
            // addLinkEventBtn
            // 
            this.addLinkEventBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.addLinkEventBtn.Location = new System.Drawing.Point(510, 51);
            this.addLinkEventBtn.Name = "addLinkEventBtn";
            this.addLinkEventBtn.Size = new System.Drawing.Size(75, 20);
            this.addLinkEventBtn.TabIndex = 5;
            this.addLinkEventBtn.Text = "Add";
            this.addLinkEventBtn.UseVisualStyleBackColor = true;
            this.addLinkEventBtn.Click += new System.EventHandler(this.addLinkEventBtn_Click);
            // 
            // LinkEventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 78);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LinkEventForm";
            this.Text = "Link event";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox linkToEventIdComboBox;
        private System.Windows.Forms.Label eventId;
        private System.Windows.Forms.Label eventTxt;
        private System.Windows.Forms.Label linkToEventTxt;
        private System.Windows.Forms.ComboBox linkEventTypeComboBox;
        private System.Windows.Forms.Button addLinkEventBtn;
    }
}