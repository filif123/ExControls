

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace ExControls;

/// <summary>
///     Displays a message window, also known as a dialog box, which presents a message to the user.<br></br>
///     It is a modal window, blocking other actions in the application until the user closes it.<br></br>
///     A MessageBox can contain text, buttons, and symbols that inform and instruct the user.
/// </summary>
public sealed class ExMessageBox
{
    private MessageBoxButtons _buttons;
    private MessageBoxDefaultButton _defaultButton;
    private MessageBoxIcon _icon;
    private MessageBoxOptions _options;

    /// <summary>
    ///     Initializes a new instance of the ExMessageBox class.
    /// </summary>
    public ExMessageBox() : this((IWin32Window)null)
    {
        Form = new ExMessageBoxForm(Style);
        Form.HelpRequested += Form_HelpRequested;
    }

    /// <summary>
    ///     Initializes a new instance of the ExMessageBox class with specified owner.
    /// </summary>
    /// <param name="owner">owner of this MessageBox</param>
    public ExMessageBox(IWin32Window owner)
    {
        Owner = owner;
    }

    /// <summary>
    ///     Initializes a new instance of the ExMessageBox class with text.
    /// </summary>
    /// <param name="text">text in MessageBox's message</param>
    public ExMessageBox([Localizable(true)] string text) : this((IWin32Window)null)
    {
        Text = text;
    }

    /// <summary>
    ///     Initializes a new instance of the ExMessageBox class with text and caption.
    /// </summary>
    /// <param name="text">text in MessageBox's message</param>
    /// <param name="caption">caption of the MessageBox</param>
    public ExMessageBox([Localizable(true)] string text, [Localizable(true)] string caption) : this((IWin32Window)null)
    {
        Text = text;
        Caption = caption;
    }

    /// <summary>
    ///     Initializes a new instance of the ExMessageBox class with specified owner and text.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="text"></param>
    public ExMessageBox(IWin32Window owner, [Localizable(true)] string text) : this(owner)
    {
        Text = text;
    }

    /// <summary>
    ///     Initializes a new instance of the ExMessageBox class with specified owner, text and caption.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="text"></param>
    /// <param name="caption"></param>
    public ExMessageBox(IWin32Window owner, [Localizable(true)] string text, [Localizable(true)] string caption) : this(owner)
    {
        Text = text;
        Caption = caption;
    }

