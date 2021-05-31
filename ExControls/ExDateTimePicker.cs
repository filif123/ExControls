using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded DateTimePicker Control. WORK IN PROGRESS
    /// </summary>
    [ToolboxBitmap(typeof(DateTimePicker), "DateTimePicker.bmp")]
    public partial class ExDateTimePicker : DateTimePicker, IExControl
    {
        private Color _backColor;
        private bool _defaultStyle;
        private Color _foreColor;
        private Color _highlightColor;
        private Color _borderColor;
        private Color _disabledBackColor;
        private Color _arrowColor;

        private bool _hover;
        private bool _selected;

        //private Win32.DATETIMEPICKERINFO dtpInfo;
        //private DTPEdit Edit;

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <inheritdoc />
        public ExDateTimePicker()
        {
            _defaultStyle = true;
            _backColor = Color.White;
            _foreColor = Color.Black;
            _borderColor = Color.DimGray;
            _highlightColor = SystemColors.Highlight;
            _arrowColor = Color.Black;

            Invalidate();
        }

        /// <inheritdoc />
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool DefaultStyle
        {
            get => _defaultStyle;
            set
            {
                if (_defaultStyle == value)
                    return;
                _defaultStyle = value;

                //RecreateHandle();
                Invalidate();
            }
        }

        /// <inheritdoc/>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor == value)
                    return;
                _backColor = value;
                base.BackColor = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color),"Black")]
        public override Color ForeColor
        {
            get => _foreColor;
            set
            {
                if (_foreColor == value)
                    return;
                _foreColor = value;
                base.ForeColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Color of the border of ComboBox when mouse is over the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Color of the border of ComboBox when mouse is over the Control.")]
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
        ///     Color of the ComboBox's border
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "DimGray")]
        [Description("Color of the ComboBox's border.")]
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
        ///     Background color of the TextBox's when it is disabled
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Control")]
        [Description("Background color of the TextBox's when it is disabled.")]
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
        ///     Color of the arrow which is in this Control in the Drop down button
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the arrow which is in this Control in the Drop down button.")]
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

        /// <inheritdoc />
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DefaultStyle)
            {
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer,true);
                SetStyle(ControlStyles.AllPaintingInWmPaint,true);
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            /*if (!DefaultStyle)
            {
                dtpInfo.cbSize = Marshal.SizeOf(dtpInfo);
                SendMessage(Handle, DTM_GETDATETIMEPICKERINFO, IntPtr.Zero, ref dtpInfo);
                Edit = new DTPEdit();
                Edit.AssignHandle(dtpInfo.hwndEdit);
            }*/
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            //Edit.DestroyHandle();
        }

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref Win32.DATETIMEPICKERINFO info);
        const int DTM_FIRST = 0x1000;
        const int DTM_GETDATETIMEPICKERINFO = DTM_FIRST + 14;

        /// <summary>Processes Windows messages.</summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DefaultStyle)
                return;

            /*if (m.Msg == Win32.WM_PAINT)
            {
                using Graphics g = Graphics.FromHwndInternal(Handle);
                var clientRect = new Rectangle(0, 0, Width, Height);
                var buttonWidth = dtpInfo.rcButton.Width;
                var dropDownRect = new Rectangle(dtpInfo.rcButton.Left, dtpInfo.rcButton.Top, buttonWidth, clientRect.Height);
                if (RightToLeft == RightToLeft.Yes && RightToLeftLayout)
                {
                    dropDownRect.X = clientRect.Width - dropDownRect.Right;
                    dropDownRect.Width += 1;
                }
                var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);
                var arrow = new Point[]
                {
                    new(middle.X - 3, middle.Y - 2),
                    new(middle.X + 4, middle.Y - 2),
                    new(middle.X, middle.Y + 2)
                };

                Color borderAndButtonColor = Enabled ? BorderColor : Color.LightGray;
                Color arrorColor = BackColor;
                using (var pen = new Pen(borderAndButtonColor))
                    g.DrawRectangle(pen, 0, 0,
                        clientRect.Width - 1, clientRect.Height - 1);
                using (var brush = new SolidBrush(borderAndButtonColor))
                    g.FillRectangle(brush, dropDownRect);
                g.FillPolygon(Brushes.Black, arrow);
            }*/
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (DefaultStyle)
                return;

            e.Graphics.Clear(BackColor);

            string sText;

            if (Format == DateTimePickerFormat.Custom) sText = $"{Value:CustomFormat}";
            else sText = Checked ? base.Text : "";

            TextRenderer.DrawText(e.Graphics, sText, Font, new Point(0, 2), ForeColor);

            if (_hover)
            {
                using var p = new Pen(HighlightColor);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                e.Graphics.DrawLine(p, Width - 20, 0, Width - 20, Height);
            }
            else
            {
                using var p = new Pen(BorderColor);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            }

            int arrowX = ClientRectangle.Width - 14;
            int arrowY = ClientRectangle.Height / 2 - 1;

            using Brush brush = Enabled ? new SolidBrush(ArrowColor) : new SolidBrush(SystemColors.GrayText);
            Point[] arrows = { new(arrowX, arrowY), new(arrowX + 7, arrowY), new(arrowX + 3, arrowY + 4) };
            e.Graphics.FillPolygon(brush, arrows);

            Color border = BorderColor;
            if (_hover || _selected) border = HighlightColor;

            ControlPaint.DrawBorder(e.Graphics, DisplayRectangle, border, ButtonBorderStyle.Solid);
        }

        /// <inheritdoc />
        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            if (DefaultStyle)
                return;

            if (!_hover)
            {
                _hover = true;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            if (DefaultStyle)
                return;

            if (_hover && !_selected)
            {
                _hover = false;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (DefaultStyle)
                return;

            if (!_selected)
            {
                _hover = true;
                _selected = true;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (DefaultStyle)
                return;

            if (_selected)
            {
                _hover = false;
                _selected = false;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (DefaultStyle)
                return;

            if (!_selected)
            {
                _hover = true;
                _selected = true;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (DefaultStyle)
                return;

            if (_selected)
            {
                _hover = false;
                _selected = false;
                Invalidate();
            }
        }

        /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
        protected virtual void OnDefaultStyleChanged()
        {
            DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
        }

        /*private class DTPEdit : NativeWindow
        {
            /// <summary>Invokes the default window procedure associated with this window. </summary>
            /// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that is associated with the current Windows message. </param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == Win32.)
                {
                    
                }
            }
        }*/
    }
}
