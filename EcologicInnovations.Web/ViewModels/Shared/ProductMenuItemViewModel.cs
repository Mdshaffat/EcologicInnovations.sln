namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents one product item shown inside the shared top navigation dropdown.
/// Only products intentionally marked for menu visibility are returned here.
/// </summary>
public class ProductMenuItemViewModel
{
    /// <summary>
    /// Product id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public product title shown in the dropdown.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Product slug used to generate the details URL.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional category name for future grouped rendering if needed.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Sort order inside the dropdown.
    /// </summary>
    public int MenuSortOrder { get; set; }

    /// <summary>
    /// Final public URL to the product details page.
    /// </summary>
    public string Url => $"/products/{Slug}";
}
