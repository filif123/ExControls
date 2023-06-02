using System.Drawing;
using System.Windows.Forms;

namespace ExControls.Test
{
    public partial class Form2 : ExForm
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, System.EventArgs e)
        {

        }

        private void ExButton1_Click(object sender, System.EventArgs e)
        {
            TitleBarBackColor = Color.Aquamarine;
        }

        private void ExDateTimePicker1_MouseCaptureChanged(object sender, System.EventArgs e)
        {

        }
    }
}
