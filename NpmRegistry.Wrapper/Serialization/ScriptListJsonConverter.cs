using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper;

public class ScriptListJsonConverter : JsonConverter<ScriptList>
{
    public override ScriptList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var scriptList = new ScriptList();

        var root = document.RootElement; // "scripts" {
        foreach (var jsonElement in root.EnumerateObject()) // "preinstall" {
        {
            string operation = jsonElement.Name;
            string content = jsonElement.Value.ToString();
            var script = new Script
            {
                Operation = operation,
                Content = content
            };
            scriptList.PackageScripts.Add(script);
        }

        return scriptList;
    }

    public override void Write(Utf8JsonWriter writer, ScriptList value, JsonSerializerOptions options)
    {

    }
}
