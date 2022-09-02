
using ExControls;

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
            ExControls.OptionsNode optionsNode1 = new ExControls.OptionsNode();
            ExControls.OptionsNode optionsNode2 = new ExControls.OptionsNode();
            ExControls.OptionsNode optionsNode3 = new ExControls.OptionsNode();
            this.exOptionsView1 = new ExControls.ExOptionsView();
            this.exOptionsPanel1 = new ExControls.ExOptionsPanel(this.exOptionsView1);
            this.exOptionsPanel2 = new ExControls.ExOptionsPanel(this.exOptionsView1);
            this.exPropertyGrid1 = new ExControls.ExPropertyGrid();
            this.exOptionsPanel3 = new ExControls.ExOptionsPanel(this.exOptionsView1);
            ((System.ComponentModel.ISupportInitialize)(this.exOptionsView1)).BeginInit();
            this.exOptionsPanel2.SuspendLayout();
            this.exPropertyGrid1.SuspendLayout();
            this.SuspendLayout();
            // 
            // exOptionsView1
            // 
            this.exOptionsView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exOptionsView1.HeaderNodeNameBackColor = System.Drawing.SystemColors.Control;
            this.exOptionsView1.HeaderNodeNameFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exOptionsView1.HeaderNodeNameForeColor = System.Drawing.SystemColors.ControlText;
            this.exOptionsView1.LinkToChildrenForeColor = System.Drawing.Color.Empty;
            this.exOptionsView1.Location = new System.Drawing.Point(1, 31);
            this.exOptionsView1.Name = "exOptionsView1";
            this.exOptionsView1.Panels.Add(this.exOptionsPanel1);
            this.exOptionsView1.Panels.Add(this.exOptionsPanel2);
            this.exOptionsView1.Panels.Add(this.exOptionsPanel3);
            this.exOptionsView1.Size = new System.Drawing.Size(684, 456);
            this.exOptionsView1.TabIndex = 1;
            // 
            // exOptionsView1.ToolStripMenu
            // 
            this.exOptionsView1.ToolStripMenu.BackColor = System.Drawing.SystemColors.Control;
            this.exOptionsView1.ToolStripMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exOptionsView1.ToolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.exOptionsView1.ToolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.exOptionsView1.ToolStripMenu.Location = new System.Drawing.Point(354, 0);
            this.exOptionsView1.ToolStripMenu.Name = "ToolStripMenu";
            this.exOptionsView1.ToolStripMenu.Size = new System.Drawing.Size(102, 25);
            this.exOptionsView1.ToolStripMenu.TabIndex = 1;
            this.exOptionsView1.ToolStripMenu.Text = "toolStrip1";
            // 
            // 
            // 
            this.exOptionsView1.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exOptionsView1.TreeView.FullRowSelect = true;
            this.exOptionsView1.TreeView.HideSelection = false;
            this.exOptionsView1.TreeView.Name = "treeView";
            this.exOptionsView1.TreeView.PathSeparator = " / ";
            this.exOptionsView1.TreeView.ShowLines = false;
            this.exOptionsView1.TreeView.ShowNodeToolTips = true;
            this.exOptionsView1.TreeView.TabIndex = 0;
            // 
            // exOptionsPanel1
            // 
            this.exOptionsPanel1.GenerateLinksToChildren = true;
            this.exOptionsPanel1.Name = "exOptionsPanel1";
            optionsNode1.Name = "";
            optionsNode1.Text = "exOptionsPanel1";
            this.exOptionsPanel1.Node = optionsNode1;
            this.exOptionsPanel1.NodeText = "exOptionsPanel1";
            this.exOptionsPanel1.ParentNode = null;
            // 
            // exOptionsPanel2
            // 
            this.exOptionsPanel2.Controls.Add(this.exPropertyGrid1);
            this.exOptionsPanel2.Name = "exOptionsPanel2";
            optionsNode2.Name = "";
            optionsNode2.Text = "exOptionsPanel2";
            this.exOptionsPanel2.Node = optionsNode2;
            this.exOptionsPanel2.NodeText = "exOptionsPanel2";
            this.exOptionsPanel2.ParentNode = optionsNode1;
            // 
            // exPropertyGrid1
            // 
            this.exPropertyGrid1.BrowsableProperties = null;
            // 
            // 
            // 
            this.exPropertyGrid1.ButtonAlphabetical.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.exPropertyGrid1.ButtonAlphabetical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exPropertyGrid1.ButtonAlphabetical.ImageIndex = 0;
            this.exPropertyGrid1.ButtonAlphabetical.Name = "";
            this.exPropertyGrid1.ButtonAlphabetical.Size = new System.Drawing.Size(23, 22);
            this.exPropertyGrid1.ButtonAlphabetical.Text = "Alphabetical";
            // 
            // 
            // 
            this.exPropertyGrid1.ButtonCategorized.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.exPropertyGrid1.ButtonCategorized.Checked = true;
            this.exPropertyGrid1.ButtonCategorized.CheckState = System.Windows.Forms.CheckState.Checked;
            this.exPropertyGrid1.ButtonCategorized.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exPropertyGrid1.ButtonCategorized.ImageIndex = 1;
            this.exPropertyGrid1.ButtonCategorized.Name = "";
            this.exPropertyGrid1.ButtonCategorized.Size = new System.Drawing.Size(23, 22);
            this.exPropertyGrid1.ButtonCategorized.Text = "Categorized";
            // 
            // 
            // 
            this.exPropertyGrid1.ButtonPropertyPages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exPropertyGrid1.ButtonPropertyPages.Enabled = false;
            this.exPropertyGrid1.ButtonPropertyPages.ImageIndex = 3;
            this.exPropertyGrid1.ButtonPropertyPages.Name = "";
            this.exPropertyGrid1.ButtonPropertyPages.Size = new System.Drawing.Size(23, 22);
            this.exPropertyGrid1.ButtonPropertyPages.Text = "Property Pages";
            this.exPropertyGrid1.ButtonPropertyPages.Visible = false;
            this.exPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exPropertyGrid1.FirstHideAllProperties = false;
            this.exPropertyGrid1.HiddenAttributes = null;
            this.exPropertyGrid1.HiddenProperties = null;
            // 
            // exPropertyGrid1.InnerToolStrip
            // 
            this.exPropertyGrid1.InnerToolStrip.AccessibleName = "Property Grid";
            this.exPropertyGrid1.InnerToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.exPropertyGrid1.InnerToolStrip.AllowMerge = false;
            this.exPropertyGrid1.InnerToolStrip.AutoSize = false;
            this.exPropertyGrid1.InnerToolStrip.CanOverflow = false;
            this.exPropertyGrid1.InnerToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.exPropertyGrid1.InnerToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.exPropertyGrid1.InnerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exPropertyGrid1.ButtonCategorized,
            this.exPropertyGrid1.ButtonAlphabetical,
            this.exPropertyGrid1.Separator,
            this.exPropertyGrid1.ButtonPropertyPages});
            this.exPropertyGrid1.InnerToolStrip.Location = new System.Drawing.Point(0, 1);
            this.exPropertyGrid1.InnerToolStrip.Name = "InnerToolStrip";
            this.exPropertyGrid1.InnerToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.exPropertyGrid1.InnerToolStrip.Size = new System.Drawing.Size(450, 25);
            this.exPropertyGrid1.InnerToolStrip.TabIndex = 1;
            this.exPropertyGrid1.InnerToolStrip.TabStop = true;
            this.exPropertyGrid1.InnerToolStrip.Text = "PropertyGridToolBar";
            this.exPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.exPropertyGrid1.Name = "exPropertyGrid1";
            this.exPropertyGrid1.SelectedObject = this.exPropertyGrid1;
            this.exPropertyGrid1.Size = new System.Drawing.Size(450, 425);
            this.exPropertyGrid1.TabIndex = 0;
            // 
            // exOptionsPanel3
            // 
            this.exOptionsPanel3.Name = "exOptionsPanel3";
            optionsNode3.Name = "";
            optionsNode3.Text = "test3he";
            this.exOptionsPanel3.Node = optionsNode3;
            this.exOptionsPanel3.NodeText = "test3he";
            this.exOptionsPanel3.ParentNode = optionsNode1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(686, 488);
            this.Controls.Add(this.exOptionsView1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Controls.SetChildIndex(this.exOptionsView1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.exOptionsView1)).EndInit();
            this.exOptionsPanel2.ResumeLayout(false);
            this.exPropertyGrid1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExOptionsView exOptionsView1;
        private ExOptionsPanel exOptionsPanel1;
        private ExOptionsPanel exOptionsPanel2;
        private ExOptionsPanel exOptionsPanel3;
        private ExPropertyGrid exPropertyGrid1;
    }
}

