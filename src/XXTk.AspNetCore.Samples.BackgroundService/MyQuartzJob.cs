using System.Text.Json;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

namespace XXTk.AspNetCore.Samples.BackgroundService;

[BackgroundJobName("MyQuartzJob")]
public class MyQuartzJob : BackgroundJob<MyQuartzJobArgs>, ITransientDependency
{
    public MyQuartzJob(ILogger<MyQuartzJob> logger)
    {
        Logger = logger;
    }

    public override Task ExecuteAsync(MyQuartzJobArgs args)
    {
        Logger.LogError($"MyQuartzJob：{JsonSerializer.Serialize(args)}");

        return Task.CompletedTask;
    }
}

public class MyQuartzJobArgs
{
    public string Name { get; set; } = default!;
}