using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EcologicInnovations.Web.Controllers;

/// <summary>
/// Handles public 404 and unhandled server-error pages in a user-friendly way.
/// </summary>
[AllowAnonymous]
[Route("error")]
public class ErrorController : Controller
{
    [HttpGet("")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var model = new ErrorPageViewModel
        {
            StatusCode = 500,
            Title = "We hit an unexpected problem",
            Message = "The page could not be completed right now. Please try again, or go back to the home page.",
            PrimaryButtonText = "Go Home",
            PrimaryButtonUrl = Url.Action("Index", "Home") ?? "/",
            SecondaryButtonText = "Contact Us",
            SecondaryButtonUrl = Url.Action("Index", "Contact") ?? "/contact",
            IconCssClass = "bi bi-exclamation-octagon"
        };

        Response.StatusCode = 500;
        return View("Error", model);
    }

    [HttpGet("status/{code:int}")]
    public IActionResult StatusCodeHandler(int code)
    {
        if (code == 404)
        {
            Response.StatusCode = 404;

            var notFoundModel = new ErrorPageViewModel
            {
                StatusCode = 404,
                Title = "Page not found",
                Message = "The page you are looking for may have been removed, renamed, or never existed.",
                PrimaryButtonText = "Browse Products",
                PrimaryButtonUrl = Url.Action("Index", "Products") ?? "/products",
                SecondaryButtonText = "Go Home",
                SecondaryButtonUrl = Url.Action("Index", "Home") ?? "/",
                IconCssClass = "bi bi-search"
            };

            return View("NotFound", notFoundModel);
        }

        Response.StatusCode = code;

        var model = new ErrorPageViewModel
        {
            StatusCode = code,
            Title = "Something went wrong",
            Message = "The request could not be completed. Please try again or return to a safe page.",
            PrimaryButtonText = "Go Home",
            PrimaryButtonUrl = Url.Action("Index", "Home") ?? "/",
            SecondaryButtonText = "Contact Us",
            SecondaryButtonUrl = Url.Action("Index", "Contact") ?? "/contact",
            IconCssClass = "bi bi-exclamation-circle"
        };

        return View("Error", model);
    }
}
