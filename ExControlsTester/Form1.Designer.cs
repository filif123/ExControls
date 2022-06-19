
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            ExControls.OptionsNode optionsNode1 = new ExControls.OptionsNode();
            ExControls.OptionsNode optionsNode2 = new ExControls.OptionsNode();
            ExControls.OptionsNode optionsNode3 = new ExControls.OptionsNode();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.exOptionsView1 = new ExControls.ExOptionsView();
            this.exOptionsPanel1 = new ExControls.ExOptionsPanel();
            this.exOptionsPanel2 = new ExControls.ExOptionsPanel();
            this.exOptionsPanel3 = new ExControls.ExOptionsPanel();
            ((System.ComponentModel.ISupportInitialize)(this.exOptionsView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // exOptionsView1
            // 
            this.exOptionsView1.HeaderNodeNameFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exOptionsView1.Location = new System.Drawing.Point(5, 47);
            this.exOptionsView1.Margin = new System.Windows.Forms.Padding(4);
            this.exOptionsView1.Name = "exOptionsView1";
            this.exOptionsView1.Panels.Add(this.exOptionsPanel1);
            this.exOptionsView1.Panels.Add(this.exOptionsPanel2);
            this.exOptionsView1.Panels.Add(this.exOptionsPanel3);
            this.exOptionsView1.SelectedPanel = this.exOptionsPanel1;
            this.exOptionsView1.Size = new System.Drawing.Size(682, 469);
            this.exOptionsView1.TabIndex = 1;
            // 
            // exOptionsView1.ToolStripMenu
            // 
            this.exOptionsView1.ToolStripMenu.BackColor = System.Drawing.SystemColors.Control;
            this.exOptionsView1.ToolStripMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exOptionsView1.ToolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.exOptionsView1.ToolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.exOptionsView1.ToolStripMenu.Location = new System.Drawing.Point(350, 0);
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
            this.exOptionsPanel1.Name = "exOptionsPanel1";
            optionsNode1.Name = "";
            optionsNode1.Text = "exOptionsPanel1";
            this.exOptionsPanel1.Node = optionsNode1;
            this.exOptionsPanel1.NodeText = "exOptionsPanel1";
            this.exOptionsPanel1.ParentNode = null;
            // 
            // exOptionsPanel2
            // 
            this.exOptionsPanel2.Name = "exOptionsPanel2";
            optionsNode2.Name = "";
            optionsNode2.Text = "exOptionsPanel2";
            this.exOptionsPanel2.Node = optionsNode2;
            this.exOptionsPanel2.NodeText = "exOptionsPanel2";
            this.exOptionsPanel2.ParentNode = null;
            // 
            // exOptionsPanel3
            // 
            this.exOptionsPanel3.Name = "exOptionsPanel3";
            optionsNode3.Name = "";
            optionsNode3.Text = "exOptionsPanel3";
            this.exOptionsPanel3.Node = optionsNode3;
            this.exOptionsPanel3.NodeText = "exOptionsPanel3";
            this.exOptionsPanel3.ParentNode = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1176, 626);
            this.Controls.Add(this.exOptionsView1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Controls.SetChildIndex(this.exOptionsView1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.exOptionsView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private ExOptionsView exOptionsView1;
        private ExOptionsPanel exOptionsPanel1;
        private ExOptionsPanel exOptionsPanel2;
        private ExOptionsPanel exOptionsPanel3;
    }
}

