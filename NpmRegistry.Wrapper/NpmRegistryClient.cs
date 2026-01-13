using Microsoft.Extensions.Logging;
using NpmRegistry.Wrapper.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;

namespace NpmRegistry.Wrapper;

public interface INpmRegistryClient
{
    /// <summary>
    /// Retrieve package metadata from https://registry.npmjs.org/ by package name and namespace.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ns"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<NpmPackage?> GetPackageData(string name, string? ns, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieve package metadata from https://registry.npmjs.org/ by package name, namespace, and version.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ns"></param>
    /// <param name="version"></param>
    /// <param name="cancellationToken"></param>
    Task<PackageVersion?> GetVersionData(string name, string? ns, string version, CancellationToken cancellationToken);
}

public class NpmRegistryClient(ILogger<NpmRegistryClient> logger, IHttpClientFactory httpClientFactory) : INpmRegistryClient
{
    private readonly ILogger<NpmRegistryClient> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<NpmPackage?> GetPackageData(string name, string? ns, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = BuildUrl(name, null, ns);

        try
        {
            return await httpClient.GetFromJsonAsync(url, ModelsSerializerContext.Default.NpmPackage, cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning("NPM Registry returned 404. Package not found at: {url}", url);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error requesting package data from NPM registry: {ex}", ex);
        }

        return null;
    }

    public async Task<PackageVersion?> GetVersionData(string name, string? ns, string version, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = BuildUrl(name, version, ns);

        try
        {
            return await httpClient.GetFromJsonAsync(url, ModelsSerializerContext.Default.PackageVersion, cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning("NPM Registry returned 404. Package not found at: {url}", url);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error requesting package data from NPM registry: {ex}", ex);
        }

        return null;
    }

    private static string BuildUrl(string name, string? version, string? ns)
    {
        string encodedName = UrlEncoder.Default.Encode(name);

        var sb = new StringBuilder("https://registry.npmjs.org/");

        if(!string.IsNullOrEmpty(ns))
        {
            string encodedNs = UrlEncoder.Default.Encode($"@{ns}");
            sb.Append(encodedNs);
            sb.Append('/');
        }

        sb.Append(encodedName);

        if (!string.IsNullOrEmpty(version))
        {
            string encodedVersion = UrlEncoder.Default.Encode(version);
            sb.Append('/');
            sb.Append(encodedVersion);
        }

        return sb.ToString();
    }
}
