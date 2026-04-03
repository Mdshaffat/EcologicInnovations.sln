using EcologicInnovations.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Data.Seed;

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
                Name = "Software Solutions",
                Slug = "software-solutions",
                Description = "Web, desktop, and mobile applications built for real-world needs.",
                SortOrder = 1,
                IsActive = true
            },
            new()
            {
                Name = "Smart Systems",
                Slug = "smart-systems",
                Description = "IoT devices, drones, and connected technology for monitoring and automation.",
                SortOrder = 2,
                IsActive = true
            },
            new()
            {
                Name = "Training & Development",
                Slug = "training-development",
                Description = "Workshops, courses, and skill-building programs for teams and individuals.",
                SortOrder = 3,
                IsActive = true
            },
            new()
            {
                Name = "Impact Solutions",
                Slug = "impact-solutions",
                Description = "Technology and tools designed to create positive social and environmental change.",
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
                Name = "Software & Tech",
                Slug = "software-tech",
                Description = "Articles about software development, tools, and technical deep-dives.",
                SortOrder = 1,
                IsActive = true
            },
            new()
            {
                Name = "Smart Systems & IoT",
                Slug = "smart-systems-iot",
                Description = "Content on IoT, drones, connected devices, and automation.",
                SortOrder = 2,
                IsActive = true
            },
            new()
            {
                Name = "Training & Learning",
                Slug = "training-learning",
                Description = "Tips, resources, and insights on skill development and team training.",
                SortOrder = 3,
                IsActive = true
            },
            new()
            {
                Name = "Company Updates",
                Slug = "company-updates",
                Description = "News, launches, and announcements from Ecologic Innovations.",
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
                Title = "EcoLogic Business Platform",
                Slug = "ecologic-business-platform",
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
                MetaTitle = "EcoLogic Business Platform | Ecologic Innovations",
                MetaDescription = "Business software platform starter product for Ecologic Innovations.",
                OgImageUrl = "/images/placeholders/product-software.jpg"
            });
        }

        if (iotCategoryId > 0)
        {
            sampleProducts.Add(new Product
            {
                Title = "Smart Environment Monitor",
                Slug = "smart-environment-monitor",
                ProductCategoryId = iotCategoryId,
                ShortDescription = "An IoT-based solution for real-time environmental monitoring using connected sensors and drone data.",
                MainImageUrl = "/images/placeholders/product-iot.jpg",
                HtmlDetails = @"
<section class='content-block'>
    <h2>Overview</h2>
    <p>This sample entry shows how smart system products — including IoT devices and drones — can be presented with rich HTML content.</p>
</section>",
                ContactFormEnabled = true,
                ContactFormTitle = "Request Information About This System",
                ShowInProductMenu = true,
                MenuSortOrder = 2,
                ListSortOrder = 2,
                IsFeatured = true,
                IsPublished = true,
                IsActive = true,
                MetaTitle = "Smart Environment Monitor | Ecologic Innovations",
                MetaDescription = "Smart system product for environmental monitoring by Ecologic Innovations.",
                OgImageUrl = "/images/placeholders/product-iot.jpg"
            });
        }

        if (trainingCategoryId > 0)
        {
            sampleProducts.Add(new Product
            {
                Title = "Developer Skills Workshop",
                Slug = "developer-skills-workshop",
                ProductCategoryId = trainingCategoryId,
                ShortDescription = "A hands-on training program for teams looking to level up their software development and IoT skills.",
                MainImageUrl = "/images/placeholders/product-energy.jpg",
                HtmlDetails = @"
<section class='content-block'>
    <h2>Overview</h2>
    <p>This sample product demonstrates how training programs and workshops can be listed and managed from the admin panel.</p>
</section>",
                ContactFormEnabled = true,
                ContactFormTitle = "Ask About This Training Program",
                ShowInProductMenu = false,
                MenuSortOrder = 0,
                ListSortOrder = 3,
                IsFeatured = true,
                IsPublished = true,
                IsActive = true,
                MetaTitle = "Developer Skills Workshop | Ecologic Innovations",
                MetaDescription = "Hands-on training workshop by Ecologic Innovations.",
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

        var sustainabilityCategoryId = await dbContext.BlogCategories
            .Where(x => x.Slug == "sustainability-insights")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var energyCategoryId = await dbContext.BlogCategories
            .Where(x => x.Slug == "energy-management")
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var blogPosts = new List<BlogPost>();

        if (sustainabilityCategoryId > 0)
        {
            blogPosts.Add(new BlogPost
            {
                Title = "How Sustainability Technology Supports Business Decisions",
                Slug = "how-sustainability-technology-supports-business-decisions",
                BlogCategoryId = sustainabilityCategoryId,
                FeatureImageUrl = "/images/placeholders/blog-sustainability.jpg",
                Excerpt = "A starter article showing how blog content can be structured and presented for Ecologic Innovations.",
                HtmlContent = @"
<section class='content-block'>
    <h2>Introduction</h2>
    <p>This starter article demonstrates how rich HTML blog content can be managed from the admin panel.</p>
</section>",
                ShowContactForm = true,
                ContactFormTitle = "Talk to Us About This Topic",
                IsFeatured = true,
                IsPublished = true,
                PublishedAt = DateTime.UtcNow,
                MetaTitle = "How Sustainability Technology Supports Business Decisions | Ecologic Innovations",
                MetaDescription = "Starter sustainability blog content for the Ecologic Innovations website.",
                OgImageUrl = "/images/placeholders/blog-sustainability.jpg"
            });
        }

        if (energyCategoryId > 0)
        {
            blogPosts.Add(new BlogPost
            {
                Title = "Practical Energy Monitoring for Modern Operations",
                Slug = "practical-energy-monitoring-for-modern-operations",
                BlogCategoryId = energyCategoryId,
                FeatureImageUrl = "/images/placeholders/blog-energy.jpg",
                Excerpt = "A sample energy management blog post used to populate the latest blog section on first run.",
                HtmlContent = @"
<section class='content-block'>
    <h2>Overview</h2>
    <p>This starter blog post helps demonstrate the public blog list, detail page, and optional inquiry form.</p>
</section>",
                ShowContactForm = false,
                IsFeatured = true,
                IsPublished = true,
                PublishedAt = DateTime.UtcNow.AddMinutes(-5),
                MetaTitle = "Practical Energy Monitoring for Modern Operations | Ecologic Innovations",
                MetaDescription = "Starter energy management blog content for the Ecologic Innovations website.",
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
