using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

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

    /// <summary>
    /// Keeps the properties of an object (a control, its part or a cell style) as they were before the first theme and
    /// restores them for the system style - a control switched from the custom style back to the system one then looks
    /// like a control that never had the custom style. Call it in a handler (or <see cref="IThemeable.ApplyTheme" />) before
    /// the properties change; they are restored in the given order. A property that was not set (inherited from the parent
    /// or default) is reset, so it inherits again.
    /// </summary>
    /// <param name="target">object whose properties the theme changes</param>
    /// <param name="theme">theme being applied</param>
    /// <param name="properties">names of the properties the custom style changes</param>
    /// <exception cref="ArgumentException">when <paramref name="target" /> has no property of the given name</exception>
    public static void KeepDefaults(object target, ExTheme theme, params string[] properties)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(theme);
        ArgumentNullException.ThrowIfNull(properties);

        var saved = Defaults.GetOrCreateValue(target);
        var descriptors = TypeDescriptor.GetProperties(target);
        foreach (var name in properties)
        {
            var property = descriptors[name]
                ?? throw new ArgumentException($"{target.GetType().Name} has no property {name}.", nameof(properties));
            if (!saved.TryGetValue(name, out var value))
            {
                saved[name] = new SavedValue(property.GetValue(target), !property.ShouldSerializeValue(target));
                continue;
            }

            if (!theme.UseSystemStyle)
                continue;

            // vlastnost s resetom (zdedena alebo predvolena hodnota) sa resetuje - ak uz nastavena nie je, nemeni sa;
            // bez resetu (napr. farby stylu bunky) sa vrati povodna hodnota
            if (value.NotSet && HasReset(target, property))
            {
                if (property.CanResetValue(target))
                    property.ResetValue(target);
            }
            else
            {
                property.SetValue(target, value.Value);
            }
        }
    }

    private static bool HasReset(object target, PropertyDescriptor property) =>
        property.Attributes[typeof(DefaultValueAttribute)] is not null
        || target.GetType().GetMethod("Reset" + property.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, Type.EmptyTypes, null) is not null;

    // hodnoty vlastnosti pred prvou temou (objekt -> nazov vlastnosti -> hodnota)
    private static readonly ConditionalWeakTable<object, Dictionary<string, SavedValue>> Defaults = new();

    /// <param name="Value">hodnota pred prvou temou</param>
    /// <param name="NotSet">vlastnost nebola nastavena (zdedena alebo predvolena) - obnovi sa cez ResetValue</param>
    private sealed record SavedValue(object? Value, bool NotSet);

    private static Action<Control, ExTheme>? FindHandler(Type? type)
    {
        for (; type is not null; type = type.BaseType)
            if (Handlers.TryGetValue(type, out var handler))
                return handler;

        return null;
    }
}
