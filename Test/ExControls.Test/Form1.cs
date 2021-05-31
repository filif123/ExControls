using System;
using System.Windows.Forms;

namespace ExControls.Test
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void bbrowser_Click(object sender, EventArgs e)
        {
            exfbdialog.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exComboBox2.SelectedItem = exComboBox2.Items[0];
            exComboBox1.SelectedItem = exComboBox1.Items[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            exComboBox1.Enabled = !exComboBox1.Enabled;
        }
    }
}
