namespace ExControls.Docking;

/// <summary>
/// Polopriesvitny nahlad miesta, kam sa panel pri pusteni presunie.
/// </summary>
internal sealed class DockPreviewForm : DockOverlayForm
{
    /// <param name="colors">farby ramu spravcu</param>
    public DockPreviewForm(DockColors colors) : base(colors)
    {
        BackColor = colors.ActiveBack;
        Opacity = 0.45;
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(Colors.Mark, 2);
        e.Graphics.DrawRectangle(pen, 1, 1, Width - 2, Height - 2);
    }
}
