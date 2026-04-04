using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Blog;
using EcologicInnovations.Web.ViewModels.Contact;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Controllers;

/// <summary>
/// Public blog controller.
/// It implements the blog list page, blog details page, and optional blog inquiry workflow.
/// </summary>
[Route("blog")]
public class BlogController : Controller
{
    private const int DefaultPageSize = 9;
    private const int RelatedPostsCount = 3;

    private readonly ApplicationDbContext _dbContext;
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly ICanonicalUrlService _canonicalUrlService;
    private readonly ICurrentPageSourceService _currentPageSourceService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public BlogController(
        ApplicationDbContext dbContext,
        ISiteSettingsService siteSettingsService,
        ICanonicalUrlService canonicalUrlService,
        ICurrentPageSourceService currentPageSourceService,
        IHtmlSanitizationService htmlSanitizationService)
    {
        _dbContext = dbContext;
        _siteSettingsService = siteSettingsService;
        _canonicalUrlService = canonicalUrlService;
        _currentPageSourceService = currentPageSourceService;
        _htmlSanitizationService = htmlSanitizationService;
    }

    /// <summary>
    /// Public blog list page with category filtering, keyword search, sorting, and pagination.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? category,
        string? q,
        string? sort,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;

        var selectedCategorySlug = string.IsNullOrWhiteSpace(category)
            ? null
            : category.Trim().ToLowerInvariant();

        var searchTerm = string.IsNullOrWhiteSpace(q)
            ? null
            : q.Trim();

        var sortBy = NormalizeSort(sort);

