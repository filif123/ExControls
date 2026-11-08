#if NETFRAMEWORK
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// v mennom priestore System - rozsirenie vidi kazdy subor kniznice (aj tie v System.Linq)
// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// <c>ArgumentNullException.ThrowIfNull</c> also on .NET Framework - the library code is the same for all targets.
/// </summary>
internal static class ArgumentNullExceptionPolyfill
{
    extension(ArgumentNullException)
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if <paramref name="argument" /> is <see langword="null" />.
        /// </summary>
        /// <param name="argument">the argument to check</param>
        /// <param name="paramName">name of the parameter; the compiler fills in the expression of <paramref name="argument" /></param>
        public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
                throw new ArgumentNullException(paramName);
        }
    }
}
#endif
