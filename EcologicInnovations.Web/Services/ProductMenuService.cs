using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.ViewModels.Shared;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Services;

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
                x.ShowInProductMenu &&
                x.ProductCategory != null &&
                x.ProductCategory.IsActive)
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
