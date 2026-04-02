using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin list page model for Products.
/// </summary>
public class ProductAdminListViewModel
{
    /// <summary>
    /// Current list filter and sort state.
    /// </summary>
    public ProductAdminListFilterViewModel Filter { get; set; } = new();

    /// <summary>
    /// Product rows for the current page.
    /// </summary>
    public List<ProductAdminListItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Product category dropdown options for the filter toolbar.
    /// </summary>
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    /// <summary>
    /// Pagination state for the current list result.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// Total number of matching products across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total published products count.
    /// </summary>
    public int PublishedCount { get; set; }

    /// <summary>
    /// Total featured products count.
    /// </summary>
    public int FeaturedCount { get; set; }

    /// <summary>
    /// Total products configured for the top menu dropdown.
    /// </summary>
    public int MenuProductCount { get; set; }

    /// <summary>
    /// Optional empty-state model when no products match the filter.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
