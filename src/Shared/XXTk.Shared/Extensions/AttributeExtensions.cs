using System.ComponentModel;

namespace System;

/// <summary>
/// Attribute扩展方法
/// 扩展对象：Type类型
/// </summary>
public static class AttributeExtensions
{
    /// <summary>
    /// 获取Attribute值
    /// </summary>
    /// <returns>Attribute值</returns>
    public static TValue GetAttributeValue<TAttribute, TValue>(
        this Type type,
        Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
    {
        if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
        {
            return valueSelector(att);
        }

        return default;
    }

    /// <summary>
    /// 获取Description标签的Value
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="property">Property</param>
    /// <returns>Description</returns>
    public static string GetDescriptionByName<T>(this T property)
    {
        var fi = property?.GetType().GetField(property?.ToString());

        if (fi is null)
        {
            return property?.ToString();
        }

        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
        {
            return attributes[0].Description;
        }
        else
        {
            return property?.ToString();
        }
    }
}
