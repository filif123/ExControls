namespace ExControls;

//TODO: WIP --------------

/// <summary>
/// Defines style of control. This class is abstract.
/// </summary>
public abstract class ExStyle
{
    private Color _backColor;
    private Color _foreColor;
    private Color _borderColor;

    /// <summary>
    /// Gets or sets background color of the control.
    /// </summary>
    public Color BackColor
    {
        get => _backColor == Color.Empty && Parent is not null ? Parent._backColor : _backColor;
        set => _backColor = value;
    }

    /// <summary>
    /// Gets or sets foreground color of the control.
    /// </summary>
    public Color ForeColor
    {
        get => _foreColor == Color.Empty && Parent is not null ? Parent._foreColor : _foreColor;
        set => _foreColor = value;
    }

    /// <summary>
    /// Gets or sets border color of the control.
    /// </summary>
    public Color BorderColor
    {
        get => _borderColor == Color.Empty && Parent is not null ? Parent._borderColor : _borderColor;
        set => _borderColor = value;
    }

    /// <summary>
    /// Gets a parent style.
    /// </summary>
    public ExStyle Parent { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public ExStyle()
    {
        _backColor = Color.Empty;
        _foreColor = Color.Empty;
        _borderColor = Color.Empty;
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="original"></param>
    protected ExStyle(ExStyle original)
    {
        Parent = original.Parent;
        BackColor = original._backColor;
        ForeColor = original._foreColor;
        BorderColor = original._borderColor;
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TS"></typeparam>
public class ExStyleManager<TS> where TS : ExStyle, new() 
{
    /// <summary>
    /// 
    /// </summary>
    public TS StyleNormal { get; }

    /// <summary>
    /// 
    /// </summary>
    public TS StyleDisabled { get; }

    /// <summary>
    /// 
    /// </summary>
    public TS StyleSelected { get; }

    /// <summary>
    /// 
    /// </summary>
    public TS StyleHover { get; }

    /// <summary>
    /// 
    /// </summary>
    public TS StyleReadOnly { get; }

    /// <summary>
    /// 
    /// </summary>
    public ExStyleManager()
    {
        StyleNormal = new TS { Parent = null };
        StyleDisabled = new TS { Parent = StyleNormal };
        StyleSelected = new TS { Parent = StyleNormal };
        StyleHover = new TS { Parent = StyleNormal };
        StyleReadOnly = new TS { Parent = StyleNormal };
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IStylable<T> where T : ExStyle, new()
{
    /// <summary>
    /// 
    /// </summary>
    public ExStyleManager<T> StyleManager { get; }
}

/// <summary>
/// 
/// </summary>
public interface ISupportsDefaultStyle
{
    /// <summary>
    /// 
    /// </summary>
    public bool DefaultStyle { get; set; }
}