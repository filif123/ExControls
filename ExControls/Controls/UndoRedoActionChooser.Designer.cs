namespace ExControls
{
    partial class UndoRedoActionChooser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbActions = new System.Windows.Forms.ListBox();
            this.lActionsCount = new ExControls.ExLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lActionsCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 172);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 24);
            this.panel1.TabIndex = 1;
            // 
            // lbActions
            // 
            this.lbActions.BackColor = System.Drawing.SystemColors.Control;
            this.lbActions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbActions.FormattingEnabled = true;
            this.lbActions.Location = new System.Drawing.Point(0, 0);
            this.lbActions.Name = "lbActions";
            this.lbActions.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbActions.Size = new System.Drawing.Size(231, 172);
            this.lbActions.TabIndex = 2;
            this.lbActions.Click += new System.EventHandler(this.LbActions_Click);
            this.lbActions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LbActions_MouseMove);
            // 
            // lActionsCount
            // 
            this.lActionsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lActionsCount.Location = new System.Drawing.Point(0, 0);
            this.lActionsCount.Name = "lActionsCount";
            this.lActionsCount.Size = new System.Drawing.Size(231, 24);
            this.lActionsCount.TabIndex = 0;
            this.lActionsCount.Text = "Späť X akcií";
            this.lActionsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UndoRedoActionChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.lbActions);
            this.Controls.Add(this.panel1);
            this.Name = "UndoRedoActionChooser";
            this.Size = new System.Drawing.Size(231, 196);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private ExLabel lActionsCount;
        private ListBox lbActions;
    }
}
