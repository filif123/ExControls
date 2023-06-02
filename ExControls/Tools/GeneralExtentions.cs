using System.Collections;

namespace ExControls;

/// <summary>
/// 
/// </summary>
public static class GeneralExtentions
{
    /// <summary>
    ///     Converts collection to <see cref="ExBindingList{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">Type of item in collection.</typeparam>
    /// <param name="source">The collection.</param>
    /// <returns>list of the type <see cref="ExBindingList{TSource}"/>.</returns>
    /// <exception cref="ArgumentNullException">when <paramref name="source"/> is <see langword="null"/>.</exception>
    public static ExBindingList<TSource> ToExBindingList<TSource>(this ICollection<TSource> source)
    {
        return source != null ? new ExBindingList<TSource>(source.ToList()) : throw new ArgumentNullException(nameof(source));
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="source"></param>
    /// <param name="toCheck"></param>
    /// <param name="comp"></param>
    /// <returns></returns>
    public static bool Contains(this string source, string toCheck, StringComparison comp) => source?.IndexOf(toCheck, comp) >= 0;
}

/// <summary>
/// 
/// </summary>
public static class SortExtensions
{
    /// <summary>
    ///  Sorts an IList{T} in place.
    /// </summary>
    public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
    {
        ArrayList.Adapter((IList)list).Sort(new ComparisonComparer<T>(comparison));
    }

    /// <summary>
    /// Sorts in IList{T} in place, when T is IComparable{T}
    /// </summary>
    public static void Sort<T>(this IList<T> list) where T : IComparable<T>
    {
        int Comparison(T l, T r) => l.CompareTo(r);
        Sort(list, Comparison);
    }

    /// <summary>
    /// Convenience method on IEnumerable{T} to allow passing of a
    /// Comparison{T} delegate to the OrderBy method.
    /// </summary>
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, Comparison<T> comparison)
    {
        return list.OrderBy(t => t, new ComparisonComparer<T>(comparison));
    }
}

/// <summary>
/// Wraps a generic Comparison{T} delegate in an IComparer to make it easy
/// to use a lambda expression for methods that take an IComparer or IComparer{T}
/// </summary>
/// <typeparam name="T"></typeparam>
public class ComparisonComparer<T> : IComparer<T>, IComparer
{
    private readonly Comparison<T> _comparison;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="comparison"></param>
    public ComparisonComparer(Comparison<T> comparison)
    {
        _comparison = comparison;
    }

    /// <inheritdoc />
    public int Compare(T x, T y)
    {
        return _comparison(x, y);
    }

    /// <inheritdoc />
    public int Compare(object o1, object o2)
    {
        return _comparison((T)o1, (T)o2);
    }
}