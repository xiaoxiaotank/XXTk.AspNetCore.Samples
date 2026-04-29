using Audit.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using System.Text.Json;
using XXTk.AspNetCore.Samples.AuditLog.Converters;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("starting...");

// 使用本地时间的 JSON 转换器
JsonSerializerOptions options = new()
{
    WriteIndented = true,
    Converters = { new LocalDateTimeConverter(), new LocalDateTimeOffsetConverter() }
};

try
{
    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;
    var configuration = builder.Configuration;
    var env = builder.Environment;
    var host = builder.Host;

    #region Add services to the container

    builder.AddAppNLog(true);
    builder.UseAppAutofacServiceProviderFactory([typeof(XXTk.HttpApi.Shared.Controllers.Abstractions.AppApiControllerBase)]);

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
    // 启用 NLog 作为审计日志记录器
    Audit.Core.Configuration.Setup().UseNLog(config => config
        .Message(auditEvent => JsonSerializer.Serialize(auditEvent, options)));
    services.AddScoped<IAuditScopeFactory, AuditScopeFactory>();

    #endregion

    var app = builder.Build();

    #region Configure the HTTP request pipeline

    // 启用 ExceptionHandler
    app.UseExceptionHandler();

    if (app.Environment.IsLocal() || app.Environment.IsDev() || app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

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
