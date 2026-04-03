using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Contact;

/// <summary>
/// Input model used by public contact forms across general, product, and blog inquiry flows.
/// </summary>
public class ContactFormInputModel
{
    [Required(ErrorMessage = "Please enter your full name.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 150 characters.")]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(256)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your phone number.")]
    [StringLength(50, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 50 characters.")]
    [RegularExpression(@"^[\d\s\+\-\(\)\.]+$", ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "Company")]
    public string? Company { get; set; }

    [StringLength(200)]
    [Display(Name = "Subject")]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Please enter your message.")]
    [StringLength(4000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 4000 characters.")]
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
