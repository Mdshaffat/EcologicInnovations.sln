using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Reusable EF Core paging helpers for list pages.
/// These helpers clamp page size, prevent negative page values,
/// and keep the final pagination state consistent.
/// </summary>
public static class PaginationQueryExtensions
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 12;
    private const int MaxPageSize = 100;

    /// <summary>
    /// Converts a query into a paged result using safe page and page-size limits.
    /// The query should already have filtering, sorting, and projection applied before calling this method.
    /// </summary>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        string? basePath = null,
        CancellationToken cancellationToken = default)
    {
        pageNumber = NormalizePageNumber(pageNumber);
        pageSize = NormalizePageSize(pageSize);

        var totalItems = await query.CountAsync(cancellationToken);

        if (totalItems == 0)
        {
            return new PagedResult<T>
            {
                Items = Array.Empty<T>(),
                Pagination = new PaginationViewModel
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = 0,
                    BasePath = basePath
                }
            };
        }

        var skip = (pageNumber - 1) * pageSize;

        var items = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            Pagination = new PaginationViewModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                BasePath = basePath
            }
        };
    }

    /// <summary>
    /// Normalizes the requested page number to a safe value.
    /// </summary>
    public static int NormalizePageNumber(int pageNumber)
    {
        return pageNumber < 1 ? DefaultPageNumber : pageNumber;
    }

    /// <summary>
    /// Normalizes the requested page size to a safe bounded value.
    /// </summary>
    public static int NormalizePageSize(int pageSize)
    {
        if (pageSize <= 0)
        {
            return DefaultPageSize;
        }

        if (pageSize > MaxPageSize)
        {
            return MaxPageSize;
        }

        return pageSize;
    }
}
