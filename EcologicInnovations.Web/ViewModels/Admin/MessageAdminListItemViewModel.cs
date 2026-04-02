using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents one message row on the admin Messages list page.
/// </summary>
public class MessageAdminListItemViewModel
{
    /// <summary>
    /// Message primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sender full name.
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
    /// Optional subject line entered by the sender.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Source type of the message: General, Product, or Blog.
    /// </summary>
    public ContactSourceType SourceType { get; set; }

    /// <summary>
    /// Human-readable title of the source item if any.
    /// </summary>
    public string? SourceTitle { get; set; }

    /// <summary>
    /// Current admin workflow status.
    /// </summary>
    public ContactMessageStatus Status { get; set; }

    /// <summary>
    /// UTC creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Small preview of the message body for quick list scanning.
    /// </summary>
    public string? MessagePreview { get; set; }
}
