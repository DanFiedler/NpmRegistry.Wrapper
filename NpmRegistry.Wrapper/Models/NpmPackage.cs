using NpmRegistry.Wrapper.Serialization;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class NpmPackage
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("dist-tags")]
    public DistTags? DistTags { get; set; }

    [JsonPropertyName("versions")]
    [JsonConverter(typeof(VersionListJsonConverter))]
    public VersionList? Versions { get; set; }

    [JsonPropertyName("time")]
    public NpmTime? Time { get; set; }

    [JsonPropertyName("maintainers")]
    public List<Person> Maintainers { get; set; } = [];

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    [JsonConverter(typeof(PersonJsonConverter))]
    public Person? Author { get; set; }

    [JsonPropertyName("license")]
    public string License { get; set; } = string.Empty;

    [JsonPropertyName("readme")]
    public string Readme { get; set; } = string.Empty;

    [JsonPropertyName("readmeFilename")]
    public string ReadmeFilename { get; set; } = string.Empty;
}
