namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Options;

sealed class ConfigureClientFromNodes
    : ConfigureOptionsFromOptions<ElasticsearchClientOptions, ElasticsearchNodeOptions>
{
    public ConfigureClientFromNodes(IOptionsFactory<ElasticsearchNodeOptions> optionsFactory)
        : base(optionsFactory)
    {
    }

    protected override void Configure(ElasticsearchClientOptions options, ElasticsearchNodeOptions dependency)
    {
        if (dependency.CredentialsHeader != null)
        {
            options.ConfigureSettings += x => x.Authentication(dependency.CredentialsHeader);
        }

        var fallback = options.NodePool;
        options.NodePool = () => dependency.CreatePool() ?? fallback();
    }
}
