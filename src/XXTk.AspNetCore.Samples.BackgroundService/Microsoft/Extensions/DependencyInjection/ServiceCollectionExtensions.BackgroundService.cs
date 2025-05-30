using Quartz.AspNetCore;
using Quartz;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Managers;
using SilkierQuartz;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers.Quartz;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppBackgroundService(
        this IServiceCollection services,
        Action<IServiceCollectionQuartzConfigurator>? configureQuartz = null,
        Action<QuartzHostedServiceOptions>? configureQuartzHostedService = null,
        Action<AppBackgroundWorkerManagerOptions>? configureBackgroundWorkerManager = null)
    {
        var backgroundWorkerManagerOptions = new AppBackgroundWorkerManagerOptions();

        configureBackgroundWorkerManager?.Invoke(backgroundWorkerManagerOptions);

        services.AddSingleton(Microsoft.Extensions.Options.Options.Create(backgroundWorkerManagerOptions));

        // quartz ui
        services.AddSilkierQuartz(options =>
        {
            options.VirtualPathRoot = "/quartz";
            options.UseLocalTime = true;
            options.DefaultDateFormat = "yyyy-MM-dd";
            options.DefaultTimeFormat = "HH:mm:ss";
        }, configureAuthenticationOptions: authenticationOptions =>
        {
            authenticationOptions.AccessRequirement = SilkierQuartzAuthenticationOptions.SimpleAccessRequirement.AllowAnonymous;
        });

        // quartz
        services.AddQuartz(config =>
        {
            // 默认的，使用简单类型加载器来构造Job实例，支持DI
            config.UseSimpleTypeLoader();
            // 使用内存存储
            config.UseInMemoryStore();
            config.UseDedicatedThreadPool(options =>
            {
                // Job最大并发数
                options.MaxConcurrency = 10;
            });

            configureQuartz?.Invoke(config);
        });
        // 管理默认 scheduler 的启动和停止
        services.AddQuartzServer(options =>
        {
            // 停止应用时，等待正在运行的Job完成后再停止
            options.WaitForJobsToComplete = true;

            configureQuartzHostedService?.Invoke(options);
        });
        // 注册默认的调度器
        services.AddSingleton(sp =>
        {
            return sp.GetRequiredService<ISchedulerFactory>().GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();
        });
        services.AddSingleton(typeof(QuartzBackgroundWorkerAdapter<>));
        services.AddHostedService<BackgroundServiceHostedService>();

        return services;
    }

    public class BackgroundServiceHostedService : IHostedService
    {
        private readonly IBackgroundWorkerManager _backgroundWorkerManager;

        public BackgroundServiceHostedService(IBackgroundWorkerManager backgroundWorkerManager)
        {
            _backgroundWorkerManager = backgroundWorkerManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _backgroundWorkerManager.InitAsync(cancellationToken);
            await _backgroundWorkerManager.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}