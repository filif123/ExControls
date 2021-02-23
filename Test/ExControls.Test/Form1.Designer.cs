
namespace ExControls.Test
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ExComboBoxStyle exComboBoxStyle53 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle54 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle55 = new ExComboBoxStyle();
            ExComboBoxStyle exComboBoxStyle56 = new ExComboBoxStyle();
            this.exComboBox1 = new ExComboBox(this.components);
            this.exDateTimePicker1 = new ExDateTimePicker();
            this.exGroupBox1 = new ExGroupBox(this.components);
            this.exTabControl1 = new ExTabControl(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.exLineSeparator1 = new ExLineSeparator();
            this.exTextBox1 = new ExTextBox(this.components);
            this.exCheckBox1 = new ExCheckBox(this.components);
            this.exRadioButton1 = new ExRadioButton(this.components);
            this.exRadioButton2 = new ExRadioButton(this.components);
            this.exNumericUpDown1 = new ExNumericUpDown(this.components);
            this.exGroupBox1.SuspendLayout();
            this.exTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exNumericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // exComboBox1
            // 
            this.exComboBox1.DefaultStyle = false;
            this.exComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.exComboBox1.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.exComboBox1.FormattingEnabled = true;
            this.exComboBox1.Location = new System.Drawing.Point(6, 21);
            this.exComboBox1.Name = "exComboBox1";
            this.exComboBox1.Size = new System.Drawing.Size(121, 23);
            exComboBoxStyle53.ArrowColor = null;
            exComboBoxStyle53.BackColor = null;
            exComboBoxStyle53.BorderColor = null;
            exComboBoxStyle53.ButtonBackColor = null;
            exComboBoxStyle53.ButtonBorderColor = null;
            exComboBoxStyle53.ButtonRenderFirst = null;
            exComboBoxStyle53.ForeColor = null;
            this.exComboBox1.StyleDisabled = exComboBoxStyle53;
            exComboBoxStyle54.ArrowColor = null;
            exComboBoxStyle54.BackColor = null;
            exComboBoxStyle54.BorderColor = null;
            exComboBoxStyle54.ButtonBackColor = null;
            exComboBoxStyle54.ButtonBorderColor = null;
            exComboBoxStyle54.ButtonRenderFirst = null;
            exComboBoxStyle54.ForeColor = null;
            this.exComboBox1.StyleHighlight = exComboBoxStyle54;
            exComboBoxStyle55.ButtonBorderColor = System.Drawing.Color.White;
            exComboBoxStyle55.ButtonRenderFirst = true;
            this.exComboBox1.StyleNormal = exComboBoxStyle55;
            exComboBoxStyle56.ArrowColor = null;
            exComboBoxStyle56.BackColor = null;
            exComboBoxStyle56.BorderColor = null;
            exComboBoxStyle56.ButtonBackColor = null;
            exComboBoxStyle56.ButtonBorderColor = null;
            exComboBoxStyle56.ButtonRenderFirst = null;
            exComboBoxStyle56.ForeColor = null;
            this.exComboBox1.StyleSelected = exComboBoxStyle56;
            this.exComboBox1.TabIndex = 0;
            // 
            // exDateTimePicker1
            // 
            this.exDateTimePicker1.DefaultStyle = false;
            this.exDateTimePicker1.DisabledBackColor = System.Drawing.Color.Empty;
            this.exDateTimePicker1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exDateTimePicker1.Location = new System.Drawing.Point(6, 51);
            this.exDateTimePicker1.Name = "exDateTimePicker1";
            this.exDateTimePicker1.Size = new System.Drawing.Size(206, 22);
            this.exDateTimePicker1.TabIndex = 1;
            // 
            // exGroupBox1
            // 
            this.exGroupBox1.Controls.Add(this.exComboBox1);
            this.exGroupBox1.Controls.Add(this.exDateTimePicker1);
            this.exGroupBox1.DefaultStyle = false;
            this.exGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.exGroupBox1.Name = "exGroupBox1";
            this.exGroupBox1.Size = new System.Drawing.Size(227, 100);
            this.exGroupBox1.TabIndex = 2;
            this.exGroupBox1.TabStop = false;
            this.exGroupBox1.Text = "exGroupBox1";
            // 
            // exTabControl1
            // 
            this.exTabControl1.Controls.Add(this.tabPage1);
            this.exTabControl1.Controls.Add(this.tabPage2);
            this.exTabControl1.DefaultStyle = false;
            this.exTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.exTabControl1.Location = new System.Drawing.Point(245, 12);
            this.exTabControl1.Name = "exTabControl1";
            this.exTabControl1.SelectedIndex = 0;
            this.exTabControl1.Size = new System.Drawing.Size(324, 150);
            this.exTabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(316, 121);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(316, 121);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // exLineSeparator1
            // 
            this.exLineSeparator1.Location = new System.Drawing.Point(-3, 406);
            this.exLineSeparator1.Name = "exLineSeparator1";
            this.exLineSeparator1.Size = new System.Drawing.Size(802, 13);
            this.exLineSeparator1.TabIndex = 0;
            this.exLineSeparator1.Text = null;
            // 
            // exTextBox1
            // 
            this.exTextBox1.BorderColor = System.Drawing.Color.DimGray;
            this.exTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exTextBox1.DefaultStyle = false;
            this.exTextBox1.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.exTextBox1.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.exTextBox1.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.exTextBox1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exTextBox1.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.exTextBox1.HintText = "blahBlah";
            this.exTextBox1.Location = new System.Drawing.Point(12, 119);
            this.exTextBox1.Name = "exTextBox1";
            this.exTextBox1.Size = new System.Drawing.Size(227, 22);
            this.exTextBox1.TabIndex = 4;
            // 
            // exCheckBox1
            // 
            this.exCheckBox1.AutoSize = true;
            this.exCheckBox1.BoxBackColor = System.Drawing.Color.White;
            this.exCheckBox1.DefaultStyle = false;
            this.exCheckBox1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exCheckBox1.Location = new System.Drawing.Point(12, 148);
            this.exCheckBox1.Name = "exCheckBox1";
            this.exCheckBox1.Size = new System.Drawing.Size(77, 21);
            this.exCheckBox1.TabIndex = 5;
            this.exCheckBox1.Text = "Testing";
            this.exCheckBox1.UseVisualStyleBackColor = true;
            // 
            // exRadioButton1
            // 
            this.exRadioButton1.AutoSize = true;
            this.exRadioButton1.DefaultStyle = false;
            this.exRadioButton1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exRadioButton1.Location = new System.Drawing.Point(12, 176);
            this.exRadioButton1.Name = "exRadioButton1";
            this.exRadioButton1.Size = new System.Drawing.Size(129, 21);
            this.exRadioButton1.TabIndex = 6;
            this.exRadioButton1.TabStop = true;
            this.exRadioButton1.Text = "exRadioButton1";
            this.exRadioButton1.UseVisualStyleBackColor = true;
            // 
            // exRadioButton2
            // 
            this.exRadioButton2.AutoSize = true;
            this.exRadioButton2.DefaultStyle = false;
            this.exRadioButton2.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exRadioButton2.Location = new System.Drawing.Point(148, 176);
            this.exRadioButton2.Name = "exRadioButton2";
            this.exRadioButton2.Size = new System.Drawing.Size(129, 21);
            this.exRadioButton2.TabIndex = 7;
            this.exRadioButton2.TabStop = true;
            this.exRadioButton2.Text = "exRadioButton2";
            this.exRadioButton2.UseVisualStyleBackColor = true;
            // 
            // exNumericUpDown1
            // 
            this.exNumericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exNumericUpDown1.DefaultStyle = false;
            this.exNumericUpDown1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exNumericUpDown1.Location = new System.Drawing.Point(335, 176);
            this.exNumericUpDown1.Name = "exNumericUpDown1";
            this.exNumericUpDown1.SelectedButtonColor = System.Drawing.SystemColors.Highlight;
            this.exNumericUpDown1.Size = new System.Drawing.Size(126, 22);
            this.exNumericUpDown1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.exNumericUpDown1);
            this.Controls.Add(this.exRadioButton2);
            this.Controls.Add(this.exRadioButton1);
            this.Controls.Add(this.exCheckBox1);
            this.Controls.Add(this.exTextBox1);
            this.Controls.Add(this.exLineSeparator1);
            this.Controls.Add(this.exTabControl1);
            this.Controls.Add(this.exGroupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.exGroupBox1.ResumeLayout(false);
            this.exTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.exNumericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExComboBox exComboBox1;
        private ExDateTimePicker exDateTimePicker1;
        private ExGroupBox exGroupBox1;
        private ExTabControl exTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ExLineSeparator exLineSeparator1;
        private ExTextBox exTextBox1;
        private ExCheckBox exCheckBox1;
        private ExRadioButton exRadioButton1;
        private ExRadioButton exRadioButton2;
        private ExNumericUpDown exNumericUpDown1;
    }
}

