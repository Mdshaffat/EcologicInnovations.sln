using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.About;

/// <summary>
/// Strongly typed public page model for the About Us page.
/// It carries the sanitized HTML body plus SEO and breadcrumb information
/// required by the public view and shared layout.
/// </summary>
public class AboutPageViewModel
{
    /// <summary>
    /// Public page title shown in the hero and page heading.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Slug stored in the database for the About Us page record.
    /// </summary>
    public string Slug { get; set; } = "about-us";

    /// <summary>
    /// Optional banner image URL for the hero section.
    /// </summary>
    public string? BannerImageUrl { get; set; }

    /// <summary>
    /// Optional short introduction displayed before the rich content.
    /// </summary>
    public string? ShortIntro { get; set; }

    /// <summary>
    /// Sanitized HTML body for the public page.
    /// </summary>
    public string? HtmlContent { get; set; }

    /// <summary>
    /// Breadcrumbs for the public page header.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();

    /// <summary>
    /// SEO metadata prepared by the controller and rendered by the layout partial.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();
}
