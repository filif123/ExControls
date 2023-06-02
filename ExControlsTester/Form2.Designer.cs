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
            this.exDateTimePicker1 = new ExControls.ExDateTimePicker();
            this.exTextBox1 = new ExControls.ExTextBox();
            this.exListBox1 = new ExControls.ExListBox();
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
            // exDateTimePicker1
            // 
            this.exDateTimePicker1.BackColor = System.Drawing.Color.Salmon;
            this.exDateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exDateTimePicker1.DisabledBackColor = System.Drawing.Color.Empty;
            this.exDateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exDateTimePicker1.ForeColor = System.Drawing.Color.Silver;
            this.exDateTimePicker1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exDateTimePicker1.Location = new System.Drawing.Point(126, 95);
            this.exDateTimePicker1.Name = "exDateTimePicker1";
            this.exDateTimePicker1.Size = new System.Drawing.Size(190, 20);
            this.exDateTimePicker1.TabIndex = 1;
            this.exDateTimePicker1.MouseCaptureChanged += new System.EventHandler(this.ExDateTimePicker1_MouseCaptureChanged);
            // 
            // exTextBox1
            // 
            this.exTextBox1.BackColor = System.Drawing.SystemColors.GrayText;
            this.exTextBox1.BorderColor = System.Drawing.Color.SandyBrown;
            this.exTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exTextBox1.DefaultStyle = false;
            this.exTextBox1.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.exTextBox1.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.exTextBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exTextBox1.HighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.exTextBox1.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.exTextBox1.HintText = null;
            this.exTextBox1.Location = new System.Drawing.Point(417, 94);
            this.exTextBox1.Name = "exTextBox1";
            this.exTextBox1.Size = new System.Drawing.Size(195, 20);
            this.exTextBox1.TabIndex = 2;
            this.exTextBox1.Text = "exTextBox1";
            // 
            // exListBox1
            // 
            this.exListBox1.BackColor = System.Drawing.SystemColors.GrayText;
            this.exListBox1.BorderColor = System.Drawing.Color.IndianRed;
            this.exListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exListBox1.DefaultStyle = false;
            this.exListBox1.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.exListBox1.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.exListBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.exListBox1.FormattingEnabled = true;
            this.exListBox1.Items.AddRange(new object[] {
            "ghgfhghf",
            "fhgfhg",
            "fcg",
            "gf",
            "gfcdgffg"});
            this.exListBox1.Location = new System.Drawing.Point(391, 184);
            this.exListBox1.Name = "exListBox1";
            this.exListBox1.SelectedRowBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.exListBox1.Size = new System.Drawing.Size(120, 93);
            this.exListBox1.TabIndex = 3;
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(714, 399);
            this.Controls.Add(this.exListBox1);
            this.Controls.Add(this.exTextBox1);
            this.Controls.Add(this.exDateTimePicker1);
            this.Controls.Add(this.exButton1);
            this.CornersType = ExControls.FormCornersType.SmallRound;
            this.FormStyle = ExControls.FormStyle.Mica;
            this.Name = "Form2";
            this.Text = "hhh";
            this.TitleBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.TitleBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TitleBarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExButton exButton1;
        private ExDateTimePicker exDateTimePicker1;
        private ExTextBox exTextBox1;
        private ExListBox exListBox1;
    }
}