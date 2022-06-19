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
            this.tablePanelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.tablePanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.labelPanelName = new System.Windows.Forms.Label();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.treeView = new ExControls.ExOptionsView.ExOptionsTreeView();
            this.cbSearch = new ExControls.ExComboBox();
            this.panelsContainer = new ExControls.OptionsPanelContainer();
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
            this.split.Margin = new System.Windows.Forms.Padding(4);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.tablePanelLeft);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.tablePanelRight);
            this.split.Size = new System.Drawing.Size(656, 469);
            this.split.SplitterDistance = 217;
            this.split.SplitterWidth = 5;
            this.split.TabIndex = 0;
            // 
            // tablePanelLeft
            // 
            this.tablePanelLeft.AutoSize = true;
            this.tablePanelLeft.ColumnCount = 1;
            this.tablePanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelLeft.Controls.Add(this.treeView, 0, 1);
            this.tablePanelLeft.Controls.Add(this.cbSearch, 0, 0);
            this.tablePanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanelLeft.Location = new System.Drawing.Point(0, 0);
            this.tablePanelLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanelLeft.Name = "tablePanelLeft";
            this.tablePanelLeft.RowCount = 2;
            this.tablePanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelLeft.Size = new System.Drawing.Size(217, 469);
            this.tablePanelLeft.TabIndex = 2;
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
            this.tablePanelRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanelRight.Name = "tablePanelRight";
            this.tablePanelRight.RowCount = 2;
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelRight.Size = new System.Drawing.Size(434, 469);
            this.tablePanelRight.TabIndex = 1;
            // 
            // labelPanelName
            // 
            this.labelPanelName.AutoSize = true;
            this.labelPanelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPanelName.Location = new System.Drawing.Point(3, 0);
            this.labelPanelName.Name = "labelPanelName";
            this.labelPanelName.Size = new System.Drawing.Size(326, 25);
            this.labelPanelName.TabIndex = 0;
            this.labelPanelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMenu.Location = new System.Drawing.Point(332, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMenu.Size = new System.Drawing.Size(102, 25);
            this.toolStripMenu.TabIndex = 1;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // treeView
            // 
            this.treeView.Dock = DockStyle.Fill;
            this.treeView.FullRowSelect = true;
            this.treeView.HideSelection = false;
            this.treeView.Name = "treeView";
            this.treeView.PathSeparator = " / ";
            this.treeView.ShowLines = false;
            this.treeView.ShowNodeToolTips = true;
            this.treeView.TabIndex = 0;
            // 
            // cbSearch
            // 
            this.cbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSearch.DropDownSelectedRowBackColor = System.Drawing.SystemColors.Highlight;
            this.cbSearch.FormattingEnabled = true;
            this.cbSearch.Location = new System.Drawing.Point(3, 2);
            this.cbSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbSearch.Name = "cbSearch";
            this.cbSearch.Size = new System.Drawing.Size(211, 24);
            this.cbSearch.StyleDisabled.ArrowColor = null;
            this.cbSearch.StyleDisabled.BackColor = null;
            this.cbSearch.StyleDisabled.BorderColor = null;
            this.cbSearch.StyleDisabled.ButtonBackColor = null;
            this.cbSearch.StyleDisabled.ButtonBorderColor = null;
            this.cbSearch.StyleDisabled.ButtonRenderFirst = null;
            this.cbSearch.StyleDisabled.ForeColor = null;
            this.cbSearch.StyleHighlight.ArrowColor = null;
            this.cbSearch.StyleHighlight.BackColor = null;
            this.cbSearch.StyleHighlight.BorderColor = null;
            this.cbSearch.StyleHighlight.ButtonBackColor = null;
            this.cbSearch.StyleHighlight.ButtonBorderColor = null;
            this.cbSearch.StyleHighlight.ButtonRenderFirst = null;
            this.cbSearch.StyleHighlight.ForeColor = null;
            this.cbSearch.StyleNormal.ArrowColor = null;
            this.cbSearch.StyleNormal.BackColor = null;
            this.cbSearch.StyleNormal.BorderColor = null;
            this.cbSearch.StyleNormal.ButtonBackColor = null;
            this.cbSearch.StyleNormal.ButtonBorderColor = null;
            this.cbSearch.StyleNormal.ButtonRenderFirst = null;
            this.cbSearch.StyleNormal.ForeColor = null;
            this.cbSearch.StyleSelected.ArrowColor = null;
            this.cbSearch.StyleSelected.BackColor = null;
            this.cbSearch.StyleSelected.BorderColor = null;
            this.cbSearch.StyleSelected.ButtonBackColor = null;
            this.cbSearch.StyleSelected.ButtonBorderColor = null;
            this.cbSearch.StyleSelected.ButtonRenderFirst = null;
            this.cbSearch.StyleSelected.ForeColor = null;
            this.cbSearch.TabIndex = 1;
            this.cbSearch.UseDarkScrollBar = false;
            // 
            // panelsContainer
            // 
            this.tablePanelRight.SetColumnSpan(this.panelsContainer, 2);
            this.panelsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelsContainer.Location = new System.Drawing.Point(4, 29);
            this.panelsContainer.Margin = new System.Windows.Forms.Padding(4);
            this.panelsContainer.Name = "panelsContainer";
            this.panelsContainer.Size = new System.Drawing.Size(426, 436);
            this.panelsContainer.TabIndex = 0;
            // 
            // ExOptionsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.split);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ExOptionsView";
            this.Size = new System.Drawing.Size(656, 469);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel1.PerformLayout();
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tablePanelLeft.ResumeLayout(false);
            this.tablePanelRight.ResumeLayout(false);
            this.tablePanelRight.PerformLayout();
            this.ResumeLayout(false);

        }

        internal SplitContainer split;
        internal ExOptionsTreeView treeView;
        internal OptionsPanelContainer panelsContainer;
        private ExComboBox cbSearch;
        private TableLayoutPanel tablePanelRight;
        internal Label labelPanelName;
        private ToolStrip toolStripMenu;
        private TableLayoutPanel tablePanelLeft;
    }
}