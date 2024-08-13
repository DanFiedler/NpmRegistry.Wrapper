using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class PackageVersion
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("main")]
    public string Main { get; set; } = string.Empty;

    [JsonPropertyName("scripts")]
    [JsonConverter(typeof(ScriptListJsonConverter))]
    public ScriptList? Scripts { get; set; }

    //[JsonPropertyName("author")]
    //public Author? Author { get; set; }

    [JsonPropertyName("license")]
    public string License { get; set; } = string.Empty;

    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("_nodeVersion")]
    public string NodeVersion { get; set; } = string.Empty;

    [JsonPropertyName("_npmVersion")]
    public string NpmVersion { get; set; } = string.Empty;

    [JsonPropertyName("dist")]
    public Dist? Dist { get; set; }

    [JsonPropertyName("_npmUser")]
    public Person? NpmUser { get; set; }

    //[JsonPropertyName("directories")]
    //public object? Directories { get; set; }

    [JsonPropertyName("maintainers")]
    public List<Person> Maintainers { get; set; } = [];

    [JsonPropertyName("_npmOperationalInternal")]
    public NpmOperationalInternal? NpmOperationalInternal { get; set; }

    [JsonPropertyName("_hasShrinkwrap")]
    public bool HasShrinkwrap { get; set; }
}
