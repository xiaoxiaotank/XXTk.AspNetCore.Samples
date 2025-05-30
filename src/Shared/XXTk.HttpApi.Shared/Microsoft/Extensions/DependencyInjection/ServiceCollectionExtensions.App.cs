using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using XXTk.HttpApi.Shared.Mvc.Handlers.ExceptionHandlers;
using XXTk.HttpApi.Shared.Options.Configures;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppDefault(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

        services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
        services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSingleton<IConfigureOptions<ApiVersioningOptions>, ConfigureApiVersioningOptions>();
        services.AddSingleton<IConfigureOptions<ApiExplorerOptions>, ConfigureApiExplorerOptions>();
        services.AddSingleton<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();

        services.AddExceptionHandler<ApiExceptionHandler>();
        // 当启用 app.UseExceptionHandler(); 未传入任何参数时，必须注册该服务
        services.AddProblemDetails();

        return services;
    }
}