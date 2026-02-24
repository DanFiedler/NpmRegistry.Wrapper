# NpmRegistry.Wrapper

## Introduction

A C#/.NET API wrapper for the [npm registry](https://registry.npmjs.org/).

The primary goal of this library is to provide strong types and corresponding support for deserialization of the npm registry's particular format of JSON responses.

## Usage 

Use `INpmRegistryClient` to retrieve data about specific packages. 
This package assumes usage of [.NET dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
and requires registration of [IHttpClientFactory](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory).

Basic usage is demonstrated in the `samples\NpmRegistry.Wrapper.Sample` project and in the code block below:

```csharp
    // Sample DI setup 
    var builder = Host.CreateApplicationBuilder(args);    
    builder.Services.AddHttpClient();
    builder.Services.AddSingleton<INpmRegistryClient, NpmRegistryClient>();
    var host = builder.Build();

    // Sample usage to get information of a specific package version
    var npmRegistryClient = host.Services.GetRequiredService<INpmRegistryClient>();
    var cts = new CancellationTokenSource();
    var packageData = await npmRegistryClient.GetPackageData("my-package", null, cts.Token);
    Console.WriteLine($"Package:'{packageData.Name}' by author:'{packageData.Author?.Name}'");
```

# Documentation

For more information, please refer to the package's [GitHub repository](https://github.com/DanFiedler/NpmRegistry.Wrapper).
