using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Services;

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
