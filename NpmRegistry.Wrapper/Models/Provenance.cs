using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;
public class Provenance
{
    [JsonPropertyName("predicateType")]
    public string PredicateType { get; set; } = string.Empty;
}
