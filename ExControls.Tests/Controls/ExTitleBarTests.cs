using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ExControls.Tests.Controls;

/// <summary>
/// Okno s <see cref="ExTitleBar" />: bez systemoveho zahlavia, horny okraj meni velkost, volne miesto zahlavia a hlavnej
/// ponuky taha okno, polozky ponuky ostavaju klikatelne.
/// </summary>
[TestClass]
public class ExTitleBarTests
{
    private const int HtTransparent = -1;
    private const int HtClient = 1;
    private const int HtCaption = 2;
    private const int HtSysMenu = 3;
    private const int HtMinButton = 8;
    private const int HtMaxButton = 9;
    private const int HtTop = 12;
    private const int HtClose = 20;
    private const int HtHelp = 21;

    [TestMethod]
    public void HitTest_VolneMiestoAPopisSuZahlavie_PolozkyAPrvkySuKlientske()
    {
        using var window = new TestWindow();
        var file = window.Menu.Items[0];
        var label = window.Menu.Items[2];

        Assert.AreEqual(TitleBarHit.Client, window.TitleBar.HitTest(InTitleBar(window, file)));
        Assert.AreEqual(TitleBarHit.Caption, window.TitleBar.HitTest(InTitleBar(window, label)));
        Assert.AreEqual(TitleBarHit.Caption, window.TitleBar.HitTest(new Point(window.Menu.Right - 5, 16)));
        Assert.AreEqual(TitleBarHit.Client, window.TitleBar.HitTest(Center(window.Search.Bounds)));
        Assert.AreEqual(TitleBarHit.Caption, window.TitleBar.HitTest(window.Free));
    }

    [TestMethod]
    public void PanelNastrojovVZahlavi_MaPolozkyVedlaSeba()
    {
        using var window = new TestWindow();
        var items = window.Menu.Items;

        Assert.AreEqual(ToolStripLayoutStyle.HorizontalStackWithOverflow, window.Menu.LayoutStyle);
        Assert.AreEqual(items[0].Bounds.Top, items[1].Bounds.Top);
        Assert.IsTrue(items[1].Bounds.Left >= items[0].Bounds.Right);
    }

    [TestMethod]
    public void Rozlozenie_PrvkyMedziIkonouATlacidlamiOkna()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var bar = window.TitleBar;

