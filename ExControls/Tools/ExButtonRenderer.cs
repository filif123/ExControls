using ExControls.Controls;

namespace ExControls;

/// <summary>
///     This class contains static methods to measure and draw buttons and text into the control graphics. This class
///     cannot be inherited.
/// </summary>
public static class ExButtonRenderer
{
    /// <summary>
    ///     Draws a Drop-down button at the specific relative position with custom colors.
    /// </summary>
    /// <param name="g">Graphics, where button will be drawn.</param>
    /// <param name="rec">Relative bounds of drop-down button.</param>
    /// <param name="backColor">Background color of the button.</param>
    /// <param name="borderColor">Border color of the button.</param>
    /// <param name="arrowColor">Color of the button's arrow.</param>
    public static void DrawDropDownButton(Graphics g, Rectangle rec, Color backColor, Color borderColor, Color arrowColor)
    {
        using Brush brushBack = new SolidBrush(backColor);
        using Brush brushArrow = new SolidBrush(arrowColor);
        using var penBorder = new Pen(borderColor);

        g.FillRectangle(brushBack, rec);

        var arrowX = rec.X + rec.Width / 2 - 3;
        var arrowY = rec.Y + rec.Height / 2 - 1;
        Point[] arrows = { new(arrowX, arrowY), new(arrowX + 7, arrowY), new(arrowX + 3, arrowY + 4) };
        g.FillPolygon(brushArrow, arrows);

        var dropButton = new Rectangle(rec.X, rec.Y, rec.Width - 1, rec.Height - 1);
        g.DrawRectangle(penBorder, dropButton);
    }

    /// <summary>
    ///     Measures and returns bounds of the Text and Box of the <see cref="ExCheckBox" /> or <see cref="ExRadioButton" />.
    /// </summary>
    /// <param name="g">Graphics ot the control.</param>
    /// <param name="c">Control (<see cref="ExCheckBox" /> or <see cref="ExRadioButton" />).</param>
    /// <param name="boxSize">Size of box.</param>
    /// <param name="boxOffset">Distance between box and text.</param>
    /// <returns></returns>
    public static BoxAndTextRectangle GetBoxAndTextRectangle(IDeviceContext g, ICheckableExControl c, int boxSize, int boxOffset)
    {
        var boxtext = new BoxAndTextRectangle();

        var halfw = (int)Math.Round(c.ClientRectangle.Width / 2d - boxSize / 2d);
        var halfh = (int)Math.Round(c.ClientRectangle.Height / 2d - boxSize / 2d);

        var texts = TextRenderer.MeasureText(g, c.Text, c.Font);
        var halftextw = texts.Width / 2;

        switch (c.CheckAlign)
        {
            case ContentAlignment.TopLeft:
                boxtext.BoxRectangle = c.RightToLeft == RightToLeft.No
                    ? new Rectangle(0, 0, boxSize, boxSize)
                    : new Rectangle(c.ClientRectangle.Width - boxSize, 0, boxSize, boxSize);
                break;
            case ContentAlignment.TopCenter:
                boxtext.BoxRectangle = new Rectangle(halfw, 0, boxSize, boxSize);
                break;
            case ContentAlignment.TopRight:
                boxtext.BoxRectangle = c.RightToLeft == RightToLeft.No
                    ? new Rectangle(c.ClientRectangle.Width - boxSize, 0, boxSize, boxSize)
                    : new Rectangle(0, 0, boxSize, boxSize);
                break;
            case ContentAlignment.MiddleLeft:
                boxtext.BoxRectangle = c.RightToLeft == RightToLeft.No
                    ? new Rectangle(0, halfh, boxSize, boxSize)
                    : new Rectangle(c.ClientRectangle.Width - boxSize, halfh, boxSize, boxSize);
                break;
            case ContentAlignment.MiddleCenter:
                boxtext.BoxRectangle = new Rectangle(halfw, halfh, boxSize, boxSize);
                break;
            case ContentAlignment.MiddleRight:
                boxtext.BoxRectangle = c.RightToLeft == RightToLeft.No
                    ? new Rectangle(c.ClientRectangle.Width - boxSize, halfh, boxSize, boxSize)
                    : new Rectangle(0, halfh, boxSize, boxSize);
                break;
            case ContentAlignment.BottomLeft:
                boxtext.BoxRectangle = c.RightToLeft == RightToLeft.No
                    ? new Rectangle(0, c.ClientRectangle.Height - boxSize, boxSize, boxSize)
                    : new Rectangle(c.ClientRectangle.Width - boxSize, c.ClientRectangle.Height - boxSize, boxSize, boxSize);
                break;
            case ContentAlignment.BottomCenter:
                boxtext.BoxRectangle = new Rectangle(halfw, c.ClientRectangle.Height - boxSize, boxSize, boxSize);
                break;
            case ContentAlignment.BottomRight:
                boxtext.BoxRectangle = c.RightToLeft == RightToLeft.No
                    ? new Rectangle(c.ClientRectangle.Width - boxSize, c.ClientRectangle.Height - boxSize, boxSize, boxSize)
                    : new Rectangle(0, c.ClientRectangle.Height - boxSize, boxSize, boxSize);
                break;
        }

        switch (c.TextAlign)
        {
            case ContentAlignment.TopLeft:
                boxtext.TextRectangle = new Rectangle(0, 0, texts.Width, texts.Height);
                if (c.RightToLeft == RightToLeft.Yes)
                    boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width - texts.Width, 0, texts.Width, texts.Height);
                boxtext.TextRectangle = OffsetText(boxtext.TextRectangle, c, boxSize, boxOffset);
                break;

            case ContentAlignment.TopCenter:
                boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width / 2 - halftextw, 0, texts.Width, texts.Height);
                if (c.CheckAlign == ContentAlignment.TopCenter)
                    boxtext.TextRectangle = ChangeRecValue(boxtext.TextRectangle, 0, boxSize + boxOffset);
                break;

