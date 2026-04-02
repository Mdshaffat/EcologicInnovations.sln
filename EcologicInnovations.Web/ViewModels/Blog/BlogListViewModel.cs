using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Blog;

/// <summary>
/// Main public blog list page model.
/// It contains filters, cards, pagination, breadcrumbs, and SEO data.
/// </summary>
public class BlogListViewModel
{
    /// <summary>
    /// Public page heading.
    /// </summary>
    public string PageTitle { get; set; } = "Blog";

    /// <summary>
    /// Optional intro text shown under the heading.
    /// </summary>
    public string? IntroText { get; set; }

    /// <summary>
    /// Sidebar filter model.
    /// </summary>
    public BlogFilterSidebarViewModel Sidebar { get; set; } = new();

    /// <summary>
    /// Current article cards for the page.
    /// </summary>
    public List<BlogCardViewModel> Posts { get; set; } = new();

    /// <summary>
    /// Pagination model for navigation.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// SEO metadata.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();

    /// <summary>
    /// Breadcrumb trail.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();

    /// <summary>
    /// Total matching result count.
    /// </summary>
    public int ResultCount { get; set; }

    /// <summary>
    /// Optional selected category name.
    /// </summary>
    public string? SelectedCategoryName { get; set; }

    /// <summary>
    /// Empty-state block when nothing matches.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
