using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Data.Seed;

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
            CompanyName = "Ecologic Innovations",
            Tagline = "Software, sustainability IoT devices, energy equipment, and eco-focused digital solutions.",
            SupportEmail = "support@ecologicinnovations.com",
            SalesEmail = "sales@ecologicinnovations.com",
            Phone = "+880-0000-000000",
            Address = "Bangladesh",
            FooterHtml = "<p>&copy; 2026 Ecologic Innovations. All rights reserved.</p>",
            FacebookUrl = "https://facebook.com/",
            LinkedInUrl = "https://linkedin.com/",
            YouTubeUrl = "https://youtube.com/",
            MetaTitleDefault = "Ecologic Innovations",
            MetaDescriptionDefault = "Ecologic Innovations delivers software, sustainability IoT devices, energy equipment, and business-focused eco technology solutions."
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
            ShortIntro = "Ecologic Innovations helps organizations grow through practical software, sustainability technology, and energy-focused solutions.",
            BannerImageUrl = "/images/placeholders/about-banner.jpg",
            HtmlContent = @"
<section class='content-block'>
    <h2>Who We Are</h2>
    <p>Ecologic Innovations is a business-focused technology company working in software, sustainability-related IoT devices, energy equipment, and solution-driven digital services.</p>
    <h2>What We Do</h2>
    <p>We build practical systems that help businesses improve visibility, efficiency, sustainability performance, and operational decision-making.</p>
    <h2>Our Direction</h2>
    <p>Our goal is to combine modern software thinking with real-world sustainability and energy applications.</p>
</section>",
            MetaTitle = "About Us | Ecologic Innovations",
            MetaDescription = "Learn about Ecologic Innovations, our mission, and our work in software, sustainability IoT devices, and energy equipment.",
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
            ShortIntro = "This page contains the current public business and site policy information for Ecologic Innovations.",
            BannerImageUrl = "/images/placeholders/policy-banner.jpg",
            HtmlContent = @"
<section class='content-block'>
    <h2>Introduction</h2>
    <p>This policy page is managed from the admin panel. Replace this starter content with your real business policy, privacy statements, service terms, or compliance notes.</p>
    <h2>Data and Communication</h2>
    <p>Ecologic Innovations may receive contact information through website forms for business communication and support purposes.</p>
    <h2>Updates</h2>
    <p>This policy can be updated by authorized administrators from the CMS at any time.</p>
</section>",
            MetaTitle = "Policy | Ecologic Innovations",
            MetaDescription = "Read the current website and business policy information for Ecologic Innovations.",
            IsPublished = true,
            SortOrder = 2
        };

        dbContext.SitePages.Add(policyPage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
