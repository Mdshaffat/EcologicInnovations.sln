namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Filter, sort, and paging input state for the Product admin list page.
/// </summary>
public class ProductAdminListFilterViewModel
{
    /// <summary>
    /// Keyword search across title, slug, short description, and category name.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Selected category id filter. Null means all categories.
    /// </summary>
    public int? ProductCategoryId { get; set; }

    /// <summary>
    /// Filter by published state. Null means all.
    /// </summary>
    public bool? IsPublished { get; set; }

    /// <summary>
    /// Filter by active state. Null means all.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter by featured state. Null means all.
    /// </summary>
    public bool? IsFeatured { get; set; }

    /// <summary>
    /// Filter by whether the product is shown in the top menu dropdown. Null means all.
    /// </summary>
    public bool? ShowInProductMenu { get; set; }

    /// <summary>
    /// Sort option for the list page.
    /// Supported values:
    /// newest, oldest, title_asc, title_desc, listsort_asc, listsort_desc, menu_asc, menu_desc
    /// </summary>
    public string SortBy { get; set; } = "listsort_asc";

    /// <summary>
    /// Current page number starting from 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of rows shown per page.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
