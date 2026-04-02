using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Builds inquiry source information using the current request context.
/// This makes it easy to prefill hidden source fields for ContactMessage creation.
/// </summary>
public class CurrentPageSourceService : ICurrentPageSourceService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentPageSourceService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public PageSourceContext CreateGeneral()
    {
        return new PageSourceContext(
            SourceType: ContactSourceType.General,
            ProductId: null,
            BlogPostId: null,
            SourceTitle: "General Contact",
            PageUrl: GetCurrentRelativeUrl());
    }

    public PageSourceContext CreateForProduct(Product product)
    {
        return new PageSourceContext(
            SourceType: ContactSourceType.Product,
            ProductId: product.Id,
            BlogPostId: null,
            SourceTitle: product.Title,
            PageUrl: GetCurrentRelativeUrl());
    }

    public PageSourceContext CreateForBlogPost(BlogPost blogPost)
    {
        return new PageSourceContext(
            SourceType: ContactSourceType.Blog,
            ProductId: null,
            BlogPostId: blogPost.Id,
            SourceTitle: blogPost.Title,
            PageUrl: GetCurrentRelativeUrl());
    }

    public string? GetCurrentRelativeUrl()
    {
        return _httpContextAccessor.HttpContext?.Request.GetRelativePathAndQuery();
    }
}
