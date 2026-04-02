using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Manages Blog Category CRUD in the Admin area.
/// </summary>
public class BlogCategoriesController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISlugService _slugService;

    public BlogCategoriesController(
        ApplicationDbContext dbContext,
        ISlugService slugService)
    {
        _dbContext = dbContext;
        _slugService = slugService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchTerm,
        bool? isActive,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100);

        var filter = new BlogCategoryAdminListFilterViewModel
        {
            SearchTerm = searchTerm,
            IsActive = isActive,
            SortBy = string.IsNullOrWhiteSpace(sortBy) ? "sort_asc" : sortBy,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = _dbContext.BlogCategories
            .AsNoTracking()
            .Include(x => x.BlogPosts)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = filter.SearchTerm.Trim();

            query = query.Where(x =>
                x.Name.Contains(keyword) ||
                x.Slug.Contains(keyword) ||
                (x.Description != null && x.Description.Contains(keyword)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.IsActive.Value);
        }

        query = filter.SortBy switch
        {
            "name_asc" => query.OrderBy(x => x.Name).ThenBy(x => x.Id),
            "name_desc" => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
            "sort_desc" => query.OrderByDescending(x => x.SortOrder).ThenBy(x => x.Name),
            "newest" => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id),
            "oldest" => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            _ => query.OrderBy(x => x.SortOrder).ThenBy(x => x.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new BlogCategoryAdminListItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                BlogCount = x.BlogPosts.Count,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var activeCount = await _dbContext.BlogCategories
            .AsNoTracking()
            .CountAsync(x => x.IsActive, cancellationToken);

        var model = new BlogCategoryAdminListViewModel
        {
            Filter = filter,
            Items = items,
            TotalCount = totalCount,
            ActiveCount = activeCount,
            Pagination = new PaginationViewModel
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalItems = totalCount,
                BasePath = Url.Action(nameof(Index), "BlogCategories", new { area = "Admin" })
            },
            EmptyState = items.Count == 0
                ? new EmptyStateViewModel
                {
                    Title = "No blog categories found",
                    Message = "Try changing the search or filter options, or create a new category.",
                    ButtonText = "Create Category",
                    ButtonUrl = Url.Action(nameof(Create), "BlogCategories", new { area = "Admin" })
                }
                : null
        };

        ViewData["AdminPageTitle"] = "Blog Categories";
        ViewData["AdminPageDescription"] = "Manage categories used by blog posts.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Blog Categories");

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new BlogCategoryAdminEditViewModel
        {
            IsActive = true,
            SortOrder = 0
        };

        ViewData["AdminPageTitle"] = "Create Blog Category";
        ViewData["AdminPageDescription"] = "Add a new category for blog posts.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Blog Categories", "Create");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BlogCategoryAdminEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            ViewData["AdminPageTitle"] = "Create Blog Category";
            ViewData["AdminPageDescription"] = "Add a new category for blog posts.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Blog Categories", "Create");
            return View(model);
        }

        var requestedSlug = string.IsNullOrWhiteSpace(model.Slug)
            ? model.Name
            : model.Slug;

        var uniqueSlug = await _slugService.GenerateUniqueBlogCategorySlugAsync(
            requestedSlug,
            ignoreId: null,
            cancellationToken: cancellationToken);

        var entity = new BlogCategory
        {
            Name = model.Name.Trim(),
            Slug = uniqueSlug,
            Description = string.IsNullOrWhiteSpace(model.Description)
                ? null
                : model.Description.Trim(),
            SortOrder = model.SortOrder,
            IsActive = model.IsActive
        };

        _dbContext.BlogCategories.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Blog category created successfully.";
        TempData["AdminToastSuccess"] = "Blog category created.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.BlogCategories
            .AsNoTracking()
            .Include(x => x.BlogPosts)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        var model = new BlogCategoryAdminEditViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Slug = entity.Slug,
            Description = entity.Description,
            SortOrder = entity.SortOrder,
            IsActive = entity.IsActive,
            BlogCount = entity.BlogPosts.Count
        };

        ViewData["AdminPageTitle"] = "Edit Blog Category";
        ViewData["AdminPageDescription"] = "Update the category used by blog posts.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Blog Categories", "Edit");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        BlogCategoryAdminEditViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var entity = await _dbContext.BlogCategories
            .Include(x => x.BlogPosts)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        model.BlogCount = entity.BlogPosts.Count;

        if (!ModelState.IsValid)
        {
            ViewData["AdminPageTitle"] = "Edit Blog Category";
            ViewData["AdminPageDescription"] = "Update the category used by blog posts.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Blog Categories", "Edit");
            return View(model);
        }

        var requestedSlug = string.IsNullOrWhiteSpace(model.Slug)
            ? model.Name
            : model.Slug;

        entity.Name = model.Name.Trim();
        entity.Slug = await _slugService.GenerateUniqueBlogCategorySlugAsync(
            requestedSlug,
            ignoreId: entity.Id,
            cancellationToken: cancellationToken);
        entity.Description = string.IsNullOrWhiteSpace(model.Description)
            ? null
            : model.Description.Trim();
        entity.SortOrder = model.SortOrder;
        entity.IsActive = model.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Blog category updated successfully.";
        TempData["AdminToastSuccess"] = "Blog category updated.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
    {
        var model = await _dbContext.BlogCategories
            .AsNoTracking()
            .Include(x => x.BlogPosts)
            .Where(x => x.Id == id)
            .Select(x => new BlogCategoryAdminDetailsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                BlogCount = x.BlogPosts.Count,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (model is null)
        {
            return NotFound();
        }

        ViewData["AdminPageTitle"] = "Blog Category Details";
        ViewData["AdminPageDescription"] = "Review blog category information.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Blog Categories", "Details");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var model = await _dbContext.BlogCategories
            .AsNoTracking()
            .Include(x => x.BlogPosts)
            .Where(x => x.Id == id)
            .Select(x => new BlogCategoryAdminDetailsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                BlogCount = x.BlogPosts.Count,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (model is null)
        {
            return NotFound();
        }

        ViewData["AdminPageTitle"] = "Delete Blog Category";
        ViewData["AdminPageDescription"] = "Delete a blog category if no blog posts are assigned.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Blog Categories", "Delete");

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.BlogCategories
            .Include(x => x.BlogPosts)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        if (entity.BlogPosts.Count > 0)
        {
            TempData["AdminErrorMessage"] = "This category cannot be deleted because blog posts are assigned to it.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        _dbContext.BlogCategories.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Blog category deleted successfully.";
        TempData["AdminToastSuccess"] = "Blog category deleted.";

        return RedirectToAction(nameof(Index));
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
                Url = Url.Action(nameof(Index), "BlogCategories", new { area = "Admin" }),
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
