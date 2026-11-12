namespace ExControls.Docking;

/// <summary>
/// Pohlad skupiny panelov: zahlavie s nazvom aktivneho panela a tlacidlami (ponuka, spendlik, skryt), obsah aktivneho
/// panela a pri viacerych paneloch karty dole ako v nastrojovych oknach Visual Studia. Tahanie zahlavia presuva celu
/// skupinu, tahanie karty jeden panel.
/// </summary>
internal sealed class DockGroupView : Control
{
    private readonly ExDockManager _manager;
    private readonly List<TabInfo> _tabs = [];
    private Rectangle _caption;
    private Rectangle _tabStrip;
    private Rectangle _content;
    private Rectangle _menuButton;
    private Rectangle _pinButton;
    private Rectangle _closeButton;
    private Hit _hover;
    private Hit _pressed;
    private Point _downPoint;
    private bool _dragArmed;
    private ExDockPanel? _dragPanel;
    private DockDragOperation? _drag;
    private DockViewMode _mode;

    /// <param name="manager">spravca skupiny</param>
    /// <param name="group">zobrazena skupina</param>
    public DockGroupView(ExDockManager manager, DockGroup group)
    {
        _manager = manager;
        Group = group;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.Selectable, false);
    }

    /// <summary>
    /// Zobrazena skupina.
    /// </summary>
    public DockGroup Group { get; }

    /// <summary>
    /// Kde je pohlad zobrazeny.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DockViewMode Mode
    {
        get => _mode;
        set
        {
            if (_mode == value)
                return;
            _mode = value;
            PerformLayout();
            Invalidate();
        }
    }

    /// <summary>
    /// Karty dole sa zobrazia pri viacerych zobrazenych paneloch (nie vo vysunutej zbalenej skupine).
    /// </summary>
    public bool ShowsTabs => Mode != DockViewMode.AutoHide && Group.Panels.Count(p => !p.IsClosed) > 1;

    /// <summary>
    /// Oblast obsahu aktivneho panela.
    /// </summary>
    internal Rectangle ContentBounds => _content;

    /// <summary>
    /// Zahlavie (testy a tahanie).
    /// </summary>
    internal Rectangle CaptionBounds => _caption;

    /// <summary>
    /// Karty v poradi (testy).
    /// </summary>
    internal IReadOnlyList<Rectangle> TabBounds => _tabs.Select(t => t.Bounds).ToList();

    /// <summary>
    /// Zosuladi detske prvky so skupinou: zobrazeni clenovia su detmi pohladu, viditelny je len aktivny.
    /// </summary>
    public void SyncPanels()
    {
        Group.FixActive();
        SuspendLayout();
        // najprv aktivny, aby fokus nepreskocil mimo skupiny
        if (Group.Active is { } active)
        {
            if (active.Parent != this)
                Controls.Add(active);
            active.Visible = true;
        }

        foreach (var panel in Group.Panels)
        {
            if (panel == Group.Active)
                continue;
            if (panel.IsClosed)
            {
                if (panel.Parent == this)
                    Controls.Remove(panel);
                continue;
            }

            if (panel.Parent != this)
                Controls.Add(panel);
            panel.Visible = false;
        }

        // panely, ktore uz nie su clenmi (presunute inam)
        foreach (var stray in Controls.OfType<ExDockPanel>().Where(p => p.Group != Group).ToList())
            Controls.Remove(stray);
        ResumeLayout(true);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        const int border = 1;
        var captionHeight = _manager.CaptionHeight;
        var tabHeight = ShowsTabs ? captionHeight : 0;
        var width = Math.Max(0, Width - 2 * border);
        _caption = new Rectangle(border, border, width, Math.Min(captionHeight, Math.Max(0, Height - 2 * border)));
        _tabStrip = new Rectangle(border, Math.Max(_caption.Bottom, Height - border - tabHeight), width, tabHeight);
        _content = new Rectangle(border, _caption.Bottom, width, Math.Max(0, _tabStrip.Y - _caption.Bottom));

        var button = captionHeight;
        var right = _caption.Right;
        _closeButton = Group.Active is { CanClose: true } ? new Rectangle(right -= button, _caption.Y, button, _caption.Height) : Rectangle.Empty;
        _pinButton = Mode != DockViewMode.Floating && Group.Active is { CanAutoHide: true }
            ? new Rectangle(right -= button, _caption.Y, button, _caption.Height)
            : Rectangle.Empty;
        _menuButton = new Rectangle(right - button, _caption.Y, button, _caption.Height);

        LayoutTabs();
        if (Group.Active is { } active)
            active.Bounds = _content;
    }

    private void LayoutTabs()
    {
        _tabs.Clear();
        if (!ShowsTabs)
            return;

        var padding = _manager.ToDevice(8);
        var visible = Group.Panels.Where(p => !p.IsClosed).ToList();
        var widths = visible.Select(p => TabWidth(p, padding)).ToList();
        var total = widths.Sum();
        var available = _tabStrip.Width;
        var x = _tabStrip.X;
        for (var i = 0; i < visible.Count; i++)
        {
            var width = total <= available ? widths[i] : Math.Max(_manager.ToDevice(24), widths[i] * available / Math.Max(1, total));
            _tabs.Add(new TabInfo(visible[i], new Rectangle(x, _tabStrip.Y, width, _tabStrip.Height)));
            x += width;
        }
    }

    private int TabWidth(ExDockPanel panel, int padding)
    {
        var text = TextRenderer.MeasureText(panel.Text, Font, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width;
        var image = panel.Image is null ? 0 : _manager.ToDevice(16) + _manager.ToDevice(4);
        return padding * 2 + image + text;
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        var colors = _manager.Colors;
        g.Clear(colors.Back);

        var focused = ContainsFocus;
        var captionBack = focused ? colors.ActiveBack : colors.Back;
        var captionFore = focused ? colors.ActiveFore : colors.Fore;
        using (var brush = new SolidBrush(captionBack))
            g.FillRectangle(brush, _caption);

        var titleRight = _menuButton.IsEmpty ? _caption.Right : _menuButton.X;
        var padding = _manager.ToDevice(6);
        var title = new Rectangle(_caption.X + padding, _caption.Y, Math.Max(0, titleRight - _caption.X - padding), _caption.Height);
        if (Mode == DockViewMode.AutoHide || !ShowsTabs)
            DrawImageAndText(g, Group.Active, title, captionFore, false);
        else
            TextRenderer.DrawText(g, Group.Active?.Text, Font, title, captionFore, TextFlags);

        DrawButton(g, _menuButton, HitKind.Menu, captionFore);
        DrawButton(g, _pinButton, HitKind.Pin, captionFore);
        DrawButton(g, _closeButton, HitKind.Close, captionFore);

        foreach (var tab in _tabs)
        {
            var selected = tab.Panel == Group.Active;
            var hover = _hover.Kind == HitKind.Tab && _hover.Panel == tab.Panel;
            if (selected || hover)
                using (var brush = new SolidBrush(selected ? colors.ButtonBack : DockColors.Hover(colors.Fore)))
                    g.FillRectangle(brush, tab.Bounds);
            DrawImageAndText(g, tab.Panel, Rectangle.Inflate(tab.Bounds, -_manager.ToDevice(8), 0), selected ? colors.ButtonFore : colors.Fore,
                true);
        }

        using var pen = new Pen(colors.Border);
        if (!_tabStrip.IsEmpty)
            g.DrawLine(pen, _tabStrip.X, _tabStrip.Y, _tabStrip.Right - 1, _tabStrip.Y);
        g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    private const TextFormatFlags TextFlags =
        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis |
        TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;

    private void DrawImageAndText(Graphics g, ExDockPanel? panel, Rectangle bounds, Color color, bool center)
    {
        if (panel is null)
            return;

        if (panel.Image is { } image)
        {
            var size = _manager.ToDevice(16);
            g.DrawImage(image, new Rectangle(bounds.X, bounds.Y + (bounds.Height - size) / 2, size, size));
            var shift = size + _manager.ToDevice(4);
            bounds = new Rectangle(bounds.X + shift, bounds.Y, Math.Max(0, bounds.Width - shift), bounds.Height);
        }

        TextRenderer.DrawText(g, panel.Text, Font, bounds, color, center ? TextFlags | TextFormatFlags.HorizontalCenter : TextFlags);
    }

    private void DrawButton(Graphics g, Rectangle bounds, HitKind kind, Color fore)
    {
        if (bounds.IsEmpty)
            return;

        var hover = _hover.Kind == kind;
        var pressed = _pressed.Kind == kind && hover;
        if (hover)
            using (var brush = new SolidBrush(Color.FromArgb(pressed ? 80 : 48, fore)))
                g.FillRectangle(brush, Rectangle.Inflate(bounds, -_manager.ToDevice(2), -_manager.ToDevice(2)));

        switch (kind)
        {
            case HitKind.Menu:
                DockPainter.DrawMenuArrow(g, bounds, fore, _manager.ToDevice(3));
                break;
            case HitKind.Pin:
                DockPainter.DrawPin(g, bounds, fore, _manager.ToDevice(10), Mode != DockViewMode.AutoHide);
                break;
            default:
                DockPainter.DrawClose(g, bounds, fore, _manager.ToDevice(8), Math.Max(1f, DeviceDpi / 96f));
                break;
        }
    }

    /// <summary>
    /// Cast pohladu pod bodom.
    /// </summary>
    internal Hit HitTest(Point point)
    {
        if (_closeButton.Contains(point))
            return new Hit(HitKind.Close, Group.Active);
        if (_pinButton.Contains(point))
            return new Hit(HitKind.Pin, Group.Active);
        if (_menuButton.Contains(point))
            return new Hit(HitKind.Menu, Group.Active);
        if (_caption.Contains(point))
            return new Hit(HitKind.Caption, Group.Active);
        foreach (var tab in _tabs)
            if (tab.Bounds.Contains(point))
                return new Hit(HitKind.Tab, tab.Panel);
        return default;
    }

    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        var hit = HitTest(e.Location);
        if (e.Button == MouseButtons.Right)
        {
            if (hit.Kind is HitKind.Caption or HitKind.Tab && hit.Panel is { } menuPanel)
            {
                _manager.SelectTab(Group, menuPanel, true);
                _manager.ShowMenu(this, menuPanel, e.Location);
            }

            return;
        }

        if (e.Button != MouseButtons.Left)
            return;

        switch (hit.Kind)
        {
            case HitKind.Menu when hit.Panel is { } panel:
                _manager.ShowMenu(this, panel, new Point(_menuButton.X, _menuButton.Bottom));
                break;
            case HitKind.Pin:
            case HitKind.Close:
                _pressed = hit;
                Invalidate();
                break;
            case HitKind.Tab when hit.Panel is { } panel:
                _manager.SelectTab(Group, panel, true);
                Arm(e.Location, panel);
                break;
            case HitKind.Caption:
                _manager.SelectTab(Group, Group.Active, true);
                Arm(e.Location, null);
                break;
        }
    }

    private void Arm(Point location, ExDockPanel? panel)
    {
        if (Mode == DockViewMode.AutoHide)
            return;
        _dragArmed = true;
        _downPoint = location;
        _dragPanel = panel;
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_drag is not null)
        {
            _drag.Move(MousePosition);
            return;
        }

        if (_dragArmed && e.Button == MouseButtons.Left)
        {
            var drag = SystemInformation.DragSize;
            if (Math.Abs(e.X - _downPoint.X) > drag.Width / 2 || Math.Abs(e.Y - _downPoint.Y) > drag.Height / 2)
            {
                _dragArmed = false;
                _drag = _manager.BeginDrag(this, _dragPanel, PointToScreen(_downPoint), MousePosition);
                return;
            }
        }

        SetHover(HitTest(e.Location));
    }

    /// <inheritdoc />
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _dragArmed = false;
        if (_drag is { } drag)
        {
            _drag = null;
            drag.Finish(MousePosition);
            return;
        }

        var pressed = _pressed;
        _pressed = default;
        Invalidate();
        if (e.Button != MouseButtons.Left || pressed.Kind == HitKind.None || HitTest(e.Location).Kind != pressed.Kind)
            return;

        if (pressed.Kind == HitKind.Close && pressed.Panel is { } panel)
            _manager.Post(() => _manager.ClosePanelByUser(panel));
        else if (pressed.Kind == HitKind.Pin)
            _manager.Post(() => _manager.ToggleAutoHide(Group));
    }

    /// <inheritdoc />
    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
        base.OnMouseDoubleClick(e);
        if (e.Button == MouseButtons.Left && HitTest(e.Location).Kind == HitKind.Caption && Mode != DockViewMode.AutoHide)
            _manager.Post(() => _manager.ToggleFloat(Group));
    }

    /// <inheritdoc />
    protected override void OnMouseCaptureChanged(EventArgs e)
    {
        base.OnMouseCaptureChanged(e);
        if (_drag is { } drag && !Capture)
        {
            _drag = null;
            drag.Cancel();
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        SetHover(default);
    }

    private void SetHover(Hit hit)
    {
        if (hit == _hover)
            return;
        _hover = hit;
        Invalidate();
        var strings = _manager.Strings;
        var tip = hit.Kind switch
        {
            HitKind.Close => strings.Hide,
            HitKind.Pin => Mode == DockViewMode.AutoHide ? strings.Dock : strings.AutoHide,
            HitKind.Menu => strings.Options,
            HitKind.Tab => hit.Panel?.Text,
            _ => null
        };
        _manager.ToolTip.SetToolTip(this, tip);
    }

    /// <inheritdoc />
    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        _manager.OnViewEnter(this);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        PerformLayout();
    }

    /// <summary>
    /// Cast pohladu.
    /// </summary>
    internal enum HitKind
    {
        None,
        Caption,
        Tab,
        Menu,
        Pin,
        Close
    }

    /// <summary>
    /// Cast pohladu pod mysou a jej panel.
    /// </summary>
    internal readonly record struct Hit(HitKind Kind, ExDockPanel? Panel);

    private readonly record struct TabInfo(ExDockPanel Panel, Rectangle Bounds);
}
