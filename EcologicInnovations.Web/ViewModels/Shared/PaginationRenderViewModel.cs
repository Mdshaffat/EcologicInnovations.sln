namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// View model used by the shared pagination partial.
/// It carries the pagination state plus the current path and query parameters.
/// </summary>
public class PaginationRenderViewModel
{
    /// <summary>
    /// Existing pagination metadata for the page.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// Current request path such as /products, /blog, or /admin/messages.
    /// </summary>
    public string CurrentPath { get; set; } = "/";

    /// <summary>
    /// Current querystring values that must be preserved across page clicks.
    /// </summary>
    public Dictionary<string, string?> Query { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Querystring key used for page navigation.
    /// </summary>
    public string PageParameterName { get; set; } = "page";

    /// <summary>
    /// Number of pages to show on both sides of the current page.
    /// </summary>
    public int SurroundingPageCount { get; set; } = 2;
}
