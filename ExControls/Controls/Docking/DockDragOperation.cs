namespace ExControls.Docking;

/// <summary>
/// Tahanie skupiny (zahlavie) alebo panela (karta): navadzacie znacky, nahlad ciela, pri plavajucom okne jeho presun;
/// Escape tahanie zrusi. Presun vykona spravca az po pusteni mysi.
/// </summary>
internal sealed class DockDragOperation : IMessageFilter, IDisposable
{
    // ReSharper disable InconsistentNaming
    private const int WM_KEYDOWN = 0x0100;
    private const int VK_ESCAPE = 0x1B;
    // ReSharper restore InconsistentNaming

    private readonly ExDockManager _manager;
    private readonly DockFloatForm? _moving;
    private readonly Point _offset;
    private readonly Size _size;
    private readonly bool _canFloat;
    private readonly DockGuideForm _guides;
    private readonly DockPreviewForm _preview;
    private DockDropTarget? _target;
    private bool _done;

    /// <param name="manager">spravca</param>
    /// <param name="source">tahana skupina alebo skupina tahaneho panela</param>
    /// <param name="panel">tahany panel; <see langword="null" /> = cela skupina</param>
    /// <param name="moving">plavajuce okno, ktore sa taha s mysou</param>
    /// <param name="offset">kurzor voci lavemu hornemu rohu okna alebo pohladu</param>
    /// <param name="size">rozmery plavajuceho okna po pusteni mimo znaciek</param>
    /// <param name="canFloat">pustenie mimo znaciek vytvori plavajuce okno</param>
    public DockDragOperation(ExDockManager manager, DockGroup source, ExDockPanel? panel, DockFloatForm? moving, Point offset,
        Size size, bool canFloat)
    {
        _manager = manager;
        Source = source;
        Panel = panel;
        _moving = moving;
        _offset = offset;
        _size = size;
        _canFloat = canFloat;
        _preview = new DockPreviewForm(manager.Colors);
        _guides = new DockGuideForm(manager.Colors);
    }

    /// <summary>
    /// Tahana skupina alebo skupina tahaneho panela.
    /// </summary>
    public DockGroup Source { get; }

    /// <summary>
    /// Tahany panel; <see langword="null" /> = cela skupina.
    /// </summary>
    public ExDockPanel? Panel { get; }

    /// <summary>
    /// Zacne tahanie.
    /// </summary>
    public void Start(Point screen)
    {
        Application.AddMessageFilter(this);
        Move(screen);
    }

    /// <summary>
    /// Mys sa pohla: presunie plavajuce okno, obnovi znacky a nahlad.
    /// </summary>
    public void Move(Point screen)
    {
        if (_done)
            return;

        if (_moving is not null)
            _moving.Location = new Point(screen.X - _offset.X, screen.Y - _offset.Y);
        var guides = _manager.GuidesAt(screen, Source, Panel, _moving);
        var hit = DockGuides.HitTest(guides, screen);
        _target = hit?.Target ?? (_canFloat && _moving is null ? DockDropTarget.Floating : null);
        _preview.ShowAt(_target is null ? Rectangle.Empty : _manager.PreviewBounds(_target, Source, Panel, FloatBounds(screen)));
        _guides.SetGuides(guides, hit);
    }

    /// <summary>
    /// Mys sa pustila: zatvori znacky a presunie zdroj na ciel.
    /// </summary>
    public void Finish(Point screen)
    {
        if (_done)
            return;

        Move(screen);
        var target = _target;
        End();
        if (target is not null)
            _manager.Post(() => _manager.Drop(Source, Panel, target, FloatBounds(screen)));
    }

    /// <summary>
    /// Zrusi tahanie (Escape, strata zachytenia mysi); presunute plavajuce okno ostane na mieste.
    /// </summary>
    public void Cancel() => End();

    /// <inheritdoc />
    public void Dispose() => End();

    private Rectangle FloatBounds(Point screen) => new(screen.X - _offset.X, screen.Y - _offset.Y, _size.Width, _size.Height);

    private void End()
    {
        if (_done)
            return;
        _done = true;
        Application.RemoveMessageFilter(this);
        _guides.Dispose();
        _preview.Dispose();
    }

    /// <inheritdoc />
    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg != WM_KEYDOWN || (int)(long)m.WParam != VK_ESCAPE)
            return false;
        Cancel();
        return true;
    }
}
