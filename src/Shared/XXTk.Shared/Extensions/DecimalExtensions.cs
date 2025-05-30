namespace System;

public static class DecimalExtensions
{
    /// <summary>
    /// 截断小数位数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static decimal Truncate(this decimal value, int decimals)
    {
        if (decimals < 0 || decimals > 28)
        {
            throw new ArgumentOutOfRangeException(nameof(decimals));
        }

        if (value == 0)
        {
            return value;
        }

        if (decimals == 0)
        {
            return decimal.Truncate(value);
        }

        var p = 5 / (decimal)Math.Pow(10, decimals + 1);

        var result = Math.Round(value + (value > 0 ? -p : p), decimals, MidpointRounding.AwayFromZero);

        return result;
    }

    /// <summary>
    /// 截断小数位数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static decimal? Truncate(this decimal? value, int decimals)
    {
        if (!value.HasValue)
        {
            return value;
        }

        return Truncate(value.Value, decimals);
    }
}
