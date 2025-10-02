using NpmRegistry.Wrapper.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Serialization;

// The custom JsonConverter is necessary because the NPM registry uses properties for the collection of versions
// (rather than using the more conventional JSON array)
public class VersionListJsonConverter : JsonConverter<VersionList>
{
    public override VersionList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var versionList = new VersionList();

        var root = document.RootElement; // "versions" {
        foreach (var jsonElement in root.EnumerateObject()) // "1.2.3" {
        {
            var version = jsonElement.Value.Deserialize<PackageVersion>();
            if (version != null)
            {
                versionList.PackageVersions.Add(version);
            }
        }

        return versionList;
    }

    public override void Write(Utf8JsonWriter writer, VersionList value, JsonSerializerOptions options)
    {

    }
}
