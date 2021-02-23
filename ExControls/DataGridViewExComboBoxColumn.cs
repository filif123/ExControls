using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExControls
{
    /// <summary>
    ///     Expanded ComboBox Column for DataGridView
    /// </summary>
    public class DataGridViewExComboBoxColumn : DataGridViewComboBoxColumn
    {

        /// <summary>
        ///     Constructor
        /// </summary>
        public DataGridViewExComboBoxColumn()
        {
            CellTemplate = new DataGridViewExComboBoxCell();
            DefaultStyle = true;
            DropDownSelectedBackColor = SystemColors.Highlight;

            StyleNormal = new ExComboBoxStyle();
            StyleHighlight = new ExComboBoxStyle();
            StyleSelected = new ExComboBoxStyle();
            StyleDisabled = new ExComboBoxStyle();

            StyleNormal.BackColor = Color.White;
            StyleNormal.ForeColor = Color.Black;
            StyleNormal.BorderColor = Color.DimGray;
            StyleNormal.ArrowColor = Color.Black;
            StyleNormal.ButtonBackColor = StyleNormal.BackColor;
            StyleNormal.ButtonBorderColor = StyleNormal.BackColor; 
            StyleNormal.ButtonRenderFirst = true;

            DataGridView?.InvalidateColumn(Index);
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
                    case DataGridViewComboBoxCell:
                        base.CellTemplate = value;
                        break;
                    default:
                        throw new InvalidCastException();
                }
            }
        }

        private DataGridViewExComboBoxCell ExComboBoxCellTemplate => (DataGridViewExComboBoxCell)CellTemplate;

        /// <summary>
        ///     Default style of the Control
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Default style of the Control.")]
        public bool DefaultStyle
        {
            get => ExComboBoxCellTemplate.DefaultStyle;
            set
            {
                if (DefaultStyle == value)
                    return;
                ExComboBoxCellTemplate.DefaultStyle = value;
                if (DataGridView == null)
                    return;
                DataGridViewRowCollection rows = DataGridView.Rows;
                for (int i = 1; i < DataGridView.Rows.Count; i++)
                {
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExComboBoxCell cell)
                    {
                        cell.DefaultStyle = value;
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Color of the selected row in drop down menu
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "Highlight")]
        [Description("Color of the selected row in drop down menu.")]
        public Color DropDownSelectedBackColor
        {
            get => ExComboBoxCellTemplate.DropDownSelectedBackColor;
            set
            {
                if (DropDownSelectedBackColor == value)
                    return;
                ExComboBoxCellTemplate.DropDownSelectedBackColor = value;
                if (DataGridView == null)
                    return;
                DataGridViewRowCollection rows = DataGridView.Rows;
                for (int i = 1; i < DataGridView.Rows.Count; i++)
                {
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExComboBoxCell cell)
                    {
                        cell.DropDownSelectedBackColor = value;
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Normal style of the Control (when is inactive).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Normal style of the Control (when is inactive).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleNormal
        {
            get => ExComboBoxCellTemplate.StyleNormal;
            set
            {
                ExComboBoxCellTemplate.StyleNormal = value;
                if (DataGridView == null)
                    return;
                DataGridViewRowCollection rows = DataGridView.Rows;
                for (int i = 1; i < DataGridView.Rows.Count; i++)
                {
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExComboBoxCell cell)
                    {
                        cell.StyleNormal = value;
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        /// Highlight style of the Control (when mouse is over control).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Highlight style of the Control (when mouse is over control).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleHighlight
        {
            get => ExComboBoxCellTemplate.StyleHighlight;
            set
            {
                ExComboBoxCellTemplate.StyleHighlight = value;
                if (DataGridView == null)
                    return;
                DataGridViewRowCollection rows = DataGridView.Rows;
                for (int i = 1; i < DataGridView.Rows.Count; i++)
                {
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExComboBoxCell cell)
                    {
                        cell.StyleHighlight = value;
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Selected style of the Control (when control is selected).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Selected style of the Control (when control is selected).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleSelected
        {
            get => ExComboBoxCellTemplate.StyleSelected;
            set
            {
                ExComboBoxCellTemplate.StyleSelected = value;
                if (DataGridView == null)
                    return;
                DataGridViewRowCollection rows = DataGridView.Rows;
                for (int i = 1; i < DataGridView.Rows.Count; i++)
                {
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExComboBoxCell cell)
                    {
                        cell.StyleSelected = value;
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }

        /// <summary>
        ///     Disabled style of the Control (when control is not Enabled).
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Disabled style of the Control (when control is not Enabled).")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExComboBoxStyle StyleDisabled
        {
            get => ExComboBoxCellTemplate.StyleDisabled;
            set
            {
                ExComboBoxCellTemplate.StyleDisabled = value;
                if (DataGridView == null)
                    return;
                DataGridViewRowCollection rows = DataGridView.Rows;
                for (int i = 1; i < DataGridView.Rows.Count; i++)
                {
                    if (rows.SharedRow(i).Cells[Index] is DataGridViewExComboBoxCell cell)
                    {
                        cell.StyleDisabled = value;
                    }
                }
                DataGridView.InvalidateColumn(Index);
            }
        }
    }

    /// <summary>
    ///     Expanded ComboBox Cell for DataGridView
    /// </summary>
    public class DataGridViewExComboBoxCell : DataGridViewComboBoxCell
    {
        private Color _dropDownSelectedBackColor;
        private bool _defaultStyle;

        private bool _drawing;

        /// <summary>
        ///     Constructor
        /// </summary>
        public DataGridViewExComboBoxCell()
        {
            DefaultStyle = true;
            DropDownSelectedBackColor = SystemColors.Highlight;

            StyleNormal = new ExComboBoxStyle();
            StyleHighlight = new ExComboBoxStyle();
            StyleSelected = new ExComboBoxStyle();
            StyleDisabled = new ExComboBoxStyle();

            StyleNormal.PropertyChanged += StyleOnPropertyChanged;
            StyleDisabled.PropertyChanged += StyleOnPropertyChanged;
            StyleHighlight.PropertyChanged += StyleOnPropertyChanged;
            StyleSelected.PropertyChanged += StyleOnPropertyChanged;
        }

        private void StyleOnPropertyChanged(object sender, ExPropertyChangedEventArgs e)
        {
            if (!_drawing) DataGridView?.InvalidateCell(ColumnIndex, RowIndex);
        }

        /// <inheritdoc />
        public override Type EditType => typeof(DataGridViewExComboBoxEditingControl);

        /// <summary>
        ///     Default style of the Control
        /// </summary>
        public bool DefaultStyle
        {
            get => _defaultStyle;
            set
            {
                _defaultStyle = value;
                DataGridView?.InvalidateCell(ColumnIndex, RowIndex);
            }
        }

        /// <summary>
        ///     Color of the selected row in drop down menu
        /// </summary>
        public Color DropDownSelectedBackColor
        {
            get => _dropDownSelectedBackColor;
            set
            {
                _dropDownSelectedBackColor = value;
                DataGridView?.InvalidateCell(ColumnIndex, RowIndex);
            }
        }

        /// <summary>
        ///     Normal style of the Control (when is inactive).
        /// </summary>
        public ExComboBoxStyle StyleNormal { get; set; }

        /// <summary>
        /// Highlight style of the Control (when mouse is over control).
        /// </summary>
        public ExComboBoxStyle StyleHighlight { get; set; }

        /// <summary>
        ///     Selected style of the Control (when control is selected).
        /// </summary>
        public ExComboBoxStyle StyleSelected { get; set; }

        /// <summary>
        ///     Disabled style of the Control (when control is not Enabled).
        /// </summary>
        public ExComboBoxStyle StyleDisabled { get; set; }

        /// <inheritdoc />
        public override object Clone()
        {
            var cell = (DataGridViewExComboBoxCell)base.Clone();
            cell.DefaultStyle = DefaultStyle;
            cell.DropDownSelectedBackColor = DropDownSelectedBackColor;
            cell.StyleNormal = (ExComboBoxStyle)StyleNormal.Clone();
            cell.StyleHighlight = (ExComboBoxStyle)StyleHighlight.Clone();
            cell.StyleSelected = (ExComboBoxStyle)StyleSelected.Clone();
            cell.StyleDisabled = (ExComboBoxStyle)StyleDisabled.Clone();
            return cell;
        }

        /// <summary>Attaches and initializes the hosted editing control.</summary>
        /// <param name="rowIndex">The index of the cell's parent row.</param>
        /// <param name="initialFormattedValue">The initial value to be displayed in the control.</param>
        /// <param name="dataGridViewCellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that determines the appearance of the hosted control.</param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            if (DataGridView.EditingControl is DataGridViewExComboBoxEditingControl ctl)
            {
                //ClearEventInvocations(ctl, "DropDown");
                ctl.DefaultStyle = DefaultStyle;
                ctl.DropDownSelectedRowBackColor = DropDownSelectedBackColor;
                ctl.StyleNormal = StyleNormal;
                ctl.StyleHighlight = StyleHighlight;
                ctl.StyleSelected = StyleSelected;
                ctl.StyleDisabled = StyleDisabled;

                ctl.Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void Paint(Graphics g, 
            Rectangle clipBounds, Rectangle cellBounds, 
            int rowIndex,
            DataGridViewElementStates state, 
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

            _drawing = true;

            Size texts = TextRenderer.MeasureText(g, formattedValue.ToString(), cellStyle.Font);
            Point textStart = new Point(cellBounds.X + 2, cellBounds.Y + (int)Math.Round(cellBounds.Height / 2d - texts.Height / 2d));
            Rectangle dropButton = new Rectangle(cellBounds.X + cellBounds.Width - 20, cellBounds.Y, 20, cellBounds.Height);

            Color back = StyleNormal.BackColor ??= Color.White;
            Color fore = StyleNormal.ForeColor ??= Color.Black;
            Color border = StyleNormal.BorderColor ??= Color.DimGray;
            Color arrow = StyleNormal.ArrowColor ??= Color.Black;
            Color backbut = StyleNormal.ButtonBackColor ??= back;
            Color borbut = StyleNormal.ButtonBorderColor ??= back;
            bool bbfirst = StyleNormal.ButtonRenderFirst ??= true;

            if (!DataGridView.Enabled)
            {
                StyleDisabled.BackColor = back = StyleDisabled.BackColor ?? DataGridView.BackColor;
                StyleDisabled.ForeColor = fore = StyleDisabled.ForeColor ?? SystemColors.GrayText;
                if (StyleDisabled.BorderColor.HasValue) border = StyleDisabled.BorderColor.Value;
                StyleDisabled.ArrowColor = arrow = StyleDisabled.ArrowColor ?? SystemColors.GrayText;
                StyleDisabled.ButtonBackColor = backbut = StyleDisabled.ButtonBackColor ?? back;
                StyleDisabled.ButtonBorderColor = borbut = StyleDisabled.ButtonBorderColor ?? back;
                if (StyleDisabled.ButtonRenderFirst.HasValue) bbfirst = StyleDisabled.ButtonRenderFirst.Value;
            }

            g.FillRectangle(new SolidBrush(back),cellBounds);

            using var penBorder = new Pen(border);

            if (bbfirst)
            {
                ExButtonsRenderer.DrawDropDownButton(g, dropButton, backbut, borbut, arrow);
                g.DrawRectangle(penBorder, cellBounds.X, cellBounds.Y, cellBounds.Width - 1, cellBounds.Height - 1);
            }
            else
            {
                g.DrawRectangle(penBorder, cellBounds.X, cellBounds.Y, cellBounds.Width - 1, cellBounds.Height - 1);
                ExButtonsRenderer.DrawDropDownButton(g, dropButton, backbut, borbut, arrow);
            }

            TextRenderer.DrawText(g, formattedValue.ToString(), cellStyle.Font, textStart, fore, back, DataGridView.RightToLeft == RightToLeft.Yes ? TextFormatFlags.Right : TextFormatFlags.Default);

            _drawing = false;
        }
    }

    /// <summary>Represents the hosted combo box control in a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class DataGridViewExComboBoxEditingControl : ExComboBox, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged;

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewComboBoxEditingControl" /> class.</summary>
        public DataGridViewExComboBoxEditingControl() => TabStop = false;

        /// <summary>Gets or sets the <see cref="T:System.Windows.Forms.DataGridView" /> that contains the combo box control.</summary>
        /// <returns>The <see cref="T:System.Windows.Forms.DataGridView" /> that contains the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> that contains this control; otherwise, <see langword="null" /> if there is no associated <see cref="T:System.Windows.Forms.DataGridView" />.</returns>
        public virtual DataGridView EditingControlDataGridView
        {
            get => dataGridView;
            set => dataGridView = value;
        }

        /// <summary>Gets or sets the formatted representation of the current value of the control.</summary>
        /// <returns>An object representing the current value of this control.</returns>
        public virtual object EditingControlFormattedValue
        {
            get => GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
            set
            {
                if (value is not string strA)
                    return;
                Text = strA;
                if (string.Compare(strA, Text, true, CultureInfo.CurrentCulture) == 0)
                    return;
                SelectedIndex = -1;
            }
        }

        /// <summary>Gets or sets the index of the owning cell's parent row.</summary>
        /// <returns>The index of the row that contains the owning cell; -1 if there is no owning row.</returns>
        public virtual int EditingControlRowIndex { get; set; }

        /// <summary>Gets or sets a value indicating whether the current value of the control has changed.</summary>
        /// <returns>
        /// <see langword="true" /> if the value of the control has changed; otherwise, <see langword="false" />.</returns>
        public virtual bool EditingControlValueChanged
        {
            get => valueChanged;
            set => valueChanged = value;
        }

        /// <summary>Gets the cursor used during editing.</summary>
        /// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor image used by the mouse pointer during editing.</returns>
        public virtual Cursor EditingPanelCursor => Cursors.Default;

        /// <summary>Gets a value indicating whether the cell contents need to be repositioned whenever the value changes.</summary>
        /// <returns>
        /// <see langword="false" /> in all cases.</returns>
        public virtual bool RepositionEditingControlOnValueChange => false;

        /// <summary>Changes the control's user interface (UI) to be consistent with the specified cell style.</summary>
        /// <param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to use as a pattern for the UI.</param>
        public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            Font = dataGridViewCellStyle.Font;

            if (DefaultStyle)
            {
                if (dataGridViewCellStyle.BackColor.A < byte.MaxValue)
                {
                    Color color = Color.FromArgb(byte.MaxValue, dataGridViewCellStyle.BackColor);
                    BackColor = color;
                    dataGridView.EditingPanel.BackColor = color;
                }
                else
                    BackColor = dataGridViewCellStyle.BackColor;
                ForeColor = dataGridViewCellStyle.ForeColor;
            }
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.DropDown" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnDropDown(EventArgs e)
        {
        }

        /// <summary>Determines whether the specified key is a regular input key that the editing control should process or a special key that the <see cref="T:System.Windows.Forms.DataGridView" /> should process.</summary>
        /// <param name="keyData">A bitwise combination of <see cref="T:System.Windows.Forms.Keys" /> values that represents the key that was pressed.</param>
        /// <param name="dataGridViewWantsInputKey">
        /// <see langword="true" /> to indicate that the <see cref="T:System.Windows.Forms.DataGridView" /> control can process the key; otherwise, <see langword="false" />.</param>
        /// <returns>
        /// <see langword="true" /> if the specified key is a regular input key that should be handled by the editing control; otherwise, <see langword="false" />.</returns>
        public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return (keyData & Keys.KeyCode) == Keys.Down || (keyData & Keys.KeyCode) == Keys.Up ||
                   DroppedDown && (keyData & Keys.KeyCode) == Keys.Escape || (keyData & Keys.KeyCode) == Keys.Return || !dataGridViewWantsInputKey;
        }

        /// <summary>Retrieves the formatted value of the cell.</summary>
        /// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values that specifies the data error context.</param>
        /// <returns>An <see cref="T:System.Object" /> that represents the formatted version of the cell contents.</returns>
        public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => Text;

        /// <summary>Prepares the currently selected cell for editing.</summary>
        /// <param name="selectAll">
        /// <see langword="true" /> to select all of the cell's content; otherwise, <see langword="false" />.</param>
        public virtual void PrepareEditingControlForEdit(bool selectAll)
        {
            if (!selectAll)
                return;
            SelectAll();
        }

        private void NotifyDataGridViewOfValueChange()
        {
            valueChanged = true;
            dataGridView.NotifyCurrentCellDirty(true);
        }

        /// <summary>Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectedIndexChanged" /> event.</summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (SelectedIndex == -1)
                return;
            NotifyDataGridViewOfValueChange();
        }
    }
}
