using ExControls.Collections;
using ExControls.Designers;
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ExControls;

/// <summary>
/// </summary>
[Designer(typeof(ExVerticalTabPageDesigner))]
[TypeConverter(typeof(ExVerticalTabPageConverter))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
[DefaultProperty("Text")]
public class ExVerticalTabPage : Panel, ICloneable
{
    private VerticalTabPagesCollection pagesCollection;
    internal ExVerticalTabPage[] children;
    internal ITypeDescriptorContext parentContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="children"></param>
    public ExVerticalTabPage(ExVerticalTabPage[] children)
    {
        Node = new TreeNode();
        base.Dock = DockStyle.Fill;
        this.children = children;
    }

    /// <summary>
    /// </summary>
    public ExVerticalTabPage()
    {
        Node = new TreeNode();
        base.Dock = DockStyle.Fill;
    }

    /// <summary>Initializes a new instance of the <see cref="ExVerticalTabPage" /> class with the specified label text.</summary>
    /// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node.</param>
    public ExVerticalTabPage(string text) : this()
    {
        Node.Text = text;
        base.Text = text;
    }

    /// <summary>Initializes a new instance of the <see cref="ExVerticalTabPage" /> class with the specified label text and child tree nodes.</summary>
    /// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node.</param>
    /// <param name="children">An array of child <see cref="ExVerticalTabPage" /> objects.</param>
    public ExVerticalTabPage(string text, ExVerticalTabPage[] children) : this(text)
    {
        this.children = children;
    }

    /// <summary>Initializes a new instance of the <see cref="ExVerticalTabPage" /> class with the specified label text and images to display when the tree node is in a selected and unselected state.</summary>
    /// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node.</param>
    /// <param name="imageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is unselected.</param>
    /// <param name="selectedImageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is selected.</param>
    public ExVerticalTabPage(string text, int imageIndex, int selectedImageIndex) : this(text)
    {
        Node.ImageIndex = imageIndex;
        Node.SelectedImageIndex = selectedImageIndex;
    }

    /// <summary>Initializes a new instance of the <see cref="ExVerticalTabPage" /> class with the specified label text, child tree nodes, and images to display when the tree node is in a selected and unselected state.</summary>
    /// <param name="text">The label <see cref="P:System.Windows.Forms.TreeNode.Text" /> of the new tree node.</param>
    /// <param name="imageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is unselected.</param>
    /// <param name="selectedImageIndex">The index value of <see cref="T:System.Drawing.Image" /> to display when the tree node is selected.</param>
    /// <param name="children">An array of child <see cref="ExVerticalTabPage" /> objects.</param>
    public ExVerticalTabPage(string text, int imageIndex, int selectedImageIndex, ExVerticalTabPage[] children) : this(text, imageIndex, selectedImageIndex)
    {
        this.children = children;
    }

    /// <summary>
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public TreeNode Node { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public VerticalTabPagesCollection Children => pagesCollection ??= new VerticalTabPagesCollection(this);

    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [Localizable(false)]
    public override AutoSizeMode AutoSizeMode
    {
        get => AutoSizeMode.GrowOnly;
        set
        {
        }
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool AutoSize
    {
        get => base.AutoSize;
        set => base.AutoSize = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler AutoSizeChanged
    {
        add => base.AutoSizeChanged += value;
        remove => base.AutoSizeChanged -= value;
    }

    /// <inheritdoc />
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override AnchorStyles Anchor
    {
        get => base.Anchor;
        set => base.Anchor = value;
    }

    /// <inheritdoc />
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override DockStyle Dock
    {
        get => base.Dock;
        set => base.Dock = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new Size Size
    {
        get => base.Size;
        set => base.Size = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler DockChanged
    {
        add => base.DockChanged += value;
        remove => base.DockChanged -= value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool Enabled
    {
        get => base.Enabled;
        set => base.Enabled = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler EnabledChanged
    {
        add => base.EnabledChanged += value;
        remove => base.EnabledChanged -= value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new Point Location
    {
        get => base.Location;
        set => base.Location = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler LocationChanged
    {
        add => base.LocationChanged += value;
        remove => base.LocationChanged -= value;
    }

    /// <inheritdoc />
    [DefaultValue(typeof (Size), "0, 0")]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Size MaximumSize
    {
        get => base.MaximumSize;
        set => base.MaximumSize = value;
    }

    /// <inheritdoc />
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Size MinimumSize
    {
        get => base.MinimumSize;
        set => base.MinimumSize = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Size PreferredSize => base.PreferredSize;

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new int TabIndex
    {
        get => base.TabIndex;
        set => base.TabIndex = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler TabIndexChanged
    {
        add => base.TabIndexChanged += value;
        remove => base.TabIndexChanged -= value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool TabStop
    {
        get => base.TabStop;
        set => base.TabStop = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler TabStopChanged
    {
        add => base.TabStopChanged += value;
        remove => base.TabStopChanged -= value;
    }

    /// <inheritdoc />
    [Localizable(true)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public new event EventHandler TextChanged
    {
        add => base.TextChanged += value;
        remove => base.TextChanged -= value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool Visible
    {
        get => base.Visible;
        set => base.Visible = value;
    }

    /// <summary>
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new event EventHandler VisibleChanged
    {
        add => base.VisibleChanged += value;
        remove => base.VisibleChanged -= value;
    }

    /// <inheritdoc />
    public object Clone()
    {
        var page = new ExVerticalTabPage();
        page.Node = (TreeNode) Node.Clone();
        if (Children.Count > 0)
        {
            page.children = new ExVerticalTabPage[Children.Count];
            for (var i = 0; i < Children.Count; i++)
            {
                page.Children.Add((pagesCollection[i] as ExVerticalTabPage)!.Clone());
            }
        }
        return page;
    }
}