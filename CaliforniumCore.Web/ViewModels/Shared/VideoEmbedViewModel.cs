namespace CaliforniumCore.Web.ViewModels.Shared;

public enum VideoEmbedKind
{
    EmbeddedPlayer,
    DirectVideoFile,
    ExternalLink
}

public class VideoEmbedViewModel
{
    public string SourceUrl { get; set; } = string.Empty;

    public string? EmbedUrl { get; set; }

    public string? DirectVideoUrl { get; set; }

    public string? ExternalUrl { get; set; }

    public string ProviderName { get; set; } = "Video";

    public string IconCssClass { get; set; } = "bi bi-play-circle";

    public string? MimeType { get; set; }

    public VideoEmbedKind Kind { get; set; } = VideoEmbedKind.ExternalLink;

    public bool IsEmbeddedPlayer => Kind == VideoEmbedKind.EmbeddedPlayer && !string.IsNullOrWhiteSpace(EmbedUrl);

    public bool IsDirectVideoFile => Kind == VideoEmbedKind.DirectVideoFile && !string.IsNullOrWhiteSpace(DirectVideoUrl);
}
