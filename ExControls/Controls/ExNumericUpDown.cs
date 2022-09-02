using ExControls.Controls;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>
///     Expanded NumericUpDown Control
/// </summary>
[ToolboxBitmap(typeof(NumericUpDown), "NumericUpDown.bmp")]
public class ExNumericUpDown : NumericUpDown, IExControl
{
    private readonly UpDownButtons _newButtonUpDown;
    private readonly Control _originalButtonUpDown;
    private Color _arrowsColor;
    private Color _borderColor;

    private bool _defaultStyle;
    private Color _highlightColor;
    private bool _hover;
    private bool _selected;
    private Color _selectedButtonColor;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ExNumericUpDown()
    {
        _originalButtonUpDown = Controls[0];
        _newButtonUpDown = new UpDownButtons(this);
        var textBox = Controls[1];
        textBox.MouseEnter += TextBox_MouseEnter;
        textBox.MouseLeave += TextBox_MouseLeave;

        var defaultButtonsWidth = LogicalToDeviceUnits(16);
        _newButtonUpDown.TabStop = false;
        _newButtonUpDown.Size = new Size(defaultButtonsWidth, PreferredHeight);
        _newButtonUpDown.UpDown += OnUpDown;
        _newButtonUpDown.Dock = DockStyle.Right;

        _defaultStyle = true;
        _borderColor = Color.Gainsboro;
        _arrowsColor = Color.Black;
        _highlightColor = SystemColors.Highlight;
        _selectedButtonColor = SystemColors.Highlight;

        base.DoubleBuffered = true;

        Invalidate(true);
    }

    /// <summary>
    ///     Color of the Control's border when mouse is over the Control
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color of the Control's border when mouse is over the Control.")]
    public Color HighlightColor
    {
        get => _highlightColor;
        set
        {
            _highlightColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the arrows which is in this Control on the Up and Down buttons
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Black")]
    [ExDescription("Color of the arrows which is in this Control in the Up and Down buttons.")]
    public Color ArrowsColor
    {
        get => _arrowsColor;
        set
        {
            _arrowsColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color of the Control's border
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "Gainsboro")]
    [ExDescription("Color of the Control's border.")]
    public Color BorderColor
    {
        get => _borderColor;
        set
        {
            _borderColor = value;
            Invalidate();
        }
    }

    /// <summary>
    ///     Color the Up and Down buttons when they are selected
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(SystemColors), "Highlight")]
    [ExDescription("Color the Up and Down buttons when they are selected.")]
    public Color SelectedButtonColor
    {
        get => _selectedButtonColor;
        set
        {
            _selectedButtonColor = value;
            Invalidate();
        }
    }

    /// <summary>Gets or sets the background color for the text box portion of the spin box (also known as an up-down control).</summary>
    /// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of the text box portion of the spin box.</returns>
    public override Color BackColor
    {
        get => base.BackColor;
        set
        {
            if (base.BackColor == value)
                return;

            base.BackColor = value;
            _newButtonUpDown.BackColor = value;
            Invalidate(true);
        }
    }

    /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the BorderColor property changes.")]
    public event EventHandler DefaultStyleChanged;

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
            _defaultStyle = value;

            if (value)
            {
                Controls.Remove(_newButtonUpDown);
                Controls.Add(_originalButtonUpDown);
            }
            else
            {
                Controls.Remove(_originalButtonUpDown);
                Controls.Add(_newButtonUpDown);
                BorderStyle = BorderStyle.FixedSingle;
            }

