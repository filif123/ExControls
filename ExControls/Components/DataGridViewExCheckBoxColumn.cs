using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded CheckBox Column for DataGridView
    /// </summary>
    public class DataGridViewExCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DataGridViewExCheckBoxColumn()
        {
            CellTemplate = new DataGridViewExCheckBoxCell();
            DefaultStyle = true;
            BorderColor = Color.Black;
            MarkColor = Color.Black;
            SquareBackColor = Color.White;
            HighlightColor = Color.FromArgb(0, 120, 215);
        }

        /// <inheritdoc />
        public sealed override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                switch (value)
                {
                    case null:
                    case DataGridViewCheckBoxCell:
                        base.CellTemplate = value;
                        break;
                    default:
                        throw new InvalidCastException();
                }
            }
        }

        private DataGridViewExCheckBoxCell DynCheckBoxCellTemplate => (DataGridViewExCheckBoxCell)CellTemplate;

        /// <summary>
        ///     Default style of the Control
        /// </summary>
        [Browsable(true)]
        [ExCategory("Appearance")]
        [DefaultValue(true)]
        [Description("Default style of the Control.")]
        public bool DefaultStyle
        {
            get => DynCheckBoxCellTemplate.DefaultStyle;
            set
            {
                if (DefaultStyle == value)
                    return;
                DynCheckBoxCellTemplate.DefaultStyle = value;
                if (DataGridView == null)
                    return;
                var rows = DataGridView.Rows;
                for (var i = 1; i < DataGridView.Rows.Count; i++)
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                        cell.DefaultStyle = value;
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Color of the CheckBox's border
        /// </summary>
        [Browsable(true)]
        [ExCategory("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the CheckBox's border.")]
        public Color BorderColor
        {
            get => DynCheckBoxCellTemplate.BorderColor;
            set
            {
                if (BorderColor == value)
                    return;
                DynCheckBoxCellTemplate.BorderColor = value;
                if (DataGridView == null)
                    return;
                var rows = DataGridView.Rows;
                for (var i = 1; i < DataGridView.Rows.Count; i++)
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                        cell.BorderColor = value;
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Color of the CheckBox's mark
        /// </summary>
        [Browsable(true)]
        [ExCategory("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Color of the CheckBox's mark.")]
        public Color MarkColor
        {
            get => DynCheckBoxCellTemplate.MarkColor;
            set
            {
                if (MarkColor == value)
                    return;
                DynCheckBoxCellTemplate.MarkColor = value;
                if (DataGridView == null)
                    return;
                var rows = DataGridView.Rows;
                for (var i = 1; i < DataGridView.Rows.Count; i++)
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                        cell.MarkColor = value;
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Background color of CheckBox's square
        /// </summary>
        [Browsable(true)]
        [ExCategory("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("Background color of CheckBox's square.")]
        public Color SquareBackColor
        {
            get => DynCheckBoxCellTemplate.SquareBackColor;
            set
            {
                if (SquareBackColor == value)
                    return;
                DynCheckBoxCellTemplate.SquareBackColor = value;
                if (DataGridView == null)
                    return;
                var rows = DataGridView.Rows;
                for (var i = 1; i < DataGridView.Rows.Count; i++)
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                        cell.SquareBackColor = value;
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Color of the border and mark of CheckBox when mouse is over the Control
        /// </summary>
        [Browsable(true)]
        [ExCategory("Appearance")]
        [DefaultValue(typeof(Color), "0, 120, 215")]
        [Description("Color of the border and mark of CheckBox when mouse is over the Control.")]
        public Color HighlightColor
        {
            get => DynCheckBoxCellTemplate.HighlightColor;
            set
            {
                if (HighlightColor == value)
                    return;
                DynCheckBoxCellTemplate.HighlightColor = value;
                if (DataGridView == null)
                    return;
                var rows = DataGridView.Rows;
                for (var i = 1; i < DataGridView.Rows.Count; i++)
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExCheckBoxCell cell)
                        cell.HighlightColor = value;
                DataGridView.InvalidateColumn(Index);
            }
        }
    }

    /// <summary>
    ///     Expanded CheckBox Cell for DataGridView
    /// </summary>
    public class DataGridViewExCheckBoxCell : DataGridViewCheckBoxCell
    {
        private static bool _hover;
        private Color _borderColor;

        private bool _defaultStyle;
        private Color _highlightColor;
        private Color _markColor;
        private Color _squareBackColor;

        /// <summary>
        ///     Constructor
        /// </summary>
        public DataGridViewExCheckBoxCell()
        {
            DefaultStyle = true;
            BorderColor = Color.Black;
            MarkColor = Color.Black;
            SquareBackColor = Color.White;
            HighlightColor = Color.FromArgb(0, 120, 215);
        }

        /// <summary>
        ///     Default style of the Control
        /// </summary>
        public bool DefaultStyle
        {
            get => _defaultStyle;
            set
            {
                _defaultStyle = value;
                DataGridView?.InvalidateColumn(ColumnIndex);
            }
        }

        /// <summary>
        ///     Color of the CheckBox's border
        /// </summary>
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                DataGridView?.InvalidateColumn(ColumnIndex);
            }
        }

        /// <summary>
        ///     Color of the CheckBox's mark
        /// </summary>
        public Color MarkColor
        {
            get => _markColor;
            set
            {
                _markColor = value;
                DataGridView?.InvalidateColumn(ColumnIndex);
            }
        }

        /// <summary>
        ///     Background color of CheckBox's square
        /// </summary>
        public Color SquareBackColor
        {
            get => _squareBackColor;
            set
            {
                _squareBackColor = value;
                DataGridView?.InvalidateColumn(ColumnIndex);
            }
        }

        /// <summary>
        ///     Color of the border and mark of CheckBox when mouse is over the Control
        /// </summary>
        public Color HighlightColor
        {
            get => _highlightColor;
            set
            {
                _highlightColor = value;
                DataGridView?.InvalidateColumn(ColumnIndex);
            }
        }

        /// <inheritdoc />
        public override object Clone()
        {
            var cell = (DataGridViewExCheckBoxCell)base.Clone();
            cell.DefaultStyle = DefaultStyle;
            cell.BorderColor = BorderColor;
            cell.MarkColor = MarkColor;
            cell.SquareBackColor = SquareBackColor;
            cell.HighlightColor = HighlightColor;
            return cell;
        }

        /// <inheritdoc />
        protected override void Paint(Graphics g,
            Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates state,
            object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            if (DefaultStyle)
            {
                base.Paint(g, clipBounds, cellBounds, rowIndex, state, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                return;
            }

            // Draw the cell background, if specified.
            if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            {
                var selected = (state & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
                using var cellBackground = selected ? new SolidBrush(cellStyle.SelectionBackColor) : new SolidBrush(cellStyle.BackColor);
                g.FillRectangle(cellBackground, cellBounds);
            }

            // Draw the cell borders, if specified.
            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

            var pt = GetCheckBoxPoint(cellBounds);
            var rect = new Rectangle(pt, new Size(16, 16));
            using var background = new SolidBrush(SquareBackColor);
            g.FillRectangle(background, rect);

            var colorMark = _hover ? HighlightColor : state != DataGridViewElementStates.ReadOnly ? MarkColor : Color.DimGray;
            var colorBorder = _hover ? HighlightColor : state != DataGridViewElementStates.ReadOnly ? BorderColor : Color.DimGray;

            var bs = ButtonState.Normal;

            if (formattedValue is bool val) bs = val == false ? ButtonState.Normal : ButtonState.Checked;

            //mark
            if (bs == ButtonState.Checked)
            {
                using var brushMark = new SolidBrush(colorMark);
                using var penMark = new Pen(brushMark, 2);
                g.DrawLine(penMark, pt.X + 3, pt.Y + 8, pt.X + 6, pt.Y + 11);
                g.DrawLine(penMark, pt.X + 6, pt.Y + 11, pt.X + 12, pt.Y + 5);
                g.FillRectangle(brushMark, pt.X + 6, pt.Y + 12, 1, 1);
            }

            //border
            using var border = new SolidBrush(colorBorder);
            using var penborder = new Pen(border);
            var rectb = new Rectangle(pt, new Size(15, 15));
            g.DrawRectangle(penborder, rectb);
        }

        private static Point GetCheckBoxPoint(Rectangle cellBounds)
        {
            var x = cellBounds.Width / 2 - 8;
            var y = cellBounds.Height / 2 - 8;

            if (x < 0) x = 0;
            if (y < 0) y = 0;

            return new Point(cellBounds.X + x, cellBounds.Y + y);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            if (DefaultStyle)
            {
                base.OnMouseMove(e);
                return;
            }

            if (DataGridView == null)
                return;
            var mouseInContentBounds = _hover;
            _hover = GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
            if (mouseInContentBounds != _hover) DataGridView.InvalidateCell(ColumnIndex, e.RowIndex);
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(int rowIndex)
        {
            if (DefaultStyle)
            {
                base.OnMouseLeave(rowIndex);
                return;
            }

            if (DataGridView == null)
                return;

            if (_hover)
            {
                _hover = false;
                DataGridView.InvalidateCell(ColumnIndex, rowIndex);
            }
        }
    }
}