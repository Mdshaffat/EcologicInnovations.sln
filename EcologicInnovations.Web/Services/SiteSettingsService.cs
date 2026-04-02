using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Loads and caches the single active SiteSetting record.
/// This avoids repeated DB reads from the layout, contact page, footer, and SEO services.
/// </summary>
public class SiteSettingsService : ISiteSettingsService
{
    private const string CacheKey = "site-settings-primary";

    private readonly ApplicationDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    private readonly SeoOptions _seoOptions;

    public SiteSettingsService(
        ApplicationDbContext dbContext,
        IMemoryCache memoryCache,
        IOptions<SeoOptions> seoOptions)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
        _seoOptions = seoOptions.Value;
    }

    public async Task<SiteSetting?> GetPrimaryAsync(CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(CacheKey, out SiteSetting? cached))
        {
            return cached;
        }

        var record = await _dbContext.SiteSettings
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        _memoryCache.Set(CacheKey, record, TimeSpan.FromMinutes(15));
        return record;
    }

    public async Task<SiteSetting> GetPrimaryOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        var record = await GetPrimaryAsync(cancellationToken);

        if (record is not null)
        {
            return record;
        }

        return new SiteSetting
        {
            CompanyName = _seoOptions.OrganizationName,
            MetaTitleDefault = _seoOptions.DefaultTitle,
            MetaDescriptionDefault = _seoOptions.DefaultDescription,
            LogoUrl = _seoOptions.DefaultOgImage,
            FooterHtml = $"<p>&copy; {DateTime.UtcNow.Year} {_seoOptions.OrganizationName}. All rights reserved.</p>"
        };
    }

    public void ClearCache()
    {
        _memoryCache.Remove(CacheKey);
    }
}
