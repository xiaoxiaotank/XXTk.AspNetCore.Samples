using Quartz;
using System.Collections.Immutable;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Managers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs.Quartz;

/// <summary>
/// 基于 Quartz 的后台作业选项
/// </summary>
public class QuartzBackgroundJobOptions
{
    // 重试策略
    private Func<int, IJobExecutionContext, JobExecutionException, Task> _retryStrategy;

    // 作业配置字典，按参数类型索引
    private readonly Dictionary<Type, BackgroundJobConfiguration> _jobConfigurationsByArgsType = [];

    // 作业配置自第那，按作业名索引
    private readonly Dictionary<string, BackgroundJobConfiguration> _jobConfigurationsByName = [];

    public QuartzBackgroundJobOptions()
    {
        RetryCount = 3;
        RetryIntervalMillisecond = 3000;
        _retryStrategy = DefaultRetryStrategy;
    }

    /// <summary>
    /// 重试次数
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// 重试时间间隔（毫秒）
    /// </summary>
    public int RetryIntervalMillisecond { get; set; }

    /// <summary>
    /// 重试策略
    /// </summary>
    public Func<int, IJobExecutionContext, JobExecutionException, Task> RetryStrategy
    {
        get => _retryStrategy;
        set => _retryStrategy = Check.NotNull(value, nameof(value));
    }

    /// <summary>
    /// 默认重试策略：立即按照重试间隔重试，直到超过最大重试次数
    /// </summary>
    /// <param name="retryIndex"></param>
    /// <param name="executionContext"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private async Task DefaultRetryStrategy(int retryIndex, IJobExecutionContext executionContext, JobExecutionException exception)
    {
        exception.RefireImmediately = true;

        var retryCount = executionContext.JobDetail.JobDataMap.GetString(QuartzJobDataConsts.RetryCount)!.To<int>();
        if (retryIndex > retryCount)
        {
            exception.RefireImmediately = false;
            exception.UnscheduleAllTriggers = true;
            return;
        }

        var retryInterval = executionContext.JobDetail.JobDataMap.GetString(QuartzJobDataConsts.RetryIntervalMillisecond)!.To<int>();
        await Task.Delay(retryInterval);
    }

    /// <summary>
    /// 根据参数类型获取作业配置
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    /// <returns></returns>
    public BackgroundJobConfiguration GetJob<TArgs>()
    {
        return GetJob(typeof(TArgs));
    }

    /// <summary>
    /// 根据参数类型获取作业配置
    /// </summary>
    /// <param name="argsType"></param>
    /// <returns></returns>
    /// <exception cref="AbpException"></exception>
    public BackgroundJobConfiguration GetJob(Type argsType)
    {
        var jobConfiguration = _jobConfigurationsByArgsType.GetOrDefault(argsType)
            ?? throw new Exception("Undefined background job for the job args type: " + argsType.AssemblyQualifiedName);
        return jobConfiguration;
    }

    /// <summary>
    /// 根据作业名获取作业配置
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public BackgroundJobConfiguration GetJob(string name)
    {
        var jobConfiguration = _jobConfigurationsByName.GetOrDefault(name)
            ?? throw new Exception("Undefined background job for the job name: " + name);
        return jobConfiguration;
    }

    /// <summary>
    /// 获取所有作业
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<BackgroundJobConfiguration> GetJobs()
    {
        return _jobConfigurationsByArgsType.Values.ToImmutableList();
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    public void AddJob<TJob>()
    {
        AddJob(typeof(TJob));
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <param name="jobType"></param>
    public void AddJob(Type jobType)
    {
        AddJob(new BackgroundJobConfiguration(jobType));
    }

    /// <summary>
    /// 添加作业
    /// </summary>
    /// <param name="jobConfiguration"></param>
    public void AddJob(BackgroundJobConfiguration jobConfiguration)
    {
        _jobConfigurationsByArgsType[jobConfiguration.ArgsType] = jobConfiguration;
        _jobConfigurationsByName[jobConfiguration.JobName] = jobConfiguration;
    }
}
