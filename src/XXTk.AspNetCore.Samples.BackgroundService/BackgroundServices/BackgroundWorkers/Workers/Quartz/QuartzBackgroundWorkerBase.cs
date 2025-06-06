using Quartz;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers.Quartz;

/// <summary>
/// 基于 Quartz 的后台工作者抽象基类
/// </summary>
public abstract class QuartzBackgroundWorkerBase : BackgroundWorkerBase, IQuartzBackgroundWorker
{
    public ITrigger Trigger { get; set; } = default!;
    public IJobDetail JobDetail { get; set; } = default!;
    public Func<IScheduler, Task>? ScheduleJob { get; set; }

    /// <summary>
    /// 定时任务核心执行逻辑
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task DoWorkAsync(QuartzBackgroundWorkerContext context, CancellationToken cancellationToken);
}

public abstract class QuartzBackgroundWorkerBase<TWorker> : QuartzBackgroundWorkerBase
    where TWorker : QuartzBackgroundWorkerBase
{
    protected virtual JobBuilder CreateJobBuilder()
    {
        return JobBuilder.Create<QuartzBackgroundWorkerAdapter<TWorker>>().WithIdentity(typeof(TWorker).FullName!);
    }

    protected virtual TriggerBuilder CreateTriggerBuilder(BackgroundServicePriority priority = BackgroundServicePriority.Normal)
    {
        return TriggerBuilder.Create().WithIdentity(typeof(TWorker).FullName!).WithPriority((int)priority);
    }

    /// <summary>
    /// 通过周期快速设置触发器和作业详情
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    protected virtual void SetByPeriod(int period, BackgroundServicePriority priority = BackgroundServicePriority.Normal)
    {
        JobDetail = CreateJobBuilder().Build();
        Trigger = CreateTriggerBuilder(priority: priority).WithSimpleSchedule(builder =>
        {
            builder.WithInterval(TimeSpan.FromMilliseconds(period)).RepeatForever();
        }).Build();
    }
}
