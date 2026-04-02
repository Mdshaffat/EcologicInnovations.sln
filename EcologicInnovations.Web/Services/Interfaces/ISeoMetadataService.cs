using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Builds SEO metadata with consistent fallback rules.
/// </summary>
public interface ISeoMetadataService
{
    Task<SeoMetaViewModel> BuildDefaultAsync(
        string? title = null,
        string? description = null,
        string? canonicalPath = null,
        string? ogImageUrl = null,
        string? robots = null,
        CancellationToken cancellationToken = default);

    Task<SeoMetaViewModel> BuildForSitePageAsync(
        SitePage? page,
        string? canonicalPath = null,
        CancellationToken cancellationToken = default);

    Task<SeoMetaViewModel> BuildForProductAsync(
        Product product,
        string? canonicalPath = null,
        CancellationToken cancellationToken = default);

    Task<SeoMetaViewModel> BuildForBlogPostAsync(
        BlogPost blogPost,
        string? canonicalPath = null,
        CancellationToken cancellationToken = default);
}
