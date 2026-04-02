namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents a reusable empty-state block for public or admin pages that may have no data.
/// </summary>
public class EmptyStateViewModel
{
    /// <summary>
    /// Main empty-state title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Supporting description shown below the title.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Optional Bootstrap icon class.
    /// Example: "bi bi-box-seam" or "bi bi-search".
    /// </summary>
    public string? IconCssClass { get; set; }

    /// <summary>
    /// Optional CTA button text.
    /// </summary>
    public string? ButtonText { get; set; }

    /// <summary>
    /// Optional CTA button URL.
    /// </summary>
    public string? ButtonUrl { get; set; }

    /// <summary>
    /// Optional Bootstrap button class.
    /// Example: "btn-primary" or "btn-outline-secondary".
    /// </summary>
    public string ButtonCssClass { get; set; } = "btn-primary";

    /// <summary>
    /// Optional theme CSS helper class for custom empty-state accents.
    /// </summary>
    public string? ThemeCssClass { get; set; }
}
