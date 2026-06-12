namespace CaliforniumCore.Web.Configuration;

/// <summary>
/// Binds the "Seo" section from appsettings.
/// These values act as site-wide SEO defaults when page-specific metadata is missing.
/// </summary>
public class SeoOptions
{
    /// <summary>
    /// Fallback site title or title suffix used across the application.
    /// Example: "Californium Core".
    /// </summary>
    public string DefaultTitle { get; set; } = "Californium Core";

    /// <summary>
    /// Optional suffix appended to page titles when appropriate.
    /// Example: "| Californium Core".
    /// </summary>
    public string? TitleSuffix { get; set; } = "| Californium Core";

    /// <summary>
    /// Default description used when a page-specific description is unavailable.
    /// </summary>
    public string DefaultDescription { get; set; } =
        "Californium Core builds custom software, smart systems, training programs, and impact-driven technology for businesses and communities.";

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
    public string OrganizationName { get; set; } = "Californium Core";

    /// <summary>
    /// Absolute base URL for sitemap and canonical generation when request context is unavailable.
    /// Example: "https://www.californiumcore.com".
    /// </summary>
    public string? BaseUrl { get; set; }
}
