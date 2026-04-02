using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcologicInnovations.Web.Controllers;

/// <summary>
/// Public endpoints for robots.txt and sitemap.xml.
/// </summary>
public class SeoController : Controller
{
    private readonly IRobotsService _robotsService;
    private readonly ISitemapService _sitemapService;

    public SeoController(
        IRobotsService robotsService,
        ISitemapService sitemapService)
    {
        _robotsService = robotsService;
        _sitemapService = sitemapService;
    }

    [HttpGet("/robots.txt")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Robots(CancellationToken cancellationToken)
    {
        var content = await _robotsService.RenderAsync(cancellationToken);
        return Content(content, "text/plain; charset=utf-8");
    }

    [HttpGet("/sitemap.xml")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Sitemap(CancellationToken cancellationToken)
    {
        var xml = await _sitemapService.RenderXmlAsync(cancellationToken);
        return Content(xml, "application/xml; charset=utf-8");
    }
}
