namespace Elastic.Clients.Elasticsearch.Options;

using Elastic.Transport;
using Microsoft.Extensions.Options;

sealed class ConfigureClientFromNodes : IConfigureNamedOptions<ElasticsearchClientOptions>
{
    private readonly IOptionsFactory<ElasticsearchNodeOptions> nodeOptionsFactory;

    public ConfigureClientFromNodes(IOptionsFactory<ElasticsearchNodeOptions> nodeOptionsFactory)
    {
        this.nodeOptionsFactory = nodeOptionsFactory;
    }

    public void Configure(string? name, ElasticsearchClientOptions options)
    {
        var nodeOptions = nodeOptionsFactory.Create(name ?? Options.DefaultName);

        AuthorizationHeader? credentials = nodeOptions switch
        {
            { ApiKeyId: not null, ApiKey: not null } => new Base64ApiKey(nodeOptions.ApiKeyId, nodeOptions.ApiKey),
            { ApiKey: not null } => new ApiKey(nodeOptions.ApiKey),
            _ => null,
        };

        options.NodePool = nodeOptions switch
        {
            { CloudId: not null } => new CloudNodePool(nodeOptions.CloudId, credentials ?? NoAuthorization.Instance),
            { NodeUris.Count : 0 } => options.NodePool,
            { IsSticky : true, UseSniffing : true } => new StickySniffingNodePool(nodeOptions.NodeUris, x => 0),
            { IsSticky : true } => new StickyNodePool(nodeOptions.NodeUris),
            { UseSniffing : true } => new SniffingNodePool(nodeOptions.NodeUris),
            _ => new StaticNodePool(nodeOptions.NodeUris),
        };

        if (credentials != null)
        {
            options.ConfigureSettings += x => x.Authentication(credentials);
        }
    }

    public void Configure(ElasticsearchClientOptions options)
    {
        throw new NotSupportedException();
    }

    sealed class NoAuthorization : AuthorizationHeader
    {
        public static NoAuthorization Instance { get; } = new NoAuthorization();

        public override string AuthScheme => throw new NotSupportedException();

        private NoAuthorization()
        {
        }

        public override bool TryGetAuthorizationParameters(out string value)
        {
            value = string.Empty;
            return false;
        }
    }
}
