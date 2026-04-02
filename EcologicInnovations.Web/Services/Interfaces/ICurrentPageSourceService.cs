using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Describes the current inquiry source so contact forms can store correct context.
/// </summary>
public interface ICurrentPageSourceService
{
    PageSourceContext CreateGeneral();

    PageSourceContext CreateForProduct(Product product);

    PageSourceContext CreateForBlogPost(BlogPost blogPost);

    string? GetCurrentRelativeUrl();
}

/// <summary>
/// Serializable source context for product, blog, or general inquiries.
/// </summary>
public record PageSourceContext(
    ContactSourceType SourceType,
    int? ProductId,
    int? BlogPostId,
    string? SourceTitle,
    string? PageUrl);
