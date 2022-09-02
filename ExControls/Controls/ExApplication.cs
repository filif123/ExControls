namespace ExControls;

/// <summary>
/// WORK IN PROGRESS
/// </summary>
public static class ExApplication
{
    private static ExAppTheme _theme;

    /// <summary>
    /// 
    /// </summary>
    public static ExAppTheme Theme => _theme ??= new ExAppTheme();
}

public class ExAppTheme
{
    private bool _darkTitleBar;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs> DarkTitleBarChanged;

    /// <summary>
    ///     
    /// </summary>
    public ExStyleManager<ExStyle> StyleManager { get; } = new();

    /// <summary>
    ///     Gets or sets
    /// </summary>
    public bool DarkTitleBar
    {
        get => _darkTitleBar;
        set
        {
            if (value == _darkTitleBar)
                return;
            _darkTitleBar = value;
            OnDarkTitleBarChanged();
            foreach (Form form in Application.OpenForms)
            {
                if (form is ExForm ef)
                    ef.DarkTitleBar = value;
                else
                    form.SetImmersiveDarkMode(value);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnDarkTitleBarChanged() => DarkTitleBarChanged?.Invoke(this, EventArgs.Empty);
}