// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedType.Global
namespace ExControls;

//TODO WORK IN PROGRESS
/// <summary>
///     Theme for WinForms application and its controls.
/// </summary>
internal class ExThemeOld
{
    /// <summary>
    ///     Gets or sets
    /// </summary>
    public bool DefaultStyle
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnDefaultStyleChanged();
        }
    }

    /// <summary>
    ///     Gets or sets
    /// </summary>
    public ExStyleOld? StyleNormal
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnStyleNormalChanged();
        }
    }

    /// <summary>
    ///     Gets or sets
    /// </summary>
    public ExStyleOld? StyleHover
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnStyleHoverChanged();
        }
    }

    /// <summary>
    ///     Gets or sets
    /// </summary>
    public ExStyleOld? StyleSelected
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnStyleSelectedChanged();
        }
    }

    /// <summary>
    ///     Gets or sets
    /// </summary>
    public ExStyleOld? StyleDisabled
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnStyleDisabledChanged();
        }
    }

    /// <summary>
    ///     Gets or sets
    /// </summary>
    public ExStyleOld? StyleReadOnly
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnStyleReadOnlyChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? DefaultStyleChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? StyleNormalChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? StyleHoverChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? StyleSelectedChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? StyleDisabledChanged;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? StyleReadOnlyChanged;


    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnDefaultStyleChanged()
    {
        DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnStyleNormalChanged()
    {
        StyleNormalChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnStyleHoverChanged()
    {
        StyleHoverChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnStyleSelectedChanged()
    {
        StyleSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnStyleDisabledChanged()
    {
        StyleDisabledChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnStyleReadOnlyChanged()
    {
        StyleReadOnlyChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// 
/// </summary>
internal class ExAppThemeOld : ExThemeOld
{
    /// <summary>
    ///     Gets or sets
    /// </summary>
    public bool DarkTitleBar
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnDarkTitleBarChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? DarkTitleBarChanged;

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnDarkTitleBarChanged()
    {
        DarkTitleBarChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// 
/// </summary>
internal class ExScrollableControlTheme : ExThemeOld
{
    /// <summary>
    ///     Gets or sets
    /// </summary>
    public bool DarkScrollBars
    {
        get;
        set
        {
            if (value == field)
                return;
            field = value;
            OnDarkScrollBarChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<EventArgs>? DarkScrollBarChanged;

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnDarkScrollBarChanged()
    {
        DarkScrollBarChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// 
/// </summary>
internal static class ExApplicationOld
{
    /// <summary>
    /// 
    /// </summary>
    public static ExAppThemeOld Theme => field ??= new ExAppThemeOld();
}