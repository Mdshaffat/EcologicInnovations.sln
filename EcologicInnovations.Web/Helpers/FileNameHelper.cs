using System.Text.RegularExpressions;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Contains reusable file name normalization logic for uploaded media.
/// This helps keep stored file names URL-friendly, predictable, and safe.
/// </summary>
public static partial class FileNameHelper
{
    /// <summary>
    /// Sanitizes a display name or original file name into a safe file-name stem.
    /// </summary>
    public static string SanitizeFileNameStem(string? input, int maxLength = 120)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "file";
        }

        var fileName = Path.GetFileNameWithoutExtension(input).Trim().ToLowerInvariant();
        fileName = fileName.Replace("&", " and ", StringComparison.Ordinal);
        fileName = InvalidFileNameRegex().Replace(fileName, "-");
        fileName = RepeatedDashRegex().Replace(fileName, "-").Trim('-', '.');

        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = "file";
        }

        if (fileName.Length > maxLength)
        {
            fileName = fileName[..maxLength].Trim('-', '.');
        }

        return string.IsNullOrWhiteSpace(fileName) ? "file" : fileName;
    }

    /// <summary>
    /// Builds a final stored file name using a sanitized stem, UTC timestamp, and short unique suffix.
    /// </summary>
    public static string BuildStoredFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var stem = SanitizeFileNameStem(originalFileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var uniqueSuffix = Guid.NewGuid().ToString("N")[..8];

        return $"{stem}-{timestamp}-{uniqueSuffix}{extension}";
    }

    [GeneratedRegex(@"[^a-z0-9\-]+", RegexOptions.Compiled)]
    private static partial Regex InvalidFileNameRegex();

    [GeneratedRegex("-{2,}", RegexOptions.Compiled)]
    private static partial Regex RepeatedDashRegex();
}
