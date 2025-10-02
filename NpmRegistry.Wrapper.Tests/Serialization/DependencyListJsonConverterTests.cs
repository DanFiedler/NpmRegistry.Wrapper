using NpmRegistry.Wrapper.Serialization;
using System.Text;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Tests.Serialization;
public class DependencyListJsonConverterTests
{
    [Fact]
    public void When_deserialized_then_dependency_count_matches_expected()
    {
        string depName1 = "ora";
        string depVersion1 = "5.3.0";
        string depName2 = "chalk";
        string depVersion2 = "^4.1.0";
        string json = @$"{{
    ""{depName1}"": ""{depVersion1}"",
    ""{depName2}"": ""{depVersion2}""
}}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new DependencyListJsonConverter();

        var dependencies = converter.Read(ref reader, typeof(Models.DependencyList), new JsonSerializerOptions());

        Assert.NotNull(dependencies);
        Assert.Equal(2, dependencies.Dependencies.Count);
        var dependencyOne = dependencies.Dependencies[0];
        var dependencyTwo = dependencies.Dependencies[1];
        Assert.Equal(depName1, dependencyOne.Name);
        Assert.Equal(depVersion1, dependencyOne.Version);
        Assert.Equal(depName2, dependencyTwo.Name);
        Assert.Equal(depVersion2, dependencyTwo.Version);
    }
}
