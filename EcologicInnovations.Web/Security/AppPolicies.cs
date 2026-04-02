namespace EcologicInnovations.Web.Security;

/// <summary>
/// Central place for application authorization policy names.
/// </summary>
public static class AppPolicies
{
    /// <summary>
    /// Policy requiring the authenticated user to be in the Admin role.
    /// </summary>
    public const string RequireAdminRole = "RequireAdminRole";
}
