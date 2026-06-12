using System.Text.RegularExpressions;
using CaliforniumCore.Web.Services.Interfaces;
using CaliforniumCore.Web.ViewModels.Shared;

namespace CaliforniumCore.Web.Services;

public class VideoEmbedService : IVideoEmbedService
{
    private static readonly Regex IframeSrcRegex = new(
        "src\\s*=\\s*[\"'](?<url>[^\"']+)[\"']",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public string? NormalizeSourceUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = System.Net.WebUtility.HtmlDecode(value.Trim());
        var iframeSrc = IframeSrcRegex.Match(trimmed);
        if (iframeSrc.Success)
        {
            trimmed = System.Net.WebUtility.HtmlDecode(iframeSrc.Groups["url"].Value.Trim());
        }

        if (trimmed.StartsWith('/') && !trimmed.StartsWith("//"))
        {
            return IsDirectVideoPath(trimmed) ? trimmed : null;
        }

        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri) &&
            Uri.TryCreate($"https://{trimmed}", UriKind.Absolute, out uri))
        {
            trimmed = uri.ToString();
        }

        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out uri) ||
            (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        {
            return null;
        }

        return trimmed;
    }

    public VideoEmbedViewModel? BuildEmbed(string? value)
    {
        var sourceUrl = NormalizeSourceUrl(value);
        if (sourceUrl is null)
        {
            return null;
        }

        if (IsDirectVideoPath(sourceUrl))
        {
            return new VideoEmbedViewModel
            {
                SourceUrl = sourceUrl,
                DirectVideoUrl = sourceUrl,
                ProviderName = "Video file",
                IconCssClass = "bi bi-file-play",
                MimeType = GetVideoMimeType(sourceUrl),
                Kind = VideoEmbedKind.DirectVideoFile
            };
        }

        if (!Uri.TryCreate(sourceUrl, UriKind.Absolute, out var uri))
        {
            return null;
        }

        if (TryBuildYouTube(uri, sourceUrl, out var youtube))
        {
            return youtube;
        }

        if (TryBuildVimeo(uri, sourceUrl, out var vimeo))
        {
            return vimeo;
        }

        if (TryBuildDailymotion(uri, sourceUrl, out var dailymotion))
        {
            return dailymotion;
        }

        if (TryBuildWistia(uri, sourceUrl, out var wistia))
        {
            return wistia;
        }

        return new VideoEmbedViewModel
        {
            SourceUrl = sourceUrl,
            ExternalUrl = sourceUrl,
            ProviderName = BuildProviderName(uri),
            IconCssClass = "bi bi-box-arrow-up-right",
            Kind = VideoEmbedKind.ExternalLink
        };
    }

    private static bool TryBuildYouTube(Uri uri, string sourceUrl, out VideoEmbedViewModel? model)
    {
        model = null;
        var host = NormalizeHost(uri.Host);
        if (host is not ("youtube.com" or "youtu.be" or "youtube-nocookie.com"))
        {
            return false;
        }

        var videoId = ExtractYouTubeId(uri);
        if (!IsSafeVideoId(videoId))
        {
            return false;
        }

        var embedUrl = $"https://www.youtube-nocookie.com/embed/{videoId}";
        var startSeconds = ExtractYouTubeStartSeconds(uri);
        if (startSeconds > 0)
        {
            embedUrl += $"?start={startSeconds}";
        }

        model = new VideoEmbedViewModel
        {
            SourceUrl = sourceUrl,
            EmbedUrl = embedUrl,
            ProviderName = "YouTube",
            IconCssClass = "bi bi-youtube",
            Kind = VideoEmbedKind.EmbeddedPlayer
        };

        return true;
    }

    private static bool TryBuildVimeo(Uri uri, string sourceUrl, out VideoEmbedViewModel? model)
    {
        model = null;
        var host = NormalizeHost(uri.Host);
        if (host is not ("vimeo.com" or "player.vimeo.com"))
        {
            return false;
        }

        var videoId = uri.AbsolutePath
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault(segment => segment.All(char.IsDigit));

        if (string.IsNullOrWhiteSpace(videoId))
        {
            return false;
        }

        model = new VideoEmbedViewModel
        {
            SourceUrl = sourceUrl,
            EmbedUrl = $"https://player.vimeo.com/video/{videoId}",
            ProviderName = "Vimeo",
            IconCssClass = "bi bi-vimeo",
            Kind = VideoEmbedKind.EmbeddedPlayer
        };

        return true;
    }

    private static bool TryBuildDailymotion(Uri uri, string sourceUrl, out VideoEmbedViewModel? model)
    {
        model = null;
        var host = NormalizeHost(uri.Host);
        if (host is not ("dailymotion.com" or "dai.ly"))
        {
            return false;
        }

        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var videoId = host == "dai.ly"
            ? segments.FirstOrDefault()
            : segments.Length >= 2 && string.Equals(segments[0], "video", StringComparison.OrdinalIgnoreCase)
                ? segments[1]
                : null;

        videoId = videoId?.Split('_', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(videoId) || !Regex.IsMatch(videoId, "^[A-Za-z0-9]+$", RegexOptions.CultureInvariant))
        {
            return false;
        }

        model = new VideoEmbedViewModel
        {
            SourceUrl = sourceUrl,
            EmbedUrl = $"https://www.dailymotion.com/embed/video/{videoId}",
            ProviderName = "Dailymotion",
            IconCssClass = "bi bi-play-btn",
            Kind = VideoEmbedKind.EmbeddedPlayer
        };

        return true;
    }

    private static bool TryBuildWistia(Uri uri, string sourceUrl, out VideoEmbedViewModel? model)
    {
        model = null;
        var host = NormalizeHost(uri.Host);
        if (host is not ("wistia.com" or "fast.wistia.net"))
        {
            return false;
        }

        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var videoId = host == "fast.wistia.net" && segments.Length >= 3
            ? segments[2]
            : segments.Length >= 2 && string.Equals(segments[0], "medias", StringComparison.OrdinalIgnoreCase)
                ? segments[1]
                : null;

        if (string.IsNullOrWhiteSpace(videoId) || !Regex.IsMatch(videoId, "^[A-Za-z0-9]+$", RegexOptions.CultureInvariant))
        {
            return false;
        }

        model = new VideoEmbedViewModel
        {
            SourceUrl = sourceUrl,
            EmbedUrl = $"https://fast.wistia.net/embed/iframe/{videoId}",
            ProviderName = "Wistia",
            IconCssClass = "bi bi-play-btn",
            Kind = VideoEmbedKind.EmbeddedPlayer
        };

        return true;
    }

    private static string? ExtractYouTubeId(Uri uri)
    {
        var host = NormalizeHost(uri.Host);
        var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (host == "youtu.be")
        {
            return segments.FirstOrDefault();
        }

        if (segments.Length >= 2 &&
            (segments[0].Equals("embed", StringComparison.OrdinalIgnoreCase) ||
             segments[0].Equals("shorts", StringComparison.OrdinalIgnoreCase) ||
             segments[0].Equals("live", StringComparison.OrdinalIgnoreCase)))
        {
            return segments[1];
        }

        return GetQueryParameter(uri, "v");
    }

    private static int ExtractYouTubeStartSeconds(Uri uri)
    {
        var start = GetQueryParameter(uri, "start") ?? GetQueryParameter(uri, "t");
        if (string.IsNullOrWhiteSpace(start))
        {
            return 0;
        }

        if (int.TryParse(start.TrimEnd('s'), out var seconds))
        {
            return Math.Max(0, seconds);
        }

        var match = Regex.Match(start, "^(?:(?<h>\\d+)h)?(?:(?<m>\\d+)m)?(?:(?<s>\\d+)s)?$", RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return 0;
        }

        var total = 0;
        if (int.TryParse(match.Groups["h"].Value, out var hours))
        {
            total += hours * 3600;
        }

        if (int.TryParse(match.Groups["m"].Value, out var minutes))
        {
            total += minutes * 60;
        }

        if (int.TryParse(match.Groups["s"].Value, out var parsedSeconds))
        {
            total += parsedSeconds;
        }

        return Math.Max(0, total);
    }

    private static string? GetQueryParameter(Uri uri, string name)
    {
        var query = uri.Query.TrimStart('?');
        if (string.IsNullOrWhiteSpace(query))
        {
            return null;
        }

        foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = pair.Split('=', 2);
            var key = Uri.UnescapeDataString(parts[0]);
            if (!string.Equals(key, name, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            return parts.Length == 2 ? Uri.UnescapeDataString(parts[1]) : string.Empty;
        }

        return null;
    }

    private static bool IsSafeVideoId(string? value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               Regex.IsMatch(value, "^[A-Za-z0-9_-]{6,}$", RegexOptions.CultureInvariant);
    }

    private static string NormalizeHost(string host)
    {
        var normalized = host.Trim().ToLowerInvariant();
        return normalized.StartsWith("www.", StringComparison.OrdinalIgnoreCase)
            ? normalized[4..]
            : normalized;
    }

    private static bool IsDirectVideoPath(string value)
    {
        var path = value;
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            path = uri.AbsolutePath;
        }

        var extension = Path.GetExtension(path).ToLowerInvariant();
        return extension is ".mp4" or ".webm" or ".ogg";
    }

    private static string? GetVideoMimeType(string value)
    {
        var path = value;
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            path = uri.AbsolutePath;
        }

        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".ogg" => "video/ogg",
            _ => null
        };
    }

    private static string BuildProviderName(Uri uri)
    {
        var host = NormalizeHost(uri.Host);
        var firstPart = host.Split('.', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(firstPart))
        {
            return "External video";
        }

        return char.ToUpperInvariant(firstPart[0]) + firstPart[1..];
    }
}
