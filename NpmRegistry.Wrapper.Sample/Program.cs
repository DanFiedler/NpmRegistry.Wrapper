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
        if( args.Length > 1 )
        {
            version = args[1];
        }
        if( args.Length > 2 )
        {
            ns = args[2];
        }

        var host = CreateApplicationHost(args);
        var npmClient = host.Services.GetRequiredService<INpmRegistryClient>();

        var cts = new CancellationTokenSource();
        var packageData = await npmClient.GetPackageData(packageName, version, ns, cts.Token);
        if( packageData == null )
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

        Console.WriteLine("------------------------------------");
        Console.WriteLine("NpmRegistry.Wrapper.Sample complete.");
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
