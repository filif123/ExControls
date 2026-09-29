#if NETFRAMEWORK
// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;

/// <summary>
/// Reserved to be used by the compiler for tracking metadata (init accessors on .NET Framework).
/// Internal - consumers on net48 define their own polyfill, a public one would clash with it.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit;
#endif
