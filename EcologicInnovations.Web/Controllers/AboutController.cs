using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.About;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Controllers;

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
            Breadcrumbs = BreadcrumbBuilder.CreateForAbout(),
            Seo = seo
        };

        ViewData.SetSeoMeta(model.Seo);

        return View(model);
    }
}
