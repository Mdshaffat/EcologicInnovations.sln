namespace EcologicInnovations.Web.Configuration;

/// <summary>
/// Binds the "Media" section from appsettings.
/// This controls where uploaded media is stored, how files are validated,
/// and which public base path is used when generating reusable URLs.
/// </summary>
public class MediaOptions
{
    /// <summary>
    /// Physical upload root relative to the application content root.
    /// Example: "wwwroot/uploads/media"
    /// </summary>
    public string UploadRoot { get; set; } = "wwwroot/uploads/media";

    /// <summary>
    /// Public base path used when generating media URLs for HTML content.
    /// Example: "/uploads/media"
    /// </summary>
    public string PublicBasePath { get; set; } = "/uploads/media";

    /// <summary>
    /// Maximum allowed file size in megabytes.
    /// </summary>
    public int MaxFileSizeMb { get; set; } = 10;

    /// <summary>
    /// Allowed image extensions for image upload scenarios.
    /// </summary>
    public List<string> AllowedImageExtensions { get; set; } =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",
        ".gif"
    ];

    /// <summary>
    /// Allowed document extensions for future reusable media support.
    /// </summary>
    public List<string> AllowedDocumentExtensions { get; set; } =
    [
        ".pdf"
    ];
}
