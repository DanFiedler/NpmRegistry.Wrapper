using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Serialization;

public class NpmTimeJsonConverter : JsonConverter<NpmTime>
{
    private static readonly HashSet<string> KnownProperties = new(StringComparer.OrdinalIgnoreCase)
    {
        "created",
        "modified",
        "unpublished"
    };

    public override NpmTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var npmTime = new NpmTime();
        var root = document.RootElement;

        if (root.TryGetProperty("created", out var createdElement))
        {
            npmTime.Created = createdElement.GetString() ?? string.Empty;
        }

        if (root.TryGetProperty("modified", out var modifiedElement))
        {
            npmTime.Modified = modifiedElement.GetString() ?? string.Empty;
        }

        if (root.TryGetProperty("unpublished", out var unpublishedElement))
        {
            npmTime.Unpublished = unpublishedElement.Deserialize(ModelsSerializerContext.Default.UnpublishedTime);
        }

        foreach (var property in root.EnumerateObject())
        {
            if (KnownProperties.Contains(property.Name))
            {
                continue;
            }

            if (property.Value.ValueKind == JsonValueKind.String &&
                DateTime.TryParse(property.Value.GetString(), out var dateTime))
            {
                npmTime.VersionTimes[property.Name] = dateTime;
            }
        }

        return npmTime;
    }

    public override void Write(Utf8JsonWriter writer, NpmTime value, JsonSerializerOptions options)
    {
    }
}