using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace XXTk.HttpApi.Shared.Options.Configures;

public class ConfigureSwaggerUIOptions : IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionProvider;
    public ConfigureSwaggerUIOptions(IApiVersionDescriptionProvider apiVersionProvider)
    {
        _apiVersionProvider = apiVersionProvider;
    }

    /// <inheritdoc />
    public void Configure(SwaggerUIOptions options)
    {
        options.RoutePrefix = "swagger";

        foreach (var description in _apiVersionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    }
}
