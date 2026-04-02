using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Policy;

/// <summary>
/// Public page model for the Policy page.
/// It holds display content, formatted metadata, breadcrumbs, and SEO information.
/// </summary>
public class PolicyPageViewModel
{
    /// <summary>
    /// Database id of the backing SitePage record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public page title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional banner image URL displayed at the top of the page.
    /// </summary>
    public string? BannerImageUrl { get; set; }

    /// <summary>
    /// Optional short intro text shown before the main content block.
    /// </summary>
    public string? ShortIntro { get; set; }

    /// <summary>
    /// Sanitized HTML content ready for rendering.
    /// </summary>
    public string? HtmlContent { get; set; }

    /// <summary>
    /// Human-friendly updated label shown near the top of the page.
    /// Example: "Updated on 02 April 2026".
    /// </summary>
    public string? UpdatedText { get; set; }

    /// <summary>
    /// Raw updated date value for machine use if needed later.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Breadcrumb trail for the page.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();

    /// <summary>
    /// Final SEO metadata for the page.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();
}
