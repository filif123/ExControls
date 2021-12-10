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
            this.panelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.treeNodeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeNodeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lbPages
            // 
            this.lbPages.DataSource = this.treeNodeBindingSource;
            this.lbPages.DisplayMember = "Text";
            this.lbPages.FormattingEnabled = true;
            this.lbPages.ItemHeight = 16;
            this.lbPages.Location = new System.Drawing.Point(12, 39);
            this.lbPages.Name = "lbPages";
            this.lbPages.Size = new System.Drawing.Size(211, 308);
            this.lbPages.TabIndex = 0;
            // 
            // panelBindingSource
            // 
            this.panelBindingSource.DataSource = typeof(System.Windows.Forms.Panel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a page:";
            // 
            // bOK
            // 
            this.bOK.AutoSize = true;
            this.bOK.Location = new System.Drawing.Point(80, 353);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(80, 36);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // treeNodeBindingSource
            // 
            this.treeNodeBindingSource.DataSource = typeof(System.Windows.Forms.TreeNode);
            // 
            // FSelectPage
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 398);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSelectPage";
            this.ShowIcon = false;
            this.Text = "Select a page";
            ((System.ComponentModel.ISupportInitialize)(this.panelBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeNodeBindingSource)).EndInit();
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