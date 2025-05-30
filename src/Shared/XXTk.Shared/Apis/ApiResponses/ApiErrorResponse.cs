namespace XXTk.Shared.Apis.ApiResponses;

/// <summary>
/// Api错误响应
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    /// 错误消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 错误码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 错误详情
    /// </summary>
    public string? Details { get; set; }

    public static ApiErrorResponse Create(string? message = null, int code = 0, string? details = null)
    {
        return new ApiErrorResponse { Message = message, Code = code, Details = details };
    }
}
