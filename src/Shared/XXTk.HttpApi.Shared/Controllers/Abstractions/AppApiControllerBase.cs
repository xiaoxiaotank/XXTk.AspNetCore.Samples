using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace XXTk.HttpApi.Shared.Controllers.Abstractions;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class AppApiControllerBase : ControllerBase
{
    public ITransientCachedServiceProvider LazyServiceProvider { get; set; } = default!;

    protected IHttpContextAccessor HttpContextAccessor => LazyServiceProvider.GetRequiredService<IHttpContextAccessor>();

    protected IConfiguration Configuration => LazyServiceProvider.GetRequiredService<IConfiguration>();

    protected ILoggerFactory? LoggerFactory => LazyServiceProvider.GetService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.GetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);
}