using System;
using System.Drawing;

namespace ExControls.Test
{
    public partial class Form1 : ExForm
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.SetFormStyle(FormStyle.Mica);
            this.SetTitlebarAndBorderColor(Color.Blue, Color.White, Color.Blue);
            //this.SetFormCorners(FormCornersType.Rectangular);
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