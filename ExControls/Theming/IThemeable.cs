namespace ExControls;

/// <summary>
/// Prvok, ktory sa podla temy nastyluje sam (vratane svojich vnorenych prvkov).
/// </summary>
public interface IThemeable
{
    /// <summary>
    /// Nastavi farby a vzhlad prvku podla temy.
    /// </summary>
    void ApplyTheme(ExTheme theme);
}
