using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin media library page model.
/// It combines filter state, upload form state, media cards, pagination, and small summary data.
/// </summary>
public class MediaLibraryViewModel
{
    /// <summary>
    /// Current active media filters.
    /// </summary>
    public MediaLibraryFilterViewModel Filter { get; set; } = new();

    /// <summary>
    /// Media items shown in the current page.
    /// </summary>
    public List<MediaLibraryItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Upload model shown at the top of the page.
    /// </summary>
    public MediaUploadInputModel UploadModel { get; set; } = new();

    /// <summary>
    /// Pagination state.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// Dropdown items for common media groups.
    /// </summary>
    public List<SelectListItem> MediaGroupOptions { get; set; } = new();

    /// <summary>
    /// Total count of matching media items.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total count of active media items.
    /// </summary>
    public int ActiveCount { get; set; }

    /// <summary>
    /// Maximum allowed upload size in bytes.
    /// </summary>
    public long MaxUploadBytes { get; set; }

    /// <summary>
    /// Allowed extensions shown to the admin for convenience.
    /// </summary>
    public List<string> AllowedExtensions { get; set; } = new();

    /// <summary>
    /// Optional empty-state model when no media exists.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
