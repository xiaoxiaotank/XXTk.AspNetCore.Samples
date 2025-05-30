namespace System;

/// <summary>
/// Provides extensions for <see cref="Type"/> class.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// 数字类型集合
    /// </summary>
    private static readonly List<Type> NumericTypes = new List<Type>
    {
        typeof(decimal),
        typeof(byte), typeof(sbyte),
        typeof(short), typeof(ushort),
        typeof(int), typeof(uint),
        typeof(long), typeof(ulong),
        typeof(float), typeof(double)
    };

    /// <summary>
    /// 检查是否为数字类型
    /// </summary>
    /// <param name="type">The type to be checked.</param>
    /// <returns><c>true</c> if it's numeric; otherwise <c>false</c>.</returns>
    public static bool IsNumeric(this Type type)
    {
        return NumericTypes.Contains(type);
    }

    /// <summary>
    /// 获取类型的全名（包含程序集名称）
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetFullNameWithAssemblyName(this Type type)
    {
        return type.FullName + ", " + type.Assembly.GetName().Name;
    }

    /// <summary>
    /// 判断 <paramref name="type"/> 是否能够分配到 <typeparamref name="TTarget"/>
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsAssignableTo<TTarget>(this Type type)
    {
        return type.IsAssignableTo(typeof(TTarget));
    }

    /// <summary>
    /// 获取 <paramref name="type"/> 的所有基类类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="includeObject"></param>
    /// <returns></returns>
    public static Type[] GetBaseClasses(this Type type, bool includeObject = true)
    {
        Check.NotNull(type, nameof(type));

        var types = new List<Type>();
        AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject);
        return types.ToArray();
    }

    /// <summary>
    /// 获取 <paramref name="type"/> 的所有基类类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="stoppingType">当查询到该类型时，不再深入获取该类型的基类，该类型会包含在返回结果中</param>
    /// <param name="includeObject"></param>
    /// <returns></returns>
    public static Type[] GetBaseClasses(this Type type, Type stoppingType, bool includeObject = true)
    {
        Check.NotNull(type, nameof(type));

        var types = new List<Type>();
        AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
        return [.. types];
    }

    /// <summary>
    /// 判断泛型类型 <paramref name="type"/> 是否能够创建 <paramref name="targetType"/> 的实例
    /// </summary>
    /// <param name="type"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public static bool CanMakeGenericTo(this Type type, Type targetType)
    {
        Check.NotNull(type, nameof(type));
        Check.NotNull(targetType, nameof(targetType));
        if (!type.IsGenericTypeDefinition)
        {
            throw new InvalidOperationException("The current type does not represent a generic type definition");
        }
        return targetType.GetInterfaces()
                .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == type);
    }

    private static void AddTypeAndBaseTypesRecursively(
        List<Type> types,
        Type type,
        bool includeObject,
        Type stoppingType = null)
    {
        if (type == null || type == stoppingType)
        {
            return;
        }

        if (!includeObject && type == typeof(object))
        {
            return;
        }

        AddTypeAndBaseTypesRecursively(types, type.BaseType, includeObject, stoppingType);
        types.Add(type);
    }
}
