namespace EcologicInnovations.Web.Services.Interfaces;

/// <summary>
/// Sanitizes admin-managed HTML content before storage or before render.
/// </summary>
public interface IHtmlSanitizationService
{
    string SanitizeRichHtml(string? html, string? baseUrl = null);

    string SanitizeFooterHtml(string? html, string? baseUrl = null);
}
