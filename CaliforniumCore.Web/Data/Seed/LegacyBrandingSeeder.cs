using CaliforniumCore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Data.Seed;

/// <summary>
/// Updates existing starter/admin content that still carries the previous brand.
/// </summary>
public static class LegacyBrandingSeeder
{
    private const string NewLogoUrl = "/uploads/californium-core-logo-atom.svg";
    private const string LegacyLogoUrl = "/uploads/logo.png";

    public static async Task UpdateAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        var changed = false;

        changed |= await UpdateSiteSettingsAsync(dbContext, cancellationToken);
        changed |= await UpdateSitePagesAsync(dbContext, cancellationToken);
        changed |= await UpdateProductCategoriesAsync(dbContext, cancellationToken);
        changed |= await UpdateBlogCategoriesAsync(dbContext, cancellationToken);
        changed |= await UpdateProductsAsync(dbContext, cancellationToken);
        changed |= await UpdateBlogPostsAsync(dbContext, cancellationToken);

        if (changed)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static async Task<bool> UpdateSiteSettingsAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var changed = false;
        var settings = await dbContext.SiteSettings.ToListAsync(cancellationToken);

        foreach (var item in settings)
        {
            changed |= ReplaceIfChanged(item.CompanyName, value => item.CompanyName = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Tagline, value => item.Tagline = value);
            changed |= ReplaceIfChanged(item.SupportEmail, value => item.SupportEmail = value);
            changed |= ReplaceIfChanged(item.SalesEmail, value => item.SalesEmail = value);
            changed |= ReplaceIfChanged(item.FooterHtml, value => item.FooterHtml = value);
            changed |= ReplaceIfChanged(item.MetaTitleDefault, value => item.MetaTitleDefault = value);
            changed |= ReplaceIfChanged(item.MetaDescriptionDefault, value => item.MetaDescriptionDefault = value);

            if (string.IsNullOrWhiteSpace(item.LogoUrl) ||
                string.Equals(item.LogoUrl, LegacyLogoUrl, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(item.LogoUrl, "/uploads/californium-core-logo.svg", StringComparison.OrdinalIgnoreCase))
            {
                item.LogoUrl = NewLogoUrl;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(item.FaviconUrl) ||
                string.Equals(item.FaviconUrl, LegacyLogoUrl, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(item.FaviconUrl, "/uploads/californium-core-logo.svg", StringComparison.OrdinalIgnoreCase))
            {
                item.FaviconUrl = NewLogoUrl;
                changed = true;
            }

            if (string.Equals(
                    item.Tagline,
                    "Empowering businesses through purpose-built technology and professional development.",
                    StringComparison.OrdinalIgnoreCase))
            {
                item.Tagline = "Californium Core delivers advanced solutions in engineering, technology, and innovation to power a smarter, sustainable future.";
                changed = true;
            }

            changed |= SetIfBlank(item.HomeValueKicker, value => item.HomeValueKicker = value, "Why Choose Us");
            changed |= SetIfBlank(item.HomeValueTitle, value => item.HomeValueTitle = value, "Built with scientific discipline and practical delivery");
            changed |= SetIfBlank(
                item.HomeValueIntro,
                value => item.HomeValueIntro = value,
                "We bring engineering depth, structured thinking, and careful implementation to domains where accuracy and reliability matter.");
            changed |= SetIfBlank(item.HomeValue1IconCssClass, value => item.HomeValue1IconCssClass = value, "bi bi-code-slash");
            changed |= SetIfBlank(item.HomeValue1Title, value => item.HomeValue1Title = value, "Scientific Software");
            changed |= SetIfBlank(
                item.HomeValue1Description,
                value => item.HomeValue1Description = value,
                "Purpose-built web, desktop, and workflow tools for teams that need dependable data, reporting, and process control.");
            changed |= SetIfBlank(item.HomeValue2IconCssClass, value => item.HomeValue2IconCssClass = value, "bi bi-heart-pulse");
            changed |= SetIfBlank(item.HomeValue2Title, value => item.HomeValue2Title = value, "Medical & Laboratory Systems");
            changed |= SetIfBlank(
                item.HomeValue2Description,
                value => item.HomeValue2Description = value,
                "Secure, practical systems for medical operations, lab coordination, sample tracking, and decision support.");
            changed |= SetIfBlank(item.HomeValue3IconCssClass, value => item.HomeValue3IconCssClass = value, "bi bi-droplet-half");
            changed |= SetIfBlank(item.HomeValue3Title, value => item.HomeValue3Title = value, "Chemical & Materials Intelligence");
            changed |= SetIfBlank(
                item.HomeValue3Description,
                value => item.HomeValue3Description = value,
                "Digital products for chemical workflows, material records, quality signals, and experimental knowledge capture.");
            changed |= SetIfBlank(item.HomeValue4IconCssClass, value => item.HomeValue4IconCssClass = value, "bi bi-cpu");
            changed |= SetIfBlank(item.HomeValue4Title, value => item.HomeValue4Title = value, "Systems & Adoption");
            changed |= SetIfBlank(
                item.HomeValue4Description,
                value => item.HomeValue4Description = value,
                "Implementation support that helps teams adopt new systems, strengthen technical capability, and keep work moving.");
        }

        return changed;
    }

    private static async Task<bool> UpdateSitePagesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var changed = false;
        var pages = await dbContext.SitePages.ToListAsync(cancellationToken);

        foreach (var item in pages)
        {
            changed |= ReplaceIfChanged(item.Title, value => item.Title = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.ShortIntro, value => item.ShortIntro = value);
            changed |= ReplaceIfChanged(item.HtmlContent, value => item.HtmlContent = value);
            changed |= ReplaceIfChanged(item.MetaTitle, value => item.MetaTitle = value);
            changed |= ReplaceIfChanged(item.MetaDescription, value => item.MetaDescription = value);
        }

        return changed;
    }

