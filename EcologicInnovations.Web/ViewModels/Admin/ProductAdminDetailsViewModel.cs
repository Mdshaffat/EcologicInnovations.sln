using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Read-only details model for a Product in the Admin area.
/// </summary>
public class ProductAdminDetailsViewModel
{
    /// <summary>
    /// Product id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Product slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Category name.
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// Short description shown in the public list.
    /// </summary>
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Main image URL.
    /// </summary>
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Sanitized rich HTML preview for admin inspection.
    /// </summary>
    public string? HtmlDetailsPreview { get; set; }

    /// <summary>
    /// Contact form enabled state.
    /// </summary>
    public bool ContactFormEnabled { get; set; }

    /// <summary>
    /// Contact form title if configured.
    /// </summary>
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// Show in product menu state.
    /// </summary>
    public bool ShowInProductMenu { get; set; }

    /// <summary>
    /// Product menu sort order.
    /// </summary>
    public int MenuSortOrder { get; set; }

    /// <summary>
    /// Public list sort order.
    /// </summary>
    public int ListSortOrder { get; set; }

    /// <summary>
    /// Featured state.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Published state.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Active state.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Meta title.
    /// </summary>
    public string? MetaTitle { get; set; }

    /// <summary>
    /// Meta description.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// OG image URL.
    /// </summary>
    public string? OgImageUrl { get; set; }

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last updated date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Optional breadcrumb data for future expansion.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();
}
