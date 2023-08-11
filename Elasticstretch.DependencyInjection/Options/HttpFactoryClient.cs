namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using System.Net.Http;

sealed class HttpFactoryClient : HttpTransportClient
{
    public const string ClientName = nameof(ElasticsearchClient);

    readonly IServiceProvider _provider;
    readonly IOptionsMonitor<HttpClientFactoryOptions> _options;
    readonly IEnumerable<IHttpMessageHandlerBuilderFilter> _filters;

    public HttpFactoryClient(
        IServiceProvider provider,
        IOptionsMonitor<HttpClientFactoryOptions> options,
        IEnumerable<IHttpMessageHandlerBuilderFilter> filters)
    {
        _provider = provider;
        _options = options;
        _filters = filters;
    }

    protected override HttpMessageHandler CreateHttpClientHandler(RequestData requestData)
    {
        var clientOptions = _options.Get(ClientName);

        var configureHandler = (HttpMessageHandlerBuilder builder) =>
        {
            foreach (var item in clientOptions.HttpMessageHandlerBuilderActions)
            {
                item(builder);
            }
        };

        // Not sure why filters are needed in addition to builder actions, but OK...
        foreach (var filter in _filters.Reverse())
        {
            configureHandler = filter.Configure(configureHandler);
        }

        var builder = _provider.GetRequiredService<HttpMessageHandlerBuilder>(); 
        builder.Name = ClientName;

        configureHandler(builder);
        builder.PrimaryHandler = base.CreateHttpClientHandler(requestData);

        return builder.Build();
    }
}
