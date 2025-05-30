using NLog.Web;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAppNLog(
        this WebApplicationBuilder builder,
        bool clearProviders = false)
    {
        if (clearProviders)
        {
            builder.Logging.ClearProviders();
        }
        builder.Host.UseNLog();

        return builder;
    }
}
