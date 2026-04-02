namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Filter, sort, and paging input state for the Blog admin list page.
/// </summary>
public class BlogAdminListFilterViewModel
{
    /// <summary>
    /// Keyword search across title, slug, excerpt, and category name.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Selected blog category id filter. Null means all categories.
    /// </summary>
    public int? BlogCategoryId { get; set; }

    /// <summary>
    /// Filter by published state. Null means all.
    /// </summary>
    public bool? IsPublished { get; set; }

    /// <summary>
    /// Filter by featured state. Null means all.
    /// </summary>
    public bool? IsFeatured { get; set; }

    /// <summary>
    /// Filter by whether the contact form is shown below the blog details page.
    /// Null means all.
    /// </summary>
    public bool? ShowContactForm { get; set; }

    /// <summary>
    /// Filter by publication date state. True = has PublishedAt, false = no PublishedAt, null = all.
    /// </summary>
    public bool? HasPublishedAt { get; set; }

    /// <summary>
    /// Supported values:
    /// newest, oldest, title_asc, title_desc, published_desc, published_asc, featured_first
    /// </summary>
    public string SortBy { get; set; } = "published_desc";

    /// <summary>
    /// Current page number starting from 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of rows shown per page.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
