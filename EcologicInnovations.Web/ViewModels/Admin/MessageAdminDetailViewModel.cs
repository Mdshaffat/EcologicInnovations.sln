using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Detailed admin page model for a single contact message.
/// </summary>
public class MessageAdminDetailViewModel
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
    /// Optional sender company.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Optional sender subject.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Full message body from the sender.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Source type of the inquiry.
    /// </summary>
    public ContactSourceType SourceType { get; set; }

    /// <summary>
    /// Product id when applicable.
    /// </summary>
    public int? ProductId { get; set; }

    /// <summary>
    /// Blog post id when applicable.
    /// </summary>
    public int? BlogPostId { get; set; }

    /// <summary>
    /// Human-readable source title captured at submission time.
    /// </summary>
    public string? SourceTitle { get; set; }

    /// <summary>
    /// Relative page URL where the inquiry was submitted.
    /// </summary>
    public string? PageUrl { get; set; }

    /// <summary>
    /// Current workflow status.
    /// </summary>
    public ContactMessageStatus Status { get; set; }

    /// <summary>
    /// Existing internal admin note.
    /// </summary>
    public string? AdminNote { get; set; }

    /// <summary>
    /// Whether admin has marked this message as important.
    /// </summary>
    public bool IsImportant { get; set; }

    /// <summary>
    /// Whether admin has flagged this message (red mark).
    /// </summary>
    public bool IsFlagged { get; set; }

    /// <summary>
    /// IP address of the client that submitted the form.
    /// </summary>
    public string? SubmitterIpAddress { get; set; }

    /// <summary>
    /// Browser/device User-Agent string captured at submission time.
    /// </summary>
    public string? SubmitterUserAgent { get; set; }

    /// <summary>
    /// Creation time of the message.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update time of the message.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Form used by the details page to update status and note.
    /// </summary>
    public MessageStatusUpdateInputModel UpdateForm { get; set; } = new();

    /// <summary>
    /// Status dropdown options.
    /// </summary>
    public List<SelectListItem> StatusOptions { get; set; } = new();

    /// <summary>
    /// Breadcrumb trail for the admin details page.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();
}
