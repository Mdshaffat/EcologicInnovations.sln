namespace EcologicInnovations.Web.Models.Enums;

/// <summary>
/// Identifies system-managed single-instance pages that are edited from admin.
/// These pages are stored in the SitePage table but are distinguished by a fixed key.
/// </summary>
public enum SitePageKey
{
    /// <summary>
    /// The About Us page.
    /// </summary>
    AboutUs = 1,

    /// <summary>
    /// The Policy page.
    /// </summary>
    Policy = 2
}
