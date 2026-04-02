namespace EcologicInnovations.Web.Models.Enums;

/// <summary>
/// Represents the current admin workflow state of a contact message.
/// </summary>
public enum ContactMessageStatus
{
    /// <summary>
    /// Newly submitted message that has not yet been reviewed.
    /// </summary>
    New = 1,

    /// <summary>
    /// Message has been opened or reviewed by an admin.
    /// </summary>
    Read = 2,

    /// <summary>
    /// Admin has responded to the inquiry.
    /// </summary>
    Replied = 3,

    /// <summary>
    /// Inquiry has been completed or closed.
    /// </summary>
    Closed = 4
}
