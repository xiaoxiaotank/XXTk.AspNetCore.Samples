using Microsoft.Extensions.Logging.Abstractions;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers;

/// <summary>
/// 所有后台工作者的抽象基类
/// </summary>
public abstract class BackgroundWorkerBase : IBackgroundWorker, ISingletonDependency
{
    public BackgroundWorkerBase()
    {
        StoppingToken = StoppingTokenSource.Token;
    }

    public ITransientCachedServiceProvider LazyServiceProvider { get; set; } = default!;

    protected ILoggerFactory? LoggerFactory => LazyServiceProvider.GetService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.GetService<ILogger>(sp => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);

    protected CancellationTokenSource StoppingTokenSource { get; } = new();

    protected CancellationToken StoppingToken { get; }

    public virtual Task StartAsync(CancellationToken cancellationToken = default)
    {
        // 可根据需要设定日志级别
        Logger.LogInformation($"Started background worker: {ToString()}");
        return Task.CompletedTask;
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await StoppingTokenSource.CancelAsync();
        StoppingTokenSource.Dispose();
        // 可根据需要设定日志级别
        Logger.LogInformation($"Stopping background worker: {ToString()}");
    }

    public override string ToString()
    {
        return GetType().FullName!;
    }
}
