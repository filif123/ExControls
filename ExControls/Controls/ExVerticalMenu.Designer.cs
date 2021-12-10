namespace ExControls.Controls
{
    partial class ExVerticalMenu
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
            ExControls.ExComboBoxStyle exComboBoxStyle1 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle2 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle3 = new ExControls.ExComboBoxStyle();
            ExControls.ExComboBoxStyle exComboBoxStyle4 = new ExControls.ExComboBoxStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ComboBoxFind = new ExControls.ExComboBox();
            this.TreeMenu = new ExControls.Controls.ExTreeView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.RootPanel = new System.Windows.Forms.Panel();
            this.LabelPath = new System.Windows.Forms.Label();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(677, 443);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ComboBoxFind, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TreeMenu, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(225, 443);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ComboBoxFind
            // 
            this.ComboBoxFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboBoxFind.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.ComboBoxFind.FormattingEnabled = true;
            this.ComboBoxFind.Location = new System.Drawing.Point(7, 7);
            this.ComboBoxFind.Margin = new System.Windows.Forms.Padding(7);
            this.ComboBoxFind.Name = "ComboBoxFind";
            this.ComboBoxFind.Size = new System.Drawing.Size(211, 24);
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            this.ComboBoxFind.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            this.ComboBoxFind.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            this.ComboBoxFind.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            this.ComboBoxFind.StyleSelected = exComboBoxStyle4;
            this.ComboBoxFind.TabIndex = 0;
            this.ComboBoxFind.UseDarkScrollBar = false;
            // 
            // TreeMenu
            // 
            this.TreeMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeMenu.Location = new System.Drawing.Point(7, 45);
            this.TreeMenu.Margin = new System.Windows.Forms.Padding(7);
            this.TreeMenu.Name = "TreeMenu";
            this.TreeMenu.ShowRootLines = false;
            this.TreeMenu.Size = new System.Drawing.Size(211, 391);
            this.TreeMenu.TabIndex = 1;
            this.TreeMenu.TreeNodeAdded += new System.EventHandler<ExControls.Controls.ExTreeViewNodeAddedEventArgs>(this.TreeMenu_TreeNodeAdded);
            this.TreeMenu.TreeNodeRemoved += new System.EventHandler<ExControls.Controls.ExTreeViewNodeRemovedEventArgs>(this.TreeMenu_TreeNodeRemoved);
            this.TreeMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeMenu_AfterSelect);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.RootPanel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.LabelPath, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ToolBar, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(448, 443);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // RootPanel
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.RootPanel, 2);
            this.RootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootPanel.Location = new System.Drawing.Point(3, 39);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.Size = new System.Drawing.Size(442, 401);
            this.RootPanel.TabIndex = 0;
            // 
            // LabelPath
            // 
            this.LabelPath.AutoSize = true;
            this.LabelPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelPath.Location = new System.Drawing.Point(3, 0);
            this.LabelPath.Name = "LabelPath";
            this.LabelPath.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.LabelPath.Size = new System.Drawing.Size(218, 36);
            this.LabelPath.TabIndex = 1;
            this.LabelPath.Text = "label1";
            // 
            // ToolBar
            // 
            this.ToolBar.BackColor = System.Drawing.SystemColors.Control;
            this.ToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolBar.Location = new System.Drawing.Point(224, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolBar.Size = new System.Drawing.Size(224, 36);
            this.ToolBar.TabIndex = 2;
            this.ToolBar.Text = "toolStrip1";
            // 
            // ExVerticalMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ExVerticalMenu";
            this.Size = new System.Drawing.Size(677, 443);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel1;
        private ExComboBox ComboBoxFind;
        public ExTreeView TreeMenu;
        private TableLayoutPanel tableLayoutPanel2;
        private Label LabelPath;
        public ToolStrip ToolBar;
        public Panel RootPanel;
    }
}
