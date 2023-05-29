namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

sealed class ConfigureCredentialsFromConfig : ConfigureFromConfigurationOptions<ElasticsearchCredentialOptions>
{
    public ConfigureCredentialsFromConfig(IConfiguration config)
        : base(config.GetSection(ConfigurationPath.Combine("Elasticsearch", "Credentials")))
    {
    }
}
