using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EcologicInnovations.Web.Controllers;

/// <summary>
/// Handles all public error pages (404, 403, 500, etc.) in a user-friendly,
/// deployment-ready way. No stack traces or technical details are ever exposed.
/// </summary>
[AllowAnonymous]
[Route("error")]
public class ErrorController : Controller
{
    /// <summary>
    /// Catches unhandled exceptions via UseExceptionHandler("/error").
    /// </summary>
    [HttpGet("")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var model = new ErrorPageViewModel
        {
            StatusCode = 500,
            Title = "We hit an unexpected problem",
            Message = "Something went wrong on our end. Our team has been notified and is looking into it. Please try again in a moment.",
            PrimaryButtonText = "Go to Home Page",
            PrimaryButtonUrl = Url.Action("Index", "Home") ?? "/",
            SecondaryButtonText = "Contact Support",
            SecondaryButtonUrl = Url.Action("Index", "Contact") ?? "/contact",
            IconCssClass = "bi bi-exclamation-octagon"
        };

        Response.StatusCode = 500;
        SetErrorSeo("Server Error");
        return View("Error", model);
    }

    /// <summary>
    /// Handles HTTP status codes via UseStatusCodePagesWithReExecute("/error/status/{0}").
    /// Each common status code gets a unique, human-friendly message.
    /// </summary>
    [HttpGet("status/{code:int}")]
    public IActionResult StatusCodeHandler(int code)
    {
        var homeUrl = Url.Action("Index", "Home") ?? "/";
        var contactUrl = Url.Action("Index", "Contact") ?? "/contact";
        var productsUrl = Url.Action("Index", "Products") ?? "/products";

        var model = code switch
        {
            400 => new ErrorPageViewModel
            {
                StatusCode = 400,
                Title = "Bad request",
                Message = "We couldn't understand that request. Please check the URL or form data and try again.",
                PrimaryButtonText = "Go to Home Page",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Contact Support",
                SecondaryButtonUrl = contactUrl,
                IconCssClass = "bi bi-x-circle"
            },
            401 => new ErrorPageViewModel
            {
                StatusCode = 401,
                Title = "Sign in required",
                Message = "You need to sign in before accessing this page. Please log in and try again.",
                PrimaryButtonText = "Sign In",
                PrimaryButtonUrl = "/Identity/Account/Login",
                SecondaryButtonText = "Go to Home Page",
                SecondaryButtonUrl = homeUrl,
                IconCssClass = "bi bi-lock"
            },
            403 => new ErrorPageViewModel
            {
                StatusCode = 403,
                Title = "Access denied",
                Message = "You don't have permission to view this page. If you believe this is a mistake, please reach out to us.",
                PrimaryButtonText = "Go to Home Page",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Contact Support",
                SecondaryButtonUrl = contactUrl,
                IconCssClass = "bi bi-shield-lock"
            },
            404 => new ErrorPageViewModel
            {
                StatusCode = 404,
                Title = "Page not found",
                Message = "The page you're looking for doesn't exist. It may have been moved, renamed, or removed.",
                PrimaryButtonText = "Browse Products",
                PrimaryButtonUrl = productsUrl,
                SecondaryButtonText = "Go to Home Page",
                SecondaryButtonUrl = homeUrl,
                IconCssClass = "bi bi-search"
            },
            405 => new ErrorPageViewModel
            {
                StatusCode = 405,
                Title = "Action not allowed",
                Message = "This action isn't supported for the requested page. Please navigate using the site menus.",
                PrimaryButtonText = "Go to Home Page",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Browse Products",
                SecondaryButtonUrl = productsUrl,
                IconCssClass = "bi bi-slash-circle"
            },
            408 => new ErrorPageViewModel
            {
                StatusCode = 408,
                Title = "Request timed out",
                Message = "The server took too long to respond. Please check your connection and try again.",
                PrimaryButtonText = "Try Again",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Contact Support",
                SecondaryButtonUrl = contactUrl,
                IconCssClass = "bi bi-clock-history"
            },
            429 => new ErrorPageViewModel
            {
                StatusCode = 429,
                Title = "Too many requests",
                Message = "You've made too many requests in a short period. Please wait a moment and try again.",
                PrimaryButtonText = "Go to Home Page",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Contact Support",
                SecondaryButtonUrl = contactUrl,
                IconCssClass = "bi bi-hourglass-split"
            },
            502 => new ErrorPageViewModel
            {
                StatusCode = 502,
                Title = "Service temporarily unavailable",
                Message = "We're experiencing a temporary issue with our servers. Please try again in a few moments.",
                PrimaryButtonText = "Try Again",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Contact Support",
                SecondaryButtonUrl = contactUrl,
                IconCssClass = "bi bi-cloud-slash"
            },
            503 => new ErrorPageViewModel
            {
                StatusCode = 503,
                Title = "Under maintenance",
                Message = "We're performing scheduled maintenance to improve your experience. Please check back shortly.",
                PrimaryButtonText = "Try Again",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Contact Support",
                SecondaryButtonUrl = contactUrl,
                IconCssClass = "bi bi-tools"
            },
            _ => new ErrorPageViewModel
            {
                StatusCode = code,
                Title = "Something went wrong",
                Message = "An unexpected error occurred. Please try again or return to the home page.",
                PrimaryButtonText = "Go to Home Page",
                PrimaryButtonUrl = homeUrl,
                SecondaryButtonText = "Contact Support",
                SecondaryButtonUrl = contactUrl,
                IconCssClass = "bi bi-exclamation-circle"
            }
        };

        Response.StatusCode = code;
        SetErrorSeo(model.Title);

        return code == 404
            ? View("NotFound", model)
            : View("Error", model);
    }

    /// <summary>
    /// Sets SEO metadata on ViewData so _SeoHead.cshtml renders noindex and a proper title.
    /// </summary>
    private void SetErrorSeo(string title)
    {
        ViewData["Title"] = title;
        ViewData["Seo"] = new SeoMetaViewModel
        {
            Title = title,
            Robots = "noindex,nofollow"
        };
    }
}
