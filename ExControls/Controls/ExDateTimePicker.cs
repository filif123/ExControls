using System.Globalization;
using System.Windows.Forms.VisualStyles;
using ExControls.Controls;
using ExControls.Designers;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace ExControls;

/// <summary>
///     Date/time picker drawn completely by the library (no Win32 DateTimePicker). The drop-down part is an
///     <see cref="ExCalendar" />. Public API mirrors <see cref="DateTimePicker" /> so the controls are interchangeable.
/// </summary>
[ToolboxBitmap(typeof(DateTimePicker), "DateTimePicker.bmp")]
[DefaultProperty(nameof(Value))]
[DefaultEvent(nameof(ValueChanged))]
[DefaultBindingProperty(nameof(Value))]
[Designer("ExControls.Designers.ExDateTimePickerDesigner, ExControls")]
public class ExDateTimePicker : Control, IExControl
{
    /// <summary>Minimum date the control accepts (same as <see cref="DateTimePicker.MinimumDateTime" />).</summary>
    public static readonly DateTime MinimumDateTime = new(1753, 1, 1);

    /// <summary>Maximum date the control accepts (same as <see cref="DateTimePicker.MaximumDateTime" />).</summary>
    public static readonly DateTime MaximumDateTime = new(9998, 12, 31);

    private enum FieldKind
    {
        Literal,
        Day,
        DayName,
        Month,
        MonthName,
        Year,
        Hour12,
        Hour24,
        Minute,
        Second,
        AmPm
    }

    private sealed class Field
    {
        public FieldKind Kind;
        public int Length;
        public string Literal = string.Empty;
        public string Text = string.Empty;
        public Rectangle Bounds;
        public bool Editable => Kind is not (FieldKind.Literal or FieldKind.DayName);
        public bool Numeric => Kind is 
            FieldKind.Day or FieldKind.Month or FieldKind.MonthName or FieldKind.Year or 
            FieldKind.Hour12 or FieldKind.Hour24 or FieldKind.Minute or FieldKind.Second;
    }

    private enum Part
    {
        None,
        CheckBox,
        Text,
        DropButton,
        SpinUp,
        SpinDown
    }

    private bool _defaultStyle;
    private DateTime _value;
    private bool _userHasSetValue;
    private DateTime _minDate;
    private DateTime _maxDate;
    private DateTimePickerFormat _format;
    private string? _customFormat;
    private bool _showUpDown;
    private bool _showCheckBox;
    private bool _checked;
    private LeftRightAlignment _dropDownAlign;

    private Color _borderColor;
    private Color _highlightColor;
    private Color _arrowColor;
    private Color _buttonBackColor;
    private Color _disabledBackColor;
    private Color _disabledForeColor;
    private Color _selectedFieldBackColor;
    private Color _selectedFieldForeColor;

    private readonly List<Field> _fields = [];
    private bool _fieldsDirty = true;
    private int _selectedField = -1;
    private bool _checkBoxSelected;
    private string _typed = string.Empty;

    private bool _hover;
    private Part _hoverPart;
    private Part _pressedPart;
    private Rectangle _checkRect;
    private Rectangle _textRect;
    private Rectangle _buttonRect;
    private bool _narrowButton;
    private System.Windows.Forms.Timer? _spinTimer;

    private ToolStripDropDown? _dropDown;
    private ToolStripControlHost? _host;
    private bool _dropDownOpen;
    private int _dropDownClosedAt;

