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