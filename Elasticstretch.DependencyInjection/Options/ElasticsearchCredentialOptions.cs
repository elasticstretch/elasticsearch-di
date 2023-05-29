namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Transport;

/// <summary>
/// Options for authenticating with Elasticsearch nodes.
/// </summary>
public class ElasticsearchCredentialOptions
{
    /// <summary>
    /// Gets or sets the ID of the API key for authentication, if any.
    /// </summary>
    public string? ApiKeyId { get; set; }

    /// <summary>
    /// Gets or sets the API key for authentication, if any.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Attempts to create an Elasticsearch authorization header from the options.
    /// </summary>
    /// <returns>
    /// An HTTP header containing the credentials, or <see langword="null"/> if no credentials could be created.
    /// </returns>
    public AuthorizationHeader? CreateHeader()
    {
        if (ApiKey != null)
        {
            return ApiKeyId != null ? new Base64ApiKey(ApiKeyId, ApiKey) : new ApiKey(ApiKey);
        }

        return null;
    }
}
