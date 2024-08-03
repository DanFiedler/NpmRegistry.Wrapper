using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class Scripts
{
    [JsonPropertyName("test")]
    public string Test { get; set; } = string.Empty;
}
