using System.Collections;

namespace ExControls;

/// <summary>
/// Applies a theme to the controls of a form. Controls with <see cref="IThemeable" /> style themselves; for the others
/// (DataGridView, ToolStrip, Panel…) the application registers a handler through <see cref="Register{T}" /> - the handler of
/// the nearest registered base type is used. A control without a handler is not changed and its child controls are not visited.
/// </summary>
public static class ExThemer
{
    private static readonly Dictionary<Type, Action<Control, ExTheme>> Handlers = new();

    /// <summary>
    /// Registers a theme handler for controls of type <typeparamref name="T" /> and derived types (unless they have their own).
    /// The handler of a container should call <see cref="Apply(IEnumerable, ExTheme)" /> on its child controls.
    /// </summary>
    /// <typeparam name="T">type of the controls</typeparam>
    /// <param name="apply">handler that applies the theme to a control</param>
    public static void Register<T>(Action<T, ExTheme> apply) where T : Control
    {
        ArgumentNullException.ThrowIfNull(apply);
        Handlers[typeof(T)] = (control, theme) => apply((T)control, theme);
    }

    /// <summary>
    /// Applies the theme to the controls of a collection (e.g. <see cref="Control.Controls" />).
    /// </summary>
    /// <param name="controls">collection of controls</param>
    /// <param name="theme">theme to apply</param>
    public static void Apply(IEnumerable controls, ExTheme theme)
    {
        ArgumentNullException.ThrowIfNull(controls);

        foreach (Control control in controls)
            Apply(control, theme);
    }

    /// <summary>
    /// Applies the theme to a control and its context menu.
    /// </summary>
    /// <param name="control">control to style</param>
    /// <param name="theme">theme to apply</param>
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
