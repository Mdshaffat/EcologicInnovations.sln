using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Contact;
using EcologicInnovations.Web.ViewModels.Products;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Controllers;

/// <summary>
/// Public products controller.
/// This file implements both the public products list page and the product details page with inquiry handling.
/// </summary>
[Route("products")]
public class ProductsController : Controller
{
    private const int DefaultPageSize = 12;
    private const int RelatedProductsCount = 4;

    private readonly ApplicationDbContext _dbContext;
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly ICanonicalUrlService _canonicalUrlService;
    private readonly ICurrentPageSourceService _currentPageSourceService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public ProductsController(
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
    /// Public products listing page.
    /// Supports category filtering, keyword search, sorting, and pagination using querystring parameters.
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

        var selectedCategory = await _dbContext.ProductCategories
            .AsNoTracking()
            .Where(x => x.IsActive && x.Slug == selectedCategorySlug)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug
            })
            .FirstOrDefaultAsync(cancellationToken);

        var baseQuery = _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .Where(x => x.IsActive && x.IsPublished && x.ProductCategory.IsActive);

        if (selectedCategory is not null)
        {
            baseQuery = baseQuery.Where(x => x.ProductCategoryId == selectedCategory.Id);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            baseQuery = baseQuery.Where(x =>
                x.Title.Contains(searchTerm) ||
                x.ShortDescription.Contains(searchTerm) ||
                (x.MetaTitle != null && x.MetaTitle.Contains(searchTerm)) ||
                (x.MetaDescription != null && x.MetaDescription.Contains(searchTerm)) ||
                x.ProductCategory.Name.Contains(searchTerm));
        }

        baseQuery = ApplySorting(baseQuery, sortBy);

        var totalResults = await baseQuery.CountAsync(cancellationToken);

        var products = await baseQuery
            .Skip((page - 1) * DefaultPageSize)
            .Take(DefaultPageSize)
            .Select(x => new ProductCardViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.ProductCategory.Name,
                CategorySlug = x.ProductCategory.Slug,
                MainImageUrl = x.MainImageUrl,
                ShortDescription = x.ShortDescription,
                IsFeatured = x.IsFeatured,
                DetailsUrl = Url.Action(nameof(Details), "Products", new { slug = x.Slug })
            })
            .ToListAsync(cancellationToken);

        var categoryCounts = await _dbContext.ProductCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new ProductCategoryFilterItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                ProductCount = x.Products.Count(p => p.IsActive && p.IsPublished),
                IsSelected = selectedCategorySlug != null && x.Slug == selectedCategorySlug
            })
            .ToListAsync(cancellationToken);

        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);

        var pageTitle = selectedCategory is not null
            ? $"{selectedCategory.Name} Products"
            : "Products";

        var introText = selectedCategory is not null
            ? $"Browse {selectedCategory.Name.ToLowerInvariant()} solutions from Ecologic Innovations."
            : "Discover our software, smart systems, training resources, and impact-driven solutions.";

        var hasFilterState =
            !string.IsNullOrWhiteSpace(searchTerm) ||
            !string.IsNullOrWhiteSpace(selectedCategorySlug) ||
            page > 1 ||
            !string.Equals(sortBy, "featured", StringComparison.OrdinalIgnoreCase);

        var seo = new SeoMetaViewModel
        {
            Title = selectedCategory is not null
                ? $"{selectedCategory.Name} Products | {siteSettings.CompanyName}"
                : $"Products | {siteSettings.CompanyName}",
            Description = selectedCategory is not null
                ? $"Browse {selectedCategory.Name.ToLowerInvariant()} products from {siteSettings.CompanyName}."
                : siteSettings.MetaDescriptionDefault ?? "Browse products from Ecologic Innovations.",
            CanonicalUrl = _canonicalUrlService.BuildAbsolute(BuildCanonicalPath(selectedCategorySlug, searchTerm, sortBy, page)),
            OgTitle = selectedCategory is not null
                ? $"{selectedCategory.Name} Products"
                : "Products",
            OgDescription = selectedCategory is not null
                ? $"Browse {selectedCategory.Name.ToLowerInvariant()} products from {siteSettings.CompanyName}."
                : "Explore products from Ecologic Innovations.",
            Robots = hasFilterState ? "noindex,follow" : "index,follow"
        };

        var model = new ProductListViewModel
        {
            PageTitle = pageTitle,
            IntroText = introText,
            Products = products,
            ResultCount = totalResults,
            SelectedCategoryName = selectedCategory?.Name,
            Breadcrumbs = BreadcrumbBuilder.CreateForProducts(),
            Seo = seo,
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = DefaultPageSize,
                TotalItems = totalResults,
                BasePath = Url.Action(nameof(Index), "Products")
            },
            Sidebar = new ProductFilterSidebarViewModel
            {
                Categories = categoryCounts,
                SearchTerm = searchTerm,
                SelectedCategoryId = selectedCategory?.Id,
                SelectedCategorySlug = selectedCategorySlug,
                SortBy = sortBy,
                SortOptions = BuildSortOptions(sortBy),
                ClearFilterUrl = Url.Action(nameof(Index), "Products")
            },
            EmptyState = products.Count == 0
                ? new EmptyStateViewModel
                {
                    Title = "No products found",
                    Message = "Try adjusting the search term, category filter, or sort option.",
                    ButtonText = "Clear Filters",
                    ButtonUrl = Url.Action(nameof(Index), "Products")
                }
                : null
        };

        ViewData.SetSeoMeta(model.Seo);

        return View("Index", model);
    }

    /// <summary>
    /// Public product details page routed by the product slug.
    /// It renders the sanitized HTML details and shows the product inquiry form when enabled.
    /// </summary>
    [HttpGet("{slug}")]
    public async Task<IActionResult> Details(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var model = await BuildProductDetailsViewModelAsync(slug.Trim().ToLowerInvariant(), cancellationToken);
        if (model is null)
        {
            return NotFound();
        }

        ViewData.SetSeoMeta(model.Seo);
        return View(model);
    }

    /// <summary>
    /// Handles the product inquiry form below the details page.
    /// The message is saved to ContactMessage with Product source tracking.
    /// </summary>
    [HttpPost("{slug}/inquiry")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendInquiry(
        string slug,
        [Bind(Prefix = "InquiryForm")] ContactFormInputModel input,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        slug = slug.Trim().ToLowerInvariant();

        var product = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .FirstOrDefaultAsync(
                x => x.IsActive && x.IsPublished && x.Slug == slug,
                cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        // Force source consistency from the route/product rather than trusting posted values.
        var source = _currentPageSourceService.CreateForProduct(product);
        input.SourceType = ContactSourceType.Product;
        input.ProductId = product.Id;
        input.BlogPostId = null;
        input.SourceTitle = source.SourceTitle;
        input.PageUrl = source.PageUrl;
        input.RegardingProductTitle = product.Title;

        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildProductDetailsViewModelAsync(slug, cancellationToken, input);
            if (invalidModel is null)
            {
                return NotFound();
            }

            invalidModel.InquirySubmittedSuccessfully = false;
            ViewData.SetSeoMeta(invalidModel.Seo);
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("Details", invalidModel);
        }

        var entity = new ContactMessage
        {
            Name = input.Name.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            Company = string.IsNullOrWhiteSpace(input.Company) ? null : input.Company.Trim(),
            Subject = string.IsNullOrWhiteSpace(input.Subject) ? null : input.Subject.Trim(),
            Message = input.Message.Trim(),
            SourceType = ContactSourceType.Product,
            ProductId = product.Id,
            BlogPostId = null,
            SourceTitle = product.Title,
            PageUrl = source.PageUrl,
            Status = ContactMessageStatus.New
        };

        _dbContext.ContactMessages.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["ProductInquirySuccess"] = "Thank you. Your message has been sent successfully.";
        return RedirectToAction(nameof(Details), new { slug });
    }

    private async Task<ProductDetailsViewModel?> BuildProductDetailsViewModelAsync(
        string slug,
        CancellationToken cancellationToken,
        ContactFormInputModel? postedInput = null)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .FirstOrDefaultAsync(
                x => x.IsActive && x.IsPublished && x.Slug == slug,
                cancellationToken);

        if (product is null)
        {
            return null;
        }

        var relatedProducts = await _dbContext.Products
            .AsNoTracking()
            .Where(x =>
                x.Id != product.Id &&
                x.IsActive &&
                x.IsPublished &&
                x.ProductCategoryId == product.ProductCategoryId)
            .OrderByDescending(x => x.IsFeatured)
            .ThenBy(x => x.ListSortOrder)
            .ThenBy(x => x.Title)
            .Take(RelatedProductsCount)
            .Select(x => new RelatedProductViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                ImageUrl = x.MainImageUrl,
                ShortDescription = x.ShortDescription
            })
            .ToListAsync(cancellationToken);

        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);
        var source = _currentPageSourceService.CreateForProduct(product);

        var inquiryForm = postedInput ?? new ContactFormInputModel
        {
            SourceType = ContactSourceType.Product,
            ProductId = product.Id,
            BlogPostId = null,
            SourceTitle = source.SourceTitle,
            PageUrl = source.PageUrl,
            RegardingProductTitle = product.Title
        };

        var successMessage = TempData["ProductInquirySuccess"]?.ToString();

        var model = new ProductDetailsViewModel
        {
            Id = product.Id,
            Title = product.Title,
            Slug = product.Slug,
            CategoryName = product.ProductCategory.Name,
            CategorySlug = product.ProductCategory.Slug,
            MainImageUrl = product.MainImageUrl,
            ShortDescription = product.ShortDescription,
            HtmlDetails = _htmlSanitizationService.SanitizeRichHtml(product.HtmlDetails),
            ContactFormEnabled = product.ContactFormEnabled,
            ContactFormTitle = string.IsNullOrWhiteSpace(product.ContactFormTitle)
                ? "Contact Us About This Product"
                : product.ContactFormTitle,
            InquiryForm = inquiryForm,
            RelatedProducts = relatedProducts,
            Breadcrumbs = BreadcrumbBuilder.CreateForProductDetails(product.Title),
            InquirySubmittedSuccessfully = !string.IsNullOrWhiteSpace(successMessage),
            SuccessMessage = successMessage,
            CompanyPhone = siteSettings.Phone,
            Seo = new SeoMetaViewModel
            {
                Title = string.IsNullOrWhiteSpace(product.MetaTitle)
                    ? $"{product.Title} | {siteSettings.CompanyName}"
                    : product.MetaTitle,
                Description = string.IsNullOrWhiteSpace(product.MetaDescription)
                    ? product.ShortDescription
                    : product.MetaDescription,
                CanonicalUrl = _canonicalUrlService.BuildAbsolute($"/products/{product.Slug}"),
                OgTitle = product.Title,
                OgDescription = string.IsNullOrWhiteSpace(product.MetaDescription)
                    ? product.ShortDescription
                    : product.MetaDescription,
                OgImageUrl = string.IsNullOrWhiteSpace(product.OgImageUrl)
                    ? product.MainImageUrl
                    : product.OgImageUrl,
                Robots = "index,follow"
            }
        };

        return model;
    }

    private static IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortBy)
    {
        return sortBy switch
        {
            "newest" => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id),
            "title_asc" => query.OrderBy(x => x.Title).ThenBy(x => x.Id),
            "title_desc" => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.Id),
            _ => query.OrderByDescending(x => x.IsFeatured).ThenBy(x => x.ListSortOrder).ThenBy(x => x.Title)
        };
    }

    private static string NormalizeSort(string? sort)
    {
        return sort?.Trim().ToLowerInvariant() switch
        {
            "newest" => "newest",
            "title_asc" => "title_asc",
            "title_desc" => "title_desc",
            _ => "featured"
        };
    }

    private List<SelectListItem> BuildSortOptions(string selected)
    {
        return new List<SelectListItem>
        {
            new() { Text = "Featured", Value = "featured", Selected = selected == "featured" },
            new() { Text = "Newest", Value = "newest", Selected = selected == "newest" },
            new() { Text = "Title A–Z", Value = "title_asc", Selected = selected == "title_asc" },
            new() { Text = "Title Z–A", Value = "title_desc", Selected = selected == "title_desc" }
        };
    }

    private string BuildCanonicalPath(string? categorySlug, string? searchTerm, string sortBy, int page)
    {
        var parameters = new List<string>();

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            parameters.Add($"category={Uri.EscapeDataString(categorySlug)}");
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            parameters.Add($"q={Uri.EscapeDataString(searchTerm)}");
        }

        if (!string.Equals(sortBy, "featured", StringComparison.OrdinalIgnoreCase))
        {
            parameters.Add($"sort={Uri.EscapeDataString(sortBy)}");
        }

        if (page > 1)
        {
            parameters.Add($"page={page}");
        }

        return parameters.Count == 0
            ? "/products"
            : $"/products?{string.Join("&", parameters)}";
    }
}
