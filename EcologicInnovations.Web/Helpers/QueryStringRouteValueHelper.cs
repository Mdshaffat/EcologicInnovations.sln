using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Builds pagination URLs while preserving existing querystring filters and sort values.
/// This is used by the shared pagination partial.
/// </summary>
public static class QueryStringRouteValueHelper
{
    /// <summary>
    /// Creates a new URL for the target page number while preserving all current query values
    /// except the page parameter that will be replaced.
    /// </summary>
    public static string BuildPageUrl(
        string path,
        IReadOnlyDictionary<string, string?> query,
        int targetPage,
        string pageParameterName = "page")
    {
        var values = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (var kv in query)
        {
            if (string.Equals(kv.Key, pageParameterName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            values[kv.Key] = kv.Value;
        }

        values[pageParameterName] = targetPage.ToString();

        return QueryHelpers.AddQueryString(path, values!);
    }

    /// <summary>
    /// Converts the current request querystring into a string dictionary
    /// suitable for pagination rendering.
    /// </summary>
    public static Dictionary<string, string?> ToSimpleDictionary(IQueryCollection query)
    {
        var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in query)
        {
            result[item.Key] = item.Value.ToString();
        }

        return result;
    }
}
