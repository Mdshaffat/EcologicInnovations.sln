using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Filter, search, sort, and paging input state for the admin Messages list page.
/// </summary>
public class MessageAdminListFilterViewModel
{
    /// <summary>
    /// Keyword search across sender name, email, phone, subject, source title, and message body.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filter by message workflow status. Null means all statuses.
    /// </summary>
    public ContactMessageStatus? Status { get; set; }

    /// <summary>
    /// Filter by source type. Null means all source types.
    /// </summary>
    public ContactSourceType? SourceType { get; set; }

    /// <summary>
    /// Sort option for the list page.
    /// Supported values: newest, oldest, status_asc, status_desc, name_asc, name_desc.
    /// </summary>
    public string SortBy { get; set; } = "newest";

    /// <summary>
    /// Current page number starting from 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of rows shown per page.
    /// </summary>
    public int PageSize { get; set; } = 15;
}
