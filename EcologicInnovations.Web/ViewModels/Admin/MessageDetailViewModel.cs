using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Full message details model for the admin message details page.
/// </summary>
public class MessageDetailViewModel
{
    /// <summary>
    /// Contact message id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sender full name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Sender email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Sender phone.
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
    /// Full user message body.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Origin of the inquiry.
    /// </summary>
    public ContactSourceType SourceType { get; set; }

    /// <summary>
    /// Related product id if this is a product inquiry.
    /// </summary>
    public int? ProductId { get; set; }

    /// <summary>
    /// Related blog post id if this is a blog inquiry.
    /// </summary>
    public int? BlogPostId { get; set; }

    /// <summary>
    /// Human-readable title of the source item.
    /// </summary>
    public string? SourceTitle { get; set; }

    /// <summary>
    /// Public page URL from which the user submitted the inquiry.
    /// </summary>
    public string? PageUrl { get; set; }

    /// <summary>
    /// Current admin workflow status.
    /// </summary>
    public ContactMessageStatus Status { get; set; }

    /// <summary>
    /// Existing internal admin note.
    /// </summary>
    public string? AdminNote { get; set; }

    /// <summary>
    /// Submission timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Current update form for status/note changes.
    /// </summary>
    public MessageStatusUpdateInputModel UpdateModel { get; set; } = new();

    /// <summary>
    /// Breadcrumbs for the admin page.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();
}
