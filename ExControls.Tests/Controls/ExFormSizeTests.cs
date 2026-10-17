using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;

namespace ExControls.Tests.Controls;

/// <summary>
/// Rozmery <see cref="ExForm" /> s vlastnym zahlavim: navrhar ukazuje okno bez titulku a ramu, za behu ma okno ram len
/// vlavo, vpravo a dole - klientska oblast ulozena navrharom je v oboch rovnaka a nacitanie a ulozenie ju nemeni. Navrhova
/// plocha sa vo vlakne MTA nacitava desiatky sekund, preto STA.
/// </summary>
[STATestClass]
public class ExFormSizeTests
{
    private static readonly Size Designed = new(400, 300);

    [TestMethod]
    [DataRow(FormBorderStyle.Sizable)]
    [DataRow(FormBorderStyle.FixedSingle)]
    [DataRow(FormBorderStyle.Fixed3D)]
    [DataRow(FormBorderStyle.FixedDialog)]
    [DataRow(FormBorderStyle.FixedToolWindow)]
    [DataRow(FormBorderStyle.SizableToolWindow)]
    public void Beh_ClientSizeZNavrharaOstavaAjPoZobrazeni(FormBorderStyle border)
    {
        RequireCustomTitleBar();
        using var form = RuntimeForm(Designed, CurrentDimensions(), border);

        Assert.AreEqual(Designed, form.ClientSize, "odhad pred vytvorenim okna");

        form.Show();

        Assert.IsTrue(form.UsesCustomTitleBar);
        Assert.AreEqual(Designed, form.ClientSize);
        AssertCustomFrame(form);
    }

