namespace System;

public static class EnumExtensions
{
    public static bool IsDefined<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Any(e => e.Equals(enumValue));
    }
}
