using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Seo;
using EcologicInnovations.Web.Services.Interfaces;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Generates focused schema.org blocks for Organization, Product, and Article pages.
/// </summary>
public class SchemaMarkupService : ISchemaMarkupService
{
    private readonly ISiteSettingsService _siteSettingsService;

    public SchemaMarkupService(ISiteSettingsService siteSettingsService)
    {
        _siteSettingsService = siteSettingsService;
    }

    public async Task<SchemaMarkupResult> BuildOrganizationAsync(CancellationToken cancellationToken = default)
    {
        var site = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);

        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Organization",
            ["name"] = string.IsNullOrWhiteSpace(site.CompanyName) ? "Ecologic Innovations" : site.CompanyName,
            ["url"] = null,
            ["logo"] = site.LogoUrl,
            ["email"] = site.SupportEmail,
            ["telephone"] = site.Phone,
            ["sameAs"] = new[]
            {
                site.FacebookUrl,
                site.LinkedInUrl,
                site.YouTubeUrl
            }.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()
        };

        return new SchemaMarkupResult
        {
            Json = SchemaJsonHelper.Serialize(payload)
        };
    }

    public SchemaMarkupResult BuildProduct(Product product, string absoluteUrl)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Product",
            ["name"] = product.Title,
            ["description"] = product.ShortDescription,
            ["image"] = string.IsNullOrWhiteSpace(product.MainImageUrl) ? null : new[] { product.MainImageUrl },
            ["url"] = absoluteUrl,
            ["category"] = product.ProductCategory?.Name
        };

        return new SchemaMarkupResult
        {
            Json = SchemaJsonHelper.Serialize(payload)
        };
    }

    public SchemaMarkupResult BuildArticle(BlogPost blogPost, string absoluteUrl)
    {
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Article",
            ["headline"] = blogPost.Title,
            ["description"] = blogPost.Excerpt,
            ["image"] = string.IsNullOrWhiteSpace(blogPost.FeatureImageUrl) ? null : new[] { blogPost.FeatureImageUrl },
            ["datePublished"] = blogPost.PublishedAt?.ToString("yyyy-MM-dd"),
            ["dateModified"] = (blogPost.UpdatedAt ?? blogPost.PublishedAt ?? blogPost.CreatedAt).ToString("yyyy-MM-dd"),
            ["mainEntityOfPage"] = absoluteUrl,
            ["articleSection"] = blogPost.BlogCategory?.Name
        };

        return new SchemaMarkupResult
        {
            Json = SchemaJsonHelper.Serialize(payload)
        };
    }
}
