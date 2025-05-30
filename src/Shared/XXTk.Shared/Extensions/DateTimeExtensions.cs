namespace System;

public static class DateTimeExtensions
{
    /// <summary>
    /// yyyy-MM-dd HH:mm:ss
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToStandardString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");

    /// <summary>
    /// yyyy-MM-dd
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToStandardDateString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

    /// <summary>
    /// 获取Unix时间戳（秒）
    /// </summary>
    /// <returns></returns>
    public static long ToUnixTimeSeconds(this DateTime dateTime) => new DateTimeOffset(dateTime).ToUnixTimeSeconds();

    /// <summary>
    /// 获取Unix时间戳（毫秒）
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToUnixTimeMilliseconds(this DateTime dateTime) => new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
}
