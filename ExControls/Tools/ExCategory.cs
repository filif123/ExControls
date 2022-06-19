using System.Globalization;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ExControls;

/// <inheritdoc />
public class ExCategoryAttribute : CategoryAttribute
{
    /// <summary>
    ///     Gets a type of category
    /// </summary>
    public CategoryType Type { get; }

    /// <inheritdoc />
    public ExCategoryAttribute(CategoryType type) : this(CategoryToString(type))
    {
        Type = type;
    }

    /// <inheritdoc />
    public ExCategoryAttribute(string categoryName) : base(categoryName)
    {
    }

    /// <summary>
    /// Converts category type to resource string.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string CategoryToString(CategoryType type) => SystemResources.GetString<Form>("Cat" + type, CultureInfo.CurrentCulture);
}

/// <summary>
///     Type of category in PropertyGrid.
/// </summary>
public enum CategoryType
{
    /// <summary>
    ///     Default category.
    /// </summary>
    Default,

    /// <summary>
    ///     Action category.
    /// </summary>
    Action,

    /// <summary>
    ///     Appearance category.
    /// </summary>
    Appearance,

    /// <summary>
    ///     Asynchronous category.
    /// </summary>
    Asynchronous,

    /// <summary>
    ///     Behavior category.
    /// </summary>
    Behavior,

    /// <summary>
    ///     Data category.
    /// </summary>
    Data,

    /// <summary>
    ///     Design category.
    /// </summary>
    Design,

    /// <summary>
    ///     DragDrop category.
    /// </summary>
    DragDrop,

    /// <summary>
    ///     Focus category.
    /// </summary>
    Focus,

    /// <summary>
    ///     Format category.
    /// </summary>
    Format,

    /// <summary>
    ///     Key category.
    /// </summary>
    Key,

    /// <summary>
    /// Layout category.
    /// </summary>
    Layout,

    /// <summary>
    ///  Mousde category.
    /// </summary>
    Mouse,

    /// <summary>
    ///     WindowStyle category.
    /// </summary>
    WindowStyle
}