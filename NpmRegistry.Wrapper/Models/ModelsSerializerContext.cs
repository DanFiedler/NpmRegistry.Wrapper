using System.Text.Json;
using System.Text.Json.Serialization;

namespace NpmRegistry.Wrapper.Models;

[JsonSerializable(typeof(NpmPackage))]
[JsonSourceGenerationOptions(AllowTrailingCommas = true,
                             GenerationMode = JsonSourceGenerationMode.Default,
                             IgnoreReadOnlyFields = false,
                             IgnoreReadOnlyProperties = false,
                             IncludeFields = false,
                             MaxDepth = 15,
                             NumberHandling = JsonNumberHandling.Strict,
                             PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace,
                             ReadCommentHandling = JsonCommentHandling.Skip,
                             UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
                             UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
                             UseStringEnumConverter = true,
                             WriteIndented = false)]
internal sealed partial class ModelsSerializerContext : JsonSerializerContext
{
}
