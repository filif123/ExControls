using System;
using System.Drawing;

namespace ExControls.Test
{
    public partial class Form1 : ExForm
    {
        public Form1()
        {
            InitializeComponent();
            //pictureBox1.Image = new ShellIcon(ShellIconType.Info).ToBitmap();
            ExMessageBox.Style.DefaultStyle = false;
            ExMessageBox.Style.BackColor = Color.Black;
            ExMessageBox.Style.UseDarkTitleBar = true;
            ExMessageBox.Style.FooterBackColor = Color.Black; //Color.FromArgb(38, 38, 38);
            ExMessageBox.Style.ForeColor = Color.White;
            ExMessageBox.Style.ButtonBorderColor = Color.White;
            ExMessageBox.Style.ButtonForeColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ExOptionsPanel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void ExPropertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void ExCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ExColorSelector1_Load(object sender, EventArgs e)
        {

        }
    }
}