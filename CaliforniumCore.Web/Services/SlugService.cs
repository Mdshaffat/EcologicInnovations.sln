using System.Threading;
using System.Threading.Tasks;
using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using CaliforniumCore.Web.Services.Interfaces;

namespace CaliforniumCore.Web.Services
{
    public class SlugService : ISlugService
    {
        private readonly ApplicationDbContext _dbContext;

        public SlugService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateUniqueBlogCategorySlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default)
        {
            var baseSlug = SlugTextHelper.NormalizeToSlug(source);
            var slug = baseSlug;
            var counter = 2;

            while (await _dbContext.BlogCategories
                .AsNoTracking()
                .AnyAsync(x => x.Slug == slug && (!ignoreId.HasValue || x.Id != ignoreId.Value), cancellationToken))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        public async Task<string> GenerateUniqueBlogPostSlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default)
        {
            var baseSlug = SlugTextHelper.NormalizeToSlug(source);
            var slug = baseSlug;
            var counter = 2;

            while (await _dbContext.BlogPosts
                .AsNoTracking()
                .AnyAsync(x => x.Slug == slug && (!ignoreId.HasValue || x.Id != ignoreId.Value), cancellationToken))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        public async Task<string> GenerateUniqueProductCategorySlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default)
        {
            var baseSlug = SlugTextHelper.NormalizeToSlug(source);
            var slug = baseSlug;
            var counter = 2;

            while (await _dbContext.ProductCategories
                .AsNoTracking()
                .AnyAsync(x => x.Slug == slug && (!ignoreId.HasValue || x.Id != ignoreId.Value), cancellationToken))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        public Task<string> GenerateUniqueProductSlugAsync(string v, CancellationToken cancellationToken, int ignoreId)
        {
            return GenerateUniqueProductSlugAsync(v, cancellationToken, (int?)ignoreId);
        }

        public Task<string> GenerateUniqueProductSlugAsync(string v, CancellationToken cancellationToken)
        {
            return GenerateUniqueProductSlugAsync(v, cancellationToken, (int?)null);
        }

        private async Task<string> GenerateUniqueProductSlugAsync(string source, CancellationToken cancellationToken, int? ignoreId)
        {
            var baseSlug = SlugTextHelper.NormalizeToSlug(source);
            var slug = baseSlug;
            var counter = 2;

            while (await _dbContext.Products
                .AsNoTracking()
                .AnyAsync(x => x.Slug == slug && (!ignoreId.HasValue || x.Id != ignoreId.Value), cancellationToken))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        public async Task<string> GenerateUniqueSitePageSlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default)
        {
            var baseSlug = SlugTextHelper.NormalizeToSlug(source);
            var slug = baseSlug;
            var counter = 2;

            while (await _dbContext.SitePages
                .AsNoTracking()
                .AnyAsync(x => x.Slug == slug && (!ignoreId.HasValue || x.Id != ignoreId.Value), cancellationToken))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }
    }
}
