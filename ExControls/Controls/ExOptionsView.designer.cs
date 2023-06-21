using System.Diagnostics;

namespace ExControls
{
    public partial class ExOptionsView 
    {

        /// <summary>
        /// UserControl overrides dispose to clean up the component list.
        /// </summary>
        /// <param name="disposing"></param>
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
#pragma warning disable CS0649 // Field 'ExOptionsView.components' is never assigned to, and will always have its default value null
        private System.ComponentModel.IContainer components;
#pragma warning restore CS0649

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.split = new System.Windows.Forms.SplitContainer();
            this.tablePanelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new ExControls.ExOptionsView.ExOptionsTreeView();
            this.tbSearch = new ExControls.ExTextBox();
            this.tablePanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.panelsContainer = new ExControls.OptionsPanelContainer();
            this.labelPanelName = new System.Windows.Forms.Label();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tablePanelLeft.SuspendLayout();
            this.tablePanelRight.SuspendLayout();
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
            this.split.Size = new System.Drawing.Size(615, 476);
            this.split.SplitterDistance = 202;
            this.split.TabIndex = 0;
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
            this.tablePanelLeft.Size = new System.Drawing.Size(202, 476);
            this.tablePanelLeft.TabIndex = 2;
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
            this.tbSearch.Size = new System.Drawing.Size(196, 20);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.TextChanged += new System.EventHandler(this.TbSearch_TextChanged);
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
            this.tablePanelRight.Margin = new System.Windows.Forms.Padding(2);
            this.tablePanelRight.Name = "tablePanelRight";
            this.tablePanelRight.RowCount = 2;
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelRight.Size = new System.Drawing.Size(409, 476);
            this.tablePanelRight.TabIndex = 1;
            // 
            // panelsContainer
            // 
            this.tablePanelRight.SetColumnSpan(this.panelsContainer, 2);
            this.panelsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelsContainer.Location = new System.Drawing.Point(3, 28);
            this.panelsContainer.Name = "panelsContainer";
            this.panelsContainer.Size = new System.Drawing.Size(403, 445);
            this.panelsContainer.TabIndex = 0;
            // 
            // labelPanelName
            // 
            this.labelPanelName.AutoSize = true;
            this.labelPanelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPanelName.Location = new System.Drawing.Point(2, 0);
            this.labelPanelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPanelName.Name = "labelPanelName";
            this.labelPanelName.Size = new System.Drawing.Size(303, 25);
            this.labelPanelName.TabIndex = 0;
            this.labelPanelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMenu.Location = new System.Drawing.Point(307, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMenu.Size = new System.Drawing.Size(102, 25);
            this.toolStripMenu.TabIndex = 1;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // ExOptionsView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.split);
            this.Name = "ExOptionsView";
            this.Size = new System.Drawing.Size(615, 476);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel1.PerformLayout();
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tablePanelLeft.ResumeLayout(false);
            this.tablePanelLeft.PerformLayout();
            this.tablePanelRight.ResumeLayout(false);
            this.tablePanelRight.PerformLayout();
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