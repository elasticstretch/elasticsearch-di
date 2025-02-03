namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

using System.Net.Http;

// Use as much as we can from HTTP client factory configuration, while injecting a primary handler on demand.
// Compare with https://github.com/dotnet/runtime/blob/05d94d94028b5622b19734e0e1b60d7aca4667b3/src/libraries/Microsoft.Extensions.Http/src/DefaultHttpMessageHandlerBuilder.cs#L47
sealed class DelegatingHttpHandlerFactory(
    IServiceProvider services,
    IOptionsMonitor<HttpClientFactoryOptions> options,
    IEnumerable<IHttpMessageHandlerBuilderFilter> filters)
{
    public HttpMessageHandler CreateDelegatingHandler(string clientName, HttpMessageHandler primaryHandler)
    {
        var clientOptions = options.Get(clientName);

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

        var builder = services.GetRequiredService<HttpMessageHandlerBuilder>(); 
        builder.Name = clientName;

        configureHandler(builder);
        builder.PrimaryHandler = primaryHandler;

        return builder.Build();
    }
}
