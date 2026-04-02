using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Contact;

/// <summary>
/// Input model used by public contact forms across general, product, and blog inquiry flows.
/// </summary>
public class ContactFormInputModel
{
    [Required]
    [StringLength(150)]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "Company")]
    public string? Company { get; set; }

    [StringLength(200)]
    [Display(Name = "Subject")]
    public string? Subject { get; set; }

    [Required]
    [StringLength(4000)]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;

    public ContactSourceType SourceType { get; set; } = ContactSourceType.General;

    public int? ProductId { get; set; }

    public int? BlogPostId { get; set; }

    [StringLength(200)]
    public string? SourceTitle { get; set; }

    [StringLength(500)]
    public string? PageUrl { get; set; }

    [Display(Name = "Regarding Product")]
    public string? RegardingProductTitle { get; set; }

    [Display(Name = "Regarding Article")]
    public string? RegardingBlogTitle { get; set; }
}
