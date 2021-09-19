using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExControls.Controls;

namespace ExControls
{
    /// <summary>
    ///     Expanded MaskedTextBox Control
    /// </summary>
    [ToolboxBitmap(typeof(MaskedTextBox), "MaskedTextBox.bmp")]
    public class ExMaskedTextBox : MaskedTextBox, IExControl
    {
        private const int RGN_DIFF = 0x4;

        private Color _borderColor;
        private int _borderThickness;

        private bool _defaultStyle;
        private Color _disabledBorderColor;
        private Color _highlightColor;
        private bool _hover;
        private bool _selected;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExMaskedTextBox()
        {
            _defaultStyle = true;
            BorderStyle = BorderStyle.Fixed3D;
            _borderColor = Color.DimGray;
            _highlightColor = SystemColors.Highlight;
            _borderThickness = 1;
            _disabledBorderColor = SystemColors.InactiveBorder;

            Invalidate();
        }

        /// <summary>
        ///     Color of the TextBox's border
        /// </summary>
        [Browsable(true)]
        [ExCategory(CategoryType.Appearance)]
        [DefaultValue(typeof(Color), "DimGray")]
        [Description("Color of the TextBox's border.")]
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
        ///     Color of the TextBox's border when it is disabled
        /// </summary>
        [Browsable(true)]
        [ExCategory(CategoryType.Appearance)]
        [DefaultValue(typeof(SystemColors), "InactiveBorder")]
        [Description("Color of the TextBox's border when it is disabled.")]
        public Color DisabledBorderColor
        {
            get => _disabledBorderColor;
            set
            {
                if (_disabledBorderColor == value)
                    return;
                _disabledBorderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Color of the border of TextBox when mouse is over the Control
        /// </summary>
        [Browsable(true)]
        [ExCategory(CategoryType.Appearance)]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Color of the border of TextBox when mouse is over the Control.")]
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
        ///     Width of the TextBox's border
        /// </summary>
        [Browsable(true)]
        [ExCategory(CategoryType.Appearance)]
        [DefaultValue(1)]
        [Description("Width of the TabControl's border.")]
        public int BorderThickness
        {
            get => _borderThickness;
            set
            {
                if (_borderThickness == value)
                    return;
                _borderThickness = value;
                Invalidate();
            }
        }

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
        [ExCategory("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <inheritdoc />
        [Browsable(true)]
        [ExCategory(CategoryType.Appearance)]
        [DefaultValue(true)]
        [Description("Default style of the Control.")]
        public bool DefaultStyle
        {
            get => _defaultStyle;
            set
            {
                if (_defaultStyle == value)
                    return;
                _defaultStyle = value;
                BorderStyle = value ? BorderStyle.Fixed3D : BorderStyle.FixedSingle;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!DefaultStyle)
            {
                SetStyle(ControlStyles.Opaque, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
            }
        }

        /// <inheritdoc />
        protected override void WndProc(ref Message m)
        {
            if (!DefaultStyle && m.Msg == (int)Win32.WM.PAINT)
            {
                base.WndProc(ref m);
                PaintInternal(ref m);
                return;
            }

            if (!DefaultStyle && m.Msg == (int)Win32.WM.NCPAINT)
                return;

            base.WndProc(ref m);
        }

        private void PaintInternal(ref Message m)
        {
            //border
            var hdc = Win32.GetWindowDC(Handle);
            var rgn = Win32.CreateRectRgn(0, 0, Width, Height);
            var border = _hover || _selected ? HighlightColor : BorderColor;
            if (!Enabled) border = DisabledBorderColor;
            var brush = Win32.CreateSolidBrush((uint)BGRtoInt(border.R, border.G, border.B));

            Win32.CombineRgn(rgn, rgn, Win32.CreateRectRgn(BorderThickness, BorderThickness, Width - BorderThickness, Height - BorderThickness),
                RGN_DIFF);

            Win32.FillRgn(hdc, rgn, brush);

            Win32.ReleaseDC(Handle, hdc);
            Win32.DeleteObject(rgn);
            Win32.DeleteObject(brush);

            m.Result = IntPtr.Zero;
        }

        private static int BGRtoInt(int r, int g, int b)
        {
            return (r << 0) | (g << 8) | (b << 16);
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
    }
}