    /// <summary>
    ///     Constructor.
    /// </summary>
    public ExDateTimePicker()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserMouse | ControlStyles.FixedHeight, true);
        SetStyle(ControlStyles.StandardDoubleClick, false);

        _defaultStyle = true;
        _value = DateTime.Now;
        _minDate = MinimumDateTime;
        _maxDate = MaximumDateTime;
        _format = DateTimePickerFormat.Long;
        _checked = true;
        _dropDownAlign = LeftRightAlignment.Left;

        _borderColor = Color.DimGray;
        _highlightColor = SystemColors.Highlight;
        _arrowColor = Color.Black;
        _buttonBackColor = Color.White;
        _disabledBackColor = SystemColors.Control;
        _disabledForeColor = SystemColors.GrayText;
        _selectedFieldBackColor = SystemColors.Highlight;
        _selectedFieldForeColor = SystemColors.HighlightText;

        base.BackColor = Color.White;
        base.ForeColor = Color.Black;

        Calendar = new ExCalendar { DrawBorder = true, TabStop = false };
        Calendar.DateSelected += Calendar_DateSelected;
    }

    #region Properties - data

    /// <summary>
    ///     Selected date and time.
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [ExDescription("Selected date and time.")]
    [Bindable(true)]
    [RefreshProperties(RefreshProperties.All)]
    public DateTime Value
    {
        get => _value;
        set
        {
            _userHasSetValue = true;
            if (value < _minDate) value = _minDate;
            if (value > _maxDate) value = _maxDate;
            if (_value == value)
                return;
            _value = value;
            _fieldsDirty = true;
            Invalidate();
            OnValueChanged(EventArgs.Empty);
            OnTextChanged(EventArgs.Empty);
        }
    }

    private bool ShouldSerializeValue() => _userHasSetValue;

    private void ResetValue()
    {
        Value = DateTime.Now;
        _userHasSetValue = false;
    }

    /// <summary>
    ///     Minimum selectable date.
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [ExDescription("Minimum selectable date.")]
    public DateTime MinDate
    {
        get => _minDate;
        set
        {
            if (value < MinimumDateTime) value = MinimumDateTime;
            if (value > _maxDate)
                throw new ArgumentOutOfRangeException(nameof(value), "MinDate must be less than or equal to MaxDate.");
            if (_minDate == value)
                return;
            _minDate = value;
            Calendar.MinDate = value.Date;
            if (_value < value) Value = value;
            Invalidate();
        }
    }

    private bool ShouldSerializeMinDate() => _minDate != MinimumDateTime;
    private void ResetMinDate() => MinDate = MinimumDateTime;

    /// <summary>
    ///     Maximum selectable date.
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [ExDescription("Maximum selectable date.")]
    public DateTime MaxDate
    {
        get => _maxDate;
        set
        {
            if (value > MaximumDateTime) value = MaximumDateTime;
            if (value < _minDate)
                throw new ArgumentOutOfRangeException(nameof(value), "MaxDate must be greater than or equal to MinDate.");
            if (_maxDate == value)
                return;
            _maxDate = value;
            Calendar.MaxDate = value.Date;
            if (_value > value) Value = value;
            Invalidate();
        }
    }

    private bool ShouldSerializeMaxDate() => _maxDate != MaximumDateTime;
    private void ResetMaxDate() => MaxDate = MaximumDateTime;

    /// <summary>
    ///     Format of the displayed date/time.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(DateTimePickerFormat.Long)]
    [RefreshProperties(RefreshProperties.Repaint)]
    [ExDescription("Format of the displayed date/time.")]
    public DateTimePickerFormat Format
    {
        get => _format;
        set
        {
            if (_format == value)
                return;
            _format = value;
            RebuildFields();
            OnFormatChanged(EventArgs.Empty);
        }
    }

    /// <summary>
    ///     Custom format string used when <see cref="Format" /> is <see cref="DateTimePickerFormat.Custom" />.
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [DefaultValue(null)]
    [Localizable(true)]
    [RefreshProperties(RefreshProperties.Repaint)]
    [ExDescription("Custom format string used when Format is Custom.")]
    public string? CustomFormat
    {
        get => _customFormat;
        set
        {
            if (_customFormat == value)
                return;
            _customFormat = value;
            if (_format == DateTimePickerFormat.Custom)
                RebuildFields();
        }
    }

    /// <summary>
    ///     Shows a spin control instead of the drop-down calendar.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(false)]
    [ExDescription("Shows a spin control instead of the drop-down calendar.")]
    public bool ShowUpDown
    {
        get => _showUpDown;
        set
        {
            if (_showUpDown == value)
                return;
            _showUpDown = value;
            if (value) CloseDropDown();
            Invalidate();
        }
    }

    /// <summary>
    ///     Shows a check box; when unchecked, no value is selected.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(false)]
    [ExDescription("Shows a check box; when unchecked, no value is selected.")]
    public bool ShowCheckBox
    {
        get => _showCheckBox;
        set
        {
            if (_showCheckBox == value)
                return;
            _showCheckBox = value;
            if (!value) _checkBoxSelected = false;
            Invalidate();
        }
    }

    /// <summary>
    ///     Whether the value is selected (has meaning only when <see cref="ShowCheckBox" /> is true).
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [DefaultValue(true)]
    [Bindable(true)]
    [ExDescription("Whether the value is selected (has meaning only when ShowCheckBox is true).")]
    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value)
                return;
            _checked = value;
            Invalidate();
            OnCheckedChanged(EventArgs.Empty);
            OnTextChanged(EventArgs.Empty);
        }
    }

    /// <summary>
    ///     Alignment of the drop-down calendar.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(LeftRightAlignment.Left)]
    [Localizable(true)]
    [ExDescription("Alignment of the drop-down calendar.")]
    public LeftRightAlignment DropDownAlign
    {
        get => _dropDownAlign;
        set => _dropDownAlign = value;
    }

    /// <summary>
    ///     Calendar shown in the drop-down. Its appearance (colors, TodayText, ShowWeekNumbers...) can be set here.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [ExDescription("Calendar shown in the drop-down.")]
    public ExCalendar Calendar { get; }

    /// <summary>
    ///     Height of the control computed from the font.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int PreferredHeight => FontHeight + LogicalToDeviceUnits(7);

    /// <summary>
    ///     Formatted value. Empty when <see cref="ShowCheckBox" /> is true and the control is unchecked.
    ///     Setting parses the string using the current format.
    /// </summary>
    [Browsable(false)]
    [Bindable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    // Control.Text je v BCL [AllowNull]; atribut sa na net48 neda pouzit (CS0122), preto pragma.
#pragma warning disable CS8765
    public override string Text
    {
        get
        {
            if (_showCheckBox && !_checked)
                return string.Empty;
            EnsureFields();
            return string.Concat(_fields.Select(f => f.Text));
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (_showCheckBox) Checked = false;
                return;
            }

            var ci = CultureInfo.CurrentCulture;
            if (DateTime.TryParseExact(value, GetPattern(), ci, DateTimeStyles.AllowWhiteSpaces, out var dt) ||
                DateTime.TryParse(value, ci, DateTimeStyles.AllowWhiteSpaces, out dt))
            {
                Value = dt;
                if (_showCheckBox) Checked = true;
            }
            else
            {
                throw new FormatException($"'{value}' is not a valid date/time.");
            }
        }
    }
