namespace ExControls.Designers
{
    partial class FSelectPage
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbPages = new System.Windows.Forms.ListBox();
            this.treeNodeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.treeNodeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lbPages
            // 
            this.lbPages.DataSource = this.treeNodeBindingSource;
            this.lbPages.DisplayMember = "Text";
            this.lbPages.FormattingEnabled = true;
            this.lbPages.Location = new System.Drawing.Point(9, 32);
            this.lbPages.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lbPages.Name = "lbPages";
            this.lbPages.Size = new System.Drawing.Size(159, 251);
            this.lbPages.TabIndex = 0;
            // 
            // treeNodeBindingSource
            // 
            this.treeNodeBindingSource.DataSource = typeof(System.Windows.Forms.TreeNode);
            // 
            // panelBindingSource
            // 
            this.panelBindingSource.DataSource = typeof(System.Windows.Forms.Panel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a page:";
            // 
            // bOK
            // 
            this.bOK.AutoSize = true;
            this.bOK.Location = new System.Drawing.Point(60, 287);
            this.bOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(60, 29);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // FSelectPage
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 324);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSelectPage";
            this.ShowIcon = false;
            this.Text = "Select a page";
            ((System.ComponentModel.ISupportInitialize)(this.treeNodeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lbPages;
        private Label label1;
        private Button bOK;
        private BindingSource panelBindingSource;
        private BindingSource treeNodeBindingSource;
    }
}