using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Data.Seed;

/// <summary>
/// Coordinates database migration and startup seeding in a safe, idempotent way.
/// </summary>
public class ApplicationDbInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationDbInitializer> _logger;
    private readonly SeedOptions _seedOptions;

    public ApplicationDbInitializer(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider,
        IOptions<SeedOptions> seedOptions,
        ILogger<ApplicationDbInitializer> logger)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _seedOptions = seedOptions.Value;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_seedOptions.ApplyMigrationsOnStartup)
        {
            _logger.LogInformation("Applying pending database migrations on startup.");
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }

        if (_seedOptions.SeedCoreContent)
        {
            _logger.LogInformation("Seeding core site content.");
            await DefaultSiteContentSeeder.SeedAsync(_dbContext, cancellationToken);
        }

        if (_seedOptions.SeedCategories || _seedOptions.SeedSampleCatalogContent)
        {
            _logger.LogInformation("Seeding catalog structure and optional sample content.");
            await DefaultCatalogSeeder.SeedAsync(
                _dbContext,
                seedCategories: _seedOptions.SeedCategories,
                seedSampleCatalogContent: _seedOptions.SeedSampleCatalogContent,
                cancellationToken: cancellationToken);
        }

        if (_seedOptions.SeedAdminUserAndRole)
        {
            _logger.LogInformation("Seeding admin role and optional default admin user.");
            using var scope = _serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await DefaultAdminRoleSeeder.SeedAsync(
                roleManager,
                userManager,
                _seedOptions,
                cancellationToken);
        }
    }
}
