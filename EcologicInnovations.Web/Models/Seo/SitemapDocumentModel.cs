namespace EcologicInnovations.Web.Models.Seo;

/// <summary>
/// Represents the full sitemap document.
/// </summary>
public class SitemapDocumentModel
{
    /// <summary>
    /// URL items included in sitemap.xml.
    /// </summary>
    public List<SitemapUrlItem> Urls { get; set; } = new();
}
