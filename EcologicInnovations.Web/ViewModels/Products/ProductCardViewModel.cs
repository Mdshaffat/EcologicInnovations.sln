namespace EcologicInnovations.Web.ViewModels.Products;

/// <summary>
/// Lightweight product card model used in product lists, home page featured sections,
/// related products, and dropdown-friendly UI blocks.
/// </summary>
public class ProductCardViewModel
{
    /// <summary>
    /// Product id used for linking or hidden references when needed.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product title shown on the card.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Product slug used for details page routing.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Category name shown as a label or badge.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Category slug useful for filter links.
    /// </summary>
    public string? CategorySlug { get; set; }

    /// <summary>
    /// Main card image URL.
    /// </summary>
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Short marketing summary shown in lists.
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Indicates whether the product is featured.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Optional precomputed URL to details page.
    /// </summary>
    public string? DetailsUrl { get; set; }
}
