using System.Drawing.Design;

namespace ExControls;

/// <summary>
///     Expanded ComboBox Control for ToolStrip.
/// </summary>
public class ToolStripExComboBox : ToolStripControlHost
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public ToolStripExComboBox() : base(new ExComboBox())
    {
    }

    /// <summary>
    ///     Object ComboBox, type <see cref="ExComboBox" />.
    /// </summary>
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ExComboBox ComboBox => Control as ExComboBox;

    /// <summary>
    ///     Gets or sets the custom string collection to use when the
    ///     <see cref="P:System.Windows.Forms.ToolStripComboBox.AutoCompleteSource" /> property is set to
    ///     <see cref="F:System.Windows.Forms.AutoCompleteSource.CustomSource" />.
    /// </summary>
    /// <returns>An <see cref="T:System.Windows.Forms.AutoCompleteStringCollection" /> that contains the strings.</returns>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Localizable(true)]
    [Editor(
        "System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        typeof(UITypeEditor))]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public AutoCompleteStringCollection AutoCompleteCustomSource
    {
        get => ComboBox.AutoCompleteCustomSource;
        set => ComboBox.AutoCompleteCustomSource = value;
    }

    /// <summary>
    ///     Gets or sets a value that indicates the text completion behavior of the
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" />.
    /// </summary>
    /// <returns>
    ///     One of the <see cref="T:System.Windows.Forms.AutoCompleteMode" /> values. The default is
    ///     <see cref="F:System.Windows.Forms.AutoCompleteMode.None" />.
    /// </returns>
    [DefaultValue(AutoCompleteMode.None)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public AutoCompleteMode AutoCompleteMode
    {
        get => ComboBox.AutoCompleteMode;
        set => ComboBox.AutoCompleteMode = value;
    }

    /// <summary>Gets or sets the source of complete strings used for automatic completion.</summary>
    /// <returns>
    ///     One of the <see cref="T:System.Windows.Forms.AutoCompleteSource" /> values. The default is
    ///     <see cref="F:System.Windows.Forms.AutoCompleteSource.None" />.
    /// </returns>
    [DefaultValue(AutoCompleteSource.None)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public AutoCompleteSource AutoCompleteSource
    {
        get => ComboBox.AutoCompleteSource;
        set => ComboBox.AutoCompleteSource = value;
    }

    /// <summary>This property is not relevant to this class.</summary>
    /// <returns>The background image displayed in the control.</returns>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get => base.BackgroundImage;
        set => base.BackgroundImage = value;
    }

    /// <summary>This property is not relevant to this class.</summary>
    /// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout" />.</returns>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get => base.BackgroundImageLayout;
        set => base.BackgroundImageLayout = value;
    }

    /// <summary>
    ///     Gets or sets the height, in pixels, of the drop-down portion box of a
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" />.
    /// </summary>
    /// <returns>The height, in pixels, of the drop-down box.</returns>
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(106)]
    public int DropDownHeight
    {
        get => ComboBox.DropDownHeight;
        set => ComboBox.DropDownHeight = value;
    }

    /// <summary>Gets or sets a value specifying the style of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
    /// <returns>
    ///     One of the <see cref="T:System.Windows.Forms.ComboBoxStyle" /> values. The default is
    ///     <see cref="F:System.Windows.Forms.ComboBoxStyle.DropDown" />.
    /// </returns>
    [DefaultValue(ComboBoxStyle.DropDown)]
    [RefreshProperties(RefreshProperties.Repaint)]
    public ComboBoxStyle DropDownStyle
    {
        get => ComboBox.DropDownStyle;
        set => ComboBox.DropDownStyle = value;
    }

    /// <summary>
    ///     Gets or sets the width, in pixels, of the drop-down portion of a
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" />.
    /// </summary>
    /// <returns>The width, in pixels, of the drop-down box.</returns>
    public int DropDownWidth
    {
        get => ComboBox.DropDownWidth;
        set => ComboBox.DropDownWidth = value;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> currently
    ///     displays its drop-down portion.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> currently displays its
    ///     drop-down portion; otherwise, <see langword="false" />.
    /// </returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool DroppedDown
    {
        get => ComboBox.DroppedDown;
        set => ComboBox.DroppedDown = value;
    }

    /// <summary>Gets or sets the appearance of the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
    /// <returns>
    ///     One of the values of <see cref="T:System.Windows.Forms.FlatStyle" />. The options are
    ///     <see cref="F:System.Windows.Forms.FlatStyle.Flat" />, <see cref="F:System.Windows.Forms.FlatStyle.Popup" />,
    ///     <see cref="F:System.Windows.Forms.FlatStyle.Standard" />, and
    ///     <see cref="F:System.Windows.Forms.FlatStyle.System" />. The default is
    ///     <see cref="F:System.Windows.Forms.FlatStyle.Popup" />.
    /// </returns>
    [DefaultValue(FlatStyle.Popup)]
    [Localizable(true)]
    public FlatStyle FlatStyle
    {
        get => ComboBox.FlatStyle;
        set => ComboBox.FlatStyle = value;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> should
    ///     resize to avoid showing partial items.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if the list portion can contain only complete items; otherwise, <see langword="false" />.
    ///     The default is <see langword="true" />.
    /// </returns>
    [DefaultValue(true)]
    [Localizable(true)]
    public bool IntegralHeight
    {
        get => ComboBox.IntegralHeight;
        set => ComboBox.IntegralHeight = value;
    }

    /// <summary>Gets a collection of the items contained in this <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
    /// <returns>A collection of items.</returns>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Localizable(true)]
    [Editor(
        "System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        typeof(UITypeEditor))]
    public ComboBox.ObjectCollection Items => ComboBox.Items;

    /// <summary>
    ///     Gets or sets the maximum number of items to be shown in the drop-down portion of the
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" />.
    /// </summary>
    /// <returns>
    ///     The maximum number of items in the drop-down portion. The minimum for this property is 1 and the maximum is
    ///     100.
    /// </returns>
    [DefaultValue(8)]
    [Localizable(true)]
    public int MaxDropDownItems
    {
        get => ComboBox.MaxDropDownItems;
        set => ComboBox.MaxDropDownItems = value;
    }

    /// <summary>Gets or sets the maximum number of characters allowed in the editable portion of a combo box.</summary>
    /// <returns>
    ///     The maximum number of characters the user can enter. Values of less than zero are reset to zero, which is the
    ///     default value.
    /// </returns>
    [DefaultValue(0)]
    [Localizable(true)]
    public int MaxLength
    {
        get => ComboBox.MaxLength;
        set => ComboBox.MaxLength = value;
    }

    /// <summary>Gets or sets the index specifying the currently selected item.</summary>
    /// <returns>
    ///     A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is
    ///     selected.
    /// </returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex
    {
        get => ComboBox.SelectedIndex;
        set => ComboBox.SelectedIndex = value;
    }

    /// <summary>Gets or sets currently selected item in the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</summary>
    /// <returns>
    ///     The object that is the currently selected item or <see langword="null" /> if there is no currently selected
    ///     item.
    /// </returns>
    [Browsable(false)]
    [Bindable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object SelectedItem
    {
        get => ComboBox.SelectedItem;
        set => ComboBox.SelectedItem = value;
    }

    /// <summary>
    ///     Gets or sets the text that is selected in the editable portion of a
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" />.
    /// </summary>
    /// <returns>
    ///     A string that represents the currently selected text in the combo box. If
    ///     <see cref="P:System.Windows.Forms.ToolStripComboBox.DropDownStyle" /> is set to <see langword="DropDownList" />,
    ///     the return value is an empty string ("").
    /// </returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SelectedText
    {
        get => ComboBox.SelectedText;
        set => ComboBox.SelectedText = value;
    }

    /// <summary>
    ///     Gets or sets the number of characters selected in the editable portion of the
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" />.
    /// </summary>
    /// <returns>The number of characters selected in the <see cref="T:System.Windows.Forms.ToolStripComboBox" />.</returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectionLength
    {
        get => ComboBox.SelectionLength;
        set => ComboBox.SelectionLength = value;
    }

    /// <summary>
    ///     Gets or sets the starting index of text selected in the
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" />.
    /// </summary>
    /// <returns>The zero-based index of the first character in the string of the current text selection.</returns>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectionStart
    {
        get => ComboBox.SelectionStart;
        set => ComboBox.SelectionStart = value;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the items in the
    ///     <see cref="T:System.Windows.Forms.ToolStripComboBox" /> are sorted.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if the combo box is sorted; otherwise, <see langword="false" />. The default is
    ///     <see langword="false" />.
    /// </returns>
    [DefaultValue(false)]
    public bool Sorted
    {
        get => ComboBox.Sorted;
        set => ComboBox.Sorted = value;
    }

    /// <summary>This event is not relevant to this class.</summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler DoubleClick
    {
        add => base.DoubleClick += value;
        remove => base.DoubleClick -= value;
    }

    /// <summary>Occurs when the drop-down portion of a <see cref="T:System.Windows.Forms.ToolStripComboBox" /> is shown.</summary>
    public event EventHandler DropDown
    {
        add => ComboBox.DropDown += value;
        remove => ComboBox.DropDown -= value;
    }

    /// <summary>Occurs when the drop-down portion of the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> has closed.</summary>
    public event EventHandler DropDownClosed
    {
        add => ComboBox.DropDownClosed += value;
        remove => ComboBox.DropDownClosed -= value;
    }

    /// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripComboBox.DropDownStyle" /> property has changed.</summary>
    public event EventHandler DropDownStyleChanged
    {
        add => ComboBox.DropDownStyleChanged += value;
        remove => ComboBox.DropDownStyleChanged -= value;
    }

    /// <summary>Occurs when the <see cref="T:System.Windows.Forms.ToolStripComboBox" /> text has changed.</summary>
    public event EventHandler TextUpdate
    {
        add => ComboBox.TextUpdate += value;
        remove => ComboBox.TextUpdate -= value;
    }

    /// <summary>
    ///     Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripComboBox.SelectedIndex" /> property
    ///     has changed.
    /// </summary>
    public event EventHandler SelectedIndexChanged
    {
        add => ComboBox.SelectedIndexChanged += value;
        remove => ComboBox.SelectedIndexChanged -= value;
    }
}