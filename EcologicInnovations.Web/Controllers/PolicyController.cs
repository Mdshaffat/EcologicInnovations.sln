using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Controllers;

/// <summary>
/// Public controller responsible for the Policy page.
/// </summary>
public class PolicyController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHtmlSanitizationService _htmlSanitizationService;
    private readonly ISeoMetadataService _seoMetadataService;
    private readonly ISchemaMarkupService _schemaMarkupService;

    public PolicyController(
        ApplicationDbContext dbContext,
        IHtmlSanitizationService htmlSanitizationService,
        ISeoMetadataService seoMetadataService,
        ISchemaMarkupService schemaMarkupService)
    {
        _dbContext = dbContext;
        _htmlSanitizationService = htmlSanitizationService;
        _seoMetadataService = seoMetadataService;
        _schemaMarkupService = schemaMarkupService;
    }

    [HttpGet("policy")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var page = await _dbContext.SitePages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PageKey == SitePageKey.Policy && x.IsPublished, cancellationToken);

        if (page is null)
        {
            return NotFound();
        }

        var sanitizedHtml = _htmlSanitizationService.SanitizeRichHtml(page.HtmlContent);

        var model = new PolicyPageViewModel
        {
            Id = page.Id,
            Title = page.Title,
            BannerImageUrl = page.BannerImageUrl,
            ShortIntro = page.ShortIntro,
            HtmlContent = sanitizedHtml,
            UpdatedAt = page.UpdatedAt ?? page.CreatedAt,
            UpdatedText = $"Updated on {(page.UpdatedAt ?? page.CreatedAt):dd MMMM yyyy}",
            Breadcrumbs = BreadcrumbBuilder.CreateForPolicy()
        };

        var seo = await _seoMetadataService.BuildForSitePageAsync(page, "/policy", cancellationToken);
        model.Seo = seo;

        ViewData.SetSeoMeta(seo);

        var schema = await _schemaMarkupService.BuildForSitePageAsync(page, cancellationToken);
        ViewData.SetSchemaBlocks([schema]);

        return View(model);
    }
}
