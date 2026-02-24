using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Serialization;

public class BugsJsonConverter : JsonConverter<Bugs>
{
    public override Bugs? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var root = document.RootElement;
        var bugs = new Bugs();

        if (root.ValueKind == JsonValueKind.String)
        {
            bugs.Url = root.GetString() ?? string.Empty;
        }
        else if (root.TryGetProperty("url", out var urlElement) && urlElement.ValueKind == JsonValueKind.String)
        {
            bugs.Url = urlElement.GetString() ?? string.Empty;
        }
        else if (root.TryGetProperty("name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String)
        {
            bugs.Url = nameElement.GetString() ?? string.Empty;
        }

        return bugs;
    }

    public override void Write(Utf8JsonWriter writer, Bugs value, JsonSerializerOptions options)
    {
    }
}
