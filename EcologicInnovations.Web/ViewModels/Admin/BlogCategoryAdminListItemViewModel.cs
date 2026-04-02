namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents one category row in the Blog Category admin list page.
/// </summary>
public class BlogCategoryAdminListItemViewModel
{
    /// <summary>
    /// Primary key of the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public category name.
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
    /// Admin-controlled display order.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Indicates whether the category is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Number of blog posts assigned to the category.
    /// </summary>
    public int BlogCount { get; set; }

    /// <summary>
    /// When the category was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
