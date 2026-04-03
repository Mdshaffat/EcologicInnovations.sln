using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.Configuration;

namespace EcologicInnovations.Web.Services
{
    public static class SeoServiceCollectionExtensions
    {
        public static IServiceCollection AddEcologicInnovationsSeoInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Register SEO related services here
            services.AddSingleton<ICanonicalUrlService, CanonicalUrlService>();
            services.Configure<SeoOptions>(configuration.GetSection("Seo"));
            services.AddHttpContextAccessor();


            return services;
        }
    }
}