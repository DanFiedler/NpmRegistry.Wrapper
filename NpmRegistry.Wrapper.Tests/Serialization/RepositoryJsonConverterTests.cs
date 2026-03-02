using NpmRegistry.Wrapper.Models;
using NpmRegistry.Wrapper.Serialization;
using System.Text;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Tests.Serialization;

public class RepositoryJsonConverterTests
{
    [Fact]
    public void When_repository_is_string_then_url_matches_expected()
    {
        string url = "github:user/repo";
        string json = @$"""{url}""";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new RepositoryJsonConverter();

        var repository = converter.Read(ref reader, typeof(Repository), new JsonSerializerOptions());

        Assert.NotNull(repository);
        Assert.Equal(url, repository.Url);
    }

    [Fact]
    public void When_repository_is_object_then_type_and_url_match_expected()
    {
        string type = "git";
        string url = "git+https://github.com/user/repo.git";
        string json = @$"
{{
    ""type"":""{type}"",
    ""url"": ""{url}""
}}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new RepositoryJsonConverter();

        var repository = converter.Read(ref reader, typeof(Repository), new JsonSerializerOptions());

        Assert.NotNull(repository);
        Assert.Equal(type, repository.Type);
        Assert.Equal(url, repository.Url);
    }

    [Fact]
    public void When_repository_is_object_with_directory_then_directory_is_set()
    {
        string json = """
        {
            "type": "git",
            "url": "git+https://github.com/user/monorepo.git",
            "directory": "packages/core"
        }
        """;
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new RepositoryJsonConverter();

        var repository = converter.Read(ref reader, typeof(Repository), new JsonSerializerOptions());

        Assert.NotNull(repository);
        Assert.Equal("git", repository.Type);
        Assert.Equal("git+https://github.com/user/monorepo.git", repository.Url);
        Assert.Equal("packages/core", repository.Directory);
    }
}
