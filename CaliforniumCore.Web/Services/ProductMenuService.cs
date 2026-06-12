using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.ViewModels.Shared;
using CaliforniumCore.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Services;

/// <summary>
/// Loads the Products dropdown items for the shared public navbar.
/// </summary>
public class ProductMenuService : IProductMenuService
{
    private readonly ApplicationDbContext _dbContext;

    public ProductMenuService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProductMenuItemViewModel>> GetMenuItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .Where(x =>
                x.IsPublished &&
                x.IsActive &&
                x.ShowInProductMenu)
            .OrderBy(x => x.MenuSortOrder)
            .ThenBy(x => x.Title)
            .Select(x => new ProductMenuItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                CategoryName = x.ProductCategory.Name,
                MenuSortOrder = x.MenuSortOrder
            })
            .ToListAsync(cancellationToken);
    }
}
