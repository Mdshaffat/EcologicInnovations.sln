namespace EcologicInnovations.Web.Models.Seo;

/// <summary>
/// Represents the final robots.txt content before rendering.
/// </summary>
public class RobotsFileModel
{
    /// <summary>
    /// List of robots directives to render in order.
    /// </summary>
    public List<string> Lines { get; set; } = new();
}
