using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;

namespace XXTk.HttpApi.Shared.Options.Configures;

public class ConfigureApiExplorerOptions : IConfigureOptions<ApiExplorerOptions>
{
    public void Configure(ApiExplorerOptions options)
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
}
