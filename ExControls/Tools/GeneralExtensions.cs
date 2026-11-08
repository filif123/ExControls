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
