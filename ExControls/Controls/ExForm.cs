namespace ExControls;

/// <summary>
///     Extended form.
/// </summary>
public class ExForm : Form
{
    private bool _darkTitleBar;
    private Color _titleBarBackColor;
    private Color _titleBarForeColor;
    private Color _titleBarBorderColor;
    private FormCornersType _cornersType;
    private FormStyle _formStyle = FormStyle.Default;

    /// <inheritdoc />
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            if (!DesignMode) 
                cp.ExStyle |= (int)Win32.WindowStylesEx.WS_EX_COMPOSITED;
            return cp;
        }
    }

    /// <inheritdoc />
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        this.SetImmersiveDarkMode(_darkTitleBar);
        if (ExTools.IsWin11Build22000)
        {
            this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
            this.SetFormCorners(_cornersType);
            this.SetFormStyle(_formStyle);
        }
    }

    /// <summary>
    ///     Gets or sets whether form's titlebar uses dark mode. Default is false.
    /// Works only in Windows 10+.
    /// </summary>
    [ExDescription("Gets or sets whether form's titlebar uses dark mode.", true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(false)]
    public bool DarkTitleBar
    {
        get => _darkTitleBar;
        set
        {
            if (_darkTitleBar == value)
                return;
            _darkTitleBar = value;
            if (Handle != IntPtr.Zero) 
                this.SetImmersiveDarkMode(_darkTitleBar);
            if (_titleBarBackColor != Color.Empty && _darkTitleBar)
                _titleBarBackColor = Color.Empty;
        }
    }

    /// <summary>
    ///     Gets or sets background color of the titlebar. Set Color.Empty for default color.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color))]
    public Color TitleBarBackColor
    {
        get => _titleBarBackColor;
        set
        {
            if (_titleBarBackColor == value)
                return;
            _titleBarBackColor = value;
            if (Handle != IntPtr.Zero)
                this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
            if (_titleBarBackColor != Color.Empty && _darkTitleBar) 
                _darkTitleBar = false;
        }
    }

    /// <summary>
    ///     Gets or sets foreground color of the titlebar. Set Color.Empty for default color.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color))]
    public Color TitleBarForeColor
    {
        get => _titleBarForeColor;
        set
        {
            if (_titleBarForeColor == value)
                return;
            _titleBarForeColor = value;
            if (Handle != IntPtr.Zero)
                this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
        }
    }

    /// <summary>
    ///     Gets or sets border color of the titlebar. Set Color.Empty for default color.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color))]
    public Color TitleBarBorderColor
    {
        get => _titleBarBorderColor;
        set
        {
            if (_titleBarBorderColor == value)
                return;
            _titleBarBorderColor = value;
            if (Handle != IntPtr.Zero)
                this.SetTitlebarAndBorderColor(_titleBarBackColor, _titleBarForeColor, _titleBarBorderColor);
        }
    }

    /// <summary>
    ///     Gets or sets type of form corner. Default is FormCornersType.Default.
    /// Works only in Windows 11+.
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(FormCornersType.Default)]
    public FormCornersType CornersType
    {
        get => _cornersType;
        set
        {
            if (_cornersType == value)
                return;
            
            _cornersType = value;
            if (Handle != IntPtr.Zero)
                this.SetFormCorners(_cornersType);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(FormStyle.Default)]
    public FormStyle FormStyle
    {
        get => _formStyle;
        set
        {
            if (_formStyle == value)
                return;
            _formStyle = value;
            if (Handle != IntPtr.Zero)
                this.SetFormStyle(_formStyle);
        }
    }
}