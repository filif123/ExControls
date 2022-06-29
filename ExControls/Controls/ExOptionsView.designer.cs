using System.Diagnostics;

namespace ExControls
{
    public partial class ExOptionsView 
    {

        // UserControl overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.split = new System.Windows.Forms.SplitContainer();
            this.tablePanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.panelsContainer = new ExControls.OptionsPanelContainer();
            this.labelPanelName = new System.Windows.Forms.Label();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.treeView = new ExControls.ExOptionsView.ExOptionsTreeView();
            this.tablePanelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.tbSearch = new ExControls.ExTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tablePanelRight.SuspendLayout();
            this.tablePanelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.tablePanelLeft);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.tablePanelRight);
            this.split.Size = new System.Drawing.Size(492, 381);
            this.split.SplitterDistance = 162;
            this.split.TabIndex = 0;
            // 
            // tablePanelRight
            // 
            this.tablePanelRight.ColumnCount = 2;
            this.tablePanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanelRight.Controls.Add(this.panelsContainer, 0, 1);
            this.tablePanelRight.Controls.Add(this.labelPanelName, 0, 0);
            this.tablePanelRight.Controls.Add(this.toolStripMenu, 1, 0);
            this.tablePanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanelRight.Location = new System.Drawing.Point(0, 0);
            this.tablePanelRight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tablePanelRight.Name = "tablePanelRight";
            this.tablePanelRight.RowCount = 2;
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelRight.Size = new System.Drawing.Size(326, 381);
            this.tablePanelRight.TabIndex = 1;
            // 
            // panelsContainer
            // 
            this.tablePanelRight.SetColumnSpan(this.panelsContainer, 2);
            this.panelsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelsContainer.Location = new System.Drawing.Point(3, 28);
            this.panelsContainer.Name = "panelsContainer";
            this.panelsContainer.Size = new System.Drawing.Size(320, 350);
            this.panelsContainer.TabIndex = 0;
            // 
            // labelPanelName
            // 
            this.labelPanelName.AutoSize = true;
            this.labelPanelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPanelName.Location = new System.Drawing.Point(2, 0);
            this.labelPanelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPanelName.Name = "labelPanelName";
            this.labelPanelName.Size = new System.Drawing.Size(220, 25);
            this.labelPanelName.TabIndex = 0;
            this.labelPanelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMenu.Location = new System.Drawing.Point(224, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMenu.Size = new System.Drawing.Size(102, 25);
            this.toolStripMenu.TabIndex = 1;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.FullRowSelect = true;
            this.treeView.HideSelection = false;
            this.treeView.Name = "treeView";
            this.treeView.PathSeparator = " / ";
            this.treeView.ShowLines = false;
            this.treeView.ShowNodeToolTips = true;
            this.treeView.TabIndex = 0;
            // 
            // tablePanelLeft
            // 
            this.tablePanelLeft.AutoSize = true;
            this.tablePanelLeft.ColumnCount = 1;
            this.tablePanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelLeft.Controls.Add(this.treeView, 0, 1);
            this.tablePanelLeft.Controls.Add(this.tbSearch, 0, 0);
            this.tablePanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanelLeft.Location = new System.Drawing.Point(0, 0);
            this.tablePanelLeft.Margin = new System.Windows.Forms.Padding(2);
            this.tablePanelLeft.Name = "tablePanelLeft";
            this.tablePanelLeft.RowCount = 2;
            this.tablePanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelLeft.Size = new System.Drawing.Size(162, 381);
            this.tablePanelLeft.TabIndex = 2;
            // 
            // tbSearch
            // 
            this.tbSearch.BorderColor = System.Drawing.Color.DimGray;
            this.tbSearch.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbSearch.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbSearch.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSearch.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbSearch.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbSearch.HintText = "Search";
            this.tbSearch.Location = new System.Drawing.Point(3, 3);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(156, 20);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.TextChanged += new System.EventHandler(this.TbSearch_TextChanged);
            // 
            // ExOptionsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.split);
            this.Name = "ExOptionsView";
            this.Size = new System.Drawing.Size(615, 476);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel1.PerformLayout();
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tablePanelRight.ResumeLayout(false);
            this.tablePanelRight.PerformLayout();
            this.tablePanelLeft.ResumeLayout(false);
            this.tablePanelLeft.PerformLayout();
            this.ResumeLayout(false);

        }

        internal SplitContainer split;
        internal OptionsPanelContainer panelsContainer;
        private TableLayoutPanel tablePanelRight;
        internal Label labelPanelName;
        private ToolStrip toolStripMenu;
        private TableLayoutPanel tablePanelLeft;
        internal ExOptionsTreeView treeView;
        private ExTextBox tbSearch;
    }
}