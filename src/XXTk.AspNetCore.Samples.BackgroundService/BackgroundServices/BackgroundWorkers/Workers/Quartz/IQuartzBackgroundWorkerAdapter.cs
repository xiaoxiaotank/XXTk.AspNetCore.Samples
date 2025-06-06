using Quartz;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers.Quartz;

/// <summary>
/// 基于 Quartz 的后台工作者适配器接口
/// </summary>
public interface IQuartzBackgroundWorkerAdapter : IQuartzBackgroundWorker, IJob
{
}
