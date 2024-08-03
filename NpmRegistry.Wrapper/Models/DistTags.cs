using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class DistTags
{
    [JsonPropertyName("latest")]
    public string Latest { get; set; } = string.Empty;
}
