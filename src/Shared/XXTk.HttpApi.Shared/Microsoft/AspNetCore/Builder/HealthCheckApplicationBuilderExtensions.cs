namespace Microsoft.AspNetCore.Builder;

public static class HealthCheckApplicationBuilderExtensions
{
    public static void UseHealthChecksDefault(this IApplicationBuilder app)
    {
        app.UseHealthChecks(new PathString($"/api/health"));
    }
}
