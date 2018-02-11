using System.ComponentModel;
using System.Linq;

namespace dbEnumerator.Test.Infrastructure
{
    public static class TestUtils
    {
        public static string GetEnumDescription<TEnum>(TEnum item)
        {
            var attribute = item.GetType().GetField(item.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>()
                .FirstOrDefault();

            return attribute?.Description ?? string.Empty;
        }
    }
}