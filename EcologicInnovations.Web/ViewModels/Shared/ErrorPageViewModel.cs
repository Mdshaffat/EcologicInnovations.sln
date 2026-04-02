namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Shared view model for 404 and generic error pages.
/// </summary>
public class ErrorPageViewModel
{
    /// <summary>
    /// HTTP status code or logical error code if available.
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// Main page heading.
    /// </summary>
    public string Title { get; set; } = "Something went wrong";

    /// <summary>
    /// Supporting message shown under the heading.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Optional CTA text.
    /// </summary>
    public string? PrimaryButtonText { get; set; }

    /// <summary>
    /// Optional CTA URL.
    /// </summary>
    public string? PrimaryButtonUrl { get; set; }

    /// <summary>
    /// Optional secondary CTA text.
    /// </summary>
    public string? SecondaryButtonText { get; set; }

    /// <summary>
    /// Optional secondary CTA URL.
    /// </summary>
    public string? SecondaryButtonUrl { get; set; }

    /// <summary>
    /// Optional icon class for the hero.
    /// </summary>
    public string IconCssClass { get; set; } = "bi bi-exclamation-circle";
}
