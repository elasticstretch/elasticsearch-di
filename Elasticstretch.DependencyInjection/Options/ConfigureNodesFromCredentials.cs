namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Options;

sealed class ConfigureNodesFromCredentials(IOptionsFactory<ElasticsearchCredentialOptions> optionsFactory)
    : ConfigureOptionsFromOptions<ElasticsearchNodeOptions, ElasticsearchCredentialOptions>(optionsFactory)
{
    protected override void Configure(ElasticsearchNodeOptions options, ElasticsearchCredentialOptions dependency)
    {
        var credentials = dependency.CreateHeader();

        if (credentials != null)
        {
            options.CredentialsHeader = credentials;
        }
    }
}
