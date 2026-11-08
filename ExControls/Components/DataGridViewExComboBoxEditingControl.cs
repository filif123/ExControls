using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
// ReSharper disable UnusedMember.Global

namespace ExControls;

/// <summary>Represents the hosted combo box control in a <see cref="System.Windows.Forms.DataGridViewComboBoxCell" />.</summary>
[ComVisible(true)]
#if NETFRAMEWORK
[ClassInterface(ClassInterfaceType.AutoDispatch)]
#endif
[ToolboxItem(false)]
public class DataGridViewExComboBoxEditingControl : ExComboBox, IDataGridViewEditingControl
{
    private DataGridView? _dataGridView;
    private bool _valueChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="System.Windows.Forms.DataGridViewComboBoxEditingControl" />
    /// class.
    /// </summary>
    public DataGridViewExComboBoxEditingControl()
    {
        TabStop = false;
    }

    /// <summary>Gets or sets the <see cref="System.Windows.Forms.DataGridView" /> that contains the combo box control.</summary>
    /// <returns>
    /// The <see cref="System.Windows.Forms.DataGridView" /> that contains the
    /// <see cref="System.Windows.Forms.DataGridViewComboBoxCell" /> that contains this control; otherwise,
    /// <see langword="null" /> if there is no associated <see cref="System.Windows.Forms.DataGridView" />.
    /// </returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual DataGridView? EditingControlDataGridView
    {
        get => _dataGridView;
        set => _dataGridView = value;
    }

    /// <summary>Gets or sets the formatted representation of the current value of the control.</summary>
    /// <returns>An object representing the current value of this control.</returns>
    // IDataGridViewEditingControl.EditingControlFormattedValue je v BCL asymetricka ([AllowNull] set, get nie null)
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [AllowNull]
    public virtual object EditingControlFormattedValue
    {
        get => GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
        set
        {
            if (value is not string strA)
                return;
            Text = strA;
            if (string.Equals(strA, Text, StringComparison.OrdinalIgnoreCase))
                return;
            SelectedIndex = -1;
        }
    }

    /// <summary>Gets or sets the index of the owning cell's parent row.</summary>
    /// <returns>The index of the row that contains the owning cell; -1 if there is no owning row.</returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int EditingControlRowIndex { get; set; }

    /// <summary>Gets or sets a value indicating whether the current value of the control has changed.</summary>
    /// <returns>
    /// <see langword="true" /> if the value of the control has changed; otherwise, <see langword="false" />.
    /// </returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool EditingControlValueChanged
    {
        get => _valueChanged;
        set => _valueChanged = value;
    }

    /// <summary>Gets the cursor used during editing.</summary>
    /// <returns>
    /// A <see cref="System.Windows.Forms.Cursor" /> that represents the cursor image used by the mouse pointer
    /// during editing.
    /// </returns>
    public virtual Cursor EditingPanelCursor => Cursors.Default;

    /// <summary>Gets a value indicating whether the cell contents need to be repositioned whenever the value changes.</summary>
    /// <returns>
    /// <see langword="false" /> in all cases.
    /// </returns>
    public virtual bool RepositionEditingControlOnValueChange => false;

    /// <summary>Changes the control's user interface (UI) to be consistent with the specified cell style.</summary>
    /// <param name="dataGridViewCellStyle">
    /// The <see cref="System.Windows.Forms.DataGridViewCellStyle" /> to use as a pattern
    /// for the UI.
    /// </param>
    public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
        Font = dataGridViewCellStyle.Font;

        if (!DefaultStyle) return;

        if (dataGridViewCellStyle.BackColor.A < byte.MaxValue)
        {
            var color = Color.FromArgb(byte.MaxValue, dataGridViewCellStyle.BackColor);
            BackColor = color;
            _dataGridView!.EditingPanel.BackColor = color;
        }
        else
        {
            BackColor = dataGridViewCellStyle.BackColor;
        }

        ForeColor = dataGridViewCellStyle.ForeColor;
    }

    /// <summary>
    /// Determines whether the specified key is a regular input key that the editing control should process or a
    /// special key that the <see cref="System.Windows.Forms.DataGridView" /> should process.
    /// </summary>
    /// <param name="keyData">
    /// A bitwise combination of <see cref="System.Windows.Forms.Keys" /> values that represents the
    /// key that was pressed.
    /// </param>
    /// <param name="dataGridViewWantsInputKey">
    /// <see langword="true" /> to indicate that the <see cref="System.Windows.Forms.DataGridView" /> control can process
    /// the key; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if the specified key is a regular input key that should be handled by the editing control;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
    {
        return (keyData & Keys.KeyCode) == Keys.Down || (keyData & Keys.KeyCode) == Keys.Up ||
               DroppedDown && (keyData & Keys.KeyCode) == Keys.Escape || (keyData & Keys.KeyCode) == Keys.Return || !dataGridViewWantsInputKey;
    }

    /// <summary>Retrieves the formatted value of the cell.</summary>
    /// <param name="context">
    /// A bitwise combination of <see cref="System.Windows.Forms.DataGridViewDataErrorContexts" />
    /// values that specifies the data error context.
    /// </param>
    /// <returns>An <see cref="System.Object" /> that represents the formatted version of the cell contents.</returns>
    public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    {
        return Text;
    }

    /// <summary>Prepares the currently selected cell for editing.</summary>
    /// <param name="selectAll">
    /// <see langword="true" /> to select all of the cell's content; otherwise, <see langword="false" />.
    /// </param>
    public virtual void PrepareEditingControlForEdit(bool selectAll)
    {
        if (!selectAll)
            return;
        SelectAll();
    }

    /// <summary>Raises the <see cref="System.Windows.Forms.ComboBox.DropDown" /> event.</summary>
    /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data. </param>
    protected override void OnDropDown(EventArgs e)
    {
    }

    private void NotifyDataGridViewOfValueChange()
    {
        _valueChanged = true;
        _dataGridView!.NotifyCurrentCellDirty(true);
    }

    /// <summary>Raises the <see cref="System.Windows.Forms.ComboBox.SelectedIndexChanged" /> event.</summary>
    /// <param name="e">An <see cref="System.EventArgs" /> that contains the event data.</param>
    protected override void OnSelectedIndexChanged(EventArgs e)
    {
        base.OnSelectedIndexChanged(e);
        if (SelectedIndex == -1)
            return;
        NotifyDataGridViewOfValueChange();
    }
}
