using CaliforniumCore.Web.Configuration;
using CaliforniumCore.Web.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using CaliforniumCore.Web.Services;

namespace CaliforniumCore.Web.Services;

/// <summary>
/// Registers shared application services used by public and admin features.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaliforniumCoreSharedServices(this IServiceCollection services, IConfiguration configuration)
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
        services.AddScoped<IContactRateLimitService, ContactRateLimitService>();
        services.AddScoped<IVideoEmbedService, VideoEmbedService>();

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
