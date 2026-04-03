using System.Threading;
using System.Threading.Tasks;

namespace EcologicInnovations.Web.Services.Interfaces
{
    public interface ISlugService
    {
        Task<string> GenerateUniqueBlogCategorySlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default);
        Task<string> GenerateUniqueBlogPostSlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default);
        Task<string> GenerateUniqueProductCategorySlugAsync(string source, int? ignoreId = null, CancellationToken cancellationToken = default);
        Task<string> GenerateUniqueProductSlugAsync(string v, CancellationToken cancellationToken, int ignoreId);
        Task<string> GenerateUniqueProductSlugAsync(string v, CancellationToken cancellationToken);
        Task<string> GenerateUniqueSitePageSlugAsync(string v, object value, CancellationToken cancellationToken);
    }
}