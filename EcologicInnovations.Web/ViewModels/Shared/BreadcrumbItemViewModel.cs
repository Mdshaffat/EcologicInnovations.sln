namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents one breadcrumb node in public or admin page navigation.
/// </summary>
public class BreadcrumbItemViewModel
{
    /// <summary>
    /// Display text for the breadcrumb item.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional URL. Null or empty means the breadcrumb is the current page.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Indicates whether this breadcrumb item is the active/current page.
    /// </summary>
    public bool IsActive { get; set; }
}
