namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents one product row in the admin list page.
/// </summary>
public class ProductAdminListItemViewModel
{
    /// <summary>
    /// Product primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product title shown prominently in the list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO-friendly product slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Product category name for quick reference.
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// Optional main image URL shown as a small thumbnail if available.
    /// </summary>
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Short summary shown in truncated form.
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Indicates whether the contact form is enabled on the public product details page.
    /// </summary>
    public bool ContactFormEnabled { get; set; }

    /// <summary>
    /// Indicates whether the product appears in the top menu dropdown.
    /// </summary>
    public bool ShowInProductMenu { get; set; }

    /// <summary>
    /// Sort order for menu dropdown items.
    /// </summary>
    public int MenuSortOrder { get; set; }

    /// <summary>
    /// Sort order for the public product list.
    /// </summary>
    public int ListSortOrder { get; set; }

    /// <summary>
    /// Indicates whether this product is featured.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Indicates whether this product is publicly published.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Indicates whether this product is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// When the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
