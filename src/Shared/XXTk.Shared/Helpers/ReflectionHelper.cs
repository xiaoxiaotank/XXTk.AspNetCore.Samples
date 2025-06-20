﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XXTk.Utils.Helpers;
public static class ReflectionHelper
{
    /// <summary>
    /// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
    /// </summary>
    /// <param name="givenType">Type to check</param>
    /// <param name="genericType">Generic type</param>
    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var givenTypeInfo = givenType.GetTypeInfo();

        if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        foreach (var interfaceType in givenTypeInfo.GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenTypeInfo.BaseType == null)
        {
            return false;
        }

        return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
    }
}
