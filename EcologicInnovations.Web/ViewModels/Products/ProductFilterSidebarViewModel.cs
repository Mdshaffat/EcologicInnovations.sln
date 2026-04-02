using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Products;

/// <summary>
/// Represents the full left sidebar filtering state for the public products page.
/// </summary>
public class ProductFilterSidebarViewModel
{
    /// <summary>
    /// Category items shown in the sidebar.
    /// </summary>
    public List<ProductCategoryFilterItemViewModel> Categories { get; set; } = new();

    /// <summary>
    /// Current search keyword entered by the user.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Current selected category id if filtering by a single category.
    /// </summary>
    public int? SelectedCategoryId { get; set; }

    /// <summary>
    /// Current selected category slug used by the public list page.
    /// </summary>
    public string? SelectedCategorySlug { get; set; }

    /// <summary>
    /// Current sort option such as featured, newest, title_asc, or title_desc.
    /// </summary>
    public string SortBy { get; set; } = "featured";

    /// <summary>
    /// Sort dropdown options rendered above the results list.
    /// </summary>
    public List<SelectListItem> SortOptions { get; set; } = new();

    /// <summary>
    /// URL used to clear category and search filters quickly.
    /// </summary>
    public string? ClearFilterUrl { get; set; }
}
