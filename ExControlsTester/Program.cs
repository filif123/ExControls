using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls.Test
{
    internal static class Program
    {
        /// <summary>
        ///     Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ExMessageBox.Style = new ExMessageBoxStyle
            {
                LabelFont = new Font(new FontFamily(SystemFonts.MenuFont.Name), SystemFonts.MenuFont.SizeInPoints - 1),
                ButtonsFont = null
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
        }
    }
}