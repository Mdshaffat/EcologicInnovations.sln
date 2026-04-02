namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents page-level SEO metadata prepared by controllers/services
/// and rendered by the shared public layout.
/// </summary>
public class SeoMetaViewModel
{
    /// <summary>
    /// Final page title rendered inside the HTML title tag.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Final meta description for the page.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Final canonical URL for the page.
    /// </summary>
    public string? CanonicalUrl { get; set; }

    /// <summary>
    /// Open Graph title.
    /// If empty, Title should be used as fallback.
    /// </summary>
    public string? OgTitle { get; set; }

    /// <summary>
    /// Open Graph description.
    /// If empty, Description should be used as fallback.
    /// </summary>
    public string? OgDescription { get; set; }

    /// <summary>
    /// Open Graph image URL.
    /// </summary>
    public string? OgImageUrl { get; set; }

    /// <summary>
    /// Open Graph type.
    /// Examples: website, article, product.
    /// </summary>
    public string? OgType { get; set; }

    /// <summary>
    /// Meta robots directive.
    /// Example: index,follow or noindex,follow.
    /// </summary>
    public string? Robots { get; set; }
}
