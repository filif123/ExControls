namespace ExControls;

/// <summary>
/// Defines a control with its own text search, used by the search box of <see cref="ExOptionsView" />.
/// </summary>
public interface ISearchable
{
    /// <summary>
    /// Determines whether the control contains the specified text.
    /// </summary>
    /// <param name="text">The text to search for.</param>
    /// <returns><see langword="true" /> if the text was found; otherwise, <see langword="false" />.</returns>
    public bool Search(string text);
}