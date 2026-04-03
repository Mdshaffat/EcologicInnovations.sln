using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcologicInnovations.Web.Security;

/// <summary>
/// Authorization filter that only allows users in the Admin role to access the resource.
/// When the user is not an admin (or not authenticated) it returns 404 Not Found to hide
/// the existence of the admin area from unauthorized users.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class AdminOnlyAttribute : Attribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Allow if the user is authenticated and in the Admin role
        if (user?.Identity?.IsAuthenticated == true && user.IsInRole(AppRoles.Admin))
        {
            return Task.CompletedTask;
        }

        // Hide admin routes by returning NotFound for unauthorized requests
        context.Result = new NotFoundResult();
        return Task.CompletedTask;
    }
}
