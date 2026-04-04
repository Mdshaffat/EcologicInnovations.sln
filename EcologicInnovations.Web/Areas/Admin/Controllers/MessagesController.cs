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
/// Supports status workflow (New, Read, Unread, Replied, Closed),
/// important flag, red-mark flag, and quick-toggle actions from the list.
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
        bool? important,
        bool? flagged,
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
            Important = important,
            Flagged = flagged,
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

        if (filter.Important == true)
        {
            query = query.Where(x => x.IsImportant);
        }

        if (filter.Flagged == true)
        {
            query = query.Where(x => x.IsFlagged);
        }

        query = filter.SortBy switch
        {
            "oldest" => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            "status_asc" => query.OrderBy(x => x.Status).ThenByDescending(x => x.CreatedAt),
            "status_desc" => query.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreatedAt),
            "name_asc" => query.OrderBy(x => x.Name).ThenByDescending(x => x.CreatedAt),
            "name_desc" => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.CreatedAt),
            "flagged" => query.OrderByDescending(x => x.IsFlagged).ThenByDescending(x => x.IsImportant).ThenByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

        // Execute queries sequentially on the same DbContext to avoid concurrent operations.
        var totalCount = await query.CountAsync(cancellationToken);

        var newCount = await _dbContext.ContactMessages
            .AsNoTracking()
            .CountAsync(x => x.Status == ContactMessageStatus.New, cancellationToken);

        var unreadCount = await _dbContext.ContactMessages
            .AsNoTracking()
            .CountAsync(x => x.Status == ContactMessageStatus.New || x.Status == ContactMessageStatus.Unread, cancellationToken);

        var importantCount = await _dbContext.ContactMessages
            .AsNoTracking()
            .CountAsync(x => x.IsImportant, cancellationToken);

        var flaggedCount = await _dbContext.ContactMessages
            .AsNoTracking()
            .CountAsync(x => x.IsFlagged, cancellationToken);

        var items = await query
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
                IsImportant = x.IsImportant,
                IsFlagged = x.IsFlagged,
                CreatedAt = x.CreatedAt,
                MessagePreview = x.Message.Length > 140
                    ? x.Message.Substring(0, 140) + "..."
                    : x.Message
            })
            .ToListAsync(cancellationToken);

        var model = new MessageAdminListViewModel
        {
            Filter = filter,
            Items = items,
            TotalCount = totalCount,
            NewCount = newCount,
            UnreadCount = unreadCount,
            ImportantCount = importantCount,
            FlaggedCount = flaggedCount,
            Pagination = new PaginationViewModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalCount,
                BasePath = Url.Action(nameof(Index), "Messages", new { area = "Admin" })
            },
            StatusOptions = BuildStatusOptions(filter.Status),
            SourceTypeOptions = BuildSourceTypeOptions(filter.SourceType),
            EmptyState = items.Count == 0
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
        ViewData["AdminPageDescription"] = "Review inquiries submitted from the contact page, product pages, and article pages.";
        ViewData["AdminBreadcrumbs"] = new List<BreadcrumbItemViewModel>
        {
            new() { Title = "Admin", Url = Url.Action("Index", "Dashboard", new { area = "Admin" }), IsActive = false },
            new() { Title = "Messages", Url = null, IsActive = true }
        };

        return View(model);
    }

    /// <summary>
    /// Admin detail page for one message. Auto-marks New messages as Read.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.ContactMessages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (message is null)
        {
            return NotFound();
        }

        // Auto-mark as Read when an admin opens the message for the first time.
        if (message.Status == ContactMessageStatus.New)
        {
            message.Status = ContactMessageStatus.Read;
            await _dbContext.SaveChangesAsync(cancellationToken);
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
            IsImportant = message.IsImportant,
            IsFlagged = message.IsFlagged,
            SubmitterIpAddress = message.SubmitterIpAddress,
            SubmitterUserAgent = message.SubmitterUserAgent,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            StatusOptions = BuildStatusOptions(message.Status),
            UpdateForm = new MessageStatusUpdateInputModel
            {
                Id = message.Id,
                Status = message.Status,
                AdminNote = message.AdminNote,
                IsImportant = message.IsImportant,
                IsFlagged = message.IsFlagged
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
    /// Updates workflow status, flags, and internal note for a message.
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
        message.IsImportant = input.IsImportant;
        message.IsFlagged = input.IsFlagged;

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminSuccessMessage"] = "Message status updated successfully.";
        TempData["AdminToastSuccess"] = "Message updated.";

        return RedirectToAction(nameof(Details), new { id = input.Id });
    }

    /// <summary>
    /// Quick-toggle read/unread status from the list page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleRead(int id, string? returnUrl, CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.ContactMessages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (message is null)
        {
            return NotFound();
        }

        message.Status = message.Status is ContactMessageStatus.Read or ContactMessageStatus.Replied or ContactMessageStatus.Closed
            ? ContactMessageStatus.Unread
            : ContactMessageStatus.Read;

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminToastSuccess"] = message.Status == ContactMessageStatus.Read
            ? "Marked as read."
            : "Marked as unread.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Quick-toggle important flag from the list page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleImportant(int id, string? returnUrl, CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.ContactMessages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (message is null)
        {
            return NotFound();
        }

        message.IsImportant = !message.IsImportant;

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminToastSuccess"] = message.IsImportant
            ? "Marked as important."
            : "Removed important mark.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Quick-toggle red-mark / flagged from the list page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFlag(int id, string? returnUrl, CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.ContactMessages
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (message is null)
        {
            return NotFound();
        }

        message.IsFlagged = !message.IsFlagged;

        await _dbContext.SaveChangesAsync(cancellationToken);

        TempData["AdminToastSuccess"] = message.IsFlagged
            ? "Red mark applied."
            : "Red mark removed.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
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
