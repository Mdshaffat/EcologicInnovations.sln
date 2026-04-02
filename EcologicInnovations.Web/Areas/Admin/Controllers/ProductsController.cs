using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Manages Product CRUD in the Admin area.
/// Products support structured list data, rich HTML details, product-menu dropdown configuration,
/// contact form settings, and SEO metadata.
/// </summary>
public class ProductsController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISlugService _slugService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public ProductsController(
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
        int? productCategoryId,
        bool? isPublished,
        bool? isActive,
        bool? isFeatured,
        bool? showInProductMenu,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 100);

        var filter = new ProductAdminListFilterViewModel
        {
            SearchTerm = searchTerm,
            ProductCategoryId = productCategoryId,
            IsPublished = isPublished,
            IsActive = isActive,
            IsFeatured = isFeatured,
            ShowInProductMenu = showInProductMenu,
            SortBy = string.IsNullOrWhiteSpace(sortBy) ? "listsort_asc" : sortBy,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = filter.SearchTerm.Trim();

            query = query.Where(x =>
                x.Title.Contains(keyword) ||
                x.Slug.Contains(keyword) ||
                x.ShortDescription.Contains(keyword) ||
                x.ProductCategory.Name.Contains(keyword));
        }

        if (filter.ProductCategoryId.HasValue)
        {
            query = query.Where(x => x.ProductCategoryId == filter.ProductCategoryId.Value);
        }

        if (filter.IsPublished.HasValue)
        {
            query = query.Where(x => x.IsPublished == filter.IsPublished.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.IsActive.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(x => x.IsFeatured == filter.IsFeatured.Value);
        }

        if (filter.ShowInProductMenu.HasValue)
        {
            query = query.Where(x => x.ShowInProductMenu == filter.ShowInProductMenu.Value);
        }

        query = filter.SortBy switch
        {
            "title_asc" => query.OrderBy(x => x.Title).ThenBy(x => x.Id),
            "title_desc" => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.Id),
            "listsort_desc" => query.OrderByDescending(x => x.ListSortOrder).ThenBy(x => x.Title),
            "newest" => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id),
            "oldest" => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            "menu_asc" => query.OrderBy(x => x.MenuSortOrder).ThenBy(x => x.Title),
            "menu_desc" => query.OrderByDescending(x => x.MenuSortOrder).ThenBy(x => x.Title),
            _ => query.OrderBy(x => x.ListSortOrder).ThenBy(x => x.Title)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new ProductAdminListItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.ProductCategory.Name,
                MainImageUrl = x.MainImageUrl,
                ShortDescription = x.ShortDescription,
                ContactFormEnabled = x.ContactFormEnabled,
                ShowInProductMenu = x.ShowInProductMenu,
                MenuSortOrder = x.MenuSortOrder,
                ListSortOrder = x.ListSortOrder,
                IsFeatured = x.IsFeatured,
                IsPublished = x.IsPublished,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var categoryOptions = await GetCategorySelectListAsync(filter.ProductCategoryId, cancellationToken, includeAllOption: true);

        var publishedCountTask = _dbContext.Products
            .AsNoTracking()
            .CountAsync(x => x.IsPublished, cancellationToken);

        var featuredCountTask = _dbContext.Products
            .AsNoTracking()
            .CountAsync(x => x.IsFeatured, cancellationToken);

        var menuProductCountTask = _dbContext.Products
            .AsNoTracking()
            .CountAsync(x => x.ShowInProductMenu, cancellationToken);

        await Task.WhenAll(publishedCountTask, featuredCountTask, menuProductCountTask);

        var model = new ProductAdminListViewModel
        {
            Filter = filter,
            Items = items,
            CategoryOptions = categoryOptions,
            TotalCount = totalCount,
            PublishedCount = publishedCountTask.Result,
            FeaturedCount = featuredCountTask.Result,
            MenuProductCount = menuProductCountTask.Result,
            Pagination = new PaginationViewModel
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalItems = totalCount,
                BasePath = Url.Action(nameof(Index), "Products", new { area = "Admin" })
            },
            EmptyState = items.Count == 0
                ? new EmptyStateViewModel
                {
                    Title = "No products found",
                    Message = "Try changing the search or filter options, or create a new product.",
                    ButtonText = "Create Product",
                    ButtonUrl = Url.Action(nameof(Create), "Products", new { area = "Admin" })
                }
                : null
        };

        ViewData["AdminPageTitle"] = "Products";
        ViewData["AdminPageDescription"] = "Manage products, rich HTML details, menu dropdown items, and SEO fields.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Products");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new ProductAdminEditViewModel
        {
            ContactFormEnabled = true,
            IsActive = true,
            IsPublished = false,
            ListSortOrder = 0,
            MenuSortOrder = 0,
            CategoryOptions = await GetCategorySelectListAsync(null, cancellationToken)
        };

        ViewData["AdminPageTitle"] = "Create Product";
        ViewData["AdminPageDescription"] = "Add a new product with HTML details, contact form options, menu settings, and SEO metadata.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Create");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductAdminEditViewModel model, CancellationToken cancellationToken)
    {
        await PopulateCategoryOptionsAsync(model, cancellationToken);

        if (!await ProductCategoryExistsAsync(model.ProductCategoryId, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.ProductCategoryId), "Selected category does not exist.");
        }

        if (!ModelState.IsValid)
        {
            model.HtmlDetailsPreview = _htmlSanitizationService.SanitizeRichHtml(model.HtmlDetails);

            ViewData["AdminPageTitle"] = "Create Product";
            ViewData["AdminPageDescription"] = "Add a new product with HTML details, contact form options, menu settings, and SEO metadata.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Create");

            return View(model);
        }

        var entity = new Product
        {
            Title = model.Title.Trim(),
            Slug = await _slugService.GenerateUniqueProductSlugAsync(
                string.IsNullOrWhiteSpace(model.Slug) ? model.Title : model.Slug,
                cancellationToken: cancellationToken),
            ProductCategoryId = model.ProductCategoryId,
            ShortDescription = model.ShortDescription.Trim(),
            MainImageUrl = CleanOrNull(model.MainImageUrl),
            HtmlDetails = _htmlSanitizationService.SanitizeRichHtml(model.HtmlDetails),
            ContactFormEnabled = model.ContactFormEnabled,
            ContactFormTitle = CleanOrNull(model.ContactFormTitle),
            ShowInProductMenu = model.ShowInProductMenu,
            MenuSortOrder = model.MenuSortOrder,
            ListSortOrder = model.ListSortOrder,
            IsFeatured = model.IsFeatured,
            IsPublished = model.IsPublished,
            IsActive = model.IsActive,
            MetaTitle = CleanOrNull(model.MetaTitle),
            MetaDescription = CleanOrNull(model.MetaDescription),
            OgImageUrl = CleanOrNull(model.OgImageUrl)
        };

        _dbContext.Products.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Product created successfully.";
        TempData["AdminToastSuccess"] = "Product created.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        var model = new ProductAdminEditViewModel
        {
            Id = product.Id,
            Title = product.Title,
            Slug = product.Slug,
            ProductCategoryId = product.ProductCategoryId,
            ShortDescription = product.ShortDescription,
            MainImageUrl = product.MainImageUrl,
            HtmlDetails = product.HtmlDetails,
            ContactFormEnabled = product.ContactFormEnabled,
            ContactFormTitle = product.ContactFormTitle,
            ShowInProductMenu = product.ShowInProductMenu,
            MenuSortOrder = product.MenuSortOrder,
            ListSortOrder = product.ListSortOrder,
            IsFeatured = product.IsFeatured,
            IsPublished = product.IsPublished,
            IsActive = product.IsActive,
            MetaTitle = product.MetaTitle,
            MetaDescription = product.MetaDescription,
            OgImageUrl = product.OgImageUrl,
            HtmlDetailsPreview = _htmlSanitizationService.SanitizeRichHtml(product.HtmlDetails),
            CategoryOptions = await GetCategorySelectListAsync(product.ProductCategoryId, cancellationToken)
        };

        ViewData["AdminPageTitle"] = "Edit Product";
        ViewData["AdminPageDescription"] = "Update product content, display settings, contact form behavior, and SEO metadata.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Edit");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductAdminEditViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        await PopulateCategoryOptionsAsync(model, cancellationToken);

        if (!await ProductCategoryExistsAsync(model.ProductCategoryId, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.ProductCategoryId), "Selected category does not exist.");
        }

        if (!ModelState.IsValid)
        {
            model.HtmlDetailsPreview = _htmlSanitizationService.SanitizeRichHtml(model.HtmlDetails);

            ViewData["AdminPageTitle"] = "Edit Product";
            ViewData["AdminPageDescription"] = "Update product content, display settings, contact form behavior, and SEO metadata.";
            ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Edit");

            return View(model);
        }

        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        product.Title = model.Title.Trim();
        product.Slug = await _slugService.GenerateUniqueProductSlugAsync(
            string.IsNullOrWhiteSpace(model.Slug) ? model.Title : model.Slug,
            ignoreId: product.Id,
            cancellationToken: cancellationToken);
        product.ProductCategoryId = model.ProductCategoryId;
        product.ShortDescription = model.ShortDescription.Trim();
        product.MainImageUrl = CleanOrNull(model.MainImageUrl);
        product.HtmlDetails = _htmlSanitizationService.SanitizeRichHtml(model.HtmlDetails);
        product.ContactFormEnabled = model.ContactFormEnabled;
        product.ContactFormTitle = CleanOrNull(model.ContactFormTitle);
        product.ShowInProductMenu = model.ShowInProductMenu;
        product.MenuSortOrder = model.MenuSortOrder;
        product.ListSortOrder = model.ListSortOrder;
        product.IsFeatured = model.IsFeatured;
        product.IsPublished = model.IsPublished;
        product.IsActive = model.IsActive;
        product.MetaTitle = CleanOrNull(model.MetaTitle);
        product.MetaDescription = CleanOrNull(model.MetaDescription);
        product.OgImageUrl = CleanOrNull(model.OgImageUrl);

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Product updated successfully.";
        TempData["AdminToastSuccess"] = "Product updated.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        var model = new ProductAdminDetailsViewModel
        {
            Id = product.Id,
            Title = product.Title,
            Slug = product.Slug,
            CategoryName = product.ProductCategory.Name,
            ShortDescription = product.ShortDescription,
            MainImageUrl = product.MainImageUrl,
            HtmlDetailsPreview = _htmlSanitizationService.SanitizeRichHtml(product.HtmlDetails),
            ContactFormEnabled = product.ContactFormEnabled,
            ContactFormTitle = product.ContactFormTitle,
            ShowInProductMenu = product.ShowInProductMenu,
            MenuSortOrder = product.MenuSortOrder,
            ListSortOrder = product.ListSortOrder,
            IsFeatured = product.IsFeatured,
            IsPublished = product.IsPublished,
            IsActive = product.IsActive,
            MetaTitle = product.MetaTitle,
            MetaDescription = product.MetaDescription,
            OgImageUrl = product.OgImageUrl,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
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
                    Title = "Products",
                    Url = Url.Action("Index", "Products", new { area = "Admin" }),
                    IsActive = false
                },
                new BreadcrumbItemViewModel
                {
                    Title = product.Title,
                    Url = null,
                    IsActive = true
                }
            ]
        };

        ViewData["AdminPageTitle"] = "Product Details";
        ViewData["AdminPageDescription"] = "Review product information, HTML details, SEO settings, and menu behavior.";
        ViewData["AdminBreadcrumbs"] = model.Breadcrumbs;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        var model = new ProductAdminDetailsViewModel
        {
            Id = product.Id,
            Title = product.Title,
            Slug = product.Slug,
            CategoryName = product.ProductCategory.Name,
            ShortDescription = product.ShortDescription,
            MainImageUrl = product.MainImageUrl,
            HtmlDetailsPreview = _htmlSanitizationService.SanitizeRichHtml(product.HtmlDetails),
            ContactFormEnabled = product.ContactFormEnabled,
            ContactFormTitle = product.ContactFormTitle,
            ShowInProductMenu = product.ShowInProductMenu,
            MenuSortOrder = product.MenuSortOrder,
            ListSortOrder = product.ListSortOrder,
            IsFeatured = product.IsFeatured,
            IsPublished = product.IsPublished,
            IsActive = product.IsActive,
            MetaTitle = product.MetaTitle,
            MetaDescription = product.MetaDescription,
            OgImageUrl = product.OgImageUrl,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };

        ViewData["AdminPageTitle"] = "Delete Product";
        ViewData["AdminPageDescription"] = "Confirm deletion of the selected product.";
        ViewData["AdminBreadcrumbs"] = BuildAdminBreadcrumbs("Delete");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            TempData["AdminErrorMessage"] = "Product not found.";
            TempData["AdminToastError"] = "Delete failed.";

            return RedirectToAction(nameof(Index));
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Product deleted successfully.";
        TempData["AdminToastSuccess"] = "Product deleted.";

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoryOptionsAsync(ProductAdminEditViewModel model, CancellationToken cancellationToken)
    {
        model.CategoryOptions = await GetCategorySelectListAsync(model.ProductCategoryId, cancellationToken);
    }

    private async Task<List<SelectListItem>> GetCategorySelectListAsync(
        int? selectedValue,
        CancellationToken cancellationToken,
        bool includeAllOption = false)
    {
        var options = new List<SelectListItem>();

        if (includeAllOption)
        {
            options.Add(new SelectListItem
            {
                Text = "All Categories",
                Value = string.Empty,
                Selected = !selectedValue.HasValue
            });
        }

        var categories = await _dbContext.ProductCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = selectedValue.HasValue && x.Id == selectedValue.Value
            })
            .ToListAsync(cancellationToken);

        options.AddRange(categories);
        return options;
    }

    private async Task<bool> ProductCategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProductCategories
            .AsNoTracking()
            .AnyAsync(x => x.Id == categoryId && x.IsActive, cancellationToken);
    }

    private static string? CleanOrNull(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
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
                Title = "Products",
                Url = Url.Action("Index", "Products", new { area = "Admin" }),
                IsActive = currentTitle == "Products"
            },
            new BreadcrumbItemViewModel
            {
                Title = currentTitle,
                Url = currentTitle == "Products" ? Url.Action("Index", "Products", new { area = "Admin" }) : null,
                IsActive = currentTitle != "Products"
            }
        ];
    }
}
