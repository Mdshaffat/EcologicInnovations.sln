using EcologicInnovations.Web.ViewModels.Contact;
using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Blog;

/// <summary>
/// Main public blog details page model.
/// It carries sanitized article HTML plus the optional inquiry form.
/// </summary>
public class BlogDetailsViewModel
{
    /// <summary>
    /// Blog post id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Public title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO/public slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Optional category name.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Optional category slug.
    /// </summary>
    public string? CategorySlug { get; set; }

    /// <summary>
    /// Feature image URL.
    /// </summary>
    public string? FeatureImageUrl { get; set; }

    /// <summary>
    /// Short excerpt shown near the heading.
    /// </summary>
    public string? Excerpt { get; set; }

    /// <summary>
    /// Sanitized HTML article body.
    /// </summary>
    public string? HtmlContent { get; set; }

    /// <summary>
    /// Publish date for display.
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// Controls whether the inquiry form is shown below the article.
    /// </summary>
    public bool ShowContactForm { get; set; }

    /// <summary>
    /// Optional heading shown above the inquiry form.
    /// </summary>
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// Inquiry form model for the article.
    /// </summary>
    public ContactFormInputModel InquiryForm { get; set; } = new();

    /// <summary>
    /// Related articles shown under the content.
    /// </summary>
    public List<RelatedBlogPostViewModel> RelatedPosts { get; set; } = new();

    /// <summary>
    /// Breadcrumbs for the details page.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();

    /// <summary>
    /// SEO metadata for the page.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();

    /// <summary>
    /// Success flag after a valid inquiry post.
    /// </summary>
    public bool InquirySubmittedSuccessfully { get; set; }

    /// <summary>
    /// Success message shown above the inquiry form.
    /// </summary>
    public string? SuccessMessage { get; set; }
}
