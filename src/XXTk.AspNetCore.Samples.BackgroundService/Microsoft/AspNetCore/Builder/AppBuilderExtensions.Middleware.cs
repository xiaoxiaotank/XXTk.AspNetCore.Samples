using SilkierQuartz;

namespace Microsoft.AspNetCore.Builder;

public static partial class AppBuilderExtensions
{
    /// <summary>
    /// 使用后台任务看板（仅支持Quartz）
    /// 注意：
    /// 1. 必须提前 UseRouting
    /// 2. 必须提前 UseAuthentication 和 UseAuthorization
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAppBackgroundServiceUI(this IApplicationBuilder app, Action<Services>? configure = null) => app.UseSilkierQuartz(configure);
}
