namespace EcologicInnovations.Web.ViewModels.Products;

/// <summary>
/// Represents one category item shown in the left sidebar filter on the products page.
/// </summary>
public class ProductCategoryFilterItemViewModel
{
    /// <summary>
    /// Category id used by filter logic.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category slug used in querystring or future SEO category links.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Number of matching published products under the category.
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// True when the category is currently selected.
    /// </summary>
    public bool IsSelected { get; set; }
}
