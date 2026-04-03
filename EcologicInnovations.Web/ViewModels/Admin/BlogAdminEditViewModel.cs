using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Form model used to create and edit blog posts in the Admin area.
/// </summary>
public class BlogAdminEditViewModel
{
    /// <summary>
    /// Blog post id. Zero means create mode.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public blog title.
    /// </summary>
    [Required]
    [StringLength(200)]
    [Display(Name = "Blog Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug. If left blank, it will be generated from the title.
    /// </summary>
    [StringLength(220)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    [Display(Name = "Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Selected blog category id. This is optional.
    /// </summary>
    [Display(Name = "Category")]
    public int? BlogCategoryId { get; set; }

    /// <summary>
    /// Feature image URL. Usually comes from Media / Uploads.
    /// </summary>
    [StringLength(500)]
    [Display(Name = "Feature Image URL")]
    [EcologicInnovations.Web.Validation.RelativeOrAbsoluteUrl]
    public string? FeatureImageUrl { get; set; }

    /// <summary>
    /// Short summary shown on the public blog list and SEO previews.
    /// </summary>
    [Required]
    [StringLength(1000)]
    [Display(Name = "Excerpt")]
    public string Excerpt { get; set; } = string.Empty;

    /// <summary>
    /// Rich HTML article body displayed on the public blog details page.
    /// This content is sanitized before save.
    /// </summary>
    [Display(Name = "HTML Content")]
    public string? HtmlContent { get; set; }

    /// <summary>
    /// Controls whether a contact form is shown below the public blog details page.
    /// </summary>
    [Display(Name = "Show Contact Form")]
    public bool ShowContactForm { get; set; }

    /// <summary>
    /// Optional heading shown above the contact form on the public blog page.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Contact Form Title")]
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// Featured state.
    /// </summary>
    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Published state.
    /// </summary>
    [Display(Name = "Published")]
    public bool IsPublished { get; set; }

    /// <summary>
    /// Optional publication time. If the post is published and this is blank,
    /// the controller can set it automatically.
    /// </summary>
    [Display(Name = "Published At (UTC)")]
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// SEO meta title.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Meta Title")]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO meta description.
    /// </summary>
    [StringLength(500)]
    [Display(Name = "Meta Description")]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Open Graph image URL. If blank, the feature image can be used later as fallback.
    /// </summary>
    [StringLength(500)]
    [Display(Name = "OG Image URL")]
    [EcologicInnovations.Web.Validation.RelativeOrAbsoluteUrl]
    public string? OgImageUrl { get; set; }

    /// <summary>
    /// Category dropdown options.
    /// </summary>
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    /// <summary>
    /// Optional sanitized preview of the HTML content used in the admin UI.
    /// </summary>
    public string? HtmlContentPreview { get; set; }
}
