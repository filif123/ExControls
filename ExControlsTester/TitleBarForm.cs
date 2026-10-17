using System.Globalization;

namespace ExControls.Test;

/// <summary>
/// Skuska vlastneho zahlavia <see cref="ExTitleBar" />: ram okna, tlacidla okna, ikona, text, aktivita a tema.
/// </summary>
public partial class TitleBarForm : ExForm
{
    private const string ShortTitle = "ExControls – vlastné záhlavie";
    private const string LongTitle = "ExControls – vlastné záhlavie s veľmi dlhým textom okna, ktorý sa do voľného miesta záhlavia nezmestí";

    public TitleBarForm()
    {
        InitializeComponent();
        txtInfo.Text = string.Join(Environment.NewLine,
            "Čo vyskúšať:",
            "",
            "• Tlačidlá minimalizovať, maximalizovať/obnoviť a zavrieť: zvýraznenie pod myšou a pri stlačení, zatvorenie je červené.",
            "• Windows 11: podržanie myši nad maximalizovaním ukáže rozloženia okien (Snap Layouts).",
            "• Stlačiť tlačidlo, odísť z neho myšou a pustiť = nič sa nestane.",
            "• Ikona vľavo: klik = systémová ponuka, dvojklik = zavrieť.",
            "• Text okna je v strede voľného miesta záhlavia. Pri zúžení okna sa posunie a skráti, keď sa nezmestí, zmizne (Okno → Dlhý text okna).",
            "• Neaktívne okno (klik do iného programu): text a znaky tlačidiel sú stlmené.",
            "• Okno → Tmavá téma: záhlavie aj ponuka podľa témy, položka ponuky pod myšou je čitateľná.",
            "• Okno → Systémové záhlavie: vráti systémové záhlavie (v tmavej téme tmavé), ponuka ostane v paneli bez ikony a tlačidiel.",
            "• Ťahanie, dvojklik, systémová ponuka pravým klikom, Aero Snap a horný okraj ako v etape 1.",
            "• Windows 10: horný 1 px okraj kreslí záhlavie (pri maximalizácii nie je).");
        ApplyTesterTheme(miDarkTheme.Checked);
        UpdateState();
    }

    /// <inheritdoc />
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateState();
    }

    /// <inheritdoc />
    protected override void OnMove(EventArgs e)
    {
        base.OnMove(e);
        UpdateState();
    }

    private void ApplyTesterTheme(bool dark)
    {
        var theme = dark ? TesterTheme.Dark : TesterTheme.Light;
        BackColor = theme.PanelBackColor;
        ForeColor = theme.PanelForeColor;
        DarkTitleBar = dark;
        txtInfo.BackColor = theme.BoxBackColor;
        txtInfo.ForeColor = theme.BoxForeColor;
        txtInfo.SetTheme(dark ? WindowsTheme.DarkExplorer : WindowsTheme.Explorer);
        btnToggle.FlatStyle = dark ? FlatStyle.Flat : FlatStyle.Standard;
        btnToggle.BackColor = dark ? theme.ButtonBackColor : SystemColors.Control;
        btnToggle.ForeColor = theme.ButtonForeColor;
        btnToggle.FlatAppearance.BorderColor = theme.BorderColor;
        btnToggle.UseVisualStyleBackColor = !dark;
        ExThemer.Apply(Controls, theme);
    }

    private void UpdateState()
    {
        if (lblState is null)
            return;
        var client = IsHandleCreated ? RectangleToScreen(ClientRectangle) : Rectangle.Empty;
        lblState.Text = string.Format(CultureInfo.CurrentCulture,
            "Stav: {0}, vlastné záhlavie: {1}{2}Okno: {3}{2}Klientska oblasť: {4}, monitor: {5}",
            WindowState, UsesCustomTitleBar, Environment.NewLine, Bounds, client, Screen.FromControl(this).Bounds);
    }

    private void MiExit_Click(object? sender, EventArgs e) => Close();

    private void MiMinimize_Click(object? sender, EventArgs e) => WindowState = FormWindowState.Minimized;

    private void MiMaximize_Click(object? sender, EventArgs e) =>
        WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;

    private void MiDarkTheme_Click(object? sender, EventArgs e) => ApplyTesterTheme(miDarkTheme.Checked);

    private void MiLongTitle_Click(object? sender, EventArgs e) => Text = miLongTitle.Checked ? LongTitle : ShortTitle;

    private void BtnToggle_Click(object? sender, EventArgs e)
    {
        titleBar.Visible = !titleBar.Visible;
        UpdateState();
    }

    private void miFile_Click(object sender, EventArgs e)
    {

    }
}
