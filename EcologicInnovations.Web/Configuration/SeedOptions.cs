namespace EcologicInnovations.Web.Configuration;

/// <summary>
/// Controls startup migration and seed behavior.
/// This allows development-friendly automation while keeping production safe.
/// </summary>
public class SeedOptions
{
    /// <summary>
    /// When true, pending EF Core migrations are applied on startup.
    /// Recommended for development environments only unless carefully managed.
    /// </summary>
    public bool ApplyMigrationsOnStartup { get; set; }

    /// <summary>
    /// When true, default core content such as SiteSetting and SitePages is seeded.
    /// </summary>
    public bool SeedCoreContent { get; set; } = true;

    /// <summary>
    /// When true, starter product and blog categories are seeded.
    /// </summary>
    public bool SeedCategories { get; set; } = true;

    /// <summary>
    /// When true, sample products and sample blog posts are seeded if no records exist.
    /// Keep this false in production unless you explicitly want demo content.
    /// </summary>
    public bool SeedSampleCatalogContent { get; set; } = false;

    /// <summary>
    /// When true, an Admin role and optional default admin user can be created.
    /// </summary>
    public bool SeedAdminUserAndRole { get; set; } = false;

    /// <summary>
    /// Default admin email to create when admin seeding is enabled.
    /// </summary>
    public string? DefaultAdminEmail { get; set; }

    /// <summary>
    /// Default admin password to create when admin seeding is enabled.
    /// Must be strong and should come from secure config in real environments.
    /// </summary>
    public string? DefaultAdminPassword { get; set; }
}
