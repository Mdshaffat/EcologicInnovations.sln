namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents a reusable visual placeholder when an expected image URL is missing.
/// </summary>
public class ImagePlaceholderViewModel
{
    /// <summary>
    /// Main placeholder title or label.
    /// </summary>
    public string Title { get; set; } = "No Image";

    /// <summary>
    /// Optional helper text shown under the title.
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Optional Bootstrap icon class shown in the placeholder area.
    /// </summary>
    public string IconCssClass { get; set; } = "bi bi-image";

    /// <summary>
    /// Optional outer wrapper CSS class for height/spacing control.
    /// </summary>
    public string? WrapperCssClass { get; set; }

    /// <summary>
    /// Optional additional CSS class for the placeholder body.
    /// </summary>
    public string? ThemeCssClass { get; set; }
}
