

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

// ReSharper disable MemberCanBePrivate.Global
namespace ExControls;

/// <summary>
/// 
/// </summary>
public partial class ExMessageBoxForm : Form
{
    private const int WS_EX_LEFT = 0x00000000;
    private const int WS_EX_LEFTSCROLLBAR = 0x00004000;
    private const int WS_EX_LAYOUTRTL = 0x00400000;
    private const int WS_EX_NOINHERITLAYOUT = 0x00100000;

    private int _countdown;
    private int _remains;

    internal HelpInfo HelpInfo;
    internal ShellIcon ShellIcon;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="style"></param>
    /// <param name="icon"></param>
    public ExMessageBoxForm(ExMessageBoxStyle style, MessageBoxIcon icon = MessageBoxIcon.None)
    {
        InitializeComponent();
        Style = style;
        MessageIcon = icon;

        if (Style == null)
            return;

        if (Style.BackColor.HasValue)
            tableLayoutPanel.BackColor = Style.BackColor.Value;
        if (Style.ForeColor.HasValue)
            lText.ForeColor = Style.ForeColor.Value;
        if (Style.FooterBackColor.HasValue)
            flowLPFooter.BackColor = Style.FooterBackColor.Value;
        if (Style.ButtonsFont != null)
            base.Font = Style.ButtonsFont;
        if (Style.LabelFont != null)
            lText.Font = Style.LabelFont;

        if (Style.DefaultStyle.HasValue && !Style.DefaultStyle.Value)
            foreach (Control control in flowLPFooter.Controls)
                if (control is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    if (Style.ButtonBackColor.HasValue)
                        btn.BackColor = Style.ButtonBackColor.Value;
                    if (Style.ButtonForeColor.HasValue)
                        btn.ForeColor = Style.ButtonForeColor.Value;
                    if (Style.ButtonBorderColor.HasValue)
                        btn.FlatAppearance.BorderColor = Style.ButtonBorderColor.Value;
                    if (Style.ButtonBorderSize.HasValue)
                        btn.FlatAppearance.BorderSize = Style.ButtonBorderSize.Value;
                    if (Style.ButtonMouseDownColor.HasValue)
                        btn.FlatAppearance.MouseDownBackColor = Style.ButtonMouseDownColor.Value;
                    if (Style.ButtonMouseOverColor.HasValue)
                        btn.FlatAppearance.MouseOverBackColor = Style.ButtonMouseOverColor.Value;
                }

        if (Style.UseDarkTitleBar.HasValue && Style.UseDarkTitleBar.Value) this.SetImmersiveDarkMode(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public ExMessageBoxStyle Style { get; }

    /// <summary>
    /// 
    /// </summary>
    public MessageBoxIcon MessageIcon { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MessageBoxButtons Buttons { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MessageBoxDefaultButton DefaultButton { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Countdown
    {
        get => _countdown;
        set
        {
            _countdown = value;
            if (value <= 0)
            {
                timerCountDown.Enabled = false;
                _remains = -1;
            }
            else
            {
                timerCountDown.Enabled = true;
                timerCountDown.Interval = 1000;
                _remains = _countdown;
            }
        }
    }

    /// <summary>Gets the required creation parameters when the control handle is created.</summary>
    /// <returns>
    ///     A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the
    ///     handle to the control is created.
    /// </returns>
    protected override CreateParams CreateParams
    {
        get
        {
            var par = base.CreateParams;
            if (RightToLeft == RightToLeft.Yes) par.ExStyle |= WS_EX_LEFT | WS_EX_LEFTSCROLLBAR | WS_EX_LAYOUTRTL | WS_EX_NOINHERITLAYOUT;
            return par;
        }
    }

    private void bYes_Click(object sender, EventArgs e) => DialogResult = DialogResult.Yes;

    private void bNo_Click(object sender, EventArgs e) => DialogResult = DialogResult.No;

    private void bOK_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

    private void bCancel_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

    private void bAbort_Click(object sender, EventArgs e) => DialogResult = DialogResult.Abort;

    private void bRetry_Click(object sender, EventArgs e) => DialogResult = DialogResult.Retry;

    private void bIgnore_Click(object sender, EventArgs e) => DialogResult = DialogResult.Ignore;

    private void bHelp_Click(object sender, EventArgs e)
    {
        timerCountDown.Enabled = false;
        ChangeTimerState();
        var args = new HelpEventArgs(bHelp.Bounds.Location);
        OnHelpRequested(args);
    }

    private void ExMessageBoxForm_Shown(object sender, EventArgs e)
    {
        ChangeTimerState();

        switch (MessageIcon)
        {
            case MessageBoxIcon.None:
                break;
            case MessageBoxIcon.Hand:
                ExTools.Beep(BeepType.OK);
                break;
            case MessageBoxIcon.Question:
                ExTools.Beep(BeepType.Question);
                break;
            case MessageBoxIcon.Exclamation:
                ExTools.Beep(BeepType.Exclamation);
                break;
            case MessageBoxIcon.Asterisk:
                ExTools.Beep(BeepType.Asterisk);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HelpRequested" /> event.</summary>
    /// <param name="hevent">A <see cref="T:System.Windows.Forms.HelpEventArgs" /> that contains the event data. </param>
    protected override void OnHelpRequested(HelpEventArgs hevent)
    {
        base.OnHelpRequested(hevent);

        if (hevent.Handled || HelpInfo == null)
            return;

        switch (HelpInfo.Option)
        {
            case HelpInfo.HLP_FILE:
                Help.ShowHelp(this, HelpInfo.HelpFilePath);
                break;
            case HelpInfo.HLP_KEYWORD:
                Help.ShowHelp(this, HelpInfo.HelpFilePath, HelpInfo.Keyword);
                break;
            case HelpInfo.HLP_NAVIGATOR:
                Help.ShowHelp(this, HelpInfo.HelpFilePath, HelpInfo.Navigator);
                break;
            case HelpInfo.HLP_OBJECT:
                Help.ShowHelp(this, HelpInfo.HelpFilePath, HelpInfo.Navigator, HelpInfo.Param);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(HelpInfo));
        }
    }

    private void ChangeTimerState()
    {
        switch (Buttons)
        {
            case MessageBoxButtons.OK:
                bOK.Text = _remains == -1 ? ExMessageBox.ButtonOKText : $@"{ExMessageBox.ButtonOKText} ({_remains})";
                break;
            case MessageBoxButtons.OKCancel:
                switch (DefaultButton)
                {
                    case MessageBoxDefaultButton.Button1:
                        bOK.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonOKText : $@"{ExMessageBox.ButtonOKText} ({_remains})";
                        break;
                    case MessageBoxDefaultButton.Button2:
                        bCancel.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonCancelText : $@"{ExMessageBox.ButtonCancelText} ({_remains})";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case MessageBoxButtons.AbortRetryIgnore:
                switch (DefaultButton)
                {
                    case MessageBoxDefaultButton.Button1:
                        bAbort.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonAbortText : $@"{ExMessageBox.ButtonAbortText} ({_remains})";
                        break;
                    case MessageBoxDefaultButton.Button2:
                        bRetry.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonRetryText : $@"{ExMessageBox.ButtonRetryText} ({_remains})";
                        break;
                    case MessageBoxDefaultButton.Button3:
                        bIgnore.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonIgnoreText : $@"{ExMessageBox.ButtonIgnoreText} ({_remains})";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case MessageBoxButtons.YesNoCancel:
                switch (DefaultButton)
                {
                    case MessageBoxDefaultButton.Button1:
                        bYes.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonYesText : $@"{ExMessageBox.ButtonYesText} ({_remains})";
                        break;
                    case MessageBoxDefaultButton.Button2:
                        bNo.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonNoText : $@"{ExMessageBox.ButtonNoText} ({_remains})";
                        break;
                    case MessageBoxDefaultButton.Button3:
                        bCancel.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonCancelText : $@"{ExMessageBox.ButtonCancelText} ({_remains})";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case MessageBoxButtons.YesNo:
                switch (DefaultButton)
                {
                    case MessageBoxDefaultButton.Button1:
                        bYes.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonYesText : $@"{ExMessageBox.ButtonYesText} ({_remains})";
                        break;
                    case MessageBoxDefaultButton.Button2:
                        bNo.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonNoText : $@"{ExMessageBox.ButtonNoText} ({_remains})";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case MessageBoxButtons.RetryCancel:
                switch (DefaultButton)
                {
                    case MessageBoxDefaultButton.Button1:
                        bRetry.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonRetryText : $@"{ExMessageBox.ButtonRetryText} ({_remains})";
                        break;
                    case MessageBoxDefaultButton.Button2:
                        bCancel.Text = !timerCountDown.Enabled ? ExMessageBox.ButtonCancelText : $@"{ExMessageBox.ButtonCancelText} ({_remains})";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void timerCountDown_Tick(object sender, EventArgs e)
    {
        _remains--;
        ChangeTimerState();
        if (_remains <= 0) Close();
    }

    private void ExMessageBoxForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        timerCountDown.Enabled = false;
    }

    private void ExMessageBoxForm_Deactivate(object sender, EventArgs e)
    {
        timerCountDown.Enabled = false;
        ChangeTimerState();
    }
}