using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Base;
using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.Models.Entities;

/// <summary>
/// Stores content-managed single-instance pages such as About Us and Policy.
/// Admin users can edit title, intro, banner, SEO metadata, and raw HTML content.
/// </summary>
public class SitePage : BaseEntity
{
    /// <summary>
    /// Fixed key used to identify the system page type.
    /// Examples: AboutUs, Policy.
    /// </summary>
    [Required]
    public SitePageKey PageKey { get; set; }

    /// <summary>
    /// Public title displayed on the page and sometimes in metadata.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Public slug used for route generation.
    /// Example: about-us or policy.
    /// </summary>
    [Required]
    [StringLength(200)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional banner image URL shown at the top of the page.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? BannerImageUrl { get; set; }

    /// <summary>
    /// Optional short intro paragraph shown before the main HTML content.
    /// </summary>
    [StringLength(1000)]
    public string? ShortIntro { get; set; }

    /// <summary>
    /// Main admin-managed HTML content.
    /// This should be sanitized before storage or render.
    /// </summary>
    public string? HtmlContent { get; set; }

    /// <summary>
    /// SEO-specific title. If empty, the page title can be used as fallback.
    /// </summary>
    [StringLength(200)]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO-specific meta description for search engines and social previews.
    /// </summary>
    [StringLength(500)]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Controls whether the page is publicly visible.
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// Optional display order if multiple site pages are listed anywhere later.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int SortOrder { get; set; } = 0;
}
