namespace ExControls.Docking;

/// <summary>
/// Plavajuce okno so skupinou panelov: bez systemoveho zahlavia (zahlavie kresli pohlad skupiny a tahanie ho presuva so
/// zobrazenim navadzacich znaciek), ram v okraji meni velkost okna.
/// </summary>
internal sealed class DockFloatForm : Form
{
    // ReSharper disable InconsistentNaming
    private const int WM_NCHITTEST = 0x0084;
    private const int HTCLIENT = 1;
    private const int HTLEFT = 10;
    private const int HTRIGHT = 11;
    private const int HTTOP = 12;
    private const int HTTOPLEFT = 13;
    private const int HTTOPRIGHT = 14;
    private const int HTBOTTOM = 15;
    private const int HTBOTTOMLEFT = 16;
    private const int HTBOTTOMRIGHT = 17;
    private const int CS_DROPSHADOW = 0x00020000;
    private const int WS_EX_TOOLWINDOW = 0x00000080;
    // ReSharper restore InconsistentNaming

    private readonly ExDockManager _manager;
    private bool _closingByManager;

    /// <param name="manager">spravca</param>
    /// <param name="group">skupina okna</param>
    public DockFloatForm(ExDockManager manager, DockGroup group)
    {
        _manager = manager;
        Group = group;
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        AutoScaleMode = AutoScaleMode.None;
        Font = manager.Font;
        var frame = manager.ToDevice(4);
        Padding = new Padding(frame);
        MinimumSize = new Size(manager.ToDevice(120), manager.CaptionHeight + manager.ToDevice(40));
        BackColor = manager.Colors.Back;
    }

    /// <summary>
    /// Skupina okna.
    /// </summary>
    public DockGroup Group { get; }

    /// <inheritdoc />
    protected override bool ShowWithoutActivation => true;

    /// <inheritdoc />
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ClassStyle |= CS_DROPSHADOW;
            cp.ExStyle |= WS_EX_TOOLWINDOW;
            return cp;
        }
    }

    /// <summary>
    /// Vlozi pohlad skupiny.
    /// </summary>
    public void Attach(DockGroupView view)
    {
        view.Mode = DockViewMode.Floating;
        view.Dock = DockStyle.Fill;
        if (view.Parent != this)
            Controls.Add(view);
        Text = Group.Active?.Text ?? "";
    }

    /// <summary>
    /// Zatvori okno bez skrytia panelov (skupina sa ukotvila alebo zanikla).
    /// </summary>
    public void CloseByManager()
    {
        _closingByManager = true;
        if (Group.View is { } view && view.Parent == this)
            Controls.Remove(view);
        Close();
        Dispose();
    }

    /// <summary>
    /// Prefarbi ram podla temy.
    /// </summary>
    public void UpdateColors()
    {
        BackColor = _manager.Colors.Back;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(_manager.Colors.Border);
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg != WM_NCHITTEST || (int)m.Result != HTCLIENT)
            return;

        var point = PointToClient(new Point(unchecked((short)(long)m.LParam), unchecked((short)((long)m.LParam >> 16))));
        var frame = Math.Max(Padding.Left, 1);
        var left = point.X < frame;
        var right = point.X >= Width - frame;
        var top = point.Y < frame;
        var bottom = point.Y >= Height - frame;
        var hit = top ? left ? HTTOPLEFT : right ? HTTOPRIGHT : HTTOP
            : bottom ? left ? HTBOTTOMLEFT : right ? HTBOTTOMRIGHT : HTBOTTOM
            : left ? HTLEFT
            : right ? HTRIGHT
            : HTCLIENT;
        m.Result = (IntPtr)hit;
    }

    /// <inheritdoc />
    protected override void OnMove(EventArgs e)
    {
        base.OnMove(e);
        KeepBounds();
    }

    /// <inheritdoc />
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        KeepBounds();
    }

    /// <inheritdoc />
    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);
        Group.View?.Invalidate();
    }

    /// <inheritdoc />
    protected override void OnDeactivate(EventArgs e)
    {
        base.OnDeactivate(e);
        Group.View?.Invalidate();
    }

    // poloha a velkost pre ulozene rozlozenie a dalsie zobrazenie skupiny
    private void KeepBounds()
    {
        if (Group.FloatForm == this && WindowState == FormWindowState.Normal && IsHandleCreated)
            Group.FloatBounds = Bounds;
    }

    /// <inheritdoc />
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        if (_closingByManager || e.Cancel || e.CloseReason != CloseReason.UserClosing)
            return;

        // Alt+F4 skryje panely okna (ako tlacidlo skryt), okno zatvori spravca
        e.Cancel = true;
        _manager.Post(() => _manager.CloseGroupByUser(Group));
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        // pohlad a panely patria spravcovi - zatvorenie okna (aj s vlastnikom) ich nesmie zrusit
        if (disposing && Group.View is { } view && view.Parent == this)
            Controls.Remove(view);
        base.Dispose(disposing);
    }
}
