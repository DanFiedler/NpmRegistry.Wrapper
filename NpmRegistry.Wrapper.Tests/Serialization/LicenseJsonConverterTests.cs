using NpmRegistry.Wrapper.Models;
using NpmRegistry.Wrapper.Serialization;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Tests.Serialization;

public class LicenseJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new LicenseJsonConverter() }
    };

    [Fact]
    public void Deserialize_WithStringValue_ReturnsString()
    {
        const string json = "\"MIT\"";

        var result = JsonSerializer.Deserialize<string>(json, Options);

        Assert.Equal("MIT", result);
    }

    [Fact]
    public void Deserialize_WithObjectValue_ReturnsType()
    {
        const string json = """
        {
            "type": "ISC",
            "url": "https://opensource.org/licenses/ISC"
        }
        """;

        var result = JsonSerializer.Deserialize<string>(json, Options);

        Assert.Equal("ISC", result);
    }

    [Fact]
    public void Deserialize_WithObjectMissingType_ReturnsEmpty()
    {
        const string json = """
        {
            "url": "https://opensource.org/licenses/ISC"
        }
        """;

        var result = JsonSerializer.Deserialize<string>(json, Options);

        Assert.Equal(string.Empty, result);
    }
}
