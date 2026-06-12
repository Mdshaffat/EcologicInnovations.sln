using CaliforniumCore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Data.Seed;

/// <summary>
/// Seeds starter product and blog categories and optionally sample products/blogs.
/// All operations are idempotent and only fill missing starter data.
/// </summary>
public static class DefaultCatalogSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext dbContext,
        bool seedCategories,
        bool seedSampleCatalogContent,
        CancellationToken cancellationToken = default)
    {
        if (seedCategories)
        {
            await SeedProductCategoriesAsync(dbContext, cancellationToken);
            await SeedBlogCategoriesAsync(dbContext, cancellationToken);
        }

        if (seedSampleCatalogContent)
        {
            await SeedSampleProductsAsync(dbContext, cancellationToken);
            await SeedSampleBlogPostsAsync(dbContext, cancellationToken);
        }
    }

    private static async Task SeedProductCategoriesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var starterCategories = new List<ProductCategory>
        {
            new()
            {
                Name = "Scientific Software",
                Slug = "software-solutions",
                Description = "Web, desktop, and workflow applications built for technical and scientific teams.",
                SortOrder = 1,
                IsActive = true
            },
            new()
            {
                Name = "Medical & Chemical Systems",
                Slug = "smart-systems",
                Description = "Operational systems for clinics, labs, formulas, samples, and quality workflows.",
                SortOrder = 2,
                IsActive = true
            },
            new()
            {
                Name = "Materials Intelligence",
                Slug = "training-development",
                Description = "Structured systems for materials data, experiment records, and technical insight.",
                SortOrder = 3,
                IsActive = true
            },
            new()
            {
                Name = "Sustainability & Operations",
                Slug = "impact-solutions",
                Description = "Applied technology that supports resilient, efficient, and sustainable operations.",
                SortOrder = 4,
                IsActive = true
            }
        };

        foreach (var category in starterCategories)
        {
            var exists = await dbContext.ProductCategories
                .AsNoTracking()
                .AnyAsync(x => x.Slug == category.Slug, cancellationToken);

            if (!exists)
            {
                dbContext.ProductCategories.Add(category);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedBlogCategoriesAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var starterCategories = new List<BlogCategory>
        {
            new()
            {
                Name = "Scientific Software",
                Slug = "software-tech",
                Description = "Articles about scientific software, tools, and technical deep-dives.",
                SortOrder = 1,
                IsActive = true
            },
            new()
            {
                Name = "Medical & Chemical Systems",
                Slug = "smart-systems-iot",
                Description = "Content on medical operations, laboratory systems, chemical workflows, and automation.",
                SortOrder = 2,
                IsActive = true
            },
            new()
            {
                Name = "Materials Intelligence",
                Slug = "training-learning",
                Description = "Resources and insights on materials data, experimental records, and technical knowledge.",
                SortOrder = 3,
                IsActive = true
            },
            new()
            {
                Name = "Company Updates",
                Slug = "company-updates",
                Description = "News, launches, and announcements from Californium Core.",
                SortOrder = 4,
                IsActive = true
            }
        };

        foreach (var category in starterCategories)
        {
            var exists = await dbContext.BlogCategories
                .AsNoTracking()
                .AnyAsync(x => x.Slug == category.Slug, cancellationToken);

            if (!exists)
            {
                dbContext.BlogCategories.Add(category);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedSampleProductsAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var hasProducts = await dbContext.Products
            .AsNoTracking()
            .AnyAsync(cancellationToken);

        if (hasProducts)
        {
            return;
        }

        var softwareCategoryId = await dbContext.ProductCategories
            .Where(x => x.Slug == "software-solutions")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var iotCategoryId = await dbContext.ProductCategories
            .Where(x => x.Slug == "smart-systems")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var trainingCategoryId = await dbContext.ProductCategories
            .Where(x => x.Slug == "training-development")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var sampleProducts = new List<Product>();

        if (softwareCategoryId > 0)
        {
            sampleProducts.Add(new Product
            {
                Title = "Californium Core Business Platform",
                Slug = "californium-core-business-platform",
                ProductCategoryId = softwareCategoryId,
                ShortDescription = "A business-ready digital platform for operational visibility, content management, and customer engagement.",
                MainImageUrl = "/images/placeholders/product-software.jpg",
                HtmlDetails = @"
<section class='content-block'>
    <h2>Overview</h2>
    <p>This starter product demonstrates how rich HTML product details can be managed from admin.</p>
    <h2>Use Cases</h2>
    <ul>
        <li>Business websites</li>
        <li>Content-driven systems</li>
        <li>Digital management portals</li>
    </ul>
</section>",
                ContactFormEnabled = true,
                ContactFormTitle = "Contact Us About This Product",
                ShowInProductMenu = true,
                MenuSortOrder = 1,
                ListSortOrder = 1,
                IsFeatured = true,
                IsPublished = true,
                IsActive = true,
                MetaTitle = "Californium Core Business Platform | Californium Core",
                MetaDescription = "Business software platform starter product for Californium Core.",
                OgImageUrl = "/images/placeholders/product-software.jpg"
            });
        }

        if (iotCategoryId > 0)
        {
            sampleProducts.Add(new Product
            {
                Title = "Laboratory Environment Monitor",
                Slug = "laboratory-environment-monitor",
                ProductCategoryId = iotCategoryId,
                ShortDescription = "A connected monitoring system for laboratory conditions, sample environments, and operational signals.",
                MainImageUrl = "/images/placeholders/product-iot.jpg",
                HtmlDetails = @"
<section class='content-block'>
    <h2>Overview</h2>
    <p>This sample entry shows how laboratory and connected monitoring systems can be presented with rich HTML content.</p>
</section>",
                ContactFormEnabled = true,
                ContactFormTitle = "Request Information About This System",
                ShowInProductMenu = true,
                MenuSortOrder = 2,
                ListSortOrder = 2,
                IsFeatured = true,
                IsPublished = true,
                IsActive = true,
                MetaTitle = "Laboratory Environment Monitor | Californium Core",
                MetaDescription = "Laboratory environment monitoring product by Californium Core.",
                OgImageUrl = "/images/placeholders/product-iot.jpg"
            });
        }

        if (trainingCategoryId > 0)
        {
            sampleProducts.Add(new Product
            {
                Title = "Materials Data Workspace",
                Slug = "materials-data-workspace",
                ProductCategoryId = trainingCategoryId,
                ShortDescription = "A structured workspace for materials records, experiment notes, technical files, and review workflows.",
                MainImageUrl = "/images/placeholders/product-energy.jpg",
                HtmlDetails = @"
<section class='content-block'>
    <h2>Overview</h2>
    <p>This sample product demonstrates how materials-focused systems can be listed and managed from the admin panel.</p>
</section>",
                ContactFormEnabled = true,
                ContactFormTitle = "Ask About This Materials System",
                ShowInProductMenu = false,
                MenuSortOrder = 0,
                ListSortOrder = 3,
                IsFeatured = true,
                IsPublished = true,
                IsActive = true,
                MetaTitle = "Materials Data Workspace | Californium Core",
                MetaDescription = "Materials data workspace by Californium Core.",
                OgImageUrl = "/images/placeholders/product-energy.jpg"
            });
        }

        if (sampleProducts.Count > 0)
        {
            dbContext.Products.AddRange(sampleProducts);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static async Task SeedSampleBlogPostsAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var hasPosts = await dbContext.BlogPosts
            .AsNoTracking()
            .AnyAsync(cancellationToken);

        if (hasPosts)
        {
            return;
        }

        var softwareTechCategoryId = await dbContext.BlogCategories
            .Where(x => x.Slug == "software-tech")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var smartSystemsCategoryId = await dbContext.BlogCategories
            .Where(x => x.Slug == "smart-systems-iot")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var blogPosts = new List<BlogPost>();

        if (softwareTechCategoryId > 0)
        {
            blogPosts.Add(new BlogPost
            {
                Title = "How Sustainability Technology Supports Business Decisions",
                Slug = "how-sustainability-technology-supports-business-decisions",
                BlogCategoryId = softwareTechCategoryId,
                FeatureImageUrl = "/images/placeholders/blog-sustainability.jpg",
                Excerpt = "A starter article showing how article content can be structured and presented for Californium Core.",
                HtmlContent = @"
<section class='content-block'>
    <h2>Introduction</h2>
    <p>This starter article demonstrates how rich HTML article content can be managed from the admin panel.</p>
</section>",
                ShowContactForm = true,
                ContactFormTitle = "Talk to Us About This Topic",
                IsFeatured = true,
                IsPublished = true,
                PublishedAt = DateTime.UtcNow,
                MetaTitle = "How Sustainability Technology Supports Business Decisions | Californium Core",
                MetaDescription = "Starter sustainability article content for the Californium Core website.",
                OgImageUrl = "/images/placeholders/blog-sustainability.jpg"
            });
        }

        if (smartSystemsCategoryId > 0)
        {
            blogPosts.Add(new BlogPost
            {
                Title = "Practical Energy Monitoring for Modern Operations",
                Slug = "practical-energy-monitoring-for-modern-operations",
                BlogCategoryId = smartSystemsCategoryId,
                FeatureImageUrl = "/images/placeholders/blog-energy.jpg",
                Excerpt = "A sample energy management article used to populate the latest articles section on first run.",
                HtmlContent = @"
<section class='content-block'>
    <h2>Overview</h2>
    <p>This starter article helps demonstrate the public article list, detail page, and optional inquiry form.</p>
</section>",
                ShowContactForm = false,
                IsFeatured = true,
                IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddMinutes(-5),
                MetaTitle = "Practical Energy Monitoring for Modern Operations | Californium Core",
                MetaDescription = "Starter energy management article content for the Californium Core website.",
                OgImageUrl = "/images/placeholders/blog-energy.jpg"
            });
        }

        if (blogPosts.Count > 0)
        {
            dbContext.BlogPosts.AddRange(blogPosts);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
