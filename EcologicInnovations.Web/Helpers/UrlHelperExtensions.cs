using Microsoft.AspNetCore.Http;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Provides URL-related helper methods used by SEO, uploads, and public routing logic.
/// </summary>
public static class UrlHelperExtensions
{
    /// <summary>
    /// Returns true if the URL is already absolute.
    /// </summary>
    public static bool IsAbsoluteUrl(this string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    /// <summary>
    /// Ensures a relative application path begins with a leading slash.
    /// </summary>
    public static string EnsureLeadingSlash(this string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        return path.StartsWith('/') ? path : "/" + path;
    }

    /// <summary>
    /// Converts a relative application URL to an absolute URL using the current request.
    /// If the input is already absolute, it is returned unchanged.
    /// </summary>
    public static string? ToAbsoluteUrl(this HttpRequest? request, string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        if (url.IsAbsoluteUrl())
        {
            return url;
        }

        if (request is null)
        {
            return url.EnsureLeadingSlash();
        }

        var baseUri = $"{request.Scheme}://{request.Host}";
        return $"{baseUri}{url.EnsureLeadingSlash()}";
    }

    /// <summary>
    /// Returns the current relative path and query string from the request.
    /// </summary>
    public static string? GetRelativePathAndQuery(this HttpRequest? request)
    {
        if (request is null)
        {
            return null;
        }

        return $"{request.PathBase}{request.Path}{request.QueryString}";
    }
}
