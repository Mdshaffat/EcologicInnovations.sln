namespace EcologicInnovations.Web.Models.Seo;

/// <summary>
/// Represents one JSON-LD structured data block ready for rendering.
/// </summary>
public class SchemaMarkupResult
{
    /// <summary>
    /// Raw JSON-LD string.
    /// </summary>
    public string Json { get; set; } = string.Empty;
}
