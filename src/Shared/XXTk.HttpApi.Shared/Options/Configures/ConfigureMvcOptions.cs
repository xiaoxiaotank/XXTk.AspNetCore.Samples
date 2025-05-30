using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using XXTk.HttpApi.Shared.Mvc.Filters.ApiResponse;

namespace XXTk.HttpApi.Shared.Options.Configures
{
    public class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.Filters.Add<ApiResponseWrapFilter>();
        }
    }
}
