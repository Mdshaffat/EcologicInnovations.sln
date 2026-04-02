namespace EcologicInnovations.Web.Models.Enums;

/// <summary>
/// Identifies where a contact message originated from.
/// This helps the admin understand whether the inquiry came
/// from the general contact page, a product page, or a blog article.
/// </summary>
public enum ContactSourceType
{
    /// <summary>
    /// Message submitted from the general contact page or other generic form.
    /// </summary>
    General = 1,

    /// <summary>
    /// Message submitted from a product details page.
    /// </summary>
    Product = 2,

    /// <summary>
    /// Message submitted from a blog details page.
    /// </summary>
    Blog = 3
}
