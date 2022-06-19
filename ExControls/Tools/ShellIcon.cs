using System.Runtime.InteropServices;
using static ExControls.Win32.SHGSI;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace ExControls;

/// <summary>
///     Represents a Windows icon, which is a small bitmap image that is used to represent an object.
///     Icons can be thought of as transparent bitmaps, although their size is determined by the system.
/// </summary>
public sealed class ShellIcon : IDisposable
{
    /// <summary>
    ///     Creates new instance of ShellIcon class with specified type and size.
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
    ///     Gets the type of <see cref="ShellIcon"/>.
    /// </summary>
    public ShellIconType Type { get; }

    /// <summary>
    ///     Gets a Handle of the <see cref="ShellIcon"/>.
    /// </summary>
    public IntPtr Handle { get; private set; }

    /// <summary>
    ///     Gets whether this instance was disposed.
    /// </summary>
    public bool Disposed { get; private set; }

    /// <summary>
    ///     Gets an index of this icon in system's internal ImageList.
    /// </summary>
    public int Index { get; private set; }

    /// <summary>
    ///     Size of the <see cref="ShellIcon"/>.
    /// </summary>
    public ShellIconSize Size { get; }

    /// <summary>
    ///     Modifies the <see cref="Size"/> of <see cref="ShellIcon"/> by causing the function to retrieve the
    ///     Shell-sized icons rather than the sizes specified by the system metrics.
    /// </summary>
    public bool UseShellIconSize { get; }

    /// <summary>
    ///     Modifies the <see cref="ShellIcon"/> by causing the function to blend the icon with the system highlight color.
    /// </summary>
    public bool Selected { get; }

    /// <summary>
    ///     Modifies the <see cref="ShellIcon"/> value by causing the function to add the link overlay to the file's icon.
    /// </summary>
    public bool LinkOverlay { get; }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // ReSharper disable once UnusedParameter.Local
#pragma warning disable IDE0060 // Remove unused parameter
    private void Dispose(bool disposing)
    {
        if (Disposed) return;

        Win32.DestroyIcon(Handle);
        Disposed = true;
        Handle = IntPtr.Zero;
    }
#pragma warning restore IDE0060 // Remove unused parameter

    /// <summary>
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static implicit operator Icon(ShellIcon icon) => Icon.FromHandle(icon.Handle);

    /// <summary>
    ///     Converts <see cref="ShellIcon"/> to <see cref="Bitmap"/> image.
    /// </summary>
    /// <returns></returns>
    public Bitmap ToBitmap() => Icon.FromHandle(Handle).ToBitmap();

    private static Win32.SHSTOCKICONINFO GetStockIconInfo(Win32.SHGSI flags, ShellIconType type)
    {
        var sii = new Win32.SHSTOCKICONINFO
        {
            cbSize = (uint)Marshal.SizeOf(typeof(Win32.SHSTOCKICONINFO))
        };

        Marshal.ThrowExceptionForHR(Win32.SHGetStockIconInfo(type, flags, ref sii));
        return sii;
    }

