#if NETFRAMEWORK
// ReSharper disable CheckNamespace
namespace System.Linq;

/// <summary>
/// Overloads with a default value that the BCL has only since .NET 6 - on .NET Framework they are provided here.
/// </summary>
public static partial class ExEnumerable
{
    /// <summary>Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.</summary>
    /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
    {
        return source.FirstOrDefault(predicate) ?? defaultValue;
    }

    /// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
    /// <param name="source">The <see cref="System.Collections.Generic.IEnumerable{T}" /> to return the first element of.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="source" /> is <see langword="null" />.</exception>
    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        return source.FirstOrDefault() ?? defaultValue;
    }

    /// <summary>Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.</summary>
    /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty or if no elements pass the test in the predicate function; otherwise, the last element that passes the test in the predicate function.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
    {
        return source.LastOrDefault(predicate) ?? defaultValue;
    }

    /// <summary>Returns the last element of a sequence, or a default value if the sequence contains no elements.</summary>
    /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}" /> to return the last element of.</param>
    /// <param name="defaultValue">A value used as default value.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>
    /// <paramref name="defaultValue"/> if <paramref name="source" /> is empty; otherwise, the last element in the <see cref="System.Collections.Generic.IEnumerable{T}" />.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="source" /> is <see langword="null" />.</exception>
    public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        return source.LastOrDefault() ?? defaultValue;
    }
}
#endif
