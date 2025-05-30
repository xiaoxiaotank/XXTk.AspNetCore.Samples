namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers.Memory;

/// <summary>
/// 基于内存的后台工作者上下文
/// </summary>
public class MemoryBackgroundWorkerContext
{
    public MemoryBackgroundWorkerContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; set; }
}
