# NpmRegistry.Wrapper

## Introduction

A C#/.NET API wrapper for the [NPM Registry](https://registry.npmjs.org/).

The primary goal of this library is to provide strong types and corresponding support for deserialization of NPM registry's particular format of JSON responses.

## Usage 

Add the [NpmRegistry.Wrapper Nuget package](https://www.nuget.org/packages/Fiedler.NpmRegistry.Wrapper) to your project
and use `INpmRegistryClient` to retrieve data about specific packages. 
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
    var packageData = await npmRegistryClient.GetPackageData("my-package", "1.2.3", cts.Token);
    Console.WriteLine($"Package:'{packageData.Name}' by author:'{packageData.Author?.Name}'");
```


## Build and Test

This project targets [.NET 10.0](https://dotnet.microsoft.com/en-us/download) and can be built and tested using standard commands.

- Build: `dotnet build`
- Run Tests: `dotnet test`

## Contribute

Please feel free to send pull requests and raise issues 
(but first do a search of open issues to see if someone has already filed a similar request).

## Security

See [SECURITY](SECURITY.md).

## License

**NpmRegistry.Wrapper** is licensed under the [MIT](LICENSE.TXT) license.
