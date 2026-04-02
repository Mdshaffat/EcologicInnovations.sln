using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Manages Product Category CRUD in the Admin area.
/// Categories are used by products, public filters, and future category SEO structure.
/// </summary>
public class ProductCategoriesController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISlugService _slugService;

    public ProductCategoriesController(
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

        var filter = new ProductCategoryAdminListFilterViewModel
        {
            SearchTerm = searchTerm,
            IsActive = isActive,
            SortBy = string.IsNullOrWhiteSpace(sortBy) ? "sort_asc" : sortBy,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = _dbContext.ProductCategories
            .AsNoTracking()
            .Include(x => x.Products)
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
            .Select(x => new ProductCategoryAdminListItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                ProductCount = x.Products.Count,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var activeCount = await _dbContext.ProductCategories
            .AsNoTracking()
            .CountAsync(x => x.IsActive, cancellationToken);

        var model = new ProductCategoryAdminListViewModel
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
                BasePath = Url.Action(nameof(Index), "ProductCategories", new { area = "Admin" })
            },
            EmptyState = items.Count == 0
                ? new EmptyStateViewModel
                {
                    Title = "No product categories found",
                    Message = "Try changing the search or filter options, or create a new category.",
                    ButtonText = "Create Category",
                    ButtonUrl = Url.Action(nameof(Create), "ProductCategories", new { area = "Admin" })
                }
                : null
        };

        ViewData["AdminPageTitle"] = "Product Categories";
        ViewData["AdminPageDescription"] = "Manage product categories used by products and public filters.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Product Categories");

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new ProductCategoryAdminEditViewModel
        {
            IsActive = true,
            SortOrder = 0
        };

        ViewData["AdminPageTitle"] = "Create Product Category";
        ViewData["AdminPageDescription"] = "Add a new product category for products and public filters.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Create");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCategoryAdminEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ViewData["AdminPageTitle"] = "Create Product Category";
            ViewData["AdminPageDescription"] = "Add a new product category for products and public filters.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Create");

            return View(model);
        }

        var finalSlug = await _slugService.GenerateUniqueProductCategorySlugAsync(
            string.IsNullOrWhiteSpace(model.Slug) ? model.Name : model.Slug,
            cancellationToken: cancellationToken);

        var entity = new ProductCategory
        {
            Name = model.Name.Trim(),
            Slug = finalSlug,
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            SortOrder = model.SortOrder,
            IsActive = model.IsActive
        };

        _dbContext.ProductCategories.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Product category created successfully.";
        TempData["AdminToastSuccess"] = "Category created.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var category = await _dbContext.ProductCategories
            .AsNoTracking()
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound();
        }

        var model = new ProductCategoryAdminEditViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            SortOrder = category.SortOrder,
            IsActive = category.IsActive,
            ProductCount = category.Products.Count
        };

        ViewData["AdminPageTitle"] = "Edit Product Category";
        ViewData["AdminPageDescription"] = "Update category information, slug, and public availability.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Edit");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductCategoryAdminEditViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            ViewData["AdminPageTitle"] = "Edit Product Category";
            ViewData["AdminPageDescription"] = "Update category information, slug, and public availability.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Edit");

            return View(model);
        }

        var category = await _dbContext.ProductCategories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound();
        }

        category.Name = model.Name.Trim();
        category.Slug = await _slugService.GenerateUniqueProductCategorySlugAsync(
            string.IsNullOrWhiteSpace(model.Slug) ? model.Name : model.Slug,
            ignoreId: category.Id,
            cancellationToken: cancellationToken);
        category.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        category.SortOrder = model.SortOrder;
        category.IsActive = model.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Product category updated successfully.";
        TempData["AdminToastSuccess"] = "Category updated.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var category = await _dbContext.ProductCategories
            .AsNoTracking()
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound();
        }

        var model = new ProductCategoryAdminDetailsViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            SortOrder = category.SortOrder,
            IsActive = category.IsActive,
            ProductCount = category.Products.Count,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            Breadcrumbs =
            [
                new BreadcrumbItemViewModel
                {
                    Title = "Admin",
                    Url = Url.Action("Index", "Dashboard", new { area = "Admin" }),
                    IsActive = false
                },
                new BreadcrumbItemViewModel
                {
                    Title = "Product Categories",
                    Url = Url.Action(nameof(Index), "ProductCategories", new { area = "Admin" }),
                    IsActive = false
                },
                new BreadcrumbItemViewModel
                {
                    Title = category.Name,
                    Url = null,
                    IsActive = true
                }
            ]
        };

        ViewData["AdminPageTitle"] = "Category Details";
        ViewData["AdminPageDescription"] = "Review category metadata and current product usage.";
        ViewData["AdminBreadcrumbs"] = model.Breadcrumbs;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var category = await _dbContext.ProductCategories
            .AsNoTracking()
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return NotFound();
        }

        var model = new ProductCategoryAdminDetailsViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            SortOrder = category.SortOrder,
            IsActive = category.IsActive,
            ProductCount = category.Products.Count,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };

        ViewData["AdminPageTitle"] = "Delete Product Category";
        ViewData["AdminPageDescription"] = "Confirm deletion. Categories with linked products cannot be deleted.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Delete");

        return View(model);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var category = await _dbContext.ProductCategories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            TempData["AdminWarningMessage"] = "The category was not found.";
            return RedirectToAction(nameof(Index));
        }

        if (category.Products.Any())
        {
            TempData["AdminErrorMessage"] = "This category cannot be deleted because products are already assigned to it.";
            TempData["AdminToastError"] = "Delete blocked.";

            return RedirectToAction(nameof(Delete), new { id });
        }

        _dbContext.ProductCategories.Remove(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Product category deleted successfully.";
        TempData["AdminToastSuccess"] = "Category deleted.";

        return RedirectToAction(nameof(Index));
    }

    private List<BreadcrumbItemViewModel> BuildAdminBreadcrumbs(string currentTitle)
    {
        return
        [
            new BreadcrumbItemViewModel
            {
                Title = "Admin",
                Url = Url.Action("Index", "Dashboard", new { area = "Admin" }),
                IsActive = false
            },
            new BreadcrumbItemViewModel
            {
                Title = "Product Categories",
                Url = Url.Action(nameof(Index), "ProductCategories", new { area = "Admin" }),
                IsActive = currentTitle == "Product Categories"
            },
            new BreadcrumbItemViewModel
            {
                Title = currentTitle,
                Url = currentTitle == "Product Categories" ? null : null,
                IsActive = true
            }
        ];
    }
}
