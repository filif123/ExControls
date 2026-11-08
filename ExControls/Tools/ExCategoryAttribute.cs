using System.Globalization;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ExControls;

/// <summary>
/// Category of a property or event in the property grid; a <see cref="CategoryType" /> gets the localized name of the standard
/// WinForms category.
/// </summary>
public class ExCategoryAttribute : CategoryAttribute
{
    /// <summary>
    /// Gets the type of the category (<see cref="CategoryType.Default" /> for a custom category).
    /// </summary>
    public CategoryType Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExCategoryAttribute" /> class with a standard category.
    /// </summary>
    /// <param name="type">type of the category</param>
    public ExCategoryAttribute(CategoryType type) : this(CategoryToString(type))
    {
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExCategoryAttribute" /> class with a custom category.
    /// </summary>
    /// <param name="categoryName">name of the category</param>
    public ExCategoryAttribute(string categoryName) : base(categoryName)
    {
    }

    /// <summary>
    /// Converts category type to resource string.
    /// </summary>
    /// <param name="type">type of the category</param>
    /// <returns>the localized name of the category as WinForms shows it; the name of the type if it is not found</returns>
    public static string CategoryToString(CategoryType type) => SystemResources.GetString<Form>("Cat" + type, CultureInfo.CurrentCulture) ?? type.ToString();
}
