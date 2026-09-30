// ReSharper disable CheckNamespace
namespace System.Linq;

/// <summary>
/// Extension methods for <see cref="System.Collections.Generic.IEnumerable{T}" /> missing in the BCL.
/// Kept in the <c>System.Linq</c> namespace so existing calls keep compiling, but under its own name -
/// a second <c>System.Linq.Enumerable</c> made <c>Enumerable.Range</c> and similar calls ambiguous (CS0433).
/// </summary>
public static partial class ExEnumerable
{
    ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
    ///<param name="items">The enumerable to search.</param>
    ///<param name="predicate">The expression to test the items against.</param>
    ///<returns>The index of the first matching item, or -1 if no items match.</returns>
    public static int FindIndex<TSource>(this IEnumerable<TSource> items, Func<TSource, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(predicate);

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
}
