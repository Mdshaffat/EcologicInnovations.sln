using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Base;

namespace EcologicInnovations.Web.Models.Entities;

/// <summary>
/// Stores metadata about uploaded files in the reusable media library.
/// Admin users can upload images once, copy the public URL, and reuse the file
/// inside HTML-driven pages such as blogs, products, About Us, and Policy.
/// </summary>
public class MediaFile : BaseEntity
{
    /// <summary>
    /// Sanitized stored file name on disk.
    /// </summary>
    [Required]
    [StringLength(260)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Original client-side file name before sanitization.
    /// </summary>
    [Required]
    [StringLength(260)]
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// Relative or absolute storage path used by the application.
    /// Example: uploads/media/2026/04/hero-banner.webp
    /// </summary>
    [Required]
    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Publicly accessible URL used inside HTML or image previews.
    /// </summary>
    [Required]
    [StringLength(500)]
    public string PublicUrl { get; set; } = string.Empty;

    /// <summary>
    /// MIME content type of the uploaded file.
    /// Example: image/jpeg, image/png, image/webp.
    /// </summary>
    [Required]
    [StringLength(150)]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes.
    /// </summary>
    [Range(0, long.MaxValue)]
    public long FileSize { get; set; }

    /// <summary>
    /// Optional alt text for accessibility and SEO when the file is an image.
    /// </summary>
    [StringLength(300)]
    public string? AltText { get; set; }

    /// <summary>
    /// Optional friendly media title used in admin listings.
    /// </summary>
    [StringLength(200)]
    public string? Title { get; set; }

    /// <summary>
    /// Optional grouping label such as Blog, Product, About, Policy, Banner, or Logo.
    /// This helps organize the media library.
    /// </summary>
    [StringLength(100)]
    public string? MediaGroup { get; set; }

    /// <summary>
    /// Optional user id of the uploader if identity tracking is enabled.
    /// Stored as string to match the common ASP.NET Identity key style.
    /// </summary>
    [StringLength(450)]
    public string? UploadedByUserId { get; set; }

    /// <summary>
    /// UTC date and time when the file was uploaded.
    /// This is separate from BaseEntity.CreatedAt for clarity in admin screens if desired.
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Controls whether the media file is available for reuse.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
