namespace ExControls.Test
{
    partial class TitleBarForm
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
            titleBar = new ExTitleBar();
            menuStrip = new MenuStrip();
            miFile = new ToolStripMenuItem();
            miExit = new ToolStripMenuItem();
            miWindow = new ToolStripMenuItem();
            miMinimize = new ToolStripMenuItem();
            miMaximize = new ToolStripMenuItem();
            miToggleTitleBar = new ToolStripMenuItem();
            miDarkTheme = new ToolStripMenuItem();
            miLongTitle = new ToolStripMenuItem();
            txtSearch = new ExTextBox();
            txtInfo = new TextBox();
            lblState = new Label();
            btnToggle = new Button();
            titleBar.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // titleBar
            // 
            titleBar.Controls.Add(menuStrip);
            titleBar.Controls.Add(txtSearch);
            titleBar.Location = new Point(0, 0);
            titleBar.Name = "titleBar";
            titleBar.Padding = new Padding(0, 5, 8, 5);
            titleBar.Size = new Size(784, 36);
            titleBar.TabIndex = 0;
            // 
            // menuStrip
            // 
            menuStrip.Dock = DockStyle.Left;
            menuStrip.Items.AddRange(new ToolStripItem[] { miFile, miWindow });
            menuStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            menuStrip.Location = new Point(36, 5);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(0);
            menuStrip.Size = new Size(100, 26);
            menuStrip.TabIndex = 0;
            // 
            // miFile
            // 
            miFile.DropDownItems.AddRange(new ToolStripItem[] { miExit });
            miFile.Name = "miFile";
            miFile.Size = new Size(50, 26);
            miFile.Text = "&Súbor";
            miFile.Click += miFile_Click;
            // 
            // miExit
            // 
            miExit.Name = "miExit";
            miExit.Size = new Size(110, 22);
            miExit.Text = "&Koniec";
            miExit.Click += MiExit_Click;
            // 
            // miWindow
            // 
            miWindow.DropDownItems.AddRange(new ToolStripItem[] { miMinimize, miMaximize, miToggleTitleBar, miDarkTheme, miLongTitle });
            miWindow.Name = "miWindow";
            miWindow.Size = new Size(48, 26);
            miWindow.Text = "&Okno";
            // 
            // miMinimize
            // 
            miMinimize.Name = "miMinimize";
            miMinimize.Size = new Size(204, 22);
            miMinimize.Text = "Mi&nimalizovať";
            miMinimize.Click += MiMinimize_Click;
            // 
            // miMaximize
            // 
            miMaximize.Name = "miMaximize";
            miMaximize.Size = new Size(204, 22);
            miMaximize.Text = "Ma&ximalizovať / obnoviť";
            miMaximize.Click += MiMaximize_Click;
            // 
            // miToggleTitleBar
            // 
            miToggleTitleBar.Name = "miToggleTitleBar";
            miToggleTitleBar.Size = new Size(204, 22);
            miToggleTitleBar.Text = "&Systémové záhlavie";
            miToggleTitleBar.Click += BtnToggle_Click;
            // 
            // miDarkTheme
            // 
            miDarkTheme.Checked = true;
            miDarkTheme.CheckOnClick = true;
            miDarkTheme.CheckState = CheckState.Checked;
            miDarkTheme.Name = "miDarkTheme";
            miDarkTheme.Size = new Size(204, 22);
            miDarkTheme.Text = "&Tmavá téma";
            miDarkTheme.Click += MiDarkTheme_Click;
            // 
            // miLongTitle
            // 
            miLongTitle.CheckOnClick = true;
            miLongTitle.Name = "miLongTitle";
            miLongTitle.Size = new Size(204, 22);
            miLongTitle.Text = "&Dlhý text okna";
            miLongTitle.Click += MiLongTitle_Click;
            // 
            // txtSearch
            // 
            txtSearch.BorderColor = Color.DimGray;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.DefaultStyle = false;
            txtSearch.DisabledBackColor = SystemColors.Control;
            txtSearch.DisabledBorderColor = SystemColors.InactiveBorder;
            txtSearch.DisabledForeColor = SystemColors.GrayText;
            txtSearch.Dock = DockStyle.Right;
            txtSearch.HighlightColor = SystemColors.Highlight;
            txtSearch.HintForeColor = Color.Gray;
            txtSearch.HintText = "Hľadať (prvok v záhlaví)";
            txtSearch.Location = new Point(478, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(160, 23);
            txtSearch.TabIndex = 1;
            // 
            // txtInfo
            // 
            txtInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtInfo.Location = new Point(12, 34);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.ReadOnly = true;
            txtInfo.ScrollBars = ScrollBars.Vertical;
            txtInfo.Size = new Size(760, 342);
            txtInfo.TabIndex = 1;
            // 
            // lblState
            // 
            lblState.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblState.Location = new Point(12, 388);
            lblState.Name = "lblState";
            lblState.Size = new Size(560, 52);
            lblState.TabIndex = 2;
            // 
            // btnToggle
            // 
            btnToggle.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnToggle.Location = new Point(582, 388);
            btnToggle.Name = "btnToggle";
            btnToggle.Size = new Size(190, 27);
            btnToggle.TabIndex = 3;
            btnToggle.Text = "Prepnúť záhlavie";
            btnToggle.UseVisualStyleBackColor = true;
            btnToggle.Click += BtnToggle_Click;
            // 
            // TitleBarForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 452);
            Controls.Add(btnToggle);
            Controls.Add(lblState);
            Controls.Add(txtInfo);
            Controls.Add(titleBar);
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(400, 300);
            Name = "TitleBarForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ExControls – vlastné záhlavie";
            titleBar.ResumeLayout(false);
            titleBar.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ExTitleBar titleBar;
        private MenuStrip menuStrip;
        private ToolStripMenuItem miFile;
        private ToolStripMenuItem miExit;
        private ToolStripMenuItem miWindow;
        private ToolStripMenuItem miMinimize;
        private ToolStripMenuItem miMaximize;
        private ToolStripMenuItem miToggleTitleBar;
        private ToolStripMenuItem miDarkTheme;
        private ToolStripMenuItem miLongTitle;
        private ExTextBox txtSearch;
        private TextBox txtInfo;
        private Label lblState;
        private Button btnToggle;
    }
}
