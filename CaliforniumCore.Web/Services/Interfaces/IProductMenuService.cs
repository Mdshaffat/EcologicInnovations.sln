using CaliforniumCore.Web.ViewModels.Shared;

namespace CaliforniumCore.Web.Services.Interfaces;

/// <summary>
/// Provides the product items used in the shared public Products dropdown.
/// </summary>
public interface IProductMenuService
{
    /// <summary>
    /// Gets published and active products that are explicitly configured
    /// to appear inside the shared top navigation dropdown.
    /// </summary>
    Task<List<ProductMenuItemViewModel>> GetMenuItemsAsync(CancellationToken cancellationToken = default);
}
