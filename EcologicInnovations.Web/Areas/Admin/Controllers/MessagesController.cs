using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Admin controller for reviewing and managing all unified contact messages.
/// </summary>
public class MessagesController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public MessagesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Admin list of all messages with search, filter, sort, and pagination.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(
        string? searchTerm,
        ContactMessageStatus? status,
        ContactSourceType? sourceType,
        string? sortBy,
        int pageNumber = 1,
        int pageSize = 15,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 15 : Math.Min(pageSize, 100);

        var filter = new MessageAdminListFilterViewModel
        {
            SearchTerm = searchTerm,
            Status = status,
            SourceType = sourceType,
            SortBy = string.IsNullOrWhiteSpace(sortBy) ? "newest" : sortBy,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = _dbContext.ContactMessages
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = filter.SearchTerm.Trim();

            query = query.Where(x =>
                x.Name.Contains(keyword) ||
                x.Email.Contains(keyword) ||
                x.Phone.Contains(keyword) ||
                (x.Company != null && x.Company.Contains(keyword)) ||
                (x.Subject != null && x.Subject.Contains(keyword)) ||
                x.Message.Contains(keyword) ||
                (x.SourceTitle != null && x.SourceTitle.Contains(keyword)));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(x => x.Status == filter.Status.Value);
        }

        if (filter.SourceType.HasValue)
        {
            query = query.Where(x => x.SourceType == filter.SourceType.Value);
        }

        query = filter.SortBy switch
        {
            "oldest" => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            "status_asc" => query.OrderBy(x => x.Status).ThenByDescending(x => x.CreatedAt),
            "status_desc" => query.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreatedAt),
            "name_asc" => query.OrderBy(x => x.Name).ThenByDescending(x => x.CreatedAt),
            "name_desc" => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

        var totalCountTask = query.CountAsync(cancellationToken);
        var newCountTask = _dbContext.ContactMessages
            .AsNoTracking()
            .CountAsync(x => x.Status == ContactMessageStatus.New, cancellationToken);

        var itemsTask = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new MessageAdminListItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Subject = x.Subject,
                SourceType = x.SourceType,
                SourceTitle = x.SourceTitle,
                Status = x.Status,
                CreatedAt = x.CreatedAt,
                MessagePreview = x.Message.Length > 140
                    ? x.Message.Substring(0, 140) + "..."
                    : x.Message
            })
            .ToListAsync(cancellationToken);

        await Task.WhenAll(totalCountTask, newCountTask, itemsTask);

        var model = new MessageAdminListViewModel
        {
            Filter = filter,
            Items = itemsTask.Result,
            TotalCount = totalCountTask.Result,
            NewCount = newCountTask.Result,
            Pagination = new PaginationViewModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalCountTask.Result,
                BasePath = Url.Action(nameof(Index), "Messages", new { area = "Admin" })
            },
            StatusOptions = BuildStatusOptions(filter.Status),
            SourceTypeOptions = BuildSourceTypeOptions(filter.SourceType),
            EmptyState = itemsTask.Result.Count == 0
                ? new EmptyStateViewModel
                {
                    Title = "No messages found",
                    Message = "Try changing the search term or filters.",
                    ButtonText = "Clear Filters",
                    ButtonUrl = Url.Action(nameof(Index), "Messages", new { area = "Admin" })
                }
                : null
        };

        ViewData["AdminPageTitle"] = "Messages";
        ViewData["AdminPageDescription"] = "Review inquiries submitted from the contact page, product pages, and blog pages.";
        ViewData["AdminBreadcrumbs"] = new List<BreadcrumbItemViewModel>
        {
            new() { Title = "Admin", Url = Url.Action("Index", "Dashboard", new { area = "Admin" }), IsActive = false },
            new() { Title = "Messages", Url = null, IsActive = true }
        };

        return View(model);
    }

    /// <summary>
    /// Admin detail page for one message.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.ContactMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (message is null)
        {
            return NotFound();
        }

        var model = new MessageAdminDetailViewModel
        {
            Id = message.Id,
            Name = message.Name,
            Email = message.Email,
            Phone = message.Phone,
            Company = message.Company,
            Subject = message.Subject,
            Message = message.Message,
            SourceType = message.SourceType,
            ProductId = message.ProductId,
            BlogPostId = message.BlogPostId,
            SourceTitle = message.SourceTitle,
            PageUrl = message.PageUrl,
            Status = message.Status,
            AdminNote = message.AdminNote,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            StatusOptions = BuildStatusOptions(message.Status),
            UpdateForm = new MessageStatusUpdateInputModel
            {
                Id = message.Id,
                Status = message.Status,
                AdminNote = message.AdminNote
            },
            Breadcrumbs = new List<BreadcrumbItemViewModel>
            {
                new() { Title = "Admin", Url = Url.Action("Index", "Dashboard", new { area = "Admin" }), IsActive = false },
                new() { Title = "Messages", Url = Url.Action(nameof(Index), "Messages", new { area = "Admin" }), IsActive = false },
                new() { Title = $"Message #{message.Id}", Url = null, IsActive = true }
            }
        };

        ViewData["AdminPageTitle"] = $"Message #{message.Id}";
        ViewData["AdminPageDescription"] = "Review sender details, source tracking, workflow status, and internal notes.";
        ViewData["AdminBreadcrumbs"] = model.Breadcrumbs;

        return View(model);
    }

    /// <summary>
    /// Updates workflow status and internal note for a message.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(
        MessageStatusUpdateInputModel input,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Details), new { id = input.Id });
        }

        var message = await _dbContext.ContactMessages
            .FirstOrDefaultAsync(x => x.Id == input.Id, cancellationToken);

        if (message is null)
        {
            return NotFound();
        }

        message.Status = input.Status;
        message.AdminNote = string.IsNullOrWhiteSpace(input.AdminNote) ? null : input.AdminNote.Trim();

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Message status updated successfully.";
        TempData["AdminToastSuccess"] = "Message updated.";

        return RedirectToAction(nameof(Details), new { id = input.Id });
    }

    private static List<SelectListItem> BuildStatusOptions(ContactMessageStatus? selected)
    {
        return Enum.GetValues<ContactMessageStatus>()
            .Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString(),
                Selected = selected.HasValue && x == selected.Value
            })
            .ToList();
    }

    private static List<SelectListItem> BuildSourceTypeOptions(ContactSourceType? selected)
    {
        var items = new List<SelectListItem>
        {
            new() { Value = "", Text = "All Sources", Selected = !selected.HasValue }
        };

        items.AddRange(
            Enum.GetValues<ContactSourceType>()
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString(),
                    Selected = selected.HasValue && x == selected.Value
                }));

        return items;
    }
}
