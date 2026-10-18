#if NETFRAMEWORK
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Argument text for <see cref="ArgumentNullExceptionPolyfill" /> (missing in .NET Framework).
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
    {
        /// <summary>
        /// Name of the parameter whose expression is filled in.
        /// </summary>
        public string ParameterName { get; } = parameterName;
    }
}

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// The value of the parameter is not <see langword="null" /> after the method returns (missing in .NET Framework).
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.ReturnValue)]
    internal sealed class NotNullAttribute : Attribute;

    /// <summary>
    /// The property accepts <see langword="null" /> although it does not return it (missing in .NET Framework; the internal
    /// attribute from another assembly is inaccessible - CS0122).
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class AllowNullAttribute : Attribute;
}

// v mennom priestore System - rozsirenie vidi kazdy subor kniznice (aj tie v System.Linq)
namespace System
{
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
}
#endif
