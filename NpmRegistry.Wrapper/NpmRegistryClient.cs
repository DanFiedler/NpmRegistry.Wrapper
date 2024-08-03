using Microsoft.Extensions.Logging;
using NpmRegistry.Wrapper.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;

namespace NpmRegistry.Wrapper;

public interface INpmRegistryClient
{
    Task<NpmPackage?> GetPackageData(string name, string? version, CancellationToken cancellationToken); 
}

public class NpmRegistryClient(ILogger<NpmRegistryClient> logger, IHttpClientFactory httpClientFactory) : INpmRegistryClient
{
    private readonly ILogger<NpmRegistryClient> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<NpmPackage?> GetPackageData(string name, string? version, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var url = BuildUrl(name, version);

        try
        {
            return await httpClient.GetFromJsonAsync<NpmPackage>(url, cancellationToken);
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

    private static string BuildUrl(string name, string? version)
    {
        string encodedName = UrlEncoder.Default.Encode(name);

        var sb = new StringBuilder("https://registry.npmjs.org/");
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
