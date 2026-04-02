using Ganss.Xss;
using EcologicInnovations.Web.Services.Interfaces;

namespace EcologicInnovations.Web.Services;

/// <summary>
/// Uses HtmlSanitizer to clean admin-managed HTML while still allowing useful layout/content tags.
/// This is intended for About Us, Policy, Blog content, Product details, and footer HTML.
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

    private static HtmlSanitizer BuildRichContentSanitizer()
    {
        var sanitizer = new HtmlSanitizer();

        sanitizer.AllowedTags.Clear();
        sanitizer.AllowedTags.UnionWith(
        [
            "a", "article", "aside", "blockquote", "br", "caption", "code",
            "col", "colgroup", "div", "em", "figcaption", "figure",
            "h1", "h2", "h3", "h4", "h5", "h6", "hr", "i", "img",
            "li", "ol", "p", "pre", "section", "small", "span",
            "strong", "sub", "sup", "table", "tbody", "td", "tfoot",
            "th", "thead", "tr", "u", "ul"
        ]);

        sanitizer.AllowedAttributes.Clear();
        sanitizer.AllowedAttributes.UnionWith(
        [
            "alt", "class", "colspan", "height", "href", "id", "rel",
            "rowspan", "src", "style", "target", "title", "width"
        ]);

        sanitizer.AllowedCssProperties.Clear();
        sanitizer.AllowedCssProperties.UnionWith(
        [
            "background-color", "border", "border-bottom", "border-collapse",
            "border-color", "border-left", "border-radius", "border-right",
            "border-style", "border-top", "border-width", "color", "display",
            "font-size", "font-style", "font-weight", "height", "line-height",
            "margin", "margin-bottom", "margin-left", "margin-right",
            "margin-top", "max-width", "padding", "padding-bottom",
            "padding-left", "padding-right", "padding-top", "text-align",
            "text-decoration", "width"
        ]);

        sanitizer.AllowedSchemes.Clear();
        sanitizer.AllowedSchemes.UnionWith(["http", "https", "mailto"]);

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
