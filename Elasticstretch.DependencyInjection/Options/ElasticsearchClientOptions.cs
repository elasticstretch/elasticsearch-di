namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Transport;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A model to configure Elasticsearch clients using the .NET options pattern.
/// </summary>
public class ElasticsearchClientOptions
{
    /// <summary>
    /// Gets or sets the client node pool, or <see langword="null"/> to connect locally.
    /// </summary>
    public NodePool? NodePool { get; set; }

    /// <summary>
    /// Gets or sets the underlying transport connection, or <see langword="null"/> to use HTTP.
    /// </summary>
    public TransportClient? Connection { get; set; }

    /// <summary>
    /// Gets or sets a custom factory for the client source serializer, if any.
    /// </summary>
    public ElasticsearchClientSettings.SourceSerializerFactory? SourceSerializer { get; set; }

    /// <summary>
    /// Gets or sets a custom mapper for property serialization, if any.
    /// </summary>
    public IPropertyMappingProvider? PropertyMappingProvider { get; set; }

    /// <summary>
    /// Gets or sets the delegate to configure additional client settings.
    /// </summary>
    public Action<ElasticsearchClientSettings>? ConfigureSettings { get; set; }

    /// <summary>
    /// Converts the options to Elastic client settings.
    /// </summary>
    /// <returns>The client settings.</returns>
    [SuppressMessage(
        "Reliability",
        "CA2000:Dispose objects before losing scope",
        Justification = "Dispose ownership transferred to settings.")]
    public IElasticsearchClientSettings ToSettings()
    {
        var settings = new ElasticsearchClientSettings(
            NodePool ?? new SingleNodePool(new("localhost:9200")),
            Connection ?? new HttpTransportClient(),
            SourceSerializer ?? new((x, y) => x),
            PropertyMappingProvider ?? new DefaultPropertyMappingProvider());

        ConfigureSettings?.Invoke(settings);
        return settings;
    }
}
