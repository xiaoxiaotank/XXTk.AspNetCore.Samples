using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Managers;

public class AppBackgroundWorkerManagerOptions
{
    public HashSet<Type> BackgroundWorkerTypes { get; } = [];

    public AppBackgroundWorkerManagerOptions AddBackgroundWoker<TWorker>()
        where TWorker : IBackgroundWorker
    {
        BackgroundWorkerTypes.Add(typeof(TWorker));

        return this;
    }
}
