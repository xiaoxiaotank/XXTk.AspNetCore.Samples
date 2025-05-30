using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using XXTk.HttpApi.Shared.Swagger;
using XXTk.HttpApi.Shared.Swagger.Filters;

namespace XXTk.HttpApi.Shared.Options.Configures;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionProvider;
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionProvider) => _apiVersionProvider = apiVersionProvider;
    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        options.OperationFilter<SwaggerDefaultValues>();
        options.IncludeXmlCommentsIfFileExist(Path.Combine(AppContext.BaseDirectory, Assembly.GetEntryAssembly()!.GetName().Name + ".xml"), true);
        options.CustomSchemaIds(t => t.ToString());
        options.CustomOperationIds(apiDesc =>
        {
            var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
            return controllerAction!.ControllerName + "-" + controllerAction.ActionName;
        });
        foreach (var description in _apiVersionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
        options.OperationFilter<CustomSummaryOperationFilter>();
    }
    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Sample API",
            Version = description.ApiVersion.ToString(),
            Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
            //License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        };
        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }
        return info;
    }
}
