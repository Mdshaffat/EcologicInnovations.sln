using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Blog;
using EcologicInnovations.Web.ViewModels.Home;
using EcologicInnovations.Web.ViewModels.Products;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Controllers;

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
            .Where(x => x.IsPublished && x.IsActive && x.ProductCategory.IsActive)
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
            title: siteSettings.MetaTitleDefault ?? "Ecologic Innovations | Software, Sustainability IoT, and Energy Equipment",
            description: siteSettings.MetaDescriptionDefault ??
                         "Ecologic Innovations delivers software solutions, sustainability IoT devices, energy equipment, and practical support for smarter business operations.",
            canonicalPath: "/",
            ogImageUrl: siteSettings.LogoUrl,
            cancellationToken: cancellationToken);

        var model = new HomePageViewModel
        {
            Hero = new HomeHeroViewModel
            {
                Title = "Software, Sustainability IoT, and Energy Equipment for Smarter Operations",
                Subtitle = siteSettings.Tagline ??
                           "Ecologic Innovations helps businesses digitize sustainability work, monitor utilities, manage efficiency, and deploy practical eco-technology solutions.",
                ImageUrl = siteSettings.LogoUrl,
                PrimaryButtonText = "Explore Products",
                PrimaryButtonUrl = Url.Action("Index", "Products"),
                SecondaryButtonText = "Contact Us",
                SecondaryButtonUrl = Url.Action("Index", "Contact")
            },
            AboutTitle = aboutPage?.Title ?? "About Ecologic Innovations",
            AboutSummary = aboutPage?.ShortIntro ??
                           "We build business-ready software, sustainability IoT devices, and energy-focused solutions that help organizations improve visibility, compliance, efficiency, and decision-making.",
            AboutImageUrl = aboutPage?.BannerImageUrl,
            FeaturedProducts = featuredProducts,
            LatestBlogs = latestBlogs,
            ValuePoints =
            [
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-laptop",
                    Title = "Business Software",
                    Description = "Practical digital platforms and custom web systems designed for operational control and growth."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-cpu",
                    Title = "Sustainability IoT Devices",
                    Description = "Smart monitoring tools and connected devices that improve visibility into sustainability and resource use."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-lightning-charge",
                    Title = "Energy Equipment",
                    Description = "Energy-focused products and supporting solutions for monitoring, efficiency improvement, and control."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-people",
                    Title = "Practical Support",
                    Description = "Business-ready guidance and implementation thinking built around real operational needs."
                }
            ],
            CtaTitle = "Ready to build smarter sustainability and energy solutions?",
            CtaText = "Talk to Ecologic Innovations about software, monitoring devices, energy equipment, or a tailored solution for your business.",
            CtaButtonText = "Send an Inquiry",
            CtaButtonUrl = Url.Action("Index", "Contact"),
            SiteName = siteSettings.CompanyName ?? "Ecologic Innovations",
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
                Message = "Your home page structure is working. Add products, blogs, and page content from the admin panel to fully populate this landing page.",
                ButtonText = "Contact Us",
                ButtonUrl = Url.Action("Index", "Contact")
            };
        }

        ViewData.SetSeoMeta(model.Seo);

        return View(model);
    }
}
