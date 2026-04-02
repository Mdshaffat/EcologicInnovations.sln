using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Blog;

/// <summary>
/// Represents the filter and search state for the public blog list page.
/// </summary>
public class BlogFilterSidebarViewModel
{
    /// <summary>
    /// Category items shown in the sidebar.
    /// </summary>
    public List<BlogCategoryFilterItemViewModel> Categories { get; set; } = new();

    /// <summary>
    /// Current search keyword.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Current selected category id if needed by the UI.
    /// </summary>
    public int? SelectedCategoryId { get; set; }

    /// <summary>
    /// Current selected category slug used by the public route.
    /// </summary>
    public string? SelectedCategorySlug { get; set; }

    /// <summary>
    /// Current sort option such as newest, oldest, featured, or title.
    /// </summary>
    public string SortBy { get; set; } = "newest";

    /// <summary>
    /// Sort dropdown options rendered above the results list.
    /// </summary>
    public List<SelectListItem> SortOptions { get; set; } = new();

    /// <summary>
    /// URL used to clear the filters quickly.
    /// </summary>
    public string? ClearFilterUrl { get; set; }
}
