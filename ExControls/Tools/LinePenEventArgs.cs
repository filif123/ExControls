// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ExControls;

/// <summary>
/// Provides data for the LineDrawing events of <see cref="ExGroupBox" /> and <see cref="ExLineSeparator" />: the pen used to draw
/// the line, which the handler can change.
/// </summary>
public class LinePenEventArgs : EventArgs
{
    internal LinePenEventArgs(Pen pen)
    {
        Pen = pen;
    }

    /// <summary>
    /// Gets the pen used to draw the line.
    /// </summary>
    public Pen Pen { get; }
}
