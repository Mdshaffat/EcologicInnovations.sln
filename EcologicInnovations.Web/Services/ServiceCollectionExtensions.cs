using EcologicInnovations.Web.Configuration;
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

        services.Configure<MediaOptions>(configuration.GetSection("Media"));

        services.AddScoped<ISlugService, SlugService>();
        services.AddScoped<IHtmlSanitizationService, HtmlSanitizationService>();
        services.AddScoped<ISeoMetadataService, SeoMetadataService>();
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<ICurrentPageSourceService, CurrentPageSourceService>();
        services.AddScoped<IProductMenuService, ProductMenuService>();

        return services;
    }
}
