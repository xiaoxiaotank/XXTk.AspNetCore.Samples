namespace System.Collections.Generic;

public static class AbpEnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        if (source is ICollection<T> collection)
        {
            return collection.IsNullOrEmpty();
        }

        return source == null || !source.Any();
    }

    /// <summary>
    /// 通过指定连接字符串连接为字符串
    /// </summary>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string JoinAsString(this IEnumerable<string> source, string separator)
    {
        return string.Join(separator, source);
    }

    /// <summary>
    /// 通过指定连接字符串连接为字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
    {
        return string.Join(separator, source);
    }

    /// <summary>
    /// 当 condition == true 时，通过 predicate 对 source 进行过滤，否则，直接返回 source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }

    /// <summary>
    /// 当 condition == true 时，通过 predicate 对 source 进行过滤，否则，直接返回 source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }

    /// <summary>
    /// 分割集合数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable">集合</param>
    /// <param name="chunkSize">每次分割数量</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> ToChunks<T>(this IEnumerable<T> enumerable, int chunkSize)
    {
        int returnedCount = 0;
        var list = enumerable?.ToList();
        int count = list == null ? 0 : list.Count;
        while (returnedCount < count)
        {
            int currentChunkSize = Math.Min(chunkSize, count - returnedCount);
            yield return list.GetRange(returnedCount, currentChunkSize);
            returnedCount += currentChunkSize;
        }
    }
}
