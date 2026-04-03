using EcologicInnovations.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcologicInnovations.Web.Areas.Admin.Controllers;

/// <summary>
/// Base controller for all Admin area controllers.
/// This ensures consistent area routing and admin-only authorization.
/// </summary>
[Area("Admin")]
[EcologicInnovations.Web.Security.AdminOnly]
public abstract class AdminControllerBase : Controller
{
}
