using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Contact;

/// <summary>
/// Strongly typed page model for the public Contact page.
/// It supports a general contact form and optional source context
/// when the page is opened for a related Product or Blog entry.
/// </summary>
public class ContactPageViewModel
{
    /// <summary>
    /// Main page title shown in the hero/header area.
    /// </summary>
    public string PageTitle { get; set; } = "Contact Us";

    /// <summary>
    /// Optional page intro text shown under the title.
    /// </summary>
    public string? IntroText { get; set; }

    /// <summary>
    /// Contact form model used by the page.
    /// </summary>
    public ContactFormInputModel Form { get; set; } = new();

    /// <summary>
    /// Optional human-readable heading describing the current source context.
    /// Example: "Regarding Product" or "Regarding Article".
    /// </summary>
    public string? SourceHeading { get; set; }

    /// <summary>
    /// Optional human-readable value shown above the form when the page is opened
    /// with a source context such as a Product or Blog.
    /// </summary>
    public string? SourceDisplayTitle { get; set; }

    /// <summary>
    /// Current source type used for visual messaging and state.
    /// </summary>
    public ContactSourceType SourceType { get; set; } = ContactSourceType.General;

    /// <summary>
    /// Final SEO metadata for the page.
    /// </summary>
    public SeoMetaViewModel Seo { get; set; } = new();

    /// <summary>
    /// Breadcrumb trail shown near the top of the page.
    /// </summary>
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new();

    /// <summary>
    /// Indicates whether the contact form was submitted successfully.
    /// </summary>
    public bool SubmittedSuccessfully { get; set; }

    /// <summary>
    /// Optional success message shown after successful form submission.
    /// </summary>
    public string? SuccessMessage { get; set; }

    /// <summary>
    /// Site-wide public contact information loaded from SiteSettings.
    /// </summary>
    public string? SupportEmail { get; set; }

    /// <summary>
    /// Optional sales email shown in the contact card.
    /// </summary>
    public string? SalesEmail { get; set; }

    /// <summary>
    /// Optional phone number shown in the contact card.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Optional address shown in the contact card.
    /// </summary>
    public string? Address { get; set; }
}
