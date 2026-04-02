namespace EcologicInnovations.Web.ViewModels.Home;

/// <summary>
/// Represents the hero/banner content on the home page.
/// This keeps hero text editable and easy to replace with CMS-backed content later.
/// </summary>
public class HomeHeroViewModel
{
    /// <summary>
    /// Large headline shown in the main hero section.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Supporting subtitle or paragraph under the headline.
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Background or hero illustration image URL.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Primary button text.
    /// </summary>
    public string? PrimaryButtonText { get; set; }

    /// <summary>
    /// Primary button target URL.
    /// </summary>
    public string? PrimaryButtonUrl { get; set; }

    /// <summary>
    /// Secondary button text.
    /// </summary>
    public string? SecondaryButtonText { get; set; }

    /// <summary>
    /// Secondary button target URL.
    /// </summary>
    public string? SecondaryButtonUrl { get; set; }
}
