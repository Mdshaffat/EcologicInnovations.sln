using System.ComponentModel.DataAnnotations;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Form model used to create and edit blog categories in the Admin area.
/// </summary>
public class BlogCategoryAdminEditViewModel
{
    /// <summary>
    /// Category id. Zero means create mode.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public category name.
    /// </summary>
    [Required]
    [StringLength(150)]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug. If blank, it will be generated from Name.
    /// </summary>
    [StringLength(200)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    [Display(Name = "Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Optional description used for admin context and future public/category SEO use.
    /// </summary>
    [StringLength(1000)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Sort order used for admin lists and public ordering where needed.
    /// </summary>
    [Range(0, int.MaxValue)]
    [Display(Name = "Sort Order")]
    public int SortOrder { get; set; }

    /// <summary>
    /// Controls whether the category is active.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Read-only value shown in edit/details modes.
    /// </summary>
    public int BlogCount { get; set; }
}
