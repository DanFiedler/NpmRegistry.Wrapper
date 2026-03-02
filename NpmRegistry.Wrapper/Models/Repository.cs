using NpmRegistry.Wrapper.Serialization;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

[JsonConverter(typeof(RepositoryJsonConverter))]
public class Repository
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("directory")]
    public string Directory { get; set; } = string.Empty;
}
