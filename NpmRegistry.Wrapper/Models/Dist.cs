using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

public class Dist
{
    [JsonPropertyName("integrity")]
    public string Integrity { get; set; } = string.Empty;

    [JsonPropertyName("shasum")]
    public string Shasum { get; set; } = string.Empty;

    [JsonPropertyName("tarball")]
    public string Tarball { get; set; } = string.Empty;

    [JsonPropertyName("fileCount")]
    public int FileCount { get; set; }

    [JsonPropertyName("unpackedSize")]
    public int UnpackedSize { get; set; }

    [JsonPropertyName("signatures")]
    public List<Signature> Signatures { get; set; } = [];

    [JsonPropertyName("npm-signature")]
    public string NpmSignature { get; set; } = string.Empty;
}
