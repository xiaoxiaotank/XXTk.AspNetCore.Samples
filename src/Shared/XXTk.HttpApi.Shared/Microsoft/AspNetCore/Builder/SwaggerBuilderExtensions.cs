namespace Microsoft.AspNetCore.Builder;

public static class SwaggerBuilderExtensions
{
    public static void MapSwaggerDefault(this IEndpointRouteBuilder app)
    {
        app.MapSwagger($"swagger/{{documentName}}/swagger.json");
    }
}
