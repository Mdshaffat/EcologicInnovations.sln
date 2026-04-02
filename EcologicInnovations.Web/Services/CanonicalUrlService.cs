using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Builds stable canonical URLs for public pages.
/// </summary>
public class CanonicalUrlService : ICanonicalUrlService
{
    private readonly SeoOptions _seoOptions;

    public CanonicalUrlService(IOptions<SeoOptions> seoOptions)
    {
        _seoOptions = seoOptions.Value;
    }

    public string BuildFromCurrentRequest(HttpRequest request, bool includeQueryString = true)
    {
        var root = GetBaseUrl(request);
        var path = request.Path.HasValue ? request.Path.Value! : "/";
        var query = includeQueryString ? request.QueryString.Value ?? string.Empty : string.Empty;

        return $"{root}{path}{query}";
    }

    public string BuildAbsolute(string relativePathOrUrl, HttpRequest? request = null)
    {
        if (string.IsNullOrWhiteSpace(relativePathOrUrl))
        {
            return request is null ? (_seoOptions.BaseUrl ?? string.Empty).TrimEnd('/') : GetBaseUrl(request);
        }

        if (Uri.TryCreate(relativePathOrUrl, UriKind.Absolute, out var absolute))
        {
            return absolute.ToString();
        }

        var root = request is null ? (_seoOptions.BaseUrl ?? string.Empty).TrimEnd('/') : GetBaseUrl(request);
        var path = relativePathOrUrl.StartsWith("/") ? relativePathOrUrl : "/" + relativePathOrUrl;

        return root + path;
    }

    public string NormalizeCanonical(string? value, HttpRequest? request = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return request is null ? (_seoOptions.BaseUrl ?? string.Empty).TrimEnd('/') : BuildFromCurrentRequest(request, includeQueryString: true);
        }

        return BuildAbsolute(value, request);
    }

    private string GetBaseUrl(HttpRequest request)
    {
        if (!string.IsNullOrWhiteSpace(_seoOptions.BaseUrl))
        {
            return _seoOptions.BaseUrl.TrimEnd('/');
        }

        return $"{request.Scheme}://{request.Host.Value}".TrimEnd('/');
    }
}
