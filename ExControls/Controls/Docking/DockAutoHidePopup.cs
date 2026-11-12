namespace ExControls.Docking;

/// <summary>
/// Vysunuta zbalena skupina nad dokumentom pri liste auto-hide; vnutorny okraj meni jej hrubku.
/// </summary>
internal sealed class DockAutoHidePopup : Control
{
    private readonly ExDockManager _manager;
    private bool _resizing;
    private int _resizeStart;
    private int _resizeInitial;

    /// <param name="manager">spravca</param>
    public DockAutoHidePopup(ExDockManager manager)
    {
        _manager = manager;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.Selectable, false);
        Visible = false;
    }

    /// <summary>
    /// Vysunuta skupina.
    /// </summary>
    public DockGroup? Group { get; private set; }

    /// <summary>
    /// Strana listy, z ktorej sa skupina vysuva.
    /// </summary>
    public ExDockSide Side { get; private set; }

    /// <summary>
    /// Hrubka sa prave meni tahanim okraja.
    /// </summary>
    public bool IsResizing => _resizing;

    /// <summary>
    /// Vlozi pohlad skupiny.
    /// </summary>
    public void Attach(DockGroup group, DockGroupView view, ExDockSide side)
    {
        if (Group is { View: { } old } && old != view && old.Parent == this)
            Controls.Remove(old);
        Group = group;
        Side = side;
        var edge = _manager.SplitterSize;
        Padding = side switch
        {
            ExDockSide.Left => new Padding(0, 0, edge, 0),
            ExDockSide.Right => new Padding(edge, 0, 0, 0),
            ExDockSide.Top => new Padding(0, 0, 0, edge),
            _ => new Padding(0, edge, 0, 0)
        };
        view.Mode = DockViewMode.AutoHide;
        view.Dock = DockStyle.Fill;
        if (view.Parent != this)
            Controls.Add(view);
    }

    /// <summary>
    /// Vyberie pohlad skupiny (skupina sa ukotvila, skryla alebo sa vysunie ina).
    /// </summary>
    public void Detach()
    {
        if (Group is { View: { } view } && view.Parent == this)
            Controls.Remove(view);
        Group = null;
        _resizing = false;
    }

    private Rectangle Edge => Side switch
    {
        ExDockSide.Left => new Rectangle(Width - Padding.Right, 0, Padding.Right, Height),
        ExDockSide.Right => new Rectangle(0, 0, Padding.Left, Height),
        ExDockSide.Top => new Rectangle(0, Height - Padding.Bottom, Width, Padding.Bottom),
        _ => new Rectangle(0, 0, Width, Padding.Top)
    };

    private bool Horizontal => DockLayoutEngine.IsHorizontalSide(Side);

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        var colors = _manager.Colors;
        e.Graphics.Clear(colors.Back);
        var edge = Edge;
        using var pen = new Pen(colors.Border);
        switch (Side)
        {
            case ExDockSide.Left:
                e.Graphics.DrawLine(pen, edge.Right - 1, 0, edge.Right - 1, Height);
                break;
            case ExDockSide.Right:
                e.Graphics.DrawLine(pen, edge.X, 0, edge.X, Height);
                break;
            case ExDockSide.Top:
                e.Graphics.DrawLine(pen, 0, edge.Bottom - 1, Width, edge.Bottom - 1);
                break;
            default:
                e.Graphics.DrawLine(pen, 0, edge.Y, Width, edge.Y);
                break;
        }
    }

    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left || !Edge.Contains(e.Location))
            return;
        _resizing = true;
        _resizeStart = Horizontal ? MousePosition.X : MousePosition.Y;
        _resizeInitial = Horizontal ? Width : Height;
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (!_resizing)
        {
            Cursor = Edge.Contains(e.Location) ? Horizontal ? Cursors.VSplit : Cursors.HSplit : Cursors.Default;
            return;
        }

        var delta = (Horizontal ? MousePosition.X : MousePosition.Y) - _resizeStart;
        if (Side is ExDockSide.Right or ExDockSide.Bottom)
            delta = -delta;
        _manager.ResizeAutoHide(_resizeInitial + delta);
    }

    /// <inheritdoc />
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _resizing = false;
    }

    /// <inheritdoc />
    protected override void OnMouseCaptureChanged(EventArgs e)
    {
        base.OnMouseCaptureChanged(e);
        if (!Capture)
            _resizing = false;
    }
}
