using Microsoft.AspNetCore.Http;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Builds canonical URLs from the current request or from explicit public paths.
/// </summary>
public interface ICanonicalUrlService
{
    /// <summary>
    /// Builds an absolute canonical URL from the current request.
    /// </summary>
    string BuildFromCurrentRequest(HttpRequest request, bool includeQueryString = true);

    /// <summary>
    /// Builds an absolute canonical URL from a relative public path such as /products/my-product.
    /// </summary>
    string BuildAbsolute(string relativePathOrUrl, HttpRequest? request = null);

    /// <summary>
    /// Normalizes a path-like canonical value to a clean absolute URL.
    /// </summary>
    string NormalizeCanonical(string? value, HttpRequest? request = null);
    string BuildCanonicalUrl(string v);
    string BuildAbsoluteCurrentUrl(HttpRequest request);
    string BuildCanonicalUrl();
}
