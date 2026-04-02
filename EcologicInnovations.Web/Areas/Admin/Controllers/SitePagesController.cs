using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Manages singleton-like site pages such as About Us and Policy.
/// </summary>
public class SitePagesController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISlugService _slugService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public SitePagesController(
        ApplicationDbContext dbContext,
        ISlugService slugService,
        IHtmlSanitizationService htmlSanitizationService)
    {
        _dbContext = dbContext;
        _slugService = slugService;
        _htmlSanitizationService = htmlSanitizationService;
    }

    [HttpGet]
    public async Task<IActionResult> EditAboutUs(CancellationToken cancellationToken)
    {
        var page = await GetOrCreateSystemPageAsync(SitePageKey.AboutUs, cancellationToken);

        var model = new SitePageEditorViewModel
        {
            Id = page.Id,
            PageKey = page.PageKey,
            Title = page.Title,
            Slug = page.Slug,
            BannerImageUrl = page.BannerImageUrl,
            ShortIntro = page.ShortIntro,
            HtmlContent = page.HtmlContent,
            MetaTitle = page.MetaTitle,
            MetaDescription = page.MetaDescription,
            IsPublished = page.IsPublished,
            SortOrder = page.SortOrder,
            Seo = new SeoMetaViewModel
            {
                Title = page.MetaTitle ?? page.Title,
                Description = page.MetaDescription
            }
        };

        ViewData["AdminPageTitle"] = "About Us";
        ViewData["AdminPageDescription"] = "Edit the public About Us page content and metadata.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("About Us");

        return View("EditAboutUs", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAboutUs(SitePageEditorViewModel model, CancellationToken cancellationToken)
    {
        model.PageKey = SitePageKey.AboutUs;

        if (!ModelState.IsValid)
        {
            ViewData["AdminPageTitle"] = "About Us";
            ViewData["AdminPageDescription"] = "Edit the public About Us page content and metadata.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("About Us");
            return View("EditAboutUs", model);
        }

        var page = await GetOrCreateSystemPageAsync(SitePageKey.AboutUs, cancellationToken);

        page.Title = model.Title.Trim();
        page.Slug = await _slugService.GenerateUniqueSitePageSlugAsync(
            string.IsNullOrWhiteSpace(model.Slug) ? "about-us" : model.Slug,
            page.Id > 0 ? page.Id : null,
            cancellationToken);

        page.BannerImageUrl = string.IsNullOrWhiteSpace(model.BannerImageUrl) ? null : model.BannerImageUrl.Trim();
        page.ShortIntro = string.IsNullOrWhiteSpace(model.ShortIntro) ? null : model.ShortIntro.Trim();
        page.HtmlContent = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContent);
        page.MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? null : model.MetaTitle.Trim();
        page.MetaDescription = string.IsNullOrWhiteSpace(model.MetaDescription) ? null : model.MetaDescription.Trim();
        page.IsPublished = model.IsPublished;
        page.SortOrder = model.SortOrder;

        if (page.Id == 0)
        {
            _dbContext.SitePages.Add(page);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "About Us page updated successfully.";
        TempData["AdminToastSuccess"] = "About Us saved.";

        return RedirectToAction(nameof(EditAboutUs));
    }

    [HttpGet]
    public async Task<IActionResult> EditPolicy(CancellationToken cancellationToken)
    {
        var page = await GetOrCreateSystemPageAsync(SitePageKey.Policy, cancellationToken);

        var model = new SitePageEditorViewModel
        {
            Id = page.Id,
            PageKey = page.PageKey,
            Title = page.Title,
            Slug = string.IsNullOrWhiteSpace(page.Slug) ? "policy" : page.Slug,
            BannerImageUrl = page.BannerImageUrl,
            ShortIntro = page.ShortIntro,
            HtmlContent = page.HtmlContent,
            MetaTitle = page.MetaTitle,
            MetaDescription = page.MetaDescription,
            IsPublished = page.IsPublished,
            SortOrder = page.SortOrder,
            Seo = new SeoMetaViewModel
            {
                Title = page.MetaTitle ?? page.Title,
                Description = page.MetaDescription
            }
        };

        ViewData["AdminPageTitle"] = "Policy";
        ViewData["AdminPageDescription"] = "Edit the public Policy page content and metadata.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Policy");

        return View("EditPolicy", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPolicy(SitePageEditorViewModel model, CancellationToken cancellationToken)
    {
        model.PageKey = SitePageKey.Policy;

        // Keep the Policy route stable.
        model.Slug = "policy";

        if (!ModelState.IsValid)
        {
            ViewData["AdminPageTitle"] = "Policy";
            ViewData["AdminPageDescription"] = "Edit the public Policy page content and metadata.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Policy");
            return View("EditPolicy", model);
        }

        var page = await GetOrCreateSystemPageAsync(SitePageKey.Policy, cancellationToken);

        page.Title = model.Title.Trim();
        page.PageKey = SitePageKey.Policy;
        page.Slug = "policy";
        page.BannerImageUrl = string.IsNullOrWhiteSpace(model.BannerImageUrl) ? null : model.BannerImageUrl.Trim();
        page.ShortIntro = string.IsNullOrWhiteSpace(model.ShortIntro) ? null : model.ShortIntro.Trim();
        page.HtmlContent = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContent);
        page.MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? null : model.MetaTitle.Trim();
        page.MetaDescription = string.IsNullOrWhiteSpace(model.MetaDescription) ? null : model.MetaDescription.Trim();
        page.IsPublished = model.IsPublished;
        page.SortOrder = model.SortOrder;

        if (page.Id == 0)
        {
            _dbContext.SitePages.Add(page);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Policy page updated successfully.";
        TempData["AdminToastSuccess"] = "Policy saved.";

        return RedirectToAction(nameof(EditPolicy));
    }

    private async Task<SitePage> GetOrCreateSystemPageAsync(SitePageKey pageKey, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.SitePages
            .FirstOrDefaultAsync(x => x.PageKey == pageKey, cancellationToken);

        if (existing is not null)
        {
            return existing;
        }

        return new SitePage
        {
            PageKey = pageKey,
            Title = pageKey == SitePageKey.AboutUs ? "About Us" : "Policy",
            Slug = pageKey == SitePageKey.AboutUs ? "about-us" : "policy",
            IsPublished = false,
            SortOrder = pageKey == SitePageKey.AboutUs ? 1 : 2
        };
    }

    private static List<BreadcrumbItemViewModel> BuildAdminBreadcrumbs(string currentTitle)
    {
        return
        [
            new BreadcrumbItemViewModel
            {
                Title = "Admin",
                Url = "/Admin/Dashboard",
                IsActive = false
            },
            new BreadcrumbItemViewModel
            {
                Title = currentTitle,
                Url = null,
                IsActive = true
            }
        ];
    }
}
