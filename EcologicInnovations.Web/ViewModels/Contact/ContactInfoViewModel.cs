namespace EcologicInnovations.Web.ViewModels.Contact;

/// <summary>
/// Represents public business contact information displayed on the Contact page or footer.
/// </summary>
public class ContactInfoViewModel
{
    /// <summary>
    /// Public business or support email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Public sales email.
    /// </summary>
    public string? SalesEmail { get; set; }

    /// <summary>
    /// Public phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Public address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Facebook page URL.
    /// </summary>
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// LinkedIn page URL.
    /// </summary>
    public string? LinkedInUrl { get; set; }

    /// <summary>
    /// YouTube channel URL.
    /// </summary>
    public string? YouTubeUrl { get; set; }
}
