using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Read-only details model for a Blog Category.
/// </summary>
public class BlogCategoryAdminDetailsViewModel
{
    public int Id { get; set; }

    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Sort order.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Active state.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Number of assigned blog posts.
    /// </summary>
    public int BlogCount { get; set; }

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last updated date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Breadcrumb data for later expansion if needed.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();
}