        Assert.AreEqual(TitleBarHit.SystemMenu, bar.HitTest(new Point(10, 16)));
        Assert.AreEqual(TitleBarHit.MinimizeButton, bar.HitTest(new Point(bar.Width - 100, 16)));
        Assert.AreEqual(TitleBarHit.MaximizeButton, bar.HitTest(new Point(bar.Width - 50, 16)));
        Assert.AreEqual(TitleBarHit.CloseButton, bar.HitTest(new Point(bar.Width - 5, 16)));
        Assert.IsTrue(window.Menu.Left >= 36, "ponuka zacina za ikonou");
        Assert.IsTrue(window.Search.Right <= bar.Width - 3 * 46, "pole hladania konci pred tlacidlami okna");
    }

    [TestMethod]
    public void BezRamu_ZahlavieBezIkonyATlacidiel()
    {
        using var window = new TestWindow(FormBorderStyle.None);

        Assert.AreEqual(0, window.Menu.Left);
        Assert.AreEqual(window.TitleBar.ClientSize.Width, window.Search.Right);
        Assert.AreEqual(TitleBarHit.Client, window.TitleBar.HitTest(Center(window.Search.Bounds)));
    }

    [TestMethod]
    public void NcHitTest_IkonaATlacidlaOkna()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var bar = window.TitleBar;

        int At(int x) => NcHitTest(window.Form, bar.PointToScreen(new Point(x, bar.Height - 6)));

        Assert.AreEqual(HtSysMenu, At(10));
        Assert.AreEqual(HtMinButton, At(bar.Width - 100));
        Assert.AreEqual(HtMaxButton, At(bar.Width - 50));
        Assert.AreEqual(HtClose, At(bar.Width - 5));
    }

    [TestMethod]
    public void KlikNaZatvorenie_ZatvaraOkno()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var closing = 0;
        window.Form.FormClosing += (_, e) =>
        {
            closing++;
            e.Cancel = true;
        };

        NcClick(window.Form, HtClose, CloseButton(window));
        Application.DoEvents();

        Assert.AreEqual(1, closing);
    }

    [TestMethod]
    public void StlacenieAOdchodMysi_NieJeKlik()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var closing = 0;
        window.Form.FormClosing += (_, e) =>
        {
            closing++;
            e.Cancel = true;
        };
        var lParam = LParam(CloseButton(window));

        _ = Win32.SendMessage(window.Form.Handle, (uint)Win32.WM.NCLBUTTONDOWN, (IntPtr)HtClose, lParam);
        _ = Win32.SendMessage(window.Form.Handle, (uint)Win32.WM.NCMOUSELEAVE, IntPtr.Zero, IntPtr.Zero);
        _ = Win32.SendMessage(window.Form.Handle, (uint)Win32.WM.NCLBUTTONUP, (IntPtr)HtClose, lParam);
        Application.DoEvents();

        Assert.AreEqual(0, closing);
    }

    [TestMethod]
    public void KlikNaMinimalizaciu_MinimalizujeOkno()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var bar = window.TitleBar;

        NcClick(window.Form, HtMinButton, bar.PointToScreen(new Point(bar.Width - 100, 16)));
        Application.DoEvents();

        Assert.AreEqual(FormWindowState.Minimized, window.Form.WindowState);
    }

    [TestMethod]
    public void Pomocnik_LenBezMinimalizacieAMaximalizacie()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var bar = window.TitleBar;
        int At(int x) => NcHitTest(window.Form, bar.PointToScreen(new Point(x, bar.Height - 6)));

        window.Form.HelpButton = true;

        Assert.AreEqual(HtMaxButton, At(bar.Width - 50), "s minimalizaciou a maximalizaciou pomocnik nie je");

        window.Form.MinimizeBox = false;
        window.Form.MaximizeBox = false;

        Assert.AreEqual(HtHelp, At(bar.Width - 50));
        Assert.AreEqual(HtClose, At(bar.Width - 5));
        Assert.AreEqual(HtCaption, At(bar.Width - 100));
    }

    [TestMethod]
    public void KlikNaPomocnika_VyvolaHelpButtonClicked()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var bar = window.TitleBar;
        window.Form.MinimizeBox = false;
        window.Form.MaximizeBox = false;
        window.Form.HelpButton = true;
        var clicked = 0;
        window.Form.HelpButtonClicked += (_, e) =>
        {
            clicked++;
            // bez zrusenia by system cakal na klik v rezime kontextovej napovedy
            e.Cancel = true;
        };

        NcClick(window.Form, HtHelp, bar.PointToScreen(new Point(bar.Width - 50, 16)));
        Application.DoEvents();

        Assert.AreEqual(1, clicked);
    }

    [TestMethod]
    public void ApplyTheme_FarbyZahlaviaAjPrvkyVNom()
    {
        var themed = new List<Control>();
        ExThemer.Register<ThemedStrip>((strip, _) => themed.Add(strip));
        using var bar = new ExTitleBar();
        var menu = new ThemedStrip();
        bar.Controls.Add(menu);
        var theme = new ExTheme { PanelBackColor = Color.FromArgb(43, 45, 48), PanelForeColor = Color.Gainsboro };

        bar.ApplyTheme(theme);

        Assert.AreEqual(theme.PanelBackColor, bar.BackColor);
        Assert.AreEqual(theme.PanelForeColor, bar.ForeColor);
        CollectionAssert.Contains(themed, menu);
    }

    [TestMethod]
    [DataRow(nameof(ExTitleBar.ShowTitle), true)]
    [DataRow(nameof(ExTitleBar.InactiveForeColor), null)]
    [DataRow(nameof(ExTitleBar.ButtonHoverBackColor), null)]
    [DataRow(nameof(ExTitleBar.ButtonPressedBackColor), null)]
    [DataRow(nameof(ExTitleBar.CloseButtonHoverBackColor), null)]
    [DataRow(nameof(ExTitleBar.TopBorderColor), null)]
    public void Vlastnosti_NavrharVidiAtributy(string name, object? defaultValue)
    {
        // vynimka v konstruktore atributu (ExCategory, ExDescription) by v navrhari zahodila vsetky atributy clena
        var property = TypeDescriptor.GetProperties(typeof(ExTitleBar))[name];

        Assert.IsNotNull(property);
        var attribute = property.Attributes.OfType<DefaultValueAttribute>().Single();
        Assert.AreEqual(defaultValue ?? Color.Empty, attribute.Value);
        Assert.IsFalse(string.IsNullOrEmpty(property.Description));
        Assert.AreNotEqual(CategoryAttribute.Default.Category, property.Category);
    }

    [TestMethod]
    public void Navrhar_TypSaNajde()
    {
        var designer = TypeDescriptor.GetAttributes(typeof(ExTitleBar)).OfType<DesignerAttribute>()
            .Single(d => d.DesignerTypeName.StartsWith("ExControls.", StringComparison.Ordinal));

        Assert.AreEqual("ExControls.Designers.ExTitleBarDesigner, ExControls", designer.DesignerTypeName);
#if NETFRAMEWORK
        // na .NET navrhar zavisi od Microsoft.DotNet.DesignTools.Server, ktory je len v procese navrhara VS
        Assert.IsNotNull(Type.GetType(designer.DesignerTypeName, true));
#endif
    }

    [TestMethod]
    public void AutoSize_VyskaPodlaPrvkovAspon32()
    {
        using var window = new TestWindow();
        var bar = window.TitleBar;

        Assert.IsTrue(bar.AutoSize);
        Assert.AreEqual(32, bar.Height);

        window.Menu.Font = new Font("Segoe UI", 24f);
        window.Form.PerformLayout();

        Assert.IsTrue(bar.Height > 32);
        Assert.IsTrue(bar.Height >= window.Menu.GetPreferredSize(Size.Empty).Height);
    }

    [TestMethod]
    public void AutoSizeVypnute_PevnaVyska()
    {
        using var window = new TestWindow();
        window.TitleBar.AutoSize = false;
        window.TitleBar.Height = 40;

        window.Menu.Font = new Font("Segoe UI", 24f);
        window.Form.PerformLayout();

        Assert.AreEqual(40, window.TitleBar.Height);
    }

    [TestMethod]
    public void UseCompositedPainting_RiadiWsExComposited()
    {
        const int gwlExStyle = -20;
        const long wsExComposited = 0x02000000;
        using var form = new ExForm { UseCompositedPainting = false };

        bool Composited() => ((long)Win32.GetWindowLongPtr(form.Handle, gwlExStyle) & wsExComposited) != 0;

        Assert.IsFalse(Composited());
        form.UseCompositedPainting = true;
        Assert.IsTrue(Composited());
        form.UseCompositedPainting = false;
        Assert.IsFalse(Composited());
    }

    [TestMethod]
    public void NeaktivneOkno_StlmiPolozkyPonuky()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var file = window.Menu.Items[0];
        var label = window.Menu.Items[2];
        // okno mimo obrazovky nemusi byt aktivne - stav sa nastavi spravou
        NcActivate(window.Form, true);
        var original = file.ForeColor;

        NcActivate(window.Form, false);

        Assert.AreNotEqual(original, file.ForeColor);
        Assert.AreEqual(file.ForeColor, label.ForeColor);

        NcActivate(window.Form, true);

        Assert.AreEqual(original, file.ForeColor);
        Assert.IsFalse(TypeDescriptor.GetProperties(file)[nameof(ToolStripItem.ForeColor)]!.ShouldSerializeValue(file), "zdedena farba ostane zdedena");
    }

    [TestMethod]
    public void NeaktivneOkno_FarbuOdAplikacieAktivaciaNeprepise()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var file = window.Menu.Items[0];
        NcActivate(window.Form, true);

        NcActivate(window.Form, false);
        file.ForeColor = Color.Red;
        NcActivate(window.Form, true);

        Assert.AreEqual(Color.Red, file.ForeColor);
    }

    [TestMethod]
    public void VlastneZahlavie_KlientskaOblastZacinaNaVrchuOkna()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();

        Assert.IsTrue(window.Form.UsesCustomTitleBar);
        Assert.AreSame(window.TitleBar, window.Form.TitleBar);
        Assert.AreEqual(WindowRect(window.Form).Top, window.Form.PointToScreen(Point.Empty).Y);
    }

    [TestMethod]
    public void SkryteZahlavie_VratiSystemoveZahlavie()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();

        window.TitleBar.Visible = false;

        Assert.IsFalse(window.Form.UsesCustomTitleBar);
        Assert.IsTrue(window.Form.PointToScreen(Point.Empty).Y > WindowRect(window.Form).Top);

        window.TitleBar.Visible = true;

        Assert.AreEqual(WindowRect(window.Form).Top, window.Form.PointToScreen(Point.Empty).Y);
    }

    [TestMethod]
    public void BezZahlavia_OknoMaSystemoveZahlavie()
    {
        using var form = new ExForm();
        _ = form.Handle;

        Assert.IsNull(form.TitleBar);
        Assert.IsFalse(form.UsesCustomTitleBar);
        Assert.IsTrue(form.PointToScreen(Point.Empty).Y > WindowRect(form).Top);
    }

    [TestMethod]
    public void BezRamu_ZahlavieJeObycajnyPanel()
    {
        using var window = new TestWindow(FormBorderStyle.None);

        Assert.IsFalse(window.Form.UsesCustomTitleBar);
        Assert.AreEqual(WindowRect(window.Form).Top, window.Form.PointToScreen(Point.Empty).Y);
    }

    [TestMethod]
    [DataRow(FormBorderStyle.FixedSingle)]
    [DataRow(FormBorderStyle.Fixed3D)]
    [DataRow(FormBorderStyle.FixedDialog)]
    public void PevnyRam_VlastneZahlavieBezZmenyVyskyZhora(FormBorderStyle border)
    {
        RequireCustomTitleBar();
        using var window = new TestWindow(border);
        var form = window.Form;

        Assert.IsTrue(form.UsesCustomTitleBar);
        Assert.AreEqual(WindowRect(form).Top, form.PointToScreen(Point.Empty).Y);
        Assert.AreEqual(HtCaption, NcHitTest(form, form.PointToScreen(new Point(form.ClientSize.Width / 2, 1))));
        Assert.AreEqual(HtClose, NcHitTest(form, window.TitleBar.PointToScreen(new Point(window.TitleBar.Width - 5, 16))));
    }

    [TestMethod]
    [DataRow(FormBorderStyle.FixedToolWindow)]
    [DataRow(FormBorderStyle.SizableToolWindow)]
    public void NastrojoveOkno_LenZatvorenieBezIkony(FormBorderStyle border)
    {
        RequireCustomTitleBar();
        using var window = new TestWindow(border);
        var bar = window.TitleBar;

        Assert.IsTrue(window.Form.UsesCustomTitleBar);
        Assert.AreEqual(0, window.Menu.Left, "bez ikony");
        Assert.AreEqual(TitleBarHit.CloseButton, bar.HitTest(new Point(bar.Width - 5, 16)));
        Assert.AreEqual(bar.Width - 46, window.Search.Right, "bez minimalizacie a maximalizacie");
    }

    [TestMethod]
    public void Dialog_IkonaLenVlastna()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow(FormBorderStyle.FixedDialog);
        var bar = window.TitleBar;

        // predvolenu ikonu WinForms dialog v systemovom zahlavi neukazuje
        Assert.AreEqual(0, window.Menu.Left);

        window.Form.Icon = SystemIcons.Information;

        Assert.AreEqual(TitleBarHit.SystemMenu, bar.HitTest(new Point(10, 16)));
        Assert.IsTrue(window.Menu.Left >= 36);
    }

    [TestMethod]
    public void DarkTitleBarAFarbaTitulku_NezrusiaSa()
    {
        using var form = new ExForm { TitleBarBackColor = Color.Purple, DarkTitleBar = true };

        Assert.AreEqual(Color.Purple, form.TitleBarBackColor);

        form.TitleBarBackColor = Color.Orange;

        Assert.IsTrue(form.DarkTitleBar);
    }

    [TestMethod]
    public void FarbyTitulkuFormulara_KresliZahlavie()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var bar = window.TitleBar;

        window.Form.TitleBarBackColor = Color.FromArgb(255, 140, 0);
        window.Form.TitleBarForeColor = Color.FromArgb(200, 0, 0);
        // tmava tema aplikacie (ako v testeri) farbu titulku nezrusi
        window.Form.DarkTitleBar = true;
        using var bitmap = new Bitmap(bar.Width, bar.Height);
        bar.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bar.Size));

        // roh pod ikonou je pozadie zahlavia
        Assert.AreEqual(Color.FromArgb(255, 140, 0).ToArgb(), bitmap.GetPixel(2, 2).ToArgb());
        Assert.IsTrue(Pixels(bitmap, new Rectangle(bar.Width - 46, 0, 46, bar.Height)).Any(c => c.R > 150 && c is { G: < 80, B: < 80 }),
            "znak zatvorenia farbou textu titulku");
    }

    [TestMethod]
    public void NcHitTest_HornyOkrajZahlavieAObsah()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var form = window.Form;
        var middle = form.ClientSize.Width / 2;

        Assert.AreEqual(HtTop, NcHitTest(form, form.PointToScreen(new Point(middle, 1))));
        Assert.AreEqual(HtCaption, NcHitTest(form, form.PointToScreen(window.Free)));
        Assert.AreEqual(HtCaption, NcHitTest(form, form.PointToScreen(new Point(window.Menu.Right - 5, window.TitleBar.Height - 4))));
        Assert.AreEqual(HtClient, NcHitTest(form, form.PointToScreen(new Point(middle, window.TitleBar.Bottom + 20))));
    }

    [TestMethod]
    public void NcHitTest_PonukaPosielaVolneMiestoZahlaviu()
    {
        RequireCustomTitleBar();
        using var window = new TestWindow();
        var menu = window.Menu;

        Assert.AreEqual(HtTransparent, NcHitTest(menu, menu.PointToScreen(new Point(menu.Width - 5, menu.Height / 2))));
        Assert.AreEqual(HtTransparent, NcHitTest(menu, menu.PointToScreen(Center(menu.Items[2].Bounds))));
        Assert.AreEqual(HtClient, NcHitTest(menu, menu.PointToScreen(Center(menu.Items[0].Bounds))));
        Assert.AreEqual(HtTransparent, NcHitTest(window.TitleBar, window.TitleBar.PointToScreen(window.Free)));
    }

    [TestMethod]
    public void NcHitTest_BezVlastnehoZahlaviaPonukaOstavaKlientska()
    {
        using var window = new TestWindow(FormBorderStyle.None);
        var menu = window.Menu;

        Assert.AreEqual(HtClient, NcHitTest(menu, menu.PointToScreen(new Point(menu.Width - 5, menu.Height / 2))));
    }

    private static void RequireCustomTitleBar()
    {
        if (!ExTools.SupportsCustomTitleBar)
            Assert.Inconclusive("vlastne zahlavie je len vo Windows 10+");
    }

    private static Point InTitleBar(TestWindow window, ToolStripItem item) =>
        new(window.Menu.Left + item.Bounds.Left + item.Bounds.Width / 2, window.Menu.Top + item.Bounds.Top + item.Bounds.Height / 2);

    private static Point Center(Rectangle r) => new(r.Left + r.Width / 2, r.Top + r.Height / 2);

    private static IEnumerable<Color> Pixels(Bitmap bitmap, Rectangle area)
    {
        for (var y = area.Top; y < area.Bottom; y++)
            for (var x = area.Left; x < area.Right; x++)
                yield return bitmap.GetPixel(x, y);
    }

    private static Win32.RECT WindowRect(Control control)
    {
        Win32.GetWindowRect(new HandleRef(control, control.Handle), out var rect);
        return rect;
    }

    private static IntPtr LParam(Point screen) => (IntPtr)((screen.Y << 16) | (screen.X & 0xFFFF));

    private static int NcHitTest(Control control, Point screen) =>
        Win32.SendMessage(control.Handle, (uint)Win32.WM.NCHITTEST, IntPtr.Zero, LParam(screen));

    private static void NcClick(Control form, int hitCode, Point screen)
    {
        _ = Win32.SendMessage(form.Handle, (uint)Win32.WM.NCLBUTTONDOWN, (IntPtr)hitCode, LParam(screen));
        _ = Win32.SendMessage(form.Handle, (uint)Win32.WM.NCLBUTTONUP, (IntPtr)hitCode, LParam(screen));
    }

    private static void NcActivate(Control form, bool active) =>
        _ = Win32.SendMessage(form.Handle, (uint)Win32.WM.NCACTIVATE, (IntPtr)(active ? 1 : 0), IntPtr.Zero);

    private static Point CloseButton(TestWindow window) => window.TitleBar.PointToScreen(new Point(window.TitleBar.Width - 5, 20));

    private sealed class ThemedStrip : MenuStrip;

    /// <summary>
    /// Okno 600x400 so zahlavim (hlavna ponuka vlavo a pole hladania vpravo) a obsahom pod nim, zobrazene mimo obrazovky
    /// (skryty panel nastrojov nerozlozi polozky).
    /// </summary>
    private sealed class TestWindow : IDisposable
    {
        public TestWindow(FormBorderStyle border = FormBorderStyle.Sizable)
        {
            Form = new ExForm
            {
                FormBorderStyle = border,
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.Manual,
                Location = new Point(-20000, -20000),
                ShowInTaskbar = false
            };
            TitleBar = new ExTitleBar { Height = 32 };
            Menu = new MenuStrip { Dock = DockStyle.Left, AutoSize = false, Width = 300, GripStyle = ToolStripGripStyle.Hidden };
            Menu.Items.Add(new ToolStripMenuItem("Subor"));
            Menu.Items.Add(new ToolStripMenuItem("Upravy"));
            Menu.Items.Add(new ToolStripLabel("Stanica"));
            Search = new TextBox { Dock = DockStyle.Right, Width = 120 };
            TitleBar.Controls.Add(Menu);
            TitleBar.Controls.Add(Search);
            Form.Controls.Add(new Panel { Dock = DockStyle.Fill });
            Form.Controls.Add(TitleBar);

            Form.Show();
        }

        public ExForm Form { get; }

        public ExTitleBar TitleBar { get; }

        public MenuStrip Menu { get; }

        public TextBox Search { get; }

        /// <summary>
        /// Volne miesto zahlavia medzi ponukou a polom hladania.
        /// </summary>
        public Point Free => new((Menu.Right + Search.Left) / 2, TitleBar.Height / 2);

        public void Dispose() => Form.Dispose();
    }
}
