using System.Text;
using System.Text.Json;

namespace System.Net.Http;

/// <summary>
/// HttpClient扩展类
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// 发送Get请求
    /// </summary>
    /// <param name="client"></param>
    /// <param name="requestUri"></param>
    /// <returns></returns>
    public static HttpResponseMessage Get(this HttpClient client, string requestUri)
    {
        return client.GetAsync(requestUri)
            .ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 获取二进制数组
    /// </summary>
    /// <param name="client"></param>
    /// <param name="requestUri"></param>
    /// <returns></returns>
    public static byte[] GetByteArray(this HttpClient client, string requestUri)
    {
        return client.GetByteArrayAsync(requestUri)
            .ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="client"></param>
    /// <param name="requestUri"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static HttpResponseMessage Post(this HttpClient client, string requestUri, HttpContent content)
    {
        return client.PostAsync(requestUri, content)
            .ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 发送Json格式的Post请求
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="client"></param>
    /// <param name="requestUri"></param>
    /// <param name="data"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static HttpResponseMessage PostAsJson<TData>(this HttpClient client, string requestUri, TData data,
        JsonSerializerOptions options = null)
    {
        var requestData = JsonSerializer.Serialize(data, options);
        var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");
        return client.PostAsync(requestUri, requestContent)
            .ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 读取为字符串
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string ReadAsString(this HttpContent content)
    {
        return content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 读取为指定类型的对象
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <param name="content"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static TData ReadAs<TData>(this HttpContent content, JsonSerializerOptions options = null)
    {
        var responseStr = content.ReadAsString();
        return JsonSerializer.Deserialize<TData>(responseStr, options);
    }
}
