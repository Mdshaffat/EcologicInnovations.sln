using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Base;

namespace EcologicInnovations.Web.Models.Entities;

/// <summary>
/// Represents a product category used for product grouping,
/// left-sidebar filtering, and future SEO-friendly category organization.
/// </summary>
public class ProductCategory : BaseEntity
{
    /// <summary>
    /// Public category name.
    /// </summary>
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug for category filtering or future category routes.
    /// </summary>
    [Required]
    [StringLength(200)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional category description shown in admin or future category blocks.
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Controls category display order in filters and admin listings.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Controls whether the category is available for public filtering and admin selection.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation to products in this category.
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
