using System.Collections;

namespace ExControls;

/// <summary>
/// General extension methods for collections and strings.
/// </summary>
public static class GeneralExtensions
{
    /// <summary>
    /// Converts collection to <see cref="ExBindingList{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">Type of item in collection.</typeparam>
    /// <param name="source">The collection to convert.</param>
    /// <returns>list of the type <see cref="ExBindingList{TSource}"/>.</returns>
    /// <exception cref="ArgumentNullException">when <paramref name="source"/> is <see langword="null"/>.</exception>
    public static ExBindingList<TSource> ToExBindingList<TSource>(this ICollection<TSource> source)
    {
        return source != null ? new ExBindingList<TSource>(source.ToList()) : throw new ArgumentNullException(nameof(source));
    }

    /// <summary>
    /// Returns whether the string contains <paramref name="toCheck" /> using the specified comparison.
    /// </summary>
    /// <param name="source">string to search in</param>
    /// <param name="toCheck">string to look for</param>
    /// <param name="comp">comparison rules</param>
    /// <returns><see langword="true" /> if <paramref name="toCheck" /> occurs in <paramref name="source" />; <see langword="false" />
    /// also when <paramref name="source" /> is <see langword="null" /></returns>
    public static bool Contains(this string source, string toCheck, StringComparison comp) => source?.IndexOf(toCheck, comp) >= 0;
}

/// <summary>
/// Sorting extension methods with a <see cref="Comparison{T}" /> delegate.
/// </summary>
public static class SortExtensions
{
    /// <summary>
    /// Sorts an IList{T} in place.
    /// </summary>
    /// <typeparam name="T">type of the items</typeparam>
    /// <param name="list">list to sort</param>
    /// <param name="comparison">comparison of the items</param>
    public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
    {
        ArrayList.Adapter((IList)list).Sort(new ComparisonComparer<T>(comparison));
    }

    /// <summary>
    /// Sorts an IList{T} in place, when T is IComparable{T}
    /// </summary>
    /// <typeparam name="T">type of the items</typeparam>
    /// <param name="list">list to sort</param>
    public static void Sort<T>(this IList<T> list) where T : IComparable<T>
    {
        int Comparison(T l, T r) => l.CompareTo(r);
        Sort(list, Comparison);
    }

    /// <summary>
    /// Convenience method on IEnumerable{T} to allow passing of a
    /// Comparison{T} delegate to the OrderBy method.
    /// </summary>
    /// <typeparam name="T">type of the items</typeparam>
    /// <param name="list">items to sort</param>
    /// <param name="comparison">comparison of the items</param>
    /// <returns>the items sorted by <paramref name="comparison" /></returns>
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, Comparison<T> comparison)
    {
        return list.OrderBy(t => t, new ComparisonComparer<T>(comparison));
    }
}

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