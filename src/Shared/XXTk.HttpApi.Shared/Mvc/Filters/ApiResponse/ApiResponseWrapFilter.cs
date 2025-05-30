using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Reflection;
using XXTk.Shared.Apis.ApiResponses;

namespace XXTk.HttpApi.Shared.Mvc.Filters.ApiResponse;

public class ApiResponseWrapFilter : IAsyncResultFilter
{
    private readonly ApiResponseWrapOptions _options;

    public ApiResponseWrapFilter(IOptions<ApiResponseWrapOptions> options)
    {
        _options = options.Value;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var hasNotWrapAttribute = (context.ActionDescriptor as ControllerActionDescriptor)!
            .MethodInfo.GetCustomAttributes<NotWrapApiResponseAttribute>().FirstOrDefault() != null;

        if (!hasNotWrapAttribute)
        {
            if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(null);
            }

            if (context.Result is ObjectResult objectResult && objectResult.Value is not null)
            {
                var wrapper = WrapApiResponse(objectResult.Value!, context.HttpContext);
                objectResult.Value = wrapper.Value;
                objectResult.DeclaredType = wrapper.Value.GetType();

                if (wrapper.RewriteHttpStatusCode)
                {
                    objectResult.StatusCode = wrapper.HttpStatusCode;
                }
            }
        }

        await next();
    }

    private ApiResponseWrapper WrapApiResponse(object currentValue, HttpContext httpContext)
    {
        if (currentValue is ProblemDetails problemDetails)
        {
            if (!_options.WrapProblemDetails)
                return new ApiResponseWrapper(currentValue);

            var errorMsg = string.Empty;
            if (currentValue is ValidationProblemDetails validationProblemDetails)
            {
                errorMsg = string.Join(",", validationProblemDetails.Errors.SelectMany(s => s.Value));
            }

            errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? problemDetails.Title : errorMsg;

            var rewriteHttpStatusCode = _options.ProblemDetailsResponseStatusCode.HasValue;
            var stautsCode = _options.ProblemDetailsResponseStatusCode ?? problemDetails.Status ?? httpContext.Response.StatusCode;

            return new ApiResponseWrapper(ApiErrorResponse.Create(errorMsg, stautsCode, problemDetails.Detail), rewriteHttpStatusCode, stautsCode);
        }

        return new ApiResponseWrapper(currentValue);
    }

    public struct ApiResponseWrapper
    {
        public object Value { get; set; }

        public bool RewriteHttpStatusCode { get; set; }

        public int? HttpStatusCode { get; set; }

        public ApiResponseWrapper(object value, bool rewriteHttpStatusCode = false, int? httpStatusCode = null)
        {
            Value = value;
            RewriteHttpStatusCode = rewriteHttpStatusCode;
            HttpStatusCode = httpStatusCode;
        }
    }
}
