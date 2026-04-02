using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Filter model for the admin message list page.
/// </summary>
public class MessageListFilterViewModel
{
    /// <summary>
    /// Keyword used to search sender name, email, subject, or message text.
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// Selected source type filter.
    /// </summary>
    public ContactSourceType? SourceType { get; set; }

    /// <summary>
    /// Selected message workflow status filter.
    /// </summary>
    public ContactMessageStatus? Status { get; set; }

    /// <summary>
    /// Optional date filter lower bound.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Optional date filter upper bound.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Current page size.
    /// </summary>
    public int PageSize { get; set; } = 20;
}
