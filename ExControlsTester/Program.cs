namespace ExControls.Test;

internal static class Program
{
    /// <summary>
    /// Hlavní vstupní bod aplikace.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // pred prvym oknom; predtym dpiAware v app.manifest
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        ExMessageBox.Style = new ExMessageBoxStyle
        {
            LabelFont = new Font(new FontFamily(SystemFonts.MenuFont!.Name), SystemFonts.MenuFont.SizeInPoints - 1),
            ButtonsFont = null
        };

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form2());
    }
}