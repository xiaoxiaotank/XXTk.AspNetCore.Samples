using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace XXTk.HttpApi.Shared.Swagger.Filters;

public class CustomSummaryOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var summary = operation.Summary;
        if (summary.IsNullOrWhiteSpace())
        {
            operation.Summary = operation.OperationId;
        }
        else
        {
            operation.Summary = $"{operation.OperationId} ({summary})";
        }
    }
}