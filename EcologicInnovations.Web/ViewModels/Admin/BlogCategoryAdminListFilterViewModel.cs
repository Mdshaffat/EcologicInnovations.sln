namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents filter, sort, and paging input state for the Blog Category admin list page.
/// </summary>
public class BlogCategoryAdminListFilterViewModel
{
    /// <summary>
    /// Keyword search across name, slug, and description.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filter by active state. Null means all categories.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Supported values: name_asc, name_desc, sort_asc, sort_desc, newest, oldest.
    /// </summary>
    public string SortBy { get; set; } = "sort_asc";

    /// <summary>
    /// Current page number starting from 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of rows shown per page.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
