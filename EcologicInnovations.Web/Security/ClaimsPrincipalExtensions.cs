using System.Security.Claims;

namespace EcologicInnovations.Web.Security;

/// <summary>
/// Helper extensions for role-aware checks in controllers, views, and shared layout code.
/// These helpers are only for convenience and UI behavior.
/// Actual security is enforced by authorization attributes and policies.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Returns true when the current user is authenticated.
    /// </summary>
    public static bool IsSignedIn(this ClaimsPrincipal? user)
    {
        return user?.Identity?.IsAuthenticated == true;
    }

    /// <summary>
    /// Returns true when the current user is in the Admin role.
    /// </summary>
    public static bool IsAdmin(this ClaimsPrincipal? user)
    {
        return user?.Identity?.IsAuthenticated == true && user.IsInRole(AppRoles.Admin);
    }
}
