using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcologicInnovations.Web.Models.Base;
using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.Models.Entities;

/// <summary>
/// Stores all inquiries submitted through the website.
/// This is a unified message table covering general contact forms,
/// product inquiry forms, and blog inquiry forms.
/// </summary>
public class ContactMessage : BaseEntity
{
    /// <summary>
    /// Sender full name.
    /// </summary>
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Sender email address.
    /// </summary>
    [Required]
    [StringLength(256)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Sender phone number.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Optional sender company or organization name.
    /// </summary>
    [StringLength(200)]
    public string? Company { get; set; }

    /// <summary>
    /// Optional user-provided subject line.
    /// </summary>
    [StringLength(200)]
    public string? Subject { get; set; }

    /// <summary>
    /// Main inquiry body submitted by the user.
    /// </summary>
    [Required]
    [StringLength(4000)]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the message came from a general page, product page, or blog page.
    /// </summary>
    [Required]
    public ContactSourceType SourceType { get; set; } = ContactSourceType.General;

    /// <summary>
    /// Optional related product id when the source is Product.
    /// </summary>
    public int? ProductId { get; set; }

    /// <summary>
    /// Optional related blog post id when the source is Blog.
    /// </summary>
    public int? BlogPostId { get; set; }

    /// <summary>
    /// Human-readable title of the source item at the time of submission.
    /// Example: product title or blog title.
    /// This is stored redundantly for easier admin review even if the source later changes.
    /// </summary>
    [StringLength(250)]
    public string? SourceTitle { get; set; }

    /// <summary>
    /// Public page URL from which the message was submitted.
    /// Helpful for troubleshooting and admin review.
    /// </summary>
    [StringLength(500)]
    public string? PageUrl { get; set; }

    /// <summary>
    /// Admin workflow state of the inquiry.
    /// </summary>
    [Required]
    public ContactMessageStatus Status { get; set; } = ContactMessageStatus.New;

    /// <summary>
    /// Internal admin note not visible to public users.
    /// </summary>
    [StringLength(2000)]
    public string? AdminNote { get; set; }

    /// <summary>
    /// Navigation to related product when applicable.
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    /// <summary>
    /// Navigation to related blog post when applicable.
    /// </summary>
    [ForeignKey(nameof(BlogPostId))]
    public BlogPost? BlogPost { get; set; }
}
