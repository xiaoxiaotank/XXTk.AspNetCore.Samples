using Microsoft.Extensions.Logging.Abstractions;
using System.Data.Common;
using System.Text.Json;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

/// <summary>
/// 后台作业执行器
/// </summary>
public class BackgroundJobExecuter : IBackgroundJobExecuter
{
    public BackgroundJobExecuter(ILogger<BackgroundJobExecuter> logger)
    {
        Logger = logger;
    }

    private ILogger<BackgroundJobExecuter> Logger { get; set; }

    public virtual async Task ExecuteAsync(JobExecutionContext context)
    {
        var job = context.ServiceProvider.GetService(context.JobType)
            ?? throw new Exception("The job type is not registered to DI: " + context.JobType);

        var jobExecuteMethod = context.JobType.GetMethod(nameof(IBackgroundJob<object>.ExecuteAsync))
            ?? throw new Exception($"Given job type does not implement {typeof(IBackgroundJob<>).Name}. " +
                                   "The job type was: " + context.JobType);

        try
        {
            Logger.LogInformation($"{context.JobType.AssemblyQualifiedName!}后台作业开始执行，Args：{JsonSerializer.Serialize(context.JobArgs)}");
            await (Task)jobExecuteMethod.Invoke(job, [context.JobArgs])!;
            Logger.LogInformation($"{context.JobType.AssemblyQualifiedName!}后台作业执行成功");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{context.JobType.AssemblyQualifiedName!}后台作业执行出错，Args：{JsonSerializer.Serialize(context.JobArgs)}");
            throw;
        }
    }
}
