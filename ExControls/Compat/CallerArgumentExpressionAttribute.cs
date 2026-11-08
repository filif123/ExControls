#if NETFRAMEWORK
// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;

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
#endif
