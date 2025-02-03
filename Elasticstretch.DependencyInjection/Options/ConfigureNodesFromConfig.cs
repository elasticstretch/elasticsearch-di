namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

sealed class ConfigureNodesFromConfig(IConfiguration config)
    : ConfigureFromConfigurationOptions<ElasticsearchNodeOptions>(config.GetSection(Path))
{
    public const string Path = "Elasticsearch";
}
