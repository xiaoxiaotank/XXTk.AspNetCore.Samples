using System.Collections.Concurrent;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Managers;

/// <summary>
/// 默认的基于内存定时器的后台工作者管理器
/// </summary>
public class BackgroundWorkerManager : IBackgroundWorkerManager, IDisposable
{
    private bool _isDisposed;

    private readonly ConcurrentBag<IBackgroundWorker> _backgroundWorkers;

    public BackgroundWorkerManager()
    {
        _backgroundWorkers = [];
    }

    protected bool IsRunning { get; private set; }

    public virtual Task InitAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken = default)
    {
        IsRunning = true;
        foreach (var backgroundWorker in _backgroundWorkers)
        {
            await backgroundWorker.StartAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken = default)
    {
        IsRunning = false;
        foreach (var backgroundWorker in _backgroundWorkers)
        {
            await backgroundWorker.StopAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual async Task AddAsync(IBackgroundWorker backgroundWorker, CancellationToken cancellationToken = default)
    {
        _backgroundWorkers.Add(backgroundWorker);
        if (IsRunning)
        {
            await backgroundWorker.StartAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;
    }
}
