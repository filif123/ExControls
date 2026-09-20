using System.Globalization;
using ExControls.Controls;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace ExControls;

/// <summary>
///     Month calendar control drawn completely by the library (no Win32 MonthCalendar).
///     Used standalone or as the drop-down part of <see cref="ExDateTimePicker" />.
/// </summary>
[ToolboxBitmap(typeof(MonthCalendar), "MonthCalendar.bmp")]
[DefaultProperty(nameof(SelectionDate))]
[DefaultEvent(nameof(DateChanged))]
public class ExCalendar : Control, IExControl
{
    private const int Columns = 7;
    private const int Rows = 6;
    private const int CellCount = Columns * Rows;

    private enum Part
    {
        None,
        PrevArrow,
        NextArrow,
        Header,
        Cell,
        Today
    }

    private bool _defaultStyle;
    private DateTime _selectionDate;
    private DateTime _displayMonth;
    private DateTime _minDate;
    private DateTime _maxDate;
    private DateTime? _todayDate;
    private Day _firstDayOfWeek;
    private bool _showToday;
    private bool _showWeekNumbers;
    private bool _drawBorder;
    private string _todayText;

    private Color _borderColor;
    private Color _headerForeColor;
    private Color _dayOfWeekForeColor;
    private Color _trailingForeColor;
    private Color _disabledForeColor;
    private Color _weekNumberForeColor;
    private Color _highlightColor;
    private Color _highlightForeColor;
    private Color _hoverBackColor;
    private Color _todayBorderColor;
    private Color _arrowColor;

    // rozlozenie
    private Font? _boldFont;
    private bool _layoutDirty = true;
    private Size _cellSize;
    private int _padding;
    private Rectangle _prevRect;
    private Rectangle _nextRect;
    private Rectangle _headerRect;
    private Rectangle _dayNamesRect;
    private Rectangle _gridRect;
    private Rectangle _todayRect;
    private readonly Rectangle[] _cellRects = new Rectangle[CellCount];
    private readonly DateTime[] _cellDates = new DateTime[CellCount];

    // pohlady rok/dekada/storocie: 4 x 3 poloziek na mieste mriezky dni
    private const int ViewItems = 12;
    private ExCalendarView _view = ExCalendarView.Month;
    private Rectangle _viewRect;
    private readonly Rectangle[] _viewRects = new Rectangle[ViewItems];
    private int _viewCursor = -1;

    // stav mysi
    private Part _hoverPart = Part.None;
    private int _hoverCell = -1;