            Invalidate();
        }
    }

    private void TextBox_MouseEnter(object sender, EventArgs e)
    {
        if (!_hover)
        {
            _hover = true;
            Invalidate();
        }
    }

    private void TextBox_MouseLeave(object sender, EventArgs e)
    {
        if (_hover)
        {
            _hover = false;
            Invalidate();
        }
    }

    private void OnUpDown(object source, UpDownEventArgs e)
    {
        if (e.ButtonID == 1)
        {
            UpButton();
        }
        else
        {
            if (e.ButtonID != 2)
                return;
            DownButton();
        }
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (DefaultStyle)
            return;

        DrawInternal(e.Graphics);
    }

    private void DrawInternal(Graphics g)
    {
        g.Clear(BackColor);
        using var p = new Pen(_hover || _selected ? HighlightColor : BorderColor);
        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(EventArgs eventargs)
    {
        if (DefaultStyle)
        {
            base.OnMouseEnter(eventargs);
            return;
        }

        if (!_hover)
        {
            _hover = true;
            Invalidate(true);
        }
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs eventargs)
    {
        if (DefaultStyle)
        {
            base.OnMouseLeave(eventargs);
            return;
        }

        if (_hover && !_selected)
        {
            _hover = false;
            Invalidate(true);
        }
    }

    /// <inheritdoc />
    protected override void OnGotFocus(EventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnGotFocus(e);
            return;
        }

        if (!_selected)
        {
            _hover = true;
            _selected = true;
            Invalidate(true);
        }
    }

    /// <inheritdoc />
    protected override void OnLostFocus(EventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnLostFocus(e);
            return;
        }

        if (_selected)
        {
            _hover = false;
            _selected = false;
            Invalidate(true);
        }
    }

    /// <inheritdoc />
    protected override void OnEnter(EventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnEnter(e);
            return;
        }

        if (!_selected)
        {
            _hover = true;
            _selected = true;
            Invalidate(true);
        }
    }

    /// <inheritdoc />
    protected override void OnLeave(EventArgs e)
    {
        if (DefaultStyle)
        {
            base.OnLeave(e);
            return;
        }

        if (_selected)
        {
            _hover = false;
            _selected = false;
            Invalidate(true);
        }
    }

    /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
    protected virtual void OnDefaultStyleChanged()
    {
        DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
    }

    private sealed class UpDownButtons : Control
    {
        private readonly ExNumericUpDown _parent;
        private ButtonId _captured;
        private bool _doubleClickFired;
        private ButtonId _mouseOver;
        private ButtonId _pushed;
        private Timer _timer;
        private int _timerInterval;
        private UpDownEventHandler _upDownEventHandler;

        internal UpDownButtons(ExNumericUpDown parent)
        {
            SetStyle(ControlStyles.FixedHeight, true);
            SetStyle(ControlStyles.FixedWidth, true);
            SetStyle(ControlStyles.Selectable, false);
            DoubleBuffered = true;
            _parent = parent;
        }

        public event UpDownEventHandler UpDown
        {
            add => _upDownEventHandler += value;
            remove => _upDownEventHandler -= value;
        }

        private void BeginButtonPress(MouseEventArgs e)
        {
            var num = Size.Height / 2;
            if (e.Y < num)
            {
                _pushed = _captured = ButtonId.Up;
                Invalidate();
            }
            else
            {
                _pushed = _captured = ButtonId.Down;
                Invalidate();
            }

            Capture = true;
            OnUpDown(new UpDownEventArgs((int)_pushed));
            StartTimer();
        }

        private void EndButtonPress()
        {
            _pushed = ButtonId.None;
            _captured = ButtonId.None;
            StopTimer();
            Capture = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left)
                BeginButtonPress(e);
            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
                _doubleClickFired = true;
            _parent.OnMouseDown(TranslateMouseEvent(this, e));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Capture)
            {
                var clientRectangle = ClientRectangle;
                clientRectangle.Height /= 2;
                if (_captured == ButtonId.Down)
                    clientRectangle.Y += clientRectangle.Height;
                if (clientRectangle.Contains(e.X, e.Y))
                {
                    if (_pushed != _captured)
                    {
                        StartTimer();
                        _pushed = _captured;
                        Invalidate();
                    }
                }
                else if (_pushed != ButtonId.None)
                {
                    StopTimer();
                    _pushed = ButtonId.None;
                    Invalidate();
                }
            }

            var clientRectangle1 = ClientRectangle;
            var clientRectangle2 = ClientRectangle;
            clientRectangle1.Height /= 2;
            clientRectangle2.Y += clientRectangle2.Height / 2;
            if (clientRectangle1.Contains(e.X, e.Y))
            {
                if (_mouseOver != ButtonId.Up)
                {
                    _mouseOver = ButtonId.Up;
                    Invalidate();
                }
            }
            else if (clientRectangle2.Contains(e.X, e.Y))
            {
                if (_mouseOver != ButtonId.Down)
                {
                    _mouseOver = ButtonId.Down;
                    Invalidate();
                }
            }

            _parent.OnMouseMove(TranslateMouseEvent(this, e));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                EndButtonPress();

            var screen = PointToScreen(new Point(e.X, e.Y));
            var e1 = TranslateMouseEvent(this, e);

            if (e.Button == MouseButtons.Left)
            {
                if (Win32.WindowFromPoint(new Win32.POINT(screen.X, screen.Y)) == Handle)
                {
                    if (!_doubleClickFired)
                    {
                        _parent.OnClick(e1);
                    }
                    else
                    {
                        _doubleClickFired = false;
                        _parent.OnDoubleClick(e1);
                        _parent.OnMouseDoubleClick(e1);
                    }
                }

                _doubleClickFired = false;
            }

            _parent.OnMouseUp(e1);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_mouseOver != ButtonId.None)
            {
                _mouseOver = ButtonId.None;
                Invalidate();
                _parent.OnMouseLeave(e);
            }
        }

        private MouseEventArgs TranslateMouseEvent(IWin32Window child, MouseEventArgs e)
        {
            if (child == null || !IsHandleCreated)
                return e;
            var pt = new Win32.POINT(e.X, e.Y);
            Win32.MapWindowPoints(child.Handle, Handle, ref pt, 1);
            return new MouseEventArgs(e.Button, e.Clicks, pt.X, pt.Y, e.Delta);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawInternal(e.Graphics);
        }

        private void DrawInternal(Graphics g)
        {
            g.Clear(_parent.BackColor);

            if (_pushed != ButtonId.None)
            {
                using var selected = new SolidBrush(_parent.SelectedButtonColor);
                if (_pushed == ButtonId.Up)
                    g.FillRectangle(selected, 0, 0, Width, Height / 2);
                else
                    g.FillRectangle(selected, 0, Height / 2, Width - 1, Height / 2);
            }

            using var sbUnhovered = new SolidBrush(_parent.BorderColor);
            using var sbHovered = new SolidBrush(_parent.HighlightColor);
            using var pUn = new Pen(sbUnhovered);
            using var pHo = new Pen(sbHovered);

            switch (_mouseOver)
            {
                case ButtonId.Up:
                    g.DrawRectangle(pUn, 0, Height / 2, Width - 1, Height / 2 - 1);
                    g.DrawRectangle(pHo, 0, 0, Width - 1, Height / 2);
                    break;
                case ButtonId.Down:
                    g.DrawRectangle(pUn, 0, 0, Width - 1, Height / 2);
                    g.DrawRectangle(pHo, 0, Height / 2, Width - 1, Height / 2 - 1);
                    break;
                case ButtonId.None:
                    g.DrawRectangle(pUn, 0, 0, Width - 1, Height / 2);
                    g.DrawRectangle(pUn, 0, Height / 2, Width - 1, Height / 2 - 1);
                    break;
            }

            if (_parent._hover)
            {
                g.DrawRectangle(pHo, 0, 0, Width - 1, Height - 1);
                //g.DrawLine(pUn, 0, 1, 0, Height - 2);
            }

            var arrowX = ClientRectangle.Width - 12;
            var arrowYu = ClientRectangle.Height / 2 - 4;
            var arrowYd = ClientRectangle.Height / 2 + 4;

            Brush brush = _parent.Enabled ? new SolidBrush(_parent.ArrowsColor) : new SolidBrush(Color.DimGray);

            Point[] arrowUp = { new(arrowX, arrowYu), new(arrowX + 6, arrowYu), new(arrowX + 3, arrowYu - 4) };
            Point[] arrowDown = { new(arrowX + 1, arrowYd), new(arrowX + 6, arrowYd), new(arrowX + 3, arrowYd + 3) };

            g.FillPolygon(brush, arrowUp);
            g.FillPolygon(brush, arrowDown);
        }

        private void OnUpDown(UpDownEventArgs upevent)
        {
            _upDownEventHandler?.Invoke(this, upevent);
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Tick += TimerHandler;
            }

            _timerInterval = 500;
            _timer.Interval = _timerInterval;
            _timer.Start();
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        private void TimerHandler(object source, EventArgs args)
        {
            if (!Capture)
            {
                EndButtonPress();
            }
            else
            {
                OnUpDown(new UpDownEventArgs((int)_pushed));
                if (_timer == null)
                    return;
                _timerInterval *= 7;
                _timerInterval /= 10;
                if (_timerInterval < 1)
                    _timerInterval = 1;
                _timer.Interval = _timerInterval;
            }
        }
    }

    private enum ButtonId
    {
        None,
        Up,
        Down
    }
}