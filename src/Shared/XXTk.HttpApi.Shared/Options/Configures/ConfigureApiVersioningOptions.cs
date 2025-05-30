using Asp.Versioning;
using Microsoft.Extensions.Options;

namespace XXTk.HttpApi.Shared.Options.Configures;

public class ConfigureApiVersioningOptions : IConfigureOptions<ApiVersioningOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        options.ReportApiVersions = true;
    }
}
