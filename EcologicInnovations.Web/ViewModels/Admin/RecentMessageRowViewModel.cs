using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents a compact recent-message row for the dashboard.
/// </summary>
public class RecentMessageRowViewModel
{
    /// <summary>
    /// Contact message id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sender name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Inquiry source type.
    /// </summary>
    public ContactSourceType SourceType { get; set; }

    /// <summary>
    /// Related source title, such as product or article title.
    /// </summary>
    public string? SourceTitle { get; set; }

    /// <summary>
    /// Workflow status.
    /// </summary>
    public ContactMessageStatus Status { get; set; }

    /// <summary>
    /// Submission timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
