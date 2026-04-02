using EcologicInnovations.Web.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Handles file validation, physical storage, and MediaFile metadata creation.
/// </summary>
public interface IFileUploadService
{
    Task<MediaFile> SaveMediaAsync(
        IFormFile file,
        string mediaGroup,
        string? title = null,
        string? altText = null,
        string? uploadedByUserId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MediaFile>> SaveMediaAsync(
        IEnumerable<IFormFile> files,
        string mediaGroup,
        string? uploadedByUserId = null,
        CancellationToken cancellationToken = default);

    Task DeleteMediaAsync(MediaFile mediaFile, bool deleteDatabaseRow = false, CancellationToken cancellationToken = default);

    bool IsAllowedFile(string fileName);

    long GetMaxFileSizeBytes();
}
