using Autofac;
using Autofac.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundWorkers.Abstractions.Workers;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("starting...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;
    var configuration = builder.Configuration;
    var env = builder.Environment;
    var host = builder.Host;

    #region Add services to the container

    builder.AddAppNLog(true);
    builder.UseAppAutofacServiceProviderFactory([typeof(BackgroundWorkerBase)]);

    #region Options

    #endregion

    services.AddAppDefault(configuration).AddAppIdentity();

    services.AddControllers();

    services.AddEndpointsApiExplorer();

    services.AddApiVersioning().AddMvc().AddApiExplorer();

    services.AddSwaggerGen();

    services.AddHttpClient();

    services.AddAutoInject();

    services.AddHealthChecks();

    services.AddAppBackgroundService(
        configureBackgroundWorkerManager: options =>
        {
            options.AddBackgroundWoker<MyQuartzWorker>();
            options.AddBackgroundWoker<MyMemoryWorker>();
        });

    #endregion

    var app = builder.Build();

    #region Configure the HTTP request pipeline

    // ∆Ù”√ ExceptionHandler
    app.UseExceptionHandler();

    if (app.Environment.IsLocal() || app.Environment.IsDev())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAppBackgroundServiceUI();

    app.MapControllers();
    app.MapSwaggerDefault();

    app.UseHealthChecksDefault();

    app.Run();
    #endregion
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}