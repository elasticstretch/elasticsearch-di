namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

sealed class ConfigureNodesFromConfig : ConfigureFromConfigurationOptions<ElasticsearchNodeOptions>
{
    public ConfigureNodesFromConfig(IConfiguration config)
        : base(config.GetSection("Elasticsearch"))
    {
    }
}
