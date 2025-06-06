namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundWorkers.Workers;

/// <summary>
/// 后台工作者：在后台持续定时执行的任务
/// </summary>
public interface IBackgroundWorker
{
    /// <summary>
    /// 启动
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task StopAsync(CancellationToken cancellationToken = default);
}
