namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

sealed class ConfigureClientFromNodes
    : ConfigureOptionsFromOptions<ElasticsearchClientOptions, ElasticsearchNodeOptions>
{
    readonly IServiceProvider _provider;
    readonly IOptionsMonitor<HttpClientFactoryOptions> _httpOptions;

    public ConfigureClientFromNodes(
        IServiceProvider provider,
        IOptionsFactory<ElasticsearchNodeOptions> nodeOptions,
        IOptionsMonitor<HttpClientFactoryOptions> httpOptions)
        : base(nodeOptions)
    {
        _provider = provider;
        _httpOptions = httpOptions;
    }

    protected override void Configure(ElasticsearchClientOptions options, ElasticsearchNodeOptions dependency)
    {
        if (dependency.CredentialsHeader != null)
        {
            options.ConfigureSettings += x => x.Authentication(dependency.CredentialsHeader);
        }

        var fallback = options.NodePool;
        options.NodePool = () => dependency.CreatePool() ?? fallback();
        options.Connection = () => new HttpFactoryClient(_provider, _httpOptions);
    }
}
