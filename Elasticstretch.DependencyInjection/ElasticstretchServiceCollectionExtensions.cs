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
    /// Relevant options/configuration:
    /// <list type="bullet">
    /// <item><see cref="ElasticsearchNodeOptions"/>, bound to <c>Elasticsearch</c></item>
    /// <item>
    /// <see cref="ElasticsearchCredentialOptions"/>, bound to <c>Elasticsearch:Credentials</c>
    /// </item>
    /// <item><see cref="ElasticsearchClientOptions"/></item>
    /// </list>
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <param name="configureSettings">A delegate to configure the client settings.</param>
    /// <param name="configureHttp">
    /// A delegate to configure the underlying HTTP client (not all features supported).
    /// </param>
    /// <returns>The same services, for chaining.</returns>
    public static IServiceCollection AddElasticsearchClient(
        this IServiceCollection services,
        Action<ElasticsearchClientSettings>? configureSettings = null,
        Action<IHttpClientBuilder>? configureHttp = null)
    {
        services.AddOptions();

        TryConfigure<ElasticsearchCredentialOptions, ConfigureCredentialsFromConfig>(services);
        TryConfigure<ElasticsearchNodeOptions, ConfigureNodesFromConfig>(services);
        TryConfigure<ElasticsearchNodeOptions, ConfigureNodesFromCredentials>(services);
        TryConfigure<ElasticsearchClientOptions, ConfigureClientFromNodes>(services);

        services.TryAddSingleton(
            x => new ElasticsearchClient(
                x.GetRequiredService<IOptions<ElasticsearchClientOptions>>().Value.ToSettings()));

        if (configureSettings != null)
        {
            services.Configure<ElasticsearchClientOptions>(x => x.ConfigureSettings += configureSettings);
        }

        configureHttp?.Invoke(services.AddHttpClient(HttpFactoryClient.ClientName));

        return services;
    }

    static void TryConfigure<TOptions, TSetup>(IServiceCollection services)
        where TOptions : class
        where TSetup : class, IConfigureOptions<TOptions>
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<TOptions>, TSetup>());
    }
}
