namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices;

/// <summary>
/// 后台服务优先级
/// </summary>
public enum BackgroundServicePriority
{
    Low = 5,
    BelowNormal = 10,
    Normal = 15,
    AboveNormal = 20,
    High = 25
}
