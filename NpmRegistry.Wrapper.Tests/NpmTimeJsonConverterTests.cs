using NpmRegistry.Wrapper.Models;
using NpmRegistry.Wrapper.Serialization;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Tests;

public class NpmTimeJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new NpmTimeJsonConverter() }
    };

    [Fact]
    public void Deserialize_SetsCreatedAndModified()
    {
        const string json = """
        {
            "created": "2012-04-23T16:37:11.912Z",
            "modified": "2026-02-17T22:07:01.094Z",
            "0.1.0": "2012-04-23T16:37:12.603Z",
            "0.2.0": "2012-05-22T04:06:24.044Z",
            "0.2.1": "2012-05-24T21:53:08.449Z"
        }
        """;

        var result = JsonSerializer.Deserialize<NpmTime>(json, Options);

        Assert.NotNull(result);
        Assert.Equal("2012-04-23T16:37:11.912Z", result.Created);
        Assert.Equal("2026-02-17T22:07:01.094Z", result.Modified);
    }

    [Fact]
    public void Deserialize_PopulatesVersionTimes()
    {
        const string json = """
        {
            "created": "2012-04-23T16:37:11.912Z",
            "modified": "2026-02-17T22:07:01.094Z",
            "0.1.0": "2012-04-23T16:37:12.603Z",
            "0.2.0": "2012-05-22T04:06:24.044Z",
            "0.2.1": "2012-05-24T21:53:08.449Z"
        }
        """;

        var result = JsonSerializer.Deserialize<NpmTime>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(3, result.VersionTimes.Count);
        Assert.True(result.VersionTimes.ContainsKey("0.1.0"));
        Assert.True(result.VersionTimes.ContainsKey("0.2.0"));
        Assert.True(result.VersionTimes.ContainsKey("0.2.1"));
        Assert.Equal(new DateTime(2012, 4, 23, 16, 37, 12, 603, DateTimeKind.Utc), result.VersionTimes["0.1.0"].ToUniversalTime());
    }

    [Fact]
    public void Deserialize_DoesNotIncludeKnownPropertiesInVersionTimes()
    {
        const string json = """
        {
            "created": "2012-04-23T16:37:11.912Z",
            "modified": "2026-02-17T22:07:01.094Z",
            "0.1.0": "2012-04-23T16:37:12.603Z"
        }
        """;

        var result = JsonSerializer.Deserialize<NpmTime>(json, Options);

        Assert.NotNull(result);
        Assert.False(result.VersionTimes.ContainsKey("created"));
        Assert.False(result.VersionTimes.ContainsKey("modified"));
    }

    [Fact]
    public void Deserialize_WithUnpublished_SetsUnpublishedProperty()
    {
        const string json = """
        {
            "created": "2012-04-23T16:37:11.912Z",
            "modified": "2026-02-17T22:07:01.094Z",
            "unpublished": {
                "time": "2016-03-23T18:00:00.000Z",
                "versions": ["0.1.0", "0.2.0"]
            }
        }
        """;

        var result = JsonSerializer.Deserialize<NpmTime>(json, Options);

        Assert.NotNull(result);
        Assert.NotNull(result.Unpublished);
        Assert.Equal("2016-03-23T18:00:00.000Z", result.Unpublished.Time);
        Assert.Equal(2, result.Unpublished.Versions.Count);
    }

    [Fact]
    public void Deserialize_EmptyObject_ReturnsDefaults()
    {
        const string json = "{}";

        var result = JsonSerializer.Deserialize<NpmTime>(json, Options);

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Created);
        Assert.Equal(string.Empty, result.Modified);
        Assert.Null(result.Unpublished);
        Assert.Empty(result.VersionTimes);
    }
}