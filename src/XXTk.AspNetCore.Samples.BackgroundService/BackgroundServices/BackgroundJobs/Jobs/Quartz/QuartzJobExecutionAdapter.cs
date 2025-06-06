using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Quartz;
using System.Text.Json;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs.Quartz;

/// <summary>
/// Quartz 作业执行适配器
/// </summary>
public class QuartzJobExecutionAdapter<TArgs> : IJob
{
    public QuartzJobExecutionAdapter(
        IOptions<QuartzBackgroundJobOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        IBackgroundJobExecuter jobExecuter)
    {
        Options = options.Value;
        ServiceScopeFactory = serviceScopeFactory;
        JobExecuter = jobExecuter;
        Logger = NullLogger<QuartzJobExecutionAdapter<TArgs>>.Instance;
    }

    public ILogger<QuartzJobExecutionAdapter<TArgs>> Logger { get; set; }

    protected QuartzBackgroundJobOptions Options { get; set; }

    protected IServiceScopeFactory ServiceScopeFactory { get; set; }

    protected IBackgroundJobExecuter JobExecuter { get; set; }

    public virtual async Task Execute(IJobExecutionContext context)
    {
        // 创建服务作用域
        await using var scope = ServiceScopeFactory.CreateAsyncScope();
        var args = JsonSerializer.Deserialize<TArgs>(context.JobDetail.JobDataMap.GetString(QuartzJobDataConsts.Args)!);
        var jobType = Options.GetJob(typeof(TArgs)).JobType;
        var jobContext = new JobExecutionContext(scope.ServiceProvider, jobType, args, context.CancellationToken);
        try
        {
            await JobExecuter.ExecuteAsync(jobContext).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // 按照重试策略重试
            var jobExecutionException = new JobExecutionException(ex);

            var retryIndex = context.JobDetail.JobDataMap.GetString(QuartzJobDataConsts.RetryIndex)?.To<int>() ?? 0;
            retryIndex++;
            context.JobDetail.JobDataMap.Put(QuartzJobDataConsts.RetryIndex, retryIndex.ToString());

            await Options.RetryStrategy(retryIndex, context, jobExecutionException).ConfigureAwait(false);

            throw jobExecutionException;
        }
    }
}
