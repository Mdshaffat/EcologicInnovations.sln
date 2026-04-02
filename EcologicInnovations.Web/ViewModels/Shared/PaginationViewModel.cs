namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents pagination state for list pages such as Products, Blog, admin lists, and media library.
/// </summary>
public class PaginationViewModel
{
    /// <summary>
    /// Current page number starting from 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items shown per page.
    /// </summary>
    public int PageSize { get; set; } = 12;

    /// <summary>
    /// Total number of matching items across all pages.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Computed total pages based on TotalItems and PageSize.
    /// </summary>
    public int TotalPages => PageSize <= 0
        ? 0
        : (int)Math.Ceiling(TotalItems / (double)PageSize);

    /// <summary>
    /// True when there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// True when there is a next page.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Optional route or path base used when generating pager links.
    /// </summary>
    public string? BasePath { get; set; }
}
