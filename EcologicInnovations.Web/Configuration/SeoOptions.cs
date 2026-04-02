namespace EcologicInnovations.Web.Configuration;

/// <summary>
/// Binds the "Seo" section from appsettings.
/// These values act as site-wide SEO defaults when page-specific metadata is missing.
/// </summary>
public class SeoOptions
{
    /// <summary>
    /// Fallback site title or title suffix used across the application.
    /// Example: "Ecologic Innovations".
    /// </summary>
    public string DefaultTitle { get; set; } = "Ecologic Innovations";

    /// <summary>
    /// Optional suffix appended to page titles when appropriate.
    /// Example: "| Ecologic Innovations".
    /// </summary>
    public string? TitleSuffix { get; set; } = "| Ecologic Innovations";

    /// <summary>
    /// Default description used when a page-specific description is unavailable.
    /// </summary>
    public string DefaultDescription { get; set; } =
        "Ecologic Innovations delivers software, sustainability IoT devices, energy equipment, and practical eco-technology solutions.";

    /// <summary>
    /// Fallback Open Graph image URL.
    /// </summary>
    public string? DefaultOgImage { get; set; } = "/images/default-og.jpg";

    /// <summary>
    /// Site-wide robots directive used for normal public pages.
    /// </summary>
    public string DefaultRobots { get; set; } = "index,follow";

    /// <summary>
    /// Organization name used by structured data.
    /// </summary>
    public string OrganizationName { get; set; } = "Ecologic Innovations";

    /// <summary>
    /// Absolute base URL for sitemap and canonical generation when request context is unavailable.
    /// Example: "https://www.ecologicinnovations.com".
    /// </summary>
    public string? BaseUrl { get; set; }
}
