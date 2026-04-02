using EcologicInnovations.Web.Models.Entities;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Provides site-wide settings from the database with safe fallback behavior.
/// </summary>
public interface ISiteSettingsService
{
    /// <summary>
    /// Gets the primary SiteSetting record or null if none exists.
    /// </summary>
    Task<SiteSetting?> GetPrimaryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the primary SiteSetting record or a safe in-memory fallback object.
    /// </summary>
    Task<SiteSetting> GetPrimaryOrDefaultAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears any in-memory cache of the site settings after admin updates.
    /// </summary>
    void ClearCache();
}
