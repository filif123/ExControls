using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls.Test
{
    public partial class Form1 : Form
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

        private void bbrowser_Click(object sender, EventArgs e)
        {
            exfbdialog.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exComboBox2.SelectedItem = exComboBox2.Items[0];
            exComboBox1.SelectedItem = exComboBox1.Items[0];
            ExMessageBox.Show(
                "Thugdsfffffhgdsuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuufygdsyhfgsduyfgsdyufhgsduyfhgsduiyfguusigighughgiurehiuhujghsiugfhsdjugfhdiugdhuigdgiudyhgiu",
                "hh", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 0, "https://google.com", 10);
            //MessageBox.Show(this,"Test\nhdhufdufdhyufsdhufsdhufsduhy\njkfdff\nddd", "Caption", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1 ,0,"ff");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            exComboBox1.Enabled = !exComboBox1.Enabled;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            /*if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.Window.CloseButton.Normal))
            {
                var renderer = new VisualStyleRenderer(VisualStyleElement.Window.CloseButton.Normal);
                //var rectangle1 = new Rectangle(pictureBox1.Location.X, pictureBox1.Location.Y, pictureBox1.Width, pictureBox1.Height);
                renderer.DrawBackground(e.Graphics, e.ClipRectangle);
            }**/
        }
    }
}