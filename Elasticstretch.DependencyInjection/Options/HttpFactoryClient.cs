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

    public HttpFactoryClient(IServiceProvider provider, IOptionsMonitor<HttpClientFactoryOptions> options)
    {
        _provider = provider;
        _options = options;
    }

    protected override HttpMessageHandler CreateHttpClientHandler(RequestData requestData)
    {
        var builder = _provider.GetRequiredService<HttpMessageHandlerBuilder>();
        builder.PrimaryHandler = base.CreateHttpClientHandler(requestData);

        foreach (var item in _options.Get(ClientName).HttpMessageHandlerBuilderActions)
        {
            item(builder);
        }

        return builder.Build();
    }
}