    /// <summary>
    ///     Constructor.
    /// </summary>
    public ExCalendar()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserMouse, true);
        SetStyle(ControlStyles.StandardDoubleClick, false);

        _defaultStyle = true;
        _selectionDate = DateTime.Today;
        _displayMonth = new DateTime(_selectionDate.Year, _selectionDate.Month, 1);
        _minDate = new DateTime(1753, 1, 1);
        _maxDate = new DateTime(9998, 12, 31);
        _firstDayOfWeek = Day.Default;
        _showToday = true;
        _drawBorder = true;
        _todayText = "Today:";

        _borderColor = Color.DimGray;
        _headerForeColor = Color.Black;
        _dayOfWeekForeColor = Color.Black;
        _trailingForeColor = Color.Gray;
        _disabledForeColor = Color.Silver;
        _weekNumberForeColor = Color.Gray;
        _highlightColor = SystemColors.Highlight;
        _highlightForeColor = SystemColors.HighlightText;
        _hoverBackColor = Color.Gainsboro;
        _todayBorderColor = SystemColors.Highlight;
        _arrowColor = Color.Black;

        base.AutoSize = true;
        base.BackColor = Color.White;
        base.ForeColor = Color.Black;
    }

    #region Properties - data

    /// <summary>
    ///     Selected date (time part is ignored).
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [ExDescription("Selected date.")]
    [Bindable(true)]
    public DateTime SelectionDate
    {
        get => _selectionDate;
        set
        {
            value = value.Date;
            if (value < _minDate) value = _minDate;
            if (value > _maxDate) value = _maxDate;
            if (_selectionDate == value)
                return;
            _selectionDate = value;
            EnsureVisible(value);
            Invalidate();
            OnDateChanged(EventArgs.Empty);
        }
    }

    private bool ShouldSerializeSelectionDate() => _selectionDate != DateTime.Today;
    private void ResetSelectionDate() => SelectionDate = DateTime.Today;

    /// <summary>
    ///     First day of the displayed month.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime DisplayMonth
    {
        get => _displayMonth;
        set
        {
            value = new DateTime(value.Year, value.Month, 1);
            var min = new DateTime(_minDate.Year, _minDate.Month, 1);
            var max = new DateTime(_maxDate.Year, _maxDate.Month, 1);
            if (value < min) value = min;
            if (value > max) value = max;
            if (_displayMonth == value)
                return;
            _displayMonth = value;
            _hoverCell = -1;
            ComputeCellDates();
            Invalidate();
            OnDisplayMonthChanged(EventArgs.Empty);
        }
    }

    /// <summary>
    ///     Current view: days of a month, months of a year, years of a decade or decades of a century.
    ///     Clicking the header switches to the next coarser view, clicking an item goes back down.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExCalendarView View
    {
        get => _view;
        set
        {
            if (_view == value)
                return;
            _view = value;
            _hoverCell = -1;
            _viewCursor = CursorForDisplayMonth();
            Invalidate();
        }
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
            value = value.Date;
            if (value > _maxDate)
                throw new ArgumentOutOfRangeException(nameof(value), "MinDate must be less than or equal to MaxDate.");
            if (_minDate == value)
                return;
            _minDate = value;
            if (_selectionDate < value) SelectionDate = value;
            DisplayMonth = _displayMonth;
            Invalidate();
        }
    }

    private bool ShouldSerializeMinDate() => _minDate != new DateTime(1753, 1, 1);
    private void ResetMinDate() => MinDate = new DateTime(1753, 1, 1);

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
            value = value.Date;
            if (value < _minDate)
                throw new ArgumentOutOfRangeException(nameof(value), "MaxDate must be greater than or equal to MinDate.");
            if (_maxDate == value)
                return;
            _maxDate = value;
            if (_selectionDate > value) SelectionDate = value;
            DisplayMonth = _displayMonth;
            Invalidate();
        }
    }

    private bool ShouldSerializeMaxDate() => _maxDate != new DateTime(9998, 12, 31);
    private void ResetMaxDate() => MaxDate = new DateTime(9998, 12, 31);

    /// <summary>
    ///     Date treated as "today". When not set, <see cref="DateTime.Today" /> is used.
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [ExDescription("Date treated as today. When not set, the system date is used.")]
    public DateTime TodayDate
    {
        get => _todayDate ?? DateTime.Today;
        set
        {
            var v = value.Date;
            if (_todayDate == v)
                return;
            _todayDate = v;
            Invalidate();
        }
    }

    private bool ShouldSerializeTodayDate() => _todayDate.HasValue;
    private void ResetTodayDate()
    {
        _todayDate = null;
        Invalidate();
    }

    /// <summary>
    ///     First day of the week. <see cref="Day.Default" /> uses the current culture.
    /// </summary>
    [ExCategory(CategoryType.Behavior)]
    [DefaultValue(Day.Default)]
    [ExDescription("First day of the week. Default uses the current culture.")]
    public Day FirstDayOfWeek
    {
        get => _firstDayOfWeek;
        set
        {
            if (_firstDayOfWeek == value)
                return;
            _firstDayOfWeek = value;
            ComputeCellDates();
            Invalidate();
        }
    }

    /// <summary>
    ///     Shows the "Today" line at the bottom of the calendar.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [ExDescription("Shows the Today line at the bottom of the calendar.")]
    public bool ShowToday
    {
        get => _showToday;
        set
        {
            if (_showToday == value)
                return;
            _showToday = value;
            InvalidateLayout();
        }
    }

    /// <summary>
    ///     Shows week numbers in the first column.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(false)]
    [ExDescription("Shows week numbers in the first column.")]
    public bool ShowWeekNumbers
    {
        get => _showWeekNumbers;
        set
        {
            if (_showWeekNumbers == value)
                return;
            _showWeekNumbers = value;
            InvalidateLayout();
        }
    }

    /// <summary>
    ///     Draws a one pixel border around the control.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(true)]
    [ExDescription("Draws a one pixel border around the control.")]
    public bool DrawBorder
    {
        get => _drawBorder;
        set
        {
            if (_drawBorder == value)
                return;
            _drawBorder = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Label shown before today's date in the "Today" line.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue("Today:")]
    [Localizable(true)]
    [ExDescription("Label shown before today's date in the Today line.")]
    public string TodayText
    {
        get => _todayText;
        set
        {
            value ??= string.Empty;
            if (_todayText == value)
                return;
            _todayText = value;
            Invalidate();
        }
    }

    /// <inheritdoc />
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(true)]
    public override bool AutoSize
    {
        get => base.AutoSize;
        set
        {
            base.AutoSize = value;
            if (value) AdjustSize();
        }
    }

    /// <summary>
    ///     Not used.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    // Control.Text je v BCL [AllowNull]; atribut sa na net48 neda pouzit (CS0122), preto pragma.
#pragma warning disable CS8765
    public override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }
