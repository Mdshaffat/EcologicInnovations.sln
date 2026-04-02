using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin list page model for Blog Categories.
/// </summary>
public class BlogCategoryAdminListViewModel
{
    /// <summary>
    /// Current filter state.
    /// </summary>
    public BlogCategoryAdminListFilterViewModel Filter { get; set; } = new();

    /// <summary>
    /// Rows to render for the current page.
    /// </summary>
    public List<BlogCategoryAdminListItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Pagination state.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// Total matching rows.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total active categories.
    /// </summary>
    public int ActiveCount { get; set; }

    /// <summary>
    /// Optional empty-state information.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
