using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Serialization;

public class RepositoryJsonConverter : JsonConverter<Repository>
{
    public override Repository? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var root = document.RootElement;
        var repository = new Repository();

        if (root.ValueKind == JsonValueKind.String)
        {
            repository.Url = root.GetString() ?? string.Empty;
        }
        else if (root.ValueKind == JsonValueKind.Object)
        {
            if (root.TryGetProperty("url", out var urlElement) && urlElement.ValueKind == JsonValueKind.String)
            {
                repository.Url = urlElement.GetString() ?? string.Empty;
            }

            if (root.TryGetProperty("type", out var typeElement) && typeElement.ValueKind == JsonValueKind.String)
            {
                repository.Type = typeElement.GetString() ?? string.Empty;
            }

            if (root.TryGetProperty("directory", out var directoryElement) && directoryElement.ValueKind == JsonValueKind.String)
            {
                repository.Directory = directoryElement.GetString() ?? string.Empty;
            }
        }

        return repository;
    }

    public override void Write(Utf8JsonWriter writer, Repository value, JsonSerializerOptions options)
    {
    }
}
