using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Serialization;

/// <summary>
/// Handles the engines field which can be an object ({"node":">=14"}) or an array (["node","rhino"]).
/// When an array is encountered, each element becomes a key with an empty string value.
/// </summary>
public class EnginesJsonConverter : JsonConverter<Dictionary<string, string>>
{
    public override Dictionary<string, string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var dict = new Dictionary<string, string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                string key = reader.GetString() ?? string.Empty;
                reader.Read();
                string value = reader.GetString() ?? string.Empty;
                dict[key] = value;
            }
            return dict;
        }

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var dict = new Dictionary<string, string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                string key = reader.GetString() ?? string.Empty;
                dict[key] = string.Empty;
            }
            return dict;
        }

        return [];
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, string> value, JsonSerializerOptions options)
    {
    }
}
