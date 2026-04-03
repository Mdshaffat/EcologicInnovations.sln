using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EcologicInnovations.Web.Services;
using EcologicInnovations.Web.Services.Interfaces;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Registers shared application services used by public and admin features.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEcologicInnovationsSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        // Memory cache required by SiteSettingsService
        services.AddMemoryCache();

        services.Configure<MediaOptions>(configuration.GetSection("Media"));

        services.AddScoped<ISlugService, SlugService>();
        services.AddScoped<IHtmlSanitizationService, HtmlSanitizationService>();
        services.AddScoped<ISeoMetadataService, SeoMetadataService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<ICurrentPageSourceService, CurrentPageSourceService>();
        services.AddScoped<IProductMenuService, ProductMenuService>();

        // Schema markup generation service used for SEO structured data
        services.AddScoped<ISchemaMarkupService, SchemaMarkupService>();

        // Site settings service used by controllers and views
        services.AddScoped<ISiteSettingsService, SiteSettingsService>();

        // Canonical URL service used by controllers and views
        services.AddSingleton<ICanonicalUrlService, CanonicalUrlService>();

        // Sitemap service used to expose sitemap.xml
        services.AddScoped<ISitemapService, SitemapService>();
        // Robots service used to expose robots.txt
        services.AddScoped<IRobotsService, RobotsService>();

        return services;
    }
}
