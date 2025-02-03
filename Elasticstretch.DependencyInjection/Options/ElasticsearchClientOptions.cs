namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Clients.Elasticsearch.Serialization;
using Elastic.Transport;

/// <summary>
/// A model to configure Elasticsearch clients using the .NET options pattern.
/// </summary>
public class ElasticsearchClientOptions
{
    /// <summary>
    /// Gets or sets the factory for the client node pool.
    /// </summary>
    /// <remarks>
    /// Default uses <c>http://localhost:9200</c>.
    /// </remarks>
    public Func<NodePool> NodePool { get; set; } = () => new SingleNodePool(new("http://localhost:9200"));

    /// <summary>
    /// Gets or sets the factory for the underlying transport connection.
    /// </summary>
    public Func<IRequestInvoker> Connection { get; set; } = () => new HttpRequestInvoker();

    /// <summary>
    /// Gets or sets the factory for the client source serializer.
    /// </summary>
    public Func<IElasticsearchClientSettings, Serializer> SourceSerializer { get; set; }
        = settings => new DefaultSourceSerializer(settings, x => { });

    /// <summary>
    /// Gets or sets the mapper for property serialization.
    /// </summary>
    public IPropertyMappingProvider PropertyMappingProvider { get; set; } = new DefaultPropertyMappingProvider();

    /// <summary>
    /// Gets or sets the delegate to configure additional client settings.
    /// </summary>
    public Action<ElasticsearchClientSettings>? ConfigureSettings { get; set; }

    /// <summary>
    /// Converts the options to Elastic client settings.
    /// </summary>
    /// <returns>The client settings.</returns>
    public IElasticsearchClientSettings ToSettings()
    {
        var settings = new ElasticsearchClientSettings(
            NodePool(),
            Connection(),
            (x, y) => SourceSerializer(y),
            PropertyMappingProvider);

        ConfigureSettings?.Invoke(settings);
        return settings;
    }
}
