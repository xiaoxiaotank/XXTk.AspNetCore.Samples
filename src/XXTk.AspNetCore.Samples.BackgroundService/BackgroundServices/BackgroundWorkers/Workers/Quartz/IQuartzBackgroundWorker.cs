using Quartz;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers.Quartz;

/// <summary>
/// 基于 Quartz 的后台工作者
/// </summary>
public interface IQuartzBackgroundWorker : IBackgroundWorker
{
    ITrigger Trigger { get; set; }

    IJobDetail JobDetail { get; set; }

    Func<IScheduler, Task>? ScheduleJob { get; set; }
}
