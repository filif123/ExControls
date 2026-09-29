#if NETFRAMEWORK
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Text argumentu pre <see cref="ArgumentNullExceptionPolyfill" /> (na .NET Framework chyba).
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
    {
        /// <summary>
        /// Nazov parametra, ktoreho vyraz sa doplni.
        /// </summary>
        public string ParameterName { get; } = parameterName;
    }
}

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Hodnota parametra nie je po navrate <see langword="null" /> (na .NET Framework chyba).
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.ReturnValue)]
    internal sealed class NotNullAttribute : Attribute;
}

// v mennom priestore System - rozsirenie vidi kazdy subor kniznice (aj tie v System.Linq)
namespace System
{
    /// <summary>
    /// <c>ArgumentNullException.ThrowIfNull</c> aj na .NET Framework - kod kniznice je rovnaky pre vsetky ciele.
    /// </summary>
    internal static class ArgumentNullExceptionPolyfill
    {
        extension(ArgumentNullException)
        {
            /// <summary>
            /// Vyhodi <see cref="ArgumentNullException" />, ak je <paramref name="argument" /> <see langword="null" />.
            /// </summary>
            public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            {
                if (argument is null)
                    throw new ArgumentNullException(paramName);
            }
        }
    }
}
#endif
