using CaliforniumCore.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaliforniumCore.Web.Areas.Admin.Controllers;

/// <summary>
/// Base controller for all Admin area controllers.
/// This ensures consistent area routing and admin-only authorization.
/// </summary>
[Area("Admin")]
[CaliforniumCore.Web.Security.AdminOnly]
public abstract class AdminControllerBase : Controller
{
}
