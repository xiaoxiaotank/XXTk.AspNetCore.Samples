using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Hosting;

public static class HostEnvironmentExtensions
{
    public static bool IsLocal(this IHostEnvironment env) => env.Is("Local");

    public static bool IsDev(this IHostEnvironment env) => env.Is("Dev");

    public static bool IsQa(this IHostEnvironment env) => env.Is("Qa");

    public static bool IsStg(this IHostEnvironment env) => env.Is("Stg");

    public static bool IsProd(this IHostEnvironment env) => env.Is("Prod");


    private static bool Is(this IHostEnvironment env, string name)
    {
        // 在linux环境下，（配置）文件名严格区分大小写，因此，环境也严格区分大小写
        return env.EnvironmentName == name;
    }
}
