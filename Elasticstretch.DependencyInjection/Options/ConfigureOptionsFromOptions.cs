namespace Elastic.Clients.Elasticsearch.Options;

using Microsoft.Extensions.Options;

internal abstract class ConfigureOptionsFromOptions<TOptions, TDep>(IOptionsFactory<TDep> optionsFactory) : IConfigureNamedOptions<TOptions>
    where TOptions : class
    where TDep : class
{
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