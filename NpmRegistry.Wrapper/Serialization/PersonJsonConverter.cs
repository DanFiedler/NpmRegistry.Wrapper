using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace NpmRegistry.Wrapper.Serialization;

public partial class PersonJsonConverter : JsonConverter<Person>
{
    // Matches: Name <email> (url) — email and url are optional
    [GeneratedRegex(@"^(?<name>[^<(]+?)(?:\s*<(?<email>[^>]+)>)?(?:\s*\((?<url>[^)]+)\))?\s*$")]
    private static partial Regex PersonStringRegex();

    public override Person? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var root = document.RootElement;
        if(root.ValueKind == JsonValueKind.String)
        {
            var person = new Person();
            string? value = root.GetString();
            if(!string.IsNullOrEmpty(value))
            {
                var match = PersonStringRegex().Match(value);
                if (match.Success)
                {
                    person.Name = match.Groups["name"].Value.Trim();
                    if (match.Groups["email"].Success)
                    {
                        person.Email = match.Groups["email"].Value;
                    }
                    if (match.Groups["url"].Success)
                    {
                        person.Url = match.Groups["url"].Value;
                    }
                }
                else
                {
                    person.Name = value;
                }
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
            if (root.TryGetProperty("url", out var urlElement) && urlElement.ValueKind == JsonValueKind.String)
            {
                person.Url = urlElement.GetString() ?? string.Empty;
            }
            return person;
        }

        return new Person();
    }

    public override void Write(Utf8JsonWriter writer, Person value, JsonSerializerOptions options)
    {
    }
}
