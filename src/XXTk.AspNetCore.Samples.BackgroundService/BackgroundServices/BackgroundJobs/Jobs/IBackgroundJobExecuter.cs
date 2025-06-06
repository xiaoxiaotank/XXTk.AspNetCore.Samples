namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

/// <summary>
/// 后台作业执行接口
/// </summary>
public interface IBackgroundJobExecuter : ITransientDependency
{
    /// <summary>
    /// 执行作业
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ExecuteAsync(JobExecutionContext context);
}
