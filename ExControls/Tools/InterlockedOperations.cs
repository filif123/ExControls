using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace ExControls;

#if NETCOREAPP3_0_OR_GREATER

public static class InterlockedOperations
{
    public static T Initialize<T>([NotNull] ref T target, T value) where T : class
    {
        if (value == null)
        {
            throw new ArgumentNullException("value");
        }
        T result;
        if ((result = Interlocked.CompareExchange(ref target, value, default)) == null)
        {
            result = value;
        }
        return result;
    }
}

#endif