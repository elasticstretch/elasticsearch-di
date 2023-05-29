# Elasticstretch Extensions
Extensions for the [Elasticsearch .NET client](https://github.com/elastic/elasticsearch-net).

### Features
* Inject/resolve Elasticsearch clients using `Microsoft.Extensions.DependencyInjection`.
* Configure Elasticsearch clients using `Microsoft.Extensions.Options`.
* Load config properties using `Microsoft.Extensions.Configuration`.

## Installation

Add the NuGet package to your project:

    $ dotnet add package Elasticstretch.DependencyInjection

## Usage

### Resolving clients

Elasticstretch Dependency Injection works out-of-the-box after registering with an `IServiceCollection`.

```csharp
services.AddElasticsearchClient();
services.AddSingleton<MyService>();
```

Inject the Elasticsearch client via constructor.

```c#
public MyService(ElasticsearchClient client)
{
    // Client is a singleton managed by the container.
    Client = client;
}
```

### Configuring clients

Supported configuration properties are bound to the `Elasticsearch` section of .NET configuration providers, such as `appsettings.json`.

```json
{
  "Elasticsearch": {
    "NodeUris": [
      "https://node1.example.com:9200",
      "https://node2.example.com:9200",
      "https://node3.example.com:9200",
      "https://node4.example.com:9200",
      "https://node5.example.com:9200"
    ],
    "Credentials": {
      "ApiKeyId": "elasticstretch",
      "ApiKey": "abcdef12345"
    },
    "UseSniffing": true,
    "Randomize": true
  }
}
```

You can also leverage `ElasticsearchClientOptions` for additional customization, such as serialization.

```csharp
services.Configure<ElasticsearchClientOptions>(
    options =>
    {
        options.ConfigureSettings += settings => settings.ThrowExceptions();
        options.SourceSerializer =
            settings => new DefaultSourceSerializer(settings, x => x.WriteIndented = true);
    })
```
