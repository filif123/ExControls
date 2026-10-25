using System.ComponentModel;
using System.Reflection;

namespace ExControls.Tests.Controls;

/// <summary>
/// Toolbox icons: <see cref="ToolboxBitmapAttribute" /> and the toolbox of Visual Studio look the bitmap up as the resource
/// <c>namespace of the type + "." + name</c>. The toolbox of .NET projects searches only the assembly of the control
/// (an icon of a Windows Forms control shows the default icon there), shows no image for a multi-size icon, draws
/// semi-transparent pixels with magenta edges and loses strokes without a halo. On .NET Framework controls derived
/// from Windows Forms keep their icons.
/// </summary>
[TestClass]
public class ToolboxBitmapTests
{
    private static readonly Assembly Library = typeof(ExOptionsView).Assembly;

    [TestMethod]
    public void ToolboxBitmap_KazdaPolozkaToolboxuMaIkonu()
    {
        var items = LoadableTypes().Where(IsToolboxItem).ToList();
        Assert.IsTrue(items.Count > 0);

        foreach (var type in items)
        {
            var icon = DeclaredIcon(type);
            Assert.IsNotNull(icon, $"{type.Name} has no ToolboxBitmap(Type, string)");
#if !NETFRAMEWORK
            Assert.AreEqual(Library, icon.Value.Source.Assembly, $"{type.Name}: the toolbox of .NET projects finds only icons of the library");
#endif
        }
    }

    [TestMethod]
    public void ToolboxBitmap_IkonySaNajduANacitaju()
    {
        var icons = LoadableTypes().Select(t => (Type: t, Icon: DeclaredIcon(t))).Where(x => x.Icon is not null).ToList();
        Assert.IsTrue(icons.Count > 0);
        var fallback = (Bitmap)new ToolboxBitmapAttribute(typeof(ToolboxBitmapTests), "Missing.ico").GetImage(typeof(ToolboxBitmapTests), false)!;

        foreach (var (type, icon) in icons)
        {
            var (source, name) = icon!.Value;
            using (var stream = source.Assembly.GetManifestResourceStream(source, name))
                Assert.IsNotNull(stream, $"{type.Name}: resource {source.Namespace}.{name} not found in {source.Assembly.GetName().Name}");

            var image = new ToolboxBitmapAttribute(source, name).GetImage(source, false) as Bitmap;
            Assert.IsNotNull(image, type.Name);
            Assert.AreEqual(new Size(16, 16), image.Size, type.Name);
            Assert.IsFalse(SamePixels(image, fallback), $"{type.Name} shows the default component icon");
        }
    }

    /// <summary>
    /// Like the Windows Forms icons: a single 16x16 image, fully opaque or transparent pixels and a light halo
    /// (#F6F6F6) between dark pixels and transparency - the toolbox loses strokes that border transparent pixels directly.
    /// </summary>
    [TestMethod]
    public void ToolboxBitmap_IkonyKniznice_Jeden16PxObrazokBezPolopriehladnostiSLemom()
    {
        var names = Library.GetManifestResourceNames().Where(n => n.EndsWith(".ico", StringComparison.OrdinalIgnoreCase)).ToList();
        Assert.IsTrue(names.Count > 0);

        foreach (var name in names)
        {
            using var stream = Library.GetManifestResourceStream(name)!;
            using var reader = new BinaryReader(stream);
            reader.ReadUInt16();
            reader.ReadUInt16();
            Assert.AreEqual(1, reader.ReadUInt16(), $"{name}: images in the icon");
            Assert.AreEqual(16, reader.ReadByte(), $"{name}: width");
            Assert.AreEqual(16, reader.ReadByte(), $"{name}: height");

            stream.Position = 0;
            using var icon = new Icon(stream);
            using var bitmap = icon.ToBitmap();
            for (var y = 0; y < bitmap.Height; y++)
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    Assert.IsTrue(pixel.A is 0 or 255, $"{name}: pixel {x},{y} has alpha {pixel.A}");
                    if (pixel.A != 0 && Math.Min(pixel.R, Math.Min(pixel.G, pixel.B)) < 230)
                        Assert.IsFalse(TouchesTransparent(bitmap, x, y), $"{name}: dark pixel {x},{y} borders transparency without a halo");
                }
        }
    }

    private static bool TouchesTransparent(Bitmap bitmap, int x, int y)
    {
        for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
            {
                int nx = x + dx, ny = y + dy;
                if (nx >= 0 && ny >= 0 && nx < bitmap.Width && ny < bitmap.Height && bitmap.GetPixel(nx, ny).A == 0)
                    return true;
            }

        return false;
    }

    /// <summary>
    /// <see cref="ToolboxBitmapAttribute" /> declared on the type, as the attribute data - instances of the attributes
    /// of controls would load their designers (on .NET from the design tools server).
    /// </summary>
    private static (Type Source, string Name)? DeclaredIcon(Type type)
    {
        var data = type.GetCustomAttributesData()
            .FirstOrDefault(a => a.AttributeType == typeof(ToolboxBitmapAttribute) && a.ConstructorArguments.Count == 2);
        if (data?.ConstructorArguments[0].Value is not Type source)
            return null;

        return (source, (string)data.ConstructorArguments[1].Value!);
    }

    /// <summary>
    /// Public component offered by the toolbox (like the toolbox of Visual Studio): not abstract, generic or nested,
    /// without <c>[ToolboxItem(false)]</c> and <c>[DesignTimeVisible(false)]</c> on the type or its base types.
    /// </summary>
    private static bool IsToolboxItem(Type type) =>
        type.IsPublic && !type.IsAbstract && !type.IsGenericTypeDefinition && typeof(IComponent).IsAssignableFrom(type)
        && FirstBoolean(type, typeof(ToolboxItemAttribute)) != false && FirstBoolean(type, typeof(DesignTimeVisibleAttribute)) != false;

    private static bool? FirstBoolean(Type? type, Type attribute)
    {
        for (; type is not null; type = type.BaseType)
        {
            var data = type.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType == attribute);
            if (data is not null)
                return data.ConstructorArguments.Count != 1 || data.ConstructorArguments[0].Value is not bool value || value;
        }

        return null;
    }

    /// <summary>
    /// Types of the library without the designer types of .NET, which need the design tools server (not in the test output).
    /// </summary>
    private static IEnumerable<Type> LoadableTypes()
    {
        try
        {
            return Library.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.OfType<Type>();
        }
    }

    private static bool SamePixels(Bitmap a, Bitmap b)
    {
        if (a.Size != b.Size)
            return false;

        for (var y = 0; y < a.Height; y++)
            for (var x = 0; x < a.Width; x++)
                if (a.GetPixel(x, y) != b.GetPixel(x, y))
                    return false;

        return true;
    }
}
