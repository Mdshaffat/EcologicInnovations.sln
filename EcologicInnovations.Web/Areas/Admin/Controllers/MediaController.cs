using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Admin media library controller.
/// It supports browsing, uploading, editing metadata, and copying reusable media URLs/HTML.
/// </summary>
public class MediaController : AdminControllerBase
{
    private const int DefaultPageSize = 24;

    private readonly ApplicationDbContext _dbContext;
    private readonly IFileUploadService _fileUploadService;

    public MediaController(
        ApplicationDbContext dbContext,
        IFileUploadService fileUploadService)
    {
        _dbContext = dbContext;
        _fileUploadService = fileUploadService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? keyword,
        string? mediaGroup,
        bool? isActive,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, 96);

        var filter = new MediaLibraryFilterViewModel
        {
            Keyword = keyword,
            MediaGroup = mediaGroup,
            IsActive = isActive,
            SortBy = NormalizeSort(sortBy),
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = _dbContext.MediaFiles
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var term = filter.Keyword.Trim();
            query = query.Where(x =>
                x.FileName.Contains(term) ||
                (x.OriginalFileName != null && x.OriginalFileName.Contains(term)) ||
                (x.Title != null && x.Title.Contains(term)) ||
                (x.AltText != null && x.AltText.Contains(term)) ||
                (x.PublicUrl != null && x.PublicUrl.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(filter.MediaGroup))
        {
            var group = filter.MediaGroup.Trim();
            query = query.Where(x => x.MediaGroup != null && x.MediaGroup == group);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == filter.IsActive.Value);
        }

        query = filter.SortBy switch
        {
            "oldest" => query.OrderBy(x => x.UploadedAt),
            "group_asc" => query.OrderBy(x => x.MediaGroup).ThenByDescending(x => x.UploadedAt),
            "group_desc" => query.OrderByDescending(x => x.MediaGroup).ThenByDescending(x => x.UploadedAt),
            "title_asc" => query.OrderBy(x => x.Title).ThenByDescending(x => x.UploadedAt),
            "title_desc" => query.OrderByDescending(x => x.Title).ThenByDescending(x => x.UploadedAt),
            _ => query.OrderByDescending(x => x.UploadedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MediaLibraryItemViewModel
            {
                Id = x.Id,
                FileName = x.FileName,
                OriginalFileName = x.OriginalFileName,
                PublicUrl = x.PublicUrl,
                FilePath = x.FilePath,
                ContentType = x.ContentType,
                FileSize = x.FileSize,
                AltText = x.AltText,
                Title = x.Title,
                MediaGroup = x.MediaGroup,
                UploadedAt = x.UploadedAt,
                IsActive = x.IsActive,
                IsImage = x.ContentType.StartsWith("image/"),
                CopyHtmlSnippet = x.ContentType.StartsWith("image/")
                    ? $"<img src=\"{x.PublicUrl}\" alt=\"{(x.AltText ?? x.Title ?? x.FileName)}\" class=\"img-fluid rounded\" />"
                    : null
            })
            .ToListAsync(cancellationToken);

        foreach (var item in items)
        {
            item.FileSizeText = FormatFileSize(item.FileSize);
        }

        var activeCount = await _dbContext.MediaFiles
            .AsNoTracking()
            .CountAsync(x => x.IsActive, cancellationToken);

        var groups = await _dbContext.MediaFiles
            .AsNoTracking()
            .Where(x => !string.IsNullOrWhiteSpace(x.MediaGroup))
            .Select(x => x.MediaGroup!)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);

        var model = new MediaLibraryViewModel
        {
            Filter = filter,
            Items = items,
            TotalCount = totalCount,
            ActiveCount = activeCount,
            UploadModel = new MediaUploadInputModel(),
            Pagination = new PaginationViewModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalCount,
                BasePath = Url.Action("Index", "Media", new { area = "Admin" })
            },
            MediaGroupOptions = groups
                .Select(x => new SelectListItem(x, x, string.Equals(x, filter.MediaGroup, StringComparison.OrdinalIgnoreCase)))
                .ToList(),
            MaxUploadBytes = _fileUploadService.GetMaxFileSizeBytes(),
            AllowedExtensions = GetAllowedExtensionsPreview(),
            EmptyState = totalCount == 0
                ? new EmptyStateViewModel
                {
                    Title = "No media found",
                    Message = "Upload images or documents once, then reuse their URLs and HTML snippets throughout the site."
                }
                : null
        };

