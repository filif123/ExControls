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

/// <summary>
/// Provides data for an event raised when a property value changes: the name of the property and its new value.
/// </summary>
public class ExPropertyChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExPropertyChangedEventArgs" /> class.
    /// </summary>
    /// <param name="name">name of changed property</param>
    /// <param name="value">new value of changed property</param>
    public ExPropertyChangedEventArgs(string name, object? value)
    {
        PropertyName = name;
        Value = value;
    }

    /// <summary>
    /// Gets the new value of the changed property.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets the name of the changed property.
    /// </summary>
    public string PropertyName { get; }
}