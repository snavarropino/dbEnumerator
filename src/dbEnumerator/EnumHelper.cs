using System;
using System.ComponentModel;
using System.Linq;

namespace dbEnumerator
{
    public static class EnumHelper
    {
        public static Type EnsureEnum<TEnum>()
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
                throw new ArgumentException($"Type '{type.AssemblyQualifiedName}' must be an enum");

            var underlyingType = Enum.GetUnderlyingType(type);

            if (underlyingType != typeof(int))
                throw new ArgumentException("Enum underlying type must be int");

            return type;
        }

        public static string GetEnumDescription<TEnum>(TEnum item)
        {
            var attribute = item.GetType().GetField(item.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return attribute?.Description ?? string.Empty;
        }

    }
}
