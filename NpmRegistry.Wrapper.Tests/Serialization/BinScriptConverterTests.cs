using NpmRegistry.Wrapper.Models;
using System.Text.Json;
using System.Text;
using NpmRegistry.Wrapper.Serialization;

namespace NpmRegistry.Wrapper.Tests.Serialization;

public class BinScriptConverterTests
{
    [Fact]
    public void When_multiple_bin_scripts_specified_then_name_and_path_matches_expected()
    {
        string name1 = "command-one";
        string name2 = "command-two";
        string path1 = "bin/one.sh";
        string path2 = "bin/two.sh";
        string json = @$"
{{
    ""{name1}"":""{path1}"",
    ""{name2}"": ""{path2}""
}}";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(jsonBytes);
        var converter = new BinScriptConverter();

        var binScripts = converter.Read(ref reader, typeof(BinScriptCollection), new JsonSerializerOptions());

        Assert.NotNull(binScripts);
        Assert.Equal(2, binScripts.BinScripts.Count);
        var scriptOne = binScripts.BinScripts[0];
        var scriptTwo = binScripts.BinScripts[1];
        Assert.Equal(name1, scriptOne.Name);
        Assert.Equal(name2, scriptTwo.Name);
        Assert.Equal(path1, scriptOne.Path);
        Assert.Equal(path2, scriptTwo.Path);
    }
}
