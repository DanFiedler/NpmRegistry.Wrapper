using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Serialization;

public class PersonJsonConverter : JsonConverter<Person>
{
    public override Person? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var root = document.RootElement;
        if(root.ValueKind == JsonValueKind.String)
        {
            var person = new Person();
            string? name = root.GetString();
            if(!string.IsNullOrEmpty(name))
            {
                person.Name = name;
            }
            return person;
        }
        else if(root.ValueKind == JsonValueKind.Object)
        {
            var person = new Person();
            if (root.TryGetProperty("name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String)
            {
                person.Name = nameElement.GetString() ?? string.Empty;
            }
            if (root.TryGetProperty("email", out var emailElement) && emailElement.ValueKind == JsonValueKind.String)
            {
                person.Email = emailElement.GetString() ?? string.Empty;
            }
            return person;
        }

        return new Person();
    }

    public override void Write(Utf8JsonWriter writer, Person value, JsonSerializerOptions options)
    {
    }
}
