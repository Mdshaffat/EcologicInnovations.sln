namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents all data required by the shared public footer.
/// </summary>
public class PublicFooterViewModel
{
    /// <summary>
    /// Company/site name shown in the footer.
    /// </summary>
    public string SiteName { get; set; } = "Ecologic Innovations";

    /// <summary>
    /// Optional tagline or short description.
    /// </summary>
    public string? Tagline { get; set; }

    /// <summary>
    /// Optional footer logo.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Support email shown in the contact block.
    /// </summary>
    public string? SupportEmail { get; set; }

    /// <summary>
    /// Sales email shown in the contact block.
    /// </summary>
    public string? SalesEmail { get; set; }

    /// <summary>
    /// Public phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Public address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Optional sanitized footer HTML managed by admin.
    /// </summary>
    public string? FooterHtml { get; set; }

    /// <summary>
    /// Public social URLs.
    /// </summary>
    public string? FacebookUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? YouTubeUrl { get; set; }

    /// <summary>
    /// Current year used in the copyright line.
    /// </summary>
    public int CurrentYear { get; set; } = DateTime.UtcNow.Year;
}
