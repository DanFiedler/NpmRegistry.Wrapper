using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class NpmTime
{
    [JsonPropertyName("created")]
    public string Created { get; set; } = string.Empty;

    [JsonPropertyName("modified")]
    public string Modified { get; set; } = string.Empty;

    [JsonPropertyName("unpublished")]
    public UnpublishedTime? Unpublished { get; set; }

    public Dictionary<string, DateTime> VersionTimes { get; set; } = [];
}

public class UnpublishedTime
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;

    [JsonPropertyName("versions")]
    public List<string> Versions { get; set; } = [];
}