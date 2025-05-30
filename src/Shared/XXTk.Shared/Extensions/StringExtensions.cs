namespace System;

public static class StringExtensions
{
    #region Join

    public static string Join(this IEnumerable<string> values, string separator)
    {
        return string.Join(separator, values);
    }

    public static string Join(this string[] values, string separator, int startIndex, int count)
    {
        return string.Join(separator, values, startIndex, count);
    }

    #endregion

    #region IsNullOrEmpty & IsNullOrWhiteSpace

    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static bool IsNullOrEmpty(this IEnumerable<string> values)
    {
        return values?.Any(v => !v.IsNullOrEmpty()) != true;
    }

    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNullOrWhiteSpace(this IEnumerable<string> values)
    {
        return values?.Any(v => !v.IsNullOrWhiteSpace()) != true;
    }

    #endregion

    public static string PutSeparator(this string value, int split, string separator)
    {
        var parts = new List<string>((int)Math.Ceiling((decimal)value.Length / split));

        for (var i = 0; i < value.Length; i += split)
        {
            parts.Add(value.Substring(i, Math.Min(value.Length - i, split)));
        }

        return string.Join(separator, parts);
    }
}
