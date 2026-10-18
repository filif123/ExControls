namespace ExControls;

/// <summary>
/// Color theme of controls. Ex* controls apply it through <see cref="IThemeable" />, other controls through handlers
/// registered in <see cref="ExThemer" />.
/// </summary>
public sealed class ExTheme
{
    /// <summary>
    /// Gets or sets whether controls use the system style (Ex* controls draw like standard Windows controls).
    /// </summary>
    public bool UseSystemStyle { get; set; }

    /// <summary>
    /// Gets or sets whether scroll bars are dark (Windows theme DarkMode_Explorer).
    /// </summary>
    public bool DarkScrollBars { get; set; }

    /// <summary>
    /// Gets or sets the background of panels, forms and menus.
    /// </summary>
    public Color PanelBackColor { get; set; } = SystemColors.Control;

    /// <summary>
    /// Gets or sets the text color of panels, forms and menus.
    /// </summary>
    public Color PanelForeColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Gets or sets the background of boxes (text, lists, grids).
    /// </summary>
    public Color BoxBackColor { get; set; } = SystemColors.Window;

    /// <summary>
    /// Gets or sets the text color of boxes.
    /// </summary>
    public Color BoxForeColor { get; set; } = SystemColors.WindowText;

    /// <summary>
    /// Gets or sets the background of buttons and headers.
    /// </summary>
    public Color ButtonBackColor { get; set; } = SystemColors.Control;

    /// <summary>
    /// Gets or sets the text color of buttons and headers.
    /// </summary>
    public Color ButtonForeColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Gets or sets the color of borders.
    /// </summary>
    public Color BorderColor { get; set; } = SystemColors.ControlDark;

    /// <summary>
    /// Gets or sets the highlight background (selection, hover, focus).
    /// </summary>
    public Color HighlightBackColor { get; set; } = SystemColors.Highlight;

    /// <summary>
    /// Gets or sets the highlight text color.
    /// </summary>
    public Color HighlightForeColor { get; set; } = SystemColors.HighlightText;

    /// <summary>
    /// Gets or sets the color of marks (check mark, radio button, week numbers).
    /// </summary>
    public Color MarkColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Gets or sets the text color of labels.
    /// </summary>
    public Color LabelForeColor { get; set; } = SystemColors.ControlText;

    /// <summary>
    /// Gets or sets the text of the Today button in the calendar; <see langword="null" /> = keep it.
    /// </summary>
    public string? TodayText { get; set; }
}
