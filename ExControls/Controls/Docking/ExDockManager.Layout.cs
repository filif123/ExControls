using ExControls.Docking;

namespace ExControls;

// rozlozenie, listy auto-hide, rozdelovace a kreslenie spravcu
public partial class ExDockManager
{
    // ReSharper disable InconsistentNaming
    private const int WM_SETCURSOR = 0x0020;
    private const int HTCLIENT = 1;
    // ReSharper restore InconsistentNaming

    private readonly List<StripButton> _stripButtons = [];
    private DockLayoutResult _layout = new();
    private StripButton? _stripHover;
    private DockSplitterInfo? _split;
    private Point _splitStart;
    private int _splitInitial;
    private int _splitPair;

    /// <summary>
    /// Hrubka rozdelovaca (body zariadenia).
    /// </summary>
    internal int SplitterSize => ToDevice(4);

    /// <summary>
    /// Vyska zahlavia a kariet skupiny (body zariadenia).
    /// </summary>
    internal int CaptionHeight => FontHeight + ToDevice(8);

    /// <summary>
    /// Hrubka listy auto-hide (body zariadenia).
    /// </summary>
    internal int StripThickness => FontHeight + ToDevice(8);

    private int MinZoneSize => ToDevice(40);

    private int MinDocumentSize => ToDevice(60);

    /// <summary>
    /// Posledne vypocitane rozlozenie (testy, nahlad ciela).
    /// </summary>
    internal DockLayoutResult LayoutResult => _layout;

    /// <summary>
    /// Tlacidla list auto-hide (testy).
    /// </summary>
    internal IReadOnlyList<StripButton> StripButtons => _stripButtons;

    /// <summary>
    /// Vstup rozlozenia podla modelu (ukotvene skupiny, zbalene skupiny na listach).
    /// </summary>
    private DockLayoutInput BuildInput()
    {
        var input = new DockLayoutInput
        {
            Client = ClientRectangle,
            Splitter = SplitterSize,
            MinDocument = MinDocumentSize,
            MinZone = MinZoneSize
        };
        foreach (var zone in _zones)
        {
            var index = (int)zone.Side;
            input.ZoneSizes[index] = ToDevice(zone.Size);
            input.Roots[index] = zone.Root;
            input.Strips[index] = AutoHideGroupsOf(zone.Side).Count > 0 ? StripThickness : 0;
        }

        input.Center = _center.Root;
        input.Document = _center.Document;

        return input;
    }

    /// <inheritdoc />
    protected override void OnLayout(LayoutEventArgs levent)
    {
        base.OnLayout(levent);
        _layout = DockLayoutEngine.Compute(BuildInput());
        _document.Bounds = _layout.Document;
        foreach (var group in AllZones.SelectMany(z => z.DockedGroups))
        {
            if (group.View is not { } view || view.Parent != this)
                continue;
            var rect = _layout.BoundsOf(group);
            view.Bounds = rect;
            view.Visible = rect.Width > 0 && rect.Height > 0;
        }

        LayoutStrips();
        LayoutPopup();
        Invalidate();
    }

    private void LayoutStrips()
    {
        _stripButtons.Clear();
        var gap = ToDevice(2);
        foreach (var zone in _zones)
        {
            var strip = _layout.Strips[(int)zone.Side];
            if (strip.IsEmpty)
                continue;

            var vertical = DockLayoutEngine.IsHorizontalSide(zone.Side);
            var position = (vertical ? strip.Y : strip.X) + gap;
            foreach (var group in AutoHideGroupsOf(zone.Side))
            {
                foreach (var panel in group.VisiblePanels)
                {
                    var length = StripButtonLength(panel, vertical);
                    var bounds = vertical ? new Rectangle(strip.X, position, strip.Width, length) : new Rectangle(position, strip.Y, length, strip.Height);
                    _stripButtons.Add(new StripButton(zone.Side, group, panel, bounds));
                    position += length + gap;
                }

                position += ToDevice(8);
            }
        }
    }

    private int StripButtonLength(ExDockPanel panel, bool vertical)
    {
        var text = DockPainter.MeasureStripText(panel.Text, Font, vertical);
        var image = panel.Image is null ? 0 : ToDevice(16) + ToDevice(4);
        return text + image + 2 * ToDevice(8);
    }

