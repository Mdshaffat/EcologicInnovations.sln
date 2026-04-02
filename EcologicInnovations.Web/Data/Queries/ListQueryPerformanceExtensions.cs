using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Data.Queries;

/// <summary>
/// Centralizes reusable filter and sort logic for high-traffic or repeated list queries.
/// This keeps controllers cleaner and encourages consistent query shapes that align
/// with the indexes already created in the EF configuration layer.
/// </summary>
public static class ListQueryPerformanceExtensions
{
    // -----------------------------
    // PUBLIC PRODUCTS
    // -----------------------------

    public static IQueryable<Product> AsPublicProductListBase(this IQueryable<Product> query)
    {
        return query
            .AsNoTracking()
            .Where(x => x.IsPublished && x.IsActive && x.ProductCategory != null && x.ProductCategory.IsActive);
    }

    public static IQueryable<Product> ApplyPublicProductFilters(
        this IQueryable<Product> query,
        string? categorySlug,
        string? searchTerm)
    {
        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var slug = categorySlug.Trim().ToLowerInvariant();
            query = query.Where(x => x.ProductCategory != null && x.ProductCategory.Slug == slug);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var keyword = $"%{searchTerm.Trim()}%";

            query = query.Where(x =>
                EF.Functions.Like(x.Title, keyword) ||
                EF.Functions.Like(x.ShortDescription, keyword) ||
                (x.ProductCategory != null && EF.Functions.Like(x.ProductCategory.Name, keyword)));
        }

