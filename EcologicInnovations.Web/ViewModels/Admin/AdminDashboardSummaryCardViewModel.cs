namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents one summary card on the admin dashboard.
/// </summary>
public class AdminDashboardSummaryCardViewModel
{
    /// <summary>
    /// Short card title such as Products, Blogs, Messages, or Media.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Main numeric value displayed on the card.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Optional icon CSS class for visual emphasis.
    /// </summary>
    public string? IconCssClass { get; set; }

    /// <summary>
    /// Optional Bootstrap contextual class such as primary, success, info, warning.
    /// </summary>
    public string? ColorClass { get; set; }

    /// <summary>
    /// Optional link to the related admin module.
    /// </summary>
    public string? Url { get; set; }
}
