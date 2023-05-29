namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Options;

internal abstract class ConfigureOptionsFromOptions<TOptions, TDep> : IConfigureNamedOptions<TOptions>
    where TOptions : class
    where TDep : class
{
    private readonly IOptionsFactory<TDep> optionsFactory;

    public ConfigureOptionsFromOptions(IOptionsFactory<TDep> optionsFactory)
    {
        this.optionsFactory = optionsFactory;
    }

    public void Configure(string? name, TOptions options)
    {
        Configure(options, optionsFactory.Create(name ?? Options.DefaultName));
    }

    public void Configure(TOptions options)
    {
        throw new NotSupportedException();
    }

    protected abstract void Configure(TOptions options, TDep dependency);
}