using ExControls.Controls;

namespace ExControls;

/// <summary>
/// Represents rectangles of the box and text of the <see cref="ICheckableExControl" />.
/// </summary>
public class BoxAndTextRectangle
{
    /// <summary>
    /// Gets or sets the bounds of the box (check box square or radio button circle).
    /// </summary>
    public Rectangle BoxRectangle { get; set; }

    /// <summary>
    /// Gets or sets the bounds of the text.
    /// </summary>
    public Rectangle TextRectangle { get; set; }
}
