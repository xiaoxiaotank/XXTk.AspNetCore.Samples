namespace System.Collections.Generic;

public static class DictionaryExtensions
{
    /// <summary>
    /// 获取与指定键关联的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
    {
        if (dictionary.TryGetValue(key, out object valueObj) && valueObj is T)
        {
            value = (T)valueObj;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// 获取与指定键关联的值，若 key 不存在，则返回默认值
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.TryGetValue(key, out TValue obj) ? obj : default;
    }

    /// <summary>
    /// 获取与指定键关联的值，若 key 不存在，则将 factory 返回值作为 value 添加到 dictionary 中，并返回该返回值
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
    {
        TValue obj;
        if (dictionary.TryGetValue(key, out obj))
        {
            return obj;
        }

        return dictionary[key] = factory(key);
    }

    /// <summary>
    /// 获取与指定键关联的值，若 key 不存在，则将 factory 返回值作为 value 添加到 dictionary 中，并返回该返回值
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
    {
        return dictionary.GetOrAdd(key, k => factory());
    }
}
