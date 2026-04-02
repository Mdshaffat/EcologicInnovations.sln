using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Products;

/// <summary>
/// Main public products list page viewmodel.
/// It contains the filter sidebar, product cards, pagination state, breadcrumbs, and SEO metadata.
/// </summary>
public class ProductListViewModel
{
    /// <summary>
    /// Page heading shown above the products list.
    /// </summary>
    public string PageTitle { get; set; } = "Products";

    /// <summary>
    /// Optional supporting introduction shown on the products page.
    /// </summary>
    public string? IntroText { get; set; }

    /// <summary>
    /// Left sidebar filter model.
    /// </summary>
    public ProductFilterSidebarViewModel Sidebar { get; set; } = new();

    /// <summary>
    /// Resulting product cards for the current filter/search/page.
    /// </summary>
    public List<ProductCardViewModel> Products { get; set; } = new();

    /// <summary>
    /// Pagination state for list navigation.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// SEO metadata for the products list page.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();

    /// <summary>
    /// Breadcrumb trail used near the top of the page.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();

    /// <summary>
    /// Total number of results matching the current filter.
    /// </summary>
    public int ResultCount { get; set; }

    /// <summary>
    /// Optional currently selected category name.
    /// </summary>
    public string? SelectedCategoryName { get; set; }

    /// <summary>
    /// Empty-state model used when no products match the current filter.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
