using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using XXTk.Shared.Apis.ApiResponses;

namespace XXTk.HttpApi.Shared.Mvc.Handlers.ExceptionHandlers
{
    public class ApiExceptionHandler(
        ILogger<ApiExceptionHandler> logger,
        IWebHostEnvironment env) : IExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger = logger;
        private readonly IWebHostEnvironment _env = env;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            int responseStatusCode;
            object responseResult;

            if (exception is IBusinessException bizException)
            {
                responseStatusCode = StatusCodes.Status400BadRequest;
                responseResult = new ObjectResult(ApiErrorResponse.Create(bizException.Message, bizException.Code));
            }
            else
            {
                var request = httpContext.Request;
                string? requestInfo = null;
                try
                {
                    requestInfo = $"{request.Method} {request.Scheme}://{request.Host}{request.PathBase}{request.Path} {request.Protocol}";
                }
                catch (Exception) { }
                _logger.LogError(exception, $"{requestInfo}接口处理发生异常");

                responseStatusCode = StatusCodes.Status500InternalServerError;
                if (_env.IsLocal() || _env.IsDev() || _env.IsQa())
                {
                    responseResult = new ObjectResult(ApiErrorResponse.Create(exception.ToString(), httpContext.Response.StatusCode));
                }
                else
                {
                    responseResult = new ObjectResult(ApiErrorResponse.Create("服务器异常，请稍候重试", httpContext.Response.StatusCode));
                }
            }

            httpContext.Response.StatusCode = responseStatusCode;
            await httpContext.Response.WriteAsJsonAsync(responseResult, cancellationToken: cancellationToken);

            return true;
        }
    }
}
