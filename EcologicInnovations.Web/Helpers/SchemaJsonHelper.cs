using System.Text.Json;
using System.Text.Json.Serialization;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Serializes schema objects into compact JSON-LD strings.
    /// </summary>
public static class SchemaJsonHelper
{
    private static readonly JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    public static string Serialize(object payload)
    {
        return JsonSerializer.Serialize(payload, _options);
    }
}
