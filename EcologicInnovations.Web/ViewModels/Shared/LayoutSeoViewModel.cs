using EcologicInnovations.Web.Models.Seo;

namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Aggregates all layout-level SEO data needed by the shared head partial.
/// </summary>
public class LayoutSeoViewModel
{
    /// <summary>
    /// Final page-level meta model.
    /// </summary>
    public SeoMetaViewModel Meta { get; set; } = new();

    /// <summary>
    /// Optional JSON-LD blocks to render in the page head.
    /// </summary>
    public List<SchemaMarkupResult> SchemaBlocks { get; set; } = new();
}
