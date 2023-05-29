using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Options;
using Elastic.Clients.Elasticsearch.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

await using var provider = new ServiceCollection()
    .AddSingleton<IConfiguration>(config)
    .AddElasticsearchClient()
    .Configure<ElasticsearchClientOptions>(
        options =>
        {
            options.ConfigureSettings += settings => settings.ThrowExceptions();
            options.SourceSerializer =
                settings => new DefaultSourceSerializer(settings, x => x.WriteIndented = true);
        })
    .BuildServiceProvider();

var client = provider.GetRequiredService<ElasticsearchClient>();

Console.WriteLine("Node pool is an {0}", client.ElasticsearchClientSettings.NodePool.GetType());

foreach (var node in client.ElasticsearchClientSettings.NodePool.Nodes)
{
    Console.WriteLine("Node configured: {0}", node.Uri);
}

if (client.ElasticsearchClientSettings.Authentication.TryGetAuthorizationParameters(out var credentials))
{
    Console.WriteLine(
        "Credentials: {0} {1}",
        client.ElasticsearchClientSettings.Authentication.AuthScheme,
        credentials);
}
