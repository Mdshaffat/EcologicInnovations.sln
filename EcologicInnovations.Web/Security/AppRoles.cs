namespace EcologicInnovations.Web.Security;

/// <summary>
/// Central place for application role names.
/// Using constants avoids string duplication and typo-related authorization bugs.
/// </summary>
public static class AppRoles
{
    /// <summary>
    /// Full access role for the admin CMS area.
    /// Users in this role can manage products, blogs, pages, messages, media, and site settings.
    /// </summary>
    public const string Admin = "Admin";
}
