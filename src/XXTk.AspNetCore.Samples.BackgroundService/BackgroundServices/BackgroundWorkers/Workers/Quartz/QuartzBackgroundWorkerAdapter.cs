using Quartz;
using System.Reflection;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers.Quartz;

/// <summary>
/// 基于 Quartz 的后台工作者适配器
/// </summary>
[IgnoreInjection]   // 禁用自动依赖注入
[DisallowConcurrentExecution]   // 禁止并发执行
public class QuartzBackgroundWorkerAdapter<TWorker> : QuartzBackgroundWorkerBase, IQuartzBackgroundWorkerAdapter
    where TWorker : IBackgroundWorker
{
    private readonly MethodInfo? _doWorkAsyncMethod;

    public QuartzBackgroundWorkerAdapter()
    {
        _doWorkAsyncMethod = typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // 创建服务作用域
        await using var scope = LazyServiceProvider.CreateAsyncScope();
        try
        {
            var worker = (IBackgroundWorker)LazyServiceProvider.GetRequiredService(typeof(TWorker));
            var workerContext = new QuartzBackgroundWorkerContext(scope.ServiceProvider, context);

            if (_doWorkAsyncMethod != null)
            {
                await (Task)_doWorkAsyncMethod.Invoke(worker, [workerContext, context.CancellationToken])!;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{GetType().FullName} background worker error");
        }
    }

    protected override Task DoWorkAsync(QuartzBackgroundWorkerContext workerContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