        SetPageChrome();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(MediaUploadInputModel model, CancellationToken cancellationToken)
    {
        if (model.Files is null || model.Files.Count == 0 || model.Files.All(x => x == null || x.Length == 0))
        {
            TempData["AdminErrorMessage"] = "Please select at least one file to upload.";
            return RedirectToAction(nameof(Index));
        }

        var uploadedByUserId = User.Identity?.Name;
        var mediaGroup = string.IsNullOrWhiteSpace(model.MediaGroup) ? "General" : model.MediaGroup.Trim();

        try
        {
            foreach (var file in model.Files.Where(x => x is not null && x.Length > 0))
            {
                await _fileUploadService.SaveMediaAsync(
                    file,
                    mediaGroup,
                    title: model.Title,
                    altText: model.AltText,
                    uploadedByUserId: uploadedByUserId,
                    cancellationToken: cancellationToken);
            }

            TempData["AdminSuccessMessage"] = "Media uploaded successfully.";
            TempData["AdminToastSuccess"] = "Upload completed.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["AdminErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var media = await _dbContext.MediaFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (media is null)
        {
            return NotFound();
        }

        var model = new MediaEditInputModel
        {
            Id = media.Id,
            AltText = media.AltText,
            Title = media.Title,
            MediaGroup = media.MediaGroup,
            IsActive = media.IsActive,
            PublicUrl = media.PublicUrl,
            CopyHtmlSnippet = media.ContentType.StartsWith("image/")
                ? $"<img src=\"{media.PublicUrl}\" alt=\"{(media.AltText ?? media.Title ?? media.FileName)}\" class=\"img-fluid rounded\" />"
                : null
        };

        ViewData["AdminPageTitle"] = "Edit Media";
        ViewData["AdminPageDescription"] = "Update alt text, title, group, and reusable copy values.";
        ViewData["AdminBreadcrumbs"] = BuildBreadcrumbs("Edit Media");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MediaEditInputModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ViewData["AdminPageTitle"] = "Edit Media";
            ViewData["AdminPageDescription"] = "Update alt text, title, group, and reusable copy values.";
            ViewData["AdminBreadcrumbs"] = BuildBreadcrumbs("Edit Media");
            return View(model);
        }

        var media = await _dbContext.MediaFiles.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
        if (media is null)
        {
            return NotFound();
        }

        media.AltText = string.IsNullOrWhiteSpace(model.AltText) ? null : model.AltText.Trim();
        media.Title = string.IsNullOrWhiteSpace(model.Title) ? null : model.Title.Trim();
        media.MediaGroup = string.IsNullOrWhiteSpace(model.MediaGroup) ? null : model.MediaGroup.Trim();
        media.IsActive = model.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Media metadata updated successfully.";
        TempData["AdminToastSuccess"] = "Media updated.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var media = await _dbContext.MediaFiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (media is null)
        {
            TempData["AdminErrorMessage"] = "Media item not found.";
            return RedirectToAction(nameof(Index));
        }

        await _fileUploadService.DeleteMediaAsync(media, deleteDatabaseRow: true, cancellationToken: cancellationToken);

        TempData["AdminSuccessMessage"] = "Media deleted successfully.";
        TempData["AdminToastSuccess"] = "Media deleted.";

        return RedirectToAction(nameof(Index));
    }

    private static string NormalizeSort(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return "newest";
        }

        var normalized = sortBy.Trim().ToLowerInvariant();
        return normalized switch
        {
            "newest" or "oldest" or "group_asc" or "group_desc" or "title_asc" or "title_desc" => normalized,
            _ => "newest"
        };
    }

    private static string FormatFileSize(long bytes)
    {
        if (bytes < 1024)
        {
            return $"{bytes} B";
        }

        var kb = bytes / 1024d;
        if (kb < 1024)
        {
            return $"{kb:0.#} KB";
        }

        var mb = kb / 1024d;
        if (mb < 1024)
        {
            return $"{mb:0.#} MB";
        }

        var gb = mb / 1024d;
        return $"{gb:0.##} GB";
    }

    private List<string> GetAllowedExtensionsPreview()
    {
        return
        [
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".gif",
            ".pdf"
        ];
    }

    private void SetPageChrome()
    {
        ViewData["AdminPageTitle"] = "Media Library";
        ViewData["AdminPageDescription"] = "Upload reusable files, copy public URLs, and generate HTML snippets for content editors.";
        ViewData["AdminBreadcrumbs"] = BuildBreadcrumbs("Media Library");
    }

    private static List<BreadcrumbItemViewModel> BuildBreadcrumbs(string current)
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Admin", Url = "/Admin/Dashboard", IsActive = false },
            new BreadcrumbItemViewModel { Title = current, Url = null, IsActive = true }
        ];
    }
}
