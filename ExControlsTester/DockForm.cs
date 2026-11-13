namespace ExControls.Test;

/// <summary>
/// Test of <see cref="ExDockManager" />: document with a tree on the left, properties and a collapsed search on the right and
/// a list of findings with an output tab at the bottom (like the Error List in Visual Studio); theme, saving and restoring
/// the layout and showing hidden panels from the menu.
/// </summary>
public sealed class DockForm : Form
{
    private readonly ExDockManager _dock = new() { Dock = DockStyle.Fill };
    private readonly MenuStrip _menu = new();
    private readonly ToolStripMenuItem _miPanels = new("&Panely");
    private readonly ToolStripMenuItem _miDarkTheme = new("&Tmavá téma") { CheckOnClick = true, Checked = true };
    private readonly ExListBox _findings = new() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.None, IntegralHeight = false };
    private string? _savedLayout;

    public DockForm()
    {
        TesterTheme.Register();
        Text = "ExControls – dock manager";
        ClientSize = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;

        _dock.Strings = new ExDockStrings
        {
            Hide = "Skryť",
            AutoHide = "Automaticky skryť",
            FloatWindow = "Plávajúce okno",
            Dock = "Ukotviť",
            Options = "Možnosti okna"
        };
        _dock.Document.Controls.Add(new ExTextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            Text = string.Join(Environment.NewLine,
                "Dokument (hlavná oblasť). Čo vyskúšať:",
                "",
                "• Rozdeľovače medzi panelmi a dokumentom aj medzi skupinami zóny menia veľkosť.",
                "• Karty dole (Kontrola inštalácie, Výstup) prepínajú panely skupiny; ťahanie karty presúva jeden panel.",
                "• Ťahanie záhlavia presúva celú skupinu: kompas nad skupinou = karta (stred) alebo rozdelenie skupiny (šípky),",
                "  kompas nad dokumentom = rozdelenie dokumentu, značky pri okrajoch = stĺpec či riadok na okraji zóny,",
                "  pustenie mimo značiek = plávajúce okno, Escape zruší ťahanie.",
                "• Dvojklik na záhlavie = plávajúce okno a späť; plávajúce okno sa ťahá za záhlavie a mení veľkosť za okraj.",
                "• Špendlík zbalí skupinu na lištu pri okraji; klik na tlačidlo lišty ju vysunie, zasunie sa po odchode myši a fokusu.",
                "• Krížik skryje panel, ponuka Panely ho vráti na to isté miesto.",
                "• Šípka v záhlaví a pravý klik na záhlavie či kartu otvárajú ponuku okna.",
                "• Okno → Uložiť rozloženie / Obnoviť rozloženie, Tmavá téma.")
        });

        var tree = new ExTreeView { Dock = DockStyle.Fill, BorderStyle = BorderStyle.None };
        var station = tree.Nodes.Add("Košice");
        station.Nodes.Add("Grafikon 2026");
        station.Nodes.Add("Stavový diagram");
        station.Nodes.Add("Tabule");
        station.Expand();
        tree.Nodes.Add("Prešov").Nodes.Add("Grafikon 2026");

        _findings.Items.AddRange([
            "Chyba      Grafikon 2026   Vlak Os 7302: stanica Kysak nie je v zozname staníc",
            "Chyba      Tabule          Tabuľa 3 nemá linku",
            "Varovanie  Grafikon 2026   Vlak R 611: kolízia koľaje 2 s vlakom R 613",
            "Varovanie  Stavový diagram Uzol Odjede: prázdna podmienka automatiky",
            "Info       Server          Overenie kódom servera nebolo spustené"
        ]);
        var output = new ExTextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            Text = "Kontrola inštalácie: 2 chyby, 2 varovania, 1 informácia."
        };
        var properties = new ExPropertyGrid { Dock = DockStyle.Fill, SelectedObject = _dock };
        var search = new ExTextBox { Dock = DockStyle.Top, HintText = "Hľadať" };

        AddPanel("tree", "Stanice", tree, ExDockSide.Left, null);
        AddPanel("analysis", "Kontrola inštalácie", _findings, ExDockSide.Bottom, null);
        AddPanel("output", "Výstup", output, ExDockSide.Bottom, "analysis");
        AddPanel("props", "Vlastnosti", properties, ExDockSide.Right, null);
        AddPanel("find", "Hľadanie", search, ExDockSide.Right, null);
        _dock.SetAutoHide(Panel("find"), true);
        _dock.SetZoneSize(ExDockSide.Bottom, 170);
        _dock.PanelStateChanged += (_, _) => UpdatePanelItems();

        var miWindow = new ToolStripMenuItem("&Okno");
        miWindow.DropDownItems.Add(_miDarkTheme);
        miWindow.DropDownItems.Add(new ToolStripSeparator());
        miWindow.DropDownItems.Add("&Uložiť rozloženie", null, (_, _) => _savedLayout = _dock.SaveLayout());
        miWindow.DropDownItems.Add("&Obnoviť rozloženie", null, (_, _) => _dock.LoadLayout(_savedLayout));
        _miDarkTheme.Click += (_, _) => ApplyTesterTheme();
        _menu.Items.AddRange([miWindow, _miPanels]);

        Controls.Add(_dock);
        Controls.Add(_menu);
        MainMenuStrip = _menu;
        UpdatePanelItems();
        ApplyTesterTheme();
    }

    /// <summary>
    /// Dock manager of the window (screenshot harness).
    /// </summary>
    public ExDockManager DockManager => _dock;

    private void AddPanel(string key, string text, Control content, ExDockSide side, string? tabWith)
    {
        var panel = new ExDockPanel(key, text);
        panel.Controls.Add(content);
        if (tabWith is null)
            _dock.AddPanel(panel, side);
        else
            _dock.AddPanel(panel, Panel(tabWith));

        var item = new ToolStripMenuItem(text) { Tag = panel };
        item.Click += (_, _) =>
        {
            if (panel.IsShown)
                _dock.HidePanel(panel);
            else
                _dock.ShowPanel(panel);
        };
        _miPanels.DropDownItems.Add(item);
    }

    private ExDockPanel Panel(string key) => _dock.Panels.First(p => p.Key == key);

    private void UpdatePanelItems()
    {
        foreach (ToolStripMenuItem item in _miPanels.DropDownItems)
            item.Checked = ((ExDockPanel)item.Tag!).IsShown;
    }

    private void ApplyTesterTheme()
    {
        var theme = _miDarkTheme.Checked ? TesterTheme.Dark : TesterTheme.Light;
        BackColor = theme.PanelBackColor;
        ForeColor = theme.PanelForeColor;
        ExThemer.Apply(Controls, theme);
        // ExListBox nema vlastnu temu (v programoch ho styluje obsluha ListBox z ToolsCore)
        _findings.BackColor = theme.BoxBackColor;
        _findings.ForeColor = theme.BoxForeColor;
    }
}
