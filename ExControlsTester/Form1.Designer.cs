
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
            this.exVerticalTabControl1 = new ExControls.ExVerticalTabControl();
            this.exVerticalTabPage1 = new ExControls.ExVerticalTabPage("exVerticalTabPage1");
            this.exVerticalTabPage2 = new ExControls.ExVerticalTabPage("exVerticalTabPage2");
            this.exButton1 = new ExControls.ExButton();
            this.exVerticalTabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // exVerticalTabControl1
            // 
            this.exVerticalTabControl1.Location = new System.Drawing.Point(4, 37);
            this.exVerticalTabControl1.Name = "exVerticalTabControl1";
            this.exVerticalTabControl1.SelectedTab = this.exVerticalTabPage1;
            this.exVerticalTabControl1.Size = new System.Drawing.Size(602, 325);
            this.exVerticalTabControl1.TabIndex = 1;
            this.exVerticalTabControl1.TabPages.AddRange(new ExControls.ExVerticalTabPage[] {
            this.exVerticalTabPage1,
            this.exVerticalTabPage2});
            // 
            // exVerticalTabControl1.ToolStripMenu
            // 
            this.exVerticalTabControl1.ToolStripMenu.BackColor = System.Drawing.Color.Transparent;
            this.exVerticalTabControl1.ToolStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.exVerticalTabControl1.ToolStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.exVerticalTabControl1.ToolStripMenu.Location = new System.Drawing.Point(200, 0);
            this.exVerticalTabControl1.ToolStripMenu.Name = "ToolStripMenu";
            this.exVerticalTabControl1.ToolStripMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.exVerticalTabControl1.ToolStripMenu.Size = new System.Drawing.Size(201, 25);
            this.exVerticalTabControl1.ToolStripMenu.TabIndex = 0;
            this.exVerticalTabControl1.ToolStripMenu.Text = "toolStrip1";
            this.exVerticalTabControl1.Load += new System.EventHandler(this.exVerticalTabControl1_Load);
            // 
            // exVerticalTabPage1
            // 
            this.exVerticalTabPage1.Name = "exVerticalTabPage1";
            this.exVerticalTabPage1.Node.Name = "exVerticalTabPage1";
            this.exVerticalTabPage1.Node.Text = "exVerticalTabPage1";
            this.exVerticalTabPage1.TabIndex = 0;
            this.exVerticalTabPage1.Text = "exVerticalTabPage1";
            // 
            // exVerticalTabPage2
            // 
            this.exVerticalTabPage2.Controls.Add(this.exButton1);
            this.exVerticalTabPage2.Name = "exVerticalTabPage2";
            this.exVerticalTabPage2.Node.Name = "exVerticalTabPage2";
            this.exVerticalTabPage2.Node.Text = "exVerticalTabPage2";
            this.exVerticalTabPage2.TabIndex = 1;
            this.exVerticalTabPage2.Text = "exVerticalTabPage2";
            // 
            // exButton1
            // 
            this.exButton1.Location = new System.Drawing.Point(79, 68);
            this.exButton1.Name = "exButton1";
            this.exButton1.Size = new System.Drawing.Size(72, 34);
            this.exButton1.TabIndex = 0;
            this.exButton1.Text = "exButton1";
            this.exButton1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderColor = System.Drawing.Color.Blue;
            this.ClientSize = new System.Drawing.Size(988, 561);
            this.Controls.Add(this.exVerticalTabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Controls.SetChildIndex(this.exVerticalTabControl1, 0);
            this.exVerticalTabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExVerticalTabControl exVerticalTabControl1;
        private ExVerticalTabPage exVerticalTabPage2;
        private ExVerticalTabPage exVerticalTabPage1;
        private ExButton exButton1;
    }
}

