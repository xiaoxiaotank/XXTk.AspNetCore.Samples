namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs;

/// <summary>
/// 后台作业配置
/// </summary>
public class BackgroundJobConfiguration
{
    /// <summary>
    /// 参数类型
    /// </summary>
    public Type ArgsType { get; }

    /// <summary>
    /// 作业类型
    /// </summary>
    public Type JobType { get; }

    /// <summary>
    /// 作业名称
    /// </summary>
    public string JobName { get; }

    public BackgroundJobConfiguration(Type jobType)
    {
        JobType = jobType;
        ArgsType = BackgroundJobArgsHelper.GetJobArgsType(jobType);
        JobName = BackgroundJobNameAttribute.GetName(ArgsType);
    }
}
