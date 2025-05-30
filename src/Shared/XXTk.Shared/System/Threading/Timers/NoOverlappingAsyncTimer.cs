using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace System.Threading.Timers;

public class NoOverlappingAsyncTimer : ITransientDependency
{
    // 真正执行的定时器
    private readonly Timer _taskTimer;
    // 指示是否正在执行任务委托
    private volatile bool _performingTasks;
    // 指示当前定时器（NoOverlappingAsyncTimer）是否正在运行
    private volatile bool _isRunning;

    public NoOverlappingAsyncTimer()
    {
        Logger = NullLogger<NoOverlappingAsyncTimer>.Instance;

        _taskTimer = new Timer(
            TimerCallBack,
            null,
            Timeout.Infinite,
            Timeout.Infinite);
    }

    /// <summary>
    /// 定时执行的委托
    /// </summary>
    public Func<NoOverlappingAsyncTimer, Task> Elapsed = _ => Task.CompletedTask;

    /// <summary>
    /// 周期（毫秒）
    /// </summary>
    public int Period { get; set; }

    /// <summary>
    /// 是否在启动时执行一次
    /// </summary>
    public bool RunOnStart { get; set; }

    /// <summary>
    /// 日志，默认为 NullLogger，可以自己指定
    /// </summary>
    public ILogger<NoOverlappingAsyncTimer> Logger { get; set; }

    public void Start(CancellationToken cancellationToken = default)
    {
        if (Period <= 0)
        {
            throw new InvalidOperationException("Period should be set before starting the timer!");
        }

        lock (_taskTimer)
        {
            _taskTimer.Change(RunOnStart ? 0 : Period, Timeout.Infinite);
            _isRunning = true;
        }
    }

    public void Stop(CancellationToken cancellation = default)
    {
        lock (_taskTimer)
        {
            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            // 等待任务委托执行完毕
            while (_performingTasks)
            {
                Monitor.Wait(_taskTimer);
            }

            _isRunning = false;
        }
    }

    private void TimerCallBack(object? state)
    {
        lock (_taskTimer)
        {
            // 防重入
            if (!_isRunning || _performingTasks)
            {
                return;
            }

            // 防重入
            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _performingTasks = true;
        }

        _ = Timer_Elapsed();
    }

    private async Task Timer_Elapsed()
    {
        try
        {
            await Elapsed(this);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{GetType().FullName} timer error");
        }
        finally
        {
            lock (_taskTimer)
            {
                _performingTasks = false;
                if (_isRunning)
                {
                    // 恢复定时器运行
                    _taskTimer.Change(Period, Timeout.Infinite);
                }

                // 通知 _taskTimer 状态发生更新
                Monitor.Pulse(_taskTimer);
            }
        }
    }
}
