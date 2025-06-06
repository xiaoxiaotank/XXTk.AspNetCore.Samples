namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

/// <summary>
/// 后台作业接口
/// </summary>
public interface IBackgroundJob<in TArgs>
{
    /// <summary>
    /// 执行作业
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    Task ExecuteAsync(TArgs args);
}
