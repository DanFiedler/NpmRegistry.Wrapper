using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Serialization;
public class DependencyListJsonConverter : JsonConverter<DependencyList>
{
    public override DependencyList? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dependencies = new DependencyList();

        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement; // "dependencies" {
        foreach (var jsonElement in root.EnumerateObject()) // "left-pad" {
        {
            string name = jsonElement.Name;
            string version = jsonElement.Value.ToString();
            var dependency = new Dependency
            {
                Name = name,
                Version = version
            };
            dependencies.Dependencies.Add(dependency);
        }

        return dependencies;
    }

    public override void Write(Utf8JsonWriter writer, DependencyList value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
