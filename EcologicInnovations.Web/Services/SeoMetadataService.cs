using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Builds SEO metadata using entity-specific values plus site-wide fallbacks from SiteSetting.
/// </summary>
public class SeoMetadataService : ISeoMetadataService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SeoMetadataService(
        ApplicationDbContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SeoMetaViewModel> BuildDefaultAsync(
        string? title = null,
        string? description = null,
        string? canonicalPath = null,
        string? ogImageUrl = null,
        string? robots = null,
        CancellationToken cancellationToken = default)
    {
        var siteSetting = await _dbContext.SiteSettings
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var request = _httpContextAccessor.HttpContext?.Request;

        var finalTitle = string.IsNullOrWhiteSpace(title)
            ? siteSetting?.MetaTitleDefault ?? siteSetting?.CompanyName ?? "Ecologic Innovations"
            : title;

        var finalDescription = string.IsNullOrWhiteSpace(description)
            ? siteSetting?.MetaDescriptionDefault ?? siteSetting?.Tagline ?? "Ecologic Innovations"
            : description;

        return new SeoMetaViewModel
        {
            Title = finalTitle,
            Description = finalDescription,
            CanonicalUrl = request.ToAbsoluteUrl(canonicalPath ?? request.GetRelativePathAndQuery()),
            OgTitle = finalTitle,
            OgDescription = finalDescription,
            OgImageUrl = request.ToAbsoluteUrl(ogImageUrl ?? siteSetting?.LogoUrl),
            Robots = robots
        };
    }

    public async Task<SeoMetaViewModel> BuildForSitePageAsync(
        SitePage? page,
        string? canonicalPath = null,
        CancellationToken cancellationToken = default)
    {
        if (page is null)
        {
            return await BuildDefaultAsync(
                title: "Page Not Found | Ecologic Innovations",
                description: "The requested page could not be found.",
                canonicalPath: canonicalPath,
                robots: "noindex, nofollow",
                cancellationToken: cancellationToken);
        }

        return await BuildDefaultAsync(
            title: page.MetaTitle ?? page.Title,
            description: page.MetaDescription ?? page.ShortIntro,
            canonicalPath: canonicalPath,
            ogImageUrl: page.BannerImageUrl,
            cancellationToken: cancellationToken);
    }

    public async Task<SeoMetaViewModel> BuildForProductAsync(
        Product product,
        string? canonicalPath = null,
        CancellationToken cancellationToken = default)
    {
        return await BuildDefaultAsync(
            title: product.MetaTitle ?? product.Title,
            description: product.MetaDescription ?? product.ShortDescription,
            canonicalPath: canonicalPath,
            ogImageUrl: product.OgImageUrl ?? product.MainImageUrl,
            cancellationToken: cancellationToken);
    }

    public async Task<SeoMetaViewModel> BuildForBlogPostAsync(
        BlogPost blogPost,
        string? canonicalPath = null,
        CancellationToken cancellationToken = default)
    {
        return await BuildDefaultAsync(
            title: blogPost.MetaTitle ?? blogPost.Title,
            description: blogPost.MetaDescription ?? blogPost.Excerpt,
            canonicalPath: canonicalPath,
            ogImageUrl: blogPost.OgImageUrl ?? blogPost.FeatureImageUrl,
            cancellationToken: cancellationToken);
    }
}
