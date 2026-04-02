using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Input model used by the admin upload form for single or multiple media uploads.
/// </summary>
public class MediaUploadInputModel
{
    /// <summary>
    /// Files selected by the admin for upload.
    /// </summary>
    [Required]
    [Display(Name = "Files")]
    public List<IFormFile> Files { get; set; } = new();

    /// <summary>
    /// Optional grouping tag such as Blog, Product, About, Policy, Banner, Logo, or General.
    /// </summary>
    [StringLength(100)]
    [Display(Name = "Media Group")]
    public string? MediaGroup { get; set; }

    /// <summary>
    /// Optional default alt text applied to uploaded items.
    /// Individual items can still be edited later.
    /// </summary>
    [StringLength(300)]
    [Display(Name = "Default Alt Text")]
    public string? AltText { get; set; }

    /// <summary>
    /// Optional default title applied to uploaded items.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Default Title")]
    public string? Title { get; set; }
}
