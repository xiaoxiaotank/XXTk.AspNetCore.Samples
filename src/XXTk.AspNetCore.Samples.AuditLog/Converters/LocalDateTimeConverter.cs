using System.Text.Json;
using System.Text.Json.Serialization;

namespace XXTk.AspNetCore.Samples.AuditLog.Converters;

/// <summary>
/// 时间转换器：将 UTC 时间转为本地时间
/// </summary>
public class LocalDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTime();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(DateTime.SpecifyKind(value, DateTimeKind.Utc).ToLocalTime());
}

/// <summary>
/// 时间偏移转换器：将 UTC 时间偏移转为本地时间
/// </summary>
public class LocalDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTimeOffset();

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToLocalTime());
}
