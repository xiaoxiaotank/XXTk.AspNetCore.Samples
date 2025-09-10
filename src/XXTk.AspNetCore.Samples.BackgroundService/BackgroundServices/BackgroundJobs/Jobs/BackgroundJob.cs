using Microsoft.Extensions.Logging.Abstractions;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

/// <summary>
/// 后台作业抽象类
/// </summary>
public abstract class BackgroundJob<TArgs> : IBackgroundJob<TArgs>
{
    public ITransientCachedServiceProvider LazyServiceProvider { get; set; } = default!;

    protected ILoggerFactory? LoggerFactory => LazyServiceProvider.GetService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.GetService<ILogger>(sp => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);

    public abstract Task ExecuteAsync(TArgs args);
}
