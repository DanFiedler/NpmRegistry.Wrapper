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

        var versionData = await npmClient.GetVersionData("left_pad", null, "0.0.11", CancellationToken.None);

        Assert.NotNull(versionData);
        Assert.Single(versionData.Maintainers);
    }

    [Fact]
    public async Task When_requesting_coffee_script_then_scripts_count_is_two()
    {
        string json = GetJson("coffee-script.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("coffee-script", null, CancellationToken.None);

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

    [Fact]
    public async Task When_requesting_custom_widget_then_bin_script_matches_expected()
    {
        string json = GetJson("custom-widget.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("custom-widget", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var version = packageData.Versions.PackageVersions[^1];
        Assert.NotNull(version.Scripts);
        Assert.Single(version.Scripts.PackageScripts);
        Assert.Single(version.Maintainers);
        var binScripts = version.BinScripts;
        Assert.NotNull(binScripts);
    }

    [Fact]
    public async Task When_requesting_edge_case_pkg_then_string_contributors_bin_and_repository_deserialize()
    {
        string json = GetJson("edge-case-pkg.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Repository);
        Assert.Equal("github:testuser/edge-case-pkg", packageData.Repository.Url);

        Assert.NotNull(packageData.Versions);
        var version = packageData.Versions.PackageVersions[0];

        Assert.Equal(3, version.Contributors.Count);
        Assert.Equal("Jane Doe", version.Contributors[0].Name);
        Assert.Equal("John Smith", version.Contributors[1].Name);
        Assert.Equal("Object Contributor", version.Contributors[2].Name);
        Assert.Equal("obj@example.com", version.Contributors[2].Email);

        Assert.NotNull(version.BinScripts);
        Assert.Single(version.BinScripts.BinScripts);
        Assert.Equal("./bin/edge-case-pkg.js", version.BinScripts.BinScripts[0].Path);

        Assert.NotNull(version.Repository);
        Assert.Equal("github:testuser/edge-case-pkg", version.Repository.Url);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_person_string_format_is_parsed()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Author);
        Assert.Equal("Barney Rubble", packageData.Author.Name);
        Assert.Equal("b@rubble.com", packageData.Author.Email);
        Assert.Equal("http://barnyrubble.tumblr.com/", packageData.Author.Url);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_license_object_is_deserialized()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var v1 = packageData.Versions.PackageVersions[0];
        var v2 = packageData.Versions.PackageVersions[1];

        Assert.Equal("ISC", v1.License);
        Assert.Equal("MIT", v2.License);
        Assert.Equal("MIT", packageData.License);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_deprecated_field_is_set()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var v1 = packageData.Versions.PackageVersions[0];
        Assert.Equal("This version has a critical bug. Please upgrade to 2.0.0.", v1.Deprecated);

        var v2 = packageData.Versions.PackageVersions[1];
        Assert.Equal(string.Empty, v2.Deprecated);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_engines_are_deserialized()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var v1 = packageData.Versions.PackageVersions[0];
        Assert.Equal(2, v1.Engines.Count);
        Assert.Equal(">=14.0.0", v1.Engines["node"]);
        Assert.Equal(">=6.0.0", v1.Engines["npm"]);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_engines_array_is_deserialized()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var v2 = packageData.Versions.PackageVersions[1];
        Assert.Equal(2, v2.Engines.Count);
        Assert.Equal(string.Empty, v2.Engines["node"]);
        Assert.Equal(string.Empty, v2.Engines["rhino"]);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_additional_dependency_types_are_deserialized()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var v1 = packageData.Versions.PackageVersions[0];

        Assert.Single(v1.Dependencies.Dependencies);
        Assert.Equal("lodash", v1.Dependencies.Dependencies[0].Name);

        Assert.Equal(2, v1.DevDependencies.Dependencies.Count);
        Assert.Equal("jest", v1.DevDependencies.Dependencies[0].Name);
        Assert.Equal("eslint", v1.DevDependencies.Dependencies[1].Name);

        Assert.Single(v1.PeerDependencies.Dependencies);
        Assert.Equal("react", v1.PeerDependencies.Dependencies[0].Name);

        Assert.Single(v1.OptionalDependencies.Dependencies);
        Assert.Equal("fsevents", v1.OptionalDependencies.Dependencies[0].Name);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_large_unpacked_size_is_deserialized()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var v1 = packageData.Versions.PackageVersions[0];
        Assert.NotNull(v1.Dist);
        Assert.Equal(3_000_000_000L, v1.Dist.UnpackedSize);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_bugs_email_is_deserialized()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Bugs);
        Assert.Equal("https://github.com/testuser/edge-case-pkg-v2/issues", packageData.Bugs.Url);
        Assert.Equal("bugs@edge-case-pkg-v2.com", packageData.Bugs.Email);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_repository_directory_is_deserialized()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Repository);
        Assert.Equal("git", packageData.Repository.Type);
        Assert.Equal("git+https://github.com/testuser/edge-case-pkg-v2.git", packageData.Repository.Url);
        Assert.Equal("packages/core", packageData.Repository.Directory);
    }

    [Fact]
    public async Task When_requesting_edge_case_v2_then_contributor_string_emails_and_urls_are_parsed()
    {
        string json = GetJson("edge-case-pkg-v2.json");
        var httpClientFactory = SetupHttpClientFactory(json);
        var npmClient = new NpmRegistryClient(Substitute.For<ILogger<NpmRegistryClient>>(), httpClientFactory);

        var packageData = await npmClient.GetPackageData("edge-case-pkg-v2", null, CancellationToken.None);

        Assert.NotNull(packageData);
        Assert.NotNull(packageData.Versions);
        var v1 = packageData.Versions.PackageVersions[0];

        Assert.Equal(3, v1.Contributors.Count);

        Assert.Equal("Jane Doe", v1.Contributors[0].Name);
        Assert.Equal("jane@example.com", v1.Contributors[0].Email);

        Assert.Equal("John Smith", v1.Contributors[1].Name);
        Assert.Equal("john@example.com", v1.Contributors[1].Email);
        Assert.Equal("https://johnsmith.dev", v1.Contributors[1].Url);

        Assert.Equal("Object Contributor", v1.Contributors[2].Name);
        Assert.Equal("obj@example.com", v1.Contributors[2].Email);
        Assert.Equal("https://objcontributor.dev", v1.Contributors[2].Url);
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