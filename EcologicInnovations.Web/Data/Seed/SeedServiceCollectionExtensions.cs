using EcologicInnovations.Web.Configuration;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Data.Seed;

/// <summary>
/// Registration helpers for the startup seeding system.
/// </summary>
public static class SeedServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationSeeding(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SeedOptions>(configuration.GetSection("Seed"));
        services.AddScoped<ApplicationDbInitializer>();

        return services;
    }

    public static async Task InitializeApplicationDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>();
        await initializer.InitializeAsync();
    }
}
