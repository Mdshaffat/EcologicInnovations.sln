using CaliforniumCore.Web.Configuration;
using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.Models.Entities;
using CaliforniumCore.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CaliforniumCore.Web.Services;

/// <summary>
/// Loads and caches the single active SiteSetting record.
/// This avoids repeated DB reads from the layout, contact page, footer, and SEO services.
/// </summary>
public class SiteSettingsService : ISiteSettingsService
{
    private const string CacheKey = "site-settings-primary";
    private const string DefaultLogoUrl = "/uploads/californium-core-logo-atom.svg";
    private const string LegacyLogoUrl = "/uploads/logo.png";

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
            if (string.IsNullOrWhiteSpace(record.LogoUrl) ||
                string.Equals(record.LogoUrl, LegacyLogoUrl, StringComparison.OrdinalIgnoreCase))
            {
                record.LogoUrl = DefaultLogoUrl;
            }

            return record;
        }

        return new SiteSetting
        {
            CompanyName = _seoOptions.OrganizationName,
            MetaTitleDefault = _seoOptions.DefaultTitle,
            MetaDescriptionDefault = _seoOptions.DefaultDescription,
            LogoUrl = DefaultLogoUrl,
            FaviconUrl = DefaultLogoUrl,
            FooterHtml = $"<p>&copy; {DateTime.UtcNow.Year} {_seoOptions.OrganizationName}. All rights reserved.</p>",
            HomeValueKicker = "Why Choose Us",
            HomeValueTitle = "Built with scientific discipline and practical delivery",
            HomeValueIntro = "We bring engineering depth, structured thinking, and careful implementation to domains where accuracy and reliability matter.",
            HomeValue1IconCssClass = "bi bi-code-slash",
            HomeValue1Title = "Scientific Software",
            HomeValue1Description = "Purpose-built web, desktop, and workflow tools for teams that need dependable data, reporting, and process control.",
            HomeValue2IconCssClass = "bi bi-heart-pulse",
            HomeValue2Title = "Medical & Laboratory Systems",
            HomeValue2Description = "Secure, practical systems for medical operations, lab coordination, sample tracking, and decision support.",
            HomeValue3IconCssClass = "bi bi-droplet-half",
            HomeValue3Title = "Chemical & Materials Intelligence",
            HomeValue3Description = "Digital products for chemical workflows, material records, quality signals, and experimental knowledge capture.",
            HomeValue4IconCssClass = "bi bi-cpu",
            HomeValue4Title = "Systems & Adoption",
            HomeValue4Description = "Implementation support that helps teams adopt new systems, strengthen technical capability, and keep work moving."
        };
    }

    public void ClearCache()
    {
        _memoryCache.Remove(CacheKey);
    }
}
