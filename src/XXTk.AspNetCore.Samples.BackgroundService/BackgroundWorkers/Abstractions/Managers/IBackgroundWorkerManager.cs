using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Managers;

/// <summary>
/// 后台工作者管理器（用于在后台持续定时执行的任务）
/// 
/// 启动后台工作者管理器方式：先调用 InitAsync，再调用 StartAsync
/// </summary>
public interface IBackgroundWorkerManager : ISingletonDependency
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 启动所有后台工作者
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 停止所有后台工作者
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task StopAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加后台工作者（会根据调度器状态决定是否启动）
    /// </summary>
    /// <param name="backgroundWorker"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(IBackgroundWorker backgroundWorker, CancellationToken cancellationToken = default);
}
