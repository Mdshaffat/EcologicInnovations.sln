namespace EcologicInnovations.Web.ViewModels.Blog;

/// <summary>
/// Represents one blog category item shown in the public blog sidebar filter.
/// </summary>
public class BlogCategoryFilterItemViewModel
{
    /// <summary>
    /// Blog category id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Blog category display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly category slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Count of matching published blog posts.
    /// </summary>
    public int BlogCount { get; set; }

    /// <summary>
    /// True when this category is currently selected.
    /// </summary>
    public bool IsSelected { get; set; }
}
