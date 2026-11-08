using System.Collections;

namespace ExControls;

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
