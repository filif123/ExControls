using System.Runtime.InteropServices;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace ExControls;

/// <summary>
/// Represents a Windows icon, which is a small bitmap image that is used to represent an object.
/// Icons can be thought of as transparent bitmaps, although their size is determined by the system.
/// </summary>
public sealed class ShellIcon : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShellIcon" /> class and loads the system stock icon of the specified type and size.
    /// </summary>
    /// <param name="type">Type of the <see cref="ShellIcon"/>.</param>
    /// <param name="size">Size of the <see cref="ShellIcon"/>.</param>
    /// <param name="selected">Selected style of the icon.</param>
    /// <param name="linkOverlay">Link overlay of the icon.</param>
    /// <param name="useShellIconSize">Use shell size of the icon instead of the system size metrics.</param>
    public ShellIcon(ShellIconType type, 
        ShellIconSize size = ShellIconSize.Normal, 
        bool selected = false, 
        bool linkOverlay = false,
        bool useShellIconSize = false)
    {
        Type = type;
        Disposed = false;
        Size = size;
        UseShellIconSize = useShellIconSize;
        Selected = selected;
        LinkOverlay = linkOverlay;
        GetStockIcon();
    }

    /// <summary>
    /// Gets the type of <see cref="ShellIcon"/>.
    /// </summary>
    public ShellIconType Type { get; }

    /// <summary>
    /// Gets a Handle of the <see cref="ShellIcon"/>.
    /// </summary>
    public IntPtr Handle { get; private set; }

    /// <summary>
    /// Gets whether this instance was disposed.
    /// </summary>
    public bool Disposed { get; private set; }

    /// <summary>
    /// Gets an index of this icon in system's internal ImageList.
    /// </summary>
    public int Index { get; private set; }

    /// <summary>
    /// Size of the <see cref="ShellIcon"/>.
    /// </summary>
    public ShellIconSize Size { get; }

    /// <summary>
    /// Modifies the <see cref="Size"/> of <see cref="ShellIcon"/> by causing the function to retrieve the
    /// Shell-sized icons rather than the sizes specified by the system metrics.
    /// </summary>
    public bool UseShellIconSize { get; }

    /// <summary>
    /// Modifies the <see cref="ShellIcon"/> by causing the function to blend the icon with the system highlight color.
    /// </summary>
    public bool Selected { get; }

    /// <summary>
    /// Modifies the <see cref="ShellIcon"/> value by causing the function to add the link overlay to the file's icon.
    /// </summary>
    public bool LinkOverlay { get; }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // ReSharper disable once UnusedParameter.Local
    private void Dispose(bool disposing)
    {
        if (Disposed) return;

        Win32.DestroyIcon(Handle);
        Disposed = true;
        Handle = IntPtr.Zero;
    }

    /// <summary>
    /// Converts <see cref="ShellIcon"/> to <see cref="System.Drawing.Icon"/> that uses the same handle (it stays valid only
    /// until the <see cref="ShellIcon"/> is disposed).
    /// </summary>
    /// <param name="icon">the shell icon</param>
    /// <returns>icon with the handle of <paramref name="icon"/></returns>
    public static implicit operator Icon(ShellIcon icon) => Icon.FromHandle(icon.Handle);

    /// <summary>
    /// Converts <see cref="ShellIcon"/> to <see cref="Bitmap"/> image.
    /// </summary>
    /// <returns>a new bitmap with the icon</returns>
    public Bitmap ToBitmap() => Icon.FromHandle(Handle).ToBitmap();

    private static Win32.SHSTOCKICONINFO GetStockIconInfo(Win32.SHGSI flags, ShellIconType type)
    {
        var sii = new Win32.SHSTOCKICONINFO
        {
            cbSize = (uint)Marshal.SizeOf<Win32.SHSTOCKICONINFO>()
        };

        Marshal.ThrowExceptionForHR(Win32.SHGetStockIconInfo(type, flags, ref sii));
        return sii;
    }

    private void GetStockIcon()
    {
        var flags = Win32.SHGSI.ICON | Win32.SHGSI.SYSICONINDEX;
        switch (Size)
        {
            case ShellIconSize.Normal:
                break;
            case ShellIconSize.Large:
                flags |= Win32.SHGSI.LARGEICON;
                break;
            case ShellIconSize.Small:
                flags |= Win32.SHGSI.SMALLICON;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (Selected) 
            flags |= Win32.SHGSI.SELECTED;

        if (UseShellIconSize) 
            flags |= Win32.SHGSI.SHELLICONSIZE;

        if (LinkOverlay) 
            flags |= Win32.SHGSI.LINKOVERLAY;

        var sii = GetStockIconInfo(flags, Type);

        Handle = sii.hIcon;
        Index = sii.iIcon;
    }

    /// <inheritdoc />
    ~ShellIcon()
    {
        if (!Disposed) 
            Dispose(false);
    }
}
