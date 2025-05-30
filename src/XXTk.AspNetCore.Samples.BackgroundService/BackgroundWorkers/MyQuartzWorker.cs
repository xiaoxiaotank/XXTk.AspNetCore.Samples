using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers.Quartz;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers;

public class MyQuartzWorker : QuartzBackgroundWorkerBase<MyQuartzWorker>
{
    public MyQuartzWorker()
    {
        SetByPeriod(1000);
    }

    protected override Task DoWorkAsync(QuartzBackgroundWorkerContext context, CancellationToken cancellationToken)
    {
        var sp = context.ServiceProvider;

        Logger.LogInformation($"[{DateTime.Now}]Executing {GetType().FullName}");

        return Task.CompletedTask;
    }
}
