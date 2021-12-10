namespace ExControls.Test
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.exTextBox1 = new ExControls.ExTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.exGroupBox1 = new ExControls.ExGroupBox();
            this.menuPanel2 = new ExControls.Controls.MenuPanel();
            this.menuPanel1 = new ExControls.Controls.MenuPanel();
            this.exVerticalMenu1 = new ExControls.Controls.ExVerticalMenu();
            this.menuPanel3 = new ExControls.Controls.MenuPanel();
            this.SuspendLayout();
            // 
            // exTextBox1
            // 
            this.exTextBox1.BorderColor = System.Drawing.Color.DimGray;
            this.exTextBox1.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.exTextBox1.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.exTextBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exTextBox1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exTextBox1.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.exTextBox1.HintText = null;
            this.exTextBox1.Location = new System.Drawing.Point(73, 66);
            this.exTextBox1.Name = "exTextBox1";
            this.exTextBox1.Size = new System.Drawing.Size(100, 22);
            this.exTextBox1.TabIndex = 0;
            this.exTextBox1.Text = "exTextBox1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 33);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // exGroupBox1
            // 
            this.exGroupBox1.Location = new System.Drawing.Point(136, 154);
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.Size = new System.Drawing.Size(200, 100);
            this.exGroupBox1.TabIndex = 0;
            this.exGroupBox1.TabStop = false;
            this.exGroupBox1.Text = "exGroupBox1";
            // 
            // menuPanel2
            // 
            this.menuPanel2.Location = new System.Drawing.Point(0, 0);
            this.menuPanel2.Name = "menuPanel2";
            this.menuPanel2.Size = new System.Drawing.Size(200, 100);
            this.menuPanel2.TabIndex = 1;
            this.menuPanel2.Text = "menuPanel2";
            // 
            // menuPanel1
            // 
            this.menuPanel1.Location = new System.Drawing.Point(0, 0);
            this.menuPanel1.Name = "menuPanel1";
            this.menuPanel1.Size = new System.Drawing.Size(200, 100);
            this.menuPanel1.TabIndex = 1;
            this.menuPanel1.Text = "menuPanel1";
            // 
            // exVerticalMenu1
            // 
            this.exVerticalMenu1.Location = new System.Drawing.Point(50, 21);
            this.exVerticalMenu1.Name = "exVerticalMenu1";
            this.exVerticalMenu1.SelectedPage = this.menuPanel3;
            this.exVerticalMenu1.Size = new System.Drawing.Size(677, 443);
            this.exVerticalMenu1.TabIndex = 0;
            // 
            // menuPanel3
            // 
            this.menuPanel3.Location = new System.Drawing.Point(0, 0);
            this.menuPanel3.Name = "menuPanel3";
            this.menuPanel3.Size = new System.Drawing.Size(200, 100);
            this.menuPanel3.TabIndex = 1;
            this.menuPanel3.Text = "menuPanel3";
            exVerticalMenu1.RootPanel.Controls.Add(menuPanel3);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 505);
            this.Controls.Add(this.exVerticalMenu1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private ExTextBox exTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private ExGroupBox exGroupBox1;
        private Controls.MenuPanel menuPanel1;
        private Controls.MenuPanel menuPanel2;
        private Controls.ExVerticalMenu exVerticalMenu1;
        private Controls.MenuPanel menuPanel3;
    }
}