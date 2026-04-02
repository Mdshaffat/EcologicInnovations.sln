namespace EcologicInnovations.Web.Models.Seo;

/// <summary>
/// Represents one URL entry in sitemap.xml.
/// </summary>
public class SitemapUrlItem
{
    /// <summary>
    /// Absolute canonical URL.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Optional last modification date in UTC.
    /// </summary>
    public DateTime? LastModifiedUtc { get; set; }

    /// <summary>
    /// Optional change frequency hint.
    /// </summary>
    public string? ChangeFrequency { get; set; }

    /// <summary>
    /// Optional priority hint.
    /// </summary>
    public decimal? Priority { get; set; }
}
