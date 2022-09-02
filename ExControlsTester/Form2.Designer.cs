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
            this.exButton1 = new ExControls.ExButton();
            this.SuspendLayout();
            // 
            // exButton1
            // 
            this.exButton1.Location = new System.Drawing.Point(252, 163);
            this.exButton1.Name = "exButton1";
            this.exButton1.Size = new System.Drawing.Size(75, 23);
            this.exButton1.TabIndex = 0;
            this.exButton1.Text = "exButton1";
            this.exButton1.UseVisualStyleBackColor = true;
            this.exButton1.Click += new System.EventHandler(this.ExButton1_Click);
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(714, 399);
            this.Controls.Add(this.exButton1);
            this.CornersType = ExControls.FormCornersType.SmallRound;
            this.Name = "Form2";
            this.Text = "hhh";
            this.TitleBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.TitleBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TitleBarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ExButton exButton1;
    }
}