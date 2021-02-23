﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded CheckListBox Control
    /// </summary>
    [ToolboxBitmap(typeof(CheckedListBox), "CheckedListBox.bmp")]
    public partial class ExCheckedListBox : CheckedListBox, IExControl
    {
        private Color _borderColor;
        private Color _markColor;
        private Color _squareBackColor;
        private Color _highlightColor;
        private Color _disabledForeColor;
        private Color _focusedBackColor;

        private bool _defaultStyle;
        private int _hoverIndex = -1;

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changes.</summary>
        [Category("Changed Property")]
        [Description("Occurs when the BorderColor property changes.")]
        public event EventHandler DefaultStyleChanged;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExCheckedListBox()
        {
            InitializeComponent();
            InitInternal();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="container"></param>
        public ExCheckedListBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitInternal();
        }

        private void InitInternal()
        {
            DefaultStyle = true;
            BorderColor = Color.Black;
            MarkColor = Color.Black;
            SquareBackColor = Color.White;
            HighlightColor = SystemColors.Highlight;
            DisabledForeColor = Color.DimGray;
            FocusedBackColor = Color.Gray;
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
                DrawMode = value ? DrawMode.Normal : DrawMode.OwnerDrawFixed;
                Invalidate();
            }
        }

        /// <summary>
        ///     Color of the CheckBox's border
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the CheckBox's border.")]
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
        ///     Color of the CheckBox's mark
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the CheckBox's mark.")]
        public Color MarkColor
        {
            get => _markColor;
            set
            {
                _markColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Background color of CheckBox's square
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Background color of CheckBox's square.")]
        public Color SquareBackColor
        {
            get => _squareBackColor;
            set
            {
                _squareBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Color of the border and mark of CheckBox when mouse is over the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Color of the border and mark of CheckBox when mouse is over the Control.")]
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
        ///     Foreground color of text if Control is disabled
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "DimGray")]
        [Description("Foreground color of text if Control is disabled.")]
        public Color DisabledForeColor
        {
            get => _disabledForeColor;
            set
            {
                _disabledForeColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Background color of focused row in ListBox
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Gray")]
        [Description("Background color of focused row in ListBox.")]
        public Color FocusedBackColor
        {
            get => _focusedBackColor;
            set
            {
                _focusedBackColor = value;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!DefaultStyle)
            {
                SetStyle(ControlStyles.ResizeRedraw, true);
            }
            else
            {
                SetStyle(ControlStyles.ResizeRedraw, true);
            }
        }

        /// <inheritdoc />
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnDrawItem(e);
                return;
            }

            Rectangle r = e.Bounds;
            r.Height += 1;

            bool focus = (e.State & DrawItemState.Focus) == DrawItemState.Focus;
            using var bc = new SolidBrush(focus ? FocusedBackColor : BackColor);
            e.Graphics.FillRectangle(bc, r);

            TextRenderer.DrawText(e.Graphics, Items[e.Index].ToString(), Font, new Point(r.X + 19, r.Y), Enabled ? ForeColor : DisabledForeColor);

            var pt = new Point(r.X + 1, r.Y + 1);
            var rect = new Rectangle(pt, new Size(16, 16));

            using var background = new SolidBrush(SquareBackColor);
            e.Graphics.FillRectangle(background, rect);

            Color colorMark = _hoverIndex == e.Index ? HighlightColor : Enabled ? MarkColor : DisabledForeColor;
            Color colorBorder = _hoverIndex == e.Index ? HighlightColor : Enabled ? BorderColor : DisabledForeColor;

            //mark
            if (GetItemCheckState(e.Index) == CheckState.Checked)
            {
                using var brushMark = new SolidBrush(colorMark);
                using var penMark = new Pen(brushMark, 2);
                e.Graphics.DrawLine(penMark, r.X + 3 + 1, r.Y + 8 + 1, r.X + 6 + 1, r.Y + 11 + 1);
                e.Graphics.DrawLine(penMark, r.X + 6 + 1, r.Y + 11 + 1, r.X + 12 + 1, r.Y + 5 + 1);
                e.Graphics.FillRectangle(brushMark, r.X + 6 + 1, r.Y + 13, 1, 1);
            }

            //border
            using var border = new SolidBrush(colorBorder);
            using var penborder = new Pen(border);
            var rectb = new Rectangle(pt, new Size(15, 15));
            e.Graphics.DrawRectangle(penborder, rectb);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnMouseMove(e);
                return;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (GetItemRectangle(i).IntersectsWith(new Rectangle(e.Location, new Size(1, 1))))
                {
                    if (_hoverIndex != i)
                    {
                        _hoverIndex = i;
                        Invalidate();
                    }
                    return;
                }
            }

            if (_hoverIndex != -1)
            {
                _hoverIndex = -1;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnMouseLeave(e);
                return;
            }

            if (_hoverIndex != -1)
            {
                _hoverIndex = -1;
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