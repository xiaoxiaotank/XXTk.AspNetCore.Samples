namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs;

/// <summary>
/// 后台作业名称提供器
/// 可以自定义接口实现
/// </summary>
public interface IBackgroundJobNameProvider
{
    string Name { get; }
}
