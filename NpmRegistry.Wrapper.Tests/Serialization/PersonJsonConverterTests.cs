using NpmRegistry.Wrapper.Models;
using NpmRegistry.Wrapper.Serialization;
using System.Text;
using System.Text.Json;

namespace NpmRegistry.Wrapper.Tests.Serialization;

public class PersonJsonConverterTests
{
    [Fact]
    public void When_author_is_string_then_author_name_matches_expected()
    {
        string name = "arbitrary value";
        string json = @$"{{""author"":""{name}""}}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        reader.Read();
        reader.Read();
        var converter = new PersonJsonConverter();

        var author = converter.Read(ref reader, typeof(Person), new JsonSerializerOptions());

        Assert.NotNull(author);
        Assert.Equal(name, author.Name);
    }

    [Fact]
    public void When_author_is_json_then_author_name_matches_expected()
    {
        string name = "arbitrary value";
        string json = @$"
{{
    ""name"":""{name}"",
    ""email"": ""arbitrary-email@mail.com""
}}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new PersonJsonConverter();

        var author = converter.Read(ref reader, typeof(Person), new JsonSerializerOptions());

        Assert.NotNull(author);
        Assert.Equal(name, author.Name);
    }

    [Fact]
    public void When_contributors_are_strings_then_names_match_expected()
    {
        string name1 = "Jane Doe";
        string name2 = "John Smith";
        string json = @$"[""{name1}"",""{name2}""]";

        var people = JsonSerializer.Deserialize<List<Person>>(json);

        Assert.NotNull(people);
        Assert.Equal(2, people.Count);
        Assert.Equal(name1, people[0].Name);
        Assert.Equal(name2, people[1].Name);
    }

    [Fact]
    public void When_author_is_string_with_email_then_name_and_email_are_parsed()
    {
        string json = @"{""author"":""Dan Fiedler <d@fiedler.com>""}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        reader.Read();
        reader.Read();
        var converter = new PersonJsonConverter();

        var author = converter.Read(ref reader, typeof(Person), new JsonSerializerOptions());

        Assert.NotNull(author);
        Assert.Equal("Dan Fiedler", author.Name);
        Assert.Equal("d@fiedler.com", author.Email);
    }

    [Fact]
    public void When_author_is_string_with_email_and_url_then_all_fields_are_parsed()
    {
        string json = @"{""author"":""Dan Fiedler <d@fiedler.com> (http://dfiedler.tumblr.com/)""}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        reader.Read();
        reader.Read();
        var converter = new PersonJsonConverter();

        var author = converter.Read(ref reader, typeof(Person), new JsonSerializerOptions());

        Assert.NotNull(author);
        Assert.Equal("Dan Fiedler", author.Name);
        Assert.Equal("d@fiedler.com", author.Email);
        Assert.Equal("http://dfiedler.tumblr.com/", author.Url);
    }

    [Fact]
    public void When_author_is_object_with_url_then_url_is_set()
    {
        string json = @"
{
    ""name"":""Dan Fiedler"",
    ""email"": ""d@fiedler.com"",
    ""url"": ""http://dfiedler.tumblr.com/""
}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new PersonJsonConverter();

        var author = converter.Read(ref reader, typeof(Person), new JsonSerializerOptions());

        Assert.NotNull(author);
        Assert.Equal("Dan Fiedler", author.Name);
        Assert.Equal("d@fiedler.com", author.Email);
        Assert.Equal("http://dfiedler.tumblr.com/", author.Url);
    }
}
