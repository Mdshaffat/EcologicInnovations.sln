using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Manages Blog CRUD in the Admin area.
/// Blog posts support structured list data, rich HTML content, optional contact form settings,
/// feature image preview, publish scheduling, and SEO metadata.
/// </summary>
public class BlogsController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISlugService _slugService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public BlogsController(
        ApplicationDbContext dbContext,
        ISlugService slugService,
        IHtmlSanitizationService htmlSanitizationService)
    {
        _dbContext = dbContext;
        _slugService = slugService;
        _htmlSanitizationService = htmlSanitizationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchTerm,
        int? blogCategoryId,
        bool? isPublished,
        bool? isFeatured,
        bool? showContactForm,
        bool? hasPublishedAt,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100);

        var filter = new BlogAdminListFilterViewModel
        {
            SearchTerm = searchTerm,
            BlogCategoryId = blogCategoryId,
            IsPublished = isPublished,
            IsFeatured = isFeatured,
            ShowContactForm = showContactForm,
            HasPublishedAt = hasPublishedAt,
            SortBy = string.IsNullOrWhiteSpace(sortBy) ? "published_desc" : sortBy,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = filter.SearchTerm.Trim();

            query = query.Where(x =>
                x.Title.Contains(keyword) ||
                x.Slug.Contains(keyword) ||
                x.Excerpt.Contains(keyword) ||
                (x.BlogCategory != null && x.BlogCategory.Name.Contains(keyword)));
        }

        if (filter.BlogCategoryId.HasValue)
        {
            query = query.Where(x => x.BlogCategoryId == filter.BlogCategoryId.Value);
        }

        if (filter.IsPublished.HasValue)
        {
            query = query.Where(x => x.IsPublished == filter.IsPublished.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(x => x.IsFeatured == filter.IsFeatured.Value);
        }

        if (filter.ShowContactForm.HasValue)
        {
            query = query.Where(x => x.ShowContactForm == filter.ShowContactForm.Value);
        }

        if (filter.HasPublishedAt.HasValue)
        {
            query = filter.HasPublishedAt.Value
                ? query.Where(x => x.PublishedAt != null)
                : query.Where(x => x.PublishedAt == null);
        }

        query = filter.SortBy switch
        {
            "title_asc" => query.OrderBy(x => x.Title).ThenByDescending(x => x.Id),
            "title_desc" => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.Id),
            "newest" => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id),
            "oldest" => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            "published_asc" => query.OrderBy(x => x.PublishedAt ?? DateTime.MaxValue).ThenBy(x => x.Title),
            "featured_first" => query.OrderByDescending(x => x.IsFeatured).ThenByDescending(x => x.PublishedAt ?? x.CreatedAt),
            _ => query.OrderByDescending(x => x.PublishedAt ?? x.CreatedAt).ThenByDescending(x => x.Id)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new BlogAdminListItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.BlogCategory != null ? x.BlogCategory.Name : null,
                FeatureImageUrl = x.FeatureImageUrl,
                Excerpt = x.Excerpt,
                ShowContactForm = x.ShowContactForm,
                IsFeatured = x.IsFeatured,
                IsPublished = x.IsPublished,
                PublishedAt = x.PublishedAt,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var categoryOptions = await BuildCategorySelectListAsync(cancellationToken);

        var publishedCount = await _dbContext.BlogPosts
            .AsNoTracking()
            .CountAsync(x => x.IsPublished, cancellationToken);

        var featuredCount = await _dbContext.BlogPosts
            .AsNoTracking()
            .CountAsync(x => x.IsFeatured, cancellationToken);

        var contactEnabledCount = await _dbContext.BlogPosts
            .AsNoTracking()
            .CountAsync(x => x.ShowContactForm, cancellationToken);

        var model = new BlogAdminListViewModel
        {
            Filter = filter,
            Items = items,
            CategoryOptions = categoryOptions,
            TotalCount = totalCount,
            PublishedCount = publishedCount,
            FeaturedCount = featuredCount,
            ContactEnabledCount = contactEnabledCount,
            Pagination = new PaginationViewModel
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalItems = totalCount,
                BasePath = Url.Action(nameof(Index), "Blogs", new { area = "Admin" })
            },
            EmptyState = items.Count == 0
                ? new EmptyStateViewModel
                {
                    Title = "No articles found",
                    Message = "Try changing the search or filter options, or create a new article.",
                    ButtonText = "Create Article",
                    ButtonUrl = Url.Action(nameof(Create), "Blogs", new { area = "Admin" })
                }
                : null
        };

        ViewData["AdminPageTitle"] = "Articles";
        ViewData["AdminPageDescription"] = "Manage articles, HTML content, publication, and SEO data.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Articles");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var model = new BlogAdminEditViewModel
        {
            IsPublished = true,
            ShowContactForm = false,
            IsFeatured = false,
            PublishedAt = DateTime.UtcNow,
            CategoryOptions = await BuildCategorySelectListAsync(cancellationToken)
        };

        ViewData["AdminPageTitle"] = "Create Article";
        ViewData["AdminPageDescription"] = "Add a new article with HTML content and SEO metadata.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Articles", "Create");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BlogAdminEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        model.CategoryOptions = await BuildCategorySelectListAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            model.HtmlContentPreview = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContent);

            ViewData["AdminPageTitle"] = "Create Article";
            ViewData["AdminPageDescription"] = "Add a new article with HTML content and SEO metadata.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Articles", "Create");

            return View(model);
        }

        var requestedSlug = string.IsNullOrWhiteSpace(model.Slug)
            ? model.Title
            : model.Slug;

        var uniqueSlug = await _slugService.GenerateUniqueBlogPostSlugAsync(
            requestedSlug,
            ignoreId: null,
            cancellationToken: cancellationToken);

        var sanitizedHtml = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContent);

        var entity = new BlogPost
        {
            Title = model.Title.Trim(),
            Slug = uniqueSlug,
            BlogCategoryId = model.BlogCategoryId,
            FeatureImageUrl = string.IsNullOrWhiteSpace(model.FeatureImageUrl)
                ? null
                : model.FeatureImageUrl.Trim(),
            Excerpt = model.Excerpt.Trim(),
            HtmlContent = sanitizedHtml,
            ShowContactForm = model.ShowContactForm,
            ContactFormTitle = string.IsNullOrWhiteSpace(model.ContactFormTitle)
                ? null
                : model.ContactFormTitle.Trim(),
            IsFeatured = model.IsFeatured,
            IsPublished = model.IsPublished,
            PublishedAt = ResolvePublishedAt(model),
            MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle)
                ? null
                : model.MetaTitle.Trim(),
            MetaDescription = string.IsNullOrWhiteSpace(model.MetaDescription)
                ? null
                : model.MetaDescription.Trim(),
            OgImageUrl = string.IsNullOrWhiteSpace(model.OgImageUrl)
                ? null
                : model.OgImageUrl.Trim()
        };

        _dbContext.BlogPosts.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Article created successfully.";
        TempData["AdminToastSuccess"] = "Article created.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.BlogPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        var model = new BlogAdminEditViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Slug = entity.Slug,
            BlogCategoryId = entity.BlogCategoryId,
            FeatureImageUrl = entity.FeatureImageUrl,
            Excerpt = entity.Excerpt,
            HtmlContent = entity.HtmlContent,
            ShowContactForm = entity.ShowContactForm,
            ContactFormTitle = entity.ContactFormTitle,
            IsFeatured = entity.IsFeatured,
            IsPublished = entity.IsPublished,
            PublishedAt = entity.PublishedAt,
            MetaTitle = entity.MetaTitle,
            MetaDescription = entity.MetaDescription,
            OgImageUrl = entity.OgImageUrl,
            CategoryOptions = await BuildCategorySelectListAsync(cancellationToken),
            HtmlContentPreview = _htmlSanitizationService.SanitizeRichHtml(entity.HtmlContent)
        };

        ViewData["AdminPageTitle"] = "Edit Article";
        ViewData["AdminPageDescription"] = "Update the article content, publication, and SEO data.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Articles", "Edit");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        BlogAdminEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var entity = await _dbContext.BlogPosts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        model.CategoryOptions = await BuildCategorySelectListAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            model.HtmlContentPreview = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContent);

            ViewData["AdminPageTitle"] = "Edit Article";
            ViewData["AdminPageDescription"] = "Update the article content, publication, and SEO data.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Articles", "Edit");

            return View(model);
        }

        var requestedSlug = string.IsNullOrWhiteSpace(model.Slug)
            ? model.Title
            : model.Slug;

        entity.Title = model.Title.Trim();
        entity.Slug = await _slugService.GenerateUniqueBlogPostSlugAsync(
            requestedSlug,
            ignoreId: entity.Id,
            cancellationToken: cancellationToken);
        entity.BlogCategoryId = model.BlogCategoryId;
        entity.FeatureImageUrl = string.IsNullOrWhiteSpace(model.FeatureImageUrl)
            ? null
            : model.FeatureImageUrl.Trim();
        entity.Excerpt = model.Excerpt.Trim();
        entity.HtmlContent = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContent);
        entity.ShowContactForm = model.ShowContactForm;
        entity.ContactFormTitle = string.IsNullOrWhiteSpace(model.ContactFormTitle)
            ? null
            : model.ContactFormTitle.Trim();
        entity.IsFeatured = model.IsFeatured;
        entity.IsPublished = model.IsPublished;
        entity.PublishedAt = ResolvePublishedAt(model, entity.PublishedAt);
        entity.MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle)
            ? null
            : model.MetaTitle.Trim();
        entity.MetaDescription = string.IsNullOrWhiteSpace(model.MetaDescription)
            ? null
            : model.MetaDescription.Trim();
        entity.OgImageUrl = string.IsNullOrWhiteSpace(model.OgImageUrl)
            ? null
            : model.OgImageUrl.Trim();

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Article updated successfully.";
        TempData["AdminToastSuccess"] = "Article updated.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
    {
        var model = await _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .Where(x => x.Id == id)
            .Select(x => new BlogAdminDetailsViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.BlogCategory != null ? x.BlogCategory.Name : null,
                FeatureImageUrl = x.FeatureImageUrl,
                Excerpt = x.Excerpt,
                HtmlContentPreview = x.HtmlContent,
                ShowContactForm = x.ShowContactForm,
                ContactFormTitle = x.ContactFormTitle,
                IsFeatured = x.IsFeatured,
                IsPublished = x.IsPublished,
                PublishedAt = x.PublishedAt,
                MetaTitle = x.MetaTitle,
                MetaDescription = x.MetaDescription,
                OgImageUrl = x.OgImageUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (model is null)
        {
            return NotFound();
        }

        model.HtmlContentPreview = _htmlSanitizationService.SanitizeRichHtml(model.HtmlContentPreview);

        ViewData["AdminPageTitle"] = "Article Details";
        ViewData["AdminPageDescription"] = "Review the saved article content and metadata.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Articles", "Details");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var model = await _dbContext.BlogPosts
            .AsNoTracking()
            .Include(x => x.BlogCategory)
            .Where(x => x.Id == id)
            .Select(x => new BlogAdminDetailsViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.BlogCategory != null ? x.BlogCategory.Name : null,
                FeatureImageUrl = x.FeatureImageUrl,
                Excerpt = x.Excerpt,
                ShowContactForm = x.ShowContactForm,
                ContactFormTitle = x.ContactFormTitle,
                IsFeatured = x.IsFeatured,
                IsPublished = x.IsPublished,
                PublishedAt = x.PublishedAt,
                MetaTitle = x.MetaTitle,
                MetaDescription = x.MetaDescription,
                OgImageUrl = x.OgImageUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (model is null)
        {
            return NotFound();
        }

        ViewData["AdminPageTitle"] = "Delete Article";
        ViewData["AdminPageDescription"] = "Delete an article after reviewing its details.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Articles", "Delete");

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.BlogPosts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        _dbContext.BlogPosts.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Article deleted successfully.";
        TempData["AdminToastSuccess"] = "Article deleted.";

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> BuildCategorySelectListAsync(CancellationToken cancellationToken)
    {
        var items = await _dbContext.BlogCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync(cancellationToken);

        items.Insert(0, new SelectListItem
        {
            Value = string.Empty,
            Text = "-- No Category --"
        });

        return items;
    }

    private static DateTime? ResolvePublishedAt(BlogAdminEditViewModel model, DateTime? currentValue = null)
    {
        if (!model.IsPublished)
        {
            return model.PublishedAt;
        }

        if (model.PublishedAt.HasValue)
        {
            return model.PublishedAt.Value;
        }

        return currentValue ?? DateTime.UtcNow;
    }

    private List<BreadcrumbItemViewModel> BuildAdminBreadcrumbs(string sectionTitle, string? currentTitle = null)
    {
        var items = new List<BreadcrumbItemViewModel>
        {
            new()
            {
                Title = "Admin",
                Url = Url.Action("Index", "Dashboard", new { area = "Admin" }),
                IsActive = false
            },
            new()
            {
                Title = sectionTitle,
                Url = Url.Action(nameof(Index), "Blogs", new { area = "Admin" }),
                IsActive = string.IsNullOrWhiteSpace(currentTitle)
            }
        };

        if (!string.IsNullOrWhiteSpace(currentTitle))
        {
            items.Add(new BreadcrumbItemViewModel
            {
                Title = currentTitle,
                Url = null,
                IsActive = true
            });
        }

        return items;
    }
}
