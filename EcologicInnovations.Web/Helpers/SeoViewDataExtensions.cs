using EcologicInnovations.Web.Models.Seo;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Strongly typed helpers for placing SEO metadata and JSON-LD blocks into ViewData.
/// </summary>
public static class SeoViewDataExtensions
{
    private const string SeoMetaKey = "SeoMeta";
    private const string SchemaBlocksKey = "SchemaBlocks";

    /// <summary>
    /// Stores page SEO metadata in ViewData so the shared layout can render it.
    /// </summary>
    public static void SetSeoMeta(this ViewDataDictionary viewData, SeoMetaViewModel seoMeta)
    {
        viewData[SeoMetaKey] = seoMeta;
    }

    /// <summary>
    /// Retrieves page SEO metadata from ViewData if present.
    /// </summary>
    public static SeoMetaViewModel? GetSeoMeta(this ViewDataDictionary viewData)
    {
        return viewData[SeoMetaKey] as SeoMetaViewModel;
    }

    /// <summary>
    /// Stores structured data blocks in ViewData.
    /// </summary>
    public static void SetSchemaBlocks(this ViewDataDictionary viewData, List<SchemaMarkupResult> schemaBlocks)
    {
        viewData[SchemaBlocksKey] = schemaBlocks;
    }

    /// <summary>
    /// Retrieves structured data blocks from ViewData if present.
    /// </summary>
    public static List<SchemaMarkupResult> GetSchemaBlocks(this ViewDataDictionary viewData)
    {
        return viewData[SchemaBlocksKey] as List<SchemaMarkupResult> ?? new List<SchemaMarkupResult>();
    }
}
