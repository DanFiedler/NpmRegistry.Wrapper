using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class Bugs
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
