using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Input model used by the Admin message details page to update workflow status and internal note.
/// </summary>
public class MessageStatusUpdateInputModel
{
    /// <summary>
    /// Message id being updated.
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// New admin workflow status.
    /// </summary>
    [Required]
    [Display(Name = "Status")]
    public ContactMessageStatus Status { get; set; }

    /// <summary>
    /// Internal admin note stored with the message.
    /// </summary>
    [StringLength(2000)]
    [Display(Name = "Admin Note")]
    public string? AdminNote { get; set; }
}
