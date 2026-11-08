#if NETFRAMEWORK
// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// The value of the parameter is not <see langword="null" /> after the method returns (missing in .NET Framework).
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.ReturnValue)]
internal sealed class NotNullAttribute : Attribute;
#endif
