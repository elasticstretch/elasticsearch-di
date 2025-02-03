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
    public ICollection<Uri> NodeUris { get; } = [];

    /// <summary>
    /// Gets or sets whether "sticky" node selection is used (e.g. <see cref="StickyNodePool"/>).
    /// </summary>
    public bool IsSticky { get; set; }

    /// <summary>
    /// Gets or sets whether "sniffing" is used to reseed nodes (e.g. <see cref="SniffingNodePool"/>).
    /// </summary>
    public bool UseSniffing { get; set; }

    /// <summary>
    /// Gets or sets whether to shuffle the node order on initialization.
    /// </summary>
    public bool Randomize { get; set; }

    /// <summary>
    /// Gets or sets the connection authentication credentials.
    /// </summary>
    public AuthorizationHeader? CredentialsHeader { get; set; }

    /// <summary>
    /// Attempts to create an Elasticsearch node pool from the options.
    /// </summary>
    /// <returns>The resulting node pool, or <see langword="null"/> if no pool could be created.</returns>
    public NodePool? CreatePool()
    {
        if (CloudId != null)
        {
            return new CloudNodePool(
                CloudId,
                CredentialsHeader ?? throw new InvalidOperationException("Missing Elastic cloud credentials."));
        }

        if (NodeUris.Count > 0)
        {
            if (IsSticky)
            {
                return UseSniffing
                    ? new StickySniffingNodePool(NodeUris, x => 0)
                    : new StickyNodePool(NodeUris);
            }
            else
            {
                return UseSniffing
                    ? new SniffingNodePool(NodeUris, Randomize)
                    : new StaticNodePool(NodeUris, Randomize);
            }
        }

        return null;
    }
}
