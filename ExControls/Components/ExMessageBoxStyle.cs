// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ExControls;

/// <summary>
/// Specifies style of ExMessageBox.
/// </summary>
public class ExMessageBoxStyle
{
    /// <summary>
    /// Gets or sets the Font of label in MessageBox.
    /// </summary>
    public Font? LabelFont { get; set; }

    /// <summary>
    /// Gets or sets the Font of buttons in MessageBox.
    /// </summary>
    public Font? ButtonsFont { get; set; }

    /// <summary>
    /// Gets or sets whether default style of components in MessageBox is used.
    /// </summary>
    public bool? DefaultStyle { get; set; }

    /// <summary>
    /// Gets or sets whether MessageBox's caption should be dark or with default color (dark caption works only in Windows 10).
    /// </summary>
    public bool? UseDarkTitleBar { get; set; }

    /// <summary>
    /// Gets or sets background color of MessageBox.
    /// </summary>
    public Color? BackColor { get; set; }

    /// <summary>
    /// Gets or sets foreground color of MessageBox.
    /// </summary>
    public Color? ForeColor { get; set; }

    /// <summary>
    /// Gets or sets background color of MessageBox's footer.
    /// </summary>
    public Color? FooterBackColor { get; set; }

    /// <summary>
    /// Gets or sets background color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonBackColor { get; set; }

    /// <summary>
    /// Gets or sets foreground color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonForeColor { get; set; }

    /// <summary>
    /// Gets or sets border color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonBorderColor { get; set; }

    /// <summary>
    /// Gets or sets border size of MessageBox's buttons.
    /// </summary>
    public int? ButtonBorderSize { get; set; }

    /// <summary>
    /// Gets or sets mouse-down color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonMouseDownColor { get; set; }

    /// <summary>
    /// Gets or sets mouse-over color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonMouseOverColor { get; set; }
}
