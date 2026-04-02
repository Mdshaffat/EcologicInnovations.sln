using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EcologicInnovations.Web.ViewComponents;

/// <summary>
/// Builds the shared public navigation model used by the site header/navbar.
/// </summary>
public class PublicNavigationViewComponent : ViewComponent
{
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly IProductMenuService _productMenuService;

    public PublicNavigationViewComponent(
        ISiteSettingsService siteSettingsService,
        IProductMenuService productMenuService)
    {
        _siteSettingsService = siteSettingsService;
        _productMenuService = productMenuService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync();
        var menuItems = await _productMenuService.GetMenuItemsAsync();

        var model = new PublicNavigationViewModel
        {
            SiteName = string.IsNullOrWhiteSpace(siteSettings.CompanyName)
                ? "Ecologic Innovations"
                : siteSettings.CompanyName,
            LogoUrl = siteSettings.LogoUrl,
            Tagline = siteSettings.Tagline,
            CurrentPath = HttpContext?.Request?.Path.Value ?? "/",
            ProductMenuItems = menuItems
        };

        return View("_PublicNavbar", model);
    }
}
