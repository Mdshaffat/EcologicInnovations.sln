using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CaliforniumCore.Web.Services.Interfaces;
using CaliforniumCore.Web.Configuration;

namespace CaliforniumCore.Web.Services
{
    public static class SeoServiceCollectionExtensions
    {
        public static IServiceCollection AddCaliforniumCoreSeoInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Register SEO related services here
            services.AddSingleton<ICanonicalUrlService, CanonicalUrlService>();
            services.Configure<SeoOptions>(configuration.GetSection("Seo"));
            services.AddHttpContextAccessor();


            return services;
        }
    }
}