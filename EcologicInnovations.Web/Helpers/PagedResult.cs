using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Represents a page of results plus reusable pagination metadata.
/// This keeps paging logic reusable across Products, Blog, Messages, Media,
/// and future admin list screens.
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// Items for the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    /// <summary>
    /// Pagination state for the current list.
    /// </summary>
    public PaginationViewModel Pagination { get; init; } = new();

    /// <summary>
    /// Total number of matching items across all pages.
    /// </summary>
    public int TotalItems => Pagination.TotalItems;

    /// <summary>
    /// True when the current page returned no items.
    /// </summary>
    public bool IsEmpty => Items.Count == 0;
}