            case ContentAlignment.TopRight:
                boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width - texts.Width, 0, texts.Width, texts.Height);
                if (c.RightToLeft == RightToLeft.Yes)
                    boxtext.TextRectangle = new Rectangle(0, 0, texts.Width, texts.Height);
                boxtext.TextRectangle = OffsetText(boxtext.TextRectangle, c, boxSize, boxOffset, false);
                break;

            case ContentAlignment.MiddleLeft:
                boxtext.TextRectangle = new Rectangle(0, c.ClientRectangle.Height / 2 - texts.Height / 2, texts.Width, texts.Height);
                if (c.RightToLeft == RightToLeft.Yes)
                    boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width - texts.Width, c.ClientRectangle.Height / 2 - texts.Height / 2,
                        texts.Width, texts.Height);
                boxtext.TextRectangle = OffsetText(boxtext.TextRectangle, c, boxSize, boxOffset);
                break;

            case ContentAlignment.MiddleCenter:
                boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width / 2 - texts.Width / 2,
                    c.ClientRectangle.Height / 2 - texts.Height / 2, texts.Width, texts.Height);
                break;

            case ContentAlignment.MiddleRight:
                boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width - texts.Width, c.ClientRectangle.Height / 2 - texts.Height / 2,
                    texts.Width, texts.Height);
                if (c.RightToLeft == RightToLeft.Yes)
                    boxtext.TextRectangle = new Rectangle(0, c.ClientRectangle.Height / 2 - texts.Height / 2, texts.Width, texts.Height);
                boxtext.TextRectangle = OffsetText(boxtext.TextRectangle, c, boxSize, boxOffset, false);
                break;

            case ContentAlignment.BottomLeft:
                boxtext.TextRectangle = new Rectangle(0, c.ClientRectangle.Height - texts.Height, texts.Width, texts.Height);
                if (c.RightToLeft == RightToLeft.Yes)
                    boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width - texts.Width, c.ClientRectangle.Height - texts.Height,
                        texts.Width, texts.Height);
                boxtext.TextRectangle = OffsetText(boxtext.TextRectangle, c, boxSize, boxOffset);
                break;

            case ContentAlignment.BottomCenter:
                boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width / 2 - halftextw, c.ClientRectangle.Height - texts.Height,
                    texts.Width, texts.Height);
                if (c.CheckAlign == ContentAlignment.BottomCenter)
                    boxtext.TextRectangle = ChangeRecValue(boxtext.TextRectangle, 0, -boxSize - boxOffset);
                break;

            case ContentAlignment.BottomRight:
                boxtext.TextRectangle = new Rectangle(c.ClientRectangle.Width - texts.Width, c.ClientRectangle.Height - texts.Height, texts.Width,
                    texts.Height);
                if (c.RightToLeft == RightToLeft.Yes)
                    boxtext.TextRectangle = new Rectangle(0, c.ClientRectangle.Height - texts.Height, texts.Width, texts.Height);
                boxtext.TextRectangle = OffsetText(boxtext.TextRectangle, c, boxSize, boxOffset, false);
                break;
        }

        return boxtext;
    }

    private static Rectangle ChangeRecValue(Rectangle rec, int plusx, int plusy)
    {
        rec.X += plusx;
        rec.Y += plusy;
        return rec;
    }

    private static Rectangle OffsetText(Rectangle rec, ICheckableExControl c, int boxSize, int boxOffset, bool left = true)
    {
        if (left)
        {
            if (c.CheckAlign is ContentAlignment.TopLeft or ContentAlignment.MiddleLeft or ContentAlignment.BottomLeft)
                rec = c.RightToLeft == RightToLeft.No
                    ? ChangeRecValue(rec, boxSize + boxOffset, 0)
                    : ChangeRecValue(rec, -boxSize - boxOffset, 0);
        }
        else
        {
            if (c.CheckAlign is ContentAlignment.TopRight or ContentAlignment.MiddleRight or ContentAlignment.BottomRight)
                rec = c.RightToLeft == RightToLeft.No
                    ? ChangeRecValue(rec, -boxSize - boxOffset, 0)
                    : ChangeRecValue(rec, boxSize + boxOffset, 0);
        }

        return rec;
    }
}

/// <summary>
///     Represents rectangles of the box and text of the <see cref="ICheckableExControl" />.
/// </summary>
public class BoxAndTextRectangle
{
    /// <summary>
    ///     Bounds of box
    /// </summary>
    public Rectangle BoxRectangle { get; set; }

    /// <summary>
    ///     Bounds of text
    /// </summary>
    public Rectangle TextRectangle { get; set; }
}