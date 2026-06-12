using Ganss.Xss;
using CaliforniumCore.Web.Services.Interfaces;

namespace CaliforniumCore.Web.Services;

/// <summary>
/// Uses HtmlSanitizer to clean admin-managed HTML while still allowing useful layout/content tags.
/// This is intended for About Us, Policy, Contact, Blog content, Product details, and footer HTML.
/// </summary>
public class HtmlSanitizationService : IHtmlSanitizationService
{
    private readonly HtmlSanitizer _richContentSanitizer;
    private readonly HtmlSanitizer _footerSanitizer;

    public HtmlSanitizationService()
    {
        _richContentSanitizer = BuildRichContentSanitizer();
        _footerSanitizer = BuildFooterSanitizer();
    }

    public string SanitizeRichHtml(string? html, string? baseUrl = null)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        return string.IsNullOrWhiteSpace(baseUrl)
            ? _richContentSanitizer.Sanitize(html)
            : _richContentSanitizer.Sanitize(html, baseUrl);
    }

    public string SanitizeFooterHtml(string? html, string? baseUrl = null)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        return string.IsNullOrWhiteSpace(baseUrl)
            ? _footerSanitizer.Sanitize(html)
            : _footerSanitizer.Sanitize(html, baseUrl);
    }

    /// <summary>
    /// Trusted hosts whose content may be embedded via iframe.
    /// </summary>
    private static readonly HashSet<string> TrustedIframeHosts = new(StringComparer.OrdinalIgnoreCase)
    {
        "www.google.com",
        "maps.google.com",
        "www.youtube.com",
        "www.youtube-nocookie.com",
        "player.vimeo.com"
    };

    private static HtmlSanitizer BuildRichContentSanitizer()
    {
        var sanitizer = new HtmlSanitizer();

        // ── Tags ────────────────────────────────────────────────────────────
        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedTags.UnionWith(
        [
            // Text & inline formatting
            "a", "abbr", "b", "bdi", "bdo", "br", "cite", "code", "del", "dfn",
            "em", "i", "ins", "kbd", "mark", "q", "s", "samp", "small", "span",
            "strong", "sub", "sup", "time", "u", "var", "wbr",

            // Headings & block-level
            "address", "article", "aside", "blockquote", "details", "div",
            "figcaption", "figure", "footer", "h1", "h2", "h3", "h4", "h5", "h6",
            "header", "hgroup", "hr", "main", "nav", "p", "pre", "section",
            "summary",

            // Lists
            "dd", "dl", "dt", "li", "ol", "ul",

            // Tables
            "caption", "col", "colgroup", "table", "tbody", "td", "tfoot",
            "th", "thead", "tr",

            // Media & embeds
            "audio", "img", "iframe", "picture", "source", "video",

            // Style block
            "style",

            // Data / progress
            "data", "meter", "progress"
        ]);

        // ── Attributes ──────────────────────────────────────────────────────
        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedAttributes.UnionWith(
        [
            // Global
            "class", "dir", "id", "lang", "style", "tabindex", "title",

            // Links
            "download", "href", "name", "rel", "target",

            // Images & media
            "alt", "autoplay", "controls", "height", "loading", "loop",
            "media", "muted", "poster", "preload", "sizes", "src",
            "srcset", "width",

            // Tables
            "colspan", "headers", "rowspan", "scope",

            // Lists
            "reversed", "start", "type", "value",

            // Iframes
            "allow", "allowfullscreen", "frameborder", "referrerpolicy",
            "sandbox",

            // Semantic / time / data
            "cite", "datetime", "open",

            // Meter / progress
            "high", "low", "max", "min", "optimum",

            // Accessibility
            "aria-label", "aria-hidden", "aria-describedby", "aria-labelledby",
            "role"
        ]);

        // Allow all data-* attributes (used by Bootstrap, editors, etc.)
        sanitizer.AllowDataAttributes = true;

        // ── CSS Properties ──────────────────────────────────────────────────
        sanitizer.AllowedCssProperties.Clear();
        sanitizer.AllowedCssProperties.UnionWith(
        [
            // Background
            "background", "background-color", "background-image",
            "background-position", "background-repeat", "background-size",

            // Borders
            "border", "border-bottom", "border-collapse", "border-color",
            "border-left", "border-radius", "border-right", "border-spacing",
            "border-style", "border-top", "border-width",

            // Box model
            "box-shadow", "height", "margin", "margin-bottom", "margin-left",
            "margin-right", "margin-top", "max-height", "max-width",
            "min-height", "min-width", "padding", "padding-bottom",
            "padding-left", "padding-right", "padding-top", "width",

            // Typography
            "color", "font-family", "font-size", "font-style", "font-weight",
            "letter-spacing", "line-height", "text-align", "text-decoration",
            "text-indent", "text-overflow", "text-transform", "vertical-align",
            "white-space", "word-break", "word-spacing",

            // Display & layout
            "clear", "display", "float", "opacity", "overflow", "overflow-x",
            "overflow-y", "visibility",

            // Positioning
            "bottom", "left", "position", "right", "top", "z-index",

            // Flexbox
            "align-content", "align-items", "align-self", "flex",
            "flex-basis", "flex-direction", "flex-grow", "flex-shrink",
            "flex-wrap", "gap", "justify-content", "order",

            // Grid (basic)
            "grid-column", "grid-row", "grid-template-columns",
            "grid-template-rows",

            // Lists
            "list-style", "list-style-position", "list-style-type",

            // Media
            "object-fit", "object-position"
        ]);

        // ── Schemes ─────────────────────────────────────────────────────────
        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.UnionWith(["http", "https", "mailto", "tel"]);

        // ── Iframe URL filter ───────────────────────────────────────────────
        // Only allow iframes whose src points to a trusted embed host.
        sanitizer.FilterUrl += (_, e) =>
        {
            if (!string.Equals(e.Tag?.LocalName, "iframe", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (Uri.TryCreate(e.SanitizedUrl, UriKind.Absolute, out var uri)
                && TrustedIframeHosts.Contains(uri.Host))
            {
                return;
            }

            e.SanitizedUrl = string.Empty;
        };

        return sanitizer;
    }

    private static HtmlSanitizer BuildFooterSanitizer()
    {
        var sanitizer = new HtmlSanitizer();

        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedTags.UnionWith(["a", "br", "div", "p", "small", "span", "strong"]);

        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedAttributes.UnionWith(["class", "href", "rel", "style", "target", "title"]);

        sanitizer.AllowedCssProperties.Clear();
        sanitizer.AllowedCssProperties.UnionWith(
        [
            "color", "font-size", "font-weight", "margin", "padding",
            "text-align", "text-decoration"
        ]);

        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.UnionWith(["http", "https", "mailto"]);

        return sanitizer;
    }
}
