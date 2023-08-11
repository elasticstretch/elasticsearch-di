using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

await using var provider = new ServiceCollection()
    .AddSingleton<IConfiguration>(new ConfigurationBuilder().Build())
    .AddLogging(x => x.AddConsole())
    .AddElasticsearchClient(x => x.ThrowExceptions())
    .BuildServiceProvider();

var client = provider.GetRequiredService<ElasticsearchClient>();
await client.PingAsync();
