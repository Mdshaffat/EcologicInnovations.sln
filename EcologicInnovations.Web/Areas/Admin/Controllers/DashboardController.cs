using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.ViewModels.Admin;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Main landing controller for the Admin area.
/// It shows high-level summary cards and recent inquiries to help the admin
/// quickly understand platform activity.
/// </summary>
public class DashboardController : AdminControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public DashboardController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {

        // Do not execute multiple EF Core queries concurrently on the same DbContext.
        // Execute each query sequentially to avoid InvalidOperationException.
        var totalProducts = await _dbContext.Products
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var publishedProducts = await _dbContext.Products
            .AsNoTracking()
            .CountAsync(x => x.IsPublished && x.IsActive, cancellationToken);

        var totalBlogs = await _dbContext.BlogPosts
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var publishedBlogs = await _dbContext.BlogPosts
            .AsNoTracking()
            .CountAsync(x => x.IsPublished, cancellationToken);

        var totalMessages = await _dbContext.ContactMessages
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var newMessages = await _dbContext.ContactMessages
            .AsNoTracking()
            .CountAsync(x => x.Status == ContactMessageStatus.New, cancellationToken);

        var totalMedia = await _dbContext.MediaFiles
            .AsNoTracking()
            .CountAsync(x => x.IsActive, cancellationToken);

        var recentMessages = await _dbContext.ContactMessages
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(8)
            .Select(x => new RecentMessageRowViewModel
            {
                Id = x.Id,
                Name = x.Name,
                SourceType = x.SourceType,
                SourceTitle = x.SourceTitle,
                Status = x.Status,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var model = new AdminDashboardViewModel
        {
            PublishedProductCount = publishedProducts,
            PublishedBlogCount = publishedBlogs,
            NewMessageCount = newMessages,
            ActiveMediaCount = totalMedia,
            RecentMessages = recentMessages,
            SummaryCards =
            [
                new AdminDashboardSummaryCardViewModel
                {
                    Title = "Products",
                    Count = totalProducts,
                    IconCssClass = "bi bi-box-seam",
                    ColorClass = "primary",
                    Url = Url.Action("Index", "Products", new { area = "Admin" })
                },
                new AdminDashboardSummaryCardViewModel
                {
                    Title = "Blogs",
                    Count = totalBlogs,
                    IconCssClass = "bi bi-journal-richtext",
                    ColorClass = "success",
                    Url = Url.Action("Index", "Blogs", new { area = "Admin" })
                },
                new AdminDashboardSummaryCardViewModel
                {
                    Title = "Messages",
                    Count = totalMessages,
                    IconCssClass = "bi bi-chat-dots",
                    ColorClass = "warning",
                    Url = Url.Action("Index", "Messages", new { area = "Admin" })
                },
                new AdminDashboardSummaryCardViewModel
                {
                    Title = "Media",
                    Count = totalMedia,
                    IconCssClass = "bi bi-images",
                    ColorClass = "info",
                    Url = Url.Action("Index", "Media", new { area = "Admin" })
                }
            ]
        };

        ViewData["AdminPageTitle"] = "Dashboard";
        ViewData["AdminPageDescription"] = "Overview of products, blogs, messages, and media activity.";
        ViewData["AdminBreadcrumbs"] = new List<BreadcrumbItemViewModel>
        {
            new() { Title = "Admin", Url = Url.Action("Index", "Dashboard", new { area = "Admin" }), IsActive = false },
            new() { Title = "Dashboard", Url = null, IsActive = true }
        };

        return View(model);
    }
}
