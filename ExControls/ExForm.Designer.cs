
namespace ExControls
{
    partial class ExForm
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
            this.FormTitle = new System.Windows.Forms.TableLayoutPanel();
            this.ToolStripLeft = new System.Windows.Forms.ToolStrip();
            this.AppIcon = new System.Windows.Forms.PictureBox();
            this.RightMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.TitleButtonExit = new ExTitleButton();
            this.TitleName = new System.Windows.Forms.Label();
            this.FormTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppIcon)).BeginInit();
            this.RightMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormTitle
            // 
            this.FormTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.FormTitle.ColumnCount = 4;
            this.FormTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FormTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FormTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FormTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.FormTitle.Controls.Add(this.ToolStripLeft, 1, 0);
            this.FormTitle.Controls.Add(this.AppIcon, 0, 0);
            this.FormTitle.Controls.Add(this.RightMenu, 3, 0);
            this.FormTitle.Controls.Add(this.TitleName, 2, 0);
            this.FormTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormTitle.Location = new System.Drawing.Point(0, 0);
            this.FormTitle.Name = "FormTitle";
            this.FormTitle.RowCount = 1;
            this.FormTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.FormTitle.Size = new System.Drawing.Size(531, 36);
            this.FormTitle.TabIndex = 0;
            this.FormTitle.DoubleClick += new System.EventHandler(this.FormTitle_DoubleClick);
            // 
            // ToolStripLeft
            // 
            this.ToolStripLeft.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolStripLeft.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripLeft.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolStripLeft.Location = new System.Drawing.Point(38, 0);
            this.ToolStripLeft.Name = "ToolStripLeft";
            this.ToolStripLeft.Size = new System.Drawing.Size(127, 36);
            this.ToolStripLeft.TabIndex = 0;
            this.ToolStripLeft.Text = "toolStrip1";
            this.ToolStripLeft.ItemAdded += new System.Windows.Forms.ToolStripItemEventHandler(this.ToolStripLeft_ItemAdded);
            // 
            // AppIcon
            // 
            this.AppIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AppIcon.Location = new System.Drawing.Point(3, 3);
            this.AppIcon.Name = "AppIcon";
            this.AppIcon.Size = new System.Drawing.Size(32, 30);
            this.AppIcon.TabIndex = 1;
            this.AppIcon.TabStop = false;
            // 
            // RightMenu
            // 
            this.RightMenu.AutoSize = true;
            this.RightMenu.Controls.Add(this.TitleButtonExit);
            this.RightMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightMenu.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.RightMenu.Location = new System.Drawing.Point(244, 3);
            this.RightMenu.Name = "RightMenu";
            this.RightMenu.Size = new System.Drawing.Size(284, 30);
            this.RightMenu.TabIndex = 1;
            this.RightMenu.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.RightMenu_ControlAdded);
            // 
            // TitleButtonExit
            // 
            this.TitleButtonExit.BackColor = System.Drawing.Color.Transparent;
            this.TitleButtonExit.ForeColor = System.Drawing.Color.Black;
            this.TitleButtonExit.HighlightBackColor = System.Drawing.Color.Red;
            this.TitleButtonExit.HighlightForeColor = System.Drawing.Color.White;
            this.TitleButtonExit.Location = new System.Drawing.Point(246, 3);
            this.TitleButtonExit.Name = "TitleButtonExit";
            this.TitleButtonExit.Size = new System.Drawing.Size(35, 27);
            this.TitleButtonExit.TabIndex = 0;
            this.TitleButtonExit.Paint += new System.Windows.Forms.PaintEventHandler(this.TitleButtonExit_Paint);
            // 
            // TitleName
            // 
            this.TitleName.AutoSize = true;
            this.TitleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitleName.Location = new System.Drawing.Point(168, 0);
            this.TitleName.Name = "TitleName";
            this.TitleName.Size = new System.Drawing.Size(70, 36);
            this.TitleName.TabIndex = 3;
            this.TitleName.Text = "AppName";
            this.TitleName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ExForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 450);
            this.Controls.Add(this.FormTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExForm";
            this.Text = "ExForm";
            this.FormTitle.ResumeLayout(false);
            this.FormTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppIcon)).EndInit();
            this.RightMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel RightMenu;
        private System.Windows.Forms.PictureBox AppIcon;
        protected System.Windows.Forms.ToolStrip ToolStripLeft;
        private ExTitleButton TitleButtonExit;
        private System.Windows.Forms.TableLayoutPanel FormTitle;
        private System.Windows.Forms.Label TitleName;
    }
}