using Microsoft.Extensions.Logging;
using NSubstitute;

namespace NpmRegistry.Wrapper.Tests;

public class NpmRegistryClientTests
{
    [Fact]
    public async Task When_requesting_left_pad_with_no_version_then_versions_list_count_is_nine()
    {
        // This also tests VersionListJsonConverter's ability to deserialize NPM Registry's odd version format
        string json = GetJson("left_pad.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("name", null, CancellationToken.None);
        
        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        Assert.Equal(9, packageData.Versions.PackageVersions.Count);
    }

    [Fact]
    public async Task When_requesting_left_pad_v11_then_versions_maintainer_count_is_one()
    {
        string json = GetJson("left_pad_v11.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("left_pad", "0.0.11", CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.Single(packageData.Maintainers);
    }

    [Fact]
    public async Task When_requesting_coffee_script_v1_12_7_then_scripts_count_is_two()
    {
        string json = GetJson("coffee_script_1_12_7.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("left_pad", "0.0.11", CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        Assert.True(packageData.Versions.PackageVersions.Count > 0);
        int i = packageData.Versions.PackageVersions.Count - 1;
        var version = packageData.Versions.PackageVersions[i];
        Assert.NotNull(version.Scripts);
        Assert.Equal(2, version.Scripts.PackageScripts.Count);
        var script = version.Scripts.PackageScripts[0];
        Assert.False(string.IsNullOrEmpty(script.Operation));
        Assert.False(string.IsNullOrEmpty(script.Content));
    }

    private static IHttpClientFactory SetupHttpClientFactory(string json)
    {
        var httpMessageHandler = new FakeHttpMessageHandler(json);
        var httpClient = new HttpClient(httpMessageHandler);
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);
        return httpClientFactory;
    }


    private static string GetJson(string resourceName)
    {
        string resourcePath = $"NpmRegistry.Wrapper.Tests.Resources.{resourceName}";
        var assembly = typeof(NpmRegistryClientTests).Assembly;
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        Assert.NotNull(stream);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}