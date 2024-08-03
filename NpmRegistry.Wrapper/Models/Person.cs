using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class Person
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
