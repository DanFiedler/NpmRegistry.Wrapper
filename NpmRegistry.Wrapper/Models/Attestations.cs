using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;
public class Attestations
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("provenance")]
    public Provenance Provenance { get; set; } = new Provenance();
}
