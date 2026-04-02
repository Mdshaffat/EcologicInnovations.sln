using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcologicInnovations.Web.Models.Base;

namespace EcologicInnovations.Web.Models.Entities;

/// <summary>
/// Represents a product displayed on the public website.
/// Products support structured list data, rich HTML details,
/// optional inquiry form behavior, and menu dropdown highlighting.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Public product title.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug used in the product details route.
    /// Example: smart-energy-monitor.
    /// </summary>
    [Required]
    [StringLength(220)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the product category.
    /// </summary>
    [Required]
    public int ProductCategoryId { get; set; }

    /// <summary>
    /// Short summary shown on cards, lists, and preview sections.
    /// </summary>
    [Required]
    [StringLength(1000)]
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Primary image URL shown in cards and details pages.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Main admin-managed HTML body for the product details page.
    /// This supports rich design using headings, images, tables, and sections.
    /// </summary>
    public string? HtmlDetails { get; set; }

    /// <summary>
    /// Controls whether a contact form is shown below the product details.
    /// </summary>
    public bool ContactFormEnabled { get; set; } = true;

    /// <summary>
    /// Optional custom heading shown above the product inquiry form.
    /// </summary>
    [StringLength(200)]
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// When true, the product appears in the top navigation Products dropdown.
    /// </summary>
    public bool ShowInProductMenu { get; set; } = false;

    /// <summary>
    /// Sort order for the top menu dropdown when ShowInProductMenu is enabled.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int MenuSortOrder { get; set; } = 0;

    /// <summary>
    /// Sort order for the public products list and admin product lists.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int ListSortOrder { get; set; } = 0;

    /// <summary>
    /// Marks the product as featured for display on the home page or promotional sections.
    /// </summary>
    public bool IsFeatured { get; set; } = false;

    /// <summary>
    /// Controls whether the product is publicly visible.
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// Soft active state used for admin control and filtering.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// SEO title for the product page.
    /// </summary>
    [StringLength(200)]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO meta description for the product page.
    /// </summary>
    [StringLength(500)]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Open Graph image URL for social sharing previews.
    /// If not supplied, MainImageUrl can be used as a fallback.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? OgImageUrl { get; set; }

    /// <summary>
    /// Navigation to the parent category.
    /// </summary>
    [ForeignKey(nameof(ProductCategoryId))]
    public ProductCategory? ProductCategory { get; set; }

    /// <summary>
    /// Navigation to messages submitted from this product page.
    /// </summary>
    public ICollection<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
}
