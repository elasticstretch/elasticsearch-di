namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

using System.Net.Http;

sealed class HttpFactoryClient(
    IServiceProvider provider,
    IOptionsMonitor<HttpClientFactoryOptions> options,
    IEnumerable<IHttpMessageHandlerBuilderFilter> filters)
    : HttpTransportClient
{
    public const string ClientName = nameof(ElasticsearchClient);

    protected override HttpMessageHandler CreateHttpClientHandler(RequestData requestData)
    {
        var clientOptions = options.Get(ClientName);

        var configureHandler = (HttpMessageHandlerBuilder builder) =>
        {
            foreach (var item in clientOptions.HttpMessageHandlerBuilderActions)
            {
                item(builder);
            }
        };

        // Not sure why filters are needed in addition to builder actions, but OK...
        foreach (var filter in filters.Reverse())
        {
            configureHandler = filter.Configure(configureHandler);
        }

        var builder = provider.GetRequiredService<HttpMessageHandlerBuilder>(); 
        builder.Name = ClientName;

        configureHandler(builder);
        builder.PrimaryHandler = base.CreateHttpClientHandler(requestData);

        return builder.Build();
    }
}
