using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class NpmOperationalInternal
{
    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;

    [JsonPropertyName("tmp")]
    public string Tmp { get; set; } = string.Empty;
}
