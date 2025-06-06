using System.Threading.Timers;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers.Memory;

/// <summary>
/// 基于内存的后台工作者抽象基类
/// </summary>
public abstract class MemoryBackgroundWorkerBase : BackgroundWorkerBase
{
    protected MemoryBackgroundWorkerBase(
        NoOverlappingAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<NoOverlappingAsyncTimer> timerLogger)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Timer = timer;
        Timer.Elapsed = Timer_ElapsedAsync;
        if (timerLogger != null)
        {
            Timer.Logger = timerLogger;
        }
    }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected NoOverlappingAsyncTimer Timer { get; }

    protected CancellationToken StartCancellationToken { get; set; }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        StartCancellationToken = cancellationToken;

        await base.StartAsync(cancellationToken);

        // 启动定时器
        Timer.Start(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken = default)
    {
        // 停止计时器
        Timer.Stop(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    private async Task Timer_ElapsedAsync(NoOverlappingAsyncTimer timer)
    {
        await DoWorkAsync(StartCancellationToken);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        // 创建服务作用域
        await using var scope = ServiceScopeFactory.CreateAsyncScope();
        try
        {
            await DoWorkAsync(new MemoryBackgroundWorkerContext(scope.ServiceProvider), cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{GetType().FullName!} background worker error");
        }
    }

    /// <summary>
    /// 定时任务核心执行逻辑
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task DoWorkAsync(MemoryBackgroundWorkerContext context, CancellationToken cancellationToken);
}