    [TestMethod]
    public void Beh_UkotvenyPrvokSaPoZobrazeniNeposunie()
    {
        RequireCustomTitleBar();
        var button = new Button { Bounds = new Rectangle(Designed.Width - 90, Designed.Height - 40, 80, 30), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
        using var form = RuntimeForm(Designed, CurrentDimensions(), FormBorderStyle.Sizable, button);
        var location = button.Location;

        form.Show();

        Assert.AreEqual(location, button.Location);
    }

    [TestMethod]
    public void Beh_VelkostZKoduOstava()
    {
        RequireCustomTitleBar();
        using var form = OffScreen(new ExForm { Size = new Size(600, 400) });
        form.Controls.Add(new ExTitleBar());

        Assert.AreEqual(new Size(600, 400), form.Size, "pred zobrazenim");

        form.Show();

        Assert.AreEqual(new Size(600, 400), form.Size);
        AssertCustomFrame(form);

        form.Size = new Size(700, 500);

        Assert.AreEqual(new Size(700, 500), form.Size);
        AssertCustomFrame(form);
    }

    [TestMethod]
    public void Beh_ClientSizeZKodu_PlatiAjPoZobrazeni()
    {
        RequireCustomTitleBar();
        using var form = RuntimeForm(Designed, CurrentDimensions());
        form.Show();

        form.ClientSize = new Size(500, 350);

        Assert.AreEqual(new Size(500, 350), form.ClientSize);
        AssertCustomFrame(form);
    }

    [TestMethod]
    public void Beh_SkalovaniePodlaDpi_ZvacsiKlientskuOblast()
    {
        RequireCustomTitleBar();
        using var form = RuntimeForm(Designed, HalfDimensions());

        Assert.AreEqual(Scaled(2), form.ClientSize, "pred zobrazenim");

        form.Show();

        Assert.AreEqual(Scaled(2), form.ClientSize);
        AssertCustomFrame(form);

        form.Scale(new SizeF(0.5f, 0.5f));

        Assert.AreEqual(Designed, form.ClientSize);
    }

    [TestMethod]
    public void Beh_SkryteZahlavie_RozmeryAkoBezNeho()
    {
        using var form = RuntimeForm(Designed, CurrentDimensions());
        using var plain = OffScreen(new Form { ClientSize = Designed });

        form.TitleBar!.Visible = false;
        form.Show();
        plain.Show();

        Assert.IsFalse(form.UsesCustomTitleBar);
        Assert.AreEqual(Designed, form.ClientSize);
        Assert.AreEqual(plain.Size, form.Size);
    }

    [TestMethod]
    public void Beh_BezZahlavia_RozmeryAkoForm()
    {
        using var form = OffScreen(new ExForm { ClientSize = Designed });
        using var plain = OffScreen(new Form { ClientSize = Designed });

        form.Show();
        plain.Show();

        Assert.AreEqual(plain.Size, form.Size);
        Assert.AreEqual(Designed, form.ClientSize);
    }

    [TestMethod]
    [DataRow(FormBorderStyle.Sizable)]
    [DataRow(FormBorderStyle.Fixed3D)]
    [DataRow(FormBorderStyle.FixedDialog)]
    [DataRow(FormBorderStyle.FixedToolWindow)]
    public void Navrhar_OknoBezTitulkuARamu(FormBorderStyle border)
    {
        RequireCustomTitleBar();
        using var designer = new DesignedForm(Designed, CurrentDimensions(), border: border);
        var form = designer.Form;

        Assert.IsTrue(form.IsHandleCreated);
        Assert.IsFalse(form.UsesCustomTitleBar, "vlastne zahlavie (ram) je len za behu");
        Assert.AreEqual(Designed, form.ClientSize);
        Assert.AreEqual(Designed, form.Size);
        Assert.AreEqual(form.PointToScreen(Point.Empty), new Point(WindowRect(form).Left, WindowRect(form).Top));
        Assert.AreEqual(Designed, designer.SavedClientSize);
    }

    [TestMethod]
    public void Navrhar_NacitanieAUlozenie_RozmerySaNemenia()
    {
        RequireCustomTitleBar();
        var saved = Designed;

        for (var i = 0; i < 3; i++)
        {
            using var designer = new DesignedForm(saved, CurrentDimensions());
            saved = designer.SavedClientSize;
        }

        Assert.AreEqual(Designed, saved);
    }

    [TestMethod]
    public void Navrhar_SkalovaniePodlaDpi_RovnakeAkoZaBehu()
    {
        RequireCustomTitleBar();
        Size saved;
        using (var designer = new DesignedForm(Designed, HalfDimensions()))
            saved = designer.SavedClientSize;
        using var form = RuntimeForm(Designed, HalfDimensions());
        form.Show();

        Assert.AreEqual(Scaled(2), saved);
        Assert.AreEqual(saved, form.ClientSize);

        // ulozene s novymi rozmermi pisma sa pri dalsom nacitani uz neskaluje
        using var reloaded = new DesignedForm(saved, CurrentDimensions());
        Assert.AreEqual(saved, reloaded.SavedClientSize);
    }

    [TestMethod]
    public void Navrhar_PridanieAOdobratieZahlavia_ZachovaClientSize()
    {
        RequireCustomTitleBar();
        using var designer = new DesignedForm(Designed, CurrentDimensions(), withTitleBar: false);
        var form = designer.Form;

        Assert.IsTrue(form.Height > Designed.Height, "bez zahlavia ma okno systemovy titulok");

        var titleBar = new ExTitleBar();
        form.Controls.Add(titleBar);

        Assert.AreEqual(Designed, form.ClientSize);
        Assert.AreEqual(Designed, form.Size);

        form.Controls.Remove(titleBar);
        titleBar.Dispose();

        Assert.AreEqual(Designed, form.ClientSize);
        Assert.IsTrue(form.Height > Designed.Height);
    }

    private static void RequireCustomTitleBar()
    {
        if (!ExTools.SupportsCustomTitleBar)
            Assert.Inconclusive("vlastne zahlavie je len vo Windows 10+");
    }

    private static Size Scaled(int factor) => new(Designed.Width * factor, Designed.Height * factor);

    /// <summary>
    /// Rozmery pisma, s ktorymi sa formular pri nacitani neskaluje.
    /// </summary>
    private static SizeF CurrentDimensions()
    {
        using var probe = new Form { AutoScaleMode = AutoScaleMode.Font };
        return probe.CurrentAutoScaleDimensions;
    }

    /// <summary>
    /// Rozmery pisma navrhnute pri polovicnom DPI - formular sa pri nacitani zvacsi dvakrat.
    /// </summary>
    private static SizeF HalfDimensions()
    {
        var current = CurrentDimensions();
        return new SizeF(current.Width / 2, current.Height / 2);
    }

    /// <summary>
    /// Ram vlastneho zahlavia: klientska oblast zacina na vrchu okna, ram dole je rovnaky ako po stranach.
    /// </summary>
    private static void AssertCustomFrame(Form form)
    {
        var window = WindowRect(form);
        var client = form.RectangleToScreen(form.ClientRectangle);

        Assert.AreEqual(form.Size, new Size(window.Right - window.Left, window.Bottom - window.Top));
        Assert.AreEqual(window.Top, client.Top);
        Assert.AreEqual(client.Left - window.Left, window.Bottom - client.Bottom);
        Assert.AreEqual(client.Left - window.Left, window.Right - client.Right);
    }

    private static Win32.RECT WindowRect(Control control)
    {
        Win32.GetWindowRect(new HandleRef(control, control.Handle), out var rect);
        return rect;
    }

    private static T OffScreen<T>(T form) where T : Form
    {
        form.StartPosition = FormStartPosition.Manual;
        form.Location = new Point(-20000, -20000);
        form.ShowInTaskbar = false;
        return form;
    }

    /// <summary>
    /// Formular za behu zostaveny ako v InitializeComponent.
    /// </summary>
    private static ExForm RuntimeForm(Size clientSize, SizeF autoScaleDimensions, FormBorderStyle border = FormBorderStyle.Sizable,
        params Control[] controls)
    {
        var form = OffScreen(new ExForm());
        InitializeComponent(form, clientSize, autoScaleDimensions, true, border, controls);
        return form;
    }

    /// <summary>
    /// Poradie prikazov z InitializeComponent: rozmery pisma, ClientSize, prvky (zahlavie nakoniec, je navrchu), potom ram.
    /// </summary>
    private static void InitializeComponent(ExForm form, Size clientSize, SizeF autoScaleDimensions, bool withTitleBar, FormBorderStyle border,
        Control[] controls)
    {
        form.SuspendLayout();
        form.AutoScaleDimensions = autoScaleDimensions;
        form.AutoScaleMode = AutoScaleMode.Font;
        form.ClientSize = clientSize;
        form.Controls.Add(new Panel { Dock = DockStyle.Fill });
        form.Controls.AddRange(controls);
        if (withTitleBar)
            form.Controls.Add(new ExTitleBar());
        form.FormBorderStyle = border;
        form.ResumeLayout(false);
        form.PerformLayout();
    }

    /// <summary>
    /// Formular nacitany na navrhovej ploche (ako navrhar Visual Studia) a zobrazeny mimo obrazovky.
    /// </summary>
    private sealed class DesignedForm : IDisposable
    {
        private readonly DesignSurface _surface = new();
        private readonly Form _window;

        public DesignedForm(Size clientSize, SizeF autoScaleDimensions, bool withTitleBar = true, FormBorderStyle border = FormBorderStyle.Sizable)
        {
            _surface.BeginLoad(new Loader(clientSize, autoScaleDimensions, withTitleBar, border));
            if (!_surface.IsLoaded)
                throw new InvalidOperationException(string.Join("; ", _surface.LoadErrors.Cast<object>()));

            var host = (IDesignerHost)_surface.GetService(typeof(IDesignerHost))!;
            Form = (ExForm)host.RootComponent;
            var view = (Control)_surface.View;
            view.Dock = DockStyle.Fill;
            _window = OffScreen(new Form { Size = new Size(1600, 1200) });
            _window.Controls.Add(view);
            _window.Show();
        }

        public ExForm Form { get; }

        /// <summary>
        /// ClientSize, ktoru navrhar zapise do InitializeComponent (vlastnost upravena navrharom formulara).
        /// </summary>
        public Size SavedClientSize => (Size)TypeDescriptor.GetProperties(Form)[nameof(Form.ClientSize)]!.GetValue(Form)!;

        public void Dispose()
        {
            _window.Dispose();
            _surface.Dispose();
        }

        private sealed class Loader(Size clientSize, SizeF autoScaleDimensions, bool withTitleBar, FormBorderStyle border) : BasicDesignerLoader
        {
            protected override void PerformLoad(IDesignerSerializationManager serializationManager)
            {
                var form = (ExForm)LoaderHost.CreateComponent(typeof(ExForm), "Form1");
                InitializeComponent(form, clientSize, autoScaleDimensions, withTitleBar, border, []);
            }

            protected override void PerformFlush(IDesignerSerializationManager serializationManager)
            {
            }
        }
    }
}
