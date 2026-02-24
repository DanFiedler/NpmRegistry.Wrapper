using NpmRegistry.Wrapper.Serialization;
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

    [JsonPropertyName("keywords")]
    public List<string> Keywords { get; set; } = [];

    [JsonPropertyName("main")]
    public string Main { get; set; } = string.Empty;

    [JsonPropertyName("bin")]
    [JsonConverter(typeof(BinScriptConverter))]
    public BinScriptCollection? BinScripts { get; set; }

    [JsonPropertyName("scripts")]
    [JsonConverter(typeof(ScriptListJsonConverter))]
    public ScriptList? Scripts { get; set; }

    [JsonPropertyName("author")]
    [JsonConverter(typeof(PersonJsonConverter))]
    public Person? Author { get; set; }

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

    [JsonPropertyName("maintainers")]
    public List<Person> Maintainers { get; set; } = [];

    [JsonPropertyName("contributors")]
    public List<Person> Contributors { get; set; } = [];

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; } = string.Empty;

    [JsonPropertyName("bugs")]
    public Bugs? Bugs { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;

    [JsonPropertyName("gitHead")]
    public string GitHead { get; set; } = string.Empty;

    [JsonPropertyName("repository")]
    public Repository? Repository { get; set; }

    [JsonPropertyName("_npmOperationalInternal")]
    public NpmOperationalInternal? NpmOperationalInternal { get; set; }

    [JsonPropertyName("_hasShrinkwrap")]
    public bool HasShrinkwrap { get; set; }

    [JsonPropertyName("dependencies")]
    [JsonConverter(typeof(DependencyListJsonConverter))]
    public DependencyList Dependencies { get; set; } = new();
}
