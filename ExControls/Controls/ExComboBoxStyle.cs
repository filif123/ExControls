using System.Drawing.Design;
using ExControls.Controls;
// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

namespace ExControls;

/// <summary>
/// Class for definition styles for ExComboBox
/// </summary>
[DisplayName("(Collection)")]
public class ExComboBoxStyle : IExNotifyPropertyChanged, ICloneable
{
    private Color? _backColor;
    private Color? _borderColor;
    private Color? _foreColor;
    private Color? _arrowColor;
    private Color? _buttonBackColor;
    private Color? _buttonBorderColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExComboBoxStyle" /> class for the normal state (used by the designer).
    /// </summary>
    public ExComboBoxStyle() : this(StyleType.Normal)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExComboBoxStyle" /> class for the specified state of the combo box.
    /// </summary>
    /// <param name="type">type of the style</param>
    public ExComboBoxStyle(StyleType type)
    {
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExComboBoxStyle" /> class with the values of another style.
    /// </summary>
    /// <param name="copy">style to copy</param>
    protected ExComboBoxStyle(ExComboBoxStyle copy)
    {
        Type = copy.Type;
        BackColor = copy.BackColor;
        ForeColor = copy.ForeColor;
        BorderColor = copy.BorderColor;
        ArrowColor = copy.ArrowColor;
        ButtonBackColor = copy.ButtonBackColor;
        ButtonBorderColor = copy.ButtonBorderColor;
        ButtonRenderFirst = copy.ButtonRenderFirst;
    }

    /// <summary>
    /// Gets the state of the combo box this style is used for.
    /// </summary>
    public StyleType Type { get; }

    /// <summary>
    /// Foreground color of the Control.
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
    /// Background color of the Control.
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
    /// Color of the control's border.
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

    /// <summary>
    /// Color of the arrow which is in this Control as the dropdown button
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the arrow which is in this Control as the dropdown button.")]
#if NETFRAMEWORK
    [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
#endif
    public Color? ArrowColor
    {
        get => _arrowColor;
        set
        {
            if (_arrowColor == value)
                return;
            _arrowColor = value;
            OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ArrowColor), value));
        }
    }

    /// <summary>
    /// Background color of the dropdown button
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [DefaultValue(typeof(Color), "White")]
    [ExDescription("Background color of the dropdown button.")]
#if NETFRAMEWORK
    [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
#endif
    public Color? ButtonBackColor
    {
        get => _buttonBackColor;
        set
        {
            if (_buttonBackColor == value)
                return;
            _buttonBackColor = value;
            OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ButtonBackColor), value));
        }
    }

    /// <summary>
    /// Border color of the dropdown button
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [DefaultValue(typeof(Color), "DimGray")]
    [ExDescription("Border color of the dropdown button.")]
#if NETFRAMEWORK
    [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
#endif
    public Color? ButtonBorderColor
    {
        get => _buttonBorderColor;
        set
        {
            if (_buttonBorderColor == value)
                return;
            _buttonBorderColor = value;
            OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ButtonBorderColor), value));
        }
    }

    /// <summary>
    /// Gets or sets whether DropDown button has to draw first
    /// </summary>
    [DefaultValue(false)]
    [ExCategory(CategoryType.Appearance)]
    [NotifyParentProperty(true)]
    [ExDescription("Gets or sets whether DropDown button has to be rendered first.")]
    public bool? ButtonRenderFirst
    {
        get;
        set
        {
            if (field == value)
                return;
            field = value;
            OnPropertyChanged(new ExPropertyChangedEventArgs(nameof(ButtonRenderFirst), value));
        }
    }

    /// <summary>Creates a new object that is a copy of the current instance.</summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public virtual object Clone() => new ExComboBoxStyle(this);

    /// <summary>Occurs when a property value changes.</summary>
    public event EventHandler<ExPropertyChangedEventArgs>? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged" /> event.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected void OnPropertyChanged(ExPropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }
}