        var selectedCategory = await _dbContext.BlogCategories
            .AsNoTracking()
            .Where(x => x.IsActive && x.Slug == selectedCategorySlug)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug
            })
            .FirstOrDefaultAsync(cancellationToken);

        var baseQuery = _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .Where(x => x.IsPublished);

        if (selectedCategory is not null)
        {
            baseQuery = baseQuery.Where(x => x.BlogCategoryId == selectedCategory.Id);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            baseQuery = baseQuery.Where(x =>
                x.Title.Contains(searchTerm) ||
                (x.Excerpt != null && x.Excerpt.Contains(searchTerm)) ||
                (x.MetaTitle != null && x.MetaTitle.Contains(searchTerm)) ||
                (x.MetaDescription != null && x.MetaDescription.Contains(searchTerm)) ||
                (x.BlogCategory != null && x.BlogCategory.Name.Contains(searchTerm)));
        }

        baseQuery = ApplySorting(baseQuery, sortBy);

        var totalResults = await baseQuery.CountAsync(cancellationToken);

        var posts = await baseQuery
            .Skip((page - 1) * DefaultPageSize)
            .Take(DefaultPageSize)
            .Select(x => new BlogCardViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.BlogCategory != null ? x.BlogCategory.Name : null,
                CategorySlug = x.BlogCategory != null ? x.BlogCategory.Slug : null,
                FeatureImageUrl = x.FeatureImageUrl,
                Excerpt = x.Excerpt,
                IsFeatured = x.IsFeatured,
                PublishedAt = x.PublishedAt,
                DetailsUrl = Url.Action(nameof(Details), "Blog", new { slug = x.Slug })
            })
            .ToListAsync(cancellationToken);

        var categoryCounts = await _dbContext.BlogCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new BlogCategoryFilterItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                BlogCount = x.BlogPosts.Count(p => p.IsPublished),
                IsSelected = selectedCategorySlug != null && x.Slug == selectedCategorySlug
            })
            .ToListAsync(cancellationToken);

        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);

        var pageTitle = selectedCategory is not null
            ? $"{selectedCategory.Name} Articles"
            : "Articles";

        var introText = selectedCategory is not null
            ? $"Explore {selectedCategory.Name.ToLowerInvariant()} insights, updates, and practical knowledge from Ecologic Innovations."
            : "Read practical articles, solution insights, sustainability knowledge, and product-related updates from Ecologic Innovations.";

        var hasFilterState =
            !string.IsNullOrWhiteSpace(searchTerm) ||
            !string.IsNullOrWhiteSpace(selectedCategorySlug) ||
            page > 1 ||
            !string.Equals(sortBy, "newest", StringComparison.OrdinalIgnoreCase);

        var seo = new SeoMetaViewModel
        {
            Title = selectedCategory is not null
                ? $"{selectedCategory.Name} Articles | {siteSettings.CompanyName}"
                : $"Articles | {siteSettings.CompanyName}",
            Description = selectedCategory is not null
                ? $"Browse published articles in the {selectedCategory.Name} category from {siteSettings.CompanyName}."
                : siteSettings.MetaDescriptionDefault ?? "Explore the latest articles and insights from Ecologic Innovations.",
            CanonicalUrl = _canonicalUrlService.BuildCanonicalUrl(),
            Robots = hasFilterState ? "noindex,follow" : "index,follow"
        };

        var model = new BlogListViewModel
        {
            PageTitle = pageTitle,
            IntroText = introText,
            Posts = posts,
            ResultCount = totalResults,
            SelectedCategoryName = selectedCategory?.Name,
            Breadcrumbs = BreadcrumbBuilder.CreateForBlog(),
            Seo = seo,
            Sidebar = new BlogFilterSidebarViewModel
            {
                Categories = categoryCounts,
                SearchTerm = searchTerm,
                SelectedCategoryId = selectedCategory?.Id,
                SelectedCategorySlug = selectedCategory?.Slug,
                SortBy = sortBy,
                SortOptions = BuildSortOptions(sortBy),
                ClearFilterUrl = Url.Action(nameof(Index), "Blog")
            },
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = DefaultPageSize,
                TotalItems = totalResults,
                BasePath = Url.Action(nameof(Index), "Blog")
            },
            EmptyState = posts.Count == 0
                ? new EmptyStateViewModel
                {
                    Title = "No articles found",
                    Message = "Try another category or search keyword.",
                    ButtonText = "View All Articles",
                    ButtonUrl = Url.Action(nameof(Index), "Blog")
                }
                : null
        };

        ViewData["Seo"] = model.Seo;
        return View(model);
    }

    /// <summary>
    /// Public blog details page routed by slug.
    /// </summary>
    [HttpGet("{slug}")]
    public async Task<IActionResult> Details(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var post = await _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == slug, cancellationToken);

        if (post is null)
        {
            return NotFound();
        }

        var model = await BuildDetailsViewModelAsync(post.Id, cancellationToken);
        ViewData["Seo"] = model.Seo;
        return View(model);
    }

    /// <summary>
    /// Handles optional inquiry form submission from the blog details page.
    /// </summary>
    [HttpPost("{slug}/inquiry")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Inquiry(
        string slug,
        [Bind(Prefix = "InquiryForm")] ContactFormInputModel input,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var post = await _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == slug, cancellationToken);

        if (post is null)
        {
            return NotFound();
        }

        input.SourceType = ContactSourceType.Blog;
        input.BlogPostId = post.Id;
        input.SourceTitle = post.Title;
        input.RegardingBlogTitle = post.Title;
        input.PageUrl = _currentPageSourceService.GetCurrentRelativeUrl() ?? $"/blog/{post.Slug}";

        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildDetailsViewModelAsync(post.Id, cancellationToken, input);
            invalidModel.InquirySubmittedSuccessfully = false;
            ViewData["Seo"] = invalidModel.Seo;
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View(nameof(Details), invalidModel);
        }

        var entity = new Models.Entities.ContactMessage
        {
            Name = input.Name.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            Company = string.IsNullOrWhiteSpace(input.Company) ? null : input.Company.Trim(),
            Subject = string.IsNullOrWhiteSpace(input.Subject) ? null : input.Subject.Trim(),
            Message = input.Message.Trim(),
            SourceType = ContactSourceType.Blog,
            BlogPostId = post.Id,
            SourceTitle = post.Title,
            PageUrl = input.PageUrl,
            Status = ContactMessageStatus.New,
            SubmitterIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            SubmitterUserAgent = Request.Headers.UserAgent.ToString() is { Length: > 0 } ua
                ? (ua.Length > 512 ? ua[..512] : ua)
                : null
        };

        _dbContext.ContactMessages.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["BlogInquirySuccess"] = "Thank you. Your message has been sent successfully. Our team will contact you soon.";
        return RedirectToAction(nameof(Details), new { slug });
    }

    private async Task<BlogDetailsViewModel> BuildDetailsViewModelAsync(
        int blogPostId,
        CancellationToken cancellationToken,
        ContactFormInputModel? input = null)
    {
        var post = await _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .FirstAsync(x => x.Id == blogPostId, cancellationToken);

        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);

        var relatedPosts = await _dbContext.BlogPosts
            .AsNoTracking()
            .Where(x => x.IsPublished && x.Id != post.Id)
            .OrderByDescending(x => x.IsFeatured)
            .ThenByDescending(x => x.PublishedAt)
            .Take(RelatedPostsCount)
            .Select(x => new RelatedBlogPostViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                FeatureImageUrl = x.FeatureImageUrl,
                Excerpt = x.Excerpt,
                PublishedAt = x.PublishedAt
            })
            .ToListAsync(cancellationToken);

        var inquiryModel = input ?? new ContactFormInputModel
        {
            SourceType = ContactSourceType.Blog,
            BlogPostId = post.Id,
            SourceTitle = post.Title,
            PageUrl = $"/blog/{post.Slug}",
            RegardingBlogTitle = post.Title
        };

        var successMessage = TempData["BlogInquirySuccess"]?.ToString();

        return new BlogDetailsViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            CategoryName = post.BlogCategory?.Name,
            CategorySlug = post.BlogCategory?.Slug,
            FeatureImageUrl = post.FeatureImageUrl,
            Excerpt = post.Excerpt,
            HtmlContent = _htmlSanitizationService.SanitizeRichHtml(post.HtmlContent),
            PublishedAt = post.PublishedAt,
            ShowContactForm = post.ShowContactForm,
            ContactFormTitle = post.ContactFormTitle,
            InquiryForm = inquiryModel,
            RelatedPosts = relatedPosts,
            Breadcrumbs = BreadcrumbBuilder.CreateForBlogDetails(post.Title),
            InquirySubmittedSuccessfully = !string.IsNullOrWhiteSpace(successMessage),
            SuccessMessage = successMessage,
            Seo = new SeoMetaViewModel
            {
                Title = !string.IsNullOrWhiteSpace(post.MetaTitle)
                    ? post.MetaTitle
                    : $"{post.Title} | {siteSettings.CompanyName}",
                Description = !string.IsNullOrWhiteSpace(post.MetaDescription)
                    ? post.MetaDescription
                    : post.Excerpt ?? siteSettings.MetaDescriptionDefault,
                CanonicalUrl = _canonicalUrlService.BuildCanonicalUrl($"/blog/{post.Slug}"),
                OgTitle = !string.IsNullOrWhiteSpace(post.MetaTitle) ? post.MetaTitle : post.Title,
                OgDescription = !string.IsNullOrWhiteSpace(post.MetaDescription)
                    ? post.MetaDescription
                    : post.Excerpt,
                OgImageUrl = post.OgImageUrl ?? post.FeatureImageUrl,
                Robots = "index,follow"
            }
        };
    }

    private static IQueryable<Models.Entities.BlogPost> ApplySorting(
        IQueryable<Models.Entities.BlogPost> query,
        string sortBy)
    {
        return sortBy switch
        {
            "oldest" => query.OrderBy(x => x.PublishedAt).ThenBy(x => x.Title),
            "title_asc" => query.OrderBy(x => x.Title),
            "title_desc" => query.OrderByDescending(x => x.Title),
            "featured" => query.OrderByDescending(x => x.IsFeatured).ThenByDescending(x => x.PublishedAt),
            _ => query.OrderByDescending(x => x.PublishedAt).ThenByDescending(x => x.Id)
        };
    }

    private static string NormalizeSort(string? sort)
    {
        return sort?.Trim().ToLowerInvariant() switch
        {
            "oldest" => "oldest",
            "title_asc" => "title_asc",
            "title_desc" => "title_desc",
            "featured" => "featured",
            _ => "newest"
        };
    }

    private static List<SelectListItem> BuildSortOptions(string current)
    {
        return
        [
            new SelectListItem { Text = "Newest", Value = "newest", Selected = current == "newest" },
            new SelectListItem { Text = "Featured", Value = "featured", Selected = current == "featured" },
            new SelectListItem { Text = "Oldest", Value = "oldest", Selected = current == "oldest" },
            new SelectListItem { Text = "Title (A-Z)", Value = "title_asc", Selected = current == "title_asc" },
            new SelectListItem { Text = "Title (Z-A)", Value = "title_desc", Selected = current == "title_desc" }
        ];
    }
}
