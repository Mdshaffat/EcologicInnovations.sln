using EcologicInnovations.Web.ViewModels.Contact;
using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Products;

/// <summary>
/// Main product details page model.
/// It supports the public product presentation plus the required inquiry form shown below the content.
/// </summary>
public class ProductDetailsViewModel
{
    /// <summary>
    /// Product id used for inquiry tracking and internal linking.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product title shown prominently on the details page.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// SEO/public slug.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Category name shown above or near the title.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Category slug useful for breadcrumb or list-back links.
    /// </summary>
    public string? CategorySlug { get; set; }

    /// <summary>
    /// Main hero/product image.
    /// </summary>
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// Short description shown as summary text.
    /// </summary>
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Sanitized rich HTML details rendered as the main product content body.
    /// </summary>
    public string? HtmlDetails { get; set; }

    /// <summary>
    /// Controls whether the contact/inquiry form is shown below the product content.
    /// </summary>
    public bool ContactFormEnabled { get; set; }

    /// <summary>
    /// Optional heading shown above the inquiry form.
    /// </summary>
    public string? ContactFormTitle { get; set; }

    /// <summary>
    /// Inquiry form model used on the same details page.
    /// </summary>
    public ContactFormInputModel InquiryForm { get; set; } = new();

    /// <summary>
    /// Related products shown below the main content.
    /// </summary>
    public List<RelatedProductViewModel> RelatedProducts { get; set; } = new();

    /// <summary>
    /// Breadcrumb items for the page.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();

    /// <summary>
    /// SEO metadata for the product details page.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();

    /// <summary>
    /// Indicates whether the inquiry was submitted successfully.
    /// This is useful when the page is returned after a valid post.
    /// </summary>
    public bool InquirySubmittedSuccessfully { get; set; }

    /// <summary>
    /// Optional success message shown above the inquiry form.
    /// </summary>
    public string? SuccessMessage { get; set; }
}
