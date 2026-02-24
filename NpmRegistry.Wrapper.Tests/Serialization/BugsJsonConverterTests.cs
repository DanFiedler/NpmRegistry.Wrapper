using NpmRegistry.Wrapper.Models;
using NpmRegistry.Wrapper.Serialization;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Tests.Serialization;

public class BugsJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new BugsJsonConverter() }
    };

    [Fact]
    public void Deserialize_WithUrlProperty_SetsUrl()
    {
        const string json = """
        {
            "url": "https://github.com/example/repo/issues"
        }
        """;

        var result = JsonSerializer.Deserialize<Bugs>(json, Options);

        Assert.NotNull(result);
        Assert.Equal("https://github.com/example/repo/issues", result.Url);
    }

    [Fact]
    public void Deserialize_WithNameProperty_SetsUrl()
    {
        const string json = """
        {
            "name": "https://github.com/jashkenas/coffee-script/issues"
        }
        """;

        var result = JsonSerializer.Deserialize<Bugs>(json, Options);

        Assert.NotNull(result);
        Assert.Equal("https://github.com/jashkenas/coffee-script/issues", result.Url);
    }

    [Fact]
    public void Deserialize_WithBothUrlAndName_PrefersUrl()
    {
        const string json = """
        {
            "url": "https://github.com/example/repo/issues",
            "name": "https://github.com/other/repo/issues"
        }
        """;

        var result = JsonSerializer.Deserialize<Bugs>(json, Options);

        Assert.NotNull(result);
        Assert.Equal("https://github.com/example/repo/issues", result.Url);
    }

    [Fact]
    public void Deserialize_WithStringValue_SetsUrl()
    {
        const string json = "\"https://github.com/jashkenas/coffee-script/issues\"";

        var result = JsonSerializer.Deserialize<Bugs>(json, Options);

        Assert.NotNull(result);
        Assert.Equal("https://github.com/jashkenas/coffee-script/issues", result.Url);
    }

    [Fact]
    public void Deserialize_WithNeitherUrlNorName_ReturnsEmptyUrl()
    {
        const string json = """
        {
            "other": "value"
        }
        """;

        var result = JsonSerializer.Deserialize<Bugs>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Url);
    }
}
