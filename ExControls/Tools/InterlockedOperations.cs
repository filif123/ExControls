using System.Diagnostics.CodeAnalysis;

namespace ExControls;

#if NETCOREAPP3_0_OR_GREATER

/// <summary>
/// Vlaknovo bezpecna lenia inicializacia.
/// </summary>
public static class InterlockedOperations
{
    /// <summary>
    /// Nastavi <paramref name="target" /> na <paramref name="value" />, ak je este <see langword="null" />.
    /// </summary>
    /// <returns>hodnota v <paramref name="target" /> po inicializacii (aj ked ju medzitym nastavilo ine vlakno)</returns>
    public static T Initialize<T>([NotNull] ref T target, T value) where T : class
    {
        ArgumentNullException.ThrowIfNull(value);
        T result;
        if ((result = Interlocked.CompareExchange(ref target, value, default)) == null)
        {
            result = value;
        }
        return result;
    }
}

#endif