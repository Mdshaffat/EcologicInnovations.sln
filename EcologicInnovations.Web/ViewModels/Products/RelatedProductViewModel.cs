namespace EcologicInnovations.Web.ViewModels.Products;

/// <summary>
/// Represents a smaller related product card shown below the product details content.
/// </summary>
public class RelatedProductViewModel
{
    /// <summary>
    /// Related product id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Related product title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Slug used for details navigation.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Related product image.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Short one-line summary.
    /// </summary>
    public string? ShortDescription { get; set; }
}
