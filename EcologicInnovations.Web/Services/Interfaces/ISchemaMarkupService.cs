using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Seo;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Generates structured data blocks for supported public page types.
/// </summary>
public interface ISchemaMarkupService
{
    /// <summary>
    /// Builds Organization schema.
    /// </summary>
    Task<SchemaMarkupResult> BuildOrganizationAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds Product schema for a public product details page.
    /// </summary>
    SchemaMarkupResult BuildProduct(Product product, string absoluteUrl);

    /// <summary>
    /// Builds Article schema for a public blog details page.
    /// </summary>
    SchemaMarkupResult BuildArticle(BlogPost blogPost, string absoluteUrl);
}
