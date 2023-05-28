namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Transport;

/// <summary>
/// Options for connecting to Elasticsearch nodes.
/// </summary>
public class ElasticsearchNodeOptions
{
    /// <summary>
    /// Gets or sets the ID for Elastic's cloud-hosted search service.
    /// </summary>
    /// <remarks>
    /// Incompatible with <see cref="NodeUris"/>.
    /// </remarks>
    public string? CloudId { get; set; }

    /// <summary>
    /// Gets or sets the list of self-hosted Elasticsearch node URIs. 
    /// </summary>
    /// <remarks>
    /// Incompatible with <see cref="CloudId"/>.
    /// </remarks>
    public ICollection<Uri> NodeUris { get; } = new List<Uri>();

    /// <summary>
    /// Gets or sets whether "sticky" node selection is used (e.g. <see cref="StickyNodePool"/>).
    /// </summary>
    public bool IsSticky { get; set; }

    /// <summary>
    /// Gets or sets whether "sniffing" is used to reseed nodes (e.g. <see cref="SniffingNodePool"/>).
    /// </summary>
    public bool UseSniffing { get; set; }

    /// <summary>
    /// Gets or sets the ID of the API key for node authentication, if any.
    /// </summary>
    public string? ApiKeyId { get; set; }

    /// <summary>
    /// Gets or sets the API key for node authentication, if any.
    /// </summary>
    public string? ApiKey { get; set; }
}
