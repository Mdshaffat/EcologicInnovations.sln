using EcologicInnovations.Web.ViewModels.Blog;
using EcologicInnovations.Web.ViewModels.Products;
using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Home;

/// <summary>
/// Main public Home page viewmodel.
/// It combines hero content, short About preview, featured products,
/// latest blog cards, value points, CTA content, footer-ready company info,
/// and page-level SEO metadata.
/// </summary>
public class HomePageViewModel
{
    /// <summary>
    /// Hero/banner content shown at the top of the page.
    /// </summary>
    public HomeHeroViewModel Hero { get; set; } = new();

    /// <summary>
    /// About section heading shown on the home page.
    /// </summary>
    public string? AboutTitle { get; set; }

    /// <summary>
    /// Short About summary shown before the CTA to the About page.
    /// </summary>
    public string? AboutSummary { get; set; }

    /// <summary>
    /// Optional About preview image.
    /// </summary>
    public string? AboutImageUrl { get; set; }

    /// <summary>
    /// Featured products displayed in the home showcase area.
    /// </summary>
    public List<ProductCardViewModel> FeaturedProducts { get; set; } = new();

    /// <summary>
    /// Latest published blogs displayed in the insight/article area.
    /// </summary>
    public List<BlogCardViewModel> LatestBlogs { get; set; } = new();

    /// <summary>
    /// Core value proposition cards displayed between sections.
    /// </summary>
    public List<HomeValuePointViewModel> ValuePoints { get; set; } = new();

    /// <summary>
    /// Optional CTA title shown near the bottom of the page.
    /// </summary>
    public string? CtaTitle { get; set; }

    /// <summary>
    /// Supporting CTA text.
    /// </summary>
    public string? CtaText { get; set; }

    /// <summary>
    /// CTA button text.
    /// </summary>
    public string? CtaButtonText { get; set; }

    /// <summary>
    /// CTA button URL.
    /// </summary>
    public string? CtaButtonUrl { get; set; }

    /// <summary>
    /// Company name shown in footer-ready business info blocks.
    /// </summary>
    public string SiteName { get; set; } = "Ecologic Innovations";

    /// <summary>
    /// Optional short company tagline.
    /// </summary>
    public string? SiteTagline { get; set; }

    /// <summary>
    /// Support email shown in the business info/footer area.
    /// </summary>
    public string? SupportEmail { get; set; }

    /// <summary>
    /// Sales email shown in the business info/footer area.
    /// </summary>
    public string? SalesEmail { get; set; }

    /// <summary>
    /// Phone number shown in the business info/footer area.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Optional sanitized footer HTML from SiteSetting.
    /// </summary>
    public string? FooterHtml { get; set; }

    /// <summary>
    /// SEO metadata for the Home page.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();

    /// <summary>
    /// Optional empty-state model used when dynamic content is missing.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
