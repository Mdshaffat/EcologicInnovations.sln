using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Models.Seo;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Builds robots.txt using site runtime options and base URL settings.
/// </summary>
public class RobotsService : IRobotsService
{
    private readonly SeoOptions _seoOptions;
    private readonly SiteRuntimeOptions _siteRuntimeOptions;

    public RobotsService(
        IOptions<SeoOptions> seoOptions,
        IOptions<SiteRuntimeOptions> siteRuntimeOptions)
    {
        _seoOptions = seoOptions.Value;
        _siteRuntimeOptions = siteRuntimeOptions.Value;
    }

    public Task<RobotsFileModel> BuildAsync(CancellationToken cancellationToken = default)
    {
        var model = new RobotsFileModel();

        model.Lines.Add("User-agent: *");
        model.Lines.Add("Allow: /");

        if (_siteRuntimeOptions.DisallowAdminInRobots)
        {
            model.Lines.Add("Disallow: /Admin");
            model.Lines.Add("Disallow: /Admin/");
        }

        if (_siteRuntimeOptions.DisallowAccountPagesInRobots)
        {
            model.Lines.Add("Disallow: /Identity/Account");
            model.Lines.Add("Disallow: /Identity/Account/");
        }

        var baseUrl = (_seoOptions.BaseUrl ?? string.Empty).TrimEnd('/');
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            model.Lines.Add($"Sitemap: {baseUrl}/sitemap.xml");
        }

        return Task.FromResult(model);
    }

    public async Task<string> RenderAsync(CancellationToken cancellationToken = default)
    {
        var model = await BuildAsync(cancellationToken);
        return string.Join(Environment.NewLine, model.Lines);
    }
}
