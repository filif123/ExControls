// ReSharper disable CheckNamespace
namespace System.Linq;



/// <summary>Provides a set of <see langword="static" /> (<see langword="Shared" /> in Visual Basic) methods for querying objects that implement <see cref="T:System.Collections.Generic.IEnumerable`1" />.</summary>

public static class Enumerable
{
    ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
    ///<param name="items">The enumerable to search.</param>
    ///<param name="predicate">The expression to test the items against.</param>
    ///<returns>The index of the first matching item, or -1 if no items match.</returns>
    public static int FindIndex<TSource>(this IEnumerable<TSource> items, Func<TSource, bool> predicate)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var retVal = 0;
        foreach (var item in items)
        {
            if (predicate(item))
                return retVal;
            retVal++;
        }
        return -1;
    }

    ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
    ///<param name="items">The enumerable to search.</param>
    ///<param name="item">The item to find.</param>
    ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
    public static int IndexOf<TSource>(this IEnumerable<TSource> items, TSource item)
    {
        return items.FindIndex(i => EqualityComparer<TSource>.Default.Equals(item, i));
    }

#if NETFRAMEWORK

    /// <summary>Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.</summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
    {
        return source.FirstOrDefault(predicate) ?? defaultValue;
    }

    /// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
    /// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the first element of.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> is <see langword="null" />.</exception>
    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        return source.FirstOrDefault() ?? defaultValue;
    }

    /// <summary>Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.</summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty or if no elements pass the test in the predicate function; otherwise, the last element that passes the test in the predicate function.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
    {
        return source.LastOrDefault(predicate) ?? defaultValue;
    }

    /// <summary>Returns the last element of a sequence, or a default value if the sequence contains no elements.</summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to return the last element of.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty; otherwise, the last element in the <see cref="T:System.Collections.Generic.IEnumerable`1" />.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="source" /> is <see langword="null" />.</exception>
    public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        return source.LastOrDefault() ?? defaultValue;
    }

#endif

}