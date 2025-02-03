namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

sealed class ConfigureClientFromNodes(
    IServiceProvider provider,
    IOptionsFactory<ElasticsearchNodeOptions> nodeOptions,
    IOptionsMonitor<HttpClientFactoryOptions> httpOptions,
    IEnumerable<IHttpMessageHandlerBuilderFilter> httpFilters)
        : ConfigureOptionsFromOptions<ElasticsearchClientOptions, ElasticsearchNodeOptions>(nodeOptions)
{
    protected override void Configure(ElasticsearchClientOptions options, ElasticsearchNodeOptions dependency)
    {
        if (dependency.CredentialsHeader != null)
        {
            options.ConfigureSettings += x => x.Authentication(dependency.CredentialsHeader);
        }

        var fallback = options.NodePool;
        options.NodePool = () => dependency.CreatePool() ?? fallback();
        options.Connection = () => new HttpFactoryClient(provider, httpOptions, httpFilters);
    }
}
