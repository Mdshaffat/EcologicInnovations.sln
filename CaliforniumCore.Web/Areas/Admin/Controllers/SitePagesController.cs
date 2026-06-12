using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.Models.Entities;
using CaliforniumCore.Web.Models.Enums;
using CaliforniumCore.Web.Services.Interfaces;
using CaliforniumCore.Web.ViewModels.Admin;
using CaliforniumCore.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Areas.Admin.Controllers;

/// <summary>
/// Manages singleton-like site pages such as About Us, Policy, and Contact.
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
    public IActionResult Index()
    {
        ViewData["AdminPageTitle"] = "Site Pages";
        ViewData["AdminPageDescription"] = "Edit singleton site pages such as About Us, Policy, and Contact.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Site Pages");

        return View();
    }


    public async Task<IActionResult> EditAboutUs(CancellationToken cancellationToken)
    {
        var page = await GetOrCreateSystemPageAsync(SitePageKey.AboutUs, cancellationToken);

        var model = BuildSitePageEditorViewModel(page);

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
            // Collect validation errors for easier debugging in the UI/alerts
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();

            if (errors.Any())
            {
                TempData["AdminErrorMessage"] = string.Join("<br/>", errors);
            }

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

        return RedirectToAction(nameof(EditAboutUs), new { area = "Admin" });
    }

    [HttpGet]
    public async Task<IActionResult> EditPolicy(CancellationToken cancellationToken)
    {
        var page = await GetOrCreateSystemPageAsync(SitePageKey.Policy, cancellationToken);

        var model = BuildSitePageEditorViewModel(page);

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
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();

            if (errors.Any())
            {
                TempData["AdminErrorMessage"] = string.Join("<br/>", errors);
            }

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

        return RedirectToAction(nameof(EditPolicy), new { area = "Admin" });
    }

    [HttpGet]
    public async Task<IActionResult> EditContact(CancellationToken cancellationToken)
    {
        var page = await GetOrCreateSystemPageAsync(SitePageKey.Contact, cancellationToken);
        var model = BuildSitePageEditorViewModel(page);

        ViewData["AdminPageTitle"] = "Contact";
        ViewData["AdminPageDescription"] = "Edit the public Contact page content and metadata.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Contact");

        return View("EditContact", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditContact(SitePageEditorViewModel model, CancellationToken cancellationToken)
    {
        model.PageKey = SitePageKey.Contact;
        model.Slug = "contact";

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToList();

            if (errors.Any())
            {
                TempData["AdminErrorMessage"] = string.Join("<br/>", errors);
            }

            model.PreviewHtml = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContent);
            ViewData["AdminPageTitle"] = "Contact";
            ViewData["AdminPageDescription"] = "Edit the public Contact page content and metadata.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Contact");
            return View("EditContact", model);
        }

        var page = await GetOrCreateSystemPageAsync(SitePageKey.Contact, cancellationToken);

        page.Title = model.Title.Trim();
        page.PageKey = SitePageKey.Contact;
        page.Slug = "contact";
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

        TempData["AdminSuccessMessage"] = "Contact page updated successfully.";
        TempData["AdminToastSuccess"] = "Contact saved.";

        return RedirectToAction(nameof(EditContact), new { area = "Admin" });
    }

    private SitePageEditorViewModel BuildSitePageEditorViewModel(SitePage page)
    {
        return new SitePageEditorViewModel
        {
            Id = page.Id,
            PageKey = page.PageKey,
            Title = page.Title,
            Slug = string.IsNullOrWhiteSpace(page.Slug) ? GetDefaultSlug(page.PageKey) : page.Slug,
            BannerImageUrl = page.BannerImageUrl,
            ShortIntro = page.ShortIntro,
            HtmlContent = page.HtmlContent,
            MetaTitle = page.MetaTitle,
            MetaDescription = page.MetaDescription,
            IsPublished = page.IsPublished,
            SortOrder = page.SortOrder,
            UpdatedAt = page.UpdatedAt,
            PreviewHtml = _htmlSanitizationService.SanitizeRichHtml(page.HtmlContent),
            Seo = new SeoMetaViewModel
            {
                Title = page.MetaTitle ?? page.Title,
                Description = page.MetaDescription
            }
        };
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
            Title = GetDefaultTitle(pageKey),
            Slug = GetDefaultSlug(pageKey),
            IsPublished = false,
            SortOrder = GetDefaultSortOrder(pageKey)
        };
    }

    private static string GetDefaultTitle(SitePageKey pageKey)
    {
        return pageKey switch
        {
            SitePageKey.AboutUs => "About Us",
            SitePageKey.Policy => "Policy",
            SitePageKey.Contact => "Contact Us",
            _ => pageKey.ToString()
        };
    }

    private static string GetDefaultSlug(SitePageKey pageKey)
    {
        return pageKey switch
        {
            SitePageKey.AboutUs => "about-us",
            SitePageKey.Policy => "policy",
            SitePageKey.Contact => "contact",
            _ => pageKey.ToString().ToLowerInvariant()
        };
    }

    private static int GetDefaultSortOrder(SitePageKey pageKey)
    {
        return pageKey switch
        {
            SitePageKey.AboutUs => 1,
            SitePageKey.Policy => 2,
            SitePageKey.Contact => 3,
            _ => 99
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