    private void LayoutPopup()
    {
        if (_popup.Group is not { Zone: not null } group)
            return;

        // skupina pri dokumente sa vysuva zo strany, na ktorej je, v hrubke zony tej strany
        var side = group.Side;
        var inner = _layout.Inner;
        var available = DockLayoutEngine.Thickness(inner, side) - ToDevice(20);
        var requested = ToDevice(group.AutoHideSize > 0 ? group.AutoHideSize : Zone(side).Size);
        var thickness = Math.Max(Math.Min(requested, available), Math.Min(MinZoneSize, available));
        if (thickness <= 0)
        {
            HidePopup();
            return;
        }

        var area = inner;
        _popup.Bounds = DockLayoutEngine.Take(ref area, side, thickness);
    }

    /// <summary>
    /// Vysunuta skupina meni hrubku tahanim okraja.
    /// </summary>
    internal void ResizeAutoHide(int thickness)
    {
        if (_popup.Group is not { } group)
            return;
        group.AutoHideSize = ToLogical(Math.Max(MinZoneSize, thickness));
        LayoutPopup();
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Colors.Back);
        foreach (var button in _stripButtons)
            DrawStripButton(g, button);
    }

    private void DrawStripButton(Graphics g, StripButton button)
    {
        var open = _popup.Visible && _popup.Group == button.Group && button.Group.Active == button.Panel;
        var hover = _stripHover is { } h && h.Panel == button.Panel;
        var bounds = button.Bounds;
        var fore = open ? Colors.ActiveFore : Colors.Fore;
        if (open || hover)
            using (var brush = new SolidBrush(open ? Colors.ActiveBack : DockColors.Hover(Colors.Fore)))
                g.FillRectangle(brush, bounds);

        // pas pri vonkajsom okraji ako zalozka
        var mark = ToDevice(2);
        var bar = button.Side switch
        {
            ExDockSide.Left => new Rectangle(bounds.X, bounds.Y, mark, bounds.Height),
            ExDockSide.Right => new Rectangle(bounds.Right - mark, bounds.Y, mark, bounds.Height),
            ExDockSide.Top => new Rectangle(bounds.X, bounds.Y, bounds.Width, mark),
            _ => new Rectangle(bounds.X, bounds.Bottom - mark, bounds.Width, mark)
        };
        using (var brush = new SolidBrush(open ? Colors.ActiveFore : Colors.Border))
            g.FillRectangle(brush, bar);

        var vertical = DockLayoutEngine.IsHorizontalSide(button.Side);
        var content = Rectangle.Inflate(bounds, vertical ? 0 : -ToDevice(8), vertical ? -ToDevice(8) : 0);
        if (button.Panel.Image is { } image)
        {
            var size = ToDevice(16);
            var imageBounds = vertical
                ? new Rectangle(content.X + (content.Width - size) / 2, content.Y, size, size)
                : new Rectangle(content.X, content.Y + (content.Height - size) / 2, size, size);
            g.DrawImage(image, imageBounds);
            var shift = size + ToDevice(4);
            content = vertical
                ? new Rectangle(content.X, content.Y + shift, content.Width, Math.Max(0, content.Height - shift))
                : new Rectangle(content.X + shift, content.Y, Math.Max(0, content.Width - shift), content.Height);
        }

        DockPainter.DrawStripText(g, button.Panel.Text, Font, content, fore, vertical);
    }

    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left)
            return;

        if (_layout.HitSplitter(e.Location) is { } hit)
        {
            StartSplit(hit, e.Location);
            return;
        }

        if (StripButtonAt(e.Location) is { } button)
        {
            if (_popup.Visible && _popup.Group == button.Group && button.Group.Active == button.Panel)
                HidePopup();
            else
                ShowPopup(button.Group, button.Panel, true);
        }
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_split is { } split)
        {
            DragSplit(split, e.Location);
            return;
        }

        SetStripHover(_layout.HitSplitter(e.Location) is null ? StripButtonAt(e.Location) : null);
    }

    /// <inheritdoc />
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _split = null;
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        SetStripHover(null);
    }

    /// <inheritdoc />
    protected override void OnMouseCaptureChanged(EventArgs e)
    {
        base.OnMouseCaptureChanged(e);
        if (!Capture)
            _split = null;
    }

    private StripButton? StripButtonAt(Point point)
    {
        foreach (var button in _stripButtons)
            if (button.Bounds.Contains(point))
                return button;
        return null;
    }

    private void SetStripHover(StripButton? button)
    {
        if (_stripHover?.Panel == button?.Panel)
            return;
        _stripHover = button;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        // kurzor rozdelovaca len nad plochou spravcu (nie nad panelmi) - vlastnost Cursor by zdedili vsetky vnorene prvky
        if (m.Msg == WM_SETCURSOR && m.WParam == Handle && ((int)(long)m.LParam & 0xFFFF) == HTCLIENT
            && SplitterCursor(PointToClient(MousePosition)) is { } cursor)
        {
            Cursor.Current = cursor;
            m.Result = (IntPtr)1;
            return;
        }

        base.WndProc(ref m);
    }

    /// <summary>
    /// Kurzor nad bodom spravcu: rozdelovac pod mysou alebo prave tahany; inak <see langword="null" /> (predvoleny).
    /// </summary>
    internal Cursor? SplitterCursor(Point point) =>
        (_split ?? _layout.HitSplitter(point)) is { } hit ? hit.IsVertical ? Cursors.VSplit : Cursors.HSplit : null;

    private void StartSplit(DockSplitterInfo hit, Point location)
    {
        _split = hit;
        Cursor.Current = SplitterCursor(location);
        _splitStart = location;
        if (hit.IsZone)
        {
            _splitInitial = DockLayoutEngine.Thickness(_layout.Zones[(int)hit.Side], hit.Side);
            return;
        }

        int Length(DockNode? node)
        {
            var bounds = node is null ? Rectangle.Empty : _layout.BoundsOf(node);
            return hit.IsVertical ? bounds.Width : bounds.Height;
        }
        _splitInitial = Length(hit.First);
        _splitPair = _splitInitial + Length(hit.Second);
    }

    private void DragSplit(DockSplitterInfo split, Point location)
    {
        var zone = Zone(split.Side);
        if (split.IsZone)
        {
            var delta = DockLayoutEngine.IsHorizontalSide(split.Side) ? location.X - _splitStart.X : location.Y - _splitStart.Y;
            if (split.Side is ExDockSide.Right or ExDockSide.Bottom)
                delta = -delta;
            zone.Size = ToLogical(Math.Max(MinZoneSize, _splitInitial + delta));
            PerformLayout();
            // zona sa pri dokumente zuzila - ulozi sa skutocna hrubka, aby sa dala hned zmensit
            var actual = DockLayoutEngine.Thickness(_layout.Zones[(int)split.Side], split.Side);
            if (actual > 0 && ToDevice(zone.Size) > actual)
                zone.Size = ToLogical(actual);
            if (IsHandleCreated)
                Update();
            return;
        }

        // uzly pred a za rozdelovacom si delia svoj spolocny podiel
        if (split is not { First: { } a, Second: { } b })
            return;

        var shift = split.IsVertical ? location.X - _splitStart.X : location.Y - _splitStart.Y;
        var minimum = Math.Min(CaptionHeight + ToDevice(20), _splitPair / 2);
        var first = Math.Max(minimum, Math.Min(_splitPair - minimum, _splitInitial + shift));
        var sum = a.Weight + b.Weight;
        a.Weight = sum * first / Math.Max(1, _splitPair);
        b.Weight = sum - a.Weight;
        PerformLayout();
        if (IsHandleCreated)
            Update();
    }

    /// <inheritdoc />
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        foreach (var group in _floating)
            if (group.FloatForm is { IsDisposed: false } form)
                form.Font = Font;
        PerformLayout();
    }

    /// <inheritdoc />
    protected override void OnDpiChangedAfterParent(EventArgs e)
    {
        base.OnDpiChangedAfterParent(e);
        PerformLayout();
    }

    /// <summary>
    /// Tlacidlo zbaleneho panela na liste.
    /// </summary>
    internal readonly record struct StripButton(ExDockSide Side, DockGroup Group, ExDockPanel Panel, Rectangle Bounds);
}
