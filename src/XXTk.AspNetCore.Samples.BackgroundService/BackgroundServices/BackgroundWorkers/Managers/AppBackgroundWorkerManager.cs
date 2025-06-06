using Microsoft.Extensions.Options;
using Quartz;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers.Quartz;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Managers;

/// <summary>
/// 基于Quartz和内存的后台工作者管理器
/// </summary>
[Dependency(ReplaceServices = true)]
public class AppBackgroundWorkerManager : BackgroundWorkerManager
{
    public AppBackgroundWorkerManager(
        IScheduler scheduler,
        IServiceProvider serviceProvider,
        IOptions<AppBackgroundWorkerManagerOptions> options)
    {
        Scheduler = scheduler;
        ServiceProvider = serviceProvider;
        Options = options.Value;
    }

    /// <summary>
    /// 默认 Quartz 调度器
    /// </summary>
    protected IScheduler Scheduler { get; }

    protected IServiceProvider ServiceProvider { get; }

    protected AppBackgroundWorkerManagerOptions Options { get; }

    public override async Task InitAsync(CancellationToken cancellationToken = default)
    {
        foreach (var backgroundWorkerType in Options.BackgroundWorkerTypes)
        {
            await AddAsync((IBackgroundWorker)ServiceProvider.GetRequiredService(backgroundWorkerType));
        }

        await base.InitAsync(cancellationToken);
    }

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        // 若调度器已启动，但处于 Standby 模式，则恢复运行
        // 这里判断 IsStarted 是为了与 QuartzHostedService 协同
        if (Scheduler.IsStarted && Scheduler.InStandbyMode)
        {
            await Scheduler.Start(cancellationToken);
        }

        // 启动非 Quartz 管理的后台工作者
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken = default)
    {
        // 若调度器已启动，且不处于 Standby 模式，则进入 Standby 模式
        if (Scheduler.IsStarted && !Scheduler.InStandbyMode)
        {
            await Scheduler.Standby(cancellationToken);
        }

        // 停止非 Quartz 管理的后台工作者
        await base.StopAsync(cancellationToken);
    }

    public override async Task AddAsync(IBackgroundWorker backgroundWorker, CancellationToken cancellationToken = default)
    {
        await ReScheduleJobAsync(backgroundWorker, cancellationToken);
    }

    protected virtual async Task ReScheduleJobAsync(IBackgroundWorker backgroundWorker, CancellationToken cancellationToken = default)
    {
        switch (backgroundWorker)
        {
            // Quartz
            case IQuartzBackgroundWorker quartzWorker:
                {
                    Check.NotNull(quartzWorker.Trigger, nameof(quartzWorker.Trigger));
                    Check.NotNull(quartzWorker.JobDetail, nameof(quartzWorker.JobDetail));

                    if (quartzWorker.ScheduleJob != null)
                    {
                        await quartzWorker.ScheduleJob.Invoke(Scheduler);
                    }
                    else
                    {
                        await DefaultScheduleJobAsync(quartzWorker, cancellationToken);
                    }

                    break;
                }
            // 其他
            default:
                await base.AddAsync(backgroundWorker, cancellationToken);
                break;
        }
    }

    /// <summary>
    /// 使用默认调度器调度Job
    /// </summary>
    /// <param name="quartzWorker"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task DefaultScheduleJobAsync(IQuartzBackgroundWorker quartzWorker, CancellationToken cancellationToken = default)
    {
        // 若 Job 已经存在于默认调度器中
        if (await Scheduler.CheckExists(quartzWorker.JobDetail.Key, cancellationToken))
        {
            // 替换掉旧的（因为 JobDetail 配置可能发生了变化）
            await Scheduler.AddJob(quartzWorker.JobDetail, true, true, cancellationToken);
            // 恢复Job运行
            await Scheduler.ResumeJob(quartzWorker.JobDetail.Key, cancellationToken);
            // 替换掉旧的（因为 Trigger 配置可能发生了变化）
            await Scheduler.RescheduleJob(quartzWorker.Trigger.Key, quartzWorker.Trigger, cancellationToken);
        }
        else
        {
            // 加入调度
            await Scheduler.ScheduleJob(quartzWorker.JobDetail, quartzWorker.Trigger, cancellationToken);
        }
    }
}
