using System.Collections;

namespace ExControls;

/// <summary>
/// Pouzitie temy na prvky okna. Prvky s <see cref="IThemeable" /> sa nastyluju samy; pre ostatne (DataGridView,
/// ToolStrip, Panel…) si aplikacia zaregistruje obsluhu cez <see cref="Register{T}" /> - pouzije sa obsluha
/// najblizsieho zaregistrovaneho predka typu. Prvok bez obsluhy sa nemeni a jeho vnorene prvky sa neprechadzaju.
/// </summary>
public static class ExThemer
{
    private static readonly Dictionary<Type, Action<Control, ExTheme>> Handlers = new();

    /// <summary>
    /// Zaregistruje obsluhu temy pre prvky typu <typeparamref name="T" /> a odvodene (ak nemaju vlastnu).
    /// Kontajner ma v obsluhe zavolat <see cref="Apply(IEnumerable, ExTheme)" /> na svoje vnorene prvky.
    /// </summary>
    public static void Register<T>(Action<T, ExTheme> apply) where T : Control
    {
        ArgumentNullException.ThrowIfNull(apply);
        Handlers[typeof(T)] = (control, theme) => apply((T)control, theme);
    }

    /// <summary>
    /// Pouzije temu na prvky kolekcie (napr. <see cref="Control.Controls" />).
    /// </summary>
    public static void Apply(IEnumerable controls, ExTheme theme)
    {
        ArgumentNullException.ThrowIfNull(controls);

        foreach (Control control in controls)
            Apply(control, theme);
    }

    /// <summary>
    /// Pouzije temu na prvok a jeho kontextovu ponuku.
    /// </summary>
    public static void Apply(Control control, ExTheme theme)
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(theme);

        if (control is IThemeable themeable)
            themeable.ApplyTheme(theme);
        else
            FindHandler(control.GetType())?.Invoke(control, theme);

        if (control.ContextMenuStrip is { } menu)
            FindHandler(menu.GetType())?.Invoke(menu, theme);
    }

    private static Action<Control, ExTheme>? FindHandler(Type? type)
    {
        for (; type is not null; type = type.BaseType)
            if (Handlers.TryGetValue(type, out var handler))
                return handler;

        return null;
    }
}
