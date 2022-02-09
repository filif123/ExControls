namespace ExControls
{
    partial class TitleBar
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

        #region Kód vygenerovaný pomocí Návrháře komponent

        /// <summary> 
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.TablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonClose = new System.Windows.Forms.Panel();
            this.ButtonMaximize = new System.Windows.Forms.Panel();
            this.ButtonMinimize = new System.Windows.Forms.Panel();
            this.ButtonHelp = new System.Windows.Forms.Panel();
            this.TextLabel = new System.Windows.Forms.Label();
            this.BoxIcon = new System.Windows.Forms.PictureBox();
            this.TablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BoxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // TablePanel
            // 
            this.TablePanel.AutoSize = true;
            this.TablePanel.BackColor = System.Drawing.Color.Transparent;
            this.TablePanel.ColumnCount = 7;
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePanel.Controls.Add(this.ButtonClose, 6, 0);
            this.TablePanel.Controls.Add(this.ButtonMaximize, 5, 0);
            this.TablePanel.Controls.Add(this.ButtonMinimize, 4, 0);
            this.TablePanel.Controls.Add(this.ButtonHelp, 3, 0);
            this.TablePanel.Controls.Add(this.TextLabel, 1, 0);
            this.TablePanel.Controls.Add(this.BoxIcon, 0, 0);
            this.TablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TablePanel.Location = new System.Drawing.Point(0, 0);
            this.TablePanel.Name = "TablePanel";
            this.TablePanel.RowCount = 1;
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TablePanel.Size = new System.Drawing.Size(549, 30);
            this.TablePanel.TabIndex = 0;
            this.TablePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.TablePanel_Paint);
            this.TablePanel.DoubleClick += new System.EventHandler(this.TablePanel_DoubleClick);
            this.TablePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TablePanel_MouseDown);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Location = new System.Drawing.Point(502, 0);
            this.ButtonClose.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(47, 28);
            this.ButtonClose.TabIndex = 1;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            this.ButtonClose.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonClose_Paint);
            this.ButtonClose.MouseEnter += new System.EventHandler(this.ButtonClose_MouseEnter);
            this.ButtonClose.MouseLeave += new System.EventHandler(this.ButtonClose_MouseLeave);
            // 
            // ButtonMaximize
            // 
            this.ButtonMaximize.Location = new System.Drawing.Point(455, 0);
            this.ButtonMaximize.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonMaximize.Name = "ButtonMaximize";
            this.ButtonMaximize.Size = new System.Drawing.Size(47, 28);
            this.ButtonMaximize.TabIndex = 2;
            this.ButtonMaximize.Click += new System.EventHandler(this.ButtonMaximize_Click);
            this.ButtonMaximize.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonMaximize_Paint);
            this.ButtonMaximize.MouseEnter += new System.EventHandler(this.ButtonMaximize_MouseEnter);
            this.ButtonMaximize.MouseLeave += new System.EventHandler(this.ButtonMaximize_MouseLeave);
            // 
            // ButtonMinimize
            // 
            this.ButtonMinimize.Location = new System.Drawing.Point(408, 0);
            this.ButtonMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonMinimize.Name = "ButtonMinimize";
            this.ButtonMinimize.Size = new System.Drawing.Size(47, 28);
            this.ButtonMinimize.TabIndex = 3;
            this.ButtonMinimize.Click += new System.EventHandler(this.ButtonMinimize_Click);
            this.ButtonMinimize.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonMinimize_Paint);
            this.ButtonMinimize.MouseEnter += new System.EventHandler(this.ButtonMinimize_MouseEnter);
            this.ButtonMinimize.MouseLeave += new System.EventHandler(this.ButtonMinimize_MouseLeave);
            // 
            // ButtonHelp
            // 
            this.ButtonHelp.Location = new System.Drawing.Point(361, 0);
            this.ButtonHelp.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonHelp.Name = "ButtonHelp";
            this.ButtonHelp.Size = new System.Drawing.Size(47, 28);
            this.ButtonHelp.TabIndex = 4;
            this.ButtonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            this.ButtonHelp.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonHelp_Paint);
            this.ButtonHelp.MouseEnter += new System.EventHandler(this.ButtonHelp_MouseEnter);
            this.ButtonHelp.MouseLeave += new System.EventHandler(this.ButtonHelp_MouseLeave);
            // 
            // TextLabel
            // 
            this.TextLabel.AutoSize = true;
            this.TextLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.TextLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TextLabel.Location = new System.Drawing.Point(25, 0);
            this.TextLabel.Name = "TextLabel";
            this.TextLabel.Size = new System.Drawing.Size(38, 30);
            this.TextLabel.TabIndex = 6;
            this.TextLabel.Text = "label1";
            this.TextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TextLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextLabel_MouseDown);
            // 
            // BoxIcon
            // 
            this.BoxIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxIcon.Location = new System.Drawing.Point(3, 7);
            this.BoxIcon.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BoxIcon.MinimumSize = new System.Drawing.Size(16, 16);
            this.BoxIcon.Name = "BoxIcon";
            this.BoxIcon.Size = new System.Drawing.Size(16, 16);
            this.BoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BoxIcon.TabIndex = 0;
            this.BoxIcon.TabStop = false;
            this.BoxIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BoxIcon_MouseDown);
            // 
            // TitleBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.TablePanel);
            this.Name = "TitleBar";
            this.Size = new System.Drawing.Size(549, 30);
            this.TablePanel.ResumeLayout(false);
            this.TablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BoxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel TablePanel;
        internal PictureBox BoxIcon;
        internal Panel ButtonClose;
        internal Panel ButtonMaximize;
        internal Panel ButtonMinimize;
        internal Panel ButtonHelp;
        internal Label TextLabel;
    }
}
