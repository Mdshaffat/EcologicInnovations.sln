using System.ComponentModel.DataAnnotations;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Form model used to create and edit a Product Category in the Admin area.
/// A dedicated viewmodel is used instead of binding the EF entity directly,
/// which keeps the UI contract stable and easier to extend later.
/// </summary>
public class ProductCategoryAdminEditViewModel
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
    /// SEO-friendly slug.
    /// If left blank, it will be auto-generated from the Name field.
    /// </summary>
    [StringLength(200)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase and hyphen-separated.")]
    [Display(Name = "Slug")]
    public string? Slug { get; set; }

    /// <summary>
    /// Optional description used for admin context and future SEO/category content.
    /// </summary>
    [StringLength(1000)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Display order used in public category lists and admin ordering.
    /// </summary>
    [Range(0, int.MaxValue)]
    [Display(Name = "Sort Order")]
    public int SortOrder { get; set; }

    /// <summary>
    /// Controls whether this category is available publicly and in admin selection.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Read-only value shown in edit/details modes.
    /// </summary>
    public int ProductCount { get; set; }
}
