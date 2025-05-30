namespace XXTk.HttpApi.Shared.Mvc.Filters.ApiResponse;

public class ApiResponseWrapOptions
{
    /// <summary>
    /// 是否包装ProblemDetails
    /// <para>
    ///     当存在模型验证错误时，将会返回格式为ProblemDetails的结果
    /// </para>
    /// <para>
    ///     Default :true.
    /// </para>
    /// </summary>
    public bool WrapProblemDetails { get; set; } = true;

    /// <summary>
    /// 当 WrapProblemDetails = true 且 ProblemDetails 被包装时，重写返回的 http status code。 
    /// 默认值为:200。
    /// <para>
    ///   当该参数为空时，将使用原有的status code。
    /// </para>
    /// </summary>
    public int? ProblemDetailsResponseStatusCode { get; set; } = StatusCodes.Status200OK;
}
