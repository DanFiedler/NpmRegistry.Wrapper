using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class Repository
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
