using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcologicInnovations.Web.Models.Base;

namespace EcologicInnovations.Web.Models.Entities;

/// <summary>
/// Represents a blog article published on the website.
/// Blog content is admin-managed HTML, allowing rich article layouts,
/// diagrams, formatted sections, and embedded media.
/// </summary>
public class BlogPost : BaseEntity
{
    /// <summary>
    /// Public article title.
    /// </summary>
    [Required]
    [StringLength(220)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug used in the public blog details route.
    /// </summary>
    [Required]
    [StringLength(220)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional category assignment for the blog post.
    /// Some posts may remain uncategorized if desired.
    /// </summary>
    public int? BlogCategoryId { get; set; }

    /// <summary>
    /// Main feature image URL used in the blog list and details page.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? FeatureImageUrl { get; set; }

    /// <summary>
    /// Short excerpt shown in list cards and previews.
    /// </summary>
    [Required]
    [StringLength(1200)]
    public string Excerpt { get; set; } = string.Empty;

    /// <summary>
    /// Main admin-managed HTML article body.
    /// This should support strong presentation while remaining sanitized.
    /// </summary>
    public string? HtmlContent { get; set; }

    /// <summary>
    /// Controls whether a contact form should appear below the article.
    /// </summary>
    public bool ShowContactForm { get; set; } = false;

    /// <summary>
    /// Optional custom title shown above the blog inquiry form.
    /// </summary>
    [StringLength(200)]
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// Marks the article for highlighted display on the home page or featured areas.
    /// </summary>
    public bool IsFeatured { get; set; } = false;

    /// <summary>
    /// Controls whether the article is publicly visible.
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// Publish date used for public display and sort order.
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// SEO title for the article page.
    /// </summary>
    [StringLength(200)]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO meta description for the article page.
    /// </summary>
    [StringLength(500)]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Open Graph image URL for social previews.
    /// If empty, FeatureImageUrl can be used as fallback.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? OgImageUrl { get; set; }

    /// <summary>
    /// Navigation to the parent category if assigned.
    /// </summary>
    [ForeignKey(nameof(BlogCategoryId))]
    public BlogCategory? BlogCategory { get; set; }

    /// <summary>
    /// Navigation to messages submitted from this blog page.
    /// </summary>
    public ICollection<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
}
