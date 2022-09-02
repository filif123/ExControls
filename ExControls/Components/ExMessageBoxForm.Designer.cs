
namespace ExControls
{
    partial class ExMessageBoxForm
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
            ShellIcon?.Dispose();
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lText = new System.Windows.Forms.Label();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.flowLPFooter = new System.Windows.Forms.FlowLayoutPanel();
            this.bHelp = new System.Windows.Forms.Button();
            this.bIgnore = new System.Windows.Forms.Button();
            this.bRetry = new System.Windows.Forms.Button();
            this.bAbort = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.bNo = new System.Windows.Forms.Button();
            this.bYes = new System.Windows.Forms.Button();
            this.timerCountDown = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.flowLPFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.lText, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.picIcon, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.flowLPFooter, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(682, 153);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lText
            // 
            this.lText.AutoEllipsis = true;
            this.lText.AutoSize = true;
            this.lText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lText.Location = new System.Drawing.Point(79, 0);
            this.lText.Name = "lText";
            this.lText.Padding = new System.Windows.Forms.Padding(10, 5, 100, 5);
            this.lText.Size = new System.Drawing.Size(600, 100);
            this.lText.TabIndex = 2;
            this.lText.Text = "text";
            this.lText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picIcon
            // 
            this.picIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picIcon.Location = new System.Drawing.Point(3, 3);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(70, 94);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picIcon.TabIndex = 1;
            this.picIcon.TabStop = false;
            // 
            // flowLPFooter
            // 
            this.flowLPFooter.AutoSize = true;
            this.flowLPFooter.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.SetColumnSpan(this.flowLPFooter, 2);
            this.flowLPFooter.Controls.Add(this.bHelp);
            this.flowLPFooter.Controls.Add(this.bIgnore);
            this.flowLPFooter.Controls.Add(this.bRetry);
            this.flowLPFooter.Controls.Add(this.bAbort);
            this.flowLPFooter.Controls.Add(this.bCancel);
            this.flowLPFooter.Controls.Add(this.bOK);
            this.flowLPFooter.Controls.Add(this.bNo);
            this.flowLPFooter.Controls.Add(this.bYes);
            this.flowLPFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLPFooter.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLPFooter.Location = new System.Drawing.Point(0, 100);
            this.flowLPFooter.Margin = new System.Windows.Forms.Padding(0);
            this.flowLPFooter.Name = "flowLPFooter";
            this.flowLPFooter.Padding = new System.Windows.Forms.Padding(10);
            this.flowLPFooter.Size = new System.Drawing.Size(682, 53);
            this.flowLPFooter.TabIndex = 0;
            this.flowLPFooter.WrapContents = false;
            // 
            // bHelp
            // 
            this.bHelp.AutoSize = true;
            this.bHelp.Location = new System.Drawing.Point(552, 13);
            this.bHelp.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.bHelp.Name = "bHelp";
            this.bHelp.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bHelp.Size = new System.Drawing.Size(90, 27);
            this.bHelp.TabIndex = 7;
            this.bHelp.Text = "Help";
            this.bHelp.UseVisualStyleBackColor = true;
            this.bHelp.Visible = false;
            this.bHelp.Click += new System.EventHandler(this.bHelp_Click);
            // 
            // bIgnore
            // 
            this.bIgnore.AutoSize = true;
            this.bIgnore.Location = new System.Drawing.Point(439, 13);
            this.bIgnore.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.bIgnore.Name = "bIgnore";
            this.bIgnore.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bIgnore.Size = new System.Drawing.Size(90, 27);
            this.bIgnore.TabIndex = 4;
            this.bIgnore.Text = "Ignore";
            this.bIgnore.UseVisualStyleBackColor = true;
            this.bIgnore.Visible = false;
            this.bIgnore.Click += new System.EventHandler(this.bIgnore_Click);
            // 
            // bRetry
            // 
            this.bRetry.AutoSize = true;
            this.bRetry.Location = new System.Drawing.Point(341, 13);
            this.bRetry.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.bRetry.Name = "bRetry";
            this.bRetry.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bRetry.Size = new System.Drawing.Size(90, 27);
            this.bRetry.TabIndex = 3;
            this.bRetry.Text = "Retry";
            this.bRetry.UseVisualStyleBackColor = true;
            this.bRetry.Visible = false;
            this.bRetry.Click += new System.EventHandler(this.bRetry_Click);
            // 
            // bAbort
            // 
            this.bAbort.AutoSize = true;
            this.bAbort.Location = new System.Drawing.Point(243, 13);
            this.bAbort.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.bAbort.Name = "bAbort";
            this.bAbort.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bAbort.Size = new System.Drawing.Size(90, 27);
            this.bAbort.TabIndex = 2;
            this.bAbort.Text = "Abort";
            this.bAbort.UseVisualStyleBackColor = true;
            this.bAbort.Visible = false;
            this.bAbort.Click += new System.EventHandler(this.bAbort_Click);
            // 
            // bCancel
            // 
            this.bCancel.AutoSize = true;
            this.bCancel.Location = new System.Drawing.Point(145, 13);
            this.bCancel.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.bCancel.Name = "bCancel";
            this.bCancel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bCancel.Size = new System.Drawing.Size(90, 27);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Visible = false;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.AutoSize = true;
            this.bOK.Location = new System.Drawing.Point(47, 13);
            this.bOK.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.bOK.Name = "bOK";
            this.bOK.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bOK.Size = new System.Drawing.Size(90, 27);
            this.bOK.TabIndex = 0;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Visible = false;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bNo
            // 
            this.bNo.AutoSize = true;
            this.bNo.Location = new System.Drawing.Point(-51, 13);
            this.bNo.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.bNo.Name = "bNo";
            this.bNo.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bNo.Size = new System.Drawing.Size(90, 27);
            this.bNo.TabIndex = 6;
            this.bNo.Text = "No";
            this.bNo.UseVisualStyleBackColor = true;
            this.bNo.Visible = false;
            this.bNo.Click += new System.EventHandler(this.bNo_Click);
            // 
            // bYes
            // 
            this.bYes.AutoSize = true;
            this.bYes.Location = new System.Drawing.Point(-149, 13);
            this.bYes.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.bYes.Name = "bYes";
            this.bYes.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.bYes.Size = new System.Drawing.Size(90, 27);
            this.bYes.TabIndex = 5;
            this.bYes.Text = "Yes";
            this.bYes.UseVisualStyleBackColor = true;
            this.bYes.Visible = false;
            this.bYes.Click += new System.EventHandler(this.bYes_Click);
            // 
            // timerCountDown
            // 
            this.timerCountDown.Tick += new System.EventHandler(this.timerCountDown_Tick);
            // 
            // ExMessageBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(682, 153);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 500);
            this.MinimizeBox = false;
            this.Name = "ExMessageBoxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExMessageBox";
            this.Deactivate += new System.EventHandler(this.ExMessageBoxForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExMessageBoxForm_FormClosed);
            this.Shown += new System.EventHandler(this.ExMessageBoxForm_Shown);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.flowLPFooter.ResumeLayout(false);
            this.flowLPFooter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLPFooter;
        internal System.Windows.Forms.Button bIgnore;
        internal System.Windows.Forms.Button bRetry;
        internal System.Windows.Forms.Button bAbort;
        internal System.Windows.Forms.Button bCancel;
        internal System.Windows.Forms.Button bOK;
        internal System.Windows.Forms.Button bNo;
        internal System.Windows.Forms.Button bYes;
        internal System.Windows.Forms.PictureBox picIcon;
        internal System.Windows.Forms.Label lText;
        internal System.Windows.Forms.Button bHelp;
        private System.Windows.Forms.Timer timerCountDown;
    }
}