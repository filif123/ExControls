#if NETFRAMEWORK
// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// The property accepts <see langword="null" /> although it does not return it (missing in .NET Framework; the internal
/// attribute from another assembly is inaccessible - CS0122).
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
internal sealed class AllowNullAttribute : Attribute;
#endif
