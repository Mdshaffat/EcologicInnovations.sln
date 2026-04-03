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
            title: siteSettings.MetaTitleDefault ?? "Ecologic Innovations | Software Development, Smart Systems, Training & Impact",
            description: siteSettings.MetaDescriptionDefault ??
                         "Ecologic Innovations builds custom software, smart IoT and drone systems, training programs, and impact-driven technology for businesses and communities.",
            canonicalPath: "/",
            ogImageUrl: siteSettings.LogoUrl,
            cancellationToken: cancellationToken);

        var model = new HomePageViewModel
        {
            Hero = new HomeHeroViewModel
            {
                Title = "Software, Smart Systems, Training, and Real-World Impact",
                Subtitle = siteSettings.Tagline ??
                           "We're a team of builders and problem-solvers. From web and mobile apps to IoT, drones, and hands-on training — we help businesses and communities grow smarter.",
                ImageUrl = siteSettings.LogoUrl,
                PrimaryButtonText = "Explore Products",
                PrimaryButtonUrl = Url.Action("Index", "Products"),
                SecondaryButtonText = "Contact Us",
                SecondaryButtonUrl = Url.Action("Index", "Contact")
            },
            AboutTitle = aboutPage?.Title ?? "About Ecologic Innovations",
            AboutSummary = aboutPage?.ShortIntro ??
                           "We're passionate about using technology to make a difference. Our team specializes in software development, smart IoT and drone systems, professional training, and building tools that create lasting impact.",
            AboutImageUrl = aboutPage?.BannerImageUrl,
            FeaturedProducts = featuredProducts,
            LatestBlogs = latestBlogs,
            ValuePoints =
            [
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-code-slash",
                    Title = "Software Development",
                    Description = "Web, desktop, and mobile applications — we design and build software from scratch to fit exactly what your business needs."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-cpu",
                    Title = "Smart Systems",
                    Description = "IoT sensors, drones, and connected devices for real-time monitoring, data collection, and intelligent automation."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-mortarboard",
                    Title = "Training & Development",
                    Description = "Practical workshops, technical courses, and skill-building programs that empower teams and individuals to grow."
                },
                new HomeValuePointViewModel
                {
                    IconCssClass = "bi bi-globe-americas",
                    Title = "Impact",
                    Description = "Everything we build is rooted in making a positive difference — for businesses, communities, and the environment."
                }
            ],
            CtaTitle = "Got a project idea? Let's make it happen.",
            CtaText = "Whether you need a web app, a mobile solution, a smart monitoring system, or a training program for your team — we'd love to hear from you.",
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
