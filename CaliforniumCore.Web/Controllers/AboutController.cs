using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.Helpers;
using CaliforniumCore.Web.Models.Enums;
using CaliforniumCore.Web.Services.Interfaces;
using CaliforniumCore.Web.ViewModels.About;
using CaliforniumCore.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Controllers;

/// <summary>
/// Public controller for the About Us page.
/// The route stays clean and SEO-friendly, while the content itself is loaded from the SitePages table.
/// </summary>
[Route("about-us")]
public class AboutController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHtmlSanitizationService _htmlSanitizationService;
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly ICanonicalUrlService _canonicalUrlService;

    public AboutController(
        ApplicationDbContext dbContext,
        IHtmlSanitizationService htmlSanitizationService,
        ISiteSettingsService siteSettingsService,
        ICanonicalUrlService canonicalUrlService)
    {
        _dbContext = dbContext;
        _htmlSanitizationService = htmlSanitizationService;
        _siteSettingsService = siteSettingsService;
        _canonicalUrlService = canonicalUrlService;
    }

    /// <summary>
    /// Renders the public About Us page from the SitePages table.
    /// Only the published AboutUs record is shown publicly.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var page = await _dbContext.SitePages
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.PageKey == SitePageKey.AboutUs && x.IsPublished,
                cancellationToken);

        if (page is null)
        {
            return NotFound();
        }

        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);

        var seo = new SeoMetaViewModel
        {
            Title = !string.IsNullOrWhiteSpace(page.MetaTitle)
                ? page.MetaTitle
                : $"{page.Title} | {siteSettings.CompanyName}",
            Description = !string.IsNullOrWhiteSpace(page.MetaDescription)
                ? page.MetaDescription
                : !string.IsNullOrWhiteSpace(page.ShortIntro)
                    ? page.ShortIntro
                    : siteSettings.MetaDescriptionDefault,
            CanonicalUrl = _canonicalUrlService.BuildCanonicalUrl("/about-us"),
            OgTitle = !string.IsNullOrWhiteSpace(page.MetaTitle)
                ? page.MetaTitle
                : page.Title,
            OgDescription = !string.IsNullOrWhiteSpace(page.MetaDescription)
                ? page.MetaDescription
                : page.ShortIntro ?? siteSettings.MetaDescriptionDefault,
            OgImageUrl = !string.IsNullOrWhiteSpace(page.BannerImageUrl)
                ? page.BannerImageUrl
                : siteSettings.LogoUrl,
            Robots = "index,follow"
        };

        var model = new AboutPageViewModel
        {
            Title = page.Title,
            Slug = page.Slug,
            BannerImageUrl = page.BannerImageUrl,
            ShortIntro = page.ShortIntro,
            HtmlContent = _htmlSanitizationService.SanitizeRichHtml(page.HtmlContent),
            GoogleMapEmbedUrl = GetSafeMapUrl(siteSettings.GoogleMapEmbedUrl),
            Breadcrumbs = BreadcrumbBuilder.CreateForAbout(),
            Seo = seo
        };

        ViewData.SetSeoMeta(model.Seo);

        return View(model);
    }

    private static string? GetSafeMapUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        return Uri.TryCreate(trimmed, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
            ? trimmed
            : null;
    }
}
