using System.Drawing.Design;
using ExControls.Controls;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
///     Class for definition styles for Control.
/// </summary>
[DisplayName("(Collection)")]
public class ExStyleOld : IExNotifyPropertyChanged, ICloneable
{
    private Color? _backColor;
    private Color? _borderColor;
    private Color? _foreColor;

    /// <summary>
    ///     Constructor for designer.
    /// </summary>
    public ExStyleOld() : this(StyleType.Normal)
    {
    }

    /// <summary>
    ///     Constructor.
    /// </summary>
    protected ExStyleOld(StyleType type)
    {
        Type = type;
    }

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="copy"></param>
    protected ExStyleOld(ExStyleOld copy)
    {
        Type = copy.Type;
        BackColor = copy.BackColor;
        ForeColor = copy.ForeColor;
        BorderColor = copy.BorderColor;
    }

    /// <summary>
    ///     Type of style.
    /// </summary>
    public StyleType Type { get; }

    /// <summary>
    ///     Foreground color of the Control.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "White")]
    [Description("Foreground color of the Control.")]
#if NETFRAMEWORK
    [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
#endif
    public Color? BackColor
    {
        get => _backColor;
        set
        {
            if (_backColor == value)
                return;
            _backColor = value;
            OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(BackColor), value));
        }
    }

    /// <summary>
    ///     Background color of the Control.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [Description("Background color of the Control.")]
#if NETFRAMEWORK
    [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
#endif
    public Color? ForeColor
    {
        get => _foreColor;
        set
        {
            if (_foreColor == value)
                return;

            _foreColor = value;
            OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ForeColor), value));
        }
    }

    /// <summary>
    ///     Color of the Controls's border.
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "DimGray")]
    [Description("Color of the Controls's border.")]
#if NETFRAMEWORK
    [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
#endif
    public Color? BorderColor
    {
        get => _borderColor;
        set
        {
            if (_borderColor == value)
                return;

            _borderColor = value;
            OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(BorderColor), BorderColor));
        }
    }

    /// <summary>Creates a new object that is a copy of the current instance.</summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public virtual object Clone()
    {
        return new ExStyleOld(this);
    }

    /// <summary>Occurs when a property value changes.</summary>
    public event EventHandler<ExPropertyChangedEventArgs> PropertyChanged;

    /// <summary>
    ///     Raises the <see cref="PropertyChanged" /> event.
    /// </summary>
    /// <param name="e"></param>
    protected void OnPropertyChanged(ExPropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }
}

/// <summary>
///     Type of style.
/// </summary>
public enum StyleType
{
    /// <summary>
    ///     Normal style of the control.
    /// </summary>
    Normal,

    /// <summary>
    ///     Hover style of the control.
    /// </summary>
    Hover,

    /// <summary>
    ///     Selected style of the control.
    /// </summary>
    Selected,

    /// <summary>
    ///     Disabled style of the control.
    /// </summary>
    Disabled,

    /// <summary>
    ///     ReadOnly style of the control.
    /// </summary>
    ReadOnly
}