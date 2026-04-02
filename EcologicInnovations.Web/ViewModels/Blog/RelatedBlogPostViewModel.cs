namespace EcologicInnovations.Web.ViewModels.Blog;

/// <summary>
/// Represents a smaller related article card shown under the blog details page.
/// </summary>
public class RelatedBlogPostViewModel
{
    /// <summary>
    /// Blog post id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Blog post title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Public slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional feature image.
    /// </summary>
    public string? FeatureImageUrl { get; set; }

    /// <summary>
    /// Short excerpt used in the related block.
    /// </summary>
    public string? Excerpt { get; set; }

    /// <summary>
    /// Publish date.
    /// </summary>
    public DateTime? PublishedAt { get; set; }
}
