namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin dashboard model.
/// It contains high-level summary counts and a short list of recent messages.
/// </summary>
public class AdminDashboardViewModel
{
    /// <summary>
    /// Summary cards shown at the top of the dashboard.
    /// </summary>
    public List<AdminDashboardSummaryCardViewModel> SummaryCards { get; set; } = new();

    /// <summary>
    /// Recent message rows shown below the summary cards.
    /// </summary>
    public List<RecentMessageRowViewModel> RecentMessages { get; set; } = new();

    /// <summary>
    /// Count of messages currently in New status.
    /// </summary>
    public int NewMessageCount { get; set; }

    /// <summary>
    /// Count of published products.
    /// </summary>
    public int PublishedProductCount { get; set; }

    /// <summary>
    /// Count of published blog posts.
    /// </summary>
    public int PublishedBlogCount { get; set; }

    /// <summary>
    /// Count of active media items.
    /// </summary>
    public int ActiveMediaCount { get; set; }
}
