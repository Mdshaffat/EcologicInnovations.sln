namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Filter model for the admin media library page.
/// </summary>
public class MediaLibraryFilterViewModel
{
    /// <summary>
    /// Keyword used to search file name, original file name, title, alt text, or public URL.
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// Optional media group filter such as Blog, Product, About, Policy, Banner, Logo, or General.
    /// </summary>
    public string? MediaGroup { get; set; }

    /// <summary>
    /// Optional active-state filter.
    /// Null means all.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Sort option for the media list.
    /// Supported values: newest, oldest, group_asc, group_desc, title_asc, title_desc.
    /// </summary>
    public string SortBy { get; set; } = "newest";

    /// <summary>
    /// Current page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size for card-based media browsing.
    /// </summary>
    public int PageSize { get; set; } = 24;
}
