using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.Models.Entities;
using CaliforniumCore.Web.Services.Interfaces;
using CaliforniumCore.Web.ViewModels.Admin;
using CaliforniumCore.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CaliforniumCore.Web.Areas.Admin.Controllers;

public class SiteSettingsController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public SiteSettingsController(
        ApplicationDbContext dbContext,
        ISiteSettingsService siteSettingsService,
        IHtmlSanitizationService htmlSanitizationService)
    {
        _dbContext = dbContext;
        _siteSettingsService = siteSettingsService;
        _htmlSanitizationService = htmlSanitizationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var settings = await GetOrCreateSettingsAsync(cancellationToken);
        var model = BuildModel(settings);

        SetPageChrome();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SiteSettingsEditViewModel model, CancellationToken cancellationToken)
    {
        SetPageChrome();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var googleMapEmbedUrl = NormalizeMapEmbedUrl(model.GoogleMapEmbedUrl);
        if (!string.IsNullOrWhiteSpace(model.GoogleMapEmbedUrl) && googleMapEmbedUrl is null)
        {
            ModelState.AddModelError(nameof(model.GoogleMapEmbedUrl), "Paste a valid Google Maps URL or iframe embed code.");
            return View(model);
        }

        var settings = await GetOrCreateSettingsAsync(cancellationToken);

        settings.CompanyName = model.CompanyName.Trim();
        settings.Tagline = Normalize(model.Tagline);
        settings.LogoUrl = Normalize(model.LogoUrl);
        settings.FaviconUrl = Normalize(model.FaviconUrl);
        settings.SupportEmail = Normalize(model.SupportEmail);
        settings.SalesEmail = Normalize(model.SalesEmail);
        settings.Phone = Normalize(model.Phone);
        settings.Address = Normalize(model.Address);
        settings.FooterHtml = _htmlSanitizationService.SanitizeFooterHtml(model.FooterHtml);
        settings.FacebookUrl = Normalize(model.FacebookUrl);
        settings.LinkedInUrl = Normalize(model.LinkedInUrl);
        settings.YouTubeUrl = Normalize(model.YouTubeUrl);
        settings.MetaTitleDefault = Normalize(model.MetaTitleDefault);
        settings.MetaDescriptionDefault = Normalize(model.MetaDescriptionDefault);
        settings.GoogleMapEmbedUrl = googleMapEmbedUrl;
        settings.HomeValueKicker = Normalize(model.HomeValueKicker);
        settings.HomeValueTitle = Normalize(model.HomeValueTitle);
        settings.HomeValueIntro = Normalize(model.HomeValueIntro);
        settings.HomeValue1IconCssClass = Normalize(model.HomeValue1IconCssClass);
        settings.HomeValue1Title = Normalize(model.HomeValue1Title);
        settings.HomeValue1Description = Normalize(model.HomeValue1Description);
        settings.HomeValue2IconCssClass = Normalize(model.HomeValue2IconCssClass);
        settings.HomeValue2Title = Normalize(model.HomeValue2Title);
        settings.HomeValue2Description = Normalize(model.HomeValue2Description);
        settings.HomeValue3IconCssClass = Normalize(model.HomeValue3IconCssClass);
        settings.HomeValue3Title = Normalize(model.HomeValue3Title);
        settings.HomeValue3Description = Normalize(model.HomeValue3Description);
        settings.HomeValue4IconCssClass = Normalize(model.HomeValue4IconCssClass);
        settings.HomeValue4Title = Normalize(model.HomeValue4Title);
        settings.HomeValue4Description = Normalize(model.HomeValue4Description);

        if (settings.Id == 0)
        {
            _dbContext.SiteSettings.Add(settings);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        _siteSettingsService.ClearCache();

        TempData["AdminSuccessMessage"] = "Site settings updated successfully.";
        TempData["AdminToastSuccess"] = "Site settings saved.";

        return RedirectToAction(nameof(Index), new { area = "Admin" });
    }

    private async Task<SiteSetting> GetOrCreateSettingsAsync(CancellationToken cancellationToken)
    {
        var settings = await _dbContext.SiteSettings
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return settings ?? new SiteSetting
        {
            CompanyName = "Californium Core",
            LogoUrl = "/uploads/californium-core-logo-atom.svg",
            FaviconUrl = "/uploads/californium-core-logo-atom.svg",
            FooterHtml = $"<p>&copy; {DateTime.UtcNow.Year} Californium Core. All rights reserved.</p>"
        };
    }

    private static SiteSettingsEditViewModel BuildModel(SiteSetting settings)
    {
        return new SiteSettingsEditViewModel
        {
            Id = settings.Id,
            CompanyName = settings.CompanyName,
            Tagline = settings.Tagline,
            LogoUrl = settings.LogoUrl,
            FaviconUrl = settings.FaviconUrl,
            SupportEmail = settings.SupportEmail,
            SalesEmail = settings.SalesEmail,
            Phone = settings.Phone,
            Address = settings.Address,
            FooterHtml = settings.FooterHtml,
            FacebookUrl = settings.FacebookUrl,
            LinkedInUrl = settings.LinkedInUrl,
            YouTubeUrl = settings.YouTubeUrl,
            MetaTitleDefault = settings.MetaTitleDefault,
            MetaDescriptionDefault = settings.MetaDescriptionDefault,
            GoogleMapEmbedUrl = settings.GoogleMapEmbedUrl,
            HomeValueKicker = settings.HomeValueKicker,
            HomeValueTitle = settings.HomeValueTitle,
            HomeValueIntro = settings.HomeValueIntro,
            HomeValue1IconCssClass = settings.HomeValue1IconCssClass,
            HomeValue1Title = settings.HomeValue1Title,
            HomeValue1Description = settings.HomeValue1Description,
            HomeValue2IconCssClass = settings.HomeValue2IconCssClass,
            HomeValue2Title = settings.HomeValue2Title,
            HomeValue2Description = settings.HomeValue2Description,
            HomeValue3IconCssClass = settings.HomeValue3IconCssClass,
            HomeValue3Title = settings.HomeValue3Title,
            HomeValue3Description = settings.HomeValue3Description,
            HomeValue4IconCssClass = settings.HomeValue4IconCssClass,
            HomeValue4Title = settings.HomeValue4Title,
            HomeValue4Description = settings.HomeValue4Description
        };
    }

    private void SetPageChrome()
    {
        ViewData["AdminPageTitle"] = "Site Settings";
        ViewData["AdminPageDescription"] = "Manage branding, contact details, home value cards, map, and SEO defaults.";
        ViewData["AdminBreadcrumbs"] = new List<BreadcrumbItemViewModel>
        {
            new() { Title = "Admin", Url = "/Admin/Dashboard", IsActive = false },
            new() { Title = "Site Settings", Url = null, IsActive = true }
        };
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string? NormalizeMapEmbedUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        var iframeSrc = Regex.Match(
            trimmed,
            "src\\s*=\\s*[\"'](?<url>[^\"']+)[\"']",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        if (iframeSrc.Success)
        {
            trimmed = System.Net.WebUtility.HtmlDecode(iframeSrc.Groups["url"].Value);
        }

        return Uri.TryCreate(trimmed, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
            ? trimmed
            : null;
    }
}
