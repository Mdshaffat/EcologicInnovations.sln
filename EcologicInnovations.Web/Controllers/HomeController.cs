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
            title: siteSettings.MetaTitleDefault ?? "Ecologic Innovations | Purpose-Built Technology for Businesses & Communities",
            description: siteSettings.MetaDescriptionDefault ??
                         "Ecologic Innovations partners with organizations to deliver custom software, intelligent connected systems, and professional development programs that drive measurable results.",
            canonicalPath: "/",
            ogImageUrl: siteSettings.LogoUrl,
            cancellationToken: cancellationToken);

        var model = new HomePageViewModel
        {
            Hero = new HomeHeroViewModel
            {
                Title = "Engineering Tomorrow's Solutions, Today",
                Subtitle = siteSettings.Tagline ??
                           "We partner with businesses and communities to design, build, and scale technology that turns complex challenges into competitive advantages.",
                ImageUrl = null,
                PrimaryButtonText = "Explore Products",
                PrimaryButtonUrl = Url.Action("Index", "Products"),
                SecondaryButtonText = "Contact Us",
                SecondaryButtonUrl = Url.Action("Index", "Contact")
            },
            AboutTitle = aboutPage?.Title ?? "About Ecologic Innovations",
            AboutSummary = aboutPage?.ShortIntro ??
                           "We combine deep technical expertise with a hands-on, outcome-driven approach — delivering solutions that work today and scale for tomorrow.",
            AboutImageUrl = aboutPage?.BannerImageUrl,
            FeaturedProducts = featuredProducts,
            LatestBlogs = latestBlogs,
            ValuePoints =
            [
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-code-slash",
                    Title = "Custom Software",
                    Description = "Tailored web, desktop, and mobile applications engineered to streamline your operations and accelerate growth."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-cpu",
                    Title = "Intelligent Systems",
                    Description = "Connected IoT sensors, drone platforms, and edge devices that deliver real-time insights and automation at scale."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-mortarboard",
                    Title = "Professional Training",
                    Description = "Hands-on workshops and structured programs designed to upskill your team and close critical knowledge gaps."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-globe-americas",
                    Title = "Lasting Impact",
                    Description = "Every solution we deliver is measured by the real-world difference it makes — for your business, your customers, and the wider community."
                }
            ],
            CtaTitle = "Ready to bring your next idea to life?",
            CtaText = "Tell us about the challenge you're facing and let's explore how the right technology can solve it — no obligation, just a conversation.",
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
                Message = "Your home page structure is working. Add products, articles, and page content from the admin panel to fully populate this landing page.",
                ButtonText = "Contact Us",
                ButtonUrl = Url.Action("Index", "Contact")
            };
        }

        ViewData.SetSeoMeta(model.Seo);

        return View(model);
    }
}