#pragma warning restore CS8765

    /// <inheritdoc />
    protected override Size DefaultSize => new(200, PreferredHeight);

    #endregion

    #region Properties - appearance

    /// <inheritdoc />
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [ExDescription("Default style of the Control.")]
    public bool DefaultStyle
    {
        get => _defaultStyle;
        set
        {
            if (_defaultStyle == value)
                return;
            _defaultStyle = value;
            Calendar.DefaultStyle = value;
            Invalidate();
            OnDefaultStyleChanged();
        }
    }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the DefaultStyle property changes.")]
    public event EventHandler? DefaultStyleChanged;

    /// <inheritdoc />
    [DefaultValue(typeof(Color), "White")]
    public override Color BackColor
    {
        get => base.BackColor;
        set => base.BackColor = value;
    }

    /// <inheritdoc />
    [DefaultValue(typeof(Color), "Black")]
    public override Color ForeColor
    {
        get => base.ForeColor;
        set => base.ForeColor = value;
    }

    /// <summary>
    ///     Color of the border.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "DimGray")]
    [ExDescription("Color of the border.")]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            if (_borderColor == value)
                return;
            _borderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the border and of the pressed button when the mouse is over the control or it has focus.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color of the border and of the pressed button when the mouse is over the control or it has focus.")]
    public Color HighlightColor
    {
        get => _highlightColor;
        set
        {
            if (_highlightColor == value)
                return;
            _highlightColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the arrow(s) on the drop-down/spin button.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the arrow(s) on the drop-down/spin button.")]
    public Color ArrowColor
    {
        get => _arrowColor;
        set
        {
            if (_arrowColor == value)
                return;
            _arrowColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the drop-down/spin button.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "White")]
    [ExDescription("Background color of the drop-down/spin button.")]
    public Color ButtonBackColor
    {
        get => _buttonBackColor;
        set
        {
            if (_buttonBackColor == value)
                return;
            _buttonBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color when the control is disabled.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Control")]
    [ExDescription("Background color when the control is disabled.")]
    public Color DisabledBackColor
    {
        get => _disabledBackColor;
        set
        {
            if (_disabledBackColor == value)
                return;
            _disabledBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Text color when the control is disabled or unchecked.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "GrayText")]
    [ExDescription("Text color when the control is disabled or unchecked.")]
    public Color DisabledForeColor
    {
        get => _disabledForeColor;
        set
        {
            if (_disabledForeColor == value)
                return;
            _disabledForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the selected field (day, month...).
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Background color of the selected field (day, month...).")]
    public Color SelectedFieldBackColor
    {
        get => _selectedFieldBackColor;
        set
        {
            if (_selectedFieldBackColor == value)
                return;
            _selectedFieldBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Text color of the selected field (day, month...).
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "HighlightText")]
    [ExDescription("Text color of the selected field (day, month...).")]
    public Color SelectedFieldForeColor
    {
        get => _selectedFieldForeColor;
        set
        {
            if (_selectedFieldForeColor == value)
                return;
            _selectedFieldForeColor = value;
            Invalidate();
        }
    }

    #endregion

    #region Events

    /// <summary>Occurs when <see cref="Value" /> changes.</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the Value property changes.")]
    public event EventHandler? ValueChanged;

    /// <summary>Occurs when <see cref="Checked" /> changes.</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the Checked property changes.")]
    public event EventHandler? CheckedChanged;

    /// <summary>Occurs when <see cref="Format" /> changes.</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the Format property changes.")]
    public event EventHandler? FormatChanged;

    /// <summary>Occurs when the drop-down calendar is shown.</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the drop-down calendar is shown.")]
    public event EventHandler? DropDown;

    /// <summary>Occurs when the drop-down calendar is closed.</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the drop-down calendar is closed.")]
    public event EventHandler? CloseUp;

    /// <summary>Raises the <see cref="ValueChanged" /> event.</summary>
    protected virtual void OnValueChanged(EventArgs e) => ValueChanged?.Invoke(this, e);

    /// <summary>Raises the <see cref="CheckedChanged" /> event.</summary>
    protected virtual void OnCheckedChanged(EventArgs e) => CheckedChanged?.Invoke(this, e);

    /// <summary>Raises the <see cref="FormatChanged" /> event.</summary>
    protected virtual void OnFormatChanged(EventArgs e) => FormatChanged?.Invoke(this, e);

    /// <summary>Raises the <see cref="DropDown" /> event.</summary>
    protected virtual void OnDropDown(EventArgs e) => DropDown?.Invoke(this, e);

    /// <summary>Raises the <see cref="CloseUp" /> event.</summary>
    protected virtual void OnCloseUp(EventArgs e) => CloseUp?.Invoke(this, e);

    /// <summary>Raises the <see cref="DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged() => DefaultStyleChanged?.Invoke(this, EventArgs.Empty);

    #endregion

    #region Fields (format)

    private string GetPattern()
    {
        var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
        return _format switch
        {
            DateTimePickerFormat.Short => dtfi.ShortDatePattern,
            DateTimePickerFormat.Time => dtfi.LongTimePattern,
            DateTimePickerFormat.Custom => _customFormat ?? string.Empty,
            _ => dtfi.LongDatePattern
        };
    }

    private void RebuildFields()
    {
        _fields.Clear();
        var pattern = GetPattern();
        var literal = new System.Text.StringBuilder();

        void FlushLiteral()
        {
            if (literal.Length == 0)
                return;
            _fields.Add(new Field { Kind = FieldKind.Literal, Literal = literal.ToString() });
            literal.Clear();
        }

        var i = 0;
        while (i < pattern.Length)
        {
            var c = pattern[i];
            switch (c)
            {
                case '\'' or '"':
                {
                    var end = pattern.IndexOf(c, i + 1);
                    if (end < 0) end = pattern.Length;
                    literal.Append(pattern, i + 1, end - i - 1);
                    i = end + 1;
                    continue;
                }
                case '\\' when i + 1 < pattern.Length:
                    literal.Append(pattern[i + 1]);
                    i += 2;
                    continue;
                case 'd' or 'M' or 'y' or 'h' or 'H' or 'm' or 's' or 't':
                {
                    var n = 1;
                    while (i + n < pattern.Length && pattern[i + n] == c) n++;
                    FlushLiteral();
                    var kind = c switch
                    {
                        'd' => n <= 2 ? FieldKind.Day : FieldKind.DayName,
                        'M' => n <= 2 ? FieldKind.Month : FieldKind.MonthName,
                        'y' => FieldKind.Year,
                        'h' => FieldKind.Hour12,
                        'H' => FieldKind.Hour24,
                        'm' => FieldKind.Minute,
                        's' => FieldKind.Second,
                        _ => FieldKind.AmPm
                    };
                    var len = kind switch
                    {
                        FieldKind.DayName or FieldKind.MonthName => n >= 4 ? 4 : 3,
                        FieldKind.Year => n <= 2 ? 2 : 4,
                        FieldKind.AmPm => n >= 2 ? 2 : 1,
                        _ => Math.Min(n, 2)
                    };
                    _fields.Add(new Field { Kind = kind, Length = len });
                    i += n;
                    continue;
                }
                default:
                    literal.Append(c);
                    i++;
                    continue;
            }
        }

        FlushLiteral();

        if (_selectedField < 0 || _selectedField >= _fields.Count || !_fields[_selectedField].Editable)
            _selectedField = _fields.FindIndex(f => f.Editable);
        _typed = string.Empty;
        _fieldsDirty = true;
        Invalidate();
        OnTextChanged(EventArgs.Empty);
    }

    private void EnsureFields()
    {
        if (_fields.Count == 0 && GetPattern().Length > 0)
            RebuildFields();
        if (!_fieldsDirty)
            return;
        _fieldsDirty = false;

        var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
        var hasDay = _fields.Any(f => f.Kind == FieldKind.Day);
        var v = _value;
        for (var i = 0; i < _fields.Count; i++)
        {
            var f = _fields[i];
            // rozpisany rok sa zobrazuje tak, ako ho pouzivatel prave pise
            if (i == _selectedField && f.Kind == FieldKind.Year && _typed.Length > 0)
            {
                f.Text = _typed;
                continue;
            }

            f.Text = f.Kind switch
            {
                FieldKind.Literal => f.Literal,
                FieldKind.Day => f.Length == 1 ? v.Day.ToString(CultureInfo.InvariantCulture) : v.Day.ToString("00", CultureInfo.InvariantCulture),
                FieldKind.DayName => f.Length == 3 ? dtfi.GetAbbreviatedDayName(v.DayOfWeek) : dtfi.GetDayName(v.DayOfWeek),
                FieldKind.Month => f.Length == 1 ? v.Month.ToString(CultureInfo.InvariantCulture) : v.Month.ToString("00", CultureInfo.InvariantCulture),
                // pri dlhom datume s dnom sa pouziva genitiv (14. decembra 2025) rovnako ako .NET/Windows
                FieldKind.MonthName => f.Length == 3
                    ? (hasDay ? dtfi.AbbreviatedMonthGenitiveNames : dtfi.AbbreviatedMonthNames)[v.Month - 1]
                    : (hasDay ? dtfi.MonthGenitiveNames : dtfi.MonthNames)[v.Month - 1],
                FieldKind.Year => f.Length == 2 ? (v.Year % 100).ToString("00", CultureInfo.InvariantCulture) : v.Year.ToString("0000", CultureInfo.InvariantCulture),
                FieldKind.Hour12 => Format12(v.Hour, f.Length),
                FieldKind.Hour24 => f.Length == 1 ? v.Hour.ToString(CultureInfo.InvariantCulture) : v.Hour.ToString("00", CultureInfo.InvariantCulture),
                FieldKind.Minute => f.Length == 1 ? v.Minute.ToString(CultureInfo.InvariantCulture) : v.Minute.ToString("00", CultureInfo.InvariantCulture),
                FieldKind.Second => f.Length == 1 ? v.Second.ToString(CultureInfo.InvariantCulture) : v.Second.ToString("00", CultureInfo.InvariantCulture),
                FieldKind.AmPm => AmPmText(v.Hour < 12 ? dtfi.AMDesignator : dtfi.PMDesignator, f.Length),
                _ => string.Empty
            };
        }
    }

    private static string Format12(int hour, int length)
    {
        var h = hour % 12;
        if (h == 0) h = 12;
        return length == 1 ? h.ToString(CultureInfo.InvariantCulture) : h.ToString("00", CultureInfo.InvariantCulture);
    }

    private static string AmPmText(string designator, int length) =>
        length == 1 && designator.Length > 0 ? designator.Substring(0, 1) : designator;

    private int FieldMax(Field f) => f.Kind switch
    {
        FieldKind.Day => DateTime.DaysInMonth(_value.Year, _value.Month),
        FieldKind.Month or FieldKind.MonthName => 12,
        FieldKind.Year => 9999,
        FieldKind.Hour12 => 12,
        FieldKind.Hour24 => 23,
        _ => 59
    };

    private static int FieldMin(Field f) => f.Kind is FieldKind.Day or FieldKind.Month or FieldKind.MonthName or FieldKind.Year or FieldKind.Hour12 ? 1 : 0;

    private int FieldValue(Field f) => f.Kind switch
    {
        FieldKind.Day => _value.Day,
        FieldKind.Month or FieldKind.MonthName => _value.Month,
        FieldKind.Year => _value.Year,
        FieldKind.Hour12 => _value.Hour % 12 == 0 ? 12 : _value.Hour % 12,
        FieldKind.Hour24 => _value.Hour,
        FieldKind.Minute => _value.Minute,
        FieldKind.Second => _value.Second,
        _ => 0
    };

    /// <summary>
    ///     Sets a numeric field; the day is clamped to the month length, the result to Min/MaxDate.
    /// </summary>
    private void SetFieldValue(Field f, int n)
    {
        var v = _value;
        DateTime result;
        try
        {
            switch (f.Kind)
            {
                case FieldKind.Day:
                    result = new DateTime(v.Year, v.Month, Math.Min(n, DateTime.DaysInMonth(v.Year, v.Month)), v.Hour, v.Minute, v.Second, v.Millisecond);
                    break;
                case FieldKind.Month or FieldKind.MonthName:
                    result = new DateTime(v.Year, n, Math.Min(v.Day, DateTime.DaysInMonth(v.Year, n)), v.Hour, v.Minute, v.Second, v.Millisecond);
                    break;
                case FieldKind.Year:
                    result = new DateTime(n, v.Month, Math.Min(v.Day, DateTime.DaysInMonth(n, v.Month)), v.Hour, v.Minute, v.Second, v.Millisecond);
                    break;
                case FieldKind.Hour12:
                    result = v.Date.AddHours(n % 12 + (v.Hour >= 12 ? 12 : 0)).AddMinutes(v.Minute).AddSeconds(v.Second).AddMilliseconds(v.Millisecond);
                    break;
                case FieldKind.Hour24:
                    result = v.Date.AddHours(n).AddMinutes(v.Minute).AddSeconds(v.Second).AddMilliseconds(v.Millisecond);
                    break;
                case FieldKind.Minute:
                    result = v.Date.AddHours(v.Hour).AddMinutes(n).AddSeconds(v.Second).AddMilliseconds(v.Millisecond);
                    break;
                case FieldKind.Second:
                    result = v.Date.AddHours(v.Hour).AddMinutes(v.Minute).AddSeconds(n).AddMilliseconds(v.Millisecond);
                    break;
                default:
                    return;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }

        Value = result;
    }

    private void StepField(int direction)
    {
        if (_selectedField < 0 || !_checked && _showCheckBox || !Enabled)
            return;
        var f = _fields[_selectedField];
        CommitTyped();

        if (f.Kind == FieldKind.AmPm)
        {
            Value = _value.AddHours(_value.Hour < 12 ? 12 : -12);
            return;
        }

        if (!f.Numeric)
            return;

        var min = FieldMin(f);
        var max = FieldMax(f);
        var n = FieldValue(f) + direction;
        // polia sa tocia dokola v ramci svojho rozsahu (ako nativny DateTimePicker)
        if (n > max) n = min;
        if (n < min) n = max;
        SetFieldValue(f, n);
    }

    private void SelectField(int index)
    {
        if (index == _selectedField)
            return;
        CommitTyped();
        _selectedField = index;
        _checkBoxSelected = false;
        Invalidate();
    }

    private void MoveSelection(int direction)
    {
        if (_fields.Count == 0)
            return;
        var editable = _fields.Select((f, i) => (f, i)).Where(t => t.f.Editable).Select(t => t.i).ToList();
        if (editable.Count == 0)
            return;

        if (_checkBoxSelected)
        {
            if (direction > 0 && (_checked || !_showCheckBox))
            {
                _checkBoxSelected = false;
                _selectedField = editable[0];
                Invalidate();
            }

            return;
        }

        var pos = editable.IndexOf(_selectedField);
        var next = pos + direction;
        if (next < 0)
        {
            if (_showCheckBox)
            {
                CommitTyped();
                _checkBoxSelected = true;
                Invalidate();
            }

            return;
        }

        if (next >= editable.Count)
            return;
        SelectField(editable[next]);
    }

    private void CommitTyped()
    {
        if (_typed.Length == 0)
            return;
        var typed = _typed;
        _typed = string.Empty;
        _fieldsDirty = true;
        if (_selectedField < 0)
            return;
        var f = _fields[_selectedField];
        if (f.Kind != FieldKind.Year || !int.TryParse(typed, NumberStyles.None, CultureInfo.InvariantCulture, out var n))
            return;
        if (f.Length == 2)
            n = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(n);
        if (n >= 1)
            SetFieldValue(f, n);
        Invalidate();
    }

    private void TypeDigit(int digit)
    {
        if (_selectedField < 0 || !Enabled || _showCheckBox && !_checked)
            return;
        var f = _fields[_selectedField];
        if (!f.Numeric)
            return;

        var maxLen = f.Kind == FieldKind.Year ? f.Length : 2;
        var max = f.Kind == FieldKind.Year ? 9999 : FieldMax(f);
        _typed += digit.ToString(CultureInfo.InvariantCulture);
        var n = int.Parse(_typed, CultureInfo.InvariantCulture);

        if (f.Kind == FieldKind.Year)
        {
            _fieldsDirty = true;
            Invalidate();
            if (_typed.Length >= maxLen)
            {
                CommitTyped();
                MoveSelection(1);
            }

            return;
        }

        if (n > max)
        {
            // napr. den "35": 3 uz je nastavena, 5 zacina zapis do dalsieho pola
            // (jedna cislica nikdy nepresiahne maximum pola, takze rekurzia skonci)
            _typed = string.Empty;
            MoveSelection(1);
            TypeDigit(digit);
            return;
        }

        if (n >= FieldMin(f))
            SetFieldValue(f, n);
        Invalidate();

        // pole je plne alebo dalsia cislica by presiahla maximum -> posun na dalsie pole
        if (_typed.Length >= maxLen || n * 10 > max)
        {
            _typed = string.Empty;
            MoveSelection(1);
        }
    }

    private void TypeLetter(char c)
    {
        if (_selectedField < 0 || !Enabled || _showCheckBox && !_checked)
            return;
        var f = _fields[_selectedField];
        var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
        c = char.ToUpperInvariant(c);

        if (f.Kind == FieldKind.AmPm)
        {
            if (dtfi.AMDesignator.Length > 0 && char.ToUpperInvariant(dtfi.AMDesignator[0]) == c && _value.Hour >= 12)
                Value = _value.AddHours(-12);
            else if (dtfi.PMDesignator.Length > 0 && char.ToUpperInvariant(dtfi.PMDesignator[0]) == c && _value.Hour < 12)
                Value = _value.AddHours(12);
            return;
        }

        if (f.Kind != FieldKind.MonthName)
            return;

        // pismeno vyberie dalsi mesiac zacinajuci tym pismenom
        for (var k = 1; k <= 12; k++)
        {
            var m = (_value.Month - 1 + k) % 12 + 1;
            var name = dtfi.MonthNames[m - 1];
            if (name.Length > 0 && char.ToUpperInvariant(name[0]) == c)
            {
                SetFieldValue(f, m);
                return;
            }
        }
    }

    #endregion

    #region Layout & painting

    /// <inheritdoc />
    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, width, PreferredHeight, specified);
    }

    /// <inheritdoc />
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        Height = PreferredHeight;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
    {
        base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
        Height = PreferredHeight;
    }

    private void ComputeLayout()
    {
        // rozmery podla nativneho DateTimePicker (DTM_GETDATETIMEPICKERINFO pri 96 dpi):
        // tlacidlo 34 px vratane ramu cez celu vysku, text od x = 7, policko 13 px na x = 5
        const int border = 1;
        // pri nedostatku miesta pre text sa tlacidlo zuzi na samotnu sipku (ako ComboBox)
        var buttonWidth = _showUpDown || _narrowButton ? SystemInformation.VerticalScrollBarWidth : LogicalToDeviceUnits(33);
        _buttonRect = new Rectangle(Width - border - buttonWidth, border, buttonWidth, Height - 2 * border);

        int left;
        if (_showCheckBox)
        {
            var size = LogicalToDeviceUnits(13);
            _checkRect = new Rectangle(LogicalToDeviceUnits(5), (Height - size) / 2, size, size);
            left = _checkRect.Right + LogicalToDeviceUnits(4);
        }
        else
        {
            _checkRect = Rectangle.Empty;
            left = border + LogicalToDeviceUnits(6);
        }

        _textRect = new Rectangle(left, border, Math.Max(0, _buttonRect.Left - left), Height - 2 * border);
    }

    private static bool UseVisualStyles => VisualStyleRenderer.IsSupported && Application.RenderWithVisualStyles;

    private static bool TryRenderer(string className, int part, int state, out VisualStyleRenderer? renderer)
    {
        renderer = null;
        try
        {
            var element = VisualStyleElement.CreateElement(className, part, state);
            if (!VisualStyleRenderer.IsElementDefined(element))
                return false;
            renderer = new VisualStyleRenderer(element);
            return true;
        }
        catch (Exception e) when (e is ArgumentException or InvalidOperationException)
        {
            return false;
        }
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        EnsureFields();
        var g = e.Graphics;
        const TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix;
        var widths = new int[_fields.Count];
        for (var i = 0; i < _fields.Count; i++)
            widths[i] = TextRenderer.MeasureText(g, _fields[i].Text, Font, Size.Empty, flags).Width;
        _narrowButton = false;
        ComputeLayout();
        if (!_showUpDown && widths.Sum() > _textRect.Width)
        {
            _narrowButton = true;
            ComputeLayout();
        }

        var enabled = Enabled;
        var active = enabled && (!_showCheckBox || _checked);
        var focused = Focused || _dropDownOpen;

        Color back, fore, border, arrow, buttonBack, selBack, selFore;
        if (_defaultStyle)
        {
            back = enabled ? SystemColors.Window : SystemColors.Control;
            fore = active ? SystemColors.WindowText : SystemColors.GrayText;
            border = focused || _hover ? SystemColors.Highlight : SystemColors.ControlDark;
            arrow = enabled ? SystemColors.WindowText : SystemColors.GrayText;
            buttonBack = back;
            selBack = SystemColors.Highlight;
            selFore = SystemColors.HighlightText;
        }
        else
        {
            back = enabled ? BackColor : _disabledBackColor;
            fore = active ? ForeColor : _disabledForeColor;
            border = enabled && (focused || _hover) ? _highlightColor : _borderColor;
            arrow = enabled ? _arrowColor : _disabledForeColor;
            buttonBack = enabled ? _buttonBackColor : _disabledBackColor;
            selBack = _selectedFieldBackColor;
            selFore = _selectedFieldForeColor;
        }

        g.Clear(back);

        // ramik + tlacidlo
        var drewStyled = false;
        if (_defaultStyle && UseVisualStyles)
            drewStyled = PaintVisualStyled(g, enabled, focused);
        if (!drewStyled)
        {
            if (_showUpDown)
                PaintSpin(g, buttonBack, arrow, border);
            else
                PaintDropButton(g, buttonBack, arrow, border);

            using var pen = new Pen(border);
            g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        // zaskrtavacie policko
        if (_showCheckBox)
        {
            if (_defaultStyle && UseVisualStyles)
            {
                var state = _checked
                    ? enabled ? CheckBoxState.CheckedNormal : CheckBoxState.CheckedDisabled
                    : enabled ? CheckBoxState.UncheckedNormal : CheckBoxState.UncheckedDisabled;
                CheckBoxRenderer.DrawCheckBox(g, _checkRect.Location, state);
            }
            else
            {
                using var brush = new SolidBrush(_defaultStyle ? SystemColors.Window : buttonBack);
                using var pen = new Pen(_defaultStyle ? SystemColors.ControlDark : enabled ? _borderColor : _disabledForeColor);
                g.FillRectangle(brush, _checkRect);
                g.DrawRectangle(pen, _checkRect.X, _checkRect.Y, _checkRect.Width - 1, _checkRect.Height - 1);
                if (_checked)
                    ExButtonRenderer.DrawCheckMark(g, _checkRect, enabled ? fore : _disabledForeColor);
            }

            if (_checkBoxSelected && focused)
                ControlPaint.DrawFocusRectangle(g, Rectangle.Inflate(_checkRect, 1, 1));
        }

        // polia
        var x = _textRect.Left;
        g.SetClip(_textRect);
        for (var i = 0; i < _fields.Count; i++)
        {
            var f = _fields[i];
            var size = new Size(widths[i], _textRect.Height);
            f.Bounds = new Rectangle(x, _textRect.Top, size.Width, _textRect.Height);
            var textFore = fore;
            if (i == _selectedField && focused && active && !_checkBoxSelected)
            {
                // vyber siaha od konca predchadzajuceho pola (pri prvom od okraja) a ma zvisly odstup 3 px od ramu
                var selLeft = i == 0 ? _textRect.Left - LogicalToDeviceUnits(5) : _fields[i - 1].Bounds.Right;
                var inset = LogicalToDeviceUnits(3);
                var sel = new Rectangle(selLeft, _textRect.Top + inset, x + size.Width - selLeft, _textRect.Height - 2 * inset);
                using var brush = new SolidBrush(selBack);
                g.FillRectangle(brush, sel);
                textFore = selFore;
            }

            TextRenderer.DrawText(g, f.Text, Font, f.Bounds, textFore, flags);
            x += size.Width;
        }

        g.ResetClip();
        base.OnPaint(e);
    }

    private bool PaintVisualStyled(Graphics g, bool enabled, bool focused)
    {
        // DATEPICKER: DP_DATEBORDER = 2 (1 normal, 2 hot, 3 focused, 4 disabled), DP_SHOWCALENDARBUTTONRIGHT = 3
        var borderState = !enabled ? 4 : focused ? 3 : _hover ? 2 : 1;
        if (!TryRenderer("DATEPICKER", 2, borderState, out var borderRenderer))
            return false;

        borderRenderer!.DrawBackground(g, ClientRectangle);

        if (_showUpDown)
        {
            var up = _buttonRect with { Height = _buttonRect.Height / 2 };
            var down = _buttonRect with { Y = up.Bottom, Height = _buttonRect.Height - up.Height };
            var upEl = !enabled ? VisualStyleElement.Spin.Up.Disabled : _pressedPart == Part.SpinUp ? VisualStyleElement.Spin.Up.Pressed : _hoverPart == Part.SpinUp ? VisualStyleElement.Spin.Up.Hot : VisualStyleElement.Spin.Up.Normal;
            var downEl = !enabled ? VisualStyleElement.Spin.Down.Disabled : _pressedPart == Part.SpinDown ? VisualStyleElement.Spin.Down.Pressed : _hoverPart == Part.SpinDown ? VisualStyleElement.Spin.Down.Hot : VisualStyleElement.Spin.Down.Normal;
            if (!VisualStyleRenderer.IsElementDefined(upEl) || !VisualStyleRenderer.IsElementDefined(downEl))
                return false;
            new VisualStyleRenderer(upEl).DrawBackground(g, up);
            new VisualStyleRenderer(downEl).DrawBackground(g, down);
        }
        else
        {
            var buttonState = !enabled ? 4 : _pressedPart == Part.DropButton || _dropDownOpen ? 3 : _hoverPart == Part.DropButton ? 2 : 1;
            if (_narrowButton)
            {
                var el = buttonState switch
                {
                    4 => VisualStyleElement.ComboBox.DropDownButton.Disabled,
                    3 => VisualStyleElement.ComboBox.DropDownButton.Pressed,
                    2 => VisualStyleElement.ComboBox.DropDownButton.Hot,
                    _ => VisualStyleElement.ComboBox.DropDownButton.Normal
                };
                if (!VisualStyleRenderer.IsElementDefined(el))
                    return false;
                new VisualStyleRenderer(el).DrawBackground(g, _buttonRect);
                return true;
            }

            if (!TryRenderer("DATEPICKER", 3, buttonState, out var buttonRenderer))
                return false;
            buttonRenderer!.DrawBackground(g, _buttonRect);
        }

        return true;
    }

    private void PaintDropButton(Graphics g, Color buttonBack, Color arrow, Color border)
    {
        var pressed = _pressedPart == Part.DropButton || _dropDownOpen;
        var back = pressed && !_defaultStyle ? _highlightColor : buttonBack;
        var arrowColor = pressed && !_defaultStyle ? _selectedFieldForeColor : arrow;
        var buttonBorder = _hoverPart == Part.DropButton || pressed ? border : back;
        if (_narrowButton)
        {
            using (var brush = new SolidBrush(back))
                g.FillRectangle(brush, _buttonRect);
            ExButtonRenderer.DrawTriangle(g, arrowColor, new Point(_buttonRect.X + _buttonRect.Width / 2, _buttonRect.Y + _buttonRect.Height / 2), LogicalToDeviceUnits(3), ArrowDirection.Down);
            using var pen = new Pen(buttonBorder);
            g.DrawRectangle(pen, _buttonRect.X, _buttonRect.Y, _buttonRect.Width - 1, _buttonRect.Height - 1);
            return;
        }

        ExButtonRenderer.DrawCalendarButton(g, _buttonRect, back, buttonBorder, arrowColor, DeviceDpi / 96f);
    }

    private void PaintSpin(Graphics g, Color buttonBack, Color arrow, Color border)
    {
        var up = _buttonRect with { Height = _buttonRect.Height / 2 };
        var down = _buttonRect with { Y = up.Bottom, Height = _buttonRect.Height - up.Height };
        PaintSpinButton(g, up, true, _pressedPart == Part.SpinUp, _hoverPart == Part.SpinUp, buttonBack, arrow, border);
        PaintSpinButton(g, down, false, _pressedPart == Part.SpinDown, _hoverPart == Part.SpinDown, buttonBack, arrow, border);
    }

    private void PaintSpinButton(Graphics g, Rectangle rect, bool up, bool pressed, bool hover, Color buttonBack, Color arrow, Color border)
    {
        var back = pressed && !_defaultStyle ? _highlightColor : buttonBack;
        var arrowColor = pressed && !_defaultStyle ? _selectedFieldForeColor : arrow;
        using (var brush = new SolidBrush(back))
            g.FillRectangle(brush, rect);
        if (hover || pressed)
        {
            using var pen = new Pen(border);
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        }

        var center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        ExButtonRenderer.DrawTriangle(g, arrowColor, center, LogicalToDeviceUnits(3), up ? ArrowDirection.Up : ArrowDirection.Down);
    }

    /// <inheritdoc />
    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        if (!Enabled) CloseDropDown();
        Invalidate();
    }

    #endregion

    #region Mouse

    private Part HitTest(Point pt)
    {
        ComputeLayout();
        if (_showCheckBox && _checkRect.Contains(pt)) return Part.CheckBox;
        if (_buttonRect.Contains(pt))
        {
            if (!_showUpDown) return Part.DropButton;
            return pt.Y < _buttonRect.Y + _buttonRect.Height / 2 ? Part.SpinUp : Part.SpinDown;
        }

        return _textRect.Contains(pt) ? Part.Text : Part.None;
    }

    private int HitTestField(Point pt)
    {
        for (var i = 0; i < _fields.Count; i++)
            if (_fields[i].Editable && _fields[i].Bounds.Contains(pt))
                return i;
        // klik za posledne pole vyberie najblizsie editovatelne pole zlava
        var best = -1;
        for (var i = 0; i < _fields.Count; i++)
            if (_fields[i].Editable && _fields[i].Bounds.Left <= pt.X)
                best = i;
        return best;
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _hover = true;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _hover = false;
        _hoverPart = Part.None;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        var part = HitTest(e.Location);
        if (part == _hoverPart)
            return;
        _hoverPart = part;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left || !Enabled)
            return;
        Focus();

        var part = HitTest(e.Location);
        switch (part)
        {
            case Part.CheckBox:
                CommitTyped();
                _checkBoxSelected = true;
                Checked = !Checked;
                break;
            case Part.DropButton:
                if (_dropDownOpen)
                    CloseDropDown();
                else if (Environment.TickCount - _dropDownClosedAt > 250)
                    ShowDropDown();
                break;
            case Part.SpinUp:
            case Part.SpinDown:
            {
                _pressedPart = part;
                StepField(part == Part.SpinUp ? 1 : -1);
                StartSpinTimer(part);
                Invalidate();
                break;
            }
            case Part.Text:
            {
                if (_showCheckBox && !_checked)
                    break;
                var idx = HitTestField(e.Location);
                if (idx >= 0) SelectField(idx);
                break;
            }
        }
    }

    /// <inheritdoc />
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        StopSpinTimer();
        if (_pressedPart == Part.None)
            return;
        _pressedPart = Part.None;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (e.Delta == 0 || !Focused)
            return;
        StepField(e.Delta > 0 ? 1 : -1);
        if (e is HandledMouseEventArgs h) h.Handled = true;
    }

    private void StartSpinTimer(Part part)
    {
        _spinTimer ??= new System.Windows.Forms.Timer();
        _spinTimer.Stop();
        _spinTimer.Interval = 400;
        _spinTimer.Tick -= SpinTimer_Tick;
        _spinTimer.Tick += SpinTimer_Tick;
        _spinTimer.Tag = part;
        _spinTimer.Start();
    }

    private void StopSpinTimer() => _spinTimer?.Stop();

    private void SpinTimer_Tick(object? sender, EventArgs e)
    {
        if (_spinTimer is null || _pressedPart == Part.None)
        {
            StopSpinTimer();
            return;
        }

        _spinTimer.Interval = 60;
        StepField(_pressedPart == Part.SpinUp ? 1 : -1);
    }

    #endregion

    #region Keyboard & focus

    /// <inheritdoc />
    protected override bool IsInputKey(Keys keyData)
    {
        switch (keyData & Keys.KeyCode)
        {
            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
            case Keys.Home:
            case Keys.End:
            case Keys.F4:
                return true;
        }

        return base.IsInputKey(keyData);
    }

    /// <inheritdoc />
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Alt | Keys.Down) || keyData == (Keys.Alt | Keys.Up))
        {
            if (!_showUpDown && Enabled)
            {
                if (_dropDownOpen) CloseDropDown();
                else ShowDropDown();
            }

            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Handled)
            return;

        switch (e.KeyCode)
        {
            case Keys.Left:
                MoveSelection(-1);
                break;
            case Keys.Right:
                MoveSelection(1);
                break;
            case Keys.Up:
                StepField(1);
                break;
            case Keys.Down:
                StepField(-1);
                break;
            case Keys.Home when _selectedField >= 0 && _fields[_selectedField].Numeric:
                CommitTyped();
                SetFieldValue(_fields[_selectedField], FieldMin(_fields[_selectedField]));
                break;
            case Keys.End when _selectedField >= 0 && _fields[_selectedField].Numeric:
                CommitTyped();
                SetFieldValue(_fields[_selectedField], FieldMax(_fields[_selectedField]));
                break;
            case Keys.F4:
                if (!_showUpDown && Enabled)
                {
                    if (_dropDownOpen) CloseDropDown();
                    else ShowDropDown();
                }

                break;
            case Keys.Space when _showCheckBox && _checkBoxSelected:
                Checked = !Checked;
                break;
            default:
                return;
        }

        e.Handled = true;
    }

    /// <inheritdoc />
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);
        if (e.Handled)
            return;
        if (_checkBoxSelected)
            return;

        if (char.IsDigit(e.KeyChar))
        {
            TypeDigit(e.KeyChar - '0');
            e.Handled = true;
        }
        else if (char.IsLetter(e.KeyChar))
        {
            TypeLetter(e.KeyChar);
            e.Handled = true;
        }
    }

    /// <inheritdoc />
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        if (_selectedField < 0 && _fields.Count > 0)
            _selectedField = _fields.FindIndex(f => f.Editable);
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        CommitTyped();
        Invalidate();
    }

    #endregion

    #region Drop-down

    private void EnsureDropDown()
    {
        if (_dropDown is not null)
            return;
        _host = new ToolStripControlHost(Calendar) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false };
        _dropDown = new ToolStripDropDown
        {
            AutoClose = true,
            AutoSize = true,
            Padding = Padding.Empty,
            Margin = Padding.Empty,
            DropShadowEnabled = true,
            Renderer = new NoBorderRenderer()
        };
        _dropDown.Items.Add(_host);
        _dropDown.Closed += DropDown_Closed;
    }

    /// <summary>
    ///     Shows the drop-down calendar.
    /// </summary>
    public void ShowDropDown()
    {
        if (_dropDownOpen || _showUpDown || !Enabled || !IsHandleCreated)
            return;
        CommitTyped();
        EnsureDropDown();

        Calendar.MinDate = _minDate.Date;
        Calendar.MaxDate = _maxDate.Date;
        Calendar.SelectionDate = _value.Date;
        Calendar.EnsureVisible(_value.Date);
        Calendar.View = ExCalendarView.Month;
        _host!.Size = Calendar.Size;
        _dropDown!.BackColor = Calendar.BackColor;

        _dropDownOpen = true;
        Invalidate();
        OnDropDown(EventArgs.Empty);

        if (_dropDownAlign == LeftRightAlignment.Right)
            _dropDown.Show(this, new Point(Width, Height), ToolStripDropDownDirection.BelowLeft);
        else
            _dropDown.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
        Calendar.Focus();
    }

    /// <summary>
    ///     Closes the drop-down calendar.
    /// </summary>
    public void CloseDropDown()
    {
        if (_dropDown is { Visible: true })
            _dropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
    }

    private void DropDown_Closed(object? sender, ToolStripDropDownClosedEventArgs e)
    {
        _dropDownOpen = false;
        _dropDownClosedAt = Environment.TickCount;
        Invalidate();
        OnCloseUp(EventArgs.Empty);
        if (e.CloseReason != ToolStripDropDownCloseReason.AppFocusChange && e.CloseReason != ToolStripDropDownCloseReason.AppClicked)
            Focus();
    }

    private void Calendar_DateSelected(object? sender, EventArgs e)
    {
        var d = Calendar.SelectionDate;
        Value = d.Add(_value.TimeOfDay);
        if (_showCheckBox) Checked = true;
        CloseDropDown();
    }

    #endregion

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _spinTimer?.Dispose();
            _spinTimer = null;
            Calendar.DateSelected -= Calendar_DateSelected;
            if (_dropDown is not null)
            {
                _dropDown.Closed -= DropDown_Closed;
                _dropDown.Dispose(); // zlikviduje aj host a kalendar
                _dropDown = null;
                _host = null;
            }
            else
            {
                Calendar.Dispose();
            }
        }

        base.Dispose(disposing);
    }
}
