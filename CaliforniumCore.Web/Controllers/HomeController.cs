using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.Helpers;
using CaliforniumCore.Web.Models.Enums;
using CaliforniumCore.Web.Services.Interfaces;
using CaliforniumCore.Web.ViewModels.Blog;
using CaliforniumCore.Web.ViewModels.Home;
using CaliforniumCore.Web.ViewModels.Products;
using CaliforniumCore.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Controllers;

/// <summary>
/// Public Home controller.
/// Loads DB-driven content for the landing page and falls back gracefully
/// when the site is still empty or only partially seeded.
/// </summary>
public class HomeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly ISeoMetadataService _seoMetadataService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public HomeController(
        ApplicationDbContext dbContext,
        ISiteSettingsService siteSettingsService,
        ISeoMetadataService seoMetadataService,
        IHtmlSanitizationService htmlSanitizationService)
    {
        _dbContext = dbContext;
        _siteSettingsService = siteSettingsService;
        _seoMetadataService = seoMetadataService;
        _htmlSanitizationService = htmlSanitizationService;
    }

    /// <summary>
    /// Public Home page.
    /// Uses real DB content from SiteSetting, About Us, Product, and BlogPost tables.
    /// </summary>
    [HttpGet("/")]
    [HttpGet("/home")]
    [HttpGet("/home/index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);

        var aboutPage = await _dbContext.SitePages
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.PageKey == SitePageKey.AboutUs && x.IsPublished,
                cancellationToken);

        var featuredProducts = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .Where(x => x.IsPublished && x.IsActive)
            .OrderByDescending(x => x.IsFeatured)
            .ThenBy(x => x.ListSortOrder)
            .ThenBy(x => x.Title)
            .Take(6)
            .Select(x => new ProductCardViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.ProductCategory.Name,
                CategorySlug = x.ProductCategory.Slug,
                MainImageUrl = x.MainImageUrl,
                ShortDescription = x.ShortDescription,
                IsFeatured = x.IsFeatured,
                DetailsUrl = Url.Action("Details", "Products", new { slug = x.Slug })
            })
            .ToListAsync(cancellationToken);

        var latestBlogs = await _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .Where(x => x.IsPublished)
            .OrderByDescending(x => x.PublishedAt ?? x.CreatedAt)
            .Take(3)
            .Select(x => new BlogCardViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.BlogCategory != null ? x.BlogCategory.Name : null,
                CategorySlug = x.BlogCategory != null ? x.BlogCategory.Slug : null,
                FeatureImageUrl = x.FeatureImageUrl,
                Excerpt = x.Excerpt,
                IsFeatured = x.IsFeatured,
                PublishedAt = x.PublishedAt,
                DetailsUrl = Url.Action("Details", "Blog", new { slug = x.Slug })
            })
            .ToListAsync(cancellationToken);

        var seo = await _seoMetadataService.BuildDefaultAsync(
            title: siteSettings.MetaTitleDefault ?? "Californium Core | Scientific Software, Medical Systems & Materials Technology",
            description: siteSettings.MetaDescriptionDefault ??
                         "Californium Core builds software, medical technology, chemical systems, and materials-focused digital tools for teams that need reliable results.",
            canonicalPath: "/",
            ogImageUrl: siteSettings.LogoUrl,
            cancellationToken: cancellationToken);

        var model = new HomePageViewModel
        {
            Hero = new HomeHeroViewModel
            {
                Title = "Building Core Technologies for a Better Tomorrow",
                Subtitle = "Californium Core delivers advanced solutions in engineering, technology, and innovation to power a smarter, sustainable future.",
                ImageUrl = null,
                PrimaryButtonText = "Explore Our Solutions",
                PrimaryButtonUrl = Url.Action("Index", "Products"),
                SecondaryButtonText = "Contact Us",
                SecondaryButtonUrl = Url.Action("Index", "Contact")
            },
            AboutTitle = aboutPage?.Title ?? "About Californium Core",
            AboutSummary = aboutPage?.ShortIntro ??
                           "We combine engineering discipline with domain sensitivity, delivering tools that support research, operations, clinical workflows, and industrial growth.",
            AboutImageUrl = aboutPage?.BannerImageUrl,
            FeaturedProducts = featuredProducts,
            LatestBlogs = latestBlogs,
            HomeValueKicker = string.IsNullOrWhiteSpace(siteSettings.HomeValueKicker)
                ? "Why Choose Us"
                : siteSettings.HomeValueKicker,
            HomeValueTitle = string.IsNullOrWhiteSpace(siteSettings.HomeValueTitle)
                ? "Built with scientific discipline and practical delivery"
                : siteSettings.HomeValueTitle,
            HomeValueIntro = string.IsNullOrWhiteSpace(siteSettings.HomeValueIntro)
                ? "We bring engineering depth, structured thinking, and careful implementation to domains where accuracy and reliability matter."
                : siteSettings.HomeValueIntro,
            ValuePoints = BuildHomeValuePoints(siteSettings),
            CtaTitle = "Ready to build a system around complex work?",
            CtaText = "Tell us what you are trying to measure, manage, automate, or scale. We will help shape the right technical path.",
            CtaButtonText = "Send an Inquiry",
            CtaButtonUrl = Url.Action("Index", "Contact"),
            SiteName = siteSettings.CompanyName ?? "Californium Core",
            LogoUrl = siteSettings.LogoUrl,
            SiteTagline = siteSettings.Tagline,
            SupportEmail = siteSettings.SupportEmail,
            SalesEmail = siteSettings.SalesEmail,
            Phone = siteSettings.Phone,
            FooterHtml = _htmlSanitizationService.SanitizeRichHtml(siteSettings.FooterHtml),
            Seo = seo
        };

        if (!model.FeaturedProducts.Any() && !model.LatestBlogs.Any())
        {
            model.EmptyState = new EmptyStateViewModel
            {
                Title = "The website is ready for content",
                Message = "Your home page structure is working. Add products, articles, and page content from the admin panel to fully populate this landing page.",
                ButtonText = "Contact Us",
                ButtonUrl = Url.Action("Index", "Contact")
            };
        }

        ViewData.SetSeoMeta(model.Seo);

        return View(model);
    }

    private static List<HomeValuePointViewModel> BuildHomeValuePoints(CaliforniumCore.Web.Models.Entities.SiteSetting siteSettings)
    {
        return
        [
            BuildValuePoint(
                siteSettings.HomeValue1IconCssClass,
                siteSettings.HomeValue1Title,
                siteSettings.HomeValue1Description,
                "bi bi-code-slash",
                "Scientific Software",
                "Purpose-built web, desktop, and workflow tools for teams that need dependable data, reporting, and process control."),
            BuildValuePoint(
                siteSettings.HomeValue2IconCssClass,
                siteSettings.HomeValue2Title,
                siteSettings.HomeValue2Description,
                "bi bi-heart-pulse",
                "Medical & Laboratory Systems",
                "Secure, practical systems for medical operations, lab coordination, sample tracking, and decision support."),
            BuildValuePoint(
                siteSettings.HomeValue3IconCssClass,
                siteSettings.HomeValue3Title,
                siteSettings.HomeValue3Description,
                "bi bi-droplet-half",
                "Chemical & Materials Intelligence",
                "Digital products for chemical workflows, material records, quality signals, and experimental knowledge capture."),
            BuildValuePoint(
                siteSettings.HomeValue4IconCssClass,
                siteSettings.HomeValue4Title,
                siteSettings.HomeValue4Description,
                "bi bi-cpu",
                "Systems & Adoption",
                "Implementation support that helps teams adopt new systems, strengthen technical capability, and keep work moving.")
        ];
    }

    private static HomeValuePointViewModel BuildValuePoint(
        string? configuredIcon,
        string? configuredTitle,
        string? configuredDescription,
        string defaultIcon,
        string defaultTitle,
        string defaultDescription)
    {
        return new HomeValuePointViewModel
        {
            IconCssClass = string.IsNullOrWhiteSpace(configuredIcon) ? defaultIcon : configuredIcon.Trim(),
            Title = string.IsNullOrWhiteSpace(configuredTitle) ? defaultTitle : configuredTitle.Trim(),
            Description = string.IsNullOrWhiteSpace(configuredDescription)
                ? defaultDescription
                : configuredDescription.Trim()
        };
    }
}
