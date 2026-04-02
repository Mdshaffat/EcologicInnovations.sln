using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin list page model for Product Categories.
/// </summary>
public class ProductCategoryAdminListViewModel
{
    /// <summary>
    /// Current filter/sort/page state.
    /// </summary>
    public ProductCategoryAdminListFilterViewModel Filter { get; set; } = new();

    /// <summary>
    /// Category rows for the current page.
    /// </summary>
    public List<ProductCategoryAdminListItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Pagination metadata for the current list result.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// Optional small stats block for the page header.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total active categories count.
    /// </summary>
    public int ActiveCount { get; set; }

    /// <summary>
    /// Optional empty-state information when no rows match the filter.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
