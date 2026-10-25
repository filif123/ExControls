namespace ExControls;

/// <summary>
/// Anchor of the toolbox icons in the <c>Toolbox</c> folder: <c>[ToolboxBitmap(typeof(ToolboxIcons), "Toolbox.Name.ico")]</c>.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="ToolboxBitmapAttribute" /> and the toolbox of Visual Studio look the icon up as the resource
/// <c>namespace of the type + "." + name</c>. The toolbox of .NET projects reads the attribute from metadata and searches
/// only the assembly of the control, so an icon of a Windows Forms control (<c>typeof(Button), "Button.bmp"</c>) shows
/// the default component icon there. On .NET Framework the in-process designer loads it from System.Windows.Forms,
/// so controls derived from Windows Forms keep those icons there (<c>#if NETFRAMEWORK</c>).
/// </para>
/// <para>
/// Each icon has a single 16x16 frame with fully opaque or fully transparent pixels and, like the Windows Forms icons,
/// a 1 px light halo (#F6F6F6) between dark pixels and transparency: the toolbox of .NET projects shows no image for
/// a multi-size icon, draws semi-transparent pixels with magenta edges and loses strokes without a halo.
/// </para>
/// </remarks>
internal static class ToolboxIcons;
