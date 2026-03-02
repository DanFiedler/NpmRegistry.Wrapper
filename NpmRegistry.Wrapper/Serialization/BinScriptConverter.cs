using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Serialization;
public class BinScriptConverter : JsonConverter<BinScriptCollection>
{
    public override BinScriptCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var collection = new BinScriptCollection();

        var root = document.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            collection.BinScripts.Add(new BinScript
            {
                Name = string.Empty,
                Path = root.GetString() ?? string.Empty
            });
        }
        else
        {
            foreach (var jsonElement in root.EnumerateObject())
            {
                string name = jsonElement.Name;
                string path = jsonElement.Value.ToString();
                var script = new BinScript
                {
                    Name = name,
                    Path = path
                };
                collection.BinScripts.Add(script);
            }
        }

        return collection;
    }

    public override void Write(Utf8JsonWriter writer, BinScriptCollection value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
