using System.Globalization;
using System.Text;
using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Seo;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Builds sitemap.xml from public pages, product details pages, and blog details pages.
/// </summary>
public class SitemapService : ISitemapService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly SeoOptions _seoOptions;
    private readonly SiteRuntimeOptions _siteRuntimeOptions;

    public SitemapService(
        ApplicationDbContext dbContext,
        IOptions<SeoOptions> seoOptions,
        IOptions<SiteRuntimeOptions> siteRuntimeOptions)
    {
        _dbContext = dbContext;
        _seoOptions = seoOptions.Value;
        _siteRuntimeOptions = siteRuntimeOptions.Value;
    }

    public async Task<SitemapDocumentModel> BuildAsync(CancellationToken cancellationToken = default)
    {
        var model = new SitemapDocumentModel();
        var root = (_seoOptions.BaseUrl ?? string.Empty).TrimEnd('/');

        if (string.IsNullOrWhiteSpace(root))
        {
            return model;
        }

        model.Urls.Add(new SitemapUrlItem
        {
            Location = $"{root}/",
            ChangeFrequency = "weekly",
            Priority = 1.0m
        });

        model.Urls.Add(new SitemapUrlItem
        {
            Location = $"{root}/about-us",
            ChangeFrequency = "monthly",
            Priority = 0.8m
        });

        model.Urls.Add(new SitemapUrlItem
        {
            Location = $"{root}/policy",
            ChangeFrequency = "monthly",
            Priority = 0.5m
        });

        model.Urls.Add(new SitemapUrlItem
        {
            Location = $"{root}/products",
            ChangeFrequency = "weekly",
            Priority = 0.9m
        });

        model.Urls.Add(new SitemapUrlItem
        {
            Location = $"{root}/blog",
            ChangeFrequency = "weekly",
            Priority = 0.8m
        });

        model.Urls.Add(new SitemapUrlItem
        {
            Location = $"{root}/contact",
            ChangeFrequency = "monthly",
            Priority = 0.5m
        });

        var products = await _dbContext.Products
            .AsNoTracking()
            .Where(x => x.IsPublished && x.IsActive)
            .OrderBy(x => x.Title)
            .Select(x => new
            {
                x.Slug,
                LastModified = x.UpdatedAt ?? x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            model.Urls.Add(new SitemapUrlItem
            {
                Location = $"{root}/products/{product.Slug}",
                LastModifiedUtc = product.LastModified,
                ChangeFrequency = "monthly",
                Priority = 0.8m
            });
        }

        var blogs = await _dbContext.BlogPosts
            .AsNoTracking()
            .Where(x => _siteRuntimeOptions.IncludeUnpublishedInSitemap || x.IsPublished)
            .OrderByDescending(x => x.PublishedAt ?? x.CreatedAt)
            .Select(x => new
            {
                x.Slug,
                LastModified = x.UpdatedAt ?? x.PublishedAt ?? x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        foreach (var blog in blogs)
        {
            model.Urls.Add(new SitemapUrlItem
            {
                Location = $"{root}/blog/{blog.Slug}",
                LastModifiedUtc = blog.LastModified,
                ChangeFrequency = "monthly",
                Priority = 0.7m
            });
        }

        return model;
    }

    public async Task<string> RenderXmlAsync(CancellationToken cancellationToken = default)
    {
        var sitemap = await BuildAsync(cancellationToken);

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var item in sitemap.Urls)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(item.Location)}</loc>");

            if (item.LastModifiedUtc.HasValue)
            {
                sb.AppendLine($"    <lastmod>{item.LastModifiedUtc.Value.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}</lastmod>");
            }

            if (!string.IsNullOrWhiteSpace(item.ChangeFrequency))
            {
                sb.AppendLine($"    <changefreq>{item.ChangeFrequency}</changefreq>");
            }

            if (item.Priority.HasValue)
            {
                sb.AppendLine($"    <priority>{item.Priority.Value.ToString("0.0", CultureInfo.InvariantCulture)}</priority>");
            }

            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");
        return sb.ToString();
    }
}
