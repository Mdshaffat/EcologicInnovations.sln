using System.Threading;
using System.Threading.Tasks;
using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using EcologicInnovations.Web.Services.Interfaces;

namespace EcologicInnovations.Web.Services
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
            throw new NotImplementedException();
        }

        public Task<string> GenerateUniqueProductSlugAsync(string v, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateUniqueSitePageSlugAsync(string v, object value, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}