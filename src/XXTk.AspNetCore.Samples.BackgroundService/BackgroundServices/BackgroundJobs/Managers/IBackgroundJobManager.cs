namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Managers;

/// <summary>
/// 后台作业：在后台执行某个任务，执行完毕后退出
/// </summary>
public interface IBackgroundJobManager : ITransientDependency
{
    /// <summary>
    /// 入队
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    /// <param name="args"></param>
    /// <param name="priority"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundServicePriority priority = BackgroundServicePriority.Normal,
        TimeSpan? delay = null
    );
}
