
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
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.exOptionsView1 = new ExControls.ExOptionsView();
            this.exOptionsPanel1 = new ExControls.ExOptionsPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.exOptionsPanel2 = new ExControls.ExOptionsPanel();
            this.exCheckBox1 = new ExControls.ExCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.exOptionsView1)).BeginInit();
            this.exOptionsPanel1.SuspendLayout();
            this.exOptionsPanel2.SuspendLayout();
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
            this.exOptionsView1.HeaderNodeNameFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.exOptionsView1.Location = new System.Drawing.Point(4, 37);
            this.exOptionsView1.Name = "exOptionsView1";
            this.exOptionsView1.Panels.Add(this.exOptionsPanel1);
            this.exOptionsView1.Panels.Add(this.exOptionsPanel2);
            this.exOptionsView1.Size = new System.Drawing.Size(615, 344);
            this.exOptionsView1.TabIndex = 1;
            // 
            // exOptionsView1.ToolStripMenu
            // 
            this.exOptionsView1.ToolStripMenu.BackColor = System.Drawing.SystemColors.Control;
            this.exOptionsView1.ToolStripMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exOptionsView1.ToolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.exOptionsView1.ToolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.exOptionsView1.ToolStripMenu.Location = new System.Drawing.Point(307, 0);
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
            this.exOptionsPanel1.Controls.Add(this.label1);
            this.exOptionsPanel1.Name = "exOptionsPanel1";
            optionsNode1.Name = "";
            optionsNode1.Text = "exOptionsPanel1";
            this.exOptionsPanel1.Node = optionsNode1;
            this.exOptionsPanel1.NodeText = "exOptionsPanel1";
            this.exOptionsPanel1.ParentNode = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // exOptionsPanel2
            // 
            this.exOptionsPanel2.Controls.Add(this.exCheckBox1);
            this.exOptionsPanel2.Name = "exOptionsPanel2";
            optionsNode2.Name = "";
            optionsNode2.Text = "exOptionsPanel2";
            this.exOptionsPanel2.Node = optionsNode2;
            this.exOptionsPanel2.NodeText = "exOptionsPanel2";
            this.exOptionsPanel2.ParentNode = null;
            // 
            // exCheckBox1
            // 
            this.exCheckBox1.AutoSize = true;
            this.exCheckBox1.BoxBackColor = System.Drawing.Color.White;
            this.exCheckBox1.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.exCheckBox1.Location = new System.Drawing.Point(128, 106);
            this.exCheckBox1.Name = "exCheckBox1";
            this.exCheckBox1.Size = new System.Drawing.Size(52, 17);
            this.exCheckBox1.TabIndex = 0;
            this.exCheckBox1.Text = "stevo";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(871, 547);
            this.Controls.Add(this.exOptionsView1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Controls.SetChildIndex(this.exOptionsView1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.exOptionsView1)).EndInit();
            this.exOptionsPanel1.ResumeLayout(false);
            this.exOptionsPanel1.PerformLayout();
            this.exOptionsPanel2.ResumeLayout(false);
            this.exOptionsPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private ExOptionsView exOptionsView1;
        private ExOptionsPanel exOptionsPanel1;
        private ExOptionsPanel exOptionsPanel2;
        private ExCheckBox exCheckBox1;
        private System.Windows.Forms.Label label1;
    }
}

