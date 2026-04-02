using EcologicInnovations.Web.Models.Seo;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Builds the final robots.txt content for the public site.
/// </summary>
public interface IRobotsService
{
    /// <summary>
    /// Builds the robots.txt model.
    /// </summary>
    Task<RobotsFileModel> BuildAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Renders the robots file as plain text.
    /// </summary>
    Task<string> RenderAsync(CancellationToken cancellationToken = default);
}
