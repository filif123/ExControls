using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExControls.Controls;

namespace ExControls
{
    /// <summary>
    ///     Expanded TableLayoutPanel Control
    /// </summary>
    [ToolboxBitmap(typeof(TableLayoutPanel), "TableLayoutPanel.bmp")]
    public class ExTableLayoutPanel : TableLayoutPanel, IExControl
    {
        private Color _borderColor;
        private int _borderThickness;
        private bool _defaultStyle;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExTableLayoutPanel()
        {
            _borderColor = Color.Empty;
            _borderThickness = 1;
            _defaultStyle = true;
        }

        /// <summary>
        ///     Color of the TableLayoutPanel's border
        /// </summary>
        [Browsable(true)]
        [ExCategory(CategoryType.Appearance)]
        [DefaultValue(typeof(Color), "Empty")]
        [Description("Color of the TableLayoutPanel's border.")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                if (value == _borderColor)
                    return;

                _borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        ///     Thickness of the TableLayoutPanel's border
        /// </summary>
        [Browsable(true)]
        [ExCategory(CategoryType.Appearance)]
        [DefaultValue(1)]
        [Description("Thickness of the TableLayoutPanel's border.")]
        public int BorderThickness
        {
            get => _borderThickness;
            set
            {
                if (value == _borderThickness)
                    return;

                _borderThickness = value;
                Invalidate();
            }
        }

        /// <summary>Occurs when the <see cref="IExControl.DefaultStyle" /> property changed.</summary>
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
                Invalidate();
            }
        }

        /// <summary>Receives a call when the cell should be refreshed.</summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TableLayoutCellPaintEventArgs" /> that provides data for the event.</param>
        protected override void OnCellPaint(TableLayoutCellPaintEventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnCellPaint(e);
                return;
            }

            var r = e.CellBounds;
            using var pen = new Pen(BorderColor, BorderThickness);
            // top and left lines
            e.Graphics.DrawLine(pen, r.X, r.Y, r.X + r.Width, r.Y);
            e.Graphics.DrawLine(pen, r.X, r.Y, r.X, r.Y + r.Height);
            // last row? move hor.lines 1 up!
            var cy = e.Row == RowCount - 1 ? -1 : 0;
            if (cy != 0)
                e.Graphics.DrawLine(pen, r.X, r.Y + r.Height + cy,
                    r.X + r.Width, r.Y + r.Height + cy);
            // last column ? move vert. lines 1 left!
            var cx = e.Column == ColumnCount - 1 ? -1 : 0;
            if (cx != 0)
                e.Graphics.DrawLine(pen, r.X + r.Width + cx, r.Y,
                    r.X + r.Width + cx, r.Y + r.Height);
        }

        /// <summary>Raises the <see cref="IExControl.DefaultStyleChanged" /> event.</summary>
        protected virtual void OnDefaultStyleChanged()
        {
            DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}