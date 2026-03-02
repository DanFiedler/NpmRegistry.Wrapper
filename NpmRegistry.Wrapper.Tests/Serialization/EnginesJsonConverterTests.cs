using NpmRegistry.Wrapper.Serialization;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Tests.Serialization;

public class EnginesJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new EnginesJsonConverter() }
    };

    [Fact]
    public void Deserialize_WithDictionary_ReturnsKeyValuePairs()
    {
        const string json = """
        {
            "node": ">=14.0.0",
            "npm": ">=6.0.0"
        }
        """;

        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(">=14.0.0", result["node"]);
        Assert.Equal(">=6.0.0", result["npm"]);
    }

    [Fact]
    public void Deserialize_WithList_ReturnsKeysWithEmptyValues()
    {
        const string json = """
        [
            "node",
            "rhino"
        ]
        """;

        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(string.Empty, result["node"]);
        Assert.Equal(string.Empty, result["rhino"]);
    }

    [Fact]
    public void Deserialize_WithEmptyDictionary_ReturnsEmpty()
    {
        const string json = "{}";

        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Options);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Deserialize_WithEmptyList_ReturnsEmpty()
    {
        const string json = "[]";

        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Options);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Deserialize_WithSingleEntryDictionary_ReturnsSinglePair()
    {
        const string json = """
        {
            "node": "*"
        }
        """;

        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Options);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("*", result["node"]);
    }

    [Fact]
    public void Deserialize_WithSingleEntryList_ReturnsSingleKey()
    {
        const string json = """["node"]""";

        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Options);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(string.Empty, result["node"]);
    }

    [Fact]
    public void Serialize_RoundTrips()
    {
        var engines = new Dictionary<string, string>
        {
            ["node"] = ">=14.0.0",
            ["npm"] = ">=6.0.0"
        };

        // Serialize with default options (produces standard JSON object)
        var json = JsonSerializer.Serialize(engines);
        // Deserialize with custom converter
        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(">=14.0.0", result["node"]);
        Assert.Equal(">=6.0.0", result["npm"]);
    }
}
