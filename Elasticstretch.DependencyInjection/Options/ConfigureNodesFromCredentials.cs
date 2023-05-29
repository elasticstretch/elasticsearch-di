namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Options;

sealed class ConfigureNodesFromCredentials
    : ConfigureOptionsFromOptions<ElasticsearchNodeOptions, ElasticsearchCredentialOptions>
{
    public ConfigureNodesFromCredentials(IOptionsFactory<ElasticsearchCredentialOptions> optionsFactory)
        : base(optionsFactory)
    {
    }

    protected override void Configure(ElasticsearchNodeOptions options, ElasticsearchCredentialOptions dependency)
    {
        var credentials = dependency.CreateHeader();

        if (credentials != null)
        {
            options.CredentialsHeader = credentials;
        }
    }
}
