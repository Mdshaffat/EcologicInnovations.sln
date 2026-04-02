using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Row model used in the admin message list table.
/// </summary>
public class MessageListItemViewModel
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
    /// Sender email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Sender phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Source type of the inquiry.
    /// </summary>
    public ContactSourceType SourceType { get; set; }

    /// <summary>
    /// Related source title captured at submission time.
    /// </summary>
    public string? SourceTitle { get; set; }

    /// <summary>
    /// Current workflow status.
    /// </summary>
    public ContactMessageStatus Status { get; set; }

    /// <summary>
    /// Submission timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
