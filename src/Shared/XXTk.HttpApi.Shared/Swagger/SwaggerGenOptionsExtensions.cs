using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerGenOptionsExtensions
{
    public static void IncludeXmlCommentsIfFileExist(this SwaggerGenOptions swaggerGenOptions, string filePath, bool includeControllerXmlComments = false)
    {
        if (File.Exists(filePath))
        {
            swaggerGenOptions.IncludeXmlComments(filePath, includeControllerXmlComments);
        }
    }
}