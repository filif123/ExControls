using System.Windows.Forms;

namespace ExControls.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            exCheckBox1.SetTheme(WindowsTheme.DarkExplorer);
        }

        private void bbrowser_Click(object sender, System.EventArgs e)
        {
            exfbdialog.Show();
        }
    }
}
