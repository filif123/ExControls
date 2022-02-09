using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace ExControls.Designers;

/// <summary>
/// This class handles all design time behavior for the panel class.  This
/// draws a visible border on the panel if it doesn't have a border so the 
/// user knows where the boundaries of the panel lie. 
/// </summary>
internal class PanelDesigner : ScrollableControlDesigner
{
    protected PanelDesigner()
    {
        AutoResizeHandles = true;
    }

    /// <summary>
    ///     This draws a nice border around our panel.  We need 
    /// this because the panel can have no border and you can't
    /// tell where it is.
    /// </summary>
    protected virtual void DrawBorder(Graphics graphics)
    {
        var panel = (Panel) Component; // if the panel is invisible, bail now 
        if (panel is not {Visible: true})
        {
            return;
        }

        var pen = BorderPen;
        var rc = Control.ClientRectangle;

        rc.Width--;
        rc.Height--;

        graphics.DrawRectangle(pen, rc);
        pen.Dispose();
    }

    /// <summary>
    ///     Overrides our base class.  Here we check to see if there 
    /// is no border on the panel.  If not, we draw one so that
    /// the panel shape is visible at design time. 
    /// </summary>
    protected override void OnPaintAdornments(PaintEventArgs pe)
    {
        var panel = (Panel) Component;

        if (panel.BorderStyle == BorderStyle.None) 
            DrawBorder(pe.Graphics);

        base.OnPaintAdornments(pe);
    }

    /// <summary>
    ///     Creates a Dashed-Pen of appropriate color.
    /// </summary>
    protected Pen BorderPen
    {
        get
        {
            var penColor = Control.BackColor.GetBrightness() < 0.5
                ? ControlPaint.Light(Control.BackColor)
                : ControlPaint.Dark(Control.BackColor);

            var pen = new Pen(penColor);
            pen.DashStyle = DashStyle.Dash;

            return pen;
        }
    } 
}