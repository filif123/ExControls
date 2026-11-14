using System.Runtime.CompilerServices;

namespace ExControls;

/// <summary>
/// Draws the 1 px border of a standard control (ListBox, TreeView, CheckedListBox...) with <see cref="BorderStyle.FixedSingle" />
/// in its own color: the system draws the non-client area (border and scroll bars) first and the frame is painted over it,
/// like the border of <see cref="ExTextBox" /> in the custom style. The window is subclassed again whenever the control
/// recreates its handle.
/// </summary>
internal sealed class NonClientBorder : NativeWindow
{
    private const int RgnDiff = 4;
    private const int PrfNonClient = 0x0002;

    private static readonly ConditionalWeakTable<Control, NonClientBorder> Borders = new();

    private readonly Control _control;
    private Color _color;

    private NonClientBorder(Control control, Color color)
    {
        _control = control;
        _color = color;
        _control.HandleCreated += Control_HandleCreated;
        _control.HandleDestroyed += Control_HandleDestroyed;
        if (_control.IsHandleCreated)
        {
            AssignHandle(_control.Handle);
            RedrawFrame();
        }
    }

    /// <summary>
    /// Sets the color of the border, <see langword="null" /> gives the control its system border back.
    /// </summary>
    public static void Set(Control control, Color? color)
    {
        if (Borders.TryGetValue(control, out var border))
        {
            if (color is { } value)
            {
                if (border._color == value)
                    return;
                border._color = value;
                border.RedrawFrame();
                return;
            }

            Borders.Remove(control);
            border.Detach();
            return;
        }

        if (color is { } newColor)
            Borders.Add(control, new NonClientBorder(control, newColor));
    }

    private void Detach()
    {
        _control.HandleCreated -= Control_HandleCreated;
        _control.HandleDestroyed -= Control_HandleDestroyed;
        if (Handle == IntPtr.Zero)
            return;
        ReleaseHandle();
        RedrawFrame();
    }

    private void Control_HandleCreated(object? sender, EventArgs e)
    {
        if (Handle == IntPtr.Zero)
            AssignHandle(_control.Handle);
    }

    private void Control_HandleDestroyed(object? sender, EventArgs e)
    {
        if (Handle != IntPtr.Zero)
            ReleaseHandle();
    }

    // WM_NCPAINT pre novu farbu ramu (alebo systemovy ram po odpojeni)
    private void RedrawFrame()
    {
        if (!_control.IsHandleCreated)
            return;
        Win32.SetWindowPos(_control.Handle, IntPtr.Zero, 0, 0, 0, 0,
            Win32.SetWindowPosFlags.FrameChanged | Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize
            | Win32.SetWindowPosFlags.IgnoreZOrder | Win32.SetWindowPosFlags.DoNotActivate);
    }

    /// <inheritdoc />
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        switch (m.Msg)
        {
            case (int)Win32.WM.NCPAINT:
            {
                var hdc = Win32.GetWindowDC(m.HWnd);
                if (hdc == IntPtr.Zero)
                    return;
                Paint(hdc);
                _ = Win32.ReleaseDC(m.HWnd, hdc);
                break;
            }
            // DrawToBitmap (WM_PRINT) kresli neklientsku oblast bez WM_NCPAINT
            case (int)Win32.WM.PRINT when ((long)m.LParam & PrfNonClient) != 0 && m.WParam != IntPtr.Zero:
                Paint(m.WParam);
                break;
        }
    }

    private void Paint(IntPtr hdc)
    {
        var width = _control.Width;
        var height = _control.Height;
        var outer = Win32.CreateRectRgn(0, 0, width, height);
        var inner = Win32.CreateRectRgn(1, 1, width - 1, height - 1);
        var brush = Win32.CreateSolidBrush(Win32.RGBtoInt(_color));
        try
        {
            _ = Win32.CombineRgn(outer, outer, inner, RgnDiff);
            _ = Win32.FillRgn(hdc, outer, brush);
        }
        finally
        {
            _ = Win32.DeleteObject(brush);
            _ = Win32.DeleteObject(inner);
            _ = Win32.DeleteObject(outer);
        }
    }
}
