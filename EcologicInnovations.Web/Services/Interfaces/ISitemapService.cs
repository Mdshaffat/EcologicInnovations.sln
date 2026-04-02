using EcologicInnovations.Web.Models.Seo;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Builds the public sitemap document used for search engine discovery.
/// </summary>
public interface ISitemapService
{
    /// <summary>
    /// Builds the sitemap model from current public content.
    /// </summary>
    Task<SitemapDocumentModel> BuildAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Renders the sitemap as XML.
    /// </summary>
    Task<string> RenderXmlAsync(CancellationToken cancellationToken = default);
}
