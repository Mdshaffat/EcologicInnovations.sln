using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin list page model for contact messages.
/// </summary>
public class MessageAdminListViewModel
{
    /// <summary>
    /// Current filter/sort/page state.
    /// </summary>
    public MessageAdminListFilterViewModel Filter { get; set; } = new();

    /// <summary>
    /// Current page result rows.
    /// </summary>
    public List<MessageAdminListItemViewModel> Items { get; set; } = new();

    /// <summary>
    /// Status dropdown options for the filter toolbar.
    /// </summary>
    public List<SelectListItem> StatusOptions { get; set; } = new();

    /// <summary>
    /// Source dropdown options for the filter toolbar.
    /// </summary>
    public List<SelectListItem> SourceTypeOptions { get; set; } = new();

    /// <summary>
    /// Pagination state for the current result set.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();

    /// <summary>
    /// Total matching messages across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total count of messages currently in New status.
    /// </summary>
    public int NewCount { get; set; }

    /// <summary>
    /// Total count of messages marked as important.
    /// </summary>
    public int ImportantCount { get; set; }

    /// <summary>
    /// Total count of messages marked with a red flag.
    /// </summary>
    public int FlaggedCount { get; set; }

    /// <summary>
    /// Total count of unread messages (New + Unread statuses).
    /// </summary>
    public int UnreadCount { get; set; }

    /// <summary>
    /// Optional empty-state block when no messages match the filter.
    /// </summary>
    public EmptyStateViewModel? EmptyState { get; set; }
}