    private void GetStockIcon()
    {
        var flags = SHGSI_ICON | SHGSI_SYSICONINDEX;
        switch (Size)
        {
            case ShellIconSize.Normal:
                break;
            case ShellIconSize.Large:
                flags |= SHGSI_LARGEICON;
                break;
            case ShellIconSize.Small:
                flags |= SHGSI_SMALLICON;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (Selected) 
            flags |= SHGSI_SELECTED;

        if (UseShellIconSize) 
            flags |= SHGSI_SHELLICONSIZE;

        if (LinkOverlay) 
            flags |= SHGSI_LINKOVERLAY;

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

/// <summary>
///     Represents the size of <see cref="ShellIcon"/>.
/// </summary>
public enum ShellIconSize
{
    /// <summary>
    ///     Normal size of icon.
    /// </summary>
    Normal = 0x000000100,

    /// <summary>
    ///     Large size of icon.
    /// </summary>
    Large = 0x000000000,

    /// <summary>
    ///     Small size of icon.
    /// </summary>
    Small = 0x000000001
}

/// <summary>
///     Represents the type of <see cref="ShellIcon"/>.
/// </summary>
public enum ShellIconType : uint
{
    /// <summary>
    ///     Document of a type with no associated application.
    /// </summary>
    DocumentNoAssociacion = 0,

    /// <summary>
    ///     Document of a type with an associated application.
    /// </summary>
    DocumentAssociacion = 1,

    /// <summary>
    ///     Generic application with no custom icon.
    /// </summary>
    Application = 2,

    /// <summary>
    ///     Folder (generic, unspecified state).
    /// </summary>
    Folder = 3,

    /// <summary>
    ///     Folder (open).
    /// </summary>
    OpenedFolder = 4,

    /// <summary>
    ///     5.25-inch disk drive.
    /// </summary>
    Drive525 = 5,

    /// <summary>
    ///     3.5-inch disk drive.
    /// </summary>
    Drive35 = 6,

    /// <summary>
    ///     Removable drive.
    /// </summary>
    RemovableDrive = 7,

    /// <summary>
    ///     Fixed drive (hard disk).
    /// </summary>
    FixedDrive = 8,

    /// <summary>
    ///     Network drive (connected).
    /// </summary>
    NetworkDrive = 9,

    /// <summary>
    ///     Network drive (disconnected).
    /// </summary>
    DisabledNetworkDriver = 10,

    /// <summary>
    ///     CD drive.
    /// </summary>
    CDDrive = 11,

    /// <summary>
    ///     RAM disk drive.
    /// </summary>
    RAMDrive = 12,

    /// <summary>
    ///     The entire network.
    /// </summary>
    World = 13,

    /// <summary>
    ///     A computer on the network.
    /// </summary>
    Server = 15,

    /// <summary>
    ///     A local printer or print destination.
    /// </summary>
    Printer = 16,

    /// <summary>
    ///     The Network virtual folder (FOLDERID_NetworkFolder/CSIDL_NETWORK).
    /// </summary>
    MyNetwork = 17,

    /// <summary>
    ///     The Search feature.
    /// </summary>
    Find = 22,

    /// <summary>
    ///     The Help and Support feature.
    /// </summary>
    Help = 23,

    /// <summary>
    ///     Overlay for a shared item.
    /// </summary>
    Share = 28,

    /// <summary>
    ///     Overlay for a shortcut.
    /// </summary>
    Link = 29,

    /// <summary>
    ///     Overlay for items that are expected to be slow to access.
    /// </summary>
    SlowFile = 30,

    /// <summary>
    ///     The Recycle Bin (empty).
    /// </summary>
    RecycleBin = 31,

    /// <summary>
    ///     The Recycle Bin (not empty).
    /// </summary>
    RecycleBinFull = 32,

    /// <summary>
    ///     Audio CD media.
    /// </summary>
    AudioCD = 40,

    /// <summary>
    ///     Security lock.
    /// </summary>
    Lock = 47,

    /// <summary>
    ///     A virtual folder that contains the results of a search.
    /// </summary>
    AutoList = 49,

    /// <summary>
    ///     A network printer.
    /// </summary>
    NetworkPrinter = 50,

    /// <summary>
    ///     A server shared on a network.
    /// </summary>
    ServerShare = 51,

    /// <summary>
    ///     A local fax printer.
    /// </summary>
    FaxPrinter = 52,

    /// <summary>
    ///     A network fax printer.
    /// </summary>
    FaxPrinterNetwork = 53,

    /// <summary>
    ///     A file that receives the output of a <b>Print to file</b> operation.
    /// </summary>
    FilePrinter = 54,

    /// <summary>
    ///     A ExCategory that results from a <b>Stack by</b> command to organize the contents of a folder.
    /// </summary>
    Stack = 55,

    /// <summary>
    ///     Super Video CD (SVCD) media.
    /// </summary>
    SVCDMedia = 56,

    /// <summary>
    ///     A folder that contains only subfolders as child items.
    /// </summary>
    StuffedFolder = 57,

    /// <summary>
    ///     Unknown drive type.
    /// </summary>
    UnknownDrive = 58,

    /// <summary>
    ///     DVD drive.
    /// </summary>
    DVDDrive = 59,

    /// <summary>
    ///     DVD media.
    /// </summary>
    DVDMedia = 60,

    /// <summary>
    ///     DVD-RAM media.
    /// </summary>
    DVDRAMMedia = 61,

    /// <summary>
    ///     DVD-RW media.
    /// </summary>
    DVDRWMedia = 62,

    /// <summary>
    ///     DVD-R media.
    /// </summary>
    DVDRMedia = 63,

    /// <summary>
    ///     DVD-ROM media.
    /// </summary>
    DVDROMMedia = 64,

    /// <summary>
    ///     CD+ (enhanced audio CD) media.
    /// </summary>
    CDAudioPlusMedia = 65,

    /// <summary>
    ///     CD-RW media.
    /// </summary>
    CDRWMedia = 66,

    /// <summary>
    ///     CD-R media.
    /// </summary>
    CDRMedia = 67,

    /// <summary>
    ///     A writeable CD in the process of being burned.
    /// </summary>
    CDMediaBurn = 68,

    /// <summary>
    ///     Blank writable CD media.
    /// </summary>
    CDMediaBlank = 69,

    /// <summary>
    ///     CD-ROM media.
    /// </summary>
    CDROMMedia = 70,

    /// <summary>
    ///     An audio file.
    /// </summary>
    AudioFiles = 71,

    /// <summary>
    ///     An image file.
    /// </summary>
    ImageFiles = 72,

    /// <summary>
    ///     A video file.
    /// </summary>
    VideoFiles = 73,

    /// <summary>
    ///     A mixed file.
    /// </summary>
    MixedFiles = 74,

    /// <summary>
    ///     Folder back.
    /// </summary>
    FolderBack = 75,

    /// <summary>
    ///     Folder front.
    /// </summary>
    FolderFront = 76,

    /// <summary>
    ///     Security shield. Use for UAC prompts only.
    /// </summary>
    Shield = 77,

    /// <summary>
    ///     Warning.
    /// </summary>
    Warning = 78,

    /// <summary>
    ///     Informational.
    /// </summary>
    Info = 79,

    /// <summary>
    ///     Error.
    /// </summary>
    Error = 80,

    /// <summary>
    ///     Key.
    /// </summary>
    Key = 81,

    /// <summary>
    ///     Software.
    /// </summary>
    Software = 82,

    /// <summary>
    ///     A UI item, such as a button, that issues a rename command.
    /// </summary>
    Rename = 83,

    /// <summary>
    ///     A UI item, such as a button, that issues a delete command.
    /// </summary>
    Delete = 84,

    /// <summary>
    ///     Audio DVD media.
    /// </summary>
    AudioDVDMedia = 85,

    /// <summary>
    ///     Movie DVD media.
    /// </summary>
    MovieDVDMedia = 86,

    /// <summary>
    ///     Enhanced CD media.
    /// </summary>
    EnhancedCDMedia = 87,

    /// <summary>
    ///     Enhanced DVD media.
    /// </summary>
    EnhancedDVDMedia = 88,

    /// <summary>
    ///     High definition DVD media in the HD DVD format.
    /// </summary>
    HDDVDMedia = 89,

    /// <summary>
    ///     High definition DVD media in the Blu-ray Disc™ format.
    /// </summary>
    BluRayMedia = 90,

    /// <summary>
    ///     Video CD (VCD) media.
    /// </summary>
    VCDMedia = 91,

    /// <summary>
    ///     DVD+R media.
    /// </summary>
    DVDPlusRMedia = 92,

    /// <summary>
    ///     DVD+RW media.
    /// </summary>
    DVDPlusRWMedia = 93,

    /// <summary>
    ///     A desktop computer.
    /// </summary>
    DesktopPC = 94,

    /// <summary>
    ///     A mobile computer (laptop).
    /// </summary>
    MobilePC = 95,

    /// <summary>
    ///     The <b>User Accounts</b> Control Panel item.
    /// </summary>
    Users = 96,

    /// <summary>
    ///     Smart media.
    /// </summary>
    SmartMedia = 97,

    /// <summary>
    ///     CompactFlash media.
    /// </summary>
    CompactFlashMedia = 98,

    /// <summary>
    ///     A cell phone.
    /// </summary>
    CellPhone = 99,

    /// <summary>
    ///     A digital camera.
    /// </summary>
    Camera = 100,

    /// <summary>
    ///     A digital video camera.
    /// </summary>
    VideoCamera = 101,

    /// <summary>
    ///     An audio player.
    /// </summary>
    AudioPlayer = 102,

    /// <summary>
    ///     Connect to network.
    /// </summary>
    NetworkConnect = 103,

    /// <summary>
    ///     The <b>Network and Internet</b> Control Panel item.
    /// </summary>
    Internet = 104,

    /// <summary>
    ///     A compressed file with a .zip file name extension.
    /// </summary>
    ZipFile = 105,

    /// <summary>
    ///     The <b>Additional Options</b> Control Panel item.
    /// </summary>
    Settings = 106,

    /// <summary>
    ///     <b>Windows Vista with Service Pack 1 (SP1) and later.</b><br />
    ///     High definition DVD drive (any type - HD DVD-ROM, HD DVD-R, HD-DVD-RAM) that uses the HD DVD format.
    /// </summary>
    HDDVDDrive = 132,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     High definition DVD drive (any type - BD-ROM, BD-R, BD-RE) that uses the Blu-ray Disc format.
    /// </summary>
    BDDrive = 133,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     High definition DVD-ROM media in the HD DVD-ROM format.
    /// </summary>
    HDDVDROMMedia = 134,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     High definition DVD-R media in the HD DVD-R format.
    /// </summary>
    HDDVDRMedia = 135,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     High definition DVD-RAM media in the HD DVD-RAM format.
    /// </summary>
    HDDVDRAMMedia = 136,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     High definition DVD-ROM media in the Blu-ray Disc BD-ROM format.
    /// </summary>
    BDROMMedia = 137,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     High definition write-once media in the Blu-ray Disc BD-R format.
    /// </summary>
    BDRMedia = 138,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     High definition read/write media in the Blu-ray Disc BD-RE format.
    /// </summary>
    BDREMedia = 139,

    /// <summary>
    ///     <b>Windows Vista with SP1 and later.</b><br />
    ///     A cluster disk array.
    /// </summary>
    ClusteredDrive = 140,

    /// <summary>
    ///     The highest valid value in the enumeration. Values over 160 are Windows 7-only icons.
    /// </summary>
    MaxValue = 175
}