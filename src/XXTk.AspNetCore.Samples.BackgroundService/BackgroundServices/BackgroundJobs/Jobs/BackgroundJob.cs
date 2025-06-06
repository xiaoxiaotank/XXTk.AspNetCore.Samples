using Microsoft.Extensions.Logging.Abstractions;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

/// <summary>
/// 后台作业抽象类
/// </summary>
public abstract class BackgroundJob<TArgs> : IBackgroundJob<TArgs>
{
    protected BackgroundJob()
    {
        Logger = NullLogger<BackgroundJob<TArgs>>.Instance;
    }

    public ILogger<BackgroundJob<TArgs>> Logger { get; set; }

    public abstract Task ExecuteAsync(TArgs args);
}
