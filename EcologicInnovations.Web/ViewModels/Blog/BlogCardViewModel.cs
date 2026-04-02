namespace EcologicInnovations.Web.ViewModels.Blog;

/// <summary>
/// Lightweight card model used for the public blog list page, home page sections,
/// and related blog blocks.
/// </summary>
public class BlogCardViewModel
{
    /// <summary>
    /// Blog post primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public article title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly slug used for the details page route.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional category name shown as a badge or label.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Optional category slug for list filtering links.
    /// </summary>
    public string? CategorySlug { get; set; }

    /// <summary>
    /// Public feature image URL.
    /// </summary>
    public string? FeatureImageUrl { get; set; }

    /// <summary>
    /// Short article excerpt shown on cards.
    /// </summary>
    public string? Excerpt { get; set; }

    /// <summary>
    /// Indicates whether the article is featured.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Public publish date.
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// Optional precomputed details URL.
    /// </summary>
    public string? DetailsUrl { get; set; }
}
