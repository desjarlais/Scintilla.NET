namespace ScintillaNET
{
    partial class FlagsEditorControl
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
                this.flowLayoutPanel_CheckBoxList = new System.Windows.Forms.FlowLayoutPanel();
                this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.button_Ok = new System.Windows.Forms.Button();
                this.button_Cancel = new System.Windows.Forms.Button();
                this.tableLayoutPanel.SuspendLayout();
                this.SuspendLayout();
                // 
                // flowLayoutPanel_CheckBoxList
                // 
                this.flowLayoutPanel_CheckBoxList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
                this.flowLayoutPanel_CheckBoxList.AutoScroll = true;
                this.flowLayoutPanel_CheckBoxList.AutoSize = true;
                this.flowLayoutPanel_CheckBoxList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tableLayoutPanel.SetColumnSpan(this.flowLayoutPanel_CheckBoxList, 2);
                this.flowLayoutPanel_CheckBoxList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                this.flowLayoutPanel_CheckBoxList.Location = new System.Drawing.Point(0, 0);
                this.flowLayoutPanel_CheckBoxList.Margin = new System.Windows.Forms.Padding(0);
                this.flowLayoutPanel_CheckBoxList.Name = "flowLayoutPanel_CheckBoxList";
                this.flowLayoutPanel_CheckBoxList.Size = new System.Drawing.Size(150, 124);
                this.flowLayoutPanel_CheckBoxList.TabIndex = 0;
                this.flowLayoutPanel_CheckBoxList.WrapContents = false;
                // 
                // tableLayoutPanel
                // 
                this.tableLayoutPanel.AutoSize = true;
                this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tableLayoutPanel.ColumnCount = 2;
                this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
                this.tableLayoutPanel.Controls.Add(this.flowLayoutPanel_CheckBoxList, 0, 0);
                this.tableLayoutPanel.Controls.Add(this.button_Ok, 0, 1);
                this.tableLayoutPanel.Controls.Add(this.button_Cancel, 1, 1);
                this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
                this.tableLayoutPanel.Name = "tableLayoutPanel";
                this.tableLayoutPanel.RowCount = 2;
                this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
                this.tableLayoutPanel.Size = new System.Drawing.Size(150, 150);
                this.tableLayoutPanel.TabIndex = 0;
                // 
                // button_Ok
                // 
                this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
                this.button_Ok.AutoSize = true;
                this.button_Ok.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.button_Ok.Location = new System.Drawing.Point(0, 124);
                this.button_Ok.Margin = new System.Windows.Forms.Padding(0);
                this.button_Ok.Name = "button_Ok";
                this.button_Ok.Size = new System.Drawing.Size(75, 26);
                this.button_Ok.TabIndex = 1;
                this.button_Ok.Text = "OK";
                this.button_Ok.UseVisualStyleBackColor = false;
                this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
                // 
                // button_Cancel
                // 
                this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
                this.button_Cancel.AutoSize = true;
                this.button_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.button_Cancel.Location = new System.Drawing.Point(75, 124);
                this.button_Cancel.Margin = new System.Windows.Forms.Padding(0);
                this.button_Cancel.Name = "button_Cancel";
                this.button_Cancel.Size = new System.Drawing.Size(75, 26);
                this.button_Cancel.TabIndex = 1;
                this.button_Cancel.Text = "Cancel";
                this.button_Cancel.UseVisualStyleBackColor = false;
                this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
                // 
                // FlagsEditorControl
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
                this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.Controls.Add(this.tableLayoutPanel);
                this.Name = "FlagsEditorControl";
                this.tableLayoutPanel.ResumeLayout(false);
                this.tableLayoutPanel.PerformLayout();
                this.ResumeLayout(false);
                this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_CheckBoxList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
    }
}
