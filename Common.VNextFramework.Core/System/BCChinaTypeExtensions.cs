using System;
using System.Collections.Generic;
using System.Text;
using Common.VNextFramework;

namespace System
{
    public static class BCChinaTypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }
        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            Check.NotNull(type, nameof(type));

            return type.IsAssignableTo(typeof(TTarget));
        }

        public static bool IsAssignableTo(this Type type, Type targetType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(targetType, nameof(targetType));

            return targetType.IsAssignableFrom(type);
        }
    }
}
