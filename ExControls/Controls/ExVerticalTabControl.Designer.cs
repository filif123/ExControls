namespace ExControls
{
    partial class ExVerticalTabControl
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
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tablePanelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.cboxSearch = new ExControls.ExComboBox();
            this.treeView = new ExControls.ExTreeView();
            this.tablePanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.labelTabName = new System.Windows.Forms.Label();
            this.rootPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.tablePanelLeft.SuspendLayout();
            this.tablePanelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.tablePanelLeft);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.tablePanelRight);
            this.mainSplitContainer.Size = new System.Drawing.Size(621, 469);
            this.mainSplitContainer.SplitterDistance = 203;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // tablePanelLeft
            // 
            this.tablePanelLeft.AutoSize = true;
            this.tablePanelLeft.ColumnCount = 1;
            this.tablePanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelLeft.Controls.Add(this.cboxSearch, 0, 0);
            this.tablePanelLeft.Controls.Add(this.treeView, 0, 1);
            this.tablePanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanelLeft.Location = new System.Drawing.Point(0, 0);
            this.tablePanelLeft.Name = "tablePanelLeft";
            this.tablePanelLeft.RowCount = 2;
            this.tablePanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelLeft.Size = new System.Drawing.Size(203, 469);
            this.tablePanelLeft.TabIndex = 0;
            // 
            // cboxSearch
            // 
            this.cboxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboxSearch.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cboxSearch.FormattingEnabled = true;
            this.cboxSearch.Location = new System.Drawing.Point(3, 3);
            this.cboxSearch.Name = "cboxSearch";
            this.cboxSearch.Size = new System.Drawing.Size(197, 21);
            exComboBoxStyle1.ArrowColor = null;
            exComboBoxStyle1.BackColor = null;
            exComboBoxStyle1.BorderColor = null;
            exComboBoxStyle1.ButtonBackColor = null;
            exComboBoxStyle1.ButtonBorderColor = null;
            exComboBoxStyle1.ButtonRenderFirst = null;
            exComboBoxStyle1.ForeColor = null;
            this.cboxSearch.StyleDisabled = exComboBoxStyle1;
            exComboBoxStyle2.ArrowColor = null;
            exComboBoxStyle2.BackColor = null;
            exComboBoxStyle2.BorderColor = null;
            exComboBoxStyle2.ButtonBackColor = null;
            exComboBoxStyle2.ButtonBorderColor = null;
            exComboBoxStyle2.ButtonRenderFirst = null;
            exComboBoxStyle2.ForeColor = null;
            this.cboxSearch.StyleHighlight = exComboBoxStyle2;
            exComboBoxStyle3.ArrowColor = null;
            exComboBoxStyle3.BackColor = null;
            exComboBoxStyle3.BorderColor = null;
            exComboBoxStyle3.ButtonBackColor = null;
            exComboBoxStyle3.ButtonBorderColor = null;
            exComboBoxStyle3.ButtonRenderFirst = null;
            exComboBoxStyle3.ForeColor = null;
            this.cboxSearch.StyleNormal = exComboBoxStyle3;
            exComboBoxStyle4.ArrowColor = null;
            exComboBoxStyle4.BackColor = null;
            exComboBoxStyle4.BorderColor = null;
            exComboBoxStyle4.ButtonBackColor = null;
            exComboBoxStyle4.ButtonBorderColor = null;
            exComboBoxStyle4.ButtonRenderFirst = null;
            exComboBoxStyle4.ForeColor = null;
            this.cboxSearch.StyleSelected = exComboBoxStyle4;
            this.cboxSearch.TabIndex = 1;
            this.cboxSearch.UseDarkScrollBar = false;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(3, 30);
            this.treeView.Name = "treeView";
            this.treeView.ShowLines = false;
            this.treeView.Size = new System.Drawing.Size(197, 436);
            this.treeView.TabIndex = 2;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            // 
            // tablePanelRight
            // 
            this.tablePanelRight.AutoSize = true;
            this.tablePanelRight.ColumnCount = 2;
            this.tablePanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePanelRight.Controls.Add(this.toolStripMenu, 1, 0);
            this.tablePanelRight.Controls.Add(this.labelTabName, 0, 0);
            this.tablePanelRight.Controls.Add(this.rootPanel, 0, 1);
            this.tablePanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanelRight.Location = new System.Drawing.Point(0, 0);
            this.tablePanelRight.Name = "tablePanelRight";
            this.tablePanelRight.RowCount = 2;
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tablePanelRight.Size = new System.Drawing.Size(414, 469);
            this.tablePanelRight.TabIndex = 0;
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.BackColor = System.Drawing.Color.Transparent;
            this.toolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMenu.Location = new System.Drawing.Point(207, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMenu.Size = new System.Drawing.Size(207, 25);
            this.toolStripMenu.TabIndex = 0;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // labelTabName
            // 
            this.labelTabName.AutoSize = true;
            this.labelTabName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTabName.Location = new System.Drawing.Point(3, 0);
            this.labelTabName.Name = "labelTabName";
            this.labelTabName.Size = new System.Drawing.Size(201, 25);
            this.labelTabName.TabIndex = 1;
            this.labelTabName.Text = "label";
            this.labelTabName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rootPanel
            // 
            this.tablePanelRight.SetColumnSpan(this.rootPanel, 2);
            this.rootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootPanel.Location = new System.Drawing.Point(3, 28);
            this.rootPanel.Name = "rootPanel";
            this.rootPanel.Size = new System.Drawing.Size(408, 438);
            this.rootPanel.TabIndex = 2;
            this.rootPanel.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.rootPanel_ControlAdded);
            this.rootPanel.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.rootPanel_ControlRemoved);
            // 
            // ExVerticalTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainSplitContainer);
            this.Name = "ExVerticalTabControl";
            this.Size = new System.Drawing.Size(621, 469);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel1.PerformLayout();
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            this.mainSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.tablePanelLeft.ResumeLayout(false);
            this.tablePanelRight.ResumeLayout(false);
            this.tablePanelRight.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer mainSplitContainer;
        private TableLayoutPanel tablePanelLeft;
        private ExComboBox cboxSearch;
        private TableLayoutPanel tablePanelRight;
        private Label labelTabName;
        private ExTreeView treeView;
        internal Panel rootPanel;
        private ToolStrip toolStripMenu;
    }
}
