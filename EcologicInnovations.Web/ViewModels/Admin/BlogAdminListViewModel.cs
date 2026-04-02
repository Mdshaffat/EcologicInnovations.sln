using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin list page model for Blogs.
/// </summary>
public class BlogAdminListViewModel
{
    /// <summary>
    /// Current list filter and sort state.
    /// </summary>
    public BlogAdminListFilterViewModel Filter { get; set; } = new();

    /// <summary>
    /// Blog rows for the current page.
    /// </summary>
    public List<BlogAdminListItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Blog category dropdown options for the filter toolbar and forms.
    /// </summary>
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    /// <summary>
    /// Pagination state for the current list result.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// Total number of matching blog posts.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total published blog count.
    /// </summary>
    public int PublishedCount { get; set; }

    /// <summary>
    /// Total featured blog count.
    /// </summary>
    public int FeaturedCount { get; set; }

    /// <summary>
    /// Total blog posts with contact form enabled.
    /// </summary>
    public int ContactEnabledCount { get; set; }

    /// <summary>
    /// Optional empty-state model.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
