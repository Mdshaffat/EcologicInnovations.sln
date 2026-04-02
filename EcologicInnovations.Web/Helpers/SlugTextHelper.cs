using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Converts free-form text into URL-safe lowercase slug text.
/// This helper performs the low-level normalization used by SlugService.
/// </summary>
public static partial class SlugTextHelper
{
    /// <summary>
    /// Normalizes arbitrary text into a lowercase hyphen-separated slug.
    /// </summary>
    /// <param name="input">Source text such as a page title or product title.</param>
    /// <param name="maxLength">Maximum desired length of the slug.</param>
    /// <returns>A normalized slug. Returns "item" if nothing usable remains.</returns>
    public static string NormalizeToSlug(string? input, int maxLength = 220)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "item";
        }

        var normalized = input.Trim().ToLowerInvariant();

        // Remove accent marks / diacritics.
        normalized = RemoveDiacritics(normalized);

        // Replace ampersand with a readable separator.
        normalized = normalized.Replace("&", " and ", StringComparison.Ordinal);

        // Replace anything that is not a-z, 0-9 with hyphen.
        normalized = NonAlphaNumericRegex().Replace(normalized, "-");

        // Collapse repeated hyphens and trim.
        normalized = RepeatedDashRegex().Replace(normalized, "-").Trim('-');

        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "item";
        }

        if (normalized.Length > maxLength)
        {
            normalized = normalized[..maxLength].Trim('-');
        }

        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "item";
        }

        return normalized;
    }

    private static string RemoveDiacritics(string input)
    {
        var formD = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var character in formD)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);

            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(character);
            }
        }

        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }

    [GeneratedRegex("[^a-z0-9]+", RegexOptions.Compiled)]
    private static partial Regex NonAlphaNumericRegex();

    [GeneratedRegex("-{2,}", RegexOptions.Compiled)]
    private static partial Regex RepeatedDashRegex();
}
