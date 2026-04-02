using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Admin editor model for system-managed pages such as About Us and Policy.
/// It allows editing of title, slug, intro, banner image, SEO fields, publish state,
/// and the raw HTML body that will later be sanitized and rendered publicly.
/// </summary>
public class SitePageEditorViewModel
{
    /// <summary>
    /// Primary key of the SitePage record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Fixed system page key such as AboutUs or Policy.
    /// </summary>
    [Required]
    public SitePageKey PageKey { get; set; }

    /// <summary>
    /// Public page title shown in hero sections and metadata fallbacks.
    /// </summary>
    [Required]
    [StringLength(200)]
    [Display(Name = "Page Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug stored in the database.
    /// For About Us, the public route will still be /about-us unless you later add a generic page router.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Optional banner image URL shown at the top of the public page.
    /// This usually comes from the Media library.
    /// </summary>
    [StringLength(500)]
    [Display(Name = "Banner Image URL")]
    public string? BannerImageUrl { get; set; }

    /// <summary>
    /// Optional short introduction shown before the rich HTML content.
    /// </summary>
    [StringLength(1000)]
    [Display(Name = "Short Introduction")]
    public string? ShortIntro { get; set; }

    /// <summary>
    /// Main admin-written HTML body.
    /// This content is sanitized before save and rendered with Html.Raw later.
    /// </summary>
    [Display(Name = "HTML Content")]
    public string? HtmlContent { get; set; }

    /// <summary>
    /// Optional SEO title for the page.
    /// If blank, the controller will fall back to Title and site defaults.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Meta Title")]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// Optional SEO description for search engine result snippets.
    /// If blank, the controller will fall back to ShortIntro and site defaults.
    /// </summary>
    [StringLength(500)]
    [Display(Name = "Meta Description")]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Controls whether the page is publicly visible.
    /// </summary>
    [Display(Name = "Published")]
    public bool IsPublished { get; set; }

    /// <summary>
    /// Optional sort order for future extensibility.
    /// </summary>
    [Display(Name = "Sort Order")]
    [Range(0, int.MaxValue)]
    public int SortOrder { get; set; }

    /// <summary>
    /// Last updated timestamp shown in the admin editor if needed.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Optional preview HTML used by the admin view after sanitization.
    /// </summary>
    public string? PreviewHtml { get; set; }
}