    /// <summary>
    ///     Gets or sets text of OK button in MessageBox.
    /// </summary>
    public static string ButtonOKText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.OK);

    /// <summary>
    ///     Gets or sets text of Cancel button in MessageBox.
    /// </summary>
    public static string ButtonCancelText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.Cancel);

    /// <summary>
    ///     Gets or sets text of Yes button in MessageBox.
    /// </summary>
    public static string ButtonYesText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.Yes);

    /// <summary>
    ///     Gets or sets text of No button in MessageBox.
    /// </summary>
    public static string ButtonNoText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.No);

    /// <summary>
    ///     Gets or sets text of Abort button in MessageBox.
    /// </summary>
    public static string ButtonAbortText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.Abort);

    /// <summary>
    ///     Gets or sets text of Retry button in MessageBox.
    /// </summary>
    public static string ButtonRetryText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.Retry);

    /// <summary>
    ///     Gets or sets text of Ignore button in MessageBox.
    /// </summary>
    public static string ButtonIgnoreText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.Ignore);

    /// <summary>
    ///     Gets or sets text of Help button in MessageBox.
    /// </summary>
    public static string ButtonHelpText { get; set; } = MessageBoxStrings.GetLocalizedString(MessageBoxCmdType.Help);

    /// <summary>
    ///     Gets or sets style of components in MessageBox.
    /// </summary>
    public static ExMessageBoxStyle Style { get; set; }

    /// <summary>
    ///     Gets form of this MessageBox
    /// </summary>
    public Form Form { get; }

    /// <summary>
    ///     Gets owner of this MessageBox.
    /// </summary>
    public IWin32Window Owner { get; }

    /// <summary>
    ///     Gets or sets the text associated with this control.
    /// </summary>
    [Localizable(true)]
    public string Text
    {
        get => ((ExMessageBoxForm)Form).lText.Text;
        set => ((ExMessageBoxForm)Form).lText.Text = value;
    }

    /// <summary>
    ///     Gets or sets the caption of the MessageBox.
    /// </summary>
    [Localizable(true)]
    public string Caption
    {
        get => Form.Text;
        set => Form.Text = value;
    }

    /// <summary>
    ///     Gets or sets buttons visible in MessageBox.
    /// </summary>
    public MessageBoxButtons Buttons
    {
        get => _buttons;
        set
        {
            _buttons = value;
            RemapButtons((ExMessageBoxForm)Form, Buttons, DefaultButton);
        }
    }

    /// <summary>
    ///     Gets or sets the icon showing in the MessageBox.
    /// </summary>
    public MessageBoxIcon Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            ChangeIcon((ExMessageBoxForm)Form, _icon);
        }
    }

    /// <summary>
    ///     Gets or sets default button in MessageBox.
    /// </summary>
    public MessageBoxDefaultButton DefaultButton
    {
        get => _defaultButton;
        set
        {
            _defaultButton = value;
            RemapButtons((ExMessageBoxForm)Form, _buttons, _defaultButton);
        }
    }

    /// <summary>
    ///     Gets or sets options for MessageBox.
    /// </summary>
    public MessageBoxOptions Options
    {
        get => _options;
        set
        {
            _options = value;
            ChangeOptions((ExMessageBoxForm)Form, _options);
        }
    }

    /// <summary>
    ///     Gets or sets visibility of Help buuton in MessageBox.
    /// </summary>
    public bool ShowHelp
    {
        get => ((ExMessageBoxForm)Form).bHelp.Visible;
        set => ((ExMessageBoxForm)Form).bHelp.Visible = value;
    }

    /// <summary>
    ///     Gets or sets an interval in seconds. After exceeding the limit the MessageBox closes itself
    /// </summary>
    public int Countdown
    {
        get => ((ExMessageBoxForm)Form).Countdown;
        set => ((ExMessageBoxForm)Form).Countdown = value;
    }

    /// <summary>
    ///     Occurs when the user requests help for a control.
    /// </summary>
    public event HelpEventHandler HelpRequested;

    /// <summary>
    ///     Displays a message window, also known as a dialog box, which presents a message to the user.
    ///     It is a modal window, blocking other actions in the application until the user closes it.
    /// </summary>
    /// <returns>One of the DialogResult values.</returns>
    /// <exception cref="ArgumentException">when Form is null</exception>
    public DialogResult Show()
    {
        if (Form == null) throw new ArgumentException(@"Form must not be null", nameof(Form));

        var form = (ExMessageBoxForm)Form;
        SetButtonsText(form);
        return form.ShowDialog(Owner);
    }

    private void Form_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
        OnHelpRequested(hlpevent);
    }

    private static void RemapButtons(ExMessageBoxForm form, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
    {
        form.Buttons = buttons;
        form.DefaultButton = defaultButton;

        switch (buttons)
        {
            case MessageBoxButtons.OK:
                form.bOK.Visible = true;
                form.AcceptButton = form.bOK;
                break;
            case MessageBoxButtons.OKCancel:
                form.bOK.Visible = true;
                form.bCancel.Visible = true;
                form.AcceptButton = defaultButton switch
                {
                    MessageBoxDefaultButton.Button1 => form.bOK,
                    MessageBoxDefaultButton.Button2 => form.bCancel,
                    _ => form.AcceptButton
                };
                break;
            case MessageBoxButtons.AbortRetryIgnore:
                form.bAbort.Visible = true;
                form.bRetry.Visible = true;
                form.bIgnore.Visible = true;
                form.AcceptButton = defaultButton switch
                {
                    MessageBoxDefaultButton.Button1 => form.bAbort,
                    MessageBoxDefaultButton.Button2 => form.bRetry,
                    MessageBoxDefaultButton.Button3 => form.bIgnore,
                    _ => form.AcceptButton
                };
                break;
            case MessageBoxButtons.YesNoCancel:
                form.bYes.Visible = true;
                form.bNo.Visible = true;
                form.bCancel.Visible = true;
                form.AcceptButton = defaultButton switch
                {
                    MessageBoxDefaultButton.Button1 => form.bYes,
                    MessageBoxDefaultButton.Button2 => form.bNo,
                    MessageBoxDefaultButton.Button3 => form.bCancel,
                    _ => form.AcceptButton
                };
                break;
            case MessageBoxButtons.YesNo:
                form.bYes.Visible = true;
                form.bNo.Visible = true;
                form.AcceptButton = defaultButton switch
                {
                    MessageBoxDefaultButton.Button1 => form.bYes,
                    MessageBoxDefaultButton.Button2 => form.bNo,
                    _ => form.AcceptButton
                };
                break;
            case MessageBoxButtons.RetryCancel:
                form.bRetry.Visible = true;
                form.bCancel.Visible = true;
                form.AcceptButton = defaultButton switch
                {
                    MessageBoxDefaultButton.Button1 => form.bRetry,
                    MessageBoxDefaultButton.Button2 => form.bCancel,
                    _ => form.AcceptButton
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
        }
    }

    private static void ChangeIcon(ExMessageBoxForm form, MessageBoxIcon icon)
    {
        if (form.picIcon.Image != null) form._shellIcon.Dispose();

        switch (icon)
        {
            case MessageBoxIcon.None:
                form.picIcon.Size = Size.Empty;
                break;
            case MessageBoxIcon.Error:
                form._shellIcon = new ShellIcon(ShellIconType.Error);
                break;
            case MessageBoxIcon.Question:
                form._shellIcon = new ShellIcon(ShellIconType.Help);
                break;
            case MessageBoxIcon.Warning:
                form._shellIcon = new ShellIcon(ShellIconType.Warning);
                break;
            case MessageBoxIcon.Information:
                form._shellIcon = new ShellIcon(ShellIconType.Info);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
        }

        if (icon != MessageBoxIcon.None) form.picIcon.Image = form._shellIcon.ToBitmap();
    }

    private static void ChangeOptions(ExMessageBoxForm form, MessageBoxOptions options)
    {
        if ((options & MessageBoxOptions.RightAlign) == MessageBoxOptions.RightAlign) form.lText.TextAlign = ContentAlignment.MiddleRight;

        if ((options & MessageBoxOptions.RtlReading) == MessageBoxOptions.RtlReading) form.RightToLeft = RightToLeft.Yes;

        if ((options & MessageBoxOptions.DefaultDesktopOnly) == MessageBoxOptions.DefaultDesktopOnly)
            throw new NotSupportedException(nameof(MessageBoxOptions.DefaultDesktopOnly) + "is not supported.");

        if ((options & MessageBoxOptions.ServiceNotification) == MessageBoxOptions.ServiceNotification)
            throw new NotSupportedException(nameof(MessageBoxOptions.ServiceNotification) + "is not supported.");
    }

    private static void SetButtonsText(ExMessageBoxForm form)
    {
        form.bOK.Text = ButtonOKText;
        form.bCancel.Text = ButtonCancelText;
        form.bYes.Text = ButtonYesText;
        form.bNo.Text = ButtonNoText;
        form.bAbort.Text = ButtonAbortText;
        form.bRetry.Text = ButtonRetryText;
        form.bIgnore.Text = ButtonIgnoreText;
        form.bHelp.Text = ButtonHelpText;
    }

    private static DialogResult ShowCore(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        bool showHelp,
        int countdown)
    {
        var form = new ExMessageBoxForm(Style, icon)
        {
            Text = string.IsNullOrEmpty(caption) ? icon == MessageBoxIcon.None ? "Message" : icon.ToString() : caption,
            lText = { Text = text },
            bHelp = { Visible = showHelp },
            Countdown = countdown
        };

        RemapButtons(form, buttons, defaultButton);
        ChangeOptions(form, options);
        ChangeIcon(form, icon);
        SetButtonsText(form);

        var result = form.ShowDialog(owner);
        form.Dispose();
        return result;
    }

    private static DialogResult ShowCore(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        HelpInfo info,
        int countdown)
    {
        var form = new ExMessageBoxForm(Style, icon)
        {
            Text = string.IsNullOrEmpty(caption) ? icon == MessageBoxIcon.None ? "Message" : icon.ToString() : caption,
            lText = { Text = text },
            bHelp = { Visible = true },
            _helpInfo = info,
            Countdown = countdown
        };

        RemapButtons(form, buttons, defaultButton);
        ChangeOptions(form, options);
        ChangeIcon(form, icon);
        SetButtonsText(form);

        var result = form.ShowDialog(owner);
        form.Dispose();
        return result;
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style with Help Button.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        bool displayHelpButton,
        int countdown = -1)
    {
        return ShowCore(null, text, caption, buttons, icon, defaultButton, options, displayHelpButton, countdown);
    }


    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style and Help file Path .
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath);
        return ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }


    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style and Help file Path for a IWin32Window.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath);
        return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }


    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style, Help file Path and keyword.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        string keyword,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath, keyword);
        return ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }


    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style, Help file Path and keyword for a IWin32Window.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        string keyword,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath, keyword);
        return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }


    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style, Help file Path and HelpNavigator.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        HelpNavigator navigator,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath, navigator);
        return ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style, Help file Path and HelpNavigator for IWin32Window.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        HelpNavigator navigator,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath, navigator);
        return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        HelpNavigator navigator,
        object param,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath, navigator, param);
        return ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }


    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, style, Help file Path ,HelpNavigator and object for a
    ///         IWin32Window.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        string helpFilePath,
        HelpNavigator navigator,
        object param,
        int countdown = -1)
    {
        var hpi = new HelpInfo(helpFilePath, navigator, param);
        return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        int countdown = -1)
    {
        return ShowCore(null, text, caption, buttons, icon, defaultButton, options, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        int countdown = -1)
    {
        return ShowCore(null, text, caption, buttons, icon, defaultButton, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        int countdown = -1)
    {
        return ShowCore(null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        int countdown = -1)
    {
        return ShowCore(null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text and caption.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        int countdown = -1)
    {
        return ShowCore(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show([Localizable(true)] string text, int countdown = -1)
    {
        return ShowCore(null, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        MessageBoxOptions options,
        int countdown = -1)
    {
        return ShowCore(owner, text, caption, buttons, icon, defaultButton, options, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        MessageBoxDefaultButton defaultButton,
        int countdown = -1)
    {
        return ShowCore(owner, text, caption, buttons, icon, defaultButton, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        MessageBoxIcon icon,
        int countdown = -1)
    {
        return ShowCore(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text, caption, and style.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        MessageBoxButtons buttons,
        int countdown = -1)
    {
        return ShowCore(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text and caption.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        [Localizable(true)] string caption,
        int countdown = -1)
    {
        return ShowCore(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <devdoc>
    ///     <para>
    ///         Displays a message box with specified text.
    ///     </para>
    /// </devdoc>
    public static DialogResult Show(
        IWin32Window owner,
        [Localizable(true)] string text,
        int countdown = -1)
    {
        return ShowCore(owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0, false, countdown);
    }

    /// <summary>
    ///     Raises the <see cref="HelpRequested" /> event.
    /// </summary>
    /// <param name="hlpevent"></param>
    private void OnHelpRequested(HelpEventArgs hlpevent)
    {
        HelpRequested?.Invoke(this, hlpevent);
    }
}

/// <summary>
///     Specifies style of ExmessageBox
/// </summary>
public class ExMessageBoxStyle
{
    /// <summary>
    ///     Gets or sets the Font of label in MessageBox
    /// </summary>
    public Font LabelFont { get; set; }

    /// <summary>
    ///     Gets or sets the Font of buttons in MessageBox
    /// </summary>
    public Font ButtonsFont { get; set; }

    /// <summary>
    ///     Gets or sets whether default style of components in MessageBox is used
    /// </summary>
    public bool? DefaultStyle { get; set; }

    /// <summary>
    ///     Gets or sets whether MessageBox's caption should be dark or with default color (dark caption works only in Windows
    ///     10).
    /// </summary>
    public bool? UseDarkTitleBar { get; set; }

    /// <summary>
    ///     Gets or sets background color of MessageBox.
    /// </summary>
    public Color? BackColor { get; set; }

    /// <summary>
    ///     Gets or sets foreground color of MessageBox.
    /// </summary>
    public Color? ForeColor { get; set; }

    /// <summary>
    ///     Gets or sets background color of MessageBox's footer.
    /// </summary>
    public Color? FooterBackColor { get; set; }

    /// <summary>
    ///     Gets or sets background color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonBackColor { get; set; }

    /// <summary>
    ///     Gets or sets foreground color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonForeColor { get; set; }

    /// <summary>
    ///     Gets or sets border color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonBorderColor { get; set; }

    /// <summary>
    ///     Gets or sets border size of MessageBox's buttons.
    /// </summary>
    public int? ButtonBorderSize { get; set; }

    /// <summary>
    ///     Gets or sets mouse-down color color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonMouseDownColor { get; set; }

    /// <summary>
    ///     Gets or sets mouse-over color color of MessageBox's buttons.
    /// </summary>
    public Color? ButtonMouseOverColor { get; set; }
}

internal class HelpInfo
{
    public const int HLP_FILE = 1, HLP_KEYWORD = 2, HLP_NAVIGATOR = 3, HLP_OBJECT = 4;

    public HelpInfo(string helpfilepath)
    {
        HelpFilePath = helpfilepath;
        Keyword = "";
        Navigator = HelpNavigator.TableOfContents;
        Param = null;
        Option = HLP_FILE;
    }

    public HelpInfo(string helpfilepath, string keyword)
    {
        HelpFilePath = helpfilepath;
        Keyword = keyword;
        Navigator = HelpNavigator.TableOfContents;
        Param = null;
        Option = HLP_KEYWORD;
    }

    public HelpInfo(string helpfilepath, HelpNavigator navigator)
    {
        HelpFilePath = helpfilepath;
        Keyword = "";
        Navigator = navigator;
        Param = null;
        Option = HLP_NAVIGATOR;
    }


    public HelpInfo(string helpfilepath, HelpNavigator navigator, object param)
    {
        HelpFilePath = helpfilepath;
        Keyword = "";
        Navigator = navigator;
        Param = param;
        Option = HLP_OBJECT;
    }

    public int Option { get; }

    public string HelpFilePath { get; }

    public string Keyword { get; }

    public HelpNavigator Navigator { get; }

    public object Param { get; }

    public override string ToString()
    {
        return "{HelpFilePath=" + HelpFilePath + ", keyword =" + Keyword + ", navigator=" + Navigator + "}";
    }
}