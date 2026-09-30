namespace ExControls
{
    partial class ExFormTest : Form
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
            this.CustomPanel = new System.Windows.Forms.Panel();
            this.TitleBar = new ExControls.TitleBar();
            this.SuspendLayout();
            // 
            // CustomPanel
            // 
            this.CustomPanel.BackColor = System.Drawing.Color.Transparent;
            this.CustomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomPanel.Location = new System.Drawing.Point(0, 0);
            this.CustomPanel.Name = "CustomPanel";
            this.CustomPanel.Size = new System.Drawing.Size(581, 24);
            this.CustomPanel.TabIndex = 0;
            // 
            // TitleBar
            // 
            this.TitleBar.BackColor = System.Drawing.Color.White;
            this.TitleBar.DeactivatedBackColor = System.Drawing.Color.White;
            this.TitleBar.DeactivatedForeColor = System.Drawing.Color.DimGray;
            this.TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleBar.Form = null;
            this.TitleBar.Location = new System.Drawing.Point(1, 1);
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.Size = new System.Drawing.Size(841, 30);
            this.TitleBar.TabIndex = 0;
            // 
            // ExFormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 381);
            this.Controls.Add(this.TitleBar);
            this.Name = "ExFormTest";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Text = "ExFormTest";
            this.ResumeLayout(false);

        }

        #endregion

        [Browsable(false)] 
        internal TitleBar TitleBar;
        public Panel CustomPanel;
    }
}