#pragma warning restore CS8765

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
    ///     Color of the month/year header text.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the month/year header text.")]
    public Color HeaderForeColor
    {
        get => _headerForeColor;
        set
        {
            if (_headerForeColor == value)
                return;
            _headerForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the day-of-week names.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the day-of-week names.")]
    public Color DayOfWeekForeColor
    {
        get => _dayOfWeekForeColor;
        set
        {
            if (_dayOfWeekForeColor == value)
                return;
            _dayOfWeekForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the days that belong to the previous or next month.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Gray")]
    [ExDescription("Color of the days that belong to the previous or next month.")]
    public Color TrailingForeColor
    {
        get => _trailingForeColor;
        set
        {
            if (_trailingForeColor == value)
                return;
            _trailingForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the days outside of the MinDate-MaxDate range and of all text when the control is disabled.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Silver")]
    [ExDescription("Color of the days outside of the MinDate-MaxDate range and of all text when the control is disabled.")]
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
    ///     Color of the week numbers.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Gray")]
    [ExDescription("Color of the week numbers.")]
    public Color WeekNumberForeColor
    {
        get => _weekNumberForeColor;
        set
        {
            if (_weekNumberForeColor == value)
                return;
            _weekNumberForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the selected day.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Background color of the selected day.")]
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
    ///     Text color of the selected day.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "HighlightText")]
    [ExDescription("Text color of the selected day.")]
    public Color HighlightForeColor
    {
        get => _highlightForeColor;
        set
        {
            if (_highlightForeColor == value)
                return;
            _highlightForeColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Background color of the day or arrow under the mouse.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Gainsboro")]
    [ExDescription("Background color of the day or arrow under the mouse.")]
    public Color HoverBackColor
    {
        get => _hoverBackColor;
        set
        {
            if (_hoverBackColor == value)
                return;
            _hoverBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the frame around today's date.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color of the frame around today's date.")]
    public Color TodayBorderColor
    {
        get => _todayBorderColor;
        set
        {
            if (_todayBorderColor == value)
                return;
            _todayBorderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the previous/next month arrows.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the previous/next month arrows.")]
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

    #endregion

    #region Events

    /// <summary>Occurs when <see cref="SelectionDate" /> changes.</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the SelectionDate property changes.")]
    public event EventHandler? DateChanged;

    /// <summary>Occurs when the user picks a date (mouse click, Enter or the Today line).</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the user picks a date with the mouse or keyboard.")]
    public event EventHandler? DateSelected;

    /// <summary>Occurs when <see cref="DisplayMonth" /> changes.</summary>
    [ExCategory(CategoryType.Action)]
    [ExDescription("Occurs when the displayed month changes.")]
    public event EventHandler? DisplayMonthChanged;

    /// <summary>Raises the <see cref="DateChanged" /> event.</summary>
    protected virtual void OnDateChanged(EventArgs e) => DateChanged?.Invoke(this, e);

    /// <summary>Raises the <see cref="DateSelected" /> event.</summary>
    protected virtual void OnDateSelected(EventArgs e) => DateSelected?.Invoke(this, e);

    /// <summary>Raises the <see cref="DisplayMonthChanged" /> event.</summary>
    protected virtual void OnDisplayMonthChanged(EventArgs e) => DisplayMonthChanged?.Invoke(this, e);

    /// <summary>Raises the <see cref="DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged() => DefaultStyleChanged?.Invoke(this, EventArgs.Empty);

    #endregion

    #region Public methods

    /// <summary>
    ///     Scrolls the calendar so that the month of the date is visible.
    /// </summary>
    public void EnsureVisible(DateTime date) => DisplayMonth = new DateTime(date.Year, date.Month, 1);

    /// <summary>
    ///     Selects today's date and raises <see cref="DateSelected" />.
    /// </summary>
    public void SelectToday() => Select(TodayDate);

    /// <summary>
    ///     Returns the date at the specified client point, or null when there is no day.
    /// </summary>
    public DateTime? HitTestDate(Point pt)
    {
        EnsureLayout();
        var idx = HitTestCell(pt);
        return idx < 0 ? null : _cellDates[idx];
    }

    #endregion

    #region Layout

    /// <inheritdoc />
    protected override Size DefaultSize => new(180, 165);

    /// <inheritdoc />
    public override Size GetPreferredSize(Size proposedSize)
    {
        EnsureLayout();
        var cols = Columns + (_showWeekNumbers ? 1 : 0);
        var w = _padding * 2 + cols * _cellSize.Width;
        var h = _padding * 2 + _cellSize.Height * (2 + Rows) + (_showToday ? _cellSize.Height + _padding : 0);
        return new Size(w, h);
    }

    private void InvalidateLayout()
    {
        _layoutDirty = true;
        AdjustSize();
        Invalidate();
    }

    private void AdjustSize()
    {
        if (!AutoSize)
            return;
        var size = GetPreferredSize(Size.Empty);
        if (Size != size)
            Size = size;
    }

    /// <inheritdoc />
    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (AutoSize)
        {
            var size = GetPreferredSize(Size.Empty);
            width = size.Width;
            height = size.Height;
        }

        base.SetBoundsCore(x, y, width, height, specified);
    }

    /// <inheritdoc />
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        _boldFont?.Dispose();
        _boldFont = null;
        InvalidateLayout();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _boldFont?.Dispose();
            _boldFont = null;
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc />
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        InvalidateLayout();
    }

    /// <inheritdoc />
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        _layoutDirty = true;
    }

    /// <inheritdoc />
    protected override void RescaleConstantsForDpi(int deviceDpiOld, int deviceDpiNew)
    {
        base.RescaleConstantsForDpi(deviceDpiOld, deviceDpiNew);
        InvalidateLayout();
    }

    private void EnsureLayout()
    {
        if (!_layoutDirty)
            return;
        _layoutDirty = false;

        _padding = LogicalToDeviceUnits(4);
        var textSize = TextRenderer.MeasureText("30", Font);
        // sirka bunky musi pojat aj najsirsiu skratku dna v tyzdni
        var maxDayName = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames.Max(n => TextRenderer.MeasureText(n, Font).Width);
        _cellSize = new Size(Math.Max(textSize.Width, maxDayName) + LogicalToDeviceUnits(8), textSize.Height + LogicalToDeviceUnits(4));
        if (_cellSize.Width < _cellSize.Height) _cellSize.Width = _cellSize.Height;

        var cols = Columns + (_showWeekNumbers ? 1 : 0);
        var gridWidth = cols * _cellSize.Width;
        var left = _padding;
        var top = _padding;

        // hlavicka: sipky na krajoch, text medzi nimi
        _prevRect = new Rectangle(left, top, _cellSize.Width, _cellSize.Height);
        _nextRect = new Rectangle(left + gridWidth - _cellSize.Width, top, _cellSize.Width, _cellSize.Height);
        _headerRect = new Rectangle(_prevRect.Right, top, gridWidth - 2 * _cellSize.Width, _cellSize.Height);
        top += _cellSize.Height;

        _dayNamesRect = new Rectangle(left, top, gridWidth, _cellSize.Height);
        top += _cellSize.Height;

        _gridRect = new Rectangle(left, top, gridWidth, Rows * _cellSize.Height);
        var dayLeft = left + (_showWeekNumbers ? _cellSize.Width : 0);
        for (var r = 0; r < Rows; r++)
        for (var c = 0; c < Columns; c++)
            _cellRects[r * Columns + c] = new Rectangle(dayLeft + c * _cellSize.Width, top + r * _cellSize.Height, _cellSize.Width, _cellSize.Height);
        top += Rows * _cellSize.Height;

        // pohlady rok/dekada/storocie zaberaju miesto nazvov dni + mriezky dni
        _viewRect = Rectangle.Union(_dayNamesRect, _gridRect);
        var vw = _viewRect.Width / 4;
        var vh = _viewRect.Height / 3;
        for (var r = 0; r < 3; r++)
        for (var c = 0; c < 4; c++)
            _viewRects[r * 4 + c] = new Rectangle(_viewRect.Left + c * vw, _viewRect.Top + r * vh, vw, vh);

        _todayRect = _showToday ? new Rectangle(left, top + _padding, gridWidth, _cellSize.Height) : Rectangle.Empty;

        ComputeCellDates();
    }

    private DayOfWeek EffectiveFirstDayOfWeek =>
        _firstDayOfWeek == Day.Default ? CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek : (DayOfWeek)_firstDayOfWeek;

    private void ComputeCellDates()
    {
        var first = _displayMonth;
        var offset = ((int)first.DayOfWeek - (int)EffectiveFirstDayOfWeek + 7) % 7;
        // vzdy 6 riadkov; ak mesiac zacina prvym dnom tyzdna, zobrazi sa cely predchadzajuci tyzden
        if (offset == 0) offset = 7;
        var start = first.AddDays(-offset);
        for (var i = 0; i < CellCount; i++)
            _cellDates[i] = start.AddDays(i);
    }

    #endregion

    #region Views (year / decade / century)

    private int DecadeStart => _displayMonth.Year / 10 * 10;
    private int CenturyStart => _displayMonth.Year / 100 * 100;

    /// <summary>
    ///     Index of the item that corresponds to the displayed month in the current view.
    /// </summary>
    private int CursorForDisplayMonth() => _view switch
    {
        ExCalendarView.Year => _displayMonth.Month - 1,
        ExCalendarView.Decade => _displayMonth.Year - DecadeStart + 1,
        ExCalendarView.Century => (_displayMonth.Year - CenturyStart) / 10 + 1,
        _ => -1
    };

    /// <summary>
    ///     First and last date covered by the item of the current view (month, year or decade).
    /// </summary>
    private (DateTime From, DateTime To, bool Trailing) ViewItemRange(int index)
    {
        switch (_view)
        {
            case ExCalendarView.Year:
            {
                var y = _displayMonth.Year;
                var from = new DateTime(y, index + 1, 1);
                return (from, from.AddMonths(1).AddDays(-1), false);
            }
            case ExCalendarView.Decade:
            {
                var y = DecadeStart - 1 + index;
                if (y < 1 || y > 9999) return (DateTime.MinValue, DateTime.MinValue, true);
                return (new DateTime(y, 1, 1), new DateTime(y, 12, 31), index is 0 or ViewItems - 1);
            }
            default:
            {
                var y = CenturyStart - 10 + index * 10;
                var to = Math.Min(9999, y + 9);
                if (y < 1 || y > 9999) return (DateTime.MinValue, DateTime.MinValue, true);
                return (new DateTime(y, 1, 1), new DateTime(to, 12, 31), index is 0 or ViewItems - 1);
            }
        }
    }

    private bool ViewItemEnabled(int index)
    {
        var (from, to, _) = ViewItemRange(index);
        return from != DateTime.MinValue && to >= _minDate && from <= _maxDate;
    }

    private string ViewItemText(int index)
    {
        var (from, to, _) = ViewItemRange(index);
        if (from == DateTime.MinValue)
            return string.Empty;
        return _view switch
        {
            ExCalendarView.Year => CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[from.Month - 1],
            ExCalendarView.Decade => from.Year.ToString(CultureInfo.InvariantCulture),
            _ => from.Year.ToString(CultureInfo.InvariantCulture) + "-\n" + to.Year.ToString(CultureInfo.InvariantCulture)
        };
    }

    private bool ViewItemSelected(int index)
    {
        var (from, to, _) = ViewItemRange(index);
        return from != DateTime.MinValue && _selectionDate >= from && _selectionDate <= to;
    }

    private string HeaderText()
    {
        var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
        return _view switch
        {
            ExCalendarView.Month => _displayMonth.ToString(dtfi.YearMonthPattern, dtfi),
            ExCalendarView.Year => _displayMonth.Year.ToString(CultureInfo.InvariantCulture),
            ExCalendarView.Decade => $"{DecadeStart}-{DecadeStart + 9}",
            _ => $"{CenturyStart}-{Math.Min(9999, CenturyStart + 99)}"
        };
    }

    /// <summary>
    ///     Moves the display by one page of the current view (month, year, decade, century).
    /// </summary>
    private void Page(int direction)
    {
        var years = _view switch
        {
            ExCalendarView.Year => 1,
            ExCalendarView.Decade => 10,
            ExCalendarView.Century => 100,
            _ => 0
        };
        if (years == 0)
        {
            DisplayMonth = _displayMonth.AddMonths(direction);
            return;
        }

        var y = _displayMonth.Year + direction * years;
        if (y < 1 || y > 9999)
            return;
        DisplayMonth = new DateTime(y, _displayMonth.Month, 1);
        _viewCursor = CursorForDisplayMonth();
    }

    /// <summary>
    ///     Header click: switches to the next coarser view.
    /// </summary>
    private void ZoomOut()
    {
        if (_view == ExCalendarView.Century)
            return;
        View = _view + 1;
    }

    /// <summary>
    ///     Item click in a year/decade/century view: goes one level down to the picked month/year/decade.
    /// </summary>
    private void ActivateViewItem(int index)
    {
        if (index < 0 || index >= ViewItems || !ViewItemEnabled(index))
            return;
        var (from, to, _) = ViewItemRange(index);
        var month = _displayMonth < from ? from : _displayMonth > to ? to : _displayMonth;
        DisplayMonth = new DateTime(month.Year, month.Month, 1);
        View = _view - 1;
    }

    private bool ViewCursorValid => _viewCursor is >= 0 and < ViewItems;

    #endregion

    #region Painting

    private struct Palette
    {
        public Color Back, Fore, Border, Header, DayNames, Trailing, Disabled, WeekNumber, Highlight, HighlightFore, Hover, TodayBorder, Arrow;
    }

    private Palette GetPalette()
    {
        if (_defaultStyle)
            return new Palette
            {
                Back = SystemColors.Window,
                Fore = SystemColors.WindowText,
                Border = SystemColors.ControlDark,
                Header = SystemColors.WindowText,
                DayNames = SystemColors.WindowText,
                Trailing = SystemColors.GrayText,
                Disabled = SystemColors.GrayText,
                WeekNumber = SystemColors.GrayText,
                Highlight = SystemColors.Highlight,
                HighlightFore = SystemColors.HighlightText,
                Hover = ControlPaint.Light(SystemColors.Highlight, 1.4f),
                TodayBorder = SystemColors.Highlight,
                Arrow = SystemColors.WindowText
            };

        return new Palette
        {
            Back = BackColor,
            Fore = ForeColor,
            Border = _borderColor,
            Header = _headerForeColor,
            DayNames = _dayOfWeekForeColor,
            Trailing = _trailingForeColor,
            Disabled = _disabledForeColor,
            WeekNumber = _weekNumberForeColor,
            Highlight = _highlightColor,
            HighlightFore = _highlightForeColor,
            Hover = _hoverBackColor,
            TodayBorder = _todayBorderColor,
            Arrow = _arrowColor
        };
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        EnsureLayout();
        var g = e.Graphics;
        var p = GetPalette();
        const TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
        var enabled = Enabled;

        g.Clear(p.Back);

        // hlavicka (text sa pri hoveri zvyrazni, lebo je klikatelny - okrem najvyssieho pohladu)
        DrawArrow(g, _prevRect, false, enabled ? p.Arrow : p.Disabled, _hoverPart == Part.PrevArrow && enabled ? p.Hover : Color.Empty);
        DrawArrow(g, _nextRect, true, enabled ? p.Arrow : p.Disabled, _hoverPart == Part.NextArrow && enabled ? p.Hover : Color.Empty);
        if (_hoverPart == Part.Header && enabled && _view != ExCalendarView.Century)
        {
            using var hb = new SolidBrush(p.Hover);
            g.FillRectangle(hb, _headerRect);
        }

        _boldFont ??= new Font(Font, FontStyle.Bold);
        TextRenderer.DrawText(g, HeaderText(), _boldFont, _headerRect, enabled ? p.Header : p.Disabled, flags);

        if (_view == ExCalendarView.Month)
            PaintDays(g, p, enabled, flags);
        else
            PaintViewItems(g, p, enabled);

        // dnes
        if (_showToday)
        {
            // stvorcek + text su centrovane ako celok (rovnako ako v nativnom MonthCalendar)
            const TextFormatFlags todayFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
            var today = TodayDate;
            var text = string.IsNullOrEmpty(_todayText) ? today.ToShortDateString() : _todayText + " " + today.ToShortDateString();
            var boxSize = LogicalToDeviceUnits(10);
            var textWidth = TextRenderer.MeasureText(g, text, Font, _todayRect.Size, todayFlags).Width;
            var total = boxSize + _padding + textWidth;
            var x = Math.Max(_todayRect.Left + _padding, _todayRect.Left + (_todayRect.Width - total) / 2);
            var box = new Rectangle(x, _todayRect.Top + (_cellSize.Height - boxSize) / 2, boxSize, boxSize);
            using (var pen = new Pen(enabled ? p.TodayBorder : p.Disabled))
                g.DrawRectangle(pen, box.X, box.Y, box.Width - 1, box.Height - 1);
            var textRect = new Rectangle(box.Right + _padding, _todayRect.Top, _todayRect.Right - _padding - box.Right - _padding, _todayRect.Height);
            var todayFore = !enabled ? p.Disabled : _hoverPart == Part.Today ? p.Highlight : p.Fore;
            TextRenderer.DrawText(g, text, Font, textRect, todayFore, todayFlags);
        }

        if (_drawBorder)
        {
            using var pen = new Pen(p.Border);
            g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        base.OnPaint(e);
    }

    private void PaintDays(Graphics g, Palette p, bool enabled, TextFormatFlags flags)
    {
        var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;

        // nazvy dni
        var names = dtfi.AbbreviatedDayNames;
        var firstDay = (int)EffectiveFirstDayOfWeek;
        var dayLeft = _dayNamesRect.Left + (_showWeekNumbers ? _cellSize.Width : 0);
        for (var c = 0; c < Columns; c++)
        {
            var rect = new Rectangle(dayLeft + c * _cellSize.Width, _dayNamesRect.Top, _cellSize.Width, _cellSize.Height);
            TextRenderer.DrawText(g, names[(firstDay + c) % 7], Font, rect, enabled ? p.DayNames : p.Disabled, flags);
        }

        // cisla tyzdnov
        if (_showWeekNumbers)
        {
            var cal = dtfi.Calendar;
            for (var r = 0; r < Rows; r++)
            {
                var week = cal.GetWeekOfYear(_cellDates[r * Columns], dtfi.CalendarWeekRule, dtfi.FirstDayOfWeek);
                var rect = new Rectangle(_gridRect.Left, _gridRect.Top + r * _cellSize.Height, _cellSize.Width, _cellSize.Height);
                TextRenderer.DrawText(g, week.ToString(CultureInfo.InvariantCulture), Font, rect, enabled ? p.WeekNumber : p.Disabled, flags);
            }
        }

        // dni
        var today = TodayDate;
        for (var i = 0; i < CellCount; i++)
        {
            var date = _cellDates[i];
            var rect = _cellRects[i];
            var inRange = date >= _minDate && date <= _maxDate;
            var selected = date == _selectionDate;
            var fore = !enabled || !inRange ? p.Disabled : date.Month != _displayMonth.Month ? p.Trailing : p.Fore;

            if (selected && enabled)
            {
                using var b = new SolidBrush(p.Highlight);
                g.FillRectangle(b, rect);
                fore = p.HighlightFore;
            }
            else if (i == _hoverCell && enabled && inRange)
            {
                using var b = new SolidBrush(p.Hover);
                g.FillRectangle(b, rect);
            }

            if (date == today)
            {
                using var pen = new Pen(enabled ? p.TodayBorder : p.Disabled);
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            }

            TextRenderer.DrawText(g, date.Day.ToString(CultureInfo.InvariantCulture), Font, rect, fore, flags);
        }
    }

    private void PaintViewItems(Graphics g, Palette p, bool enabled)
    {
        const TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;
        for (var i = 0; i < ViewItems; i++)
        {
            var rect = _viewRects[i];
            var (_, _, trailing) = ViewItemRange(i);
            var itemEnabled = enabled && ViewItemEnabled(i);
            var fore = !itemEnabled ? p.Disabled : trailing ? p.Trailing : p.Fore;

            if (ViewItemSelected(i) && enabled)
            {
                using var b = new SolidBrush(p.Highlight);
                g.FillRectangle(b, rect);
                fore = p.HighlightFore;
            }
            else if (i == _hoverCell && itemEnabled)
            {
                using var b = new SolidBrush(p.Hover);
                g.FillRectangle(b, rect);
            }

            if (i == _viewCursor && Focused && itemEnabled)
                ControlPaint.DrawFocusRectangle(g, rect, fore, p.Back);

            TextRenderer.DrawText(g, ViewItemText(i), Font, rect, fore, flags);
        }
    }

    private void DrawArrow(Graphics g, Rectangle rect, bool right, Color color, Color hoverBack)
    {
        if (!hoverBack.IsEmpty)
        {
            using var b = new SolidBrush(hoverBack);
            g.FillRectangle(b, rect);
        }

        var center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        ExButtonRenderer.DrawTriangle(g, color, center, LogicalToDeviceUnits(4), right ? ArrowDirection.Right : ArrowDirection.Left);
    }

    /// <inheritdoc />
    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        Invalidate();
    }

    #endregion

    #region Mouse

    private int HitTestCell(Point pt)
    {
        if (_view != ExCalendarView.Month)
        {
            if (!_viewRect.Contains(pt))
                return -1;
            for (var i = 0; i < ViewItems; i++)
                if (_viewRects[i].Contains(pt))
                    return i;
            return -1;
        }

        if (!_gridRect.Contains(pt))
            return -1;
        for (var i = 0; i < CellCount; i++)
            if (_cellRects[i].Contains(pt))
                return i;
        return -1;
    }

    private Part HitTest(Point pt, out int cell)
    {
        EnsureLayout();
        cell = -1;
        if (_prevRect.Contains(pt)) return Part.PrevArrow;
        if (_nextRect.Contains(pt)) return Part.NextArrow;
        if (_headerRect.Contains(pt)) return Part.Header;
        if (_showToday && _todayRect.Contains(pt)) return Part.Today;
        cell = HitTestCell(pt);
        return cell >= 0 ? Part.Cell : Part.None;
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        var part = HitTest(e.Location, out var cell);
        if (part == _hoverPart && cell == _hoverCell)
            return;
        _hoverPart = part;
        _hoverCell = cell;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (_hoverPart == Part.None && _hoverCell < 0)
            return;
        _hoverPart = Part.None;
        _hoverCell = -1;
        Invalidate();
    }

    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left)
            return;
        Focus();
        switch (HitTest(e.Location, out var cell))
        {
            case Part.PrevArrow:
                Page(-1);
                break;
            case Part.NextArrow:
                Page(1);
                break;
            case Part.Header:
                ZoomOut();
                break;
            case Part.Today:
                View = ExCalendarView.Month;
                SelectToday();
                break;
            case Part.Cell:
                if (_view == ExCalendarView.Month)
                    Select(_cellDates[cell]);
                else
                    ActivateViewItem(cell);
                break;
        }
    }

    /// <inheritdoc />
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (e.Delta == 0)
            return;
        Page(e.Delta > 0 ? -1 : 1);
        if (e is HandledMouseEventArgs h) h.Handled = true;
    }

    private void Select(DateTime date)
    {
        if (date < _minDate || date > _maxDate)
            return;
        SelectionDate = date;
        OnDateSelected(EventArgs.Empty);
    }

    #endregion

    #region Keyboard

    /// <inheritdoc />
    protected override bool IsInputKey(Keys keyData)
    {
        switch (keyData & Keys.KeyCode)
        {
            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
            case Keys.PageUp:
            case Keys.PageDown:
            case Keys.Home:
            case Keys.End:
            case Keys.Return:
                return true;
        }

        return base.IsInputKey(keyData);
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Handled)
            return;

        if (_view != ExCalendarView.Month)
        {
            OnViewKeyDown(e);
            return;
        }

        var d = _selectionDate;
        DateTime? target = e.KeyCode switch
        {
            Keys.Left => d.AddDays(-1),
            Keys.Right => d.AddDays(1),
            Keys.Up => d.AddDays(-7),
            Keys.Down => d.AddDays(7),
            Keys.PageUp => e.Control ? d.AddYears(-1) : d.AddMonths(-1),
            Keys.PageDown => e.Control ? d.AddYears(1) : d.AddMonths(1),
            Keys.Home => e.Control ? TodayDate : new DateTime(d.Year, d.Month, 1),
            Keys.End => new DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month)),
            _ => null
        };

        if (target.HasValue)
        {
            if (target.Value >= _minDate && target.Value <= _maxDate)
                SelectionDate = target.Value;
            e.Handled = true;
            return;
        }

        if (e.KeyCode is Keys.Return or Keys.Space)
        {
            OnDateSelected(EventArgs.Empty);
            e.Handled = true;
        }
    }

    /// <summary>
    ///     Keyboard in the year/decade/century views: arrows move the cursor, PgUp/PgDn page, Enter picks, Back zooms out.
    /// </summary>
    private void OnViewKeyDown(KeyEventArgs e)
    {
        if (!ViewCursorValid)
            _viewCursor = CursorForDisplayMonth();

        switch (e.KeyCode)
        {
            case Keys.Left:
                MoveViewCursor(-1);
                break;
            case Keys.Right:
                MoveViewCursor(1);
                break;
            case Keys.Up:
                MoveViewCursor(-4);
                break;
            case Keys.Down:
                MoveViewCursor(4);
                break;
            case Keys.PageUp:
                Page(-1);
                break;
            case Keys.PageDown:
                Page(1);
                break;
            case Keys.Return or Keys.Space:
                ActivateViewItem(_viewCursor);
                break;
            case Keys.Back:
                ZoomOut();
                break;
            default:
                return;
        }

        e.Handled = true;
    }

    private void MoveViewCursor(int delta)
    {
        var next = _viewCursor + delta;
        if (next < 0)
        {
            Page(-1);
            _viewCursor = Math.Max(0, Math.Min(ViewItems - 1, next + ViewItems));
        }
        else if (next >= ViewItems)
        {
            Page(1);
            _viewCursor = Math.Max(0, Math.Min(ViewItems - 1, next - ViewItems));
        }
        else
        {
            _viewCursor = next;
        }

        Invalidate();
    }

    #endregion
}

/// <summary>
///     Views of the <see cref="ExCalendar" />.
/// </summary>
public enum ExCalendarView
{
    /// <summary>Days of one month.</summary>
    Month,

    /// <summary>Months of one year.</summary>
    Year,

    /// <summary>Years of one decade.</summary>
    Decade,

    /// <summary>Decades of one century.</summary>
    Century
}
