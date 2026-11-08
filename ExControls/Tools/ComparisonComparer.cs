using System.Collections;

namespace ExControls;

/// <summary>
/// Wraps a generic Comparison{T} delegate in an IComparer to make it easy
/// to use a lambda expression for methods that take an IComparer or IComparer{T}
/// </summary>
/// <typeparam name="T">type of the compared objects</typeparam>
public class ComparisonComparer<T> : IComparer<T>, IComparer
{
    private readonly Comparison<T> _comparison;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComparisonComparer{T}" /> class.
    /// </summary>
    /// <param name="comparison">comparison used by the comparer</param>
    public ComparisonComparer(Comparison<T> comparison)
    {
        _comparison = comparison;
    }

    /// <inheritdoc />
    public int Compare(T? x, T? y)
    {
        return _comparison(x!, y!);
    }

    /// <inheritdoc />
    public int Compare(object? o1, object? o2)
    {
        return _comparison((T)o1!, (T)o2!);
    }
}
