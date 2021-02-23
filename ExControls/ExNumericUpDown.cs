using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded NumericUpDown Control
    /// </summary>
    [ToolboxBitmap(typeof(NumericUpDown), "NumericUpDown.bmp")]
    public partial class ExNumericUpDown : NumericUpDown, IExControl
    {
        private Color _highlightColor;
        private Color _arrowsColor;
        private Color _borderColor;
        private Color _selectedButtonColor;
        
        private bool _defaultStyle;
        private bool _hover;
        private int _defaultButtonsWidth;
        
        private UpDownButtons newButtonUpDown;
        private Control originalButtonUpDown;

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExNumericUpDown()
        {
            InitializeComponent();
            InitInternal();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="container"></param>
        public ExNumericUpDown(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitInternal();
        }
        
        private void InitInternal()
        {
            originalButtonUpDown = Controls[0];
            newButtonUpDown = new UpDownButtons(this);
            Control textBox = Controls[1];
            textBox.MouseEnter += TextBox_MouseEnter;
            textBox.MouseLeave += TextBox_MouseLeave;

            _defaultButtonsWidth = LogicalToDeviceUnits(16);
            newButtonUpDown.TabStop = false;
            newButtonUpDown.Size = new Size(_defaultButtonsWidth, PreferredHeight);
            newButtonUpDown.UpDown += OnUpDown;
            newButtonUpDown.Dock = DockStyle.Right;

            _defaultStyle = true;
            _borderColor = Color.Gainsboro;
            _arrowsColor = Color.Black;
            _highlightColor = SystemColors.Highlight;
            _selectedButtonColor = SystemColors.Highlight;

            Invalidate(true);
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

        /// <inheritdoc />
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Default style of the Control.")]
        public bool DefaultStyle
        {
            get => _defaultStyle;
            set
            {
                _defaultStyle = value;

                if (value)
                {
                    Controls.Remove(newButtonUpDown);
                    Controls.Add(originalButtonUpDown);
                }
                else
                {
                    Controls.Remove(originalButtonUpDown);
                    Controls.Add(newButtonUpDown);
                    BorderStyle = BorderStyle.FixedSingle;
                }

                Invalidate();
            }
        }

        /// <summary>
        ///     Color of the Control's border when mouse is over the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Color of the Control's border when mouse is over the Control.")]
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
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the arrows which is in this Control in the Up and Down buttons.")]
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
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Gainsboro")]
        [Description("Color of the Control's border.")]
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
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Color the Up and Down buttons when they are selected.")]
        public Color SelectedButtonColor
        {
            get => _selectedButtonColor;
            set
            {
                _selectedButtonColor = value; 
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
            {
                return;
            }

            DrawInternal(e.Graphics);
        }

        private void DrawInternal(Graphics g)
        {
            g.Clear(BackColor);
            using var p = new Pen(_hover ? HighlightColor : BorderColor);
            g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);

            _hover = true;
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);

            _hover = false;
            Invalidate();
        }

        /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
        protected virtual void OnDefaultStyleChanged()
        {
            DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
        }

        internal class UpDownButtons : Control
        {
            private ButtonID pushed;
            private ButtonID captured;
            private ButtonID mouseOver;
            private UpDownEventHandler upDownEventHandler;
            private Timer timer;
            private int timerInterval;
            private bool doubleClickFired;
            private readonly ExNumericUpDown parent;

            internal UpDownButtons(ExNumericUpDown parent)
            {
                SetStyle(ControlStyles.Opaque, true);
                SetStyle(ControlStyles.FixedHeight, true);
                SetStyle(ControlStyles.FixedWidth, true);
                SetStyle(ControlStyles.Selectable, false);
                this.parent = parent;
            }

            public event UpDownEventHandler UpDown
            {
                add => upDownEventHandler += value;
                remove => upDownEventHandler -= value;
            }

            private void BeginButtonPress(MouseEventArgs e)
            {
                int num = Size.Height / 2;
                if (e.Y < num)
                {
                    pushed = captured = ButtonID.Up;
                    Invalidate();
                }
                else
                {
                    pushed = captured = ButtonID.Down;
                    Invalidate();
                }
                Capture = true;
                OnUpDown(new UpDownEventArgs((int)pushed));
                StartTimer();
            }

            private void EndButtonPress()
            {
                pushed = ButtonID.None;
                captured = ButtonID.None;
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
                    doubleClickFired = true;
                parent.OnMouseDown(TranslateMouseEvent(this,e));
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                if (Capture)
                {
                    Rectangle clientRectangle = ClientRectangle;
                    clientRectangle.Height /= 2;
                    if (captured == ButtonID.Down)
                        clientRectangle.Y += clientRectangle.Height;
                    if (clientRectangle.Contains(e.X, e.Y))
                    {
                        if (pushed != captured)
                        {
                            StartTimer();
                            pushed = captured;
                            Invalidate();
                        }
                    }
                    else if (pushed != ButtonID.None)
                    {
                        StopTimer();
                        pushed = ButtonID.None;
                        Invalidate();
                    }
                }

                Rectangle clientRectangle1 = ClientRectangle;
                Rectangle clientRectangle2 = ClientRectangle;
                clientRectangle1.Height /= 2;
                clientRectangle2.Y += clientRectangle2.Height / 2;
                if (clientRectangle1.Contains(e.X, e.Y))
                {
                    if (mouseOver != ButtonID.Up)
                    {
                        mouseOver = ButtonID.Up;
                        Invalidate();
                    }
                }
                else if (clientRectangle2.Contains(e.X, e.Y))
                {
                    if (mouseOver != ButtonID.Down)
                    {
                        mouseOver = ButtonID.Down;
                        Invalidate();
                    }
                }
                parent.OnMouseMove(TranslateMouseEvent(this, e));
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                    EndButtonPress();

                Point screen = PointToScreen(new Point(e.X, e.Y));
                MouseEventArgs e1 = TranslateMouseEvent(this, e);

                if (e.Button == MouseButtons.Left)
                {
                    if (NativeMethods.WindowFromPoint(new NativeMethods.POINT(screen.X,screen.Y)) == Handle)
                    {
                        if (!doubleClickFired)
                        {
                            parent.OnClick(e1);
                        }
                        else
                        {
                            doubleClickFired = false;
                            parent.OnDoubleClick(e1);
                            parent.OnMouseDoubleClick(e1);
                        }
                    }
                    doubleClickFired = false;
                }
                parent.OnMouseUp(e1);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                if (mouseOver != ButtonID.None)
                {
                    mouseOver = ButtonID.None;
                    Invalidate();
                    parent.OnMouseLeave(e);
                }
            }

            private MouseEventArgs TranslateMouseEvent(IWin32Window child, MouseEventArgs e)
            {
                if (child == null || !IsHandleCreated)
                    return e;
                var pt = new NativeMethods.POINT(e.X, e.Y);
                NativeMethods.MapWindowPoints(child.Handle, Handle, ref pt, 1);
                return new MouseEventArgs(e.Button, e.Clicks, pt.x, pt.y, e.Delta);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                DrawInternal(e.Graphics);
            }

            private void DrawInternal(Graphics g)
            {
                g.Clear(parent.BackColor);

                if (pushed != ButtonID.None)
                {
                    using var selected = new SolidBrush(parent.SelectedButtonColor);
                    if (pushed == ButtonID.Up)
                        g.FillRectangle(selected, 0, 0, Width, Height / 2);
                    else
                        g.FillRectangle(selected, 0, Height / 2, Width - 1, Height / 2);
                }

                using var sbUnhovered = new SolidBrush(parent.BorderColor);
                using var sbHovered = new SolidBrush(parent.HighlightColor);
                using var pUn = new Pen(sbUnhovered);
                using var pHo = new Pen(sbHovered);

                switch (mouseOver)
                {
                    case ButtonID.Up:
                        g.DrawRectangle(pHo, 0, 0, Width - 1, Height / 2 - 1);
                        g.DrawRectangle(pUn, 0, Height / 2, Width - 1, Height / 2 - 1);
                        break;
                    case ButtonID.Down:
                        g.DrawRectangle(pUn, 0, 0, Width - 1, Height / 2 - 1);
                        g.DrawRectangle(pHo, 0, Height / 2, Width - 1, Height / 2 - 1);
                        break;
                    case ButtonID.None:
                        g.DrawRectangle(pUn, 0, 0, Width - 1, Height / 2 - 1);
                        g.DrawRectangle(pUn, 0, Height / 2, Width - 1, Height / 2 - 1);
                        break;
                }

                if (parent._hover)
                {
                    g.DrawRectangle(pHo, 0, 0, Width - 1, Height - 1);
                    g.DrawLine(pUn,0,1,0,Height - 2);
                }

                int arrowX = ClientRectangle.Width - 12;
                int arrowYu = ClientRectangle.Height / 2 - 4;
                int arrowYd = ClientRectangle.Height / 2 + 4;

                Brush brush = parent.Enabled ? new SolidBrush(parent.ArrowsColor) : new SolidBrush(Color.DimGray);

                Point[] arrowUp = { new(arrowX, arrowYu), new(arrowX + 6, arrowYu), new(arrowX + 3, arrowYu - 4) };
                Point[] arrowDown = { new(arrowX + 1, arrowYd), new(arrowX + 6, arrowYd), new(arrowX + 3, arrowYd + 3) };

                g.FillPolygon(brush, arrowUp);
                g.FillPolygon(brush, arrowDown);
            }

            protected virtual void OnUpDown(UpDownEventArgs upevent)
            {
                upDownEventHandler?.Invoke(this, upevent);
            }

            protected void StartTimer()
            {
                if (timer == null)
                {
                    timer = new Timer();
                    timer.Tick += TimerHandler;
                }
                timerInterval = 500;
                timer.Interval = timerInterval;
                timer.Start();
            }

            protected void StopTimer()
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
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
                    OnUpDown(new UpDownEventArgs((int)pushed));
                    if (timer == null)
                        return;
                    timerInterval *= 7;
                    timerInterval /= 10;
                    if (timerInterval < 1)
                        timerInterval = 1;
                    timer.Interval = timerInterval;
                }
            }
        }

        internal enum ButtonID
        {
            None,
            Up,
            Down,
        }
    }
}
