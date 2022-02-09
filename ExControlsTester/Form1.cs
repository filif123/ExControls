using System;
using System.Drawing;
using System.Windows.Forms;
using ExControls.Controls;

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

        private void exVerticalTabControl1_Load(object sender, EventArgs e)
        {

        }
    }
}