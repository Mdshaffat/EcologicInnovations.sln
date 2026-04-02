namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Represents one media item card in the admin media library.
/// </summary>
public class MediaLibraryItemViewModel
{
    /// <summary>
    /// Media file primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Stored sanitized file name.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Original uploaded file name.
    /// </summary>
    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>
    /// Public reusable URL.
    /// </summary>
    public string PublicUrl { get; set; } = string.Empty;

    /// <summary>
    /// Relative storage path under uploads.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// MIME type of the stored file.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Optional alt text for image reuse and accessibility.
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Optional friendly title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Optional media grouping label.
    /// </summary>
    public string? MediaGroup { get; set; }

    /// <summary>
    /// Upload timestamp.
    /// </summary>
    public DateTime UploadedAt { get; set; }

    /// <summary>
    /// Current active state.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// True when the file is an image and can show a thumbnail.
    /// </summary>
    public bool IsImage { get; set; }

    /// <summary>
    /// A reusable HTML snippet that admins can copy directly into HTML editors.
    /// </summary>
    public string? CopyHtmlSnippet { get; set; }

    /// <summary>
    /// Human-friendly file size for the UI.
    /// </summary>
    public string FileSizeText { get; set; } = string.Empty;
}
