using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class Signature
{
    [JsonPropertyName("keyid")]
    public string Keyid { get; set; } = string.Empty;

    [JsonPropertyName("sig")]
    public string Sig { get; set; } = string.Empty;
}