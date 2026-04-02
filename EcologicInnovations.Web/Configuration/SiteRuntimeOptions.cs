namespace EcologicInnovations.Web.Configuration;

/// <summary>
/// Holds runtime site behavior flags that affect SEO endpoints and generation rules.
/// </summary>
public class SiteRuntimeOptions
{
    /// <summary>
    /// Whether the sitemap endpoint should include unpublished content.
    /// This should remain false for production.
    /// </summary>
    public bool IncludeUnpublishedInSitemap { get; set; } = false;

    /// <summary>
    /// Whether robots.txt should explicitly disallow admin routes.
    /// </summary>
    public bool DisallowAdminInRobots { get; set; } = true;

    /// <summary>
    /// Whether robots.txt should disallow account/login pages.
    /// </summary>
    public bool DisallowAccountPagesInRobots { get; set; } = true;
}
