namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Transport;

using Microsoft.Extensions.Options;

sealed class ConfigureClientFromNodes(
    DelegatingHttpHandlerFactory httpFactory,
    IOptionsFactory<ElasticsearchNodeOptions> nodeOptions)
        : ConfigureOptionsFromOptions<ElasticsearchClientOptions, ElasticsearchNodeOptions>(nodeOptions)
{
    public const string HttpClientName = nameof(ElasticsearchClient);

    protected override void Configure(ElasticsearchClientOptions options, ElasticsearchNodeOptions dependency)
    {
        if (dependency.CredentialsHeader != null)
        {
            options.ConfigureSettings += x => x.Authentication(dependency.CredentialsHeader);
        }

        var fallback = options.NodePool;
        options.NodePool = () => dependency.CreatePool() ?? fallback();
        options.Connection = () => new HttpRequestInvoker(CreateHttpHandler);
    }

    private HttpMessageHandler CreateHttpHandler(HttpMessageHandler defaultHandler, BoundConfiguration config)
    {
        return httpFactory.CreateDelegatingHandler(HttpClientName, defaultHandler);
    }
}
