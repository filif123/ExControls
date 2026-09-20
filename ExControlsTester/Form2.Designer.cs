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
            exButton1 = new ExButton();
            exTextBox1 = new ExTextBox();
            exListBox1 = new ExListBox();
            undoRedoActionChooser1 = new UndoRedoActionChooser();
            dateTimePicker1 = new DateTimePicker();
            exDateTimePicker1 = new ExDateTimePicker();
            SuspendLayout();
            // 
            // exButton1
            // 
            exButton1.DefaultStyle = true;
            exButton1.Location = new Point(252, 163);
            exButton1.Name = "exButton1";
            exButton1.Size = new Size(75, 23);
            exButton1.TabIndex = 0;
            exButton1.Text = "exButton1";
            exButton1.UseVisualStyleBackColor = true;
            exButton1.Click += ExButton1_Click;
            // 
            // exTextBox1
            // 
            exTextBox1.BackColor = SystemColors.GrayText;
            exTextBox1.BorderColor = Color.SandyBrown;
            exTextBox1.BorderStyle = BorderStyle.FixedSingle;
            exTextBox1.BorderThickness = 1;
            exTextBox1.DefaultStyle = false;
            exTextBox1.DisabledBackColor = SystemColors.Control;
            exTextBox1.DisabledBorderColor = SystemColors.InactiveBorder;
            exTextBox1.DisabledForeColor = SystemColors.GrayText;
            exTextBox1.HighlightColor = Color.FromArgb(192, 255, 192);
            exTextBox1.HintForeColor = SystemColors.GrayText;
            exTextBox1.HintText = null;
            exTextBox1.Location = new Point(417, 94);
            exTextBox1.Name = "exTextBox1";
            exTextBox1.Size = new Size(195, 23);
            exTextBox1.TabIndex = 2;
            exTextBox1.Text = "exTextBox1";
            exTextBox1.UseDarkScrollBar = false;
            // 
            // exListBox1
            // 
            exListBox1.BackColor = SystemColors.GrayText;
            exListBox1.BorderColor = Color.IndianRed;
            exListBox1.BorderStyle = BorderStyle.FixedSingle;
            exListBox1.BorderThickness = 1;
            exListBox1.DefaultStyle = false;
            exListBox1.DisabledBackColor = SystemColors.Control;
            exListBox1.DisabledBorderColor = SystemColors.InactiveBorder;
            exListBox1.DisabledForeColor = SystemColors.GrayText;
            exListBox1.DrawMode = DrawMode.OwnerDrawFixed;
            exListBox1.FormattingEnabled = true;
            exListBox1.Items.AddRange(new object[] { "ghgfhghf", "fhgfhg", "fcg", "gf", "gfcdgffg" });
            exListBox1.Location = new Point(391, 184);
            exListBox1.Name = "exListBox1";
            exListBox1.SelectedRowBackColor = SystemColors.GradientActiveCaption;
            exListBox1.Size = new Size(120, 82);
            exListBox1.TabIndex = 3;
            // 
            // undoRedoActionChooser1
            // 
            undoRedoActionChooser1.AutoSize = true;
            undoRedoActionChooser1.Location = new Point(348, 132);
            undoRedoActionChooser1.Margin = new Padding(4, 3, 4, 3);
            undoRedoActionChooser1.Name = "undoRedoActionChooser1";
            undoRedoActionChooser1.Size = new Size(193, 28);
            undoRedoActionChooser1.TabIndex = 4;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(22, 73);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.ShowCheckBox = true;
            dateTimePicker1.Size = new Size(200, 23);
            dateTimePicker1.TabIndex = 5;
            // 
            // exDateTimePicker1
            // 
            exDateTimePicker1.ArrowColor = Color.Black;
            exDateTimePicker1.BorderColor = Color.DimGray;
            exDateTimePicker1.ButtonBackColor = Color.White;
            exDateTimePicker1.Checked = true;
            exDateTimePicker1.CustomFormat = null;
            exDateTimePicker1.DefaultStyle = false;
            exDateTimePicker1.DisabledBackColor = SystemColors.Control;
            exDateTimePicker1.DisabledForeColor = SystemColors.GrayText;
            exDateTimePicker1.DropDownAlign = LeftRightAlignment.Left;
            exDateTimePicker1.Format = DateTimePickerFormat.Long;
            exDateTimePicker1.HighlightColor = SystemColors.Highlight;
            exDateTimePicker1.Location = new Point(22, 102);
            exDateTimePicker1.Name = "exDateTimePicker1";
            exDateTimePicker1.SelectedFieldBackColor = SystemColors.Highlight;
            exDateTimePicker1.SelectedFieldForeColor = SystemColors.HighlightText;
            exDateTimePicker1.ShowCheckBox = true;
            exDateTimePicker1.ShowUpDown = false;
            exDateTimePicker1.Size = new Size(200, 23);
            exDateTimePicker1.TabIndex = 6;
            // 
            // Form2
            // 
            ClientSize = new Size(714, 399);
            Controls.Add(exDateTimePicker1);
            Controls.Add(dateTimePicker1);
            Controls.Add(undoRedoActionChooser1);
            Controls.Add(exListBox1);
            Controls.Add(exTextBox1);
            Controls.Add(exButton1);
            CornersType = FormCornersType.Round;
            FormStyle = FormStyle.Acrylic;
            Name = "Form2";
            Text = "hhh";
            TitleBarBackColor = Color.FromArgb(255, 192, 255);
            TitleBarBorderColor = Color.FromArgb(255, 192, 192);
            TitleBarForeColor = Color.FromArgb(255, 255, 192);
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private ExButton exButton1;
        private ExTextBox exTextBox1;
        private ExListBox exListBox1;
        private UndoRedoActionChooser undoRedoActionChooser1;
        private DateTimePicker dateTimePicker1;
        private ExDateTimePicker exDateTimePicker1;
    }
}