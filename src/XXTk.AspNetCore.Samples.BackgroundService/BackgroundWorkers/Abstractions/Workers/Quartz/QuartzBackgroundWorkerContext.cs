using Quartz;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers.Quartz;

/// <summary>
/// 基于 Quartz 的后台工作者上下文
/// </summary>
public class QuartzBackgroundWorkerContext
{
    public QuartzBackgroundWorkerContext(
        IServiceProvider serviceProvider,
        IJobExecutionContext jobExecutionContext)
    {
        ServiceProvider = serviceProvider;
        JobExecutionContext = jobExecutionContext;
    }

    public IServiceProvider ServiceProvider { get; set; }

    public IJobExecutionContext JobExecutionContext { get; set; }
}
