using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dbEnumerator
{
    public class EnumSeeder
    {
        public static async Task SeedEnumDataAsync<TData, TEnum>(DbSet<TData> items)
            where TData : EnumBase<TEnum>
            where TEnum : struct
        {
            var enumType = EnumHelper.EnsureEnum<TEnum>();

            foreach (TEnum evalue in Enum.GetValues(enumType))
            {
                var id = (int)Convert.ChangeType(evalue, typeof(int));

                if (id <= 0)
                    throw new Exception("Enum underlying value must start with 1");

                if (!items.Any(a => a.Id == id))
                {
                    var item = Activator.CreateInstance<TData>();
                    item.Id = id;
                    item.Name = Enum.GetName(enumType, evalue);
                    item.Description = EnumHelper.GetEnumDescription(evalue);
                    await items.AddAsync(item);
                }
            }
        }

    }
}