        return query;
    }

    public static IQueryable<Product> ApplyPublicProductSorting(this IQueryable<Product> query, string? sortBy)
    {
        return (sortBy ?? "featured").Trim().ToLowerInvariant() switch
        {
            "newest" => query
                .OrderByDescending(x => x.CreatedAt)
                .ThenBy(x => x.ListSortOrder)
                .ThenBy(x => x.Title),

            "title_asc" => query
                .OrderBy(x => x.Title)
                .ThenBy(x => x.ListSortOrder),

            "title_desc" => query
                .OrderByDescending(x => x.Title)
                .ThenBy(x => x.ListSortOrder),

            "listsort_asc" => query
                .OrderBy(x => x.ListSortOrder)
                .ThenBy(x => x.Title),

            "listsort_desc" => query
                .OrderByDescending(x => x.ListSortOrder)
                .ThenBy(x => x.Title),

            _ => query
                .OrderByDescending(x => x.IsFeatured)
                .ThenBy(x => x.ListSortOrder)
                .ThenBy(x => x.Title)
        };
    }

    public static IQueryable<ProductCategory> AsPublicProductSidebarBase(this IQueryable<ProductCategory> query)
    {
        return query
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name);
    }

    // -----------------------------
    // ADMIN PRODUCTS
    // -----------------------------

    public static IQueryable<Product> AsAdminProductListBase(this IQueryable<Product> query)
    {
        return query.AsNoTracking();
    }

    public static IQueryable<Product> ApplyAdminProductFilters(
        this IQueryable<Product> query,
        ProductAdminListFilterViewModel filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = $"%{filter.SearchTerm.Trim()}%";

            query = query.Where(x =>
                EF.Functions.Like(x.Title, keyword) ||
                EF.Functions.Like(x.Slug, keyword) ||
                EF.Functions.Like(x.ShortDescription, keyword) ||
                (x.ProductCategory != null && EF.Functions.Like(x.ProductCategory.Name, keyword)));
        }

        if (filter.ProductCategoryId.HasValue)
        {
            query = query.Where(x => x.ProductCategoryId == filter.ProductCategoryId.Value);
        }

        if (filter.IsPublished.HasValue)
        {
            query = query.Where(x => x.IsPublished == filter.IsPublished.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.IsActive.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(x => x.IsFeatured == filter.IsFeatured.Value);
        }

        if (filter.ShowInProductMenu.HasValue)
        {
            query = query.Where(x => x.ShowInProductMenu == filter.ShowInProductMenu.Value);
        }

        return query;
    }

    public static IQueryable<Product> ApplyAdminProductSorting(
        this IQueryable<Product> query,
        string? sortBy)
    {
        return (sortBy ?? "listsort_asc").Trim().ToLowerInvariant() switch
        {
            "newest" => query.OrderByDescending(x => x.CreatedAt),
            "oldest" => query.OrderBy(x => x.CreatedAt),
            "title_asc" => query.OrderBy(x => x.Title),
            "title_desc" => query.OrderByDescending(x => x.Title),
            "menu_asc" => query.OrderBy(x => x.MenuSortOrder).ThenBy(x => x.Title),
            "menu_desc" => query.OrderByDescending(x => x.MenuSortOrder).ThenBy(x => x.Title),
            "listsort_desc" => query.OrderByDescending(x => x.ListSortOrder).ThenBy(x => x.Title),
            _ => query.OrderBy(x => x.ListSortOrder).ThenBy(x => x.Title)
        };
    }

    // -----------------------------
    // PUBLIC BLOGS
    // -----------------------------

    public static IQueryable<BlogPost> AsPublicBlogListBase(this IQueryable<BlogPost> query)
    {
        return query
            .AsNoTracking()
            .Where(x => x.IsPublished);
    }

    public static IQueryable<BlogPost> ApplyPublicBlogFilters(
        this IQueryable<BlogPost> query,
        string? categorySlug,
        string? searchTerm)
    {
        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var slug = categorySlug.Trim().ToLowerInvariant();
            query = query.Where(x => x.BlogCategory != null && x.BlogCategory.Slug == slug);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var keyword = $"%{searchTerm.Trim()}%";

            query = query.Where(x =>
                EF.Functions.Like(x.Title, keyword) ||
                EF.Functions.Like(x.Excerpt!, keyword) ||
                (x.BlogCategory != null && EF.Functions.Like(x.BlogCategory.Name, keyword)));
        }

        return query;
    }

    public static IQueryable<BlogPost> ApplyPublicBlogSorting(this IQueryable<BlogPost> query, string? sortBy)
    {
        return (sortBy ?? "newest").Trim().ToLowerInvariant() switch
        {
            "oldest" => query
                .OrderBy(x => x.PublishedAt ?? x.CreatedAt)
                .ThenBy(x => x.Title),

            "featured" => query
                .OrderByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.PublishedAt ?? x.CreatedAt),

            "title_asc" => query
                .OrderBy(x => x.Title),

            "title_desc" => query
                .OrderByDescending(x => x.Title),

            _ => query
                .OrderByDescending(x => x.PublishedAt ?? x.CreatedAt)
                .ThenBy(x => x.Title)
        };
    }

    // -----------------------------
    // ADMIN BLOGS
    // -----------------------------

    public static IQueryable<BlogPost> AsAdminBlogListBase(this IQueryable<BlogPost> query)
    {
        return query.AsNoTracking();
    }

    public static IQueryable<BlogPost> ApplyAdminBlogFilters(
        this IQueryable<BlogPost> query,
        BlogAdminListFilterViewModel filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = $"%{filter.SearchTerm.Trim()}%";

            query = query.Where(x =>
                EF.Functions.Like(x.Title, keyword) ||
                EF.Functions.Like(x.Slug, keyword) ||
                EF.Functions.Like(x.Excerpt!, keyword) ||
                (x.BlogCategory != null && EF.Functions.Like(x.BlogCategory.Name, keyword)));
        }

        if (filter.BlogCategoryId.HasValue)
        {
            query = query.Where(x => x.BlogCategoryId == filter.BlogCategoryId.Value);
        }

        if (filter.IsPublished.HasValue)
        {
            query = query.Where(x => x.IsPublished == filter.IsPublished.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(x => x.IsFeatured == filter.IsFeatured.Value);
        }

        if (filter.ShowContactForm.HasValue)
        {
            query = query.Where(x => x.ShowContactForm == filter.ShowContactForm.Value);
        }

        if (filter.HasPublishedAt.HasValue)
        {
            query = filter.HasPublishedAt.Value
                ? query.Where(x => x.PublishedAt != null)
                : query.Where(x => x.PublishedAt == null);
        }

        return query;
    }

    public static IQueryable<BlogPost> ApplyAdminBlogSorting(
        this IQueryable<BlogPost> query,
        string? sortBy)
    {
        return (sortBy ?? "published_desc").Trim().ToLowerInvariant() switch
        {
            "newest" => query.OrderByDescending(x => x.CreatedAt),
            "oldest" => query.OrderBy(x => x.CreatedAt),
            "title_asc" => query.OrderBy(x => x.Title),
            "title_desc" => query.OrderByDescending(x => x.Title),
            "published_asc" => query.OrderBy(x => x.PublishedAt ?? x.CreatedAt),
            "featured_first" => query.OrderByDescending(x => x.IsFeatured).ThenByDescending(x => x.PublishedAt ?? x.CreatedAt),
            _ => query.OrderByDescending(x => x.PublishedAt ?? x.CreatedAt).ThenBy(x => x.Title)
        };
    }

    // -----------------------------
    // ADMIN MESSAGES
    // -----------------------------

    public static IQueryable<ContactMessage> AsAdminMessageListBase(this IQueryable<ContactMessage> query)
    {
        return query.AsNoTracking();
    }

    public static IQueryable<ContactMessage> ApplyAdminMessageFilters(
        this IQueryable<ContactMessage> query,
        MessageAdminListFilterViewModel filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = $"%{filter.SearchTerm.Trim()}%";

            query = query.Where(x =>
                EF.Functions.Like(x.Name, keyword) ||
                EF.Functions.Like(x.Email, keyword) ||
                EF.Functions.Like(x.Phone, keyword) ||
                (x.Subject != null && EF.Functions.Like(x.Subject, keyword)) ||
                (x.SourceTitle != null && EF.Functions.Like(x.SourceTitle, keyword)) ||
                EF.Functions.Like(x.Message, keyword));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(x => x.Status == filter.Status.Value);
        }

        if (filter.SourceType.HasValue)
        {
            query = query.Where(x => x.SourceType == filter.SourceType.Value);
        }

        return query;
    }

    public static IQueryable<ContactMessage> ApplyAdminMessageSorting(
        this IQueryable<ContactMessage> query,
        string? sortBy)
    {
        return (sortBy ?? "newest").Trim().ToLowerInvariant() switch
        {
            "oldest" => query.OrderBy(x => x.CreatedAt),
            "status_asc" => query.OrderBy(x => x.Status).ThenByDescending(x => x.CreatedAt),
            "status_desc" => query.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreatedAt),
            "name_asc" => query.OrderBy(x => x.Name).ThenByDescending(x => x.CreatedAt),
            "name_desc" => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };
    }

    // -----------------------------
    // ADMIN MEDIA
    // -----------------------------

    public static IQueryable<MediaFile> AsAdminMediaListBase(this IQueryable<MediaFile> query)
    {
        return query.AsNoTracking();
    }

    public static IQueryable<MediaFile> ApplyAdminMediaFilters(
        this IQueryable<MediaFile> query,
        MediaLibraryFilterViewModel filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = $"%{filter.Keyword.Trim()}%";

            query = query.Where(x =>
                EF.Functions.Like(x.FileName, keyword) ||
                EF.Functions.Like(x.OriginalFileName, keyword) ||
                (x.Title != null && EF.Functions.Like(x.Title, keyword)) ||
                (x.AltText != null && EF.Functions.Like(x.AltText, keyword)) ||
                EF.Functions.Like(x.PublicUrl, keyword));
        }

        if (!string.IsNullOrWhiteSpace(filter.MediaGroup))
        {
            var group = filter.MediaGroup.Trim();
            query = query.Where(x => x.MediaGroup == group);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.IsActive.Value);
        }

        return query;
    }

    public static IQueryable<MediaFile> ApplyAdminMediaSorting(
        this IQueryable<MediaFile> query,
        string? sortBy)
    {
        return (sortBy ?? "newest").Trim().ToLowerInvariant() switch
        {
            "oldest" => query.OrderBy(x => x.UploadedAt),
            "group_asc" => query.OrderBy(x => x.MediaGroup).ThenByDescending(x => x.UploadedAt),
            "group_desc" => query.OrderByDescending(x => x.MediaGroup).ThenByDescending(x => x.UploadedAt),
            "title_asc" => query.OrderBy(x => x.Title).ThenByDescending(x => x.UploadedAt),
            "title_desc" => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.UploadedAt),
            _ => query.OrderByDescending(x => x.UploadedAt)
        };
    }
}
