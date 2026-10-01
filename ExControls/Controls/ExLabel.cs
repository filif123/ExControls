using System.Drawing.Text;

namespace ExControls;

/// <summary>
/// Expanded Label Control.
/// </summary>
[ToolboxBitmap(typeof(Label), "Label.bmp")]
public class ExLabel : Label
{
    private const ContentAlignment AnyBottom = ContentAlignment.BottomLeft | ContentAlignment.BottomCenter | ContentAlignment.BottomRight;
    private const ContentAlignment AnyMiddle = ContentAlignment.MiddleLeft | ContentAlignment.MiddleCenter | ContentAlignment.MiddleRight;
    private const ContentAlignment AnyRight = ContentAlignment.TopRight | ContentAlignment.MiddleRight | ContentAlignment.BottomRight;
    private const ContentAlignment AnyCenter = ContentAlignment.TopCenter | ContentAlignment.MiddleCenter | ContentAlignment.BottomCenter;

    private Color _disabledForeColor;

    /// <summary>
    /// 
    /// </summary>
    public ExLabel()
    {
        _disabledForeColor = Color.DimGray;
    }

    /// <summary>
    /// Color of the CheckBox's text and box when the Control is disabled
    /// </summary>
    [Browsable(true)]
    [ExCategory(CategoryType.Appearance)]
    [DefaultValue(typeof(Color), "DimGray")]
    [ExDescription("Color of the CheckBox's text and box when the Control is disabled.")]
    public Color DisabledForeColor
    {
        get => _disabledForeColor;
        set
        {
            if (_disabledForeColor == value)
                return;
            _disabledForeColor = value;
            Invalidate();
            OnDisabledForeColorChanged();
        }
    }

    /// <summary>Occurs when the <see cref="DisabledForeColor" /> property changes.</summary>
    [ExCategory("Changed Property")]
    [ExDescription("Occurs when the DisabledForeColor property changes.")]
    public event EventHandler? DisabledForeColorChanged;

    /// <summary>Raises the <see cref="DisabledForeColorChanged" /> event.</summary>
    protected virtual void OnDisabledForeColorChanged()
    {
        DisabledForeColorChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    /// <remarks>
    /// Povoleny popis kresli <see cref="Label" />. Zakazany text ma namiesto systemovej farby <see cref="DisabledForeColor" />,
    /// inak sa kresli rovnako ako v <see cref="Label" /> (zarovnanie, zalamovanie, okraje, mnemotechnika, vypustka),
    /// aby sedel s velkostou z <see cref="Label.GetPreferredSize" />. Zakazany popis nevyvola udalost <see cref="Control.Paint" />.
    /// </remarks>
    protected override void OnPaint(PaintEventArgs e)
    {
        if (Enabled)
        {
            base.OnPaint(e);
            return;
        }

        var face = DeflateRect(ClientRectangle, Padding);
        if (Image is { } image)
            DrawImage(e.Graphics, image, face, RtlTranslateAlignment(ImageAlign));

        if (UseCompatibleTextRendering)
        {
            using var format = CreateStringFormat();
            using var brush = new SolidBrush(DisabledForeColor);
            e.Graphics.DrawString(Text, Font, brush, face, format);
        }
        else
        {
            TextRenderer.DrawText(e.Graphics, Text, Font, face, DisabledForeColor, CreateTextFormatFlags());
        }
    }

    /// <summary>Format textu GDI+ ako v <see cref="Label" /> (pri <see cref="Label.UseCompatibleTextRendering" />).</summary>
    private StringFormat CreateStringFormat()
    {
        var format = new StringFormat
        {
            Alignment = (TextAlign & AnyRight) != 0 ? StringAlignment.Far : (TextAlign & AnyCenter) != 0 ? StringAlignment.Center : StringAlignment.Near,
            LineAlignment = (TextAlign & AnyBottom) != 0 ? StringAlignment.Far : (TextAlign & AnyMiddle) != 0 ? StringAlignment.Center : StringAlignment.Near
        };
        if (RightToLeft == RightToLeft.Yes)
            format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
        if (AutoEllipsis)
        {
            format.Trimming = StringTrimming.EllipsisCharacter;
            format.FormatFlags |= StringFormatFlags.LineLimit;
        }
        format.HotkeyPrefix = !UseMnemonic ? HotkeyPrefix.None : ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide;
        if (AutoSize)
            format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        return format;
    }

    /// <summary>
    /// Priznaky textu GDI ako v <see cref="Label" />: zarovnanie podla <see cref="Label.TextAlign" /> (aj sprava dolava),
    /// zalamovanie po slovach, <see cref="Label.AutoEllipsis" /> a <see cref="Label.UseMnemonic" />.
    /// </summary>
    private TextFormatFlags CreateTextFormatFlags()
    {
        var flags = AlignmentToFlags(RtlTranslateContent(TextAlign)) | TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
        if (AutoEllipsis)
            flags |= TextFormatFlags.EndEllipsis;
        if (RightToLeft == RightToLeft.Yes)
            flags |= TextFormatFlags.RightToLeft;
        if (!UseMnemonic)
            flags |= TextFormatFlags.NoPrefix;
        else if (!ShowKeyboardCues)
            flags |= TextFormatFlags.HidePrefix;

        // rovnako ako Label: text, ktory sa zmesti na jeden riadok, sa nezalamuje
        var bordersAndPadding = Padding.Size + SizeFromClientSize(Size.Empty);
        if (BorderStyle == BorderStyle.Fixed3D)
            bordersAndPadding += new Size(2, 2);
        var unconstrained = TextRenderer.MeasureText(Text, Font, new Size(int.MaxValue, int.MaxValue), flags & ~TextFormatFlags.WordBreak);
        if (unconstrained.Width <= Width - bordersAndPadding.Width)
            flags &= ~(TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
        return flags;
    }

    private static TextFormatFlags AlignmentToFlags(ContentAlignment alignment)
    {
        var flags = TextFormatFlags.Default;
        if ((alignment & AnyBottom) != 0)
            flags |= TextFormatFlags.Bottom;
        else if ((alignment & AnyMiddle) != 0)
            flags |= TextFormatFlags.VerticalCenter;
        if ((alignment & AnyRight) != 0)
            flags |= TextFormatFlags.Right;
        else if ((alignment & AnyCenter) != 0)
            flags |= TextFormatFlags.HorizontalCenter;
        return flags;
    }

    private static Rectangle DeflateRect(Rectangle rect, Padding padding) =>
        new(rect.X + padding.Left, rect.Y + padding.Top, rect.Width - padding.Horizontal, rect.Height - padding.Vertical);
}