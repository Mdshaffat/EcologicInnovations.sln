namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents all data required by the shared public navigation bar.
/// </summary>
public class PublicNavigationViewModel
{
    /// <summary>
    /// Company/site name shown in the brand area.
    /// </summary>
    public string SiteName { get; set; } = "Ecologic Innovations";

    /// <summary>
    /// Optional logo URL shown beside the site name.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Optional small site tagline for future use.
    /// </summary>
    public string? Tagline { get; set; }

    /// <summary>
    /// Current request path used to mark active navigation state.
    /// </summary>
    public string CurrentPath { get; set; } = "/";

    /// <summary>
    /// Products intentionally shown under the Products dropdown.
    /// </summary>
    public List<ProductMenuItemViewModel> ProductMenuItems { get; set; } = new();
}
