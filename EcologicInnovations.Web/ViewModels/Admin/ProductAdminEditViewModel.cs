using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Form model used to create and edit products in the Admin area.
/// A dedicated viewmodel keeps the UI contract stable and prevents direct binding of the EF entity.
/// </summary>
public class ProductAdminEditViewModel
{
    /// <summary>
    /// Product id. Zero means create mode.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public product title.
    /// </summary>
    [Required]
    [StringLength(200)]
    [Display(Name = "Product Title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug.
    /// If left blank, it will be generated from the product title.
    /// </summary>
    [StringLength(220)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    [Display(Name = "Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Selected product category id.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a product category.")]
    [Display(Name = "Category")]
    public int ProductCategoryId { get; set; }

    /// <summary>
    /// Short summary shown on public product cards and previews.
    /// </summary>
    [Required]
    [StringLength(1000)]
    [Display(Name = "Short Description")]
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Main image URL for the product. This typically comes from Media / Uploads.
    /// </summary>
    [StringLength(500)]
    [Url]
    [Display(Name = "Main Image URL")]
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Rich HTML details displayed on the public product details page.
    /// This content is sanitized before save.
    /// </summary>
    [Display(Name = "HTML Details")]
    public string? HtmlDetails { get; set; }

    /// <summary>
    /// Controls whether the public product details page shows a contact form below the content.
    /// </summary>
    [Display(Name = "Enable Contact Form")]
    public bool ContactFormEnabled { get; set; } = true;

    /// <summary>
    /// Optional custom title shown above the contact form.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Contact Form Title")]
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// Controls whether the product appears under the Products dropdown in the main navigation.
    /// </summary>
    [Display(Name = "Show In Product Menu")]
    public bool ShowInProductMenu { get; set; }

    /// <summary>
    /// Sort order used for top menu dropdown products.
    /// </summary>
    [Range(0, int.MaxValue)]
    [Display(Name = "Menu Sort Order")]
    public int MenuSortOrder { get; set; }

    /// <summary>
    /// Sort order used for the public product list page.
    /// </summary>
    [Range(0, int.MaxValue)]
    [Display(Name = "List Sort Order")]
    public int ListSortOrder { get; set; }

    /// <summary>
    /// Marks the product as featured for the home page or highlighted sections.
    /// </summary>
    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Controls whether the product is publicly visible.
    /// </summary>
    [Display(Name = "Published")]
    public bool IsPublished { get; set; }

    /// <summary>
    /// Controls whether the product is active inside the system.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// SEO-specific meta title.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Meta Title")]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// SEO-specific meta description.
    /// </summary>
    [StringLength(500)]
    [Display(Name = "Meta Description")]
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Open Graph image URL. If blank, the main image may be used as a fallback later.
    /// </summary>
    [StringLength(500)]
    [Url]
    [Display(Name = "OG Image URL")]
    public string? OgImageUrl { get; set; }

    /// <summary>
    /// Category dropdown options for the create/edit form.
    /// </summary>
    public List<SelectListItem> CategoryOptions { get; set; } = new();

    /// <summary>
    /// Optional sanitized preview of the HTML details used by admin screens.
    /// </summary>
    public string? HtmlDetailsPreview { get; set; }
}
