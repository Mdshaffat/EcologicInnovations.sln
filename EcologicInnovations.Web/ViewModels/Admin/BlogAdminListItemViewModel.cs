namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents one blog row in the admin list page.
/// </summary>
public class BlogAdminListItemViewModel
{
    /// <summary>
    /// Blog primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Blog title shown prominently in the list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly blog slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Blog category name if assigned.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Optional feature image URL shown as a thumbnail in the list.
    /// </summary>
    public string? FeatureImageUrl { get; set; }

    /// <summary>
    /// Short article summary shown in truncated form.
    /// </summary>
    public string? Excerpt { get; set; }

    /// <summary>
    /// Whether the public blog page shows a contact form.
    /// </summary>
    public bool ShowContactForm { get; set; }

    /// <summary>
    /// Featured state.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Published state.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Publication timestamp if any.
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
