namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents one category row in the admin list page.
/// </summary>
public class ProductCategoryAdminListItemViewModel
{
    /// <summary>
    /// Category primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category name shown prominently in the list.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional short description shown in truncated form.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Display order used in public filter lists and admin ordering.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Indicates whether the category is available for public use.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Number of products currently assigned to this category.
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// When the category was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
