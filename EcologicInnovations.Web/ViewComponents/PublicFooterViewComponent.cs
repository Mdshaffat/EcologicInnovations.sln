using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EcologicInnovations.Web.ViewComponents;

/// <summary>
/// Builds the shared footer model used by the public site footer.
/// </summary>
public class PublicFooterViewComponent : ViewComponent
{
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly IHtmlSanitizationService _htmlSanitizationService;

    public PublicFooterViewComponent(
        ISiteSettingsService siteSettingsService,
        IHtmlSanitizationService htmlSanitizationService)
    {
        _siteSettingsService = siteSettingsService;
        _htmlSanitizationService = htmlSanitizationService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync();

        var model = new PublicFooterViewModel
        {
            SiteName = string.IsNullOrWhiteSpace(siteSettings.CompanyName)
                ? "Ecologic Innovations"
                : siteSettings.CompanyName,
            Tagline = siteSettings.Tagline,
            LogoUrl = siteSettings.LogoUrl,
            SupportEmail = siteSettings.SupportEmail,
            SalesEmail = siteSettings.SalesEmail,
            Phone = siteSettings.Phone,
            Address = siteSettings.Address,
            FooterHtml = string.IsNullOrWhiteSpace(siteSettings.FooterHtml)
                ? null
                : _htmlSanitizationService.SanitizeFooterHtml(siteSettings.FooterHtml),
            FacebookUrl = siteSettings.FacebookUrl,
            LinkedInUrl = siteSettings.LinkedInUrl,
            YouTubeUrl = siteSettings.YouTubeUrl,
            CurrentYear = DateTime.UtcNow.Year
        };

        return View("_PublicFooter", model);
    }
}
