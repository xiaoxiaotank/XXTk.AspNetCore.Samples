using System.Threading.Timers;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers.Memory;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers;

public class MyMemoryWorker : MemoryBackgroundWorkerBase
{
    public MyMemoryWorker(NoOverlappingAsyncTimer timer, IServiceScopeFactory serviceScopeFactory, ILogger<NoOverlappingAsyncTimer> timerLogger)
        : base(timer, serviceScopeFactory, timerLogger)
    {
        timer.Period = 1000;
    }

    protected override Task DoWorkAsync(MemoryBackgroundWorkerContext context, CancellationToken cancellationToken)
    {
        var sp = context.ServiceProvider;

        Logger.LogInformation($"[{DateTime.Now}]Executing {GetType().FullName}");

        return Task.CompletedTask;
    }
}
