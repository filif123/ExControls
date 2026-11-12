namespace ExControls.Docking;

/// <summary>
/// Zaklad prekryvnych okien pri tahani (znacky, nahlad): navrchu, bez aktivacie, bez panela uloh, mys prechadza cez ne.
/// </summary>
internal abstract class DockOverlayForm : Form
{
    // ReSharper disable InconsistentNaming
    private const int WS_EX_TOPMOST = 0x00000008;
    private const int WS_EX_TRANSPARENT = 0x00000020;
    private const int WS_EX_TOOLWINDOW = 0x00000080;
    private const int WS_EX_NOACTIVATE = 0x08000000;
    // ReSharper restore InconsistentNaming

    /// <param name="colors">farby ramu spravcu</param>
    protected DockOverlayForm(DockColors colors)
    {
        Colors = colors;
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.Manual;
        AutoScaleMode = AutoScaleMode.None;
    }

    /// <summary>
    /// Farby ramu spravcu.
    /// </summary>
    protected DockColors Colors { get; }

    /// <inheritdoc />
    protected override bool ShowWithoutActivation => true;

    /// <inheritdoc />
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= WS_EX_TOPMOST | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
            return cp;
        }
    }

    /// <summary>
    /// Zobrazi okno na mieste alebo ho skryje (prazdny obdlznik).
    /// </summary>
    public void ShowAt(Rectangle bounds)
    {
        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            if (Visible)
                Hide();
            return;
        }

        if (Bounds != bounds)
            Bounds = bounds;
        if (!Visible)
            Show();
        Invalidate();
    }
}
