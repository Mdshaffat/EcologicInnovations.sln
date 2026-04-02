using System.ComponentModel.DataAnnotations;
using EcologicInnovations.Web.Models.Base;

namespace EcologicInnovations.Web.Models.Entities;

/// <summary>
/// Represents a category used to organize blog posts.
/// This supports admin structure, filtering, and future SEO expansion.
/// </summary>
public class BlogCategory : BaseEntity
{
    /// <summary>
    /// Public category name.
    /// </summary>
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug for filtering and future category-based URLs.
    /// </summary>
    [Required]
    [StringLength(200)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional description used in admin or future category landing sections.
    /// </summary>
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Sort order for admin lists and public filters.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Controls whether the category is selectable and publicly usable.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation to the posts under this category.
    /// </summary>
    public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
