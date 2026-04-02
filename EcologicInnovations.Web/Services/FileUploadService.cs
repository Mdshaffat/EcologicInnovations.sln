using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Saves uploaded media under wwwroot/uploads/media and persists MediaFile metadata.
/// This service centralizes validation, naming, folder generation, and public URL creation.
/// </summary>
public class FileUploadService : IFileUploadService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;
    private readonly MediaOptions _mediaOptions;

    public FileUploadService(
        ApplicationDbContext dbContext,
        IWebHostEnvironment environment,
        IOptions<MediaOptions> mediaOptions)
    {
        _dbContext = dbContext;
        _environment = environment;
        _mediaOptions = mediaOptions.Value;
    }

    public async Task<MediaFile> SaveMediaAsync(
        IFormFile file,
        string mediaGroup,
        string? title = null,
        string? altText = null,
        string? uploadedByUserId = null,
        CancellationToken cancellationToken = default)
    {
        ValidateFile(file);

        var normalizedGroup = FileNameHelper.SanitizeFileNameStem(mediaGroup, 40);
        var storedFileName = FileNameHelper.BuildStoredFileName(file.FileName);

        var now = DateTime.UtcNow;
        var relativeFolder = Path.Combine(
            "uploads",
            "media",
            normalizedGroup,
            now.ToString("yyyy"),
            now.ToString("MM"));

        var relativeFilePath = Path.Combine(relativeFolder, storedFileName)
            .Replace("\\", "/", StringComparison.Ordinal);

        var physicalRoot = ResolvePhysicalUploadRoot();
        var physicalDirectory = Path.Combine(
            physicalRoot,
            normalizedGroup,
            now.ToString("yyyy"),
            now.ToString("MM"));

        Directory.CreateDirectory(physicalDirectory);

        var physicalPath = Path.Combine(physicalDirectory, storedFileName);

        await using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var publicUrl = $"{_mediaOptions.PublicBasePath.TrimEnd('/')}/{normalizedGroup}/{now:yyyy}/{now:MM}/{storedFileName}";

        var mediaFile = new MediaFile
        {
            FileName = storedFileName,
            OriginalFileName = Path.GetFileName(file.FileName),
            FilePath = relativeFilePath,
            PublicUrl = publicUrl,
            ContentType = file.ContentType ?? "application/octet-stream",
            FileSize = file.Length,
            AltText = altText,
            Title = title,
            MediaGroup = mediaGroup,
            UploadedByUserId = uploadedByUserId,
            UploadedAt = now,
            IsActive = true
        };

        _dbContext.MediaFiles.Add(mediaFile);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return mediaFile;
    }

    public async Task<IReadOnlyList<MediaFile>> SaveMediaAsync(
        IEnumerable<IFormFile> files,
        string mediaGroup,
        string? uploadedByUserId = null,
        CancellationToken cancellationToken = default)
    {
        var results = new List<MediaFile>();

        foreach (var file in files.Where(x => x is not null && x.Length > 0))
        {
            var saved = await SaveMediaAsync(
                file,
                mediaGroup,
                title: null,
                altText: null,
                uploadedByUserId: uploadedByUserId,
                cancellationToken: cancellationToken);

            results.Add(saved);
        }

        return results;
    }

    public async Task DeleteMediaAsync(MediaFile mediaFile, bool deleteDatabaseRow = false, CancellationToken cancellationToken = default)
    {
        if (mediaFile is null)
        {
            return;
        }

        var physicalPath = Path.Combine(_environment.ContentRootPath, mediaFile.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal));

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }

        if (deleteDatabaseRow)
        {
            _dbContext.MediaFiles.Remove(mediaFile);
        }
        else
        {
            mediaFile.IsActive = false;
            _dbContext.MediaFiles.Update(mediaFile);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public bool IsAllowedFile(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return _mediaOptions.AllowedImageExtensions.Contains(extension)
               || _mediaOptions.AllowedDocumentExtensions.Contains(extension);
    }

    public long GetMaxFileSizeBytes()
    {
        return _mediaOptions.MaxFileSizeMb * 1024L * 1024L;
    }

    private void ValidateFile(IFormFile file)
    {
        if (file is null || file.Length <= 0)
        {
            throw new InvalidOperationException("The uploaded file is empty.");
        }

        if (!IsAllowedFile(file.FileName))
        {
            throw new InvalidOperationException("The uploaded file type is not allowed.");
        }

        if (file.Length > GetMaxFileSizeBytes())
        {
            throw new InvalidOperationException($"The uploaded file exceeds the maximum allowed size of {_mediaOptions.MaxFileSizeMb} MB.");
        }
    }

    private string ResolvePhysicalUploadRoot()
    {
        var configuredRoot = _mediaOptions.UploadRoot.Replace("/", Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);

        if (Path.IsPathRooted(configuredRoot))
        {
            return configuredRoot;
        }

        return Path.Combine(_environment.ContentRootPath, configuredRoot);
    }
}
