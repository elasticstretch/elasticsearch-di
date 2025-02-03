namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

sealed class ConfigureCredentialsFromConfig(IConfiguration config)
    : ConfigureFromConfigurationOptions<ElasticsearchCredentialOptions>(config.GetSection(Path))
{
    public static readonly string Path = ConfigurationPath.Combine("Elasticsearch", "Credentials");
}
