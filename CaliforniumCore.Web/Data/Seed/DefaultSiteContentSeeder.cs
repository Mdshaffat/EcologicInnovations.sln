using CaliforniumCore.Web.Models.Entities;
using CaliforniumCore.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Data.Seed;

/// <summary>
/// Seeds core singleton-like site records required for a usable first-run experience.
/// It creates data only when missing and never overwrites existing admin content.
/// </summary>
public static class DefaultSiteContentSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await SeedSiteSettingsAsync(dbContext, cancellationToken);
        await SeedAboutUsPageAsync(dbContext, cancellationToken);
        await SeedPolicyPageAsync(dbContext, cancellationToken);
        await SeedContactPageAsync(dbContext, cancellationToken);
    }

    private static async Task SeedSiteSettingsAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var existing = await dbContext.SiteSettings
            .AsNoTracking()
            .AnyAsync(cancellationToken);

        if (existing)
        {
            return;
        }

        var siteSetting = new SiteSetting
        {
            CompanyName = "Californium Core",
            Tagline = "Californium Core delivers advanced solutions in engineering, technology, and innovation to power a smarter, sustainable future.",
            SupportEmail = "support@californiumcore.com",
            SalesEmail = "sales@californiumcore.com",
            Phone = "+8801517831132",
            Address = "Bangladesh",
            LogoUrl = "/uploads/californium-core-logo-atom.svg",
            FaviconUrl = "/uploads/californium-core-logo-atom.svg",
            FooterHtml = "<p>&copy; 2026 Californium Core. All rights reserved.</p>",
            FacebookUrl = "https://facebook.com/",
            LinkedInUrl = "https://linkedin.com/",
            YouTubeUrl = "https://youtube.com/",
            MetaTitleDefault = "Californium Core",
            MetaDescriptionDefault = "Californium Core partners with organizations to deliver scientific software, medical systems, chemical workflows, and materials intelligence tools.",
            HomeValueKicker = "Why Choose Us",
            HomeValueTitle = "Built with scientific discipline and practical delivery",
            HomeValueIntro = "We bring engineering depth, structured thinking, and careful implementation to domains where accuracy and reliability matter.",
            HomeValue1IconCssClass = "bi bi-code-slash",
            HomeValue1Title = "Scientific Software",
            HomeValue1Description = "Purpose-built web, desktop, and workflow tools for teams that need dependable data, reporting, and process control.",
            HomeValue2IconCssClass = "bi bi-heart-pulse",
            HomeValue2Title = "Medical & Laboratory Systems",
            HomeValue2Description = "Secure, practical systems for medical operations, lab coordination, sample tracking, and decision support.",
            HomeValue3IconCssClass = "bi bi-droplet-half",
            HomeValue3Title = "Chemical & Materials Intelligence",
            HomeValue3Description = "Digital products for chemical workflows, material records, quality signals, and experimental knowledge capture.",
            HomeValue4IconCssClass = "bi bi-cpu",
            HomeValue4Title = "Systems & Adoption",
            HomeValue4Description = "Implementation support that helps teams adopt new systems, strengthen technical capability, and keep work moving."
        };

        dbContext.SiteSettings.Add(siteSetting);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedAboutUsPageAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var exists = await dbContext.SitePages
            .AsNoTracking()
            .AnyAsync(x => x.PageKey == SitePageKey.AboutUs, cancellationToken);

        if (exists)
        {
            return;
        }

        var aboutPage = new SitePage
        {
            PageKey = SitePageKey.AboutUs,
            Title = "About Us",
            Slug = "about-us",
            ShortIntro = "We partner with organizations to build scientific software and applied systems for medical, chemical, materials, and technical operations.",
            BannerImageUrl = "/images/placeholders/about-banner.jpg",
            HtmlContent = @"
<section class='content-block'>
    <h2>Who We Are</h2>
    <p>Californium Core is a technology company focused on scientific software, medical systems, chemical workflows, materials intelligence, and reliable operational tools.</p>
    <h2>What We Do</h2>
    <p>We build practical solutions - from web applications and connected workflows to structured data systems - that help technical teams work with clarity and confidence.</p>
    <h2>Our Direction</h2>
    <p>Our goal is to combine modern software engineering with scientific discipline to support better decisions, safer operations, and stronger technical delivery.</p>
</section>",
            MetaTitle = "About Us | Californium Core",
            MetaDescription = "Learn about Californium Core, our mission, and our work in scientific software, medical systems, chemical workflows, and materials intelligence.",
            IsPublished = true,
            SortOrder = 1
        };

        dbContext.SitePages.Add(aboutPage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedPolicyPageAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var exists = await dbContext.SitePages
            .AsNoTracking()
            .AnyAsync(x => x.PageKey == SitePageKey.Policy, cancellationToken);

        if (exists)
        {
            return;
        }

        var policyPage = new SitePage
        {
            PageKey = SitePageKey.Policy,
            Title = "Policy",
            Slug = "policy",
            ShortIntro = "This page contains the current public business and site policy information for Californium Core.",
            BannerImageUrl = "/images/placeholders/policy-banner.jpg",
            HtmlContent = @"
<section class='content-block'>
    <h2>Introduction</h2>
    <p>This policy page is managed from the admin panel. Replace this starter content with your real business policy, privacy statements, service terms, or compliance notes.</p>
    <h2>Data and Communication</h2>
    <p>Californium Core may receive contact information through website forms for business communication and support purposes.</p>
    <h2>Updates</h2>
    <p>This policy can be updated by authorized administrators from the CMS at any time.</p>
</section>",
            MetaTitle = "Policy | Californium Core",
            MetaDescription = "Read the current website and business policy information for Californium Core.",
            IsPublished = true,
            SortOrder = 2
        };

        dbContext.SitePages.Add(policyPage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedContactPageAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var exists = await dbContext.SitePages
            .AsNoTracking()
            .AnyAsync(x => x.PageKey == SitePageKey.Contact, cancellationToken);

        if (exists)
        {
            return;
        }

        var contactPage = new SitePage
        {
            PageKey = SitePageKey.Contact,
            Title = "Contact Us",
            Slug = "contact",
            ShortIntro = "Have a project in mind or need help with something? Tell us about it - we are ready to help.",
            BannerImageUrl = null,
            HtmlContent = @"
<section class='content-block'>
    <h2>Start a Conversation</h2>
    <p>Share a few details about your project, product inquiry, training need, or support request. Our team will review your message and respond as soon as possible.</p>
</section>",
            MetaTitle = "Contact Us | Californium Core",
            MetaDescription = "Contact Californium Core for scientific software, medical systems, chemical workflows, materials intelligence, product inquiries, and project collaboration.",
            IsPublished = true,
            SortOrder = 3
        };

        dbContext.SitePages.Add(contactPage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