    private static async Task<bool> UpdateProductCategoriesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var changed = false;
        var categories = await dbContext.ProductCategories.ToListAsync(cancellationToken);

        foreach (var item in categories)
        {
            changed |= ReplaceIfChanged(item.Name, value => item.Name = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Slug, value => item.Slug = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Description, value => item.Description = value);
        }

        return changed;
    }

    private static async Task<bool> UpdateBlogCategoriesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var changed = false;
        var categories = await dbContext.BlogCategories.ToListAsync(cancellationToken);

        foreach (var item in categories)
        {
            changed |= ReplaceIfChanged(item.Name, value => item.Name = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Slug, value => item.Slug = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Description, value => item.Description = value);
        }

        return changed;
    }

    private static async Task<bool> UpdateProductsAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var changed = false;
        var products = await dbContext.Products.ToListAsync(cancellationToken);

        foreach (var item in products)
        {
            changed |= ReplaceIfChanged(item.Title, value => item.Title = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Slug, value => item.Slug = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.ShortDescription, value => item.ShortDescription = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.HtmlDetails, value => item.HtmlDetails = value);
            changed |= ReplaceIfChanged(item.ContactFormTitle, value => item.ContactFormTitle = value);
            changed |= ReplaceIfChanged(item.MetaTitle, value => item.MetaTitle = value);
            changed |= ReplaceIfChanged(item.MetaDescription, value => item.MetaDescription = value);
        }

        return changed;
    }

    private static async Task<bool> UpdateBlogPostsAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var changed = false;
        var posts = await dbContext.BlogPosts.ToListAsync(cancellationToken);

        foreach (var item in posts)
        {
            changed |= ReplaceIfChanged(item.Title, value => item.Title = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Slug, value => item.Slug = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.Excerpt, value => item.Excerpt = value ?? string.Empty);
            changed |= ReplaceIfChanged(item.HtmlContent, value => item.HtmlContent = value);
            changed |= ReplaceIfChanged(item.ContactFormTitle, value => item.ContactFormTitle = value);
            changed |= ReplaceIfChanged(item.MetaTitle, value => item.MetaTitle = value);
            changed |= ReplaceIfChanged(item.MetaDescription, value => item.MetaDescription = value);
        }

        return changed;
    }

    private static bool ReplaceIfChanged(string? current, Action<string?> assign)
    {
        var updated = ReplaceLegacyText(current);

        if (updated == current)
        {
            return false;
        }

        assign(updated);
        return true;
    }

    private static bool SetIfBlank(string? current, Action<string?> assign, string value)
    {
        if (!string.IsNullOrWhiteSpace(current))
        {
            return false;
        }

        assign(value);
        return true;
    }

    private static string? ReplaceLegacyText(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value
            .Replace("Ecologic Innovations", "Californium Core", StringComparison.Ordinal)
            .Replace("EcologicInnovations", "CaliforniumCore", StringComparison.Ordinal)
            .Replace("ecologicinnovations", "californiumcore", StringComparison.Ordinal)
            .Replace("EcoLogic", "Californium Core", StringComparison.Ordinal)
            .Replace("ecologic-business-platform", "californium-core-business-platform", StringComparison.Ordinal)
            .Replace("We partner with organizations to build technology that solves real problems - from custom platforms and connected devices to hands-on skills development.",
                "We partner with organizations to build scientific software and applied systems for medical, chemical, materials, and technical operations.",
                StringComparison.Ordinal)
            .Replace("Californium Core is a technology company focused on software development, smart IoT and drone systems, professional training, and building tools that create real-world impact.",
                "Californium Core is a technology company focused on scientific software, medical systems, chemical workflows, materials intelligence, and reliable operational tools.",
                StringComparison.Ordinal)
            .Replace("We build practical solutions - from web and mobile apps to connected devices and hands-on training programs - that help businesses and communities grow smarter.",
                "We build practical solutions - from web applications and connected workflows to structured data systems - that help technical teams work with clarity and confidence.",
                StringComparison.Ordinal)
            .Replace("Our goal is to combine modern software thinking with smart technology and education to drive meaningful change.",
                "Our goal is to combine modern software engineering with scientific discipline to support better decisions, safer operations, and stronger technical delivery.",
                StringComparison.Ordinal)
            .Replace("Software Solutions", "Scientific Software", StringComparison.Ordinal)
            .Replace("Smart Systems & IoT", "Medical & Chemical Systems", StringComparison.Ordinal)
            .Replace("Smart Systems", "Medical & Chemical Systems", StringComparison.Ordinal)
            .Replace("Training & Development", "Materials Intelligence", StringComparison.Ordinal)
            .Replace("Training & Learning", "Materials Intelligence", StringComparison.Ordinal)
            .Replace("Impact Solutions", "Sustainability & Operations", StringComparison.Ordinal)
            .Replace("Web, desktop, and mobile applications built for real-world needs.",
                "Web, desktop, and workflow applications built for technical and scientific teams.",
                StringComparison.Ordinal)
            .Replace("IoT devices, drones, and connected technology for monitoring and automation.",
                "Operational systems for clinics, labs, formulas, samples, and quality workflows.",
                StringComparison.Ordinal)
            .Replace("Workshops, courses, and skill-building programs for teams and individuals.",
                "Structured systems for materials data, experiment records, and technical insight.",
                StringComparison.Ordinal)
            .Replace("Technology and tools designed to create positive social and environmental change.",
                "Applied technology that supports resilient, efficient, and sustainable operations.",
                StringComparison.Ordinal)
            .Replace("Smart Environment Monitor", "Laboratory Environment Monitor", StringComparison.Ordinal)
            .Replace("smart-environment-monitor", "laboratory-environment-monitor", StringComparison.Ordinal)
            .Replace("An IoT-based solution for real-time environmental monitoring using connected sensors and drone data.",
                "A connected monitoring system for laboratory conditions, sample environments, and operational signals.",
                StringComparison.Ordinal)
            .Replace("This sample entry shows how smart system products — including IoT devices and drones — can be presented with rich HTML content.",
                "This sample entry shows how laboratory and connected monitoring systems can be presented with rich HTML content.",
                StringComparison.Ordinal)
            .Replace("Smart system product for environmental monitoring by Californium Core.",
                "Laboratory environment monitoring product by Californium Core.",
                StringComparison.Ordinal)
            .Replace("Developer Skills Workshop", "Materials Data Workspace", StringComparison.Ordinal)
            .Replace("developer-skills-workshop", "materials-data-workspace", StringComparison.Ordinal)
            .Replace("A hands-on training program for teams looking to level up their software development and IoT skills.",
                "A structured workspace for materials records, experiment notes, technical files, and review workflows.",
                StringComparison.Ordinal)
            .Replace("This sample product demonstrates how training programs and workshops can be listed and managed from the admin panel.",
                "This sample product demonstrates how materials-focused systems can be listed and managed from the admin panel.",
                StringComparison.Ordinal)
            .Replace("Ask About This Training Program", "Ask About This Materials System", StringComparison.Ordinal)
            .Replace("Hands-on training workshop by Californium Core.",
                "Materials data workspace by Californium Core.",
                StringComparison.Ordinal);
    }
}
