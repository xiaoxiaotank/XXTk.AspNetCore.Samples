using Microsoft.Extensions.Options;
using Quartz;
using System.Text.Json;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs.Quartz;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Managers;

/// <summary>
/// 基于 Quartz 的后台作业管理器
/// </summary>
public class QuartzBackgroundJobManager : IBackgroundJobManager
{
    protected IScheduler Scheduler { get; }

    protected QuartzBackgroundJobOptions Options { get; }

    public QuartzBackgroundJobManager(IScheduler scheduler, IOptions<QuartzBackgroundJobOptions> options)
    {
        Scheduler = scheduler;
        Options = options.Value;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundServicePriority priority = BackgroundServicePriority.Normal,
        TimeSpan? delay = null)
    {
        return await ReEnqueueAsync(args, Options.RetryCount, Options.RetryIntervalMillisecond, priority, delay);
    }

    public virtual async Task<string> ReEnqueueAsync<TArgs>(
        TArgs args,
        int retryCount,
        int retryIntervalMillisecond,
        BackgroundServicePriority priority = BackgroundServicePriority.Normal,
        TimeSpan? delay = null)
    {
        var jobDataMap = new JobDataMap
        {
            [QuartzJobDataConsts.Args] = JsonSerializer.Serialize(args!),
            [QuartzJobDataConsts.RetryCount] = retryCount.ToString(),
            [QuartzJobDataConsts.RetryIntervalMillisecond] = retryIntervalMillisecond.ToString(),
            [QuartzJobDataConsts.RetryIndex] = "0"
        };

        // 为了区分同一作业的不同实例，将作业名作为分组，拼上guid作为名称
        var group = GetJobName(typeof(TArgs));
        var name = group + "-" + Guid.NewGuid().ToString();
        var jobDetail = JobBuilder
            .Create<QuartzJobExecutionAdapter<TArgs>>()
            .RequestRecovery()
            .SetJobData(jobDataMap)
            .WithIdentity(name, group)
            .Build();
        var trigger = !delay.HasValue
            ? TriggerBuilder.Create().StartNow().WithIdentity(name, group).WithPriority((int)priority).Build()
            : TriggerBuilder.Create().StartAt(new DateTimeOffset(DateTime.Now.Add(delay.Value))).WithIdentity(name, group).WithPriority((int)priority).Build();

        await Scheduler.ScheduleJob(jobDetail, trigger);
        return jobDetail.Key.ToString();
    }

    protected static string GetJobName(Type jobArgsType)
    {
        Check.NotNull(jobArgsType, nameof(jobArgsType));

        return (jobArgsType
            .GetCustomAttributes(true)
            .OfType<IBackgroundJobNameProvider>()
            .FirstOrDefault()?
            .Name
            ?? jobArgsType.FullName);
    }
}
