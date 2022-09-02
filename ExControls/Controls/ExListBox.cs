using static ExControls.ExListBox;

namespace ExControls;

/// <inheritdoc />
public class ExListBox : ListBox, IStylable<ExListBoxStyle>
{
    private bool _defaultStyle;

    /// <summary>
    /// 
    /// </summary>
    public static ExStyleManager<ExListBoxStyle> TemplateStyleManager { get; } = new();

    /// <inheritdoc />
    public ExStyleManager<ExListBoxStyle> StyleManager { get; } = new();

    /// <inheritdoc />
    public ExStyleManager<ExListBoxStyle> GetTemplateStyleManager()
    {
        return TemplateStyleManager;
    }

    /*/// <inheritdoc />
    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (StyleManager.DefaultStyle)
        {
            base.OnDrawItem(e);
            return;
        }

        if (e.Index >= 0)
        {
            using Brush bBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? new SolidBrush(DropDownSelectedRowBackColor)
                : new SolidBrush(DropDownBackColor);

            e.Graphics.FillRectangle(bBrush, e.Bounds);
            TextRenderer.DrawText(e.Graphics, GetItemText(Items[e.Index]), e.Font, e.Bounds.Location, StyleNormal.ForeColor ?? e.ForeColor);
        }
        else
        {
            using var back = new SolidBrush(DropDownBackColor);
            e.Graphics.FillRectangle(back, e.Bounds);
        }

        e.DrawFocusRectangle();

        base.OnDrawItem(e);
    }*/

    public record ExListBoxStyle : ExStyle
    {

    }
}