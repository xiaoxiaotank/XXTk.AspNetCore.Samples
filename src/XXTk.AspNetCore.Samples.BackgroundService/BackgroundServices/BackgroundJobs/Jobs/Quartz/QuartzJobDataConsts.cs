namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs.Quartz;

/// <summary>
/// Quartz 作业数据常量
/// </summary>
public static class QuartzJobDataConsts
{
    public const string Prefix = "App";

    public const string Args = Prefix + "Args";
    public const string RetryCount = Prefix + "RetryCount";
    public const string RetryIntervalMillisecond = Prefix + "RetryIntervalMillisecond";
    public const string RetryIndex = Prefix + "RetryIndex";
}
