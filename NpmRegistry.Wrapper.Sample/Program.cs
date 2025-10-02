using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NpmRegistry.Wrapper.Sample;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("NpmRegistry.Wrapper.Sample running.");
        Console.WriteLine("-----------------------------------");

        // consider using CommandLineParser instead of manually parsing args
        string packageName = args[0];
        string? version = null;
        string? ns = null;
        if (args.Length > 1)
        {
            version = args[1];
        }
        if (args.Length > 2)
        {
            ns = args[2];
        }

        var host = CreateApplicationHost(args);
        var npmClient = host.Services.GetRequiredService<INpmRegistryClient>();

        var cts = new CancellationTokenSource();

        if (string.IsNullOrEmpty(version))
        {
            await WritePackageData(packageName, ns, npmClient, cts);
        }
        else
        {
            await WritePackageVersionData(packageName, ns, version, npmClient, cts);
        }

        Console.WriteLine("------------------------------------");
        Console.WriteLine("NpmRegistry.Wrapper.Sample complete.");
    }

    private static async Task WritePackageData(string packageName, string? ns, INpmRegistryClient npmClient, CancellationTokenSource cts)
    {
        var packageData = await npmClient.GetPackageData(packageName, ns, cts.Token);
        if (packageData == null)
        {
            Console.WriteLine($"Failed to retrieve package data for: {packageName}");
        }
        else
        {
            Console.WriteLine($"Package:'{packageData.Name}' by author:'{packageData.Author?.Name}'");
            if (packageData.Versions != null)
            {
                Console.WriteLine("Versions:");
                packageData.Versions.PackageVersions = [.. packageData.Versions.PackageVersions.OrderBy(v => v.Version)];
                foreach (var packageVersion in packageData.Versions.PackageVersions)
                {
                    Console.WriteLine($"  - {packageVersion.Version} with sha:{packageVersion.Dist?.Shasum}");
                }
            }
        }
    }

    private static async Task WritePackageVersionData(string packageName, string? ns, string version, INpmRegistryClient npmClient, CancellationTokenSource cts)
    {
        var packageVersion = await npmClient.GetVersionData(packageName, ns, version, cts.Token);
        if (packageVersion == null)
        {
            Console.WriteLine($"Failed to retrieve package version data for: {packageName}");
        }
        else
        {
            Console.WriteLine($"Package:'{packageVersion.Name}' version {packageVersion.NpmVersion} by author:'{packageVersion.Author?.Name}'");
            Console.WriteLine($"Attestations URL: {packageVersion.Dist?.Attestations?.Url}");
            Console.WriteLine("Dependencies:");
            foreach (var dep in packageVersion.Dependencies.Dependencies)
            {
                Console.WriteLine($"  - {dep.Name}:{dep.Version}");
            }
        }
    }

    private static IHost CreateApplicationHost(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureServices(builder);
        return builder.Build();
    }

    private static void ConfigureServices(HostApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<INpmRegistryClient, NpmRegistryClient>();
    }
}
