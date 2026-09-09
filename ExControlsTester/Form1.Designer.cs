
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ExControls.OptionsNode optionsNode1 = new ExControls.OptionsNode();
            ExControls.OptionsNode optionsNode2 = new ExControls.OptionsNode();
            ExControls.OptionsNode optionsNode3 = new ExControls.OptionsNode();
            ExControls.OptionsNode optionsNode4 = new ExControls.OptionsNode();
            exOptionsView1 = new ExControls.ExOptionsView();
            exOptionsPanel1 = new ExControls.ExOptionsPanel(exOptionsView1);
            exOptionsPanel2 = new ExControls.ExOptionsPanel(exOptionsView1);
            exPropertyGrid1 = new ExControls.ExPropertyGrid();
            exOptionsPanel3 = new ExControls.ExOptionsPanel(exOptionsView1);
            exOptionsPanel4 = new ExControls.ExOptionsPanel(exOptionsView1);
            ((System.ComponentModel.ISupportInitialize)exOptionsView1).BeginInit();
            exOptionsPanel2.SuspendLayout();
            exPropertyGrid1.SuspendLayout();
            SuspendLayout();
            // 
            // exOptionsView1
            // 
            exOptionsView1.Dock = System.Windows.Forms.DockStyle.Fill;
            exOptionsView1.HeaderNodeNameBackColor = System.Drawing.SystemColors.Control;
            exOptionsView1.HeaderNodeNameFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)238));
            exOptionsView1.HeaderNodeNameForeColor = System.Drawing.SystemColors.ControlText;
            exOptionsView1.HeaderNodeNameVisible = true;
            exOptionsView1.LinkToChildrenForeColor = System.Drawing.Color.Empty;
            exOptionsView1.Location = new System.Drawing.Point(0, 0);
            exOptionsView1.Name = "exOptionsView1";
            exOptionsView1.SearchBoxVisible = true;
            exOptionsView1.Size = new System.Drawing.Size(686, 488);
            exOptionsView1.TabIndex = 1;
            // 
            // exOptionsPanel1
            // 
            exOptionsPanel1.GenerateLinksToChildren = true;
            exOptionsPanel1.Name = "exOptionsPanel1";
            optionsNode1.Name = "";
            optionsNode1.Text = "exOptionsPanel1";
            exOptionsPanel1.Node = optionsNode1;
            exOptionsPanel1.NodeText = "exOptionsPanel1";
            exOptionsPanel1.ParentNode = null;
            // 
            // exOptionsPanel2
            // 
            exOptionsPanel2.Controls.Add(exPropertyGrid1);
            exOptionsPanel2.GenerateLinksToChildren = false;
            exOptionsPanel2.Name = "exOptionsPanel2";
            optionsNode2.Name = "";
            optionsNode2.Text = "exOptionsPanel2";
            exOptionsPanel2.Node = optionsNode2;
            exOptionsPanel2.NodeText = "exOptionsPanel2";
            exOptionsPanel2.ParentNode = optionsNode1;
            // 
            // exPropertyGrid1
            // 
            exPropertyGrid1.BackColor = System.Drawing.SystemColors.Control;
            exPropertyGrid1.BrowsableProperties = null;
            exPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            exPropertyGrid1.FirstHideAllProperties = false;
            exPropertyGrid1.HiddenAttributes = null;
            exPropertyGrid1.HiddenProperties = null;
            exPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            exPropertyGrid1.Name = "exPropertyGrid1";
            exPropertyGrid1.SelectedObject = exPropertyGrid1;
            exPropertyGrid1.Size = new System.Drawing.Size(451, 457);
            exPropertyGrid1.TabIndex = 0;
            // 
            // exOptionsPanel3
            // 
            exOptionsPanel3.GenerateLinksToChildren = false;
            exOptionsPanel3.Name = "exOptionsPanel3";
            optionsNode3.Name = "";
            optionsNode3.Text = "test3he";
            exOptionsPanel3.Node = optionsNode3;
            exOptionsPanel3.NodeText = "test3he";
            exOptionsPanel3.ParentNode = optionsNode1;
            // 
            // exOptionsPanel4
            // 
            exOptionsPanel4.GenerateLinksToChildren = false;
            exOptionsPanel4.Name = "exOptionsPanel4";
            optionsNode4.Name = "";
            optionsNode4.Text = "exOptionsPanel4";
            exOptionsPanel4.Node = optionsNode4;
            exOptionsPanel4.NodeText = "exOptionsPanel4";
            exOptionsPanel4.ParentNode = null;
            // 
            // Form1
            // 
            ClientSize = new System.Drawing.Size(686, 488);
            Controls.Add(exOptionsView1);
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)exOptionsView1).EndInit();
            exOptionsPanel2.ResumeLayout(false);
            exPropertyGrid1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private ExControls.ExOptionsPanel exOptionsPanel4;

        #endregion

        private ExControls.ExOptionsView exOptionsView1;
        private ExOptionsPanel exOptionsPanel1;
        private ExOptionsPanel exOptionsPanel2;
        private ExOptionsPanel exOptionsPanel3;
        private ExPropertyGrid exPropertyGrid1;
    }
}

