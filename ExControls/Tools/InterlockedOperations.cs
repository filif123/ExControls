using System.Diagnostics.CodeAnalysis;

namespace ExControls;

#if NETCOREAPP3_0_OR_GREATER

/// <summary>
/// Thread-safe lazy initialization.
/// </summary>
public static class InterlockedOperations
{
    /// <summary>
    /// Sets <paramref name="target" /> to <paramref name="value" /> if it is still <see langword="null" />.
    /// </summary>
    /// <returns>the value in <paramref name="target" /> after the initialization (even if another thread set it in the meantime)</returns>
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