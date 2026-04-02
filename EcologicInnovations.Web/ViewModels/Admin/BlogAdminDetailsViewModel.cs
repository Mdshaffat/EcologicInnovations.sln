using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Read-only details model for a blog post in the Admin area.
/// </summary>
public class BlogAdminDetailsViewModel
{
    public int Id { get; set; }

    /// <summary>
    /// Blog title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Blog slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Category name if assigned.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Feature image URL.
    /// </summary>
    public string? FeatureImageUrl { get; set; }

    /// <summary>
    /// Excerpt shown in summaries.
    /// </summary>
    public string Excerpt { get; set; } = string.Empty;

    /// <summary>
    /// Sanitized rich HTML preview for admin inspection.
    /// </summary>
    public string? HtmlContentPreview { get; set; }

    /// <summary>
    /// Contact form state.
    /// </summary>
    public bool ShowContactForm { get; set; }

    /// <summary>
    /// Contact form title if configured.
    /// </summary>
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// Featured state.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Published state.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Publication date.
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// Meta title.
    /// </summary>
    public string? MetaTitle { get; set; }

    /// <summary>
    /// Meta description.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Open Graph image URL.
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
