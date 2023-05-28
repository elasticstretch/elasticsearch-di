namespace Elastic.Clients.Elasticsearch;

using Elastic.Clients.Elasticsearch.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

/// <summary>
/// Extensions of <see cref="IServiceCollection"/> for the Elasticsearch client.
/// </summary>
public static class ElasticstretchServiceCollectionExtensions
{
    /// <summary>
    /// Adds a singleton <see cref="ElasticsearchClient"/> to the services.
    /// </summary>
    /// <remarks>
    /// <see cref="ElasticsearchNodeOptions"/> is also bound to the <c>Elasticsearch</c> configuration section.
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <param name="configureNodes">A delegate to configure Elasticsearch nodes.</param>
    /// <param name="configureSettings">A delegate to configure additional client settings.</param>
    /// <returns>The same services, for chaining.</returns>
    public static IServiceCollection AddElasticsearchClient(
        this IServiceCollection services,
        Action<ElasticsearchNodeOptions>? configureNodes = null,
        Action<ElasticsearchClientSettings>? configureSettings = null)
    {
        services.AddOptions();

        TryConfigure<ElasticsearchNodeOptions, ConfigureNodesFromConfig>(services);
        TryConfigure<ElasticsearchClientOptions, ConfigureClientFromNodes>(services);

        services.TryAddSingleton(
            x => new ElasticsearchClient(
                x.GetRequiredService<IOptions<ElasticsearchClientOptions>>().Value.ToSettings()));

        if (configureNodes != null)
        {
            services.Configure(configureNodes);
        }

        if (configureSettings != null)
        {
            services.Configure<ElasticsearchClientOptions>(x => x.ConfigureSettings += configureSettings);
        }

        return services;
    }

    static void TryConfigure<TOptions, TSetup>(IServiceCollection services)
        where TOptions : class
        where TSetup : class, IConfigureOptions<TOptions>
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<TOptions>, TSetup>());
    }